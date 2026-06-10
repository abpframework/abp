using System;
using System.Collections.Generic;
using TickerQ.Utilities;
using TickerQ.Utilities.Enums;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.TickerQ;

public class AbpTickerQFunctionProvider : ISingletonDependency
{
    public Dictionary<string, (string CronExpression, TickerTaskPriority Priority, TickerFunctionDelegate Function, int MaxConcurrency)> Functions { get; }

    public Dictionary<string, (string TypeName, Type Type)> RequestTypes { get; }

    public AbpTickerQFunctionProvider()
    {
        Functions = new Dictionary<string, (string, TickerTaskPriority, TickerFunctionDelegate, int)>();
        RequestTypes = new Dictionary<string, (string, Type)>();
    }

    public void AddFunction(
        string name,
        TickerFunctionDelegate function,
        TickerTaskPriority priority = TickerTaskPriority.Normal,
        int maxConcurrency = 0)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));
        Check.NotNull(function, nameof(function));

        if (maxConcurrency < 0)
        {
            throw new ArgumentException("maxConcurrency must be greater than or equal to 0.", nameof(maxConcurrency));
        }

        if (!Functions.TryAdd(name, (string.Empty, priority, function, maxConcurrency)))
        {
            throw new AbpException($"A function with the name '{name}' is already registered.");
        }
    }
}
