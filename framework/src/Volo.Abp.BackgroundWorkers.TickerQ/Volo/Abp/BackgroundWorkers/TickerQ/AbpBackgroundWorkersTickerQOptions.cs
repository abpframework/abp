using System;
using System.Collections.Generic;

namespace Volo.Abp.BackgroundWorkers.TickerQ;

public class AbpBackgroundWorkersTickerQOptions
{
    private readonly Dictionary<Type, AbpBackgroundWorkersCronTickerConfiguration> _configurations;

    public AbpBackgroundWorkersTickerQOptions()
    {
        _configurations = new Dictionary<Type, AbpBackgroundWorkersCronTickerConfiguration>();
    }

    public void AddConfiguration<TWorker>(AbpBackgroundWorkersCronTickerConfiguration configuration)
    {
        AddConfiguration(typeof(TWorker), configuration);
    }

    public void AddConfiguration(Type workerType, AbpBackgroundWorkersCronTickerConfiguration configuration)
    {
        _configurations[workerType] = configuration;
    }

    public AbpBackgroundWorkersCronTickerConfiguration? GetConfigurationOrNull<TJob>()
    {
        return GetConfigurationOrNull(typeof(TJob));
    }

    public AbpBackgroundWorkersCronTickerConfiguration? GetConfigurationOrNull(Type workerType)
    {
        return _configurations.GetValueOrDefault(workerType);
    }
}
