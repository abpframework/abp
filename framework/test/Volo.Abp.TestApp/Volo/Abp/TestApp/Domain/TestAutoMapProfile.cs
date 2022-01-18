using AutoMapper;

namespace Volo.Abp.TestApp.Domain;

public class TestAutoMapProfile : Profile
{
    public TestAutoMapProfile()
    {
        CreateMap<PersonEto, Person>().ReverseMap();
    }
}
