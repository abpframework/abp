using System;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.Authorization.Permissions
{
    public static class PermissionSimpleStateCheckerExtensions
    {
        public static TState RequirePermissions<TState>(
            [NotNull] this TState state,
            params string[] permissions)
            where TState : IHasSimpleStateCheckers<TState>
        {
            state.RequirePermissions(true, permissions);
            return state;
        }

        public static TState RequirePermissions<TState>(
            [NotNull] this TState state,
            bool requiresAll,
            params string[] permissions)
            where TState : IHasSimpleStateCheckers<TState>
        {
            Check.NotNull(state, nameof(state));
            Check.NotNullOrEmpty(permissions, nameof(permissions));

            lock (state)
            {
                RequirePermissionsSimpleMultipleStateChecker<TState>.Instance.AddCheckModels(new RequirePermissionsSimpleStateCheckerModel<TState>(state, permissions, requiresAll));
                state.SimpleStateCheckers.Add(RequirePermissionsSimpleMultipleStateChecker<TState>.Instance);
            }

            return state;
        }
    }
}
