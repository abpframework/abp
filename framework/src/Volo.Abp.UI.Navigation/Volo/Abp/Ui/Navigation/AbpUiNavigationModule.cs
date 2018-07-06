using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Volo.Abp.UI.Navigation
{
    [DependsOn(typeof(AbpUiModule))]
    public class AbpUiNavigationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAssemblyOf<AbpUiNavigationModule>();
        }
    }
}
