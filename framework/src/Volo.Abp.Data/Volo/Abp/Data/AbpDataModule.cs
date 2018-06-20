using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Data
{
    public class AbpDataModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));

            services.AddAssemblyOf<AbpDataModule>();
        }
    }
}
