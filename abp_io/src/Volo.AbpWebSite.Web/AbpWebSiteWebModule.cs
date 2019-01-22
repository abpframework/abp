using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.UI;
using Volo.Abp.VirtualFileSystem;
using Volo.AbpWebSite.Bundling;
using Volo.Blogging;
using Volo.Docs;

namespace Volo.AbpWebSite
{
    [DependsOn(
        typeof(AbpWebSiteApplicationModule),
        typeof(AbpWebSiteEntityFrameworkCoreModule),
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMvcUiThemeSharedModule),
        typeof(DocsApplicationModule),
        typeof(DocsWebModule),
        typeof(AbpAccountWebModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpIdentityWebModule),
        typeof(BloggingApplicationModule),
        typeof(BloggingWebModule)
        )]
    public class AbpWebSiteWebModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            ConfigureLanguages(context.Services);
            ConfigureDatabaseServices(context.Services, configuration);
            ConfigureVirtualFileSystem(context.Services, hostingEnvironment);
            ConfigureBundles(context.Services);
            ConfigureTheme(context.Services);
        }

        private static void ConfigureLanguages(IServiceCollection services)
        {
            services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en-US", "en-US", "English"));
            });
        }

        private static void ConfigureBundles(IServiceCollection services)
        {
            services.Configure<BundlingOptions>(options =>
            {
                options
                    .StyleBundles
                    .Add(AbpIoBundles.Styles.Global, bundle =>
                    {
                        bundle.
                            AddBaseBundles(StandardBundles.Styles.Global)
                            .AddFiles(
                                "/scss/vs.css",
                                "/js/prism/prism.css"
                                );
                    });

                options
                    .ScriptBundles
                    .Add(AbpIoBundles.Scripts.Global, bundle =>
                    {
                        bundle.AddBaseBundles(StandardBundles.Scripts.Global);
                    });
            });
        }

        private static void ConfigureDatabaseServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.Configure<DbConnectionOptions>(options =>
            {
                options.ConnectionStrings.Default = configuration.GetConnectionString("Default");
            });

            services.Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
            });
        }

        private static void ConfigureVirtualFileSystem(IServiceCollection services, IHostingEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                services.Configure<VirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<AbpUiModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}framework{0}src{0}Volo.Abp.UI", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPhysical<AbpAspNetCoreMvcUiModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}framework{0}src{0}Volo.Abp.AspNetCore.Mvc.UI", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPhysical<AbpAspNetCoreMvcUiBootstrapModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}framework{0}src{0}Volo.Abp.AspNetCore.Mvc.UI.Bootstrap", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPhysical<AbpAspNetCoreMvcUiThemeSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}framework{0}src{0}Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPhysical<DocsDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}modules{0}docs{0}src{0}Volo.Docs.Domain", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPhysical<DocsWebModule>(Path.Combine(hostingEnvironment.ContentRootPath,    string.Format("..{0}..{0}..{0}modules{0}docs{0}src{0}Volo.Docs.Web", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPhysical<BloggingWebModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}modules{0}blogging{0}src{0}Volo.Blogging.Web", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPhysical<AbpAccountWebModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}modules{0}account{0}src{0}Volo.Abp.Account.Web", Path.DirectorySeparatorChar)));
                });
            }
        }

        private void ConfigureTheme(IServiceCollection services)
        {
            services.Configure<ThemingOptions>(options =>
            {
                options.Themes.Add<AbpIoTheme>();
                options.DefaultThemeName = AbpIoTheme.Name;
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            app.UseAbpRequestLocalization();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseErrorPage();
            }

            //Necessary for LetsEncrypt
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @".well-known")),
                RequestPath = new PathString("/.well-known"),
                ServeUnknownFileTypes = true // serve extensionless file
            });

            app.UseVirtualFiles();

            app.UseAuthentication();

            app.UseMvcWithDefaultRouteAndArea();

            AsyncHelper.RunSync(async () =>
            {
                await context.ServiceProvider
                    .GetRequiredService<IIdentityDataSeeder>()
                    .SeedAsync(
                        "1q2w3E*",
                        IdentityPermissions.GetAll()
                            .Union(BloggingPermissions.GetAll())
                    );
            });
        }
    }
}
