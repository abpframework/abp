using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.EventBus;

/// <summary>
/// This <see cref="IEventHandlerFactory"/> implementation is used to get/release
/// handlers using Ioc.
/// </summary>
public class IocEventHandlerFactory : IEventHandlerFactory, IDisposable
{
    public Type HandlerType { get; }

    protected IServiceScopeFactory ScopeFactory { get; }

    public IocEventHandlerFactory(IServiceScopeFactory scopeFactory, Type handlerType)
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
        try
        {
            var handler = (IEventHandler)scope.ServiceProvider.GetRequiredService(HandlerType);
            return new EventHandlerDisposeWrapper(handler, () => scope.Dispose());
        }
        catch
        {
            try
            {
                scope.Dispose();
            }
            catch
            {
                // Report why the handler could not be created rather than why its scope could not be disposed.
            }

            throw;
        }
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
