using AutoMapper;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Pages;
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
        }
    }
}
