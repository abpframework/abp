using System;
using System.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.SignalR;

public class DefaultAbpHubContextAccessor : IAbpHubContextAccessor, ISingletonDependency
{
    public AbpHubContext Context => _currentHubContext.Value;

    private readonly AsyncLocal<AbpHubContext> _currentHubContext = new AsyncLocal<AbpHubContext>();

    public virtual IDisposable Change(AbpHubContext context)
    {
        var parent = Context;
        _currentHubContext.Value = context;
        return new DisposeAction(() =>
        {
            _currentHubContext.Value = parent;
        });
    }
}
