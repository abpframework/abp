using System;
using Volo;
using Volo.Abp.Collections;
using Volo.Abp.DynamicProxy;

namespace Microsoft.Extensions.DependencyInjection
{
    public class OnServiceRegistredArgs : IOnServiceRegistredArgs
    {
        public virtual ITypeList<IAbpInterceptor> Interceptors { get; }

        public virtual Type ImplementationType { get; }

        public OnServiceRegistredArgs(Type implementationType)
        {
            ImplementationType = Check.NotNull(implementationType, nameof(implementationType));

            Interceptors = new TypeList<IAbpInterceptor>();
        }
    }
}