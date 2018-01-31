using System;
using JetBrains.Annotations;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiResourceClaim : UserClaim
    {
        public virtual Guid ApiResourceId { get; set; }

        protected ApiResourceClaim()
        {
            
        }

        protected internal ApiResourceClaim(Guid apiResourceId, [NotNull] string type)
            : base(type)
        {
            ApiResourceId = apiResourceId;
        }
    }
}