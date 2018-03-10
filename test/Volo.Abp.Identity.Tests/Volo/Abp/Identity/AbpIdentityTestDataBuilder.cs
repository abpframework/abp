using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Permissions;

namespace Volo.Abp.Identity
{
    public class AbpIdentityTestDataBuilder : ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IIdentityRoleRepository _roleRepository;
        private readonly IPermissionGrantRepository _permissionGrantRepository;

        private IdentityRole _adminRole;
        private IdentityRole _moderator;
        private IdentityRole _supporterRole;
        private IdentityUser _adminUser;
        private IdentityUser _john;
        private IdentityUser _david;

        public AbpIdentityTestDataBuilder(
            IGuidGenerator guidGenerator,
            IIdentityUserRepository userRepository,
            IIdentityRoleRepository roleRepository,
            IPermissionGrantRepository permissionGrantRepository)
        {
            _guidGenerator = guidGenerator;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _permissionGrantRepository = permissionGrantRepository;
        }

        public void Build()
        {
            AddRoles();
            AddRolePermissions();
            AddUsers();
            AddUserPermissions();
        }

        private void AddRoles()
        {
            _adminRole = new IdentityRole(_guidGenerator.Create(), "admin");
            _moderator = new IdentityRole(_guidGenerator.Create(), "moderator");
            _supporterRole = new IdentityRole(_guidGenerator.Create(), "supporter");

            _roleRepository.Insert(_adminRole);
            _roleRepository.Insert(_moderator);
            _roleRepository.Insert(_supporterRole);
        }

        private void AddRolePermissions()
        {
            AddPermission(TestPermissionNames.MyPermission1, RolePermissionValueProvider.ProviderName, _adminRole.Name);
            AddPermission(TestPermissionNames.MyPermission2, RolePermissionValueProvider.ProviderName, _adminRole.Name);
            AddPermission(TestPermissionNames.MyPermission2_ChildPermission1, RolePermissionValueProvider.ProviderName, _adminRole.Name);

            AddPermission(TestPermissionNames.MyPermission1, RolePermissionValueProvider.ProviderName, _moderator.Name);
            AddPermission(TestPermissionNames.MyPermission2, RolePermissionValueProvider.ProviderName, _moderator.Name);

            AddPermission(TestPermissionNames.MyPermission1, RolePermissionValueProvider.ProviderName, _supporterRole.Name);
        }

        private void AddUsers()
        {
            _adminUser = new IdentityUser(_guidGenerator.Create(), "administrator");
            _adminUser.AddRole(_adminRole.Id);
            _userRepository.Insert(_adminUser);

            _john = new IdentityUser(_guidGenerator.Create(), "john.nash");
            _john.AddRole(_moderator.Id);
            _john.AddRole(_supporterRole.Id);
            _userRepository.Insert(_john);

            _david = new IdentityUser(_guidGenerator.Create(), "david");
            _userRepository.Insert(_david);
        }

        private void AddUserPermissions()
        {
            AddPermission(TestPermissionNames.MyPermission1, UserPermissionValueProvider.ProviderName, _david.Id.ToString());
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