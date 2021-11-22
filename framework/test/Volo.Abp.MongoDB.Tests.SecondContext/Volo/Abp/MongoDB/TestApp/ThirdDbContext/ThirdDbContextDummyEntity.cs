using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.MongoDB.TestApp.ThirdDbContext;

public class ThirdDbContextDummyEntity : AggregateRoot<Guid>
{
    public string Value { get; set; }
}
