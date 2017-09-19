using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;

namespace Volo.Abp.Identity
{
    public class AbpIdentityTestDataBuilder : ITransientDependency
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IIdentityUserRepository _userRepository;

        public AbpIdentityTestDataBuilder(
            IGuidGenerator guidGenerator,
            IIdentityUserRepository userRepository)
        {
            _guidGenerator = guidGenerator;
            _userRepository = userRepository;
        }

        public void Build()
        {
            _userRepository.Insert(new IdentityUser(_guidGenerator.Create(), "john.nash"));
        }
    }
}