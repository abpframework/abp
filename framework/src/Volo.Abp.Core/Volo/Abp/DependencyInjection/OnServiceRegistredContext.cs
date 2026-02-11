using System;
using JetBrains.Annotations;
using Volo.Abp.Collections;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.DependencyInjection;

public class OnServiceRegistredContext : IOnServiceRegistredContext
{
    public virtual ITypeList<IAbpInterceptor> Interceptors { get; }

    public virtual Type ServiceType { get; }

    public virtual Type ImplementationType { get; }

    public virtual object? ServiceKey { get; }

    public OnServiceRegistredContext(Type serviceType, [NotNull] Type implementationType, object? serviceKey = null)
    {
        ServiceType = Check.NotNull(serviceType, nameof(serviceType));
        ImplementationType = Check.NotNull(implementationType, nameof(implementationType));
        ServiceKey = serviceKey;

        Interceptors = new TypeList<IAbpInterceptor>();
    }
}
