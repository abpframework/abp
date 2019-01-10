using Shouldly;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Volo.Abp.AspNetCore.Mvc
{
    public class LanguageController_Tests : AspNetCoreMvcTestBase
    {
        [Fact]
        public async Task Language_Switch()
        {
            HttpResponseMessage response = await GetResponseAsync($"/Abp/Languages/Switch?culture={"en"}&uiCulture={"en"}&returnUrl={"/abc"}", HttpStatusCode.Redirect);

            response.StatusCode.ShouldBe(HttpStatusCode.Redirect);
        }
    }
}
