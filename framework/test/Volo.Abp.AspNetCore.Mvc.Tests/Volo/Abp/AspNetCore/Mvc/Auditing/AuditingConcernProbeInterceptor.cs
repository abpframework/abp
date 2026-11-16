using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Aspects;
using Volo.Abp.DependencyInjection;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.AspNetCore.Mvc.Auditing;

public class AuditingConcernProbe : ISingletonDependency
{
    public List<bool> Results { get; } = new();
}

/// <summary>
/// Completes asynchronously before proceeding, and records whether the action's auditing scope is still active.
/// </summary>
public class AuditingConcernProbeInterceptor : AbpInterceptor, ITransientDependency
{
    private readonly AuditingConcernProbe _probe;

    public AuditingConcernProbeInterceptor(AuditingConcernProbe probe)
    {
        _probe = probe;
    }

    public override async Task InterceptAsync(IAbpMethodInvocation invocation)
    {
        await Task.Delay(50);
        _probe.Results.Add(AbpCrossCuttingConcerns.IsApplied(invocation.TargetObject, AbpCrossCuttingConcerns.Auditing));
        await invocation.ProceedAsync();
    }
}
