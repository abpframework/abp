using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.Features;

public class RequireFeaturesSimpleBatchStateCheckerModel<TState>
    where TState : IHasSimpleStateCheckers<TState>
{
    public TState State { get; }

    public string[] FeatureNames { get; }

    public bool RequiresAll { get; }

    public RequireFeaturesSimpleBatchStateCheckerModel(TState state, string[] featureNames, bool requiresAll = true)
    {
        Check.NotNull(state, nameof(state));
        Check.NotNullOrEmpty(featureNames, nameof(featureNames));

        State = state;
        FeatureNames = featureNames;
        RequiresAll = requiresAll;
    }
}
