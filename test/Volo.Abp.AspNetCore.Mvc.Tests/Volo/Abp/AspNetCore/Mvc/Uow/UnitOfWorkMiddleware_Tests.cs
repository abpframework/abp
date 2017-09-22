using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Uow
{
    public class UnitOfWorkMiddleware_Tests : AspNetCoreMvcTestBase
    {
        [Fact]
        public async Task ActionRequiresUow()
        {
            await GetResponseAsStringAsync("/api/unitofwork-test/ActionRequiresUow");
        }
    }
}
