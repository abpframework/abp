using System.Collections.Generic;
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
            model.Types.IsNullOrEmpty().ShouldBeTrue();
        }

        [Fact]
        public async Task GetAsync_IncludeTypes()
        {
            var model = await GetResponseAsObjectAsync<ApplicationApiDescriptionModel>("/api/abp/api-definition?includeTypes=true");
            model.ShouldNotBeNull();
            model.Types.IsNullOrEmpty().ShouldBeFalse();
        }
    }
}
