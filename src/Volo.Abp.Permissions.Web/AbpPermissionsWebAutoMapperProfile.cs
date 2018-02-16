using AutoMapper;
using Volo.Abp.Permissions.Web.Pages.AbpPermissions;

namespace Volo.Abp.Permissions.Web
{
    public class AbpPermissionsWebAutoMapperProfile : Profile
    {
        public AbpPermissionsWebAutoMapperProfile()
        {
            CreateMap<PermissionGroupDto, PermissionManagementModal.PermissionGroupViewModel>();

            CreateMap<PermissionGrantInfoDto, PermissionManagementModal.PermissionGrantInfoViewModel>();

            CreateMap<ProviderInfoDto, PermissionManagementModal.ProviderInfoViewModel>();
        }
    }
}
