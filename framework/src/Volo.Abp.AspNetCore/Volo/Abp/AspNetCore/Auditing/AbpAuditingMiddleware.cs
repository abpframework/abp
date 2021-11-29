using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Volo.Abp.Users;

namespace Volo.Abp.AspNetCore.Auditing
{
    public class AbpAuditingMiddleware : IMiddleware, ITransientDependency
    {
        private readonly IAuditingManager _auditingManager;
        protected AbpAuditingOptions AuditingOptions { get; }
        protected AbpAspNetCoreAuditingOptions AspNetCoreAuditingOptions { get; }
        protected ICurrentUser CurrentUser { get; }
        protected IUnitOfWorkManager UnitOfWorkManager { get; }

        public AbpAuditingMiddleware(
            IAuditingManager auditingManager,
            ICurrentUser currentUser,
            IOptions<AbpAuditingOptions> auditingOptions,
            IOptions<AbpAspNetCoreAuditingOptions> aspNetCoreAuditingOptions,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _auditingManager = auditingManager;

            CurrentUser = currentUser;
            UnitOfWorkManager = unitOfWorkManager;
            AuditingOptions = auditingOptions.Value;
            AspNetCoreAuditingOptions = aspNetCoreAuditingOptions.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!AuditingOptions.IsEnabled || IsIgnoredUrl(context))
            {
                await next(context);
                return;
            }

            var hasError = false;
            using (var saveHandle = _auditingManager.BeginScope())
            {
                Debug.Assert(_auditingManager.Current != null);

                try
                {
                    await next(context);

                    if (_auditingManager.Current.Log.Exceptions.Any())
                    {
                        hasError = true;
                    }
                }
                catch (Exception ex)
                {
                    hasError = true;

                    if (!_auditingManager.Current.Log.Exceptions.Contains(ex))
                    {
                        _auditingManager.Current.Log.Exceptions.Add(ex);
                    }

                    throw;
                }
                finally
                {
                    if (ShouldWriteAuditLog(context, hasError))
                    {
                        if (UnitOfWorkManager.Current != null)
                        {
                            await UnitOfWorkManager.Current.SaveChangesAsync();
                        }

                        await saveHandle.SaveAsync();
                    }
                }
            }
        }

        private bool IsIgnoredUrl(HttpContext context)
        {
            return context.Request.Path.Value != null &&
                   AspNetCoreAuditingOptions.IgnoredUrls.Any(x => context.Request.Path.Value.StartsWith(x));
        }

        private bool ShouldWriteAuditLog(HttpContext httpContext, bool hasError)
        {
            if (AuditingOptions.AlwaysLogOnException && hasError)
            {
                return true;
            }

            if (!AuditingOptions.IsEnabledForAnonymousUsers && !CurrentUser.IsAuthenticated)
            {
                return false;
            }

            if (!AuditingOptions.IsEnabledForGetRequests &&
                string.Equals(httpContext.Request.Method, HttpMethods.Get, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}
