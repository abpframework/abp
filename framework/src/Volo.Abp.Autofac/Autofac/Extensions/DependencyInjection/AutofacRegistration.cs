// This software is part of the Autofac IoC container
// Copyright © 2015 Autofac Contributors
// https://autofac.org
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Core.Activators;
using Autofac.Core.Activators.Delegate;
using Autofac.Core.Activators.Reflection;
using Autofac.Core.Resolving.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace Autofac.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for registering ASP.NET Core dependencies with Autofac.
/// </summary>
public static class AutofacRegistration
{
    /// <summary>
    /// Populates the Autofac container builder with the set of registered service descriptors
    /// and makes <see cref="IServiceProvider"/> and <see cref="IServiceScopeFactory"/>
    /// available in the container.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="ContainerBuilder"/> into which the registrations should be made.
    /// </param>
    /// <param name="services">
    /// A container builder that can be used to create an <see cref="IServiceProvider" />.
    /// </param>
    public static void Populate(
        this ContainerBuilder builder,
        IServiceCollection services)
    {
        Populate(builder, services, null);
    }

    /// <summary>
    /// Populates the Autofac container builder with the set of registered service descriptors
    /// and makes <see cref="IServiceProvider"/> and <see cref="IServiceScopeFactory"/>
    /// available in the container. Using this overload is incompatible with the ASP.NET Core
    /// support for <see cref="IServiceProviderFactory{TContainerBuilder}"/>.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="ContainerBuilder"/> into which the registrations should be made.
    /// </param>
    /// <param name="services">
    /// A container builder that can be used to create an <see cref="IServiceProvider" />.
    /// </param>
    /// <param name="lifetimeScopeTagForSingletons">
    /// If provided and not <see langword="null"/> then all registrations with lifetime <see cref="ServiceLifetime.Singleton" /> are registered
    /// using <see cref="IRegistrationBuilder{TLimit,TActivatorData,TRegistrationStyle}.InstancePerMatchingLifetimeScope" />
    /// with provided <paramref name="lifetimeScopeTagForSingletons"/>
    /// instead of using <see cref="IRegistrationBuilder{TLimit,TActivatorData,TRegistrationStyle}.SingleInstance"/>.
    /// </param>
    /// <remarks>
    /// <para>
    /// Specifying a <paramref name="lifetimeScopeTagForSingletons"/> addresses a specific case where you have
    /// an application that uses Autofac but where you need to isolate a set of services in a child scope. For example,
    /// if you have a large application that self-hosts ASP.NET Core items, you may want to isolate the ASP.NET
    /// Core registrations in a child lifetime scope so they don't show up for the rest of the application.
    /// This overload allows that. Note it is the developer's responsibility to execute this and create an
    /// <see cref="AutofacServiceProvider"/> using the child lifetime scope.
    /// </para>
    /// </remarks>
    public static void Populate(
        this ContainerBuilder builder,
        IServiceCollection services,
        object? lifetimeScopeTagForSingletons)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        builder.RegisterType<AutofacServiceProvider>()
            .As<IServiceProvider>()
            .As<IServiceProviderIsService>()
            .As<IKeyedServiceProvider>()
            .As<IServiceProviderIsKeyedService>()
            .ExternallyOwned();

        var autofacServiceScopeFactory = typeof(AutofacServiceProvider).Assembly.GetType("Autofac.Extensions.DependencyInjection.AutofacServiceScopeFactory");
        if (autofacServiceScopeFactory == null)
        {
            throw new AbpException("Unable get type of Autofac.Extensions.DependencyInjection.AutofacServiceScopeFactory!");
        }

        // Issue #83: IServiceScopeFactory must be a singleton and scopes must be flat, not hierarchical.
        builder
            .RegisterType(autofacServiceScopeFactory)
            .As<IServiceScopeFactory>()
            .SingleInstance();

        // Shims for keyed service compatibility.
        builder.RegisterSource<AnyKeyRegistrationSource>();
        builder.ComponentRegistryBuilder.Registered += AddFromKeyedServiceParameterMiddleware;

