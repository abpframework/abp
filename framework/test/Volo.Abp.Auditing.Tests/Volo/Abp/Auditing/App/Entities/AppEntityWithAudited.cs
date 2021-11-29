using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Auditing.App.Entities
{
    [Audited]
    public class AppEntityWithAudited : AggregateRoot<Guid>
    {
        protected AppEntityWithAudited()
        {

        }

        public AppEntityWithAudited(Guid id, string name)
            : base(id)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}