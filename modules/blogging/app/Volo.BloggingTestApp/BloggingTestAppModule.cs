//#define MONGODB

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Modularity;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;
using Volo.Abp.Autofac;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.Web;
using Volo.Abp.Modularity;
using Volo.Abp.Threading;
using Volo.Abp.UI;
using Volo.Abp.VirtualFileSystem;
using Volo.Blogging;
using Volo.BloggingTestApp.EntityFrameworkCore;
using Volo.BloggingTestApp.MongoDb;

namespace Volo.BloggingTestApp
{
    [DependsOn(
        typeof(BloggingWebModule),
        typeof(BloggingApplicationModule),
#if MONGODB
               typeof(BloggingTestAppMongoDbModule),
#else
        typeof(BloggingTestAppEntityFrameworkCoreModule),
#endif
        typeof(AbpAccountWebModule),
        typeof(AbpIdentityWebModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMvcUiBasicThemeModule)
    )]
    public class BloggingTestAppModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.BuildConfiguration();

            Configure<DbConnectionOptions>(options =>
            {
#if MONGODB
                const string connStringName = "MongoDb";
#else
                const string connStringName = "SqlServer";
#endif
                options.ConnectionStrings.Default = configuration.GetConnectionString(connStringName);
            });

#if !MONGODB
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
            });
#endif
            if (hostingEnvironment.IsDevelopment())
            {
                Configure<VirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPyhsical<AbpUiModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}..{0}framework{0}src{0}Volo.Abp.UI", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPyhsical<AbpAspNetCoreMvcUiModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}..{0}framework{0}src{0}Volo.Abp.AspNetCore.Mvc.UI", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPyhsical<AbpAspNetCoreMvcUiBootstrapModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}..{0}framework{0}src{0}Volo.Abp.AspNetCore.Mvc.UI.Bootstrap", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPyhsical<AbpAspNetCoreMvcUiThemeSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}..{0}framework{0}src{0}Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPyhsical<AbpAspNetCoreMvcUiBasicThemeModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}..{0}framework{0}src{0}Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPyhsical<BloggingDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}Volo.Blogging.Domain", Path.DirectorySeparatorChar)));
                    options.FileSets.ReplaceEmbeddedByPyhsical<BloggingWebModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}src{0}Volo.Blogging.Web", Path.DirectorySeparatorChar)));
                });
            }

            context.Services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new Info { Title = "Blogging API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                });

            var cultures = new List<CultureInfo> { new CultureInfo("en"), new CultureInfo("tr") };
            Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });

            Configure<ThemingOptions>(options =>
            {
                options.DefaultThemeName = BasicTheme.Name;
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            if (context.GetEnvironment().IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseErrorPage();
            }

            app.UseVirtualFiles();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Support APP API");
            });

            app.UseAuthentication();

            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            AsyncHelper.RunSync(async () =>
            {
                await context.ServiceProvider
                    .GetRequiredService<IIdentityDataSeeder>()
                    .SeedAsync(
                        "1q2w3E*",
                        IdentityPermissions.GetAll().Union(BloggingPermissions.GetAll())
                    );
            });
        }
    }
}
