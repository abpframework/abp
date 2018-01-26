using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.AutoMapper.SampleClasses
{
    public class MyEntityDto2 : EntityDto<Guid>
    {
        public int Number { get; set; }
    }
}