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

        internal void Set(ApplicationConfigurationDto configuration)
        {
            Configuration = configuration;
        }
    }
}
