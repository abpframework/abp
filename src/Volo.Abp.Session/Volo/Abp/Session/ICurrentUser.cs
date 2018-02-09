using System;
using System.Security.Claims;
using JetBrains.Annotations;

namespace Volo.Abp.Session
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }

        [CanBeNull]
        Guid? Id { get; }

        [CanBeNull]
        string UserName { get; }

        [CanBeNull]
        string Email { get; }

        [CanBeNull]
        Claim FindClaim(string claimType);
    }
}
