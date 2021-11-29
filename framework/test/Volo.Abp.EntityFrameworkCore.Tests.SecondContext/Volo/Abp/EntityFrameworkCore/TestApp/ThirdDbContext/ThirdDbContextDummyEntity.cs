using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.EntityFrameworkCore.TestApp.ThirdDbContext
{
    public class ThirdDbContextDummyEntity : AggregateRoot<Guid>
    {
        public string Value { get; set; }
    }
}