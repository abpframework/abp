using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.TestApp.Domain
{
    [Serializable]
    public class City : AggregateRoot<Guid>
    {
        public string Name { get; set; }

        private City()
        {
            
        }

        public City(Guid id, string name)
            : base(id)
        {
            Name = name;
        }
    }
}
