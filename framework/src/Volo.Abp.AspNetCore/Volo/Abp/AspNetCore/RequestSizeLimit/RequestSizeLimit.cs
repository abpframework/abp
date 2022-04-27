using System;

namespace Volo.Abp.AspNetCore.RequestSizeLimit;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class AbpRequestSizeLimitAttribute : Attribute
{
    private readonly long _bytes;

    public AbpRequestSizeLimitAttribute(long bytes)
    {
        _bytes = bytes;
    }

    public long GetBytes()
    {
        return _bytes;
    }
}
