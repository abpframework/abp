using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.Data
{
    public class AbpDataModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<DbConnectionOptions>(configuration);

            context.Services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));
        }
    }
}
