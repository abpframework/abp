using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.Identity
{
    public class AbpIdentityTestDataBuilder : ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IIdentityUserRepository _userRepository;
        private readonly IIdentityRoleRepository _roleRepository;

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
            _roleRepository.Insert(new IdentityRole(_guidGenerator.Create(), "admin"));
            _roleRepository.Insert(new IdentityRole(_guidGenerator.Create(), "moderator"));
            _roleRepository.Insert(new IdentityRole(_guidGenerator.Create(), "supporter"));
        }

        private void AddUsers()
        {
            _userRepository.Insert(new IdentityUser(_guidGenerator.Create(), "john.nash"));
        }
    }
}