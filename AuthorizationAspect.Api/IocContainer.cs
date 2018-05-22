using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;

namespace AuthorizationAspect.Api
{
    public static class IocContainer
    {
        private static readonly Lazy<IContainer> Container = new Lazy<IContainer>(BuildContainer);

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            var assemblies = GetAllAssemblies();

            builder.RegisterAssemblyTypes(assemblies)
                .AssignableTo<IAccountRequestAuthorizer>()
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(IEntityCommandAuthorizer<>))
                .SingleInstance();

            return builder.Build();
        }

        public static IAccountRequestAuthorizer ResolveAccountRequestAuthorizer()
        {
            return Container.Value.Resolve<IAccountRequestAuthorizer>();
        }

        public static IEntityCommandAuthorizer<TCommand> ResolveEntityCommandAuthorizer<TCommand>()
            where TCommand : IAuthoriziedEntityCommand
        {
            return Container.Value.Resolve<IEntityCommandAuthorizer<TCommand>>();
        }

        internal static IAuthorizer ResolveEntityCommandAuthorizer(Type type)
        {
            Type authorizerType = null;
            Type currentType = type;
            do
            {
                authorizerType = Type.GetType($"IEntityCommandAuthorizer<{type.Name}>");
            } while (authorizerType == null && currentType.IsNestedPublic);

            return authorizerType == null ? null : Container.Value.Resolve(authorizerType) as IAuthorizer;
        }

        private static Assembly[] GetAllAssemblies()
        {
            //return new[] { Assembly.GetExecutingAssembly(), Assembly.GetCallingAssembly() };
            var files = GetFiles();
            return files.Select(Assembly.ReflectionOnlyLoadFrom).ToArray();
        }

        private static string[] GetFiles()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;

            var files = Directory.GetFiles(path, "AuthorizationAspect*.dll"); //TODO: support assembly scan filter via config
            if (files.Length == 0 && Directory.Exists(path + "bin"))
            {
                files = Directory.GetFiles(path + "bin", "AuthorizationAspect*.dll");
            }

            return files;
        }

    }
}
