using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.EntityFrameworkCore.TestApp.FifthContext;

public class FifthDbContextDummyEntity : AggregateRoot<Guid>
{
    public string Value { get; set; }
}
