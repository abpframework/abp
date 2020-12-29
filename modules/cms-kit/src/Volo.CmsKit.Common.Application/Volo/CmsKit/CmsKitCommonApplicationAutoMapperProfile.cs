using AutoMapper;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Pages;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit
{
    public class CmsKitCommonApplicationAutoMapperProfile : Profile
    {
        public CmsKitCommonApplicationAutoMapperProfile()
        {
            CreateMap<Tag, TagDto>();
        }
    }
}
