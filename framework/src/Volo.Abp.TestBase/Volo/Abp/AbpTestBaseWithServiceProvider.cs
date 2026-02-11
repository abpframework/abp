using System;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp;

public abstract class AbpTestBaseWithServiceProvider
{
    protected IServiceProvider ServiceProvider { get; set; } = default!;

    protected virtual T? GetService<T>()
    {
        return ServiceProvider.GetService<T>();
    }

    protected virtual T GetRequiredService<T>() where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }

    protected virtual T? GetKeyedServices<T>(object? serviceKey)
    {
        return ServiceProvider.GetKeyedService<T>(serviceKey);
    }

    protected virtual T GetRequiredKeyedService<T>(object? serviceKey) where T : notnull
    {
        return ServiceProvider.GetRequiredKeyedService<T>(serviceKey);
    }
}
