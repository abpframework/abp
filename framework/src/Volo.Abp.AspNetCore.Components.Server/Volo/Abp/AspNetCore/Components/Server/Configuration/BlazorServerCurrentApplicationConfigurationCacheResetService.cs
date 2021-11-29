using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Configuration;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Local;

namespace Volo.Abp.AspNetCore.Components.Server.Configuration
{
    [Dependency(ReplaceServices = true)]
    public class BlazorServerCurrentApplicationConfigurationCacheResetService : 
        ICurrentApplicationConfigurationCacheResetService, 
        ITransientDependency
    {
        private readonly ILocalEventBus _localEventBus;

        public BlazorServerCurrentApplicationConfigurationCacheResetService(
            ILocalEventBus localEventBus)
        {
            _localEventBus = localEventBus;
        }

        public async Task ResetAsync()
        {
            await _localEventBus.PublishAsync(
                new CurrentApplicationConfigurationCacheResetEventData()
            );
        }
    }
}