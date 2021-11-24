using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Threading;

namespace Volo.Abp.Identity;

public class TestPermissionDataBuilder : ITransientDependency
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly IIdentityUserRepository _userRepository;
    private readonly IPermissionGrantRepository _permissionGrantRepository;
    private readonly ILookupNormalizer _lookupNormalizer;

    public TestPermissionDataBuilder(
        IGuidGenerator guidGenerator,
        IIdentityUserRepository userRepository,
        IPermissionGrantRepository permissionGrantRepository,
        ILookupNormalizer lookupNormalizer)
    {
        _guidGenerator = guidGenerator;
        _userRepository = userRepository;
        _permissionGrantRepository = permissionGrantRepository;
        _lookupNormalizer = lookupNormalizer;
    }

    public async Task Build()
    {
        await AddRolePermissions();
        await AddUserPermissions();
    }

    private async Task AddRolePermissions()
    {
        await AddPermission(TestPermissionNames.MyPermission1, RolePermissionValueProvider.ProviderName, "moderator");
        await AddPermission(TestPermissionNames.MyPermission2, RolePermissionValueProvider.ProviderName, "moderator");

        await AddPermission(TestPermissionNames.MyPermission1, RolePermissionValueProvider.ProviderName, "supporter");
    }

    private async Task AddUserPermissions()
    {
        var david = AsyncHelper.RunSync(() => _userRepository.FindByNormalizedUserNameAsync(_lookupNormalizer.NormalizeName("david")));
        await AddPermission(TestPermissionNames.MyPermission1, UserPermissionValueProvider.ProviderName, david.Id.ToString());
    }

    private async Task AddPermission(string permissionName, string providerName, string providerKey)
    {
        await _permissionGrantRepository.InsertAsync(
            new PermissionGrant(
                _guidGenerator.Create(),
                permissionName,
                providerName,
                providerKey
            )
        );
    }
}
