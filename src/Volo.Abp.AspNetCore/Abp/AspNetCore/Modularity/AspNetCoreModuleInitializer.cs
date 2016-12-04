using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.Builder;
using Volo.Abp.Modularity;

namespace Volo.Abp.AspNetCore.Modularity
{
    public class AspNetCoreModuleInitializer : IModuleInitializer
    {
        private readonly AspNetConfigurationContext _configurationContext;

        public AspNetCoreModuleInitializer(ApplicationBuilderAccessor appAccessor)
        {
            _configurationContext = new AspNetConfigurationContext(appAccessor.App);
        }

        public void Initialize(IAbpModule module)
        {
            (module as IConfigureAspNet)?.Configure(_configurationContext);
        }
    }
}
