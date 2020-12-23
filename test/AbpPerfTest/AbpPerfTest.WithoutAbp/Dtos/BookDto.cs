using System;

namespace AbpPerfTest.WithoutAbp.Dtos
{
    public class BookDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public float Price { get; set; }

        public bool IsAvailable { get; set; }
    }
}
