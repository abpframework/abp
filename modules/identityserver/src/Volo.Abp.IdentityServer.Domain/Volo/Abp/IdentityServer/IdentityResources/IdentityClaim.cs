using System;
using JetBrains.Annotations;

namespace Volo.Abp.IdentityServer.IdentityResources
{
    public class IdentityClaim : UserClaim
    {
        public virtual Guid IdentityResourceId { get; set; }

        protected IdentityClaim()
        {
            
        }

        protected internal IdentityClaim(Guid identityResourceId, [NotNull] string type)
            : base(type)
        {
            IdentityResourceId = identityResourceId;
        }

        public override object[] GetKeys()
        {
            return new object[] { IdentityResourceId, Type };
        }
    }
}