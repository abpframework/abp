using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public class ApplicationConfigurationCache : ISingletonDependency
    {
        protected ApplicationConfigurationDto Configuration { get; set; }

        public virtual ApplicationConfigurationDto Get()
        {
            return Configuration;
        }

        public void Set(ApplicationConfigurationDto configuration)
        {
            Configuration = configuration;
        }
    }
}
