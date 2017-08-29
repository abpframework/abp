using System;
using Volo.Abp.Collections;
using Volo.Abp.DynamicProxy;

namespace Microsoft.Extensions.DependencyInjection
{
    public interface IOnServiceRegistredContext
    {
        ITypeList<IAbpInterceptor> Interceptors { get; }

        Type ImplementationType { get; }
    }
}