using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Users
{
    public class AbpUsersTestDataBuilder : ITransientDependency
    {
        private readonly IAbpUserRepository _userRepository;
        private readonly AbpUsersLocalTestData _localTestData;

        public AbpUsersTestDataBuilder(
            IAbpUserRepository userRepository,
            AbpUsersLocalTestData localTestData)
        {
            _userRepository = userRepository;
            _localTestData = localTestData;
        }

        public void Build()
        {
            AddUsers();
        }

        private void AddUsers()
        {
            _userRepository.Insert(_localTestData.John);
            _userRepository.Insert(_localTestData.David);
        }
    }
}