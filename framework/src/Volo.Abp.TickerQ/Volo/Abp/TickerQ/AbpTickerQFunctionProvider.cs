using System;
using System.Collections.Generic;
using TickerQ.Utilities;
using TickerQ.Utilities.Enums;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.TickerQ;

public class AbpTickerQFunctionProvider : ISingletonDependency
{
    public Dictionary<string, (string, TickerTaskPriority, TickerFunctionDelegate)> Functions { get;}

    public Dictionary<string, (string, Type)> RequestTypes { get; }

    public AbpTickerQFunctionProvider()
    {
        Functions = new Dictionary<string, (string, TickerTaskPriority, TickerFunctionDelegate)>();
        RequestTypes = new Dictionary<string, (string, Type)>();
    }
}
