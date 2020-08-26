using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.Client;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.DynamicProxying;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Components.WebAssembly
{
    public class WebAssemblyCachedApplicationConfigurationClient : ICachedApplicationConfigurationClient, ITransientDependency
    {
        protected IHttpClientProxy<IAbpApplicationConfigurationAppService> Proxy { get; }
        protected ICurrentUser CurrentUser { get; }

        public WebAssemblyCachedApplicationConfigurationClient(
            IHttpClientProxy<IAbpApplicationConfigurationAppService> proxy,
            ICurrentUser currentUser)
        {
            Proxy = proxy;
            CurrentUser = currentUser;
        }

        public async Task<ApplicationConfigurationDto> GetAsync()
        {
            //TODO: Cache

            return await Proxy.Service.GetAsync();
        }

        protected virtual string CreateCacheKey()
        {
            return $"ApplicationConfiguration_{CurrentUser.Id?.ToString("N") ?? "Anonymous"}_{CultureInfo.CurrentUICulture.Name}";
        }
    }
}
