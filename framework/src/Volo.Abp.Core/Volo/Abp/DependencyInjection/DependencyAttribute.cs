using System;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.DependencyInjection;

public class DependencyAttribute : Attribute
{
    public virtual ServiceLifetime? Lifetime { get; set; }

    public virtual bool TryRegister { get; set; }

    public virtual bool ReplaceServices { get; set; }

    public virtual bool IsKeyedService { get; set; }
    
    public virtual object? ServiceKey { get; set; }

    public DependencyAttribute()
    {

    }

    public DependencyAttribute(ServiceLifetime lifetime)
    {
        Lifetime = lifetime;
    }

    public DependencyAttribute(bool isKeyedService, object? serviceKey)
    {
        IsKeyedService = isKeyedService;
        ServiceKey = serviceKey;
    }

    public DependencyAttribute(bool isKeyedService, object? serviceKey, ServiceLifetime lifetime)
    {
        IsKeyedService = isKeyedService;
        ServiceKey = serviceKey;
        Lifetime = lifetime;
    }
}
