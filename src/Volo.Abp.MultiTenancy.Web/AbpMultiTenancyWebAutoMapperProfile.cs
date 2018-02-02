using AutoMapper;
using Volo.Abp.MultiTenancy.Web.Pages.MultiTenancy.Tenants;

namespace Volo.Abp.MultiTenancy.Web
{
    public class AbpMultiTenancyWebAutoMapperProfile : Profile
    {
        public AbpMultiTenancyWebAutoMapperProfile()
        {
            CreateRoleMappings();
        }

        private void CreateRoleMappings()
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
