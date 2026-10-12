using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BlobStoring.Fakes;

/// <summary>
/// Records the ambient tenant at the moment the owning scope disposes it, so tests
/// can verify the scope is released in the tenant of the BLOB operation.
/// </summary>
public class FakeTenantRecordingScopedService : IScopedDependency, IDisposable
{
    public static Guid? LastDisposeTenantId { get; private set; }

    public static bool HasRecordedDispose { get; private set; }

    private readonly ICurrentTenant _currentTenant;

    public FakeTenantRecordingScopedService(ICurrentTenant currentTenant)
    {
        _currentTenant = currentTenant;
    }

    public static void Reset()
    {
        LastDisposeTenantId = null;
        HasRecordedDispose = false;
    }

    public void Dispose()
    {
        LastDisposeTenantId = _currentTenant.Id;
        HasRecordedDispose = true;
    }
}
