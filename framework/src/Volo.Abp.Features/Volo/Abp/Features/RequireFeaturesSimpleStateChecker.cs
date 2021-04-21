using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.Features
{
    public class RequireFeaturesSimpleStateChecker<TState> : ISimpleSingleStateChecker<TState>
        where TState : IHasSimpleStateCheckers<TState>
    {
        private readonly string[] _featureNames;
        private readonly bool _requiresAll;

        public RequireFeaturesSimpleStateChecker(params string[] featureNames)
            : this(true, featureNames)
        {
        }

        public RequireFeaturesSimpleStateChecker(bool requiresAll, params string[] featureNames)
        {
            Check.NotNullOrEmpty(featureNames, nameof(featureNames));

            _requiresAll = requiresAll;
            _featureNames = featureNames;
        }

        public async Task<bool> IsEnabledAsync(SimpleSingleStateCheckerContext<TState> context)
        {
            var feature = context.ServiceProvider.GetRequiredService<IFeatureChecker>();
            return await feature.IsEnabledAsync(_requiresAll, _featureNames);
        }
    }
}
