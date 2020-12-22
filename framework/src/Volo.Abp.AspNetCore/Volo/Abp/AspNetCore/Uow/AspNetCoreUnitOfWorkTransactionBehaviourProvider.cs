using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;

namespace Volo.Abp.AspNetCore.Uow
{
    public class AspNetCoreUnitOfWorkTransactionBehaviourProvider : IUnitOfWorkTransactionBehaviourProvider, ITransientDependency
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public virtual bool? IsTransactional
        {
            get
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    return null;
                }

                //IdentityServer endpoint (TODO: Better to move to the IDS module)
                if (httpContext.Request.Path.Value?.StartsWith("/connect/", StringComparison.OrdinalIgnoreCase) == true)
                {
                    return false;
                }

                return !string.Equals(
                    httpContext.Request.Method,
                    HttpMethod.Get.Method, StringComparison.OrdinalIgnoreCase
                );
            }
        }

        public AspNetCoreUnitOfWorkTransactionBehaviourProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
