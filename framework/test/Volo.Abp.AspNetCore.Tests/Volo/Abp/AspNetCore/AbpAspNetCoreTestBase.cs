using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shouldly;
using Volo.Abp.AspNetCore.TestBase;

namespace Volo.Abp.AspNetCore
{
    public abstract class AbpAspNetCoreTestBase<TStartup> : AbpAspNetCoreIntegratedTestBase<TStartup>
        where TStartup : class
    {
        private static readonly CamelCasePropertyNamesContractResolver SharedCamelCasePropertyNamesContractResolver =
            new CamelCasePropertyNamesContractResolver();

        protected virtual async Task<T> GetResponseAsObjectAsync<T>(string url, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var strResponse = await GetResponseAsStringAsync(url, expectedStatusCode);
            return JsonConvert.DeserializeObject<T>(strResponse, new JsonSerializerSettings
            {
                ContractResolver = SharedCamelCasePropertyNamesContractResolver
            });
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