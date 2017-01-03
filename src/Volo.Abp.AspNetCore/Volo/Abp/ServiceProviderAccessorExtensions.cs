using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Volo.DependencyInjection;

namespace Volo.Abp
{
    public static class ServiceProviderAccessorExtensions
    {
        [CanBeNull]
        public static HttpContext GetHttpContext(this IServiceProviderAccessor serviceProviderAccessor)
        {
            return serviceProviderAccessor.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
        }
    }
}
