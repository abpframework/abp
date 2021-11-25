using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.Authorization.Permissions;

public class RequirePermissionsSimpleBatchStateCheckerModel<TState>
    where TState : IHasSimpleStateCheckers<TState>
{
    public TState State { get; }

    public string[] Permissions { get; }

    public bool RequiresAll { get; }

    public RequirePermissionsSimpleBatchStateCheckerModel(TState state, string[] permissions, bool requiresAll = true)
    {
        Check.NotNull(state, nameof(state));
        Check.NotNullOrEmpty(permissions, nameof(permissions));

        State = state;
        Permissions = permissions;
        RequiresAll = requiresAll;
    }
}
