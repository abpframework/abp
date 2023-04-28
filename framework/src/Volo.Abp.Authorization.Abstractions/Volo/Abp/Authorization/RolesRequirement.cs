using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace Volo.Abp.Authorization;

public class RolesRequirement : IAuthorizationRequirement
{
    public string[] NormalizedRoleNames { get; }

    public bool RequiresAll { get; }

    public RolesRequirement([NotNull] string[] normalizedRoleNames, bool requiresAll)
    {
        Check.NotNull(normalizedRoleNames, nameof(normalizedRoleNames));

        NormalizedRoleNames = normalizedRoleNames;
        RequiresAll = requiresAll;
    }

    public override string ToString()
    {
        return $"RolesRequirement: {string.Join(", ", NormalizedRoleNames)}";
    }
}