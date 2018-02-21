using System;
using Volo.Abp.DynamicProxy;

namespace Volo.Abp.EventBus
{
    /// <summary>
    /// This <see cref="IEventHandlerFactory"/> implementation is used to handle events
    /// by a single instance object. 
    /// </summary>
    /// <remarks>
    /// This class always gets the same single instance of handler.
    /// </remarks>
    internal class SingleInstanceHandlerFactory : IEventHandlerFactory
    {
        /// <summary>
        /// The event handler instance.
        /// </summary>
        public IEventHandler HandlerInstance { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        public SingleInstanceHandlerFactory(IEventHandler handler)
        {
            HandlerInstance = handler;
        }

        public IEventHandlerDisposeWrapper GetHandler()
        {
            return new EventHandlerDisposeWrapper(HandlerInstance);
        }

        public Type GetHandlerType()
        {
            return ProxyHelper.UnProxy(HandlerInstance).GetType();
        }
    }
}