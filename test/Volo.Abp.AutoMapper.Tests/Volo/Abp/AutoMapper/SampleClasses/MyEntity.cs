using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.AutoMapper.SampleClasses
{
    public class MyEntity : Entity<Guid>
    {
        public int Number { get; set; }
    }
}
