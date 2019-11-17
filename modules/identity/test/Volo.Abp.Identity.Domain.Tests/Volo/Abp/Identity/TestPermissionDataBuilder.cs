using Microsoft.AspNetCore.Identity;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.PermissionManagement;

namespace Volo.Abp.Identity
{
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

        public void Build()
        {
            AddRolePermissions();
            AddUserPermissions();
        }

        private void AddRolePermissions()
        {
            AddPermission(TestPermissionNames.MyPermission1, RolePermissionValueProvider.ProviderName, "admin");
            AddPermission(TestPermissionNames.MyPermission2, RolePermissionValueProvider.ProviderName, "admin");
            AddPermission(TestPermissionNames.MyPermission2_ChildPermission1, RolePermissionValueProvider.ProviderName, "admin");

            AddPermission(TestPermissionNames.MyPermission1, RolePermissionValueProvider.ProviderName, "moderator");
            AddPermission(TestPermissionNames.MyPermission2, RolePermissionValueProvider.ProviderName, "moderator");

            AddPermission(TestPermissionNames.MyPermission1, RolePermissionValueProvider.ProviderName, "supporter");
        }

        private void AddUserPermissions()
        {
            var david = _userRepository.FindByNormalizedUserName(_lookupNormalizer.NormalizeName("david"));
            AddPermission(TestPermissionNames.MyPermission1, UserPermissionValueProvider.ProviderName, david.Id.ToString());
        }

        private void AddPermission(string permissionName, string providerName, string providerKey)
        {
            _permissionGrantRepository.Insert(
                new PermissionGrant(
                    _guidGenerator.Create(),
                    permissionName,
                    providerName,
                    providerKey
                )
            );
        }
    }
}
