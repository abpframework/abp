using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Auditing.App.Entities
{
    [Audited]
    public class AppEntityWithAuditedAndPropertyHasDisableAuditing : AggregateRoot<Guid>
    {
        protected AppEntityWithAuditedAndPropertyHasDisableAuditing()
        {

        }

        public AppEntityWithAuditedAndPropertyHasDisableAuditing(Guid id, string name, string  name2)
            : base(id)
        {
            Name = name;       
            Name2 = name2;
        }

        public string Name { get; set; }

        [DisableAuditing]
        public string Name2 { get; set; }
    }
}
