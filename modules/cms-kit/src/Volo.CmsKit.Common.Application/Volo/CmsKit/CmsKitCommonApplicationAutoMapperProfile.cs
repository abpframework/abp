using AutoMapper;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Common.Application.Volo.CmsKit
{
    public class CmsKitCommonApplicationAutoMapperProfile : Profile
    {
        public CmsKitCommonApplicationAutoMapperProfile()
        {
            CreateMap<Content, ContentDto>();
        }
    }
}
