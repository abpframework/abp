using JetBrains.Annotations;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.Authorization.Permissions
{
    public static class PermissionSimpleStateCheckerExtensions
    {
        public static TState RequireAuthenticated<TState>([NotNull] this TState state)
            where TState : IHasSimpleStateCheckers<TState>
        {
            state.StateCheckers.Add(new RequireAuthenticatedSimpleStateChecker<TState>());
            return state;
        }

        public static TState RequirePermissions<TState>(
            [NotNull] this TState state,
            params string[] permissions)
            where TState : IHasSimpleStateCheckers<TState>
        {
            state.RequirePermissions(requiresAll: true, batchCheck: true, permissions);
            return state;
        }

        public static TState RequirePermissions<TState>(
            [NotNull] this TState state,
            bool requiresAll,
            params string[] permissions)
            where TState : IHasSimpleStateCheckers<TState>
        {
            state.RequirePermissions(requiresAll: requiresAll, batchCheck: true, permissions);
            return state;
        }

        public static TState RequirePermissions<TState>(
            [NotNull] this TState state,
            bool requiresAll,
            bool batchCheck,
            params string[] permissions)
            where TState : IHasSimpleStateCheckers<TState>
        {
            Check.NotNull(state, nameof(state));
            Check.NotNullOrEmpty(permissions, nameof(permissions));

            if (batchCheck)
            {
                RequirePermissionsSimpleBatchStateChecker<TState>.Instance.AddCheckModels(new RequirePermissionsSimpleBatchStateCheckerModel<TState>(state, permissions, requiresAll));
                state.StateCheckers.Add(RequirePermissionsSimpleBatchStateChecker<TState>.Instance);
            }
            else
            {
                state.StateCheckers.Add(new RequirePermissionsSimpleStateChecker<TState>(new RequirePermissionsSimpleBatchStateCheckerModel<TState>(state, permissions, requiresAll)));
            }

            return state;
        }
    }
}
