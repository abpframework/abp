using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.Uow
{
    public class UnitOfWorkPageFilter_Tests: AspNetCoreMvcTestBase
    {
        [Fact]
        public async Task Get_Actions_Should_Not_Be_Transactional()
        {
            await GetResponseAsStringAsync("/Uow/UnitOfWorkTestPage?handler=RequiresUow");
        }
        
        [Fact]
        public async Task Non_Get_Actions_Should_Be_Transactional()
        {
            var result = await Client.PostAsync("/Uow/UnitOfWorkTestPage?handler=RequiresUow", null);
            result.IsSuccessStatusCode.ShouldBeTrue();
        }
    }
}