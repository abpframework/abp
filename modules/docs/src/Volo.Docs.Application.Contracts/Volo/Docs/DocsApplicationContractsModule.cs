using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Docs
{
    [DependsOn(typeof(DocsDomainSharedModule))]
    public class DocsApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAssemblyOf<DocsApplicationContractsModule>();
        }
    }
}
