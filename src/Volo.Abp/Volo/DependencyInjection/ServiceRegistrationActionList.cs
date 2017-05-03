using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.DependencyInjection
{
    public class ServiceRegistrationActionList : List<Action<IOnServiceRegistredArgs>>
    {
        
    }
}