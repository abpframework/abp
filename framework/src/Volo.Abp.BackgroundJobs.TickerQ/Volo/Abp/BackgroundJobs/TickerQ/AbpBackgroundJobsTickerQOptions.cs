using System;
using System.Collections.Generic;

namespace Volo.Abp.BackgroundJobs.TickerQ;

public class AbpBackgroundJobsTickerQOptions
{
    private readonly Dictionary<Type, AbpBackgroundJobsTimeTickerConfiguration> _configurations;

    public AbpBackgroundJobsTickerQOptions()
    {
        _configurations = new Dictionary<Type, AbpBackgroundJobsTimeTickerConfiguration>();
    }

    public void AddConfiguration<TJob>(AbpBackgroundJobsTimeTickerConfiguration configuration)
    {
        AddConfiguration(typeof(TJob), configuration);
    }

    public void AddConfiguration(Type jobType, AbpBackgroundJobsTimeTickerConfiguration configuration)
    {
        _configurations[jobType] = configuration;
    }

    public AbpBackgroundJobsTimeTickerConfiguration? GetConfigurationOrNull<TJob>()
    {
        return GetConfigurationOrNull(typeof(TJob));
    }

    public AbpBackgroundJobsTimeTickerConfiguration? GetConfigurationOrNull(Type jobType)
    {
        return _configurations.GetValueOrDefault(jobType);
    }
}
