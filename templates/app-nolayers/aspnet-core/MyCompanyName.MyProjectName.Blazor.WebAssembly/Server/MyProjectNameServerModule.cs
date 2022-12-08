using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using MyCompanyName.MyProjectName.Server.Data;
using MyCompanyName.MyProjectName.Shared.Localization;
using MyCompanyName.MyProjectName.Shared;
using MyCompanyName.MyProjectName.Shared.MultiTenancy;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.AutoMapper;
using Volo.Abp.Emailing;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.PermissionManagement.OpenIddict;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.Uow;
using Volo.Abp.VirtualFileSystem;

namespace MyCompanyName.MyProjectName.Server;

[DependsOn(

    typeof(MyProjectNameSharedModule),

    // ABP Framework packages
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAspNetCoreMultiTenancyModule),
    typeof(AbpAutofacModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpAspNetCoreSerilogModule),

    // Account module packages
    typeof(AbpAccountApplicationModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpAccountWebOpenIddictModule),

    // Identity module packages
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpPermissionManagementDomainOpenIddictModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpIdentityEntityFrameworkCoreModule),
    typeof(AbpOpenIddictEntityFrameworkCoreModule),

    // Audit logging module packages
    typeof(AbpAuditLoggingEntityFrameworkCoreModule),

    // Permission Management module packages
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpPermissionManagementEntityFrameworkCoreModule),

    // Tenant Management module packages
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpTenantManagementHttpApiModule),
    typeof(AbpTenantManagementEntityFrameworkCoreModule),

    // Feature Management module packages
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpFeatureManagementEntityFrameworkCoreModule),
    typeof(AbpFeatureManagementHttpApiModule),

    // Setting Management module packages
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpSettingManagementEntityFrameworkCoreModule),
    typeof(AbpSettingManagementHttpApiModule)
)]
public class MyProjectNameServerModule : AbpModule
{
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
            {
                options.AddAssemblyResource(
                    typeof(MyProjectNameResource)
                );
            });

    		PreConfigure<OpenIddictBuilder>(builder =>
    		{
    			builder.AddValidation(options =>
    			{
    				options.AddAudiences("MyProjectName");
    				options.UseLocalServer();
    				options.UseAspNetCore();
    			});
    		});
        }

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();
            var configuration = context.Services.GetConfiguration();

            if (hostingEnvironment.IsDevelopment())
            {
                context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
            }

            ConfigureAuthentication(context);
            ConfigureBundles();
            ConfigureMultiTenancy();
            ConfigureUrls(configuration);
            ConfigureAutoMapper(context);
            ConfigureSwagger(context.Services, configuration);
            ConfigureAutoApiControllers();
            ConfigureVirtualFiles(hostingEnvironment);
            ConfigureCors(context, configuration);
            ConfigureDataProtection(context);
            ConfigureEfCore(context);
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context)
        {
            context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        }

        private void ConfigureBundles()
        {
            Configure<AbpBundlingOptions>(options =>
            {
                options.StyleBundles.Configure(
                    LeptonXLiteThemeBundles.Styles.Global,
                    bundle => { bundle.AddFiles("/global-styles.css"); }
                );
            });
        }

        private void ConfigureMultiTenancy()
        {
            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsEnabled;
            });
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
                options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"]?.Split(',') ?? Array.Empty<string>());
            });
        }

        private void ConfigureVirtualFiles(IWebHostEnvironment hostingEnvironment)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<MyProjectNameServerModule>();
                if (hostingEnvironment.IsDevelopment())
                {
                    /* Using physical files in development, so we don't need to recompile on changes */
                    options.FileSets.ReplaceEmbeddedByPhysical<MyProjectNameServerModule>(hostingEnvironment.ContentRootPath);
                }
            });
        }

        private void ConfigureAutoApiControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(MyProjectNameServerModule).Assembly);
            });
        }

        private void ConfigureSwagger(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAbpSwaggerGenWithOAuth(
                configuration["AuthServer:Authority"],
                new Dictionary<string, string>
                {
                        {"MyProjectName", "MyProjectName API"}
                },
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MyProjectName API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.CustomSchemaIds(type => type.FullName);
                });
        }

        private void ConfigureAutoMapper(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<MyProjectNameServerModule>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                /* Uncomment `validate: true` if you want to enable the Configuration Validation feature.
                 * See AutoMapper's documentation to learn what it is:
                 * https://docs.automapper.org/en/stable/Configuration-validation.html
                 */
                options.AddMaps<MyProjectNameServerModule>(/* validate: true */);
            });
        }

        private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins(
                            configuration["App:CorsOrigins"]?
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray() ?? Array.Empty<string>()
                        )
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }

        private void ConfigureDataProtection(ServiceConfigurationContext context)
        {
            context.Services.AddDataProtection().SetApplicationName("MyProjectName");
        }

        private void ConfigureEfCore(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<MyProjectNameDbContext>(options =>
            {
                /* You can remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots
                 * Documentation: https://docs.abp.io/en/abp/latest/Entity-Framework-Core#add-default-repositories
                 */
                options.AddDefaultRepositories(includeAllEntities: true);
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.Configure(configurationContext =>
                {
                    configurationContext.UseSqlServer();
                });
            });

            //<TEMPLATE-REMOVE IF-NOT='dbms:SQLite'>
            Configure<AbpUnitOfWorkDefaultOptions>(options =>
            {
                options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled;
            });
            //</TEMPLATE-REMOVE>
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();

            if (!env.IsDevelopment())
            {
                app.UseErrorPage();
            }

            app.UseCorrelationId();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAbpOpenIddictValidation();

            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }

            app.UseUnitOfWork();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseAbpSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "MyProjectName API");

                var configuration = context.GetConfiguration();
                options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                options.OAuthScopes("MyProjectName");
            });

            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();

            if (app is WebApplication webApp)
            {
                webApp.MapFallbackToFile("index.html");
            }
        }
}
