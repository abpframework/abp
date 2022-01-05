using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Volo.Abp.ObjectExtending.TestObjects;

namespace Volo.Abp.AutoMapper.SampleClasses;

public class MyMapProfile : Profile
{
    public MyMapProfile()
    {
        CreateMap<MyEntity, MyEntityDto>().EqualityComparison((dto, entity) => dto.Id == entity.Id).ReverseMap();

        CreateMap<ExtensibleTestPerson, ExtensibleTestPersonDto>()
            .MapExtraProperties(ignoredProperties: new[] { "CityName" });

        CreateMap<ExtensibleTestPerson, ExtensibleTestPersonWithRegularPropertiesDto>()
            .ForMember(x => x.Name, y => y.Ignore())
            .ForMember(x => x.Age, y => y.Ignore())
            .ForMember(x => x.IsActive, y => y.Ignore())
            .MapExtraProperties(mapToRegularProperties: true);
    }
}
