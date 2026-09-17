using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using static Volo.Abp.PermissionManagement.Web.Pages.AbpPermissionManagement.PermissionManagementModal;

namespace Volo.Abp.PermissionManagement.Web;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PermissionGroupDtoToPermissionGroupViewModelMapper : MapperBase<PermissionGroupDto, PermissionGroupViewModel>
{
    [MapperIgnoreTarget(nameof(PermissionGroupViewModel.IsAllPermissionsGranted))]
    public override partial PermissionGroupViewModel Map(PermissionGroupDto source);

    [MapperIgnoreTarget(nameof(PermissionGroupViewModel.IsAllPermissionsGranted))]
    public override partial void Map(PermissionGroupDto source, PermissionGroupViewModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PermissionGrantInfoDtoToPermissionGrantInfoViewModelMapper : MapperBase<PermissionGrantInfoDto, PermissionGrantInfoViewModel>
{
    [MapperIgnoreTarget(nameof(PermissionGrantInfoViewModel.Depth))]
    public override partial PermissionGrantInfoViewModel Map(PermissionGrantInfoDto source);

    [MapperIgnoreTarget(nameof(PermissionGrantInfoViewModel.Depth))]
    public override partial void Map(PermissionGrantInfoDto source, PermissionGrantInfoViewModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProviderInfoDtoToProviderInfoViewModelMapper : MapperBase<ProviderInfoDto, ProviderInfoViewModel>
{
    public override partial ProviderInfoViewModel Map(ProviderInfoDto source);

    public override partial void Map(ProviderInfoDto source, ProviderInfoViewModel destination);
}