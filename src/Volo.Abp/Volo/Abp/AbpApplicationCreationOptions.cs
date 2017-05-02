using JetBrains.Annotations;
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

        public AbpApplicationCreationOptions([NotNull] IServiceCollection services)
        {
            Services = Check.NotNull(services, nameof(services));
            PlugInSources = new PlugInSourceList();
        }
    }
}