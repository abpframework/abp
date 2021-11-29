using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.Abp.TenantManagement.Web.Pages.TenantManagement.Tenants;

namespace Volo.Abp.TenantManagement.Web;

public class AbpTenantManagementWebAutoMapperProfile : Profile
{
    public AbpTenantManagementWebAutoMapperProfile()
    {
        //List
        CreateMap<TenantDto, EditModalModel.TenantInfoModel>();

        //CreateModal
        CreateMap<CreateModalModel.TenantInfoModel, TenantCreateDto>()
            .MapExtraProperties();

        //EditModal
        CreateMap<EditModalModel.TenantInfoModel, TenantUpdateDto>()
            .MapExtraProperties();
    }
}
