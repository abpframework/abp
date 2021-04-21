using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.Authorization.Permissions
{
    public class RequirePermissionsSimpleStateCheckerModel<TState>
        where TState : IHasSimpleStateCheckers<TState>
    {
        public TState State { get; }

        public string[] Permissions { get; }

        public bool RequiresAll { get; }

        public RequirePermissionsSimpleStateCheckerModel(TState state, string[] permissions, bool requiresAll = true)
        {
            State = state;
            Permissions = permissions;
            RequiresAll = requiresAll;
        }
    }
}
