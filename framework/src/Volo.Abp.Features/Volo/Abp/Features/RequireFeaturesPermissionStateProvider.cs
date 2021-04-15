using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Authorization.Permissions;

namespace Volo.Abp.Features
{
    public class RequireFeaturesPermissionStateProvider : IPermissionStateProvider
    {
        private readonly string[] _featureNames;
        private readonly bool _requiresAll;

        public RequireFeaturesPermissionStateProvider(params string[] featureNames)
            : this(true, featureNames)
        {
            
        }

        public RequireFeaturesPermissionStateProvider(bool requiresAll, params string[] featureNames)
        {
            Check.NotNullOrEmpty(featureNames, nameof(featureNames));

            _requiresAll = requiresAll;
            _featureNames = featureNames;
        }

        public async Task<bool> IsEnabledAsync(PermissionStateContext context)
        {
            var feature = context.ServiceProvider.GetRequiredService<IFeatureChecker>();
            return await feature.IsEnabledAsync(_requiresAll, _featureNames);
        }
    }
}
