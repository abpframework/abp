using AutoMapper;
using Volo.Abp.TenantManagement.Web.Pages.TenantManagement.Tenants;

namespace Volo.Abp.TenantManagement.Web
{
    public class AbpTenantManagementWebAutoMapperProfile : Profile
    {
        public AbpTenantManagementWebAutoMapperProfile()
        {
            CreateRoleMappings();
        }

        protected virtual void CreateRoleMappings()
        {
            //List
            CreateMap<TenantDto, EditModalModel.TenantInfoModel>();

            //CreateModal
            CreateMap<CreateModalModel.TenantInfoModel, TenantCreateDto>();

            //EditModal
            CreateMap<EditModalModel.TenantInfoModel, TenantUpdateDto>();
        }
    }
}
