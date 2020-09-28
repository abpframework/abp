using System;
using System.Reflection;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.Modularity;

namespace Microsoft.AspNetCore.Components.WebAssembly.Hosting
{
    public static class AbpWebAssemblyHostBuilderExtensions
    {
        public static IAbpApplicationWithExternalServiceProvider AddApplication<TStartupModule>(
            [NotNull] this WebAssemblyHostBuilder builder,
            Action<AbpWebAssemblyApplicationCreationOptions> options)
            where TStartupModule : IAbpModule
        {
            Check.NotNull(builder, nameof(builder));

            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
            builder.Services.AddSingleton(builder);

            var application = builder.Services.AddApplication<TStartupModule>(opts =>
            {
                options?.Invoke(new AbpWebAssemblyApplicationCreationOptions(builder, opts));
            });

            return application;
        }

        public static async Task InitializeAsync(
            [NotNull] this IAbpApplicationWithExternalServiceProvider application,
            [NotNull] IServiceProvider serviceProvider)
        {
            Check.NotNull(application, nameof(application));
            Check.NotNull(serviceProvider, nameof(serviceProvider));

            application.Initialize(serviceProvider);

            using (var scope = serviceProvider.CreateScope())
            {
                foreach (var service in scope.ServiceProvider.GetServices<IAsyncInitialize>())
                {
                    await service.InitializeAsync();
                }
            }
        }
    }
}
