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
    /// <summary>
    /// This value is used to add prefix to all of the queues. Default is empty.
    /// </summary>
    public string DefaultQueuePrefix { get; set; } = string.Empty;

    /// <summary>
    /// Hangfire queue name max length, default is 50.
    /// </summary>
    public int MaxQueueNameLength { get; set; } = 50;

    public string DefaultQueue { get; set; } = EnqueuedState.DefaultQueue;

    public BackgroundJobServerOptions? ServerOptions { get; set; }

    public IEnumerable<IBackgroundProcess>? AdditionalProcesses { get; set; }

    public JobStorage? Storage { get; set; }

    [NotNull]
    public Func<IServiceProvider, BackgroundJobServer?> BackgroundJobServerFactory {
        get => _backgroundJobServerFactory;
        set => _backgroundJobServerFactory = Check.NotNull(value, nameof(value));
    }
    private Func<IServiceProvider, BackgroundJobServer?> _backgroundJobServerFactory;

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
