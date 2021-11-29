using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.EntityFrameworkCore.TestApp.FourthContext
{
    public class FourthDbContextDummyEntity : AggregateRoot<Guid>
    {
        public string Value { get; set; }
    }
}
