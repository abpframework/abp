using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace Volo.Abp.Authorization;

public class UserRequirement : IAuthorizationRequirement
{
    public Guid? UserId { get; }

    [CanBeNull]
    public string UserName { get; }

    public UserRequirement(Guid? userId)
    {
        UserId = userId;
    }

    public UserRequirement([NotNull] string userName)
    {
        UserName = Check.NotNull(userName, nameof(userName));
    }

    public override string ToString()
    {
        return UserId != null ? $"UserRequirement: {UserId}" : $"UserRequirement: {UserName}";
    }
}