using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Menus;

public class MenuItem : AuditedAggregateRoot<Guid>, IMultiTenant
{
    /// <summary>
    /// Presents another <see cref="MenuItem"/> Id.
    /// If it's <see langword="null"/>, then it's a root menu item.
    /// </summary>
    public virtual Guid? ParentId { get; set; }

    [NotNull]
    public virtual string DisplayName { get; protected set; }

    public virtual bool IsActive { get; set; }

    [NotNull]
    public virtual string Url { get; protected set; }

    public virtual string Icon { get; set; }

    public virtual int Order { get; set; }

    public virtual string Target { get; set; }

    public virtual string ElementId { get; set; }

    public virtual string CssClass { get; set; }

    public virtual Guid? PageId { get; protected set; }

    public virtual Guid? TenantId { get; protected set; }

    public virtual string RequiredPermissionName { get; set; }

    protected MenuItem()
    {
    }

    public MenuItem(Guid id,
                    [NotNull] string displayName,
                    [NotNull] string url,
                    bool isActive = true,
                    [CanBeNull] Guid? parentId = null,
                    [CanBeNull] string icon = null,
                    int order = 0,
                    [CanBeNull] string target = null,
                    [CanBeNull] string elementId = null,
                    [CanBeNull] string cssClass = null,
                    [CanBeNull] Guid? tenantId = null,
                    [CanBeNull] string requiredPermissionName = null)
        : base(id)
    {
        SetDisplayName(displayName);
        IsActive = isActive;
        ParentId = parentId;
        SetUrl(url);
        Icon = icon;
        Order = order;
        Target = target;
        ElementId = elementId;
        CssClass = cssClass;
        TenantId = tenantId;
        RequiredPermissionName = requiredPermissionName;
    }

    public void SetDisplayName([NotNull] string displayName)
    {
        DisplayName = Check.NotNullOrEmpty(displayName, nameof(displayName), MenuItemConsts.MaxDisplayNameLength);
    }

    public void SetUrl([NotNull] string url)
    {
        Url = Check.NotNullOrEmpty(url, nameof(url), MenuItemConsts.MaxUrlLength);
    }

    internal void SetPageId(Guid? pageId)
    {
        PageId = pageId;
    }
}
