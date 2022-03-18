using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.AspNetCore.SignalR.Auditing;
using Volo.Abp.AspNetCore.SignalR.Authentication;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.SignalR;

[DependsOn(
    typeof(AbpAspNetCoreModule)
    )]
public class AbpAspNetCoreSignalRModule : AbpModule
{
    private static readonly MethodInfo MapHubGenericMethodInfo =
        typeof(AbpAspNetCoreSignalRModule)
            .GetMethod("MapHub", BindingFlags.Static | BindingFlags.NonPublic);

    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddConventionalRegistrar(new AbpSignalRConventionalRegistrar());

        AutoAddHubTypes(context.Services);
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var routePatterns = new List<string> { "/signalr-hubs" };
        var signalRServerBuilder = context.Services.AddSignalR(options =>
        {
            options.AddFilter<AbpHubContextAccessorHubFilter>();
            options.AddFilter<AbpAuthenticationHubFilter>();
            options.AddFilter<AbpAuditHubFilter>();
        });

        context.Services.ExecutePreConfiguredActions(signalRServerBuilder);

        Configure<AbpEndpointRouterOptions>(options =>
        {
            options.EndpointConfigureActions.Add(endpointContext =>
            {
                var signalROptions = endpointContext
                    .ScopeServiceProvider
                    .GetRequiredService<IOptions<AbpSignalROptions>>()
                    .Value;

                foreach (var hubConfig in signalROptions.Hubs)
                {
                    routePatterns.AddIfNotContains(hubConfig.RoutePattern);

                    MapHubType(
                        hubConfig.HubType,
                        endpointContext.Endpoints,
                        hubConfig.RoutePattern,
                        opts =>
                        {
                            foreach (var configureAction in hubConfig.ConfigureActions)
                            {
                                configureAction(opts);
                            }
                        }
                    );
                }
            });
        });

        Configure<AbpAspNetCoreAuditingOptions>(options =>
        {
            foreach (var routePattern in routePatterns)
            {
                options.IgnoredUrls.AddIfNotContains(x => routePattern.StartsWith(x), () => routePattern);
            }
        });

        Configure<AbpAuditingOptions>(options =>
        {
            options.Contributors.Add(new AspNetCoreSignalRAuditLogContributor());
        });
    }

    private void AutoAddHubTypes(IServiceCollection services)
    {
        var hubTypes = new List<Type>();

        services.OnRegistred(context =>
        {
            if (IsHubClass(context) && !IsDisabledForAutoMap(context))
            {
                hubTypes.Add(context.ImplementationType);
            }
        });

        services.Configure<AbpSignalROptions>(options =>
        {
            foreach (var hubType in hubTypes)
            {
                options.Hubs.Add(HubConfig.Create(hubType));
            }
        });
    }

    private static bool IsHubClass(IOnServiceRegistredContext context)
    {
        return typeof(Hub).IsAssignableFrom(context.ImplementationType);
    }

    private static bool IsDisabledForAutoMap(IOnServiceRegistredContext context)
    {
        return context.ImplementationType.IsDefined(typeof(DisableAutoHubMapAttribute), true);
    }

    private void MapHubType(
        Type hubType,
        IEndpointRouteBuilder endpoints,
        string pattern,
        Action<HttpConnectionDispatcherOptions> configureOptions)
    {
        MapHubGenericMethodInfo
            .MakeGenericMethod(hubType)
            .Invoke(
                this,
                new object[]
                {
                        endpoints,
                        pattern,
                        configureOptions
                }
            );
    }

    // ReSharper disable once UnusedMember.Local (used via reflection)
    private static void MapHub<THub>(
        IEndpointRouteBuilder endpoints,
        string pattern,
        Action<HttpConnectionDispatcherOptions> configureOptions)
        where THub : Hub
    {
        endpoints.MapHub<THub>(
            pattern,
            configureOptions
        );
    }
}
