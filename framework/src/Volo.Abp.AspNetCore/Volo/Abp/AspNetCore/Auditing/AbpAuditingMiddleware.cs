using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Volo.Abp.Auditing;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Auditing
{
    public class AbpAuditingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAuditingManager _auditingManager;

        protected AuditingOptions Options { get; }
        protected ICurrentUser CurrentUser { get; }

        public AbpAuditingMiddleware(
            RequestDelegate next, 
            IAuditingManager auditingManager,
            ICurrentUser currentUser,
            IOptions<AuditingOptions> options)
        {
            _next = next;
            _auditingManager = auditingManager;

            CurrentUser = currentUser;
            Options = options.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!ShouldWriteAuditLog())
            {
                await _next(httpContext);
                return;
            }

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

        private bool ShouldWriteAuditLog()
        {
            if (!Options.IsEnabled)
            {
                return false;
            }

            if (!Options.IsEnabledForAnonymousUsers && !CurrentUser.IsAuthenticated)
            {
                return false;
            }

            return true;
        }
    }
}
