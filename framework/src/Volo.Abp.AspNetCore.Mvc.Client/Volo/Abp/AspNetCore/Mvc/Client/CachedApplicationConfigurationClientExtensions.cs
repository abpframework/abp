using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.Threading;

namespace Volo.Abp.AspNetCore.Mvc.Client
{
    public static class CachedApplicationConfigurationClientExtensions
    {
        public static ApplicationConfigurationDto Get(this ICachedApplicationConfigurationClient client)
        {
            return AsyncHelper.RunSync(client.GetAsync);
        }
    }
}