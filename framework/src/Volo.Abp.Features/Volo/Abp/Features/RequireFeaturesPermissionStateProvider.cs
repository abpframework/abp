using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.Features
{
    public class RequireFeaturesPermissionStateProvider : IPermissionStateProvider
    {
        private readonly List<string> _requireFeatures = new List<string>();

        public RequireFeaturesPermissionStateProvider(params string[] requireFeatures)
        {
            Check.NotNullOrEmpty(requireFeatures, nameof(requireFeatures));

            _requireFeatures.AddRange(requireFeatures);
        }

        public async Task<bool> IsEnabledAsync(PermissionStateContext context)
        {
            var feature = context.ServiceProvider.GetRequiredService<IFeatureChecker>();
            return await feature.IsEnabledAsync(true, _requireFeatures.ToArray());
        }
    }
}
