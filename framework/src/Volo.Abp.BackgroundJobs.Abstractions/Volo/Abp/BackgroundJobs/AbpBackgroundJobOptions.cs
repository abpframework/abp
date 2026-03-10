using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.BackgroundJobs;

public class AbpBackgroundJobOptions
{
    private readonly Dictionary<Type, BackgroundJobConfiguration> _jobConfigurationsByArgsType;
    private readonly ConcurrentDictionary<string, BackgroundJobConfiguration> _jobConfigurationsByName;
    private readonly ConcurrentDictionary<string, Func<string, IServiceProvider, CancellationToken, Task>> _anonymousHandlers = new();

    /// <summary>
    /// Default: true.
    /// </summary>
    public bool IsJobExecutionEnabled { get; set; } = true;

    /// <summary>
    /// The delegate to get the name of a background job.
    /// Default: <see cref="BackgroundJobNameAttribute.GetName"/>.
    /// </summary>
    public Func<Type, string> GetBackgroundJobName { get; set; }

    public AbpBackgroundJobOptions()
    {
        _jobConfigurationsByArgsType = new Dictionary<Type, BackgroundJobConfiguration>();
        _jobConfigurationsByName = new ConcurrentDictionary<string, BackgroundJobConfiguration>();
        GetBackgroundJobName = BackgroundJobNameAttribute.GetName;
    }

    public BackgroundJobConfiguration GetJob<TArgs>()
    {
        return GetJob(typeof(TArgs));
    }

    public BackgroundJobConfiguration GetJob(Type argsType)
    {
        var jobConfiguration = _jobConfigurationsByArgsType.GetOrDefault(argsType);

        if (jobConfiguration == null)
        {
            throw new AbpException("Undefined background job for the job args type: " + argsType.AssemblyQualifiedName);
        }

        return jobConfiguration;
    }

    public BackgroundJobConfiguration GetJob(string name)
    {
        var jobConfiguration = GetJobOrNull(name);

        if (jobConfiguration == null)
        {
            throw new AbpException("Undefined background job for the job name: " + name);
        }

        return jobConfiguration;
    }

    public BackgroundJobConfiguration? GetJobOrNull(string name)
    {
        return _jobConfigurationsByName.TryGetValue(name, out var config) ? config : null;
    }

    public IReadOnlyList<BackgroundJobConfiguration> GetJobs()
    {
        return _jobConfigurationsByArgsType.Values.ToImmutableList();
    }

    public void AddJob<TJob>()
    {
        AddJob(typeof(TJob));
    }

    public void AddJob(Type jobType)
    {
        AddJob(new BackgroundJobConfiguration(jobType, GetBackgroundJobName(BackgroundJobArgsHelper.GetJobArgsType(jobType))));
    }

    public void AddJob(BackgroundJobConfiguration jobConfiguration)
    {
        _jobConfigurationsByArgsType[jobConfiguration.ArgsType] = jobConfiguration;
        _jobConfigurationsByName[jobConfiguration.JobName] = jobConfiguration;
    }

    public void AddAnonymousJobHandler(string jobName, Func<string, IServiceProvider, CancellationToken, Task> handler)
    {
        Check.NotNullOrWhiteSpace(jobName, nameof(jobName));
        Check.NotNull(handler, nameof(handler));

        _anonymousHandlers[jobName] = handler;
    }

    public void AddAnonymousJobHandler(string jobName, Action<string, IServiceProvider, CancellationToken> handler)
    {
        AddAnonymousJobHandler(jobName, (jsonData, sp, ct) =>
        {
            handler(jsonData, sp, ct);
            return Task.CompletedTask;
        });
    }

    internal bool TryGetAnonymousHandler(string jobName, out Func<string, IServiceProvider, CancellationToken, Task>? handler)
    {
        return _anonymousHandlers.TryGetValue(jobName, out handler);
    }

    internal bool IsAnonymousJobRegistered(string jobName)
    {
        return _anonymousHandlers.ContainsKey(jobName);
    }
}
