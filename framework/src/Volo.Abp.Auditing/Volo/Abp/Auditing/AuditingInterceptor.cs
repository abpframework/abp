using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Auditing
{
    public class AuditingInterceptor : AbpInterceptor, ITransientDependency
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AuditingInterceptor(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            if (!ShouldIntercept(invocation, out var auditLog, out var auditLogAction))
            {
                await invocation.ProceedAsync();
                return;
            }

            var stopwatch = Stopwatch.StartNew();

            try
            {
                await invocation.ProceedAsync();
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

        protected virtual bool ShouldIntercept(
            IAbpMethodInvocation invocation,
            out AuditLogInfo auditLog,
            out AuditLogActionInfo auditLogAction)
        {
            auditLog = null;
            auditLogAction = null;

            if (AbpCrossCuttingConcerns.IsApplied(invocation.TargetObject, AbpCrossCuttingConcerns.Auditing))
            {
                return false;
            }

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var auditingManager = scope.ServiceProvider.GetRequiredService<IAuditingManager>();
                var auditLogScope = auditingManager.Current;
                if (auditLogScope == null)
                {
                    return false;
                }

                var auditingHelper = scope.ServiceProvider.GetRequiredService<IAuditingHelper>();
                if (!auditingHelper.ShouldSaveAudit(invocation.Method))
                {
                    return false;
                }

                auditLog = auditLogScope.Log;
                auditLogAction = auditingHelper.CreateAuditLogAction(
                    auditLog,
                    invocation.TargetObject.GetType(),
                    invocation.Method,
                    invocation.Arguments
                );

                return true;
            }
        }
    }
}
