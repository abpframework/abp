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
        typeof(AuditingModule), 
        typeof(SecurityModule),
        typeof(VirtualFileSystemModule),
        typeof(UnitOfWorkModule),
        typeof(HttpModule),
        typeof(AuthorizationModule),
        typeof(DddDomainModule), //TODO: Can we remove this?
        typeof(LocalizationModule),
        typeof(UiModule), //TODO: Can we remove this?
        typeof(ValidationModule)
        )]
    public class AspNetCoreModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddConfiguration();
        }

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
