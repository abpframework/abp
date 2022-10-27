using System;
using System.Collections.Generic;

namespace Volo.Abp.DependencyInjection;

[ExposeServices(typeof(ICachedServiceProvider))]
public class CachedServiceProvider : 
    CachedServiceProviderBase,
    ICachedServiceProvider,
    IScopedDependency
{
    public CachedServiceProvider(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
    }
}