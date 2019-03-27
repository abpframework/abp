using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class HttpContextTenantResolveResultAccessor : ITenantResolveResultAccessor, ITransientDependency
    {
        [CanBeNull]
        public TenantResolveResult Result
        {
            get
            {
                if (_httpContextAccessor.HttpContext == null)
                {
                    return null;
                }

                return _httpContextAccessor.HttpContext.Items[""] as TenantResolveResult;
            }
            set
            {
                if (_httpContextAccessor.HttpContext == null)
                {
                    return;
                }

                _httpContextAccessor.HttpContext.Items[""] = value;
            }
        }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextTenantResolveResultAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}