using System;

namespace Volo.Abp.AspNetCore.Filters;

[AttributeUsage(AttributeTargets.Class)]
public class DisableAbpFilterAttribute : Attribute
{
    public bool SkipInMiddleware { get; set; }

    public DisableAbpFilterAttribute()
    {
        SkipInMiddleware = true;
    }

    public DisableAbpFilterAttribute(bool skipInMiddleware)
    {
        SkipInMiddleware = skipInMiddleware;
    }
}
