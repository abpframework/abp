using System;
using System.Collections.Generic;

namespace Volo.Abp.DependencyInjection
{
    public class ServiceRegistrationActionList : List<Action<IOnServiceRegisteredContext>>
    {
        
    }
}