using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.SignalR;

namespace Volo.Abp.AspNetCore.SignalR;

public class AbpHubContext
{
    public IServiceProvider ServiceProvider { get; }

    public Hub Hub { get; }

    public MethodInfo HubMethod { get; }

    public IReadOnlyList<object> HubMethodArguments { get; }

    public AbpHubContext(IServiceProvider serviceProvider, Hub hub, MethodInfo hubMethod, IReadOnlyList<object> hubMethodArguments)
    {
        ServiceProvider = serviceProvider;
        Hub = hub;
        HubMethod = hubMethod;
        HubMethodArguments = hubMethodArguments;
    }
}
