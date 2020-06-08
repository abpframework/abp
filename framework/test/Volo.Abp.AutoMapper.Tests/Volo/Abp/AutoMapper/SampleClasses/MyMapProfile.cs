using AutoMapper;
using Volo.Abp.ObjectExtending.TestObjects;

namespace Volo.Abp.AutoMapper.SampleClasses
{
    public class MyMapProfile : Profile
    {
        public MyMapProfile()
        {
            CreateMap<MyEntity, MyEntityDto>().ReverseMap();

            CreateMap<ExtensibleTestPerson, ExtensibleTestPersonDto>()
                .MapExtraProperties(ignoredProperties: new[] { "CityName" });
        }
    }
}
