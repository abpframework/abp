using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.AspNetCore.GlobalAssets;
using Volo.Abp.AspNetCore.VirtualFileSystem;
using Volo.Abp.Auditing;
using Volo.Abp.Authorization;
using Volo.Abp.Bundling;
using Volo.Abp.Bundling.Styles;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;
using Volo.Abp.Minify.Scripts;
using Volo.Abp.Minify.Styles;
using Volo.Abp.Modularity;
using Volo.Abp.Security;
using Volo.Abp.Uow;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore;

[DependsOn(
    typeof(AbpAuditingModule),
    typeof(AbpSecurityModule),
    typeof(AbpVirtualFileSystemModule),
    typeof(AbpUnitOfWorkModule),
    typeof(AbpHttpModule),
    typeof(AbpAuthorizationModule),
    typeof(AbpValidationModule),
    typeof(AbpExceptionHandlingModule),
    typeof(AbpAspNetCoreAbstractionsModule)
    )]
public class AbpAspNetCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var abpHostEnvironment = context.Services.GetSingletonInstance<IAbpHostEnvironment>();
        if (abpHostEnvironment.EnvironmentName.IsNullOrWhiteSpace())
        {
            abpHostEnvironment.EnvironmentName = context.Services.GetHostingEnvironment().EnvironmentName;
        }
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAuthorization();

        Configure<AbpAuditingOptions>(options =>
        {
            options.Contributors.Add(new AspNetCoreAuditLogContributor());
        });

        Configure<StaticFileOptions>(options =>
        {
            options.ContentTypeProvider = context.Services.GetRequiredService<AbpFileExtensionContentTypeProvider>();
        });

        AddAspNetServices(context.Services);
        context.Services.AddObjectAccessor<IApplicationBuilder>();
        context.Services.AddAbpDynamicOptions<RequestLocalizationOptions, AbpRequestLocalizationOptionsManager>();
    }

    private static void AddAspNetServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var environment = context.GetEnvironmentOrNull();
        if (environment != null)
        {
            environment.WebRootFileProvider =
                new CompositeFileProvider(
                    context.GetEnvironment().WebRootFileProvider,
                    context.ServiceProvider.GetRequiredService<IWebContentFileProvider>()
                );
        }

        InitialGlobalAssets(context);
    }

    protected virtual void InitialGlobalAssets(ApplicationInitializationContext context)
    {
         var globalAssetsOptions = context.ServiceProvider.GetRequiredService<IOptions<AbpGlobalAssetsOptions>>().Value;
         if (globalAssetsOptions.StartupModuleType != null)
         {
             var bundleContributorTypes = AbpModuleHelper
                 .FindAllModuleTypes(globalAssetsOptions.StartupModuleType!, null)
                 .Where(x => x.Assembly.ExportedTypes.Any(t => t.IsAssignableTo(typeof(IBundleContributor))))
                 .SelectMany(x => x.Assembly.ExportedTypes.Where(t => t.IsAssignableTo(typeof(IBundleContributor))).Select(t => t))
                 .Distinct()
                 .ToArray();

            var bundleContributors = new List<IBundleContributor>();
             foreach (var type in bundleContributorTypes.Reverse())
             {
                 var bundleContributorInstance = Activator.CreateInstance(type);
                 if (bundleContributorInstance != null && bundleContributorInstance is IBundleContributor bundleContributor)
                 {
                     bundleContributors.Add(bundleContributor);
                 }
             }

             var styleBundleContext = new BundleContext();
             var scriptBundleContext = new BundleContext();
             foreach (var contributor in bundleContributors)
             {
                 contributor.AddStyles(styleBundleContext);
                 contributor.AddScripts(scriptBundleContext);
             }

             var webHostEnvironment = context.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
             var dynamicFileProvider = context.ServiceProvider.GetRequiredService<IDynamicFileProvider>();
             var javascriptMinifier = context.ServiceProvider.GetRequiredService<IJavascriptMinifier>();
             var cssMinifier = context.ServiceProvider.GetRequiredService<ICssMinifier>();

             var styles = string.Empty;
             foreach (var bundleDefinition in styleBundleContext.BundleDefinitions.Where(x => !x.ExcludeFromBundle))
             {
                 var fileInfo = webHostEnvironment.WebRootFileProvider?.GetFileInfo(bundleDefinition.Source);
                 if (fileInfo == null || !fileInfo.Exists)
                 {
                     continue;
                 }

                 var fileContent = fileInfo.ReadAsString();

                 fileContent = CssRelativePath.Adjust(fileContent,
                     bundleDefinition.Source,
                     Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));

                 if (globalAssetsOptions.Minify && !globalAssetsOptions.IsMinificationIgnored(bundleDefinition.Source))
                 {
                     styles += $"{Environment.NewLine}{Environment.NewLine}/*{bundleDefinition.Source}*/{Environment.NewLine}{cssMinifier.Minify(fileContent)}";
                 }
                 else
                 {
                     styles += $"{Environment.NewLine}{Environment.NewLine}/*{bundleDefinition.Source}*/{Environment.NewLine}{fileContent}";
                 }
             }

             dynamicFileProvider.AddOrUpdate(
                 new InMemoryFileInfo("/wwwroot/" + globalAssetsOptions.CssFileName,
                     Encoding.UTF8.GetBytes(styles),
                     globalAssetsOptions.CssFileName));

             var scripts = string.Empty;
             foreach (var bundleDefinition in scriptBundleContext.BundleDefinitions.Where(x => !x.ExcludeFromBundle))
             {
                 var fileInfo = webHostEnvironment.WebRootFileProvider?.GetFileInfo(bundleDefinition.Source);
                 if (fileInfo == null || !fileInfo.Exists)
                 {
                     continue;
                 }

                 var fileContent = fileInfo.ReadAsString();

                 if (globalAssetsOptions.Minify && !globalAssetsOptions.IsMinificationIgnored(bundleDefinition.Source))
                 {
                     scripts += $"{Environment.NewLine}{Environment.NewLine}//{bundleDefinition.Source}{Environment.NewLine}{javascriptMinifier.Minify(fileContent)};{Environment.NewLine}";
                 }
                 else
                 {
                     scripts += $"{Environment.NewLine}{Environment.NewLine}//{bundleDefinition.Source}{Environment.NewLine}{fileContent};{Environment.NewLine}";
                 }
             }

             dynamicFileProvider.AddOrUpdate(
                 new InMemoryFileInfo("/wwwroot/" + globalAssetsOptions.JavaScriptFileName,
                     Encoding.UTF8.GetBytes(scripts),
                     globalAssetsOptions.JavaScriptFileName));
         }
    }
}
