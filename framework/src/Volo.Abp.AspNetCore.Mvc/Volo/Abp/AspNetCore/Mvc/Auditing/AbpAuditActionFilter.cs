using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Volo.Abp.Aspects;
using Volo.Abp.Auditing;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.Auditing
{
    public class AbpAuditActionFilter : IAsyncActionFilter, ITransientDependency
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!ShouldSaveAudit(context, out var auditLog, out var auditLogAction))
            {
                await next();
                return;
            }

            using (AbpCrossCuttingConcerns.Applying(context.Controller, AbpCrossCuttingConcerns.Auditing))
            {
                var stopwatch = Stopwatch.StartNew();

                try
                {
                    var result = await next();

                    if (result.Exception != null && !result.ExceptionHandled)
                    {
                        auditLog.Exceptions.Add(result.Exception);
                    }
                }
                catch (Exception ex)
                {
                    auditLog.Exceptions.Add(ex);
                    throw;
                }
                finally
                {
                    stopwatch.Stop();
                    auditLogAction.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
                    auditLog.Actions.Add(auditLogAction);
                }
            }
        }

        private bool ShouldSaveAudit(ActionExecutingContext context, out AuditLogInfo auditLog, out AuditLogActionInfo auditLogAction)
        {
            auditLog = null;
            auditLogAction = null;

            var options = context.GetRequiredService<IOptions<AbpAuditingOptions>>().Value;
            if (!options.IsEnabled)
            {
                return false;
            }

            if (!context.ActionDescriptor.IsControllerAction())
            {
                return false;
            }

            var auditLogScope = context.GetRequiredService<IAuditingManager>().Current;
            if (auditLogScope == null)
            {
                return false;
            }

            var auditingHelper = context.GetRequiredService<IAuditingHelper>();
            if (!auditingHelper.ShouldSaveAudit(context.ActionDescriptor.GetMethodInfo(), true))
            {
                return false;
            }

            auditLog = auditLogScope.Log;
            auditLogAction = auditingHelper.CreateAuditLogAction(
                auditLog,
                context.ActionDescriptor.AsControllerActionDescriptor().ControllerTypeInfo.AsType(),
                context.ActionDescriptor.AsControllerActionDescriptor().MethodInfo,
                context.ActionArguments
            );

            return true;
        }
    }
}
