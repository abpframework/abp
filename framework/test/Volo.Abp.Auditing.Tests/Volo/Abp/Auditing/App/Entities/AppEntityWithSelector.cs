using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.Auditing.App.Entities
{
    public class AppEntityWithSelector : AggregateRoot<Guid>
    {
        public AppEntityWithSelector(Guid id, string name)
            : base(id)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}
