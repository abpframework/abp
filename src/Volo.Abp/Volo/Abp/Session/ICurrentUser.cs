using System;
using System.Security.Claims;

namespace Volo.Abp.Session
{
    public interface ICurrentUser
    {
        bool IsAuthenticated { get; }

        Guid? Id { get; }

        string UserName { get; }

        string Email { get; }

        Claim FindClaim(string claimType);

        string FindClaimValue(string claimType);

        T FindClaimValue<T>(string claimType)
            where T : struct;
    }
}
