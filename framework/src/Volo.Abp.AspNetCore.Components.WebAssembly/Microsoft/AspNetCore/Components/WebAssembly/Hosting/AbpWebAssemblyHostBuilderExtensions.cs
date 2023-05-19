using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AspNetCore.Components.Web.DependencyInjection;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;

namespace Microsoft.AspNetCore.Components.WebAssembly.Hosting;

public static class AbpWebAssemblyHostBuilderExtensions
{
    public async static Task<IAbpApplicationWithExternalServiceProvider> AddApplicationAsync<TStartupModule>(
        [NotNull] this WebAssemblyHostBuilder builder,
        Action<AbpWebAssemblyApplicationCreationOptions> options)
        where TStartupModule : IAbpModule
    {
        Check.NotNull(builder, nameof(builder));

        // Related this commit(https://github.com/dotnet/aspnetcore/commit/b99d805bc037fcac56afb79abeb7d5a43141c85e)
        // Microsoft.AspNetCore.Blazor.BuildTools has been removed in net 5.0.
        // This call may be removed when we find a suitable solution.
        // System.Runtime.CompilerServices.AsyncStateMachineAttribute
        Castle.DynamicProxy.Generators.AttributesToAvoidReplicating.Add<AsyncStateMachineAttribute>();

        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
        builder.Services.AddSingleton(builder);

        var application = await builder.Services.AddApplicationAsync<TStartupModule>(opts =>
        {
            options?.Invoke(new AbpWebAssemblyApplicationCreationOptions(builder, opts));
            if (opts.Environment.IsNullOrWhiteSpace())
            {
                opts.Environment = builder.HostEnvironment.Environment;
            }
        });

        return application;
    }

    public static IAbpApplicationWithExternalServiceProvider AddApplication<TStartupModule>(
        [NotNull] this WebAssemblyHostBuilder builder,
        Action<AbpWebAssemblyApplicationCreationOptions> options)
        where TStartupModule : IAbpModule
    {
        Check.NotNull(builder, nameof(builder));

        // Related this commit(https://github.com/dotnet/aspnetcore/commit/b99d805bc037fcac56afb79abeb7d5a43141c85e)
        // Microsoft.AspNetCore.Blazor.BuildTools has been removed in net 5.0.
        // This call may be removed when we find a suitable solution.
        // System.Runtime.CompilerServices.AsyncStateMachineAttribute
        Castle.DynamicProxy.Generators.AttributesToAvoidReplicating.Add<AsyncStateMachineAttribute>();

        builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
        builder.Services.AddSingleton(builder);

        var application = builder.Services.AddApplication<TStartupModule>(opts =>
        {
            options?.Invoke(new AbpWebAssemblyApplicationCreationOptions(builder, opts));
            if (opts.Environment.IsNullOrWhiteSpace())
            {
                opts.Environment = builder.HostEnvironment.Environment;
            }
        });

        return application;
    }

    public async static Task InitializeApplicationAsync(
        [NotNull] this IAbpApplicationWithExternalServiceProvider application,
        [NotNull] IServiceProvider serviceProvider)
    {
        Check.NotNull(application, nameof(application));
        Check.NotNull(serviceProvider, nameof(serviceProvider));

        ((ComponentsClientScopeServiceProviderAccessor)serviceProvider
            .GetRequiredService<IClientScopeServiceProviderAccessor>()).ServiceProvider = serviceProvider;

        await application.InitializeAsync(serviceProvider);
    }
}
