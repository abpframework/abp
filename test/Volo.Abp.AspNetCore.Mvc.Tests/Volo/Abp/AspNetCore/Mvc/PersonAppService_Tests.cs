using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.TestApp.Application;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class PersonAppService_Tests : AspNetCoreMvcTestBase
    {
        [Fact]
        public async Task GetAll_Test()
        {
            var result = await GetResponseAsObjectAsync<ListResultDto<PersonDto>>("/api/services/app/person/GetAll");
            result.Items.Count.ShouldBeGreaterThan(0);
        }
    }
}
