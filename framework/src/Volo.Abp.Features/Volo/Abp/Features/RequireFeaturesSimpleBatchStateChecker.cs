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
    public static RequireFeaturesSimpleBatchStateChecker<TState> Current
    {
        get
        {
            if (_current.Value == null)
            {
                _current.Value = new RequireFeaturesSimpleBatchStateChecker<TState>();
            }

            return _current.Value;
        }
    }
    private static readonly AsyncLocal<RequireFeaturesSimpleBatchStateChecker<TState>?> _current = new();

    private readonly List<RequireFeaturesSimpleBatchStateCheckerModel<TState>> _models;

    private readonly Dictionary<TState, RequireFeaturesSimpleBatchStateCheckerModel<TState>> _modelsByState;

    public RequireFeaturesSimpleBatchStateChecker()
    {
        _models = new List<RequireFeaturesSimpleBatchStateCheckerModel<TState>>();
        _modelsByState = new Dictionary<TState, RequireFeaturesSimpleBatchStateCheckerModel<TState>>();
    }

    public RequireFeaturesSimpleBatchStateChecker<TState> AddCheckModels(
        params RequireFeaturesSimpleBatchStateCheckerModel<TState>[] models)
    {
        Check.NotNullOrEmpty(models, nameof(models));

        _models.AddRange(models);
        foreach (var model in models)
        {
            if (!_modelsByState.ContainsKey(model.State))
            {
                _modelsByState[model.State] = model;
            }
        }
        return this;
    }

    public static IDisposable Use(RequireFeaturesSimpleBatchStateChecker<TState> checker)
    {
        var previousValue = Current;
        _current.Value = checker;
        return new DisposeAction(() => _current.Value = previousValue);
    }

    public virtual RequireFeaturesSimpleBatchStateCheckerModel<TState>? GetModelOrNull(TState state)
    {
        return _modelsByState.TryGetValue(state, out var model) ? model : null;
    }

    public override async Task<SimpleStateCheckerResult<TState>> IsEnabledAsync(
        SimpleBatchStateCheckerContext<TState> context)
    {
        var featureChecker = context.ServiceProvider.GetRequiredService<IFeatureChecker>();

        var result = new SimpleStateCheckerResult<TState>(context.States);

        var stateSet = new HashSet<TState>(context.States);
        var modelLookup = new Dictionary<TState, RequireFeaturesSimpleBatchStateCheckerModel<TState>>();
        var allFeatures = new HashSet<string>();

        foreach (var model in _models)
        {
            if (!stateSet.Contains(model.State))
            {
                continue;
            }

            if (!modelLookup.ContainsKey(model.State))
            {
                modelLookup[model.State] = model;
            }

            foreach (var featureName in model.FeatureNames)
            {
                allFeatures.Add(featureName);
            }
        }

        var featureValues = await featureChecker.IsEnabledAsync(allFeatures.ToArray());

        foreach (var state in context.States)
        {
            if (modelLookup.TryGetValue(state, out var model))
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
