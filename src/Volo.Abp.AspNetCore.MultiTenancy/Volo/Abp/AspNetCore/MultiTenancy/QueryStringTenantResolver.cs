using Volo.Abp.MultiTenancy;
using Volo.ExtensionMethods.Collections.Generic;

namespace Volo.Abp.AspNetCore.MultiTenancy
{
    public class QueryStringTenantResolver : ITenantResolver
    {
        public const string TenantIdKey = "__tenantId";
        
        public void Resolve(ICurrentTenantResolveContext context)
        {
            var httpContext = context.GetHttpContext();
            if (httpContext == null)
            {
                return;
            }

            if (!httpContext.Request.QueryString.HasValue)
            {
                return;
            }

            var tenantId = httpContext.Request.Query[TenantIdKey];
            if (!tenantId.IsNullOrEmpty())
            {
                context.Tenant = new TenantInfo(tenantId);
            }
        }
    }
}
