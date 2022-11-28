using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace Microsoft.Extensions.DependencyInjection;

public static class WebApplicationBuilderExtensions
{
    public static async Task<IAbpApplicationWithExternalServiceProvider> AddApplicationAsync<TStartupModule>(
        [NotNull] this WebApplicationBuilder builder,
        [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction = null)
        where TStartupModule : IAbpModule
    {
        return await builder.Services.AddApplicationAsync<TStartupModule>(options =>
        {
            options.Services.ReplaceConfiguration(builder.Configuration);
            optionsAction?.Invoke(options);
            if (options.Environment.IsNullOrWhiteSpace())
            {
                options.Environment = builder.Environment.EnvironmentName;
            }
        });
    }

    public static async Task<IAbpApplicationWithExternalServiceProvider> AddApplicationAsync(
        [NotNull] this WebApplicationBuilder builder,
        [NotNull] Type startupModuleType,
        [CanBeNull] Action<AbpApplicationCreationOptions> optionsAction = null)
    {
        return await builder.Services.AddApplicationAsync(startupModuleType, options =>
        {
            options.Services.ReplaceConfiguration(builder.Configuration);
            optionsAction?.Invoke(options);
            if (options.Environment.IsNullOrWhiteSpace())
            {
                options.Environment = builder.Environment.EnvironmentName;
            }
        });
    }
}
