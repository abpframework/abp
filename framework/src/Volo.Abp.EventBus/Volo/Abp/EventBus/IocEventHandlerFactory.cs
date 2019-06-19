using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EventBus
{
    /// <summary>
    /// This <see cref="IEventHandlerFactory"/> implementation is used to get/release
    /// handlers using Ioc.
    /// </summary>
    public class IocEventHandlerFactory : IEventHandlerFactory, IDisposable
    {
        public Type HandlerType { get; }

        protected IHybridServiceScopeFactory ScopeFactory { get; }

        public IocEventHandlerFactory(IHybridServiceScopeFactory scopeFactory, Type handlerType)
        {
            ScopeFactory = scopeFactory;
            HandlerType = handlerType;
        }

        /// <summary>
        /// Resolves handler object from Ioc container.
        /// </summary>
        /// <returns>Resolved handler object</returns>
        public IEventHandlerDisposeWrapper GetHandler()
        {
            var scope = ScopeFactory.CreateScope();
            return new EventHandlerDisposeWrapper(
                (IEventHandler) scope.ServiceProvider.GetRequiredService(HandlerType),
                () => scope.Dispose()
            );
        }

        public bool IsInFactories(List<IEventHandlerFactory> handlerFactories)
        {
            return handlerFactories
                .OfType<IocEventHandlerFactory>()
                .Any(f => f.HandlerType == HandlerType);
        }

        public void Dispose()
        {

        }
    }
}