using System;
using System.Security.Claims;
using JetBrains.Annotations;

namespace Volo.Abp.Users;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }

    [CanBeNull]
    Guid? Id { get; }

    string? UserName { get; }

    string? Name { get; }

    string? SurName { get; }

    string? PhoneNumber { get; }

    bool PhoneNumberVerified { get; }

    string? Email { get; }

    bool EmailVerified { get; }

    Guid? TenantId { get; }

    [NotNull]
    string[] Roles { get; }

    Claim? FindClaim(string claimType);

    [NotNull]
    Claim[] FindClaims(string claimType);

    [NotNull]
    Claim[] GetAllClaims();

    bool IsInRole(string roleName);
}
