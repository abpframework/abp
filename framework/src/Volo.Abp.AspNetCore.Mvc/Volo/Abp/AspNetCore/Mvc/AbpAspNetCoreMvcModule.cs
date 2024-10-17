using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Volo.Abp.ApiVersioning;
using Volo.Abp.Application;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Mvc.ApiExploring;
using Volo.Abp.AspNetCore.Mvc.ApplicationModels;
using Volo.Abp.AspNetCore.Mvc.Conventions;
using Volo.Abp.AspNetCore.Mvc.DataAnnotations;
using Volo.Abp.AspNetCore.Mvc.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Infrastructure;
using Volo.Abp.AspNetCore.Mvc.Json;
using Volo.Abp.AspNetCore.Mvc.Libs;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.Http;
using Volo.Abp.DynamicProxy;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Json.SystemTextJson;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Validation.Localization;

namespace Volo.Abp.AspNetCore.Mvc;

[DependsOn(
    typeof(AbpAspNetCoreModule),
    typeof(AbpLocalizationModule),
    typeof(AbpApiVersioningAbstractionsModule),
    typeof(AbpAspNetCoreMvcContractsModule),
    typeof(AbpUiNavigationModule),
    typeof(AbpGlobalFeaturesModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpJsonSystemTextJsonModule)
    )]
public class AbpAspNetCoreMvcModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        DynamicProxyIgnoreTypes.Add<ControllerBase>();
        DynamicProxyIgnoreTypes.Add<PageModel>();
        DynamicProxyIgnoreTypes.Add<ViewComponent>();

        context.Services.AddConventionalRegistrar(new AbpAspNetCoreMvcConventionalRegistrar());
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpApiDescriptionModelOptions>(options =>
        {
            options.IgnoredInterfaces.AddIfNotContains(typeof(IAsyncActionFilter));
            options.IgnoredInterfaces.AddIfNotContains(typeof(IFilterMetadata));
            options.IgnoredInterfaces.AddIfNotContains(typeof(IActionFilter));
        });

        Configure<AbpRemoteServiceApiDescriptionProviderOptions>(options =>
        {
            var statusCodes = new List<int>
            {
                (int) HttpStatusCode.Forbidden,
                (int) HttpStatusCode.Unauthorized,
                (int) HttpStatusCode.BadRequest,
                (int) HttpStatusCode.NotFound,
                (int) HttpStatusCode.NotImplemented,
                (int) HttpStatusCode.InternalServerError
            };

            options.SupportedResponseTypes.AddIfNotContains(statusCodes.Select(statusCode => new ApiResponseType
            {
                Type = typeof(RemoteServiceErrorResponse),
                StatusCode = statusCode
            }));
        });

        context.Services.PostConfigure<AbpAspNetCoreMvcOptions>(options =>
        {
            if (options.MinifyGeneratedScript == null)
            {
                options.MinifyGeneratedScript = context.Services.GetHostingEnvironment().IsProduction();
            }
        });

        var mvcCoreBuilder = context.Services.AddMvcCore(options =>
        {
            options.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute());
        });
        context.Services.ExecutePreConfiguredActions(mvcCoreBuilder);

        var abpMvcDataAnnotationsLocalizationOptions = context.Services
            .ExecutePreConfiguredActions(
                new AbpMvcDataAnnotationsLocalizationOptions()
            );

        context.Services
            .AddSingleton<IOptions<AbpMvcDataAnnotationsLocalizationOptions>>(
                new OptionsWrapper<AbpMvcDataAnnotationsLocalizationOptions>(
                    abpMvcDataAnnotationsLocalizationOptions
                )
            );

        var mvcBuilder = context.Services.AddMvc()
            .AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    var resourceType = abpMvcDataAnnotationsLocalizationOptions
                        .AssemblyResources
                        .GetOrDefault(type.Assembly);

                    if (resourceType != null)
                    {
                        return factory.Create(resourceType);
                    }

                    return factory.CreateDefaultOrNull() ??
                            factory.Create(type);
                };
            })
            .AddViewLocalization(); //TODO: How to configure from the application? Also, consider to move to a UI module since APIs does not care about it.

        if (context.Services.GetHostingEnvironment().IsDevelopment() &&
            context.Services.ExecutePreConfiguredActions<AbpAspNetCoreMvcOptions>().EnableRazorRuntimeCompilationOnDevelopment)
        {
            mvcCoreBuilder.AddAbpRazorRuntimeCompilation();
        }

        mvcCoreBuilder.AddAbpJson();

        context.Services.ExecutePreConfiguredActions(mvcBuilder);

        //TODO: AddViewLocalization by default..?

        context.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

        //Use DI to create controllers
        mvcBuilder.AddControllersAsServices();

        //Use DI to create view components
        mvcBuilder.AddViewComponentsAsServices();

        //Use DI to create razor page
        context.Services.Replace(ServiceDescriptor.Singleton<IPageModelActivatorProvider, ServiceBasedPageModelActivatorProvider>());

        //Add feature providers
        var partManager = context.Services.GetSingletonInstance<ApplicationPartManager>();
        var application = context.Services.GetSingletonInstance<IAbpApplication>();

        partManager.FeatureProviders.Add(new AbpConventionalControllerFeatureProvider(application));
        partManager.ApplicationParts.AddIfNotContains(typeof(AbpAspNetCoreMvcModule).Assembly);

        context.Services.Replace(ServiceDescriptor.Singleton<IValidationAttributeAdapterProvider, AbpValidationAttributeAdapterProvider>());
        context.Services.AddSingleton<ValidationAttributeAdapterProvider>();

        context.Services.TryAddEnumerable(ServiceDescriptor.Transient<IActionDescriptorProvider, AbpMvcActionDescriptorProvider>());
        context.Services.AddOptions<MvcOptions>()
            .Configure<IServiceProvider>((mvcOptions, serviceProvider) =>
            {
                mvcOptions.AddAbp(context.Services);

                // serviceProvider is root service provider.
                var stringLocalizer = serviceProvider.GetRequiredService<IStringLocalizer<AbpValidationResource>>();
                mvcOptions.ModelBindingMessageProvider.SetValueIsInvalidAccessor(_ => stringLocalizer["The value '{0}' is invalid."]);
                mvcOptions.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => stringLocalizer["The field must be a number."]);
                mvcOptions.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(value => stringLocalizer["The field {0} must be a number.", value]);
            });

        Configure<AbpEndpointRouterOptions>(options =>
        {
            options.EndpointConfigureActions.Add(endpointContext =>
            {
                endpointContext.Endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
                endpointContext.Endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}").WithStaticAssets();
                endpointContext.Endpoints.MapRazorPages().WithStaticAssets();
            });
        });

        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule("abp");
        });

        context.Services.Replace(ServiceDescriptor.Singleton<IHttpResponseStreamWriterFactory, AbpMemoryPoolHttpResponseStreamWriterFactory>());
    }

    public override void PostConfigureServices(ServiceConfigurationContext context)
    {
        ApplicationPartSorter.Sort(
            context.Services.GetSingletonInstance<ApplicationPartManager>(),
            context.Services.GetSingletonInstance<IModuleContainer>()
        );

        var preConfigureActions = context.Services.GetPreConfigureActions<AbpAspNetCoreMvcOptions>();

        DynamicProxyIgnoreTypes.Add(preConfigureActions.Configure()
            .ConventionalControllers
            .ConventionalControllerSettings.SelectMany(x => x.ControllerTypes).ToArray());

        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            preConfigureActions.Configure(options);
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        AddApplicationParts(context);
        CheckLibs(context);
    }

    private static void AddApplicationParts(ApplicationInitializationContext context)
    {
        var partManager = context.ServiceProvider.GetService<ApplicationPartManager>();
        if (partManager == null)
        {
            return;
        }

        var moduleContainer = context.ServiceProvider.GetRequiredService<IModuleContainer>();

        var plugInModuleAssemblies = moduleContainer
            .Modules
            .Where(m => m.IsLoadedAsPlugIn)
            .SelectMany(m => m.AllAssemblies)
            .Distinct();

        AddToApplicationParts(partManager, plugInModuleAssemblies);

        var controllerAssemblies = context
            .ServiceProvider
            .GetRequiredService<IOptions<AbpAspNetCoreMvcOptions>>()
            .Value
            .ConventionalControllers
            .ConventionalControllerSettings
            .Select(s => s.Assembly)
            .Distinct();

        AddToApplicationParts(partManager, controllerAssemblies);

        var additionalAssemblies = moduleContainer
            .Modules
            .SelectMany(m => m.GetAdditionalAssemblies())
            .Distinct();

        AddToApplicationParts(partManager, additionalAssemblies);
    }

    private static void AddToApplicationParts(ApplicationPartManager partManager, IEnumerable<Assembly> moduleAssemblies)
    {
        foreach (var moduleAssembly in moduleAssemblies)
        {
            partManager.ApplicationParts.AddIfNotContains(moduleAssembly);
        }
    }

    private static void CheckLibs(ApplicationInitializationContext context)
    {
        context.ServiceProvider.GetRequiredService<IAbpMvcLibsService>().CheckLibs(context);
    }
}
