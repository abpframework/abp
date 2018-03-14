using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace Volo.Abp.Auditing
{
    [DependsOn(typeof(AbpDataModule))] //TODO: Can we remove data module dependency since it only contains ISoftDelete related to auditing module!
    public class AbpAuditingModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<AbpAuditingModule>();
        }
    }
}
