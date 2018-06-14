using System;
using System.Reflection;
using AuthorizationAspect.Api;

namespace AuthorizationAspect.Tests
{
    [Serializable]
    public class ExecutingAssemblyIocContainer : IocContainer
    {
        protected override Assembly[] GetAssemblies()
        {
            return new[] { Assembly.GetExecutingAssembly() };
        }
    }
}