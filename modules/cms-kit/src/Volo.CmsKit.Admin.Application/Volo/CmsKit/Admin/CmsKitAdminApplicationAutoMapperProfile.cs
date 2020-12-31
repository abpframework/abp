using AutoMapper;
using Volo.CmsKit.Admin.Contents;
using Volo.CmsKit.Admin.Pages;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.Admin
{
    public class CmsKitAdminApplicationAutoMapperProfile : Profile
    {
        public CmsKitAdminApplicationAutoMapperProfile()
        {
            CreateMap<Page, PageDto>();

            CreateMap<Content, ContentDto>(MemberList.Destination);
            CreateMap<ContentCreateDto, Content>(MemberList.Source);
            CreateMap<ContentUpdateDto, Content>(MemberList.Source);
        }
    }
}
