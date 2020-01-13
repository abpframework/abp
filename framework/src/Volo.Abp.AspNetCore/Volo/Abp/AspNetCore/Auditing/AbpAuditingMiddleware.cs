using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Auditing
{
    public class AbpAuditingMiddleware : IMiddleware, ITransientDependency
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
                await next(context).ConfigureAwait(false);
                return;
            }

            using (var scope = _auditingManager.BeginScope())
            {
                try
                {
                    await next(context).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    scope.AddException(ex);
                }
                finally
                {
                    await scope.SaveAsync().ConfigureAwait(false);
                }
            }
        }

        private bool ShouldWriteAuditLog(HttpContext httpContext)
        {
            // IF selected, save audit logs on exception on GET requests even if audit log is disabled for GET requests.
            if (Options.AlwaysLogOnException &&
                string.Equals(httpContext.Request.Method, HttpMethods.Get, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

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
