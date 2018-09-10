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

namespace Volo.BloggingTestApp
{
    [DependsOn(
        typeof(BloggingWebModule),
        typeof(BloggingApplicationModule),
        typeof(BloggingTestAppEntityFrameworkCoreModule),
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

            context.Services.Configure<DbConnectionOptions>(options =>
            {
                const string connStringName = "SqlServer";
                options.ConnectionStrings.Default = configuration.GetConnectionString(connStringName);
            });

            context.Services.Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
            });

            if (hostingEnvironment.IsDevelopment())
            {
                context.Services.Configure<VirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPyhsical<AbpUiModule>(Path.Combine(hostingEnvironment.ContentRootPath, "..\\..\\..\\..\\framework\\src\\Volo.Abp.UI"));
                    options.FileSets.ReplaceEmbeddedByPyhsical<AbpAspNetCoreMvcUiModule>(Path.Combine(hostingEnvironment.ContentRootPath, "..\\..\\..\\..\\framework\\src\\Volo.Abp.AspNetCore.Mvc.UI"));
                    options.FileSets.ReplaceEmbeddedByPyhsical<AbpAspNetCoreMvcUiBootstrapModule>(Path.Combine(hostingEnvironment.ContentRootPath, "..\\..\\..\\..\\framework\\src\\Volo.Abp.AspNetCore.Mvc.UI.Bootstrap"));
                    options.FileSets.ReplaceEmbeddedByPyhsical<AbpAspNetCoreMvcUiThemeSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, "..\\..\\..\\..\\framework\\src\\Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared"));
                    options.FileSets.ReplaceEmbeddedByPyhsical<AbpAspNetCoreMvcUiBasicThemeModule>(Path.Combine(hostingEnvironment.ContentRootPath, "..\\..\\..\\..\\framework\\src\\Volo.Abp.AspNetCore.Mvc.UI.Theme.Basic"));
                    options.FileSets.ReplaceEmbeddedByPyhsical<BloggingDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, "..\\..\\src\\Volo.Blogging.Domain"));
                    options.FileSets.ReplaceEmbeddedByPyhsical<BloggingWebModule>(Path.Combine(hostingEnvironment.ContentRootPath, "..\\..\\src\\Volo.Blogging.Web"));
                });
            }

            context.Services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new Info { Title = "Blogging API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                });

            var cultures = new List<CultureInfo> { new CultureInfo("en"), new CultureInfo("tr") };
            context.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });

            context.Services.Configure<ThemingOptions>(options =>
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
