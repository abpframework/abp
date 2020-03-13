using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Auditing.App.Entities
{
    [DisableAuditing]
    public class AppEntityWithDisableAuditingAndPropertyHasAudited : AggregateRoot<Guid>
    {
        protected AppEntityWithDisableAuditingAndPropertyHasAudited()
        {

        }

        public AppEntityWithDisableAuditingAndPropertyHasAudited(Guid id, string name, string name2)
            : base(id)
        {
            Name = name;
            Name2 = name2;
        }

        [Audited]
        public string Name { get; set; }

        public string Name2 { get; set; }
    }
}
