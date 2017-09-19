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
        public async Task GetList()
        {
            var result = await _identityUserAppService.GetListAsync(new PagedAndSortedResultRequestDto { MaxResultCount = 10 });
            result.TotalCount.ShouldBeGreaterThan(0);
            result.Items.Count.ShouldBeGreaterThan(0);
        }
    }
}
