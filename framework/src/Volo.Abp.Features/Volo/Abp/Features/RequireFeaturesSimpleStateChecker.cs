using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.Features;

public class RequireFeaturesSimpleStateChecker<TState> : ISimpleStateChecker<TState>
    where TState : IHasSimpleStateCheckers<TState>
{
    public string[] FeatureNames { get; }
    public bool RequiresAll { get; }

    public RequireFeaturesSimpleStateChecker(params string[] featureNames)
        : this(true, featureNames)
    {
    }

    public RequireFeaturesSimpleStateChecker(bool requiresAll, params string[] featureNames)
    {
        Check.NotNullOrEmpty(featureNames, nameof(featureNames));

        RequiresAll = requiresAll;
        FeatureNames = featureNames;
    }

    public async Task<bool> IsEnabledAsync(SimpleStateCheckerContext<TState> context)
    {
        var featureChecker = context.ServiceProvider.GetRequiredService<IFeatureChecker>();
        return await featureChecker.IsEnabledAsync(RequiresAll, FeatureNames);
    }
}
