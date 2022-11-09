using AutoMapper;
using Volo.Abp.TestApp.Testing;

namespace Volo.Abp.TestApp.Domain;

public class TestAutoMapProfile : Profile
{
    public TestAutoMapProfile()
    {
        CreateMap<PersonEto, Person>().ReverseMap();

        CreateMap<Product, ProductCacheItem>();
    }
}
