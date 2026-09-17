using System;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity;

/// <summary>
/// Represents a password history entry for a user.
/// </summary>
public class IdentityUserPasswordHistory : Entity, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// Gets or sets the primary key of the user associated with this password history entry.
    /// </summary>
    public virtual Guid UserId { get; protected set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public virtual string Password { get; protected set; }

    public virtual DateTimeOffset CreatedAt { get; protected set; }

    protected IdentityUserPasswordHistory()
    {

    }

    protected internal IdentityUserPasswordHistory(
        Guid userId,
        [NotNull] string password,
        Guid? tenantId)
    {
        Check.NotNull(password, nameof(password));

        UserId = userId;
        Password = password;
        CreatedAt = DateTimeOffset.UtcNow;
        TenantId = tenantId;
    }

    public override object[] GetKeys()
    {
        return new object[] { UserId, Password };
    }
}
