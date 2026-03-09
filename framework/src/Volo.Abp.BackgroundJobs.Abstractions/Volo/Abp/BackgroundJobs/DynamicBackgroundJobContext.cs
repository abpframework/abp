using System;
using System.Collections.Generic;
using System.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundJobs;

public class DynamicBackgroundJobContext : IServiceProviderAccessor
{
    public IServiceProvider ServiceProvider { get; }

    public Dictionary<string, object> Args { get; }

    public CancellationToken CancellationToken { get; }

    public DynamicBackgroundJobContext(
        IServiceProvider serviceProvider,
        Dictionary<string, object> args,
        CancellationToken cancellationToken = default)
    {
        ServiceProvider = serviceProvider;
        Args = args;
        CancellationToken = cancellationToken;
    }
}
