using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.Users
{
    public abstract class LocalUserLookupService_Tests<TStartupModule> : AbpUsersTestBase<TStartupModule> 
        where TStartupModule : IAbpModule
    {
        private readonly IAbpUserLookupService _lookupService;
        private readonly AbpUsersLocalTestData _localTestData;

        protected LocalUserLookupService_Tests()
        {
            _lookupService = GetRequiredService<IAbpUserLookupService>();
            _localTestData = GetRequiredService<AbpUsersLocalTestData>();
        }

        [Fact]
        public async Task FindByUserNameAsync()
        {
            (await _lookupService.FindByUserNameAsync(_localTestData.John.UserName)).ShouldNotBeNull();
            (await _lookupService.FindByUserNameAsync(_localTestData.David.UserName)).ShouldNotBeNull();
            (await _lookupService.FindByUserNameAsync("undefined-user")).ShouldBeNull();
        }
    }
}
