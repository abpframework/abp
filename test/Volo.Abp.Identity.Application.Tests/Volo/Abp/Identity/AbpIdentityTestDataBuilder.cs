using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.Identity
{
    public class AbpIdentityTestDataBuilder : ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IIdentityRoleRepository _roleRepository;

        private IdentityRole _adminRole;
        private IdentityRole _moderator;
        private IdentityRole _supporterRole;

        public AbpIdentityTestDataBuilder(
            IGuidGenerator guidGenerator,
            IIdentityUserRepository userRepository,
            IIdentityRoleRepository roleRepository)
        {
            _guidGenerator = guidGenerator;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public void Build()
        {
            AddRoles();
            AddUsers();
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

        private void AddUsers()
        {
            var john = new IdentityUser(_guidGenerator.Create(), "john.nash");
            john.Roles.Add(new IdentityUserRole(_guidGenerator.Create(), john.Id, _moderator.Id));
            john.Roles.Add(new IdentityUserRole(_guidGenerator.Create(), john.Id, _supporterRole.Id));
            _userRepository.Insert(john);
        }
    }
}