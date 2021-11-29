using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Auditing.App.Entities
{
    [DisableAuditing]
    public class AppEntityWithDisableAuditing : AggregateRoot<Guid>
    {
        protected AppEntityWithDisableAuditing()
        {

        }

        public AppEntityWithDisableAuditing(Guid id, string name)
            : base(id)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
