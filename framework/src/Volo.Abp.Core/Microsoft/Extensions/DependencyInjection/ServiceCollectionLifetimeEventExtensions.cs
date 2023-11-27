using System;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionLifetimeEventExtensions
{
    // OnActivated
    public static void OnActivated(this IServiceCollection services, ServiceDescriptor descriptor, Action<IOnServiceActivatedContext> onActivatedAction)
    {
        GetOrCreateOnActivatedActionList(services).Add(new KeyValuePair<ServiceDescriptor, Action<IOnServiceActivatedContext>>(descriptor, onActivatedAction));
    }

    public static ServiceActivatedActionList GetServiceActivatedActionList(this IServiceCollection services)
    {
        return GetOrCreateOnActivatedActionList(services);
    }

    private static ServiceActivatedActionList GetOrCreateOnActivatedActionList(IServiceCollection services)
    {
        var actionList = services.GetSingletonInstanceOrNull<IObjectAccessor<ServiceActivatedActionList>>()?.Value;
        if (actionList == null)
        {
            actionList = new ServiceActivatedActionList();
            services.AddObjectAccessor(actionList);
        }

        return actionList;
    }
}
