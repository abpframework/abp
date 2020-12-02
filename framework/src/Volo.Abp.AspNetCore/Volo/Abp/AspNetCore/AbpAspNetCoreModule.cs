﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.Auditing;
using Volo.Abp.Authorization;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;
using Volo.Abp.Modularity;
using Volo.Abp.Security;
using Volo.Abp.Uow;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.AspNetCore
{
    [DependsOn(
        typeof(AbpAuditingModule),
        typeof(AbpSecurityModule),
        typeof(AbpVirtualFileSystemModule),
        typeof(AbpUnitOfWorkModule),
        typeof(AbpHttpModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpValidationModule),
        typeof(AbpExceptionHandlingModule)
        )]
    public class AbpAspNetCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAuditingOptions>(options =>
            {
                options.Contributors.Add(new AspNetCoreAuditLogContributor());
            });

            AddAspNetServices(context.Services);
            context.Services.AddObjectAccessor<IApplicationBuilder>();

            context.Services.AddAbpDynamicOptions<RequestLocalizationOptions, AbpRequestLocalizationOptionsManager>();
        }

        private static void AddAspNetServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
        }
    }
}
