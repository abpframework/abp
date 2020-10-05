using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AspNetCore.Components.WebAssembly;
using Volo.Abp.AspNetCore.Mvc.Client;
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

        public async static Task InitializeAsync(
            [NotNull] this IAbpApplicationWithExternalServiceProvider application,
            [NotNull] IServiceProvider serviceProvider)
        {
            Check.NotNull(application, nameof(application));
            Check.NotNull(serviceProvider, nameof(serviceProvider));

            application.Initialize(serviceProvider);

            using (var scope = serviceProvider.CreateScope())
            {
                await InitializeModulesAsync(scope.ServiceProvider);
                SetCurrentLanguage(scope);
            }
        }

        private async static Task InitializeModulesAsync(IServiceProvider serviceProvider)
        {
            foreach (var service in serviceProvider.GetServices<IAsyncInitialize>())
            {
                await service.InitializeAsync();
            }
        }

        private static void SetCurrentLanguage(IServiceScope scope)
        {
            var configurationClient = scope.ServiceProvider.GetRequiredService<ICachedApplicationConfigurationClient>();
            var configuration = configurationClient.Get();
            var cultureName = configuration.Localization?.CurrentCulture?.CultureName;
            if (!cultureName.IsNullOrEmpty())
            {
                var culture = new CultureInfo(cultureName);
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
            }
        }
    }
}
