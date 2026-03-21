using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.BackgroundJobs;

public class DefaultDynamicBackgroundJobManager : IDynamicBackgroundJobManager, ITransientDependency
{
    private static readonly ConcurrentDictionary<Type, Func<IBackgroundJobManager, object, BackgroundJobPriority, TimeSpan?, Task<string>>> EnqueueDelegateCache = new();

    protected IBackgroundJobManager BackgroundJobManager { get; }
    protected IDynamicBackgroundJobHandlerRegistry HandlerRegistry { get; }
    protected AbpBackgroundJobOptions BackgroundJobOptions { get; }
    protected IJsonSerializer JsonSerializer { get; }
    public ILogger<DefaultDynamicBackgroundJobManager> Logger { get; set; }

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
        Logger = NullLogger<DefaultDynamicBackgroundJobManager>.Instance;
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
            return await EnqueueDynamicHandlerJobAsync(jobName, args, priority, delay);
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

        var enqueueDelegate = GetOrCreateEnqueueDelegate(argsType);
        return await enqueueDelegate(BackgroundJobManager, typedArgs, priority, delay);
    }

    protected virtual Task<string> EnqueueDynamicHandlerJobAsync(
        string jobName,
        object args,
        BackgroundJobPriority priority,
        TimeSpan? delay)
    {
        var jsonData = JsonSerializer.Serialize(args);
        var dynamicArgs = new DynamicBackgroundJobArgs(jobName, jsonData);
        return BackgroundJobManager.EnqueueAsync(dynamicArgs, priority, delay);
    }

    private static Func<IBackgroundJobManager, object, BackgroundJobPriority, TimeSpan?, Task<string>> GetOrCreateEnqueueDelegate(Type argsType)
    {
        return EnqueueDelegateCache.GetOrAdd(argsType, static type =>
        {
            var method = typeof(IBackgroundJobManager)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(m =>
                    m.Name == nameof(IBackgroundJobManager.EnqueueAsync)
                    && m.IsGenericMethodDefinition
                    && m.GetParameters() is { Length: 3 } p
                    && p[1].ParameterType == typeof(BackgroundJobPriority)
                    && p[2].ParameterType == typeof(TimeSpan?));

            if (method == null)
            {
                throw new AbpException(
                    $"Could not find the generic EnqueueAsync method on {nameof(IBackgroundJobManager)}.");
            }

            var genericMethod = method.MakeGenericMethod(type);

            // Build: (manager, args, priority, delay) => manager.EnqueueAsync<TArgs>((TArgs)args, priority, delay)
            var managerParam = Expression.Parameter(typeof(IBackgroundJobManager), "manager");
            var argsParam = Expression.Parameter(typeof(object), "args");
            var priorityParam = Expression.Parameter(typeof(BackgroundJobPriority), "priority");
            var delayParam = Expression.Parameter(typeof(TimeSpan?), "delay");

            var call = Expression.Call(
                managerParam,
                genericMethod,
                Expression.Convert(argsParam, type),
                priorityParam,
                delayParam);

            return Expression.Lambda<Func<IBackgroundJobManager, object, BackgroundJobPriority, TimeSpan?, Task<string>>>(
                call, managerParam, argsParam, priorityParam, delayParam).Compile();
        });
    }
}
