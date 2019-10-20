using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Auditing;
using Volo.Abp.Auditing;
using Volo.Abp.Authorization;
using Volo.Abp.Domain;
using Volo.Abp.Http;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Security;
using Volo.Abp.UI;
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
        typeof(AbpDddDomainModule), //TODO: Can we remove this?
        typeof(AbpLocalizationModule),
        typeof(AbpUiModule), //TODO: Can we remove this?
        typeof(AbpValidationModule)
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
        }

        private static void AddAspNetServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
        }
    }
}
