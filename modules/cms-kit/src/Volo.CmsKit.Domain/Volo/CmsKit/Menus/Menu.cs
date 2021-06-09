using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Menus
{
    public class Menu : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public string Name { get; protected set; }

        public bool IsMainMenu { get; internal set; }

        public Guid? TenantId { get; protected set; }

        public ICollection<MenuItem> Items { get; protected set; }

        public Menu(Guid id, Guid? tenantId, [NotNull] string name) : base(id)
        {
            TenantId = tenantId;
            SetName(name);
            Items = new HashSet<MenuItem>();
        }

        public void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrEmpty(name, nameof(name), MenuConsts.MaxNameLength);
        }
    }
}