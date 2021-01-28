using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Volo.Abp.ObjectExtending.TestObjects;

namespace Volo.Abp.AutoMapper.SampleClasses
{
    public class MyMapProfile : Profile
    {
        public MyMapProfile()
        {
            CreateMap<MyEntity, MyEntityDto>().EqualityComparison((dto, entity) => dto.Id == entity.Id).ReverseMap();

            CreateMap<ExtensibleTestPerson, ExtensibleTestPersonDto>()
                .MapExtraProperties(ignoredProperties: new[] { "CityName" });
        }
    }
}
