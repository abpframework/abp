using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Nito.AsyncEx;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;

namespace Volo.Abp.Http.Client.DynamicProxying
{
    public class ApiDescriptionCache : IApiDescriptionCache, ISingletonDependency
    {
        private readonly IDynamicProxyHttpClientFactory _httpClientFactory;

        private readonly Dictionary<string, ApplicationApiDescriptionModel> _cache;
        private readonly AsyncLock _asyncLock;

        public ApiDescriptionCache(IDynamicProxyHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            _cache = new Dictionary<string, ApplicationApiDescriptionModel>();
            _asyncLock = new AsyncLock();
        }

        public async Task<ApplicationApiDescriptionModel> GetAsync(string baseUrl, CancellationToken cancellationToken = default)
        {
            using (await _asyncLock.LockAsync(cancellationToken))
            {
                var model = _cache.GetOrDefault(baseUrl);
                if (model == null)
                {
                    _cache[baseUrl] = model = await GetFromServerAsync(baseUrl);
                }

                return model;
            }
        }

        private async Task<ApplicationApiDescriptionModel> GetFromServerAsync(string baseUrl)
        {
            using (var client = _httpClientFactory.Create())
            {
                var response = await client.GetAsync(baseUrl + "api/abp/api-definition");
                if (!response.IsSuccessStatusCode)
                {
                    throw new AbpException("Remote service returns error!");
                }

                var content = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject(
                    content,
                    typeof(ApplicationApiDescriptionModel),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });

                return (ApplicationApiDescriptionModel)result;
            }
        }
    }
}