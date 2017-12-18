using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace AbpDesk.SamplePlugInModule
{
    /* This is just a sample plugin module to test loading plugin modules.
     */

    [DependsOn(typeof(AbpCommonModule))]
    public class AbpDeskSamplePlugInModule : AbpModule
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            
        }
    }
}
