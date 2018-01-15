using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer
{
    //TODO: Eleminate UserClaim class for simplicity.

    public abstract class UserClaim : Entity<Guid>
    {
        public virtual string Type { get; set; }

        protected UserClaim()
        {

        }

        protected UserClaim(Guid id)
        {
            Id = id;
        }
    }
}