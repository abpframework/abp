using System;
using Volo.Abp.Domain.Entities;

namespace AbpPerfTest.WithAbp.Entities
{
    public class Book : BasicAggregateRoot<Guid>
    {
        public string Name { get; set; }

        public float Price { get; set; }

        public bool IsAvailable { get; set; }

        public Book()
        {
            Id = Guid.NewGuid();
        }
    }
}
