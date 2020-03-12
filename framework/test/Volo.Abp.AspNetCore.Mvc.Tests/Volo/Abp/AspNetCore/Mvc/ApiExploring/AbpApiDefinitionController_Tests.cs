using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.Http.Modeling;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc.ApiExploring
{
    public class AbpApiDefinitionController_Tests : AspNetCoreMvcTestBase
    {
        [Fact]
        public async Task GetAsync()
        {
            var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>("/api/abp/api-definition");
            model.ShouldNotBeNull();
        }
    }
}
