using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity;

/// <summary>
/// Represents a passkey credential for a user in the identity system.
/// </summary>
/// <remarks>
/// See <see href="https://www.w3.org/TR/webauthn-3/#credential-record"/>.
/// </remarks>
public class IdentityUserPasskey : Entity, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// Gets or sets the primary key of the user that owns this passkey.
    /// </summary>
    public virtual Guid UserId { get; protected set; }

    /// <summary>
    /// Gets or sets the credential ID for this passkey.
    /// </summary>
    public virtual byte[] CredentialId { get; set; }

    /// <summary>
    /// Gets or sets additional data associated with this passkey.
    /// </summary>
    public virtual IdentityPasskeyData Data { get; set; }

    protected IdentityUserPasskey()
    {

    }

    public IdentityUserPasskey(
        Guid userId,
        byte[] credentialId,
        IdentityPasskeyData data,
        Guid? tenantId)
    {
        UserId = userId;
        CredentialId = credentialId;
        Data = data;
        TenantId = tenantId;
    }

    public override object[] GetKeys()
    {
        return new object[] { UserId, CredentialId };
    }
}
