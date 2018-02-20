using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EventBus.Handlers;

namespace Volo.Abp.EventBus.Factories
{
    /// <summary>
    /// This <see cref="IEventHandlerFactory"/> implementation is used to get/release
    /// handlers using Ioc.
    /// </summary>
    public class IocHandlerFactory : IEventHandlerFactory
    {
        public Type HandlerType { get; }

        private readonly IServiceProvider _iocResolver;

        public IocHandlerFactory(IServiceProvider iocResolver, Type handlerType)
        {
            _iocResolver = iocResolver;
            HandlerType = handlerType;
        }

        /// <summary>
        /// Resolves handler object from Ioc container.
        /// </summary>
        /// <returns>Resolved handler object</returns>
        public IEventHandler GetHandler()
        {
            return (IEventHandler)_iocResolver.GetRequiredService(HandlerType);
        }

        public Type GetHandlerType()
        {
            return HandlerType;
        }

        public void ReleaseHandler(IEventHandler handler)
        {
            //TODO: Scope!!!
            //_iocResolver.Release(handler);
        }
    }
}