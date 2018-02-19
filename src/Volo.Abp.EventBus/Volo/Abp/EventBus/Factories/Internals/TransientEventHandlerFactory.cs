using System;
using Volo.Abp.EventBus.Handlers;

namespace Volo.Abp.EventBus.Factories.Internals
{
    /// <summary>
    /// This <see cref="IEventHandlerFactory"/> implementation is used to handle events
    /// by a transient instance object. 
    /// </summary>
    /// <remarks>
    /// This class always creates a new transient instance of handler.
    /// </remarks>
    internal class TransientEventHandlerFactory<THandler> : IEventHandlerFactory 
        where THandler : IEventHandler, new()
    {
        /// <summary>
        /// Creates a new instance of the handler object.
        /// </summary>
        /// <returns>The handler object</returns>
        public IEventHandler GetHandler()
        {
            return new THandler();
        }

        public Type GetHandlerType()
        {
            return typeof(THandler);
        }

        /// <summary>
        /// Disposes the handler object if it's <see cref="IDisposable"/>. Does nothing if it's not.
        /// </summary>
        /// <param name="handler">Handler to be released</param>
        public void ReleaseHandler(IEventHandler handler)
        {
            if (handler is IDisposable)
            {
                (handler as IDisposable).Dispose();
            }
        }
    }
}