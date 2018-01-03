using System;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiResourceClaim : UserClaim
    {
        public virtual Guid ApiResourceId { get; set; }

        protected ApiResourceClaim()
        {
            
        }

        public ApiResourceClaim(Guid id)
            : base(id)
        {
            
        }
    }
}