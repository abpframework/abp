using Volo.Abp.Domain.Entities;

namespace Volo.Abp.EntityFrameworkCore.TestApp.SecondContext
{
    public class BookInSecondDbContext : AggregateRoot
    {
        public string Name { get; set; }
    }
}