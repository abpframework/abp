using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.GlobalFeatures
{
    public class RequireGlobalFeaturesPermissionStateProvider : IPermissionStateProvider
    {
        private readonly List<string> _requireGlobalFeatures = new List<string>();

        public RequireGlobalFeaturesPermissionStateProvider(params string[] requireGlobalFeatures)
        {
            Check.NotNullOrEmpty(requireGlobalFeatures, nameof(requireGlobalFeatures));

            _requireGlobalFeatures.AddRange(requireGlobalFeatures);
        }

        public Task<bool> IsEnabledAsync(PermissionStateContext context)
        {
            return Task.FromResult(_requireGlobalFeatures.All(x => GlobalFeatureManager.Instance.IsEnabled(x)));
        }
    }
}
