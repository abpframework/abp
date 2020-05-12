using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Microsoft.OpenApi.Models;
using MsDemo.Shared;
using Volo.Abp;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.Security.Claims;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace IdentityService.Host
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(AbpEventBusRabbitMqModule),
        typeof(AbpEntityFrameworkCoreSqlServerModule),
        typeof(AbpAuditLoggingEntityFrameworkCoreModule),
        typeof(AbpPermissionManagementEntityFrameworkCoreModule),
        typeof(AbpSettingManagementEntityFrameworkCoreModule),
        typeof(AbpIdentityHttpApiModule),
        typeof(AbpIdentityEntityFrameworkCoreModule),
        typeof(AbpIdentityApplicationModule),
        typeof(AbpAspNetCoreMultiTenancyModule),
        typeof(AbpTenantManagementEntityFrameworkCoreModule)
        )]
    public class IdentityServiceHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MsDemoConsts.IsMultiTenancyEnabled;
            });

            context.Services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = configuration["AuthServer:Authority"];
                    options.ApiName = configuration["AuthServer:ApiName"];
                    options.RequireHttpsMetadata = false;
                });

            context.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo {Title = "Identity Service API", Version = "v1"});
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
            });

            Configure<AbpDbContextOptions>(options =>
            {
                options.UseSqlServer();
            });

            context.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:Configuration"];
            });

            Configure<AbpAuditingOptions>(options =>
            {
                options.IsEnabledForGetRequests = true;
                options.ApplicationName = "IdentityService";
            });

            var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]);
            context.Services.AddDataProtection()
                .PersistKeysToStackExchangeRedis(redis, "MsDemo-DataProtection-Keys");
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();

            app.UseCorrelationId();
            app.UseVirtualFiles();
            app.UseRouting();
            app.UseAuthentication();
            if (MsDemoConsts.IsMultiTenancyEnabled)
            {
                app.UseMultiTenancy();
            }
            app.Use(async (ctx, next) =>
            {
                var currentPrincipalAccessor = ctx.RequestServices.GetRequiredService<ICurrentPrincipalAccessor>();
                var map = new Dictionary<string, string>()
                {
                    { "sub", AbpClaimTypes.UserId },
                    { "role", AbpClaimTypes.Role },
                    { "email", AbpClaimTypes.Email },
                    //any other map
                };
                var mapClaims = currentPrincipalAccessor.Principal.Claims.Where(p => map.Keys.Contains(p.Type)).ToList();
                currentPrincipalAccessor.Principal.AddIdentity(new ClaimsIdentity(mapClaims.Select(p => new Claim(map[p.Type], p.Value, p.ValueType, p.Issuer))));
                await next();
            });

            app.UseAbpRequestLocalization(); //TODO: localization?
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity Service API");
            });
            app.UseAuditing();
            app.UseConfiguredEndpoints();
        }
    }
}
