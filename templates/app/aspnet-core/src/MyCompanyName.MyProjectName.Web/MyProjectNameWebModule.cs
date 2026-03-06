using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyCompanyName.MyProjectName.EntityFrameworkCore;
using MyCompanyName.MyProjectName.Localization;
using MyCompanyName.MyProjectName.MultiTenancy;
using MyCompanyName.MyProjectName.Web.Menus;
using Microsoft.OpenApi;
using OpenIddict.Validation.AspNetCore;
using Volo.Abp;
using Volo.Abp.Account.Web;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AspNetCore.Mvc.UI;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonXLite.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Mapperly;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity.Web;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.Web;
using Volo.Abp.Security.Claims;
using Volo.Abp.SettingManagement.Web;
using Volo.Abp.Swashbuckle;
using Volo.Abp.TenantManagement.Web;
using Volo.Abp.OpenIddict;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.UI;
using Volo.Abp.UI.Navigation;
using Volo.Abp.OperationRateLimiting;
using Volo.Abp.VirtualFileSystem;

namespace MyCompanyName.MyProjectName.Web;

[DependsOn(
    typeof(MyProjectNameHttpApiModule),
    typeof(MyProjectNameApplicationModule),
    typeof(MyProjectNameEntityFrameworkCoreModule),
    typeof(AbpAutofacModule),
    typeof(AbpIdentityWebModule),
    typeof(AbpSettingManagementWebModule),
    typeof(AbpAccountWebOpenIddictModule),
    typeof(AbpAspNetCoreMvcUiLeptonXLiteThemeModule),
    typeof(AbpTenantManagementWebModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule),
    typeof(AbpOperationRateLimitingModule)
    )]
