using System;

namespace AbpPerfTest.WithoutAbp.Entities
{
    public class Book
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public bool IsAvailable { get; set; }

        public Book()
        {
            Id = Guid.NewGuid();
        }
    }
}
