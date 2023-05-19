using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity;

/// <summary>
/// Represents a login and its associated provider for a user.
/// </summary>
public class IdentityUserLogin : Entity, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// Gets or sets the of the primary key of the user associated with this login.
    /// </summary>
    public virtual Guid UserId { get; protected set; }

    /// <summary>
    /// Gets or sets the login provider for the login (e.g. facebook, google)
    /// </summary>
    public virtual string LoginProvider { get; protected set; }

    /// <summary>
    /// Gets or sets the unique provider identifier for this login.
    /// </summary>
    public virtual string ProviderKey { get; protected set; }

    /// <summary>
    /// Gets or sets the friendly name used in a UI for this login.
    /// </summary>
    public virtual string ProviderDisplayName { get; protected set; }

    protected IdentityUserLogin()
    {

    }

    protected internal IdentityUserLogin(
        Guid userId,
        [NotNull] string loginProvider,
        [NotNull] string providerKey,
        string providerDisplayName,
        Guid? tenantId)
    {
        Check.NotNull(loginProvider, nameof(loginProvider));
        Check.NotNull(providerKey, nameof(providerKey));

        UserId = userId;
        LoginProvider = loginProvider;
        ProviderKey = providerKey;
        ProviderDisplayName = providerDisplayName;
        TenantId = tenantId;
    }

    protected internal IdentityUserLogin(
        Guid userId,
        [NotNull] UserLoginInfo login,
        Guid? tenantId)
        : this(
              userId,
              login.LoginProvider,
              login.ProviderKey,
              login.ProviderDisplayName,
              tenantId)
    {
    }

    public virtual UserLoginInfo ToUserLoginInfo()
    {
        return new UserLoginInfo(LoginProvider, ProviderKey, ProviderDisplayName);
    }

    public override object[] GetKeys()
    {
        return new object[] { UserId, LoginProvider };
    }
}
