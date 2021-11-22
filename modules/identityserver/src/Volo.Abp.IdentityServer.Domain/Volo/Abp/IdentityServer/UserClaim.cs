using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer;

public abstract class UserClaim : Entity
{
    public virtual string Type { get; protected set; }

    protected UserClaim()
    {

    }

    protected UserClaim([NotNull] string type)
    {
        Check.NotNull(type, nameof(type));

        Type = type;
    }
}
