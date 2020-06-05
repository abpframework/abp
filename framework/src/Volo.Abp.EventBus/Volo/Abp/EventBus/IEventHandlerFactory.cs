using System.Collections.Generic;

namespace Volo.Abp.EventBus
{
    /// <summary>
    /// Defines an interface for factories those are responsible to create/get and release of event handlers.
    /// </summary>
    public interface IEventHandlerFactory
    {
        /// <summary>
        /// Gets an event handler.
        /// </summary>
        /// <returns>The event handler</returns>
        IEventHandlerDisposeWrapper GetHandler();

        bool IsInFactories(List<IEventHandlerFactory> handlerFactories);
    }
}