using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.ApiVersioning;
using Volo.Abp.Application;
using Volo.Abp.AspNetCore.Mvc.Conventions;
using Volo.Abp.AspNetCore.Mvc.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.VirtualFileSystem;
using Volo.Abp.Http;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Localization;
using Volo.Abp.UI;

namespace Volo.Abp.AspNetCore.Mvc
{
    [DependsOn(typeof(AbpAspNetCoreModule))]
    [DependsOn(typeof(AbpLocalizationModule))]
    [DependsOn(typeof(AbpApiVersioningAbstractionsModule))]
    [DependsOn(typeof(AbpDddApplicationModule))]
    [DependsOn(typeof(AbpUiModule))]
    public class AbpAspNetCoreMvcModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddConventionalRegistrar(new AbpAspNetCoreMvcConventionalRegistrar());
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            //Configure Razor
            context.Services.Insert(0,
                ServiceDescriptor.Singleton<IConfigureOptions<RazorViewEngineOptions>>(
                    new ConfigureOptions<RazorViewEngineOptions>(options =>
                        {
                            options.FileProviders.Add(
                                new RazorViewEngineVirtualFileProvider(
                                    context.Services.GetSingletonInstance<IObjectAccessor<IServiceProvider>>()
                                )
                            );
                        }
                    )
                )
            );

            context.Services.Configure<ApiDescriptionModelOptions>(options =>
            {
                options.IgnoredInterfaces.AddIfNotContains(typeof(IAsyncActionFilter));
                options.IgnoredInterfaces.AddIfNotContains(typeof(IFilterMetadata));
                options.IgnoredInterfaces.AddIfNotContains(typeof(IActionFilter));
            });

            context.Services.Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(AbpAspNetCoreMvcModule).Assembly, o =>
                {
                    o.RootPath = "abp";
                });
            });

            var mvcCoreBuilder = context.Services.AddMvcCore();
            context.Services.ExecutePreConfiguredActions(mvcCoreBuilder);

            var mvcBuilder = context.Services.AddMvc()
                .AddDataAnnotationsLocalization(options =>
                {
                    var assemblyResources = context.Services.ExecutePreConfiguredActions(new AbpMvcDataAnnotationsLocalizationOptions()).AssemblyResources;

                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var resourceType = assemblyResources.GetOrDefault(type.Assembly);
                        return factory.Create(resourceType ?? type);
                    };
                })
                .AddViewLocalization(); //TODO: How to configure from the application? Also, consider to move to a UI module since APIs does not care about it.

            context.Services.ExecutePreConfiguredActions(mvcBuilder);

            //TODO: AddViewLocalization by default..?

            context.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //Use DI to create controllers
            context.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            //Use DI to create view components
            context.Services.Replace(ServiceDescriptor.Singleton<IViewComponentActivator, ServiceBasedViewComponentActivator>());

            //Add feature providers
            var partManager = context.Services.GetSingletonInstance<ApplicationPartManager>();
            var application = context.Services.GetSingletonInstance<IAbpApplication>();

            partManager.FeatureProviders.Add(new AbpConventionalControllerFeatureProvider(application));

            context.Services.Configure<MvcOptions>(mvcOptions =>
            {
                mvcOptions.AddAbp(context.Services);
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            AddApplicationParts(context);
        }

        private static void AddApplicationParts(ApplicationInitializationContext context)
        {
            var partManager = context.ServiceProvider.GetService<ApplicationPartManager>();
            if (partManager == null)
            {
                return;
            }

            //Plugin modules
            var moduleAssemblies = context
                .ServiceProvider
                .GetRequiredService<IModuleContainer>()
                .Modules
                .Where(m => m.IsLoadedAsPlugIn)
                .Select(m => m.Type.Assembly)
                .Distinct();

            AddToApplicationParts(partManager, moduleAssemblies);

            //Controllers for application services
            var controllerAssemblies = context
                .ServiceProvider
                .GetRequiredService<IOptions<AbpAspNetCoreMvcOptions>>()
                .Value
                .ConventionalControllers
                .ConventionalControllerSettings
                .Select(s => s.Assembly)
                .Distinct();

            AddToApplicationParts(partManager, controllerAssemblies);
        }

        private static void AddToApplicationParts(ApplicationPartManager partManager, IEnumerable<Assembly> moduleAssemblies)
        {
            foreach (var moduleAssembly in moduleAssemblies)
            {
                if (partManager.ApplicationParts.OfType<AssemblyPart>().Any(p => p.Assembly == moduleAssembly))
                {
                    continue;
                }

                partManager.ApplicationParts.Add(new AssemblyPart(moduleAssembly));
            }
        }
    }
}
