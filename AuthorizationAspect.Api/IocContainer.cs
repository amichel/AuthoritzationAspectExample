using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;

namespace AuthorizationAspect.Api
{
    public abstract class IocContainer
    {
        protected IContainer Container;

        public static IAccountRequestAuthorizer AccountRequestAuthorizer { get; protected set; }
        public static IReadOnlyDictionary<Type, IAuthorizer> EntityCommandAuthorizers { get; protected set; }

        protected IocContainer()
        {
        }

        private IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            var assemblies = GetAssemblies();

            builder.RegisterAssemblyTypes(assemblies)
                .AssignableTo<IAccountRequestAuthorizer>()
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(assemblies)
                .AsClosedTypesOf(typeof(EntityCommandAuthorizer<>))
                .SingleInstance();

            builder.RegisterAssemblyTypes(assemblies)
                .AssignableTo<IAuthoriziedEntityCommand>()
                .AsImplementedInterfaces();

            return builder.Build();
        }

        public void Build()
        {
            Container = BuildContainer();
            ResolveAndCacheAuthorizers();
        }

        private void ResolveAndCacheAuthorizers()
        {
            AccountRequestAuthorizer = ResolveAccountRequestAuthorizer();

            var authorizers = new Dictionary<Type, IAuthorizer>();
            var commands = Container.Resolve<IEnumerable<IAuthoriziedEntityCommand>>();
            foreach (var command in commands)
            {
                if (TryResolveEntityCommandAuthorizer(command.GetType(), out object authorizer))
                    authorizers.Add(command.GetType(), authorizer as IAuthorizer);
            }

            EntityCommandAuthorizers = new ReadOnlyDictionary<Type, IAuthorizer>(authorizers);
        }

        public virtual IAccountRequestAuthorizer ResolveAccountRequestAuthorizer()
        {
            return Container.Resolve<IAccountRequestAuthorizer>();
        }

        public virtual bool TryResolveEntityCommandAuthorizer(Type type, out object authorizer)
        {
            //Type authorizerType = null;
            //Type currentType = type;
            //do
            //{
            //authorizerType = typeof(EntityCommandAuthorizer<>).MakeGenericType(type); //Type.GetType($"EntityCommandAuthorizer<{type.Name}>");
            //} while (currentType.IsNestedPublic);

            return
                Container.TryResolve(typeof(EntityCommandAuthorizer<>).MakeGenericType(type), out authorizer); //authorizerType == null ? null : Container.Resolve(authorizerType) as IAuthorizer;
        }
        //private IEnumerable GetHandlers(Type eventType) =>
        //    (IEnumerable)Container.Resolve(
        //        typeof(IEnumerable<>).MakeGenericType(
        //            typeof(EntityCommandAuthorizer<>).MakeGenericType(eventType)));

        protected abstract Assembly[] GetAssemblies();
    }
}
