using System;
using Volo.Abp.Collections;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.DependencyInjection
{
    public interface IOnServiceRegisteredContext
    {
        ITypeList<IAbpInterceptor> Interceptors { get; }

        Type ImplementationType { get; }
    }
}