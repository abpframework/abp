using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.TestApp.Domain
{
    public class City : AggregateRoot<Guid>
    {
        public virtual string Name { get; set; }

        protected City()
        {
            
        }

        public City(Guid id, string name)
            : base(id)
        {
            Name = name;
        }
    }
}
