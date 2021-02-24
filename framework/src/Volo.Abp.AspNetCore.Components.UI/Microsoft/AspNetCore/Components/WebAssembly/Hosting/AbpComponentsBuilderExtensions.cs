using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AspNetCore.Components.UI;
using Volo.Abp.AspNetCore.Components.UI.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;

namespace Microsoft.AspNetCore.Components.UI.Hosting
{
    public static class AbpComponentsBuilderExtensions
    {
        //public static IAbpApplicationWithExternalServiceProvider AddApplication<TStartupModule>(
        //    [NotNull] this IServiceCollection services
        //    /*Action<AbpWebAssemblyApplicationCreationOptions> options*/)
        //    where TStartupModule : IAbpModule
        //{
        //    Check.NotNull(services, nameof(services));

        //    // Related this commit(https://github.com/dotnet/aspnetcore/commit/b99d805bc037fcac56afb79abeb7d5a43141c85e)
        //    // Microsoft.AspNetCore.Blazor.BuildTools has been removed in net 5.0.
        //    // This call may be removed when we find a suitable solution.
        //    // System.Runtime.CompilerServices.AsyncStateMachineAttribute
        //    Castle.DynamicProxy.Generators.AttributesToAvoidReplicating.Add<AsyncStateMachineAttribute>();

        //    services.AddSingleton<IConfiguration>(builder.Configuration);
        //    services.AddSingleton(builder);

        //    var application = services.AddApplication<TStartupModule>(opts =>
        //    {
        //        options?.Invoke(new AbpAspNetCoreApplicationCreationOptions(builder, opts));
        //    });

        //    return application;
        //}

        public static async Task InitializeAsync(
            [NotNull] this IAbpApplicationWithExternalServiceProvider application,
            [NotNull] IServiceProvider serviceProvider)
        {
            Check.NotNull(application, nameof(application));
            Check.NotNull(serviceProvider, nameof(serviceProvider));

            var serviceProviderAccessor = (ComponentsClientScopeServiceProviderAccessor)
                serviceProvider.GetRequiredService<IClientScopeServiceProviderAccessor>();
            serviceProviderAccessor.ServiceProvider = serviceProvider;

            application.Initialize(serviceProvider);

            using (var scope = serviceProvider.CreateScope())
            {
                await InitializeModulesAsync(scope.ServiceProvider);
                await SetCurrentLanguageAsync(scope);
            }
        }

        private static async Task InitializeModulesAsync(IServiceProvider serviceProvider)
        {
            foreach (var service in serviceProvider.GetServices<IAsyncInitialize>())
            {
                await service.InitializeAsync();
            }
        }

        private static async Task SetCurrentLanguageAsync(IServiceScope scope)
        {
            var configurationClient = scope.ServiceProvider.GetRequiredService<ICachedApplicationConfigurationClient>();
            var utilsService = scope.ServiceProvider.GetRequiredService<IAbpUtilsService>();
            var configuration = configurationClient.Get();
            var cultureName = configuration.Localization?.CurrentCulture?.CultureName;
            if (!cultureName.IsNullOrEmpty())
            {
                var culture = new CultureInfo(cultureName);
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
            }

            if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
            {
                await utilsService.AddClassToTagAsync("body", "rtl");
            }
        }
    }
}
