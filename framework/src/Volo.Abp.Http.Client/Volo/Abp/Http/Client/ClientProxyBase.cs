using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Json;

namespace Volo.Abp.Http.Client
{
    public class ClientProxyBase<TService>
    {
        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }

        protected IHttpProxyExecuter HttpProxyExecuter => LazyServiceProvider.LazyGetRequiredService<IHttpProxyExecuter>();
        protected IJsonSerializer JsonSerializer => LazyServiceProvider.LazyGetRequiredService<IJsonSerializer>();

        protected virtual async Task MakeRequestAsync(ActionApiDescriptionModel action, params object[] arguments)
        {
            await HttpProxyExecuter.MakeRequestAsync(new HttpProxyExecuterContext(action, BuildArguments(action.Name, arguments), typeof(TService)));
        }

        protected virtual async Task<T> MakeRequestAsync<T>(ActionApiDescriptionModel action, params object[] arguments)
        {
            return await HttpProxyExecuter.MakeRequestAndGetResultAsync<T>(new HttpProxyExecuterContext(action, BuildArguments(action.Name, arguments), typeof(TService)));
        }

        protected virtual Dictionary<string, object> BuildArguments(string methodName, object[] arguments)
        {
            var method = typeof(TService).GetMethod(methodName);
            var dict = new Dictionary<string, object>();

            var methodParameters = method.GetParameters();
            for (var i = 0; i < methodParameters.Length; i++)
            {
                dict[methodParameters[i].Name] = arguments[i];
            }

            return dict;
        }
    }
}
