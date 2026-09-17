using System;
using JetBrains.Annotations;
using Volo.Abp.Data;

namespace Volo.Abp.Users;

public class RoleData : IRoleData
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public string Name { get; set; }

    public bool IsDefault { get; set; }

    public bool IsStatic { get; set; }

    public bool IsPublic { get; set; }

    public ExtraPropertyDictionary ExtraProperties { get; }

    public RoleData()
    {

    }

    public RoleData(IRoleData roleData)
    {
        Id = roleData.Id;
        Name = roleData.Name;
        IsDefault = roleData.IsDefault;
        IsStatic = roleData.IsStatic;
        IsPublic = roleData.IsPublic;
        TenantId = roleData.TenantId;
        ExtraProperties = roleData.ExtraProperties;
    }

    public RoleData(
        Guid id,
        [NotNull] string name,
        bool isDefault = false,
        bool isStatic = false,
        bool isPublic = false,
        Guid? tenantId = null,
        ExtraPropertyDictionary extraProperties = null)
    {
        Id = id;
        Name = name;
        IsDefault = isDefault;
        IsStatic = isStatic;
        IsPublic = isPublic;
        TenantId = tenantId;
        ExtraProperties = extraProperties;
    }
}
