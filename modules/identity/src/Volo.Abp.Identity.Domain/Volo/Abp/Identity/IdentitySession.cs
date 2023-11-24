using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Identity;

public class IdentitySession : BasicAggregateRoot<Guid>
{
    /// <summary>
    /// Web, CLI, STUDIO, ...
    /// </summary>
    public virtual string Device { get; protected set; }

    public virtual Guid? TenantId { get; protected set; }

    public virtual Guid UserId { get; protected set; }

    public virtual string ClientId { get; set; }

    public virtual string IpAddresses { get; protected set; }

    public virtual DateTime SignedIn { get; protected set; }

    public virtual DateTime? LastAccessed { get; protected set; }

    protected IdentitySession()
    {

    }

    public IdentitySession(
        Guid id,
        string device,
        Guid userId,
        Guid? tenantId,
        string clientId,
        string ipAddresses,
        DateTime signedIn,
        DateTime? lastAccessed)
    {
        Id = id;
        Device = device;
        UserId = userId;
        TenantId = tenantId;
        ClientId = clientId;
        IpAddresses = ipAddresses;
        SignedIn = signedIn;
        LastAccessed = lastAccessed;
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
        return serialized.IsNullOrWhiteSpace() ? null : serialized;
    }

    private string[] GetArrayFromString(string str)
    {
        return str?.Split(",", StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
    }
}
