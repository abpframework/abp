using System;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.EventBus
{
    /// <summary>
    /// This <see cref="IEventHandlerFactory"/> implementation is used to get/release
    /// handlers using Ioc.
    /// </summary>
    public class IocEventHandlerFactory : IEventHandlerFactory, IDisposable
    {
        public Type HandlerType { get; }

        protected IServiceScope ServiceScope { get; }

        //TODO: Consider to inject IServiceScopeFactory instead
        public IocEventHandlerFactory(IServiceProvider serviceProvider, Type handlerType)
        {
            HandlerType = handlerType;
            ServiceScope = serviceProvider
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
        }

        /// <summary>
        /// Resolves handler object from Ioc container.
        /// </summary>
        /// <returns>Resolved handler object</returns>
        public IEventHandlerDisposeWrapper GetHandler()
        {
            var scope = ServiceScope.ServiceProvider.CreateScope();
            return new EventHandlerDisposeWrapper(
                (IEventHandler) scope.ServiceProvider.GetRequiredService(HandlerType),
                () => scope.Dispose()
            );
        }

        public void Dispose()
        {
            ServiceScope.Dispose();
        }
    }
}