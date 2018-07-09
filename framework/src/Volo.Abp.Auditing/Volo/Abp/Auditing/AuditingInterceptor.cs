using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.Auditing
{
    public class AuditingInterceptor : AbpInterceptor, ITransientDependency
    {
        private readonly IAuditingHelper _auditingHelper;

        public AuditingInterceptor(IAuditingHelper auditingHelper)
        {
            _auditingHelper = auditingHelper;
        }

        public override void Intercept(IAbpMethodInvocation invocation)
        {
            if (AbpCrossCuttingConcerns.IsApplied(invocation.TargetObject, AbpCrossCuttingConcerns.Auditing))
            {
                invocation.Proceed();
                return;
            }

            if (!_auditingHelper.ShouldSaveAudit(invocation.Method))
            {
                invocation.Proceed();
                return;
            }

            var auditInfo = _auditingHelper.CreateAuditInfo(invocation.TargetObject.GetType(), invocation.Method, invocation.Arguments);

            var stopwatch = Stopwatch.StartNew();

            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                auditInfo.Exception = ex;
                throw;
            }
            finally
            {
                stopwatch.Stop();
                auditInfo.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
                auditInfo.ServiceName = "AuditingInterceptor->" + auditInfo.ServiceName;
                _auditingHelper.Save(auditInfo);
            }
        }

        public override async Task InterceptAsync(IAbpMethodInvocation invocation)
        {
            //Try to reduce duplication with Intercept

            if (AbpCrossCuttingConcerns.IsApplied(invocation.TargetObject, AbpCrossCuttingConcerns.Auditing))
            {
                await invocation.ProceedAsync();
                return;
            }

            if (!_auditingHelper.ShouldSaveAudit(invocation.Method))
            {
                await invocation.ProceedAsync();
                return;
            }

            var auditInfo = _auditingHelper.CreateAuditInfo(invocation.TargetObject.GetType(), invocation.Method, invocation.Arguments);

            var stopwatch = Stopwatch.StartNew();

            try
            {
                await invocation.ProceedAsync();
            }
            catch (Exception ex)
            {
                auditInfo.Exception = ex;
            }
            finally
            {
                stopwatch.Stop();
                auditInfo.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
                auditInfo.ServiceName = "AuditingInterceptor->" + auditInfo.ServiceName;
                await _auditingHelper.SaveAsync(auditInfo);
            }
        }
    }
}
