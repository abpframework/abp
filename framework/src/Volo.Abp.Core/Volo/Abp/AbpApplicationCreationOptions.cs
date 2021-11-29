using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity.PlugIns;

namespace Volo.Abp
{
    public class AbpApplicationCreationOptions
    {
        [NotNull]
        public IServiceCollection Services { get; }

        [NotNull]
        public PlugInSourceList PlugInSources { get; }

        /// <summary>
        /// The options in this property only take effect when IConfiguration not registered.
        /// </summary>
        [NotNull]
        public AbpConfigurationBuilderOptions Configuration {get; }

        public AbpApplicationCreationOptions([NotNull] IServiceCollection services)
        {
            Services = Check.NotNull(services, nameof(services));
            PlugInSources = new PlugInSourceList();
            Configuration = new AbpConfigurationBuilderOptions();
        }
    }
}
