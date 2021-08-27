using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Json;

namespace Volo.Abp.Http.Client
{
    public class ClientProxyBase<TService> : ITransientDependency
    {
        public IAbpLazyServiceProvider LazyServiceProvider { get; set; }

        protected IHttpProxyExecuter HttpProxyExecuter => LazyServiceProvider.LazyGetRequiredService<IHttpProxyExecuter>();
        protected IJsonSerializer JsonSerializer => LazyServiceProvider.LazyGetRequiredService<IJsonSerializer>();

        protected virtual async Task MakeRequestAsync(ActionApiDescriptionModel action, params object[] arguments)
        {
            await HttpProxyExecuter.MakeRequestAsync(new HttpProxyExecuterContext(action, BuildArguments(action, arguments), typeof(TService)));
        }

        protected virtual async Task<T> MakeRequestAsync<T>(ActionApiDescriptionModel action, params object[] arguments)
        {
            return await HttpProxyExecuter.MakeRequestAndGetResultAsync<T>(new HttpProxyExecuterContext(action, BuildArguments(action, arguments), typeof(TService)));
        }

        protected virtual Dictionary<string, object> BuildArguments(ActionApiDescriptionModel action, object[] arguments)
        {
            var parameters = action.Parameters.GroupBy(x => x.NameOnMethod).Select(x => x.Key).ToList();
            var dict = new Dictionary<string, object>();

            for (var i = 0; i < parameters.Count; i++)
            {
                dict[parameters[i]] = arguments[i];
            }

            return dict;
        }
    }
}
