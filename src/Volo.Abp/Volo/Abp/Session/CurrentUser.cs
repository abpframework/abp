using System;
using System.Linq;
using System.Security.Claims;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Volo.Abp.Session
{
    public class CurrentUser : ICurrentUser, ITransientDependency //TODO: Singleton?
    {
        public virtual bool IsAuthenticated => Id.HasValue;

        public virtual Guid? Id
        {
            get
            {
                var value = FindClaimValue(AbpClaimTypes.UserId);
                if (value == null)
                {
                    return null;
                }

                return Guid.Parse(value);
            }
        }

        public virtual string UserName => FindClaimValue(AbpClaimTypes.UserName);

        public virtual string Email => FindClaimValue(AbpClaimTypes.Email);

        private readonly ICurrentPrincipalAccessor _principalAccessor;

        public CurrentUser(ICurrentPrincipalAccessor principalAccessor)
        {
            _principalAccessor = principalAccessor;
        }

        public virtual Claim FindClaim(string claimType)
        {
            return _principalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == claimType);
        }

        public virtual T FindClaimValue<T>(string claimType)
            where T : struct
        {
            var value = FindClaimValue(claimType);
            if (value == null)
            {
                return default(T);
            }

            return value.To<T>();
        }

        public virtual string FindClaimValue(string claimType)
        {
            return FindClaim(claimType)?.Value;
        }
    }
}