using System;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.BackgroundJobs;

public class MyJobArgs : IMultiTenant
{
    public string Value { get; set; }

    public MyJobArgs()
    {

    }
    

    public MyJobArgs(string value, Guid? tenantId = null)
    {
        Value = value;
        TenantId = tenantId;
    }

    public Guid? TenantId { get; }
}
