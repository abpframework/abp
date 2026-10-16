using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Autofac.Core;
using Autofac.Core.Resolving.Pipeline;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Volo.Abp;
using Volo.Abp.Autofac;

namespace Autofac.Builder;

/// <summary>
/// The class and interface interception of Autofac.Extras.DynamicProxy, generating proxies with
/// <see cref="ProxyGenerator"/> instead of the single static generator that package keeps for the whole process.
/// Interceptors are read from the same registration metadata as Autofac's, so <c>InterceptedBy</c> and
/// <see cref="InterceptAttribute"/> keep working. It follows Autofac.Extras.DynamicProxy 7.1.0, whose metadata keys it
/// reads; check it again when that package is upgraded.
/// </summary>
public static class AbpInterceptionRegistrationExtensions
{
    // Autofac.Extras.DynamicProxy's metadata keys, written by its InterceptedBy.
    private const string InterceptorsPropertyName = "Autofac.Extras.DynamicProxy.RegistrationExtensions.InterceptorsPropertyName";
    private const string AttributeInterceptorsPropertyName = "Autofac.Extras.DynamicProxy.RegistrationExtensions.AttributeInterceptorsPropertyName";

    private static readonly object SyncRoot = new();
    private static int _shardCount = 16;
    private static volatile ProxyGenerator? _proxyGenerator;

    /// <summary>
    /// How many dynamic modules proxies are spread over. It is read when <see cref="ProxyGenerator"/> is first used,
    /// which happens while the container is built, and cannot change after that, because a key must keep its shard.
    /// Defaults to 16.
    /// </summary>
    public static int ShardCount
    {
        get
        {
            lock (SyncRoot)
            {
                return _shardCount;
            }
        }
        set
        {
            Check.Positive(value, nameof(value));
            lock (SyncRoot)
            {
                if (_proxyGenerator != null && value != _shardCount)
                {
                    throw new AbpException("The proxy shard count cannot change once the proxy generator has been created.");
                }

                _shardCount = value;
            }
        }
    }

    public static ProxyGenerator ProxyGenerator
    {
        get
        {
            var generator = _proxyGenerator;
            if (generator != null)
            {
                return generator;
            }

            lock (SyncRoot)
            {
                return _proxyGenerator ??= new ProxyGenerator(new AbpShardedProxyBuilder(_shardCount));
            }
        }
    }

    public static IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> EnableAbpClassInterceptors<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle>(
        this IRegistrationBuilder<TLimit, TConcreteReflectionActivatorData, TRegistrationStyle> registration,
        ProxyGenerationOptions? options = null)
        where TConcreteReflectionActivatorData : ConcreteReflectionActivatorData
    {
        Check.NotNull(registration, nameof(registration));

        options ??= ProxyGenerationOptions.Default;
        registration.ActivatorData.ImplementationType = ProxyGenerator.ProxyBuilder.CreateClassProxyType(
            registration.ActivatorData.ImplementationType,
            Type.EmptyTypes,
            options);

        var interceptorServices = GetInterceptorServicesFromAttributes(registration.ActivatorData.ImplementationType);
        AddInterceptorServicesToMetadata(registration, interceptorServices, AttributeInterceptorsPropertyName);

        // The proxy's constructor takes the options' mixins, then the interceptors, then the options' selector.
        registration.OnPreparing(e =>
        {
            var proxyParameters = new List<Parameter>();
            var index = 0;

            if (options.HasMixins)
            {
                foreach (var mixin in options.MixinData.Mixins)
                {
                    proxyParameters.Add(new PositionalParameter(index++, mixin));
                }
            }

            proxyParameters.Add(new PositionalParameter(index++, GetInterceptorServices(e.Component, registration.ActivatorData.ImplementationType)
                .Select(s => e.Context.ResolveService(s))
                .Cast<IInterceptor>()
                .ToArray()));

            if (options.Selector != null)
            {
                proxyParameters.Add(new PositionalParameter(index, options.Selector));
            }

            e.Parameters = proxyParameters.Concat(e.Parameters).ToArray();
        });

        return registration;
    }

    public static IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> EnableAbpInterfaceInterceptors<TLimit, TActivatorData, TSingleRegistrationStyle>(
        this IRegistrationBuilder<TLimit, TActivatorData, TSingleRegistrationStyle> registration,
        ProxyGenerationOptions? options = null)
    {
        Check.NotNull(registration, nameof(registration));

        registration.ConfigurePipeline(p => p.Use(PipelinePhase.Activation, MiddlewareInsertionMode.StartOfPhase, (ctx, next) =>
        {
            next(ctx);

            EnsureInterfaceInterceptionApplies(ctx.Registration);

            var proxiedInterfaces = ProxiedInterfaces.GetValue(ctx.Instance!.GetType(), static type => new ProxiedInterfaceSet(type));
            if (proxiedInterfaces.Primary == null)
            {
                return;
            }

            var theInterface = proxiedInterfaces.Primary;
            var interfaces = proxiedInterfaces.Additional;

            var interceptors = GetInterceptorServices(ctx.Registration, ctx.Instance.GetType())
                .Select(s => ctx.ResolveService(s))
                .Cast<IInterceptor>()
                .ToArray();

            ctx.Instance = options == null
                ? ProxyGenerator.CreateInterfaceProxyWithTarget(theInterface, interfaces, ctx.Instance, interceptors)
                : ProxyGenerator.CreateInterfaceProxyWithTarget(theInterface, interfaces, ctx.Instance, options, interceptors);
        }));

        return registration;
    }

