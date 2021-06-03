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
        public ICollection<MenuItem> Items { get; set; }
    }
}