        Register(builder, services, lifetimeScopeTagForSingletons);
    }

    /// <summary>
    /// Inspect each component registration, and determine whether or not we can avoid adding the
    /// <see cref="FromKeyedServicesAttribute"/> parameter to the resolve pipeline.
    /// </summary>
    private static void AddFromKeyedServiceParameterMiddleware(object? sender, ComponentRegisteredEventArgs e)
    {
        var needFromKeyedServiceParameter = false;

        // We can optimise quite significantly in the case where we are using the reflection activator.
        // In that state we can inspect the constructors ahead of time and determine whether the parameter will even need to be added.
        if (e.ComponentRegistration.Activator is ReflectionActivator reflectionActivator)
        {
            var constructors = reflectionActivator.ConstructorFinder.FindConstructors(reflectionActivator.LimitType);

            // Go through all the constructors; if any have a FromKeyedServices, then we must add our component middleware to
            // the pipeline.
            foreach (var constructor in constructors)
            {
                foreach (var constructorParameter in constructor.GetParameters())
                {
                    if (constructorParameter.GetCustomAttribute<FromKeyedServicesAttribute>() is not null)
                    {
                        // One or more of the constructors we will use to activate has a FromKeyedServicesAttribute,
                        // we must add our middleware.
                        needFromKeyedServiceParameter = true;
                        break;
                    }
                }

                if (needFromKeyedServiceParameter)
                {
                    break;
                }
            }
        }
        else if (e.ComponentRegistration.Activator is DelegateActivator)
        {
            // For delegate activation there are very few paths that would let the FromKeyedServicesAttribute
            // actually work, and none that MSDI supports directly.
            // We're explicitly choosing here not to support [FromKeyedServices] on the Autofac-specific generic
            // delegate resolve methods, to improve performance for the 99% case of other delegates that only
            // receive an IComponentContext or an IServiceProvider.
            needFromKeyedServiceParameter = false;
        }
        else if (e.ComponentRegistration.Activator is InstanceActivator)
        {
            // Instance activators don't use parameters.
            needFromKeyedServiceParameter = false;
        }
        else
        {
            // Unknown activator, assume we need the parameter.
            needFromKeyedServiceParameter = true;
        }

        e.ComponentRegistration.PipelineBuilding += (_, pipeline) =>
        {
            var keyedServiceMiddlewareType = typeof(AutofacServiceProvider).Assembly.GetType("Autofac.Extensions.DependencyInjection.KeyedServiceMiddleware");
            var instanceWithFromKeyedServicesParameter = (IResolveMiddleware)keyedServiceMiddlewareType!.GetProperty("InstanceWithFromKeyedServicesParameter", BindingFlags.Public | BindingFlags.Static)!.GetValue(null, null)!;
            var instanceWithoutFromKeyedServicesParameter = (IResolveMiddleware)keyedServiceMiddlewareType!.GetProperty("InstanceWithoutFromKeyedServicesParameter", BindingFlags.Public | BindingFlags.Static)!.GetValue(null, null)!;

            if (needFromKeyedServiceParameter)
            {
                pipeline.Use(instanceWithFromKeyedServicesParameter, MiddlewareInsertionMode.StartOfPhase);
            }
            else
            {
                pipeline.Use(instanceWithoutFromKeyedServicesParameter, MiddlewareInsertionMode.StartOfPhase);
            }
        };
    }

    /// <summary>
    /// Configures the exposed service type on a service registration.
    /// </summary>
    /// <typeparam name="TActivatorData">The activator data type.</typeparam>
    /// <typeparam name="TRegistrationStyle">The object registration style.</typeparam>
    /// <param name="registrationBuilder">The registration being built.</param>
    /// <param name="descriptor">The service descriptor with service type and key information.</param>
    /// <returns>
    /// The <paramref name="registrationBuilder" />, configured with the proper service type,
    /// and available for additional configuration.
    /// </returns>
    private static IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> ConfigureServiceType<TActivatorData, TRegistrationStyle>(
        this IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> registrationBuilder,
        ServiceDescriptor descriptor)
    {
        if (descriptor.IsKeyedService)
        {
            var key = descriptor.ServiceKey!;

            // If it's keyed, the service key won't be null. A null key results in it _not_ being a keyed service.
            registrationBuilder.Keyed(key, descriptor.ServiceType);
        }
        else
        {
            registrationBuilder.As(descriptor.ServiceType);
        }

        return registrationBuilder;
    }

    /// <summary>
    /// Configures the lifecycle on a service registration.
    /// </summary>
    /// <typeparam name="TActivatorData">The activator data type.</typeparam>
    /// <typeparam name="TRegistrationStyle">The object registration style.</typeparam>
    /// <param name="registrationBuilder">The registration being built.</param>
    /// <param name="lifecycleKind">The lifecycle specified on the service registration.</param>
    /// <param name="lifetimeScopeTagForSingleton">
    /// If not <see langword="null"/> then all registrations with lifetime <see cref="ServiceLifetime.Singleton" /> are registered
    /// using <see cref="IRegistrationBuilder{TLimit,TActivatorData,TRegistrationStyle}.InstancePerMatchingLifetimeScope" />
    /// with provided <paramref name="lifetimeScopeTagForSingleton"/>
    /// instead of using <see cref="IRegistrationBuilder{TLimit,TActivatorData,TRegistrationStyle}.SingleInstance"/>.
    /// </param>
    /// <returns>
    /// The <paramref name="registrationBuilder" />, configured with the proper lifetime scope,
    /// and available for additional configuration.
    /// </returns>
    private static IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> ConfigureLifecycle<TActivatorData, TRegistrationStyle>(
        this IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> registrationBuilder,
        ServiceLifetime lifecycleKind,
        object? lifetimeScopeTagForSingleton)
    {
        switch (lifecycleKind)
        {
            case ServiceLifetime.Singleton:
                if (lifetimeScopeTagForSingleton == null)
                {
                    registrationBuilder.SingleInstance();
                }
                else
                {
                    registrationBuilder.InstancePerMatchingLifetimeScope(lifetimeScopeTagForSingleton);
                }

                break;
            case ServiceLifetime.Scoped:
                registrationBuilder.InstancePerLifetimeScope();
                break;
            case ServiceLifetime.Transient:
                registrationBuilder.InstancePerDependency();
                break;
        }

        return registrationBuilder;
    }

    /// <summary>
    /// Populates the Autofac container builder with the set of registered service descriptors.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="ContainerBuilder"/> into which the registrations should be made.
    /// </param>
    /// <param name="services">
    /// A container builder that can be used to create an <see cref="IServiceProvider" />.
    /// </param>
    /// <param name="lifetimeScopeTagForSingletons">
    /// If not <see langword="null"/> then all registrations with lifetime <see cref="ServiceLifetime.Singleton" /> are registered
    /// using <see cref="IRegistrationBuilder{TLimit,TActivatorData,TRegistrationStyle}.InstancePerMatchingLifetimeScope" />
    /// with provided <paramref name="lifetimeScopeTagForSingletons"/>
    /// instead of using <see cref="IRegistrationBuilder{TLimit,TActivatorData,TRegistrationStyle}.SingleInstance"/>.
    /// </param>
    [SuppressMessage("CA2000", "CA2000", Justification = "Registrations created here are disposed when the built container is disposed.")]
    private static void Register(
        ContainerBuilder builder,
        IServiceCollection services,
        object? lifetimeScopeTagForSingletons)
    {
        var moduleContainer = services.GetSingletonInstance<IModuleContainer>();
        var registrationActionList = services.GetRegistrationActionList();
        var activatedActionList = services.GetServiceActivatedActionList();

        foreach (var descriptor in services)
        {
            var implementationType = descriptor.NormalizedImplementationType();
            if (implementationType != null)
            {
                // Test if the an open generic type is being registered
                var serviceTypeInfo = descriptor.ServiceType.GetTypeInfo();
                if (serviceTypeInfo.IsGenericTypeDefinition)
                {
                    builder
                        .RegisterGeneric(implementationType)
                        .ConfigureServiceType(descriptor)
                        .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons)
                        .ConfigureAbpConventions(descriptor, moduleContainer, registrationActionList, activatedActionList);
                }
                else
                {
                    builder
                        .RegisterType(implementationType)
                        .ConfigureServiceType(descriptor)
                        .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons)
                        .ConfigureAbpConventions(descriptor, moduleContainer, registrationActionList, activatedActionList);
                }

                continue;
            }

            if (descriptor.IsKeyedService && descriptor.KeyedImplementationFactory != null)
            {
                var registration = RegistrationBuilder.ForDelegate(descriptor.ServiceType, (context, parameters) =>
                {
                    // At this point the context is always a ResolveRequestContext, which will expose the actual service type.
                    var requestContext = (ResolveRequestContext)context;

                    var serviceProvider = context.Resolve<IServiceProvider>();

                    var keyedService = (Autofac.Core.KeyedService)requestContext.Service;

                    var key = keyedService.ServiceKey;

                    return descriptor.KeyedImplementationFactory(serviceProvider, key);
                })
                .ConfigureServiceType(descriptor)
                .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons)
                .CreateRegistration();
                //TODO: ConfigureAbpConventions ?

                builder.RegisterComponent(registration);

                continue;
            }

            if (!descriptor.IsKeyedService && descriptor.ImplementationFactory != null)
            {
                var registration = RegistrationBuilder.ForDelegate(descriptor.ServiceType, (context, parameters) =>
                    {
                        var serviceProvider = context.Resolve<IServiceProvider>();
                        return descriptor.ImplementationFactory(serviceProvider);
                    })
                    .ConfigureServiceType(descriptor)
                    .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons)
                    .CreateRegistration();
                //TODO: ConfigureAbpConventions ?

                builder.RegisterComponent(registration);

                continue;
            }

            // It's not a type or factory, so it must be an instance.
            builder
                .RegisterInstance(descriptor.NormalizedImplementationInstance()!)
                .ConfigureServiceType(descriptor)
                .ConfigureLifecycle(descriptor.Lifetime, null);
        }
    }
}
