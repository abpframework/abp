using System;
using System.Linq;
using Volo.Abp.Application.Services;
using Volo.Abp.Http.Client.ClientProxying;

namespace Volo.Abp.Http.Client.Web.Conventions
{
    public static class AbpHttpClientProxyHelper
    {
        public static bool IsClientProxyService(Type type)
        {
            return typeof(IApplicationService).IsAssignableFrom(type) &&
                type.GetBaseClasses().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ClientProxyBase<>));
        }
    }
}
