using System;

namespace Volo.Abp.DependencyInjection
{
    public interface IAbpLazyServiceProvider
    {
        T LazyGetRequiredService<T>();

        object LazyGetRequiredService(Type serviceType);

        T LazyGetService<T>();

        object LazyGetService(Type serviceType);

        T LazyGetService<T>(T defaultValue);

        object LazyGetService(Type serviceType, object defaultValue);

        object LazyGetService(Type serviceType, Func<IServiceProvider, object> factory);

        T LazyGetService<T>(Func<IServiceProvider, object> factory);
    }
}
