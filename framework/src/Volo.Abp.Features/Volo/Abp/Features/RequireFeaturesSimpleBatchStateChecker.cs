using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.Features;

public class RequireFeaturesSimpleBatchStateChecker<TState> : SimpleBatchStateCheckerBase<TState>
    where TState : IHasSimpleStateCheckers<TState>
{
    public static RequireFeaturesSimpleBatchStateChecker<TState> Current => _current.Value!;
    private static readonly AsyncLocal<RequireFeaturesSimpleBatchStateChecker<TState>> _current = new();

    private readonly List<RequireFeaturesSimpleBatchStateCheckerModel<TState>> _models;

    static RequireFeaturesSimpleBatchStateChecker()
    {
        _current.Value = new RequireFeaturesSimpleBatchStateChecker<TState>();
    }

    public RequireFeaturesSimpleBatchStateChecker()
    {
        _models = new List<RequireFeaturesSimpleBatchStateCheckerModel<TState>>();
    }

    public RequireFeaturesSimpleBatchStateChecker<TState> AddCheckModels(
        params RequireFeaturesSimpleBatchStateCheckerModel<TState>[] models)
    {
        Check.NotNullOrEmpty(models, nameof(models));

        _models.AddRange(models);
        return this;
    }

    public static IDisposable Use(RequireFeaturesSimpleBatchStateChecker<TState> checker)
    {
        var previousValue = Current;
        _current.Value = checker;
        return new DisposeAction(() => _current.Value = previousValue);
    }

    public override async Task<SimpleStateCheckerResult<TState>> IsEnabledAsync(
        SimpleBatchStateCheckerContext<TState> context)
    {
        var featureChecker = context.ServiceProvider.GetRequiredService<IFeatureChecker>();

        var result = new SimpleStateCheckerResult<TState>(context.States);

        var relevantModels = _models
            .Where(x => context.States.Any(s => s.Equals(x.State)))
            .ToList();

        var features = relevantModels.SelectMany(x => x.FeatureNames).Distinct().ToArray();
        var featureValues = await featureChecker.IsEnabledAsync(features);

        foreach (var state in context.States)
        {
            var model = relevantModels.FirstOrDefault(x => x.State.Equals(state));
            if (model != null)
            {
                if (model.RequiresAll)
                {
                    result[state] = model.FeatureNames.All(x => featureValues.TryGetValue(x, out var v) && v);
                }
                else
                {
                    result[state] = model.FeatureNames.Any(x => featureValues.TryGetValue(x, out var v) && v);
                }
            }
        }

        return result;
    }
}
