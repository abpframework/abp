using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Modularity;
using Xunit;

namespace Volo.Abp.Users
{
    public abstract class ExternalUserLookupService_Tests<TStartupModule> : AbpUsersTestBase<TStartupModule> 
        where TStartupModule : IAbpModule
    {
        private readonly IAbpUserLookupService _lookupService;
        private readonly AbpUsersLocalTestData _localTestData;
        private readonly AbpUsersExternalTestData _externalTestData;

        protected ExternalUserLookupService_Tests()
        {
            _lookupService = GetRequiredService<IAbpUserLookupService>();
            _localTestData = GetRequiredService<AbpUsersLocalTestData>();
            _externalTestData = GetRequiredService<AbpUsersExternalTestData>();
        }

        protected override void AfterAddApplication(IServiceCollection services)
        {
            services.AddTransient<IExternalAbpUserLookupServiceProvider, TestExternalAbpUserLookupServiceProvider>();
        }

        [Fact]
        public async Task FindByUserNameAsync()
        {
            (await GetRequiredService<IAbpUserRepository>().FindByUserNameAsync(_localTestData.John.UserName)).ShouldNotBeNull();

            (await _lookupService.FindByUserNameAsync("undefined-user")).ShouldBeNull();
            (await _lookupService.FindByUserNameAsync(_localTestData.John.UserName)).ShouldBeNull(); //Because it's not available in the external provider. And this will delete the user!
            (await _lookupService.FindByUserNameAsync(_localTestData.David.UserName)).ShouldNotBeNull();
            (await _lookupService.FindByUserNameAsync(_externalTestData.Neo.UserName)).ShouldNotBeNull();

            (await GetRequiredService<IAbpUserRepository>().FindByUserNameAsync(_localTestData.John.UserName)).ShouldBeNull();
        }

        [Fact]
        public async Task FindByIdAsync()
        {
            (await GetRequiredService<IAbpUserRepository>().FindAsync(_localTestData.John.Id)).ShouldNotBeNull();

            (await _lookupService.FindByIdAsync(Guid.NewGuid())).ShouldBeNull();
            (await _lookupService.FindByIdAsync(_localTestData.John.Id)).ShouldBeNull(); //Because it's not available in the external provider. And this will delete the user!
            (await _lookupService.FindByIdAsync(_localTestData.David.Id)).ShouldNotBeNull();
            (await _lookupService.FindByIdAsync(_externalTestData.Neo.Id)).ShouldNotBeNull();

            (await GetRequiredService<IAbpUserRepository>().FindAsync(_localTestData.John.Id)).ShouldBeNull();
        }

        public class TestExternalAbpUserLookupServiceProvider : IExternalAbpUserLookupServiceProvider
        {
            private readonly AbpUsersExternalTestData _externalTestData;

            public TestExternalAbpUserLookupServiceProvider(AbpUsersExternalTestData externalTestData)
            {
                _externalTestData = externalTestData;
            }

            public Task<IAbpUserData> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(_externalTestData.GetAllUsers().FirstOrDefault(u => u.Id == id));
            }

            public Task<IAbpUserData> FindByUserNameAsync(string userName, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(_externalTestData.GetAllUsers().FirstOrDefault(u => u.UserName == userName));
            }
        }
    }
}