    private static readonly ConditionalWeakTable<Type, ProxiedInterfaceSet> ProxiedInterfaces = new();

    /// <summary>
    /// The interfaces an interface proxy of a type implements, worked out once per type.
    /// </summary>
    /// <remarks>
    /// The proxy implements the same interfaces whichever comes first, but Castle names it after the first one and copies
    /// some of that one's attributes to the proxy type. Preferring the class's own I{ClassName} interface gives most
    /// proxies a name of their own instead of one shared by every implementation of a common interface, which keeps them
    /// spread over the proxy builder's modules. It is only preferred when neither interface has a public attribute other
    /// than the compiler's nullable annotations, so the proxy type's attributes stay as they were whatever Castle is
    /// configured to copy.
    /// </remarks>
    private sealed class ProxiedInterfaceSet
    {
        public Type? Primary { get; }

        public Type[] Additional { get; }

        public ProxiedInterfaceSet(Type type)
        {
            var interfaces = type.GetInterfaces().Where(ProxyUtil.IsAccessible).ToArray();
            if (interfaces.Length == 0)
            {
                Additional = Type.EmptyTypes;
                return;
            }

            var primary = interfaces[0];
            var own = interfaces.FirstOrDefault(i => i.Name == "I" + type.Name);
            if (own != null && own != primary && !HasPublicAttributes(primary) && !HasPublicAttributes(own))
            {
                primary = own;
            }

            Primary = primary;
            Additional = interfaces.Where(i => i != primary).ToArray();
        }

        // The compiler's nullable annotations are public on .NET but say nothing a caller reads off a proxy type;
        // counting them would keep nearly every interface of a nullable-enabled project in place.
        private static readonly string[] NullableAnnotations =
        {
            "System.Runtime.CompilerServices.NullableAttribute",
            "System.Runtime.CompilerServices.NullableContextAttribute",
            "System.Runtime.CompilerServices.NullablePublicOnlyAttribute"
        };

        private static bool HasPublicAttributes(Type interfaceType)
        {
            return interfaceType.CustomAttributes.Any(attribute =>
                (attribute.AttributeType.IsPublic || attribute.AttributeType.IsNestedPublic) &&
                !NullableAnnotations.Contains(attribute.AttributeType.FullName));
        }
    }

    private static void EnsureInterfaceInterceptionApplies(IComponentRegistration componentRegistration)
    {
        if (componentRegistration.Services
            .OfType<IServiceWithType>()
            .Any(s => !s.ServiceType.GetTypeInfo().IsInterface || !ProxyUtil.IsAccessible(s.ServiceType)))
        {
            throw new InvalidOperationException(
                $"The component {componentRegistration} cannot use interface interception as it provides services that are not publicly visible interfaces.");
        }
    }

    private static void AddInterceptorServicesToMetadata<TLimit, TActivatorData, TStyle>(
        IRegistrationBuilder<TLimit, TActivatorData, TStyle> builder,
        IEnumerable<Service> interceptorServices,
        string metadataKey)
    {
        if (builder.RegistrationData.Metadata.TryGetValue(metadataKey, out var existing) && existing is IEnumerable<Service> existingServices)
        {
            builder.RegistrationData.Metadata[metadataKey] = existingServices.Concat(interceptorServices).Distinct();
        }
        else
        {
            builder.RegistrationData.Metadata.Add(metadataKey, interceptorServices);
        }
    }

    private static IEnumerable<Service> GetInterceptorServices(IComponentRegistration registration, Type implType)
    {
        var result = Enumerable.Empty<Service>();

        if (registration.Metadata.TryGetValue(InterceptorsPropertyName, out var services) && services is IEnumerable<Service> existingPropertyServices)
        {
            result = result.Concat(existingPropertyServices);
        }

        return registration.Metadata.TryGetValue(AttributeInterceptorsPropertyName, out services) && services is IEnumerable<Service> existingAttributeServices
            ? result.Concat(existingAttributeServices)
            : result.Concat(GetInterceptorServicesFromAttributes(implType));
    }

    private static IEnumerable<Service> GetInterceptorServicesFromAttributes(Type implType)
    {
        var implTypeInfo = implType.GetTypeInfo();
        if (!implTypeInfo.IsClass)
        {
            return Enumerable.Empty<Service>();
        }

        var classAttributeServices = implTypeInfo
            .GetCustomAttributes(typeof(InterceptAttribute), true)
            .Cast<InterceptAttribute>()
            .Select(att => att.InterceptorService);

        var interfaceAttributeServices = implType
            .GetInterfaces()
            .SelectMany(i => i.GetTypeInfo().GetCustomAttributes(typeof(InterceptAttribute), true))
            .Cast<InterceptAttribute>()
            .Select(att => att.InterceptorService);

        return classAttributeServices.Concat(interfaceAttributeServices);
    }

}
