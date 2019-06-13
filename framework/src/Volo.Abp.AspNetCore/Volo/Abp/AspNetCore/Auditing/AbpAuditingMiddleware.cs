using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Volo.Abp.Auditing;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Auditing
{
    public class AbpAuditingMiddleware : IMiddleware
    {
        private readonly IAuditingManager _auditingManager;

        protected AbpAuditingOptions Options { get; }
        protected ICurrentUser CurrentUser { get; }

        public AbpAuditingMiddleware(
            IAuditingManager auditingManager,
            ICurrentUser currentUser,
            IOptions<AbpAuditingOptions> options)
        {
            _auditingManager = auditingManager;

            CurrentUser = currentUser;
            Options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!ShouldWriteAuditLog(context))
            {
                await next(context);
                return;
            }

            using (var scope = _auditingManager.BeginScope())
            {
                try
                {
                    await next(context);
                }
                finally
                {
                    await scope.SaveAsync();
                }
            }
        }

        private bool ShouldWriteAuditLog(HttpContext httpContext)
        {
            if (!Options.IsEnabled)
            {
                return false;
            }

            if (!Options.IsEnabledForAnonymousUsers && !CurrentUser.IsAuthenticated)
            {
                return false;
            }

            if (!Options.IsEnabledForGetRequests && 
                string.Equals(httpContext.Request.Method, HttpMethods.Get, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}
