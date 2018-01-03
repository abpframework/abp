using System;

namespace Volo.Abp.IdentityServer.IdentityResources
{
    public class IdentityClaim : UserClaim
    {
        public virtual Guid IdentityResourceId { get; set; }

        protected IdentityClaim()
        {
            
        }

        public IdentityClaim(Guid id)
            : base(id)
        {
            
        }
    }
}