using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace Volo.Abp.TenantManagement.Application.Volo.Abp.TenantManagement;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class TenantToTenantDtoMapper
    : MapperBase<Tenant, TenantDto>
{
    public override partial TenantDto Map(Tenant source);
    public override partial void Map(Tenant source, TenantDto destination);
}