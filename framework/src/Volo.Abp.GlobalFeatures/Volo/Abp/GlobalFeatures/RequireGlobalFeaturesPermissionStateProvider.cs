using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.GlobalFeatures
{
    public class RequireGlobalFeaturesPermissionStateProvider : IPermissionStateProvider
    {
        private readonly string[] _globalFeatureNames;
        private readonly bool _requiresAll;

        public RequireGlobalFeaturesPermissionStateProvider(params string[] globalFeatureNames)
            : this(true, globalFeatureNames)
        {
        }

        public RequireGlobalFeaturesPermissionStateProvider(bool requiresAll, params string[] globalFeatureNames)
        {
            Check.NotNullOrEmpty(globalFeatureNames, nameof(globalFeatureNames));

            _requiresAll = requiresAll;
            _globalFeatureNames = globalFeatureNames;
        }

        public RequireGlobalFeaturesPermissionStateProvider(bool requiresAll, params Type[] globalFeatureNames)
        {
            Check.NotNullOrEmpty(globalFeatureNames, nameof(globalFeatureNames));

            _requiresAll = requiresAll;
            _globalFeatureNames = globalFeatureNames.Select(GlobalFeatureNameAttribute.GetName).ToArray();
        }

        public Task<bool> IsEnabledAsync(PermissionStateContext context)
        {
            bool isEnabled = _requiresAll
                ? _globalFeatureNames.All(x => GlobalFeatureManager.Instance.IsEnabled(x))
                : _globalFeatureNames.Any(x => GlobalFeatureManager.Instance.IsEnabled(x));

            return Task.FromResult(isEnabled);
        }
    }
}
