using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AuditLogging.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Abp.AuditLogging
{
    public class AbpAuditLoggingDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {


            context.Services.Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources.Add<AuditLoggingResource>("en");
            });

            context.Services.AddAssemblyOf<AbpAuditLoggingDomainSharedModule>();
        }
    }
}
