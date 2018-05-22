using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security;
using Autofac;
using PostSharp.Aspects;

namespace AuthorizationAspect.Api
{
    [Flags]
    public enum AuthorizationMode
    {
        None = 0b0000,
        AccountEntityCommand = 0b0001,
        AccountRequest = 0b0010,
        ReturnValue = 0b0110,
        All = 0b0111
    }

    [Serializable]
    public class AuthorizationAspect : OnMethodBoundaryAspect
    {
        private readonly AuthorizationMode _mode;
        private IAccountRequestAuthorizer _accountAuthorizer;
        private readonly Dictionary<int, IAuthorizer> _authorizersCache = new Dictionary<int, IAuthorizer>();
        private readonly List<int> _authoriziedAccountRequestParams = new List<int>();
        private int _authorizedAccount;
        private bool _returnValueHasAccountNumber;
        private bool _returnValueHasAccountNumberEnumerable;

        public AuthorizationAspect(AuthorizationMode mode = AuthorizationMode.All)
        {
            _mode = mode;
        }

        public override void RuntimeInitialize(System.Reflection.MethodBase method)
        {
            if (_mode.HasFlag(AuthorizationMode.AccountRequest) || _mode.HasFlag(AuthorizationMode.AccountEntityCommand))
            {
                _accountAuthorizer = IocContainer.ResolveAccountRequestAuthorizer();

                var methodParams = method.GetParameters();
                for (int i = 0; i < methodParams.Length; i++)
                {
                    var param = methodParams[i];
                    if (param.ParameterType.IsAssignableTo<IAuthoriziedAccountRequest>())
                        _authoriziedAccountRequestParams.Add(i);

                    if (param.ParameterType.IsAssignableTo<IAuthoriziedEntityCommand>())
                    {
                        var authorizer = IocContainer.ResolveEntityCommandAuthorizer(param.ParameterType);
                        if (authorizer != null) _authorizersCache.Add(i, authorizer);
                    }
                }
            }

            if (_mode.HasFlag(AuthorizationMode.ReturnValue) && method is MethodInfo info)
            {
                if (info.ReturnType.IsAssignableTo<IHasAccountNumber>())
                    _returnValueHasAccountNumber = true;
                else if (info.ReturnType.IsAssignableTo<IEnumerable<IHasAccountNumber>>())
                    _returnValueHasAccountNumberEnumerable = true;
            }

            base.RuntimeInitialize(method);
        }

        //public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
        //{
           
        //}

        public override void OnEntry(MethodExecutionArgs args)
        {
            if (_mode.HasFlag(AuthorizationMode.AccountRequest))
                foreach (var param in _authoriziedAccountRequestParams)
                {
                    var request = args.Arguments[param] as IAuthoriziedAccountRequest;
                    if (!_accountAuthorizer.Authorize(request))
                        throw new SecurityException();
                    _authorizedAccount = request.AccountNumber;
                }

            if (_mode.HasFlag(AuthorizationMode.AccountEntityCommand))
                foreach (var authorizerKv in _authorizersCache)
                {
                    if (!authorizerKv.Value.Authorize(args.Arguments[authorizerKv.Key]))
                        throw new SecurityException();
                }
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            void AuthorizeReturnValueAccount(IHasAccountNumber returnValue)
            {
                if (returnValue.AccountNumber != _authorizedAccount)
                    throw new SecurityException();
            }

            if (_mode.HasFlag(AuthorizationMode.ReturnValue))
            {
                if (_returnValueHasAccountNumber)
                    AuthorizeReturnValueAccount((IHasAccountNumber)args.ReturnValue);

                if (_returnValueHasAccountNumberEnumerable)
                    foreach (var returnValue in (IEnumerable<IHasAccountNumber>)args.ReturnValue)
                        AuthorizeReturnValueAccount(returnValue);
            }

            base.OnSuccess(args);
        }
    }
}