public class MyProjectNameWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(
                typeof(MyProjectNameResource),
                typeof(MyProjectNameDomainModule).Assembly,
                typeof(MyProjectNameDomainSharedModule).Assembly,
                typeof(MyProjectNameApplicationModule).Assembly,
                typeof(MyProjectNameApplicationContractsModule).Assembly,
                typeof(MyProjectNameWebModule).Assembly
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

        if (!hostingEnvironment.IsDevelopment())
        {
            PreConfigure<AbpOpenIddictAspNetCoreOptions>(options =>
            {
                options.AddDevelopmentEncryptionAndSigningCertificate = false;
            });

            PreConfigure<OpenIddictServerBuilder>(serverBuilder =>
            {
                serverBuilder.AddProductionEncryptionAndSigningCertificate("openiddict.pfx", "00000000-0000-0000-0000-000000000000");
            });
        }
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var hostingEnvironment = context.Services.GetHostingEnvironment();
        var configuration = context.Services.GetConfiguration();

        ConfigureAuthentication(context);
        ConfigureUrls(configuration);
        ConfigureBundles();
        ConfigureVirtualFileSystem(hostingEnvironment);
        ConfigureNavigationServices();
        ConfigureAutoApiControllers();
        ConfigureSwaggerServices(context.Services);

        context.Services.AddMapperlyObjectMapper<MyProjectNameWebModule>();

        ConfigureOperationRateLimiting();
    }

    private void ConfigureOperationRateLimiting()
    {
        Configure<AbpOperationRateLimitingOptions>(options =>
        {
            // Demo 1: Public - rate limit by parameter (e.g. phone/email), no auth required
            options.AddPolicy("Demo_SendSmsCode", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromSeconds(20), maxCount: 3)
                      .PartitionByParameter();
            });

            // Demo 2: Public - rate limit by client IP
            options.AddPolicy("Demo_LoginAttempt", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromSeconds(20), maxCount: 5)
                      .PartitionByClientIp();
            });

            // Demo 3: Authenticated - rate limit by current user
            options.AddPolicy("Demo_GenerateApiKey", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromSeconds(20), maxCount: 3)
                      .PartitionByCurrentUser();
            });

            // Demo 4: Authenticated - rate limit by email (auto from current user)
            options.AddPolicy("Demo_SendEmailCode", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromSeconds(20), maxCount: 2)
                      .PartitionByEmail();
            });

            // Demo 5a: Composite - ByCurrentUser + ByClientIp
            // IP triggers first (3/20s). User window is 2min so it won't expire during testing.
            // After IP resets (20s), 2 more requests trigger user rule (5/2min).
            options.AddPolicy("Demo_Composite_UserIp", policy =>
            {
                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromMinutes(2), maxCount: 5)
                    .PartitionByCurrentUser());

                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromSeconds(20), maxCount: 3)
                    .PartitionByClientIp());
            });

            // Demo 5b: Composite - ByParameter + ByCurrentUser
            // User triggers first (2/20s). Parameter has higher limit (5/2min).
            // After user window resets, parameter counter remains.
            options.AddPolicy("Demo_Composite_ParamUser", policy =>
            {
                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromMinutes(2), maxCount: 5)
                    .PartitionByParameter());

                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromSeconds(20), maxCount: 2)
                    .PartitionByCurrentUser());
            });

            // Demo 5c: Composite - ByParameter + ByCurrentUser + ByClientIp (triple)
            // IP (3/20s) triggers first, then user (4/2min), then parameter (5/2min).
            options.AddPolicy("Demo_Composite_Triple", policy =>
            {
                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromMinutes(2), maxCount: 5)
                    .PartitionByParameter());

                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromMinutes(2), maxCount: 4)
                    .PartitionByCurrentUser());

                policy.AddRule(rule => rule
                    .WithFixedWindow(TimeSpan.FromSeconds(20), maxCount: 3)
                    .PartitionByClientIp());
            });

            // Demo 6: Custom error code
            options.AddPolicy("Demo_SubmitFeedback", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromSeconds(20), maxCount: 2)
                      .PartitionByParameter()
                      .WithErrorCode("App:Feedback:RateLimited");
            });

            // Demo 7: Long duration - hours (test "X hours Y minutes" formatting)
            options.AddPolicy("Demo_LongHours", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromHours(3), maxCount: 2)
                      .PartitionByParameter();
            });

            // Demo 8: Long duration - days (test "X days Y hours" formatting)
            options.AddPolicy("Demo_LongDays", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromDays(3), maxCount: 1)
                      .PartitionByClientIp();
            });

            // Demo 9: Custom multi-key resolver - combines resource ID (Parameter) + user ID
            // into a single partition key so each user has an independent quota per resource.
            options.AddPolicy("Demo_CustomMultiKey", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromSeconds(20), maxCount: 2)
                      .PartitionBy(ctx =>
                      {
                          var userId = ctx.GetRequiredService<Volo.Abp.Users.ICurrentUser>().Id?.ToString() ?? "anonymous";
                          return Task.FromResult($"{ctx.Parameter}:{userId}");
                      });
            });

            // Demo 10: Multi-tenancy - same parameter value has independent counters per tenant.
            // Without .WithMultiTenancy(), all tenants share the same counter.
            options.AddPolicy("Demo_TenantIsolated", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromSeconds(20), maxCount: 3)
                      .WithMultiTenancy()
                      .PartitionByParameter();
            });

            // Demo 11: Authenticated - PartitionByCurrentTenant
            // All users within the same tenant share one counter.
            options.AddPolicy("Demo_TenantWide", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromSeconds(20), maxCount: 3)
                      .PartitionByCurrentTenant();
            });

            // Demo 12: Ban policy (maxCount: 0) - permanently denies all requests
            options.AddPolicy("Demo_BanPolicy", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromSeconds(30), maxCount: 0)
                      .PartitionByParameter();
            });

            // Demo 13: IsAllowedAsync - read-only check without incrementing counter
            options.AddPolicy("Demo_PreCheck", policy =>
            {
                policy.WithFixedWindow(TimeSpan.FromSeconds(20), maxCount: 3)
                      .PartitionByParameter();
            });
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context)
    {
        context.Services.ForwardIdentityAuthenticationForBearer(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
        {
            options.IsDynamicClaimsEnabled = true;
        });
    }

    private void ConfigureUrls(IConfiguration configuration)
    {
        Configure<AppUrlOptions>(options =>
        {
            options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
        });
    }

    private void ConfigureBundles()
    {
        Configure<AbpBundlingOptions>(options =>
        {
            options.StyleBundles.Configure(
                LeptonXLiteThemeBundles.Styles.Global,
                bundle =>
                {
                    bundle.AddFiles("/global-styles.css");
                }
            );
        });
    }

    private void ConfigureVirtualFileSystem(IWebHostEnvironment hostingEnvironment)
    {
        if (hostingEnvironment.IsDevelopment())
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                //<TEMPLATE-REMOVE>
                options.FileSets.ReplaceEmbeddedByPhysical<AbpUiModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}..{0}..{0}framework{0}src{0}Volo.Abp.UI", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<AbpAspNetCoreMvcUiModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}..{0}..{0}framework{0}src{0}Volo.Abp.AspNetCore.Mvc.UI", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<AbpAspNetCoreMvcUiBootstrapModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}..{0}..{0}framework{0}src{0}Volo.Abp.AspNetCore.Mvc.UI.Bootstrap", Path.DirectorySeparatorChar)));
                //options.FileSets.ReplaceEmbeddedByPhysical<AbpAspNetCoreMvcUiThemeSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}..{0}..{0}framework{0}src{0}Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<AbpAspNetCoreMvcUiMultiTenancyModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}..{0}..{0}framework{0}src{0}Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<AbpPermissionManagementWebModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}..{0}..{0}modules{0}permission-management{0}src{0}Volo.Abp.PermissionManagement.Web", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<AbpFeatureManagementWebModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}..{0}..{0}modules{0}feature-management{0}src{0}Volo.Abp.FeatureManagement.Web", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<AbpIdentityWebModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}..{0}..{0}modules{0}identity{0}src{0}Volo.Abp.Identity.Web", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<AbpAccountWebOpenIddictModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}..{0}..{0}modules{0}account{0}src{0}Volo.Abp.Account.Web.OpenIddict", Path.DirectorySeparatorChar)));
                options.FileSets.ReplaceEmbeddedByPhysical<AbpTenantManagementWebModule>(Path.Combine(hostingEnvironment.ContentRootPath, string.Format("..{0}..{0}..{0}..{0}..{0}modules{0}tenant-management{0}src{0}Volo.Abp.TenantManagement.Web", Path.DirectorySeparatorChar)));
                //</TEMPLATE-REMOVE>
                options.FileSets.ReplaceEmbeddedByPhysical<MyProjectNameDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}MyCompanyName.MyProjectName.Domain.Shared"));
                options.FileSets.ReplaceEmbeddedByPhysical<MyProjectNameDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}MyCompanyName.MyProjectName.Domain"));
                options.FileSets.ReplaceEmbeddedByPhysical<MyProjectNameApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}MyCompanyName.MyProjectName.Application.Contracts"));
                options.FileSets.ReplaceEmbeddedByPhysical<MyProjectNameApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}MyCompanyName.MyProjectName.Application"));
                options.FileSets.ReplaceEmbeddedByPhysical<MyProjectNameWebModule>(hostingEnvironment.ContentRootPath);
            });
        }
    }

    private void ConfigureNavigationServices()
    {
        Configure<AbpNavigationOptions>(options =>
        {
            options.MenuContributors.Add(new MyProjectNameMenuContributor());
        });
    }

    private void ConfigureAutoApiControllers()
    {
        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            options.ConventionalControllers.Create(typeof(MyProjectNameApplicationModule).Assembly);
        });
    }

    private void ConfigureSwaggerServices(IServiceCollection services)
    {
        services.AddAbpSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "MyProjectName API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            }
        );
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
        app.MapAbpStaticAssets();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAbpOpenIddictValidation();

        if (MultiTenancyConsts.IsEnabled)
        {
            app.UseMultiTenancy();
        }

        app.UseUnitOfWork();
        app.UseDynamicClaims();
        app.UseAuthorization();

        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "MyProjectName API");
        });

        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
