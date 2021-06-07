using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.CmsKit.Menus
{
    public class Menu : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; protected set; }
        public ICollection<MenuItem> Items { get; protected set; }

        public Menu(Guid id, [NotNull] string name) : base(id)
        {
            SetName(name);
            Items = new HashSet<MenuItem>();
        }

        public void SetName([NotNull] string name)
        {
            Name = Check.NotNullOrEmpty(name, nameof(name), MenuConsts.MaxNameLength);
        }
    }
}