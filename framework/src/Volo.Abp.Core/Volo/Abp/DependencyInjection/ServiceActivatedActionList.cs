using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.DependencyInjection;

public class ServiceActivatedActionList : List<KeyValuePair<ServiceDescriptor, Action<IOnServiceActivatedContext>>>
{
    public List<Action<IOnServiceActivatedContext>> GetActions(ServiceDescriptor descriptor)
    {
        return this.Where(x => x.Key == descriptor).Select(x => x.Value).ToList();
    }
}
