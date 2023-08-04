using AutoMapper;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Tags;
using Volo.CmsKit.Users;

namespace Volo.CmsKit;

public class CmsKitCommonApplicationAutoMapperProfile : Profile
{
    public CmsKitCommonApplicationAutoMapperProfile()
    {
        CreateMap<Tag, TagDto>().MapExtraProperties();

        CreateMap<PopularTag, PopularTagDto>();

        CreateMap<CmsUser, CmsUserDto>().MapExtraProperties();

        CreateMap<BlogFeature, BlogFeatureCacheItem>().MapExtraProperties();
        CreateMap<BlogFeature, BlogFeatureDto>().MapExtraProperties();
        CreateMap<BlogFeatureCacheItem, BlogFeatureDto>()
            .MapExtraProperties()
            .ReverseMap()
            .MapExtraProperties();
    }
}
