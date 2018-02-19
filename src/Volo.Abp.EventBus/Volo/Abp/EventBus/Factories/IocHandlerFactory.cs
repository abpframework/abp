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
        /// <summary>
        /// Type of the handler.
        /// </summary>
        public Type HandlerType { get; }

        private readonly IServiceProvider _iocResolver;

        /// <summary>
        /// Creates a new instance of <see cref="IocHandlerFactory"/> class.
        /// </summary>
        /// <param name="iocResolver"></param>
        /// <param name="handlerType">Type of the handler</param>
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

        /// <summary>
        /// Releases handler object using Ioc container.
        /// </summary>
        /// <param name="handler">Handler to be released</param>
        public void ReleaseHandler(IEventHandler handler)
        {
            //TODO: Scope!!!
            //_iocResolver.Release(handler);
        }
    }
}