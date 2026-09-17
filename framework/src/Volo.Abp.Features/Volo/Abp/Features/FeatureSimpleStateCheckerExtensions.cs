using JetBrains.Annotations;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.Features;

public static class FeatureSimpleStateCheckerExtensions
{
    public static TState RequireFeatures<TState>(
        [NotNull] this TState state,
        params string[] features)
        where TState : IHasSimpleStateCheckers<TState>
    {
        state.RequireFeatures(requiresAll: true, batchCheck: true, features);
        return state;
    }

    public static TState RequireFeatures<TState>(
        [NotNull] this TState state,
        bool requiresAll,
        params string[] features)
        where TState : IHasSimpleStateCheckers<TState>
    {
        state.RequireFeatures(requiresAll: requiresAll, batchCheck: true, features);
        return state;
    }

    public static TState RequireFeatures<TState>(
        [NotNull] this TState state,
        bool requiresAll,
        bool batchCheck,
        params string[] features)
        where TState : IHasSimpleStateCheckers<TState>
    {
        Check.NotNull(state, nameof(state));
        Check.NotNullOrEmpty(features, nameof(features));

        if (batchCheck)
        {
            RequireFeaturesSimpleBatchStateChecker<TState>.Current.AddCheckModels(
                new RequireFeaturesSimpleBatchStateCheckerModel<TState>(state, features, requiresAll));
            state.StateCheckers.Add(RequireFeaturesSimpleBatchStateChecker<TState>.Current);
        }
        else
        {
            state.StateCheckers.Add(new RequireFeaturesSimpleStateChecker<TState>(requiresAll, features));
        }

        return state;
    }
}
