using System;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.EventBus
{
    /// <summary>
    /// This <see cref="IEventHandlerFactory"/> implementation is used to get/release
    /// handlers using Ioc.
    /// </summary>
    public class IocEventHandlerFactory : IEventHandlerFactory
    {
        public Type HandlerType { get; }

        private readonly IServiceProvider _serviceProvider;

        public IocEventHandlerFactory(IServiceProvider serviceProvider, Type handlerType)
        {
            _serviceProvider = serviceProvider;
            HandlerType = handlerType;
        }

        /// <summary>
        /// Resolves handler object from Ioc container.
        /// </summary>
        /// <returns>Resolved handler object</returns>
        public IEventHandlerDisposeWrapper GetHandler()
        {
            var scope = _serviceProvider.CreateScope();
            return new EventHandlerDisposeWrapper(
                (IEventHandler) scope.ServiceProvider.GetRequiredService(HandlerType),
                () => scope.Dispose()
            );
        }

        public Type GetHandlerType()
        {
            return HandlerType;
        }
    }
}