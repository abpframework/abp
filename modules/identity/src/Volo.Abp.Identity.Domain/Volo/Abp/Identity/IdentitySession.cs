using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Identity;

public class IdentitySession : BasicAggregateRoot<Guid>, IHasExtraProperties, IMultiTenant
{
    public virtual string SessionId { get; protected set; }

    /// <summary>
    /// Web, Mobile ...
    /// </summary>
    public virtual string Device { get; protected set; }

    public virtual string DeviceInfo { get; protected set; }

    public virtual Guid? TenantId { get; protected set; }

    public virtual Guid UserId { get; protected set; }

    public virtual string ClientId { get; set; }

    public virtual string IpAddresses { get; protected set; }

    public virtual DateTime SignedIn { get; protected set; }

    public virtual DateTime? LastAccessed { get; protected set; }

    public virtual ExtraPropertyDictionary ExtraProperties { get; protected set; }

    protected IdentitySession()
    {
        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public IdentitySession(
        Guid id,
        string sessionId,
        string device,
        string deviceInfo,
        Guid userId,
        Guid? tenantId,
        string clientId,
        string ipAddresses,
        DateTime signedIn,
        DateTime? lastAccessed = null)
    {
        Id = id;
        SessionId = sessionId;
        Device = device;
        DeviceInfo = deviceInfo;
        UserId = userId;
        TenantId = tenantId;
        ClientId = clientId;
        IpAddresses = ipAddresses;
        SignedIn = signedIn;
        LastAccessed = lastAccessed;

        ExtraProperties = new ExtraPropertyDictionary();
        this.SetDefaultsForExtraProperties();
    }

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        return ExtensibleObjectValidator.GetValidationErrors(
            this,
            validationContext
        );
    }

    public void SetSignedInTime(DateTime signedIn)
    {
        SignedIn = signedIn;
    }

    public void UpdateLastAccessedTime(DateTime? lastAccessed)
    {
        LastAccessed = lastAccessed;
    }

    public void SetIpAddresses(IEnumerable<string> ipAddresses)
    {
        IpAddresses = JoinAsString(ipAddresses);
    }

    public IEnumerable<string> GetIpAddresses()
    {
        return GetArrayFromString(IpAddresses);
    }

    private static string JoinAsString(IEnumerable<string> list)
    {
        var serialized = string.Join(",", list);
        if (serialized.IsNullOrWhiteSpace())
        {
            return null;
        }

        while (serialized.Length > IdentitySessionConsts.MaxIpAddressesLength)
        {
            var lastCommaIndex = serialized.IndexOf(',');
            if (lastCommaIndex < 0)
            {
                return serialized;
            }
            serialized = serialized.Substring(lastCommaIndex + 1);
        }

        return serialized;
    }

    private string[] GetArrayFromString(string str)
    {
        return str?.Split(",", StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
    }
}
