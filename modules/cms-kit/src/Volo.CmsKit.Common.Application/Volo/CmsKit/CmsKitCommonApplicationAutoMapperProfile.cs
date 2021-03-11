using AutoMapper;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Tags;
using Volo.CmsKit.Users;

namespace Volo.CmsKit
{
    public class CmsKitCommonApplicationAutoMapperProfile : Profile
    {
        public CmsKitCommonApplicationAutoMapperProfile()
        {
            CreateMap<Tag, TagDto>();

            CreateMap<CmsUser, CmsUserDto>();

            CreateMap<BlogFeature, BlogFeatureCacheItem>();
            CreateMap<BlogFeature, BlogFeatureDto>();
            CreateMap<BlogFeatureCacheItem, BlogFeatureDto>().ReverseMap();
        }
    }
}
