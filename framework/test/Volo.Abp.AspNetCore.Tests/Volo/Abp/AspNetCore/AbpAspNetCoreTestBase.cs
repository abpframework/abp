using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Shouldly;
using Volo.Abp.AspNetCore.TestBase;

namespace Volo.Abp.AspNetCore
{
    public class AbpAspNetCoreTestBase : AbpAspNetCoreTestBase<Startup>
    {

    }

    public abstract class AbpAspNetCoreTestBase<TStartup> : AbpAspNetCoreIntegratedTestBase<TStartup>
        where TStartup : class
    {
        private static readonly JsonSerializerSettings SharedJsonSerializerSettings =
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

        protected virtual async Task<T> GetResponseAsObjectAsync<T>(string url, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            var strResponse = await GetResponseAsStringAsync(url, expectedStatusCode);
            return JsonConvert.DeserializeObject<T>(strResponse, SharedJsonSerializerSettings);
        }

        protected virtual async Task<string> GetResponseAsStringAsync(string url, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            using (var response = await GetResponseAsync(url, expectedStatusCode))
            {
                return await response.Content.ReadAsStringAsync();
            }
        }

        protected virtual async Task<HttpResponseMessage> GetResponseAsync(string url, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url))
            {
                requestMessage.Headers.Add("Accept-Language", CultureInfo.CurrentUICulture.Name);
                var response = await Client.SendAsync(requestMessage);
                response.StatusCode.ShouldBe(expectedStatusCode);
                return response;
            }
        }
    }
}