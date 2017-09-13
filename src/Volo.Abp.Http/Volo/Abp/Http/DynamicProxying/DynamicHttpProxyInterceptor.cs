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

        private static readonly MethodInfo GenericInterceptAsyncMethod;
        private static readonly object[] EmptyObjectArray = new object[0];

        static DynamicHttpProxyInterceptor()
        {
            GenericInterceptAsyncMethod = typeof(DynamicHttpProxyInterceptor)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .First(m => m.Name == nameof(InterceptAsync) && m.IsGenericMethodDefinition);
        }

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
            invocation.ReturnValue = GenericInterceptAsyncMethod
                .MakeGenericMethod(returnTypeWithoutTask)
                .Invoke(this, EmptyObjectArray);
        }

        private async Task<T> InterceptAsync<T>()
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
                    typeof(T),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });

                return (T)result;
            }
        }
    }
}