using Volo.Abp.Application.Dtos;

namespace Volo.Abp.AutoMapper.SampleClasses
{
    [AutoMap(typeof(MyEntity))]
    public class MyEntityDto : EntityDto
    {
        public int Number { get; set; }
    }
}