using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.BackgroundJobs;

public class DefaultDynamicBackgroundJobManager : IDynamicBackgroundJobManager, ITransientDependency
{
    private static readonly ConcurrentDictionary<Type, MethodInfo> EnqueueMethodCache = new();

    protected IBackgroundJobManager BackgroundJobManager { get; }
    protected IDynamicBackgroundJobHandlerRegistry HandlerRegistry { get; }
    protected AbpBackgroundJobOptions BackgroundJobOptions { get; }
    protected IJsonSerializer JsonSerializer { get; }

    public DefaultDynamicBackgroundJobManager(
        IBackgroundJobManager backgroundJobManager,
        IDynamicBackgroundJobHandlerRegistry handlerRegistry,
        IOptions<AbpBackgroundJobOptions> backgroundJobOptions,
        IJsonSerializer jsonSerializer)
    {
        BackgroundJobManager = backgroundJobManager;
        HandlerRegistry = handlerRegistry;
        BackgroundJobOptions = backgroundJobOptions.Value;
        JsonSerializer = jsonSerializer;
    }

    public virtual async Task<string> EnqueueAsync(
        string jobName,
        object args,
        BackgroundJobPriority priority = BackgroundJobPriority.Normal,
        TimeSpan? delay = null)
    {
        Check.NotNullOrWhiteSpace(jobName, nameof(jobName));
        Check.NotNull(args, nameof(args));

        var jobConfiguration = BackgroundJobOptions.GetJobOrNull(jobName);
        if (jobConfiguration != null)
        {
            return await EnqueueTypedJobAsync(jobConfiguration, args, priority, delay);
        }

        if (HandlerRegistry.IsRegistered(jobName))
        {
            return await EnqueueAnonymousJobAsync(jobName, args, priority, delay);
        }

        throw new AbpException(
            $"No typed job configuration or dynamic handler registered for job name: {jobName}");
    }

    public virtual void RegisterHandler(
        string jobName,
        DynamicBackgroundJobHandler handler)
    {
        HandlerRegistry.Register(jobName, handler);
    }

    public virtual bool UnregisterHandler(string jobName)
    {
        return HandlerRegistry.Unregister(jobName);
    }

    public virtual bool IsHandlerRegistered(string jobName)
    {
        return HandlerRegistry.IsRegistered(jobName);
    }

    protected virtual async Task<string> EnqueueTypedJobAsync(
        BackgroundJobConfiguration jobConfiguration,
        object args,
        BackgroundJobPriority priority,
        TimeSpan? delay)
    {
        var argsType = jobConfiguration.ArgsType;

        // Normalize args to the expected type via JSON round-trip
        var json = JsonSerializer.Serialize(args);
        var typedArgs = JsonSerializer.Deserialize(argsType, json);

        var enqueueMethod = GetOrCreateEnqueueMethod(argsType);
        var task = (Task<string>)enqueueMethod.Invoke(BackgroundJobManager, [typedArgs, priority, delay])!;
        return await task;
    }

    protected virtual Task<string> EnqueueAnonymousJobAsync(
        string jobName,
        object args,
        BackgroundJobPriority priority,
        TimeSpan? delay)
    {
        var jsonData = JsonSerializer.Serialize(args);
        var anonymousArgs = new AnonymousJobArgs(jobName, jsonData);
        return BackgroundJobManager.EnqueueAsync(anonymousArgs, priority, delay);
    }

    private static MethodInfo GetOrCreateEnqueueMethod(Type argsType)
    {
        return EnqueueMethodCache.GetOrAdd(argsType, static type =>
        {
            var method = typeof(IBackgroundJobManager).GetMethod(
                nameof(IBackgroundJobManager.EnqueueAsync),
                BindingFlags.Public | BindingFlags.Instance);

            return method!.MakeGenericMethod(type);
        });
    }
}
