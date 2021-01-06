using AutoMapper;
using Volo.CmsKit.Admin.Pages;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.Admin
{
    public class CmsKitAdminApplicationAutoMapperProfile : Profile
    {
        public CmsKitAdminApplicationAutoMapperProfile()
        {
            CreateMap<Page, PageDto>();
        }
    }
}
