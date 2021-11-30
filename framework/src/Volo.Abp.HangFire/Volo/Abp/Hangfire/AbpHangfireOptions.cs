using System;
using System.Collections.Generic;
using Hangfire;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.States;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Hangfire;

public class AbpHangfireOptions
{
    [CanBeNull]
    public BackgroundJobServerOptions ServerOptions { get; set; }

    [CanBeNull]
    public IEnumerable<IBackgroundProcess> AdditionalProcesses { get; set; }

    [CanBeNull]
    public JobStorage Storage { get; set; }

    [NotNull]
    public Func<IServiceProvider, BackgroundJobServer> BackgroundJobServerFactory {
        get => _backgroundJobServerFactory;
        set => _backgroundJobServerFactory = Check.NotNull(value, nameof(value));
    }
    private Func<IServiceProvider, BackgroundJobServer> _backgroundJobServerFactory;

    public AbpHangfireOptions()
    {
        _backgroundJobServerFactory = CreateJobServer;
    }

    private BackgroundJobServer CreateJobServer(IServiceProvider serviceProvider)
    {
        Storage = Storage ?? serviceProvider.GetRequiredService<JobStorage>();
        ServerOptions = ServerOptions ?? serviceProvider.GetService<BackgroundJobServerOptions>() ?? new BackgroundJobServerOptions();
        AdditionalProcesses = AdditionalProcesses ?? serviceProvider.GetServices<IBackgroundProcess>();

        return new BackgroundJobServer(ServerOptions, Storage, AdditionalProcesses,
            ServerOptions.FilterProvider ?? serviceProvider.GetRequiredService<IJobFilterProvider>(),
            ServerOptions.Activator ?? serviceProvider.GetRequiredService<JobActivator>(),
            serviceProvider.GetService<IBackgroundJobFactory>(),
            serviceProvider.GetService<IBackgroundJobPerformer>(),
            serviceProvider.GetService<IBackgroundJobStateChanger>()
        );
    }
}
