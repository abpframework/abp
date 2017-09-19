using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Xunit;

namespace Volo.Abp.Identity
{
    public class IdentityUserAppService_Tests : AbpIdentityApplicationTestBase
    {
        private readonly IIdentityUserAppService _identityUserAppService;

        public IdentityUserAppService_Tests()
        {
            _identityUserAppService = ServiceProvider.GetRequiredService<IIdentityUserAppService>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            var result = await _identityUserAppService.GetListAsync(new PagedAndSortedResultRequestDto());
            result.TotalCount.ShouldBeGreaterThan(0);
            result.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task CreateAsync()
        {
            var input = new IdentityUserCreateOrUpdateDto
            {
                UserName = Guid.NewGuid().ToString(),
                Email = Guid.NewGuid().ToString("N").Left(16) + "@abp.io",
                LockoutEnabled = true,
                PhoneNumber = RandomHelper.GetRandom(10000000,100000000).ToString(),
                Password = "123qwe"
            };

            var result = await _identityUserAppService.CreateAsync(input);

            result.Id.ShouldNotBe(Guid.Empty);
            result.UserName.ShouldBe(input.UserName);
            result.Email.ShouldBe(input.Email);
            result.LockoutEnabled.ShouldBe(input.LockoutEnabled);
            result.PhoneNumber.ShouldBe(input.PhoneNumber);

            //TODO: Also check repository
        }
    }
}
