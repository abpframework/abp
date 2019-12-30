using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Uow
{
    public class UnitOfWorkMiddleware_Tests : AspNetCoreMvcTestBase
    {
        [Fact]
        public async Task Get_Actions_Should_Not_Be_Transactional()
        {
            await GetResponseAsStringAsync("/api/unitofwork-test/ActionRequiresUow").ConfigureAwait(false);
        }

        [Fact]
        public async Task Non_Get_Actions_Should_Be_Transactional()
        {
            var result = await Client.PostAsync("/api/unitofwork-test/ActionRequiresUowPost", null).ConfigureAwait(false);
            result.IsSuccessStatusCode.ShouldBeTrue();
        }
    }
}
