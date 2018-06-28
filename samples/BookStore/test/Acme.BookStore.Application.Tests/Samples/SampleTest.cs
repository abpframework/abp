using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.Identity;
using Xunit;

namespace Acme.BookStore.Samples
{
    public class SampleTest : BookStoreApplicationTestBase
    {
        private readonly IIdentityUserAppService _userAppService;

        public SampleTest()
        {
            _userAppService = ServiceProvider.GetRequiredService<IIdentityUserAppService>();
        }

        [Fact]
        public async Task Initial_Data_Should_Contain_Admin_User()
        {
            var result = await _userAppService.GetListAsync(new GetIdentityUsersInput());
            result.TotalCount.ShouldBeGreaterThan(0);
            result.Items.ShouldContain(u => u.UserName == "admin");
        }
    }
}
