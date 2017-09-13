using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Http.DynamicProxying
{
    public class DynamicHttpProxyInterceptor : AbpInterceptor, ITransientDependency
    {
        private readonly IDynamicProxyHttpClientFactory _httpClientFactory;

        public DynamicHttpProxyInterceptor(IDynamicProxyHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public override void Intercept(IAbpMethodInvocation invocation)
        {
            throw new System.NotImplementedException();
        }

        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            var returnTypeWithoutTask = invocation.Method.ReturnType.GenericTypeArguments[0];

            //var result = await GetResult(client, returnTypeWithoutTask);

            var methods = typeof(DynamicHttpProxyInterceptor).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
            var getResultMethod = methods
                .Where(m => m.Name == nameof(GetResult))
                .First()
                .MakeGenericMethod(returnTypeWithoutTask);

            invocation.ReturnValue = getResultMethod.Invoke(this, new object[] { returnTypeWithoutTask });
        }

        private async Task<T> GetResult<T>(Type returnTypeWithoutTask)
        {
            using (var client = _httpClientFactory.Create())
            {
                var response = await client.GetAsync("/api/app/people");
                if (!response.IsSuccessStatusCode)
                {
                    throw new AbpException("Remote service returns error!");
                }

                var content = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject(
                    content,
                    returnTypeWithoutTask,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });

                return (T)result;
            }
        }
    }
}