using PostSharp.Aspects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AuthoritzationAspectExample
{

    [Serializable]
    class AuthorizationAspect : OnMethodBoundaryAspect
    {
        private IAccountRequestAuthorizer _accountAuthorizer = new AccountAuthorizer();
        private AccountingEntityAuthorizer _accountingEntityAuthorizer = new AccountingEntityAuthorizer();

        public override void RuntimeInitialize(System.Reflection.MethodBase method)
        {
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            foreach (var arg in args.Arguments)
            {
                if (arg is IAuthoriziedAccountRequest)
                    if (!_accountAuthorizer.Authorize(arg as IAuthoriziedAccountRequest))
                        throw new SecurityException();

                if (arg is BaseAccountingRecordCommand)
                    if (!_accountingEntityAuthorizer.Authorize(arg as BaseAccountingRecordCommand))
                        throw new SecurityException();
            }
        }
    }
}
