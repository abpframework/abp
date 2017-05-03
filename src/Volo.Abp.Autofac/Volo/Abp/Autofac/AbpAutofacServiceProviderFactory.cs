using System;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Castle.DynamicProxy;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Autofac
{
    /// <summary>
    /// A factory for creating a <see cref="T:Autofac.ContainerBuilder" /> and an <see cref="T:System.IServiceProvider" />.
    /// </summary>
    public class AbpAutofacServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        private readonly ContainerBuilder _builder;
        private IServiceCollection _services;

        public AbpAutofacServiceProviderFactory(ContainerBuilder builder)
        {
            _builder = builder;
        }

        /// <summary>
        /// Creates a container builder from an <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.
        /// </summary>
        /// <param name="services">The collection of services</param>
        /// <returns>A container builder that can be used to create an <see cref="T:System.IServiceProvider" />.</returns>
        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            _services = services;

            _builder.Populate(services);

            return _builder;
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            Check.NotNull(containerBuilder, nameof(containerBuilder));

            HandleRegistrationActions(containerBuilder);

            return new AutofacServiceProvider(containerBuilder.Build());
        }

        private static readonly ProxyGenerator _proxyGenerator = new ProxyGenerator();

        private void HandleRegistrationActions(ContainerBuilder containerBuilder)
        {
            var registrationActions = _services.GetServiceRegistrationActionList();
            containerBuilder.RegisterCallback(registry =>
            {
                foreach (var registration in registry.Registrations)
                {
                    var registredArgs = new OnServiceRegistredArgs(registration.Activator.LimitType);

                    foreach (var registrationAction in registrationActions)
                    {
                        registrationAction(registredArgs);
                    }

                    registration.Preparing += (sender, args) =>
                    {
                        
                    };

                    registration.Activating += (sender, args) =>
                    {
                        if (args.Component.Services.OfType<IServiceWithType>().Any(swt => !swt.ServiceType.GetTypeInfo().IsVisible) || args.Instance.GetType().Namespace.StartsWith("Castle.Proxies"))
                        {
                            return;
                        }

                        if (registredArgs.Interceptors.Any())
                        {
                            ApplyInterceptors(args, registredArgs);
                        }
                    };
                }
            });
        }

        private static void ApplyInterceptors(ActivatingEventArgs<object> args, OnServiceRegistredArgs registredArgs)
        {
            var mainService = args.Component.Services.OfType<IServiceWithType>().FirstOrDefault()?.ServiceType;
            if (mainService == null)
            {
                return;
            }

            var interceptorInstances = registredArgs.Interceptors
                .Select(i => (IInterceptor)new CastleAbpInterceptorAdapter((IAbpInterceptor)args.Context.Resolve(i)))
                .ToArray();

            if (mainService.GetTypeInfo().IsInterface)
            {
                args.ReplaceInstance(
                    _proxyGenerator
                        .CreateInterfaceProxyWithTargetInterface(
                            mainService,
                            args.Instance,
                            interceptorInstances
                        )
                );
            }
            else
            {
                //var proxied = _proxyGenerator
                //    .CreateClassProxyWithTarget(
                //        mainService,
                //        args.Instance,
                //        interceptorInstances
                //    );

                //args.ReplaceInstance(proxied);

                //args.ReplaceInstance(
                //    _proxyGenerator
                //        .CreateClassProxyWithTarget(
                //            mainService,
                //            args.Instance,
                //            interceptorInstances
                //        )
                //);
            }
        }
    }
}