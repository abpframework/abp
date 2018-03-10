using System;

namespace Volo.Abp.AutoMapper.SampleClasses
{
    [AutoMap(typeof(MyEntity))]
    public class MyEntityDto
    {
        public Guid Id { get; set; }

        public int Number { get; set; }
    }
}