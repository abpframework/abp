using AutoMapper;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Common.Application.Volo.CmsKit
{
    public class CmsKitCommonApplicationAutoMapperProfile : Profile
    {
        public CmsKitCommonApplicationAutoMapperProfile()
        {
            CreateMap<Content, ContentDto>();

            CreateMap<Tag, TagDto>();
        }
    }
}
