using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.Authorization.Permissions;

public class RequirePermissionsSimpleBatchStateChecker<TState> : SimpleBatchStateCheckerBase<TState>
    where TState : IHasSimpleStateCheckers<TState>
{
    public static RequirePermissionsSimpleBatchStateChecker<TState> Current
    {
        get
        {
            if (_current.Value == null)
            {
                _current.Value = new RequirePermissionsSimpleBatchStateChecker<TState>();
            }

            return _current.Value;
        }
    }
    private static readonly AsyncLocal<RequirePermissionsSimpleBatchStateChecker<TState>?> _current = new AsyncLocal<RequirePermissionsSimpleBatchStateChecker<TState>?>();

    private readonly List<RequirePermissionsSimpleBatchStateCheckerModel<TState>> _models;

    private readonly Dictionary<TState, RequirePermissionsSimpleBatchStateCheckerModel<TState>> _modelsByState;

    public RequirePermissionsSimpleBatchStateChecker()
    {
        _models = new List<RequirePermissionsSimpleBatchStateCheckerModel<TState>>();
        _modelsByState = new Dictionary<TState, RequirePermissionsSimpleBatchStateCheckerModel<TState>>();
    }

    public RequirePermissionsSimpleBatchStateChecker<TState> AddCheckModels(params RequirePermissionsSimpleBatchStateCheckerModel<TState>[] models)
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

    public static IDisposable Use(RequirePermissionsSimpleBatchStateChecker<TState> checker)
    {
        var previousValue = Current;
        _current.Value = checker;
        return new DisposeAction(() => _current.Value = previousValue);
    }

    public virtual RequirePermissionsSimpleBatchStateCheckerModel<TState>? GetModelOrNull(TState state)
    {
        return _modelsByState.TryGetValue(state, out var model) ? model : null;
    }

    public override async Task<SimpleStateCheckerResult<TState>> IsEnabledAsync(SimpleBatchStateCheckerContext<TState> context)
    {
        var permissionChecker = context.ServiceProvider.GetRequiredService<IPermissionChecker>();

        var result = new SimpleStateCheckerResult<TState>(context.States);

        var stateSet = new HashSet<TState>(context.States);
        var modelLookup = new Dictionary<TState, RequirePermissionsSimpleBatchStateCheckerModel<TState>>();
        var allPermissions = new HashSet<string>();

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

            foreach (var permission in model.Permissions)
            {
                allPermissions.Add(permission);
            }
        }

        var grantResult = await permissionChecker.IsGrantedAsync(allPermissions.ToArray());

        foreach (var state in context.States)
        {
            if (modelLookup.TryGetValue(state, out var model))
            {
                if (model.RequiresAll)
                {
                    result[model.State] = model.Permissions.All(x => grantResult.Result.Any(y => y.Key == x && y.Value == PermissionGrantResult.Granted));
                }
                else
                {
                    result[model.State] = grantResult.Result.Any(x => model.Permissions.Contains(x.Key) && x.Value == PermissionGrantResult.Granted);
                }
            }
        }

        return result;
    }
}
