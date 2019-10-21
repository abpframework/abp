using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyCompanyName.MyProjectName.EntityFrameworkCore;
using MyCompanyName.MyProjectName.MultiTenancy;
using StackExchange.Redis;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace MyCompanyName.MyProjectName
{
    [DependsOn(
        typeof(MyProjectNameHttpApiModule),
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
        typeof(MyProjectNameApplicationModule),
        typeof(MyProjectNameEntityFrameworkCoreDbMigrationsModule)
        )]
    public class MyProjectNameHttpApiHostModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            ConfigureConventionalControllers();
            ConfigureAuthentication(context, configuration);
            ConfigureLocalization();
            ConfigureCache(configuration);
            ConfigureVirtualFileSystem(context);
            ConfigureRedis(context, configuration, hostingEnvironment);
            ConfigureCors(context, configuration);
            ConfigureSwaggerServices(context);
        }

        private void ConfigureCache(IConfiguration configuration)
        {
            Configure<AbpDistributedCacheOptions>(options =>
            {
                options.KeyPrefix = "MyProjectName:";
            });
        }

        private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<MyProjectNameDomainSharedModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}MyCompanyName.MyProjectName.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<MyProjectNameDomainModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}MyCompanyName.MyProjectName.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<MyProjectNameApplicationContractsModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}MyCompanyName.MyProjectName.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<MyProjectNameApplicationModule>(Path.Combine(hostingEnvironment.ContentRootPath, $"..{Path.DirectorySeparatorChar}MyCompanyName.MyProjectName.Application"));
                });
            }
        }

        private void ConfigureConventionalControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(MyProjectNameApplicationModule).Assembly);
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.RequireHttpsMetadata = true;
                    options.ApiName = "MyProjectName";
                });
        }

        private static void ConfigureSwaggerServices(ServiceConfigurationContext context)
        {
            context.Services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo {Title = "MyProjectName API", Version = "v1"});
                    options.DocInclusionPredicate((docName, description) => true);
                });
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("cs", "cs", "Čeština"));
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("pt-BR", "pt-BR", "Português"));
                options.Languages.Add(new LanguageInfo("tr", "tr", "Türkçe"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
            });
        }

        private void ConfigureRedis(
            ServiceConfigurationContext context,
            IConfiguration configuration,
            IWebHostEnvironment hostingEnvironment)
        {
            context.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:Configuration"];
            });

            if (!hostingEnvironment.IsDevelopment())
            {
                var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
                context.Services
                    .AddDataProtection()
                    .PersistKeysToStackExchangeRedis(redis, "MyProjectName-Protection-Keys");
            }
        }

        private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    builder
                        .WithOrigins(
                            configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseCorrelationId();
            app.UseVirtualFiles();
            app.UseRouting();
            app.UseCors(DefaultCorsPolicyName);
            app.UseAuthentication();
            app.UseAuthorization();
            if (MultiTenancyConsts.IsEnabled)
            {
                app.UseMultiTenancy();
            }
            app.UseAbpRequestLocalization();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "MyProjectName API");
            });

            app.UseAuditing();
            app.UseMvcWithDefaultRouteAndArea();
        }
    }
}
