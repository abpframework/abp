using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Riok.Mapperly.Abstractions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Mapperly;
using Volo.Abp.TenantManagement.Web.Pages.TenantManagement.Tenants;

namespace Volo.Abp.TenantManagement.Web;

[Mapper]
[MapExtraProperties]
public partial class TenantDtoToTenantInfoModelMapper
    : MapperBase<TenantDto, EditModalModel.TenantInfoModel>
{
    public override partial EditModalModel.TenantInfoModel Map(TenantDto source);

    public override partial void Map(TenantDto source, EditModalModel.TenantInfoModel destination);
}

[Mapper]
[MapExtraProperties]
public partial class CreateTenantInfoModelToTenantCreateDtoMapper
    : TwoWayMapperBase<CreateModalModel.TenantInfoModel, TenantCreateDto>
{
    public override partial TenantCreateDto Map(CreateModalModel.TenantInfoModel source);

    public override partial void Map(CreateModalModel.TenantInfoModel source, TenantCreateDto destination);

    public override partial CreateModalModel.TenantInfoModel ReverseMap(TenantCreateDto source);

    public override partial void ReverseMap(TenantCreateDto source, CreateModalModel.TenantInfoModel destination);
}

[Mapper]
[MapExtraProperties]
public partial class TenantInfoModelToTenantUpdateDtoMapper
    : MapperBase<EditModalModel.TenantInfoModel, TenantUpdateDto>
{
    [MapperIgnoreSource(nameof(EditModalModel.TenantInfoModel.Id))]
    [MapperIgnoreSource(nameof(EditModalModel.TenantInfoModel.ConcurrencyStamp))]
    [MapperIgnoreTarget(nameof(TenantUpdateDto.ConcurrencyStamp))]
    public override partial TenantUpdateDto Map(EditModalModel.TenantInfoModel source);

    [MapperIgnoreSource(nameof(EditModalModel.TenantInfoModel.Id))]
    [MapperIgnoreSource(nameof(EditModalModel.TenantInfoModel.ConcurrencyStamp))]
    [MapperIgnoreTarget(nameof(TenantUpdateDto.ConcurrencyStamp))]
    public override partial void Map(EditModalModel.TenantInfoModel source, TenantUpdateDto destination);
}