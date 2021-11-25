using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity;

/// <summary>
/// Represents an organization unit (OU).
/// </summary>
public class OrganizationUnit : FullAuditedAggregateRoot<Guid>, IMultiTenant
{
    public virtual Guid? TenantId { get; protected set; }

    /// <summary>
    /// Parent <see cref="OrganizationUnit"/> Id.
    /// Null, if this OU is a root.
    /// </summary>
    public virtual Guid? ParentId { get; internal set; }

    /// <summary>
    /// Hierarchical Code of this organization unit.
    /// Example: "00001.00042.00005".
    /// This is a unique code for an OrganizationUnit.
    /// It's changeable if OU hierarchy is changed.
    /// </summary>
    public virtual string Code { get; internal set; }

    /// <summary>
    /// Display name of this OrganizationUnit.
    /// </summary>
    public virtual string DisplayName { get; set; }

    /// <summary>
    /// Roles of this OU.
    /// </summary>
    public virtual ICollection<OrganizationUnitRole> Roles { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrganizationUnit"/> class.
    /// </summary>
    public OrganizationUnit()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrganizationUnit"/> class.
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="displayName">Display name.</param>
    /// <param name="parentId">Parent's Id or null if OU is a root.</param>
    /// <param name="tenantId">Tenant's Id or null for host.</param>
    public OrganizationUnit(Guid id, string displayName, Guid? parentId = null, Guid? tenantId = null)
        : base(id)
    {
        TenantId = tenantId;
        DisplayName = displayName;
        ParentId = parentId;
        Roles = new Collection<OrganizationUnitRole>();
    }

    /// <summary>
    /// Creates code for given numbers.
    /// Example: if numbers are 4,2 then returns "00004.00002";
    /// </summary>
    /// <param name="numbers">Numbers</param>
    public static string CreateCode(params int[] numbers)
    {
        if (numbers.IsNullOrEmpty())
        {
            return null;
        }

        return numbers.Select(number => number.ToString(new string('0', OrganizationUnitConsts.CodeUnitLength))).JoinAsString(".");
    }

    /// <summary>
    /// Appends a child code to a parent code.
    /// Example: if parentCode = "00001", childCode = "00042" then returns "00001.00042".
    /// </summary>
    /// <param name="parentCode">Parent code. Can be null or empty if parent is a root.</param>
    /// <param name="childCode">Child code.</param>
    public static string AppendCode(string parentCode, string childCode)
    {
        if (childCode.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(childCode), "childCode can not be null or empty.");
        }

        if (parentCode.IsNullOrEmpty())
        {
            return childCode;
        }

        return parentCode + "." + childCode;
    }

    /// <summary>
    /// Gets relative code to the parent.
    /// Example: if code = "00019.00055.00001" and parentCode = "00019" then returns "00055.00001".
    /// </summary>
    /// <param name="code">The code.</param>
    /// <param name="parentCode">The parent code.</param>
    public static string GetRelativeCode(string code, string parentCode)
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
        }

        if (parentCode.IsNullOrEmpty())
        {
            return code;
        }

        if (code.Length == parentCode.Length)
        {
            return null;
        }

        return code.Substring(parentCode.Length + 1);
    }

    /// <summary>
    /// Calculates next code for given code.
    /// Example: if code = "00019.00055.00001" returns "00019.00055.00002".
    /// </summary>
    /// <param name="code">The code.</param>
    public static string CalculateNextCode(string code)
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
        }

        var parentCode = GetParentCode(code);
        var lastUnitCode = GetLastUnitCode(code);

        return AppendCode(parentCode, CreateCode(Convert.ToInt32(lastUnitCode) + 1));
    }

    /// <summary>
    /// Gets the last unit code.
    /// Example: if code = "00019.00055.00001" returns "00001".
    /// </summary>
    /// <param name="code">The code.</param>
    public static string GetLastUnitCode(string code)
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
        }

        var splittedCode = code.Split('.');
        return splittedCode[splittedCode.Length - 1];
    }

    /// <summary>
    /// Gets parent code.
    /// Example: if code = "00019.00055.00001" returns "00019.00055".
    /// </summary>
    /// <param name="code">The code.</param>
    public static string GetParentCode(string code)
    {
        if (code.IsNullOrEmpty())
        {
            throw new ArgumentNullException(nameof(code), "code can not be null or empty.");
        }

        var splittedCode = code.Split('.');
        if (splittedCode.Length == 1)
        {
            return null;
        }

        return splittedCode.Take(splittedCode.Length - 1).JoinAsString(".");
    }

    public virtual void AddRole(Guid roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        if (IsInRole(roleId))
        {
            return;
        }

        Roles.Add(new OrganizationUnitRole(roleId, Id, TenantId));
    }

    public virtual void RemoveRole(Guid roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        if (!IsInRole(roleId))
        {
            return;
        }

        Roles.RemoveAll(r => r.RoleId == roleId);
    }

    public virtual bool IsInRole(Guid roleId)
    {
        Check.NotNull(roleId, nameof(roleId));

        return Roles.Any(r => r.RoleId == roleId);
    }
}
