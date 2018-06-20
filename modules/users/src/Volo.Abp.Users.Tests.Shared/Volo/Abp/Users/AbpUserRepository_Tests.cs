using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.Users
{
    public abstract class AbpUserRepository_Tests<TStartupModule> : AbpUsersTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly AbpUsersLocalTestData _localTestData;
        private readonly IAbpUserRepository _userRepository;

        protected AbpUserRepository_Tests()
        {
            _userRepository = GetRequiredService<IAbpUserRepository>();
            _localTestData = GetRequiredService<AbpUsersLocalTestData>();
        }

        [Fact]
        public async Task FindAsync()
        {
            var john = await _userRepository.FindAsync(_localTestData.John.Id);
            john.ShouldNotBeNull();
            john.UserName.ShouldBe(_localTestData.John.UserName);

            //Undefined user
            (await _userRepository.FindAsync(Guid.NewGuid())).ShouldBeNull();
        }

        [Fact]
        public async Task FindByUserNameAsync()
        {
            var john = await _userRepository.FindByUserNameAsync(_localTestData.John.UserName);
            john.ShouldNotBeNull();
            john.Id.ShouldBe(_localTestData.John.Id);

            //Undefined user
            (await _userRepository.FindByUserNameAsync("undefined-user")).ShouldBeNull();
        }

        [Fact]
        public async Task GetListAsync()
        {
            (await _userRepository.GetListAsync(new Guid[0])).Any().ShouldBeFalse();
            (await _userRepository.GetListAsync(new[] { _localTestData.John.Id })).Count.ShouldBe(1);
            (await _userRepository.GetListAsync(new[] { _localTestData.John.Id, _localTestData.David.Id })).Count.ShouldBe(2);
            (await _userRepository.GetListAsync(new[] { _localTestData.John.Id, _localTestData.David.Id, Guid.NewGuid() })).Count.ShouldBe(2);
        }
    }
}
