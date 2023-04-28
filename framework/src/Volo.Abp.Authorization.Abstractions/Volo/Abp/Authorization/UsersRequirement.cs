using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace Volo.Abp.Authorization;

public class UsersRequirement : IAuthorizationRequirement
{
    [CanBeNull]
    public IEnumerable<Guid> UserIds { get; }

    [CanBeNull]
    public IEnumerable<string> UserNames { get; }

    public UsersRequirement([NotNull] IEnumerable<Guid> userIds)
    {
        UserIds = Check.NotNull(userIds, nameof(userIds));
    }

    public UsersRequirement([NotNull] IEnumerable<string> userNames)
    {
        UserNames = Check.NotNull(userNames, nameof(userNames));
    }

    public override string ToString()
    {
        if (UserIds != null)
        {
            return $"UsersRequirement: {string.Join(", ", UserIds)}";
        }

        if (UserNames != null)
        {
            return $"UsersRequirement: {string.Join(", ", UserNames)}";
        }

        return $"UsersRequirement";
    }
}