using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Shouldly;
using Volo.Abp.AspNetCore.TestBase;

namespace MyCompanyName.MyProjectName
{
    public abstract class MyProjectNameWebTestBase : AbpAspNetCoreIntegratedTestBase<MyProjectNameWebTestStartup>
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return base
                .CreateHostBuilder()
                .UseContentRoot(WebContentDirectoryFinder.CalculateContentRootFolder());
        }

        protected virtual async Task<T> GetResponseAsObjectAsync<T>(string url, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var strResponse = await GetResponseAsStringAsync(url, expectedStatusCode);
            return JsonSerializer.Deserialize<T>(strResponse, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        }

        protected virtual async Task<string> GetResponseAsStringAsync(string url, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var response = await GetResponseAsync(url, expectedStatusCode);
            return await response.Content.ReadAsStringAsync();
        }

        protected virtual async Task<HttpResponseMessage> GetResponseAsync(string url, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var response = await Client.GetAsync(url);
            response.StatusCode.ShouldBe(expectedStatusCode);
            return response;
        }
    }
}
