using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Users
{
    public static class CurrentUserExtensions
    {
        [CanBeNull]
        public static string FindClaimValue(this ICurrentUser currentUser, string claimType)
        {
            return currentUser.FindClaim(claimType)?.Value;
        }

        public static T FindClaimValue<T>(this ICurrentUser currentUser, string claimType)
            where T : struct
        {
            var value = currentUser.FindClaimValue(claimType);
            if (value == null)
            {
                return default;
            }

            return value.To<T>();
        }

        public static Guid GetId(this ICurrentUser currentUser)
        {
            Debug.Assert(currentUser.Id != null, "currentUser.Id != null");

            return currentUser.Id.Value;
        }

        public static Guid? FindImpersonatorTenantId([NotNull] this ICurrentUser currentUser)
        {
            var impersonatorTenantId = currentUser.FindClaimValue(AbpClaimTypes.ImpersonatorTenantId);
            if (impersonatorTenantId == null || impersonatorTenantId.IsNullOrWhiteSpace())
            {
                return null;
            }
            if (Guid.TryParse(impersonatorTenantId, out var guid))
            {
                return guid;
            }

            return null;
        }

        public static Guid? FindImpersonatorUserId([NotNull] this ICurrentUser currentUser)
        {
            var impersonatorUserId = currentUser.FindClaimValue(AbpClaimTypes.ImpersonatorUserId);
            if (impersonatorUserId == null || impersonatorUserId.IsNullOrWhiteSpace())
            {
                return null;
            }
            if (Guid.TryParse(impersonatorUserId, out var guid))
            {
                return guid;
            }

            return null;
        }
    }
}
