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
    public Guid? ParentId { get; set; }

    [NotNull]
    public string DisplayName { get; protected set; }

    public bool IsActive { get; set; }

    [NotNull]
    public string Url { get; protected set; }

    public string Icon { get; set; }

    public int Order { get; set; }

    public string Target { get; set; }

    public string ElementId { get; set; }

    public string CssClass { get; set; }

    public Guid? PageId { get; protected set; }

    public Guid? TenantId { get; protected set; }

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
                    [CanBeNull] Guid? tenantId = null)
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
    }

    public void SetDisplayName([NotNull] string displayName)
    {
        DisplayName = Check.NotNullOrEmpty(displayName, nameof(displayName), MenuItemConsts.MaxDisplayNameLength);
    }

    public void SetUrl([NotNull] string url)
    {
        Url = Check.NotNullOrEmpty(url, nameof(url), MenuItemConsts.MaxUrlLength);
    }

    internal void SetPageId(Guid pageId)
    {
        PageId = pageId;
    }
}
