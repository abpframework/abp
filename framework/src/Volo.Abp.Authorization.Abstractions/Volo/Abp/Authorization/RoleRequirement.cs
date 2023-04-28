using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace Volo.Abp.Authorization;

public class RoleRequirement : IAuthorizationRequirement
{
    public string NormalizedRoleName { get; }

    public RoleRequirement([NotNull] string normalizedRoleName)
    {
        Check.NotNull(normalizedRoleName, nameof(normalizedRoleName));

        NormalizedRoleName = normalizedRoleName;
    }

    public override string ToString()
    {
        return $"RoleRequirement: {NormalizedRoleName}";
    }
}