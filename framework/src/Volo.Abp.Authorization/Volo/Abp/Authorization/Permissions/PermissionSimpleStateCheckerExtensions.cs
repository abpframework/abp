using JetBrains.Annotations;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.Authorization.Permissions
{
    public static class PermissionSimpleStateCheckerExtensions
    {
        public static TState RequireAuthenticated<TState>([NotNull] this TState state)
            where TState : IHasSimpleStateCheckers<TState>
        {
            state.SimpleStateCheckers.Add(new RequireAuthenticatedSimpleStateChecker<TState>());
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
            bool batchCheck = true,
            params string[] permissions)
            where TState : IHasSimpleStateCheckers<TState>
        {
            Check.NotNull(state, nameof(state));
            Check.NotNullOrEmpty(permissions, nameof(permissions));

            if (batchCheck)
            {
                lock (state)
                {
                    RequirePermissionsSimpleBatchStateChecker<TState>.Instance.AddCheckModels(new RequirePermissionsSimpleBatchStateCheckerModel<TState>(state, permissions, requiresAll));
                    state.SimpleStateCheckers.Add(RequirePermissionsSimpleBatchStateChecker<TState>.Instance);
                }
            }
            else
            {
                state.SimpleStateCheckers.Add(new RequirePermissionsSimpleStateChecker<TState>(new RequirePermissionsSimpleBatchStateCheckerModel<TState>(state, permissions, requiresAll)));
            }

            return state;
        }
    }
}
