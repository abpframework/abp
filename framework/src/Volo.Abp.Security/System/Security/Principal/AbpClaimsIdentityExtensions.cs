using System.Linq;
using System.Security.Claims;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Security.Claims;

namespace System.Security.Principal
{
    public static class AbpClaimsIdentityExtensions
    {
        public static Guid? FindUserId([NotNull] this ClaimsPrincipal principal)
        {
            Check.NotNull(principal, nameof(principal));

            var userIdOrNull = principal.Claims?.FirstOrDefault(c => c.Type == AbpClaimTypes.UserId);
            if (userIdOrNull == null || userIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }
            if (Guid.TryParse(userIdOrNull.Value, out Guid result))
            {
                return result;
            }
            return null;
        }

        public static Guid? FindUserId([NotNull] this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;

            var userIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == AbpClaimTypes.UserId);
            if (userIdOrNull == null || userIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Guid.Parse(userIdOrNull.Value);
        }

        public static Guid? FindImpersonatorUserId([NotNull] this ClaimsPrincipal principal)
        {
            Check.NotNull(principal, nameof(principal));

            var impersonatorUserIdOrNull = principal.Claims?.FirstOrDefault(c => c.Type == AbpClaimTypes.ImpersonatorUserId);
            if (impersonatorUserIdOrNull == null || impersonatorUserIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }
            if (Guid.TryParse(impersonatorUserIdOrNull.Value, out Guid result))
            {
                return result;
            }
            return null;
        }

        public static Guid? FindImpersonatorUserId([NotNull] this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;

            var impersonatorUserIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == AbpClaimTypes.ImpersonatorUserId);
            if (impersonatorUserIdOrNull == null || impersonatorUserIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Guid.Parse(impersonatorUserIdOrNull.Value);
        }

        public static Guid? FindTenantId([NotNull] this ClaimsPrincipal principal)
        {
            Check.NotNull(principal, nameof(principal));

            var tenantIdOrNull = principal.Claims?.FirstOrDefault(c => c.Type == AbpClaimTypes.TenantId);
            if (tenantIdOrNull == null || tenantIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Guid.Parse(tenantIdOrNull.Value);
        }

        public static Guid? FindTenantId([NotNull] this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;

            var tenantIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == AbpClaimTypes.TenantId);
            if (tenantIdOrNull == null || tenantIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Guid.Parse(tenantIdOrNull.Value);
        }

        public static Guid? FindImpersonatorTenantId([NotNull] this ClaimsPrincipal principal)
        {
            Check.NotNull(principal, nameof(principal));

            var impersonatorTenantIdOrNull = principal.Claims?.FirstOrDefault(c => c.Type == AbpClaimTypes.ImpersonatorTenantId);
            if (impersonatorTenantIdOrNull == null || impersonatorTenantIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Guid.Parse(impersonatorTenantIdOrNull.Value);
        }

        public static Guid? FindImpersonatorTenantId([NotNull] this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;

            var impersonatorTenantIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == AbpClaimTypes.ImpersonatorTenantId);
            if (impersonatorTenantIdOrNull == null || impersonatorTenantIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Guid.Parse(impersonatorTenantIdOrNull.Value);
        }

        public static string FindClientId([NotNull] this ClaimsPrincipal principal)
        {
            Check.NotNull(principal, nameof(principal));

            var clientIdOrNull = principal.Claims?.FirstOrDefault(c => c.Type == AbpClaimTypes.ClientId);
            if (clientIdOrNull == null || clientIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return clientIdOrNull.Value;
        }

        public static string FindClientId([NotNull] this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;

            var clientIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == AbpClaimTypes.ClientId);
            if (clientIdOrNull == null || clientIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return clientIdOrNull.Value;
        }

        public static Guid? FindEditionId([NotNull] this ClaimsPrincipal principal)
        {
            Check.NotNull(principal, nameof(principal));

            var editionIdOrNull = principal.Claims?.FirstOrDefault(c => c.Type == AbpClaimTypes.EditionId);
            if (editionIdOrNull == null || editionIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Guid.Parse(editionIdOrNull.Value);
        }

        public static Guid? FindEditionId([NotNull] this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;

            var editionIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == AbpClaimTypes.EditionId);
            if (editionIdOrNull == null || editionIdOrNull.Value.IsNullOrWhiteSpace())
            {
                return null;
            }

            return Guid.Parse(editionIdOrNull.Value);
        }
    }
}
