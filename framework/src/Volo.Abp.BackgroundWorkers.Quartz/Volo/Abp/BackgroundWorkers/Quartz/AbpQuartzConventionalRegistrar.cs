using System;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundWorkers.Quartz;

public class AbpQuartzConventionalRegistrar : DefaultConventionalRegistrar
{
    protected override bool IsConventionalRegistrationDisabled(Type type)
    {
        return !typeof(IQuartzBackgroundWorker).IsAssignableFrom(type) || base.IsConventionalRegistrationDisabled(type);
    }

    protected override List<Type> GetExposedServiceTypes(Type type)
    {
        return new List<Type>()
            {
                typeof(IQuartzBackgroundWorker)
            };
    }
}
