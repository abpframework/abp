using AutoMapper;

namespace Volo.Abp.AutoMapper.SampleClasses
{
    public class MyMapProfile : Profile
    {
        public MyMapProfile()
        {
            CreateMap<MyEntity, MyEntityDto>().ReverseMap();
        }
    }
}
