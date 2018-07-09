using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.Auditing;

namespace Volo.Abp.AspNetCore.Auditing
{
    public class AbpAuditingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAuditingManager _auditingManager;

        public AbpAuditingMiddleware(RequestDelegate next, IAuditingManager auditingManager)
        {
            _next = next;
            _auditingManager = auditingManager;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            using (var scope = _auditingManager.BeginScope())
            {
                try
                {
                    await _next(httpContext);
                }
                finally
                {
                    await scope.SaveAsync();
                }
            }
        }
    }
}
