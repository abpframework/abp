using AutoMapper;
using Volo.Abp.PermissionManagement.Web.Pages.AbpPermissionManagement;

namespace Volo.Abp.PermissionManagement.Web
{
    public class AbpPermissionManagementWebAutoMapperProfile : Profile
    {
        public AbpPermissionManagementWebAutoMapperProfile()
        {
            CreateMap<PermissionGroupDto, PermissionManagementModal.PermissionGroupViewModel>();

            CreateMap<PermissionGrantInfoDto, PermissionManagementModal.PermissionGrantInfoViewModel>()
                .ForMember(p => p.Depth, opts => opts.Ignore());

            CreateMap<ProviderInfoDto, PermissionManagementModal.ProviderInfoViewModel>();
        }
    }
}
