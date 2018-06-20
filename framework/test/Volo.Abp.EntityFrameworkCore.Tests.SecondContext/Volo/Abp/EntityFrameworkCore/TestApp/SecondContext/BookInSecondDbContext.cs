using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.EntityFrameworkCore.TestApp.SecondContext
{
    public class BookInSecondDbContext : AggregateRoot<Guid>
    {
        public string Name { get; set; }
    }
}