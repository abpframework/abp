using System;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiScopeClaim : UserClaim
    {
        public virtual Guid ApiScopeId { get; set; }

        protected ApiScopeClaim()
        {

        }

        protected ApiScopeClaim(Guid id)
            : base(id)
        {
        }
    }
}