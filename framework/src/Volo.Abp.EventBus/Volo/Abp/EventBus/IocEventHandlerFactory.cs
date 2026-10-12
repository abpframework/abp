using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.EventBus;

/// <summary>
/// This <see cref="IEventHandlerFactory"/> implementation is used to get/release
/// handlers using Ioc.
/// </summary>
public class IocEventHandlerFactory : IEventHandlerFactory, IDisposable
{
    public Type HandlerType { get; }

    protected IServiceScopeFactory ScopeFactory { get; }

    private StrongBox<Type?>? _implementationType;

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

            var implementationType = Volatile.Read(ref _implementationType)?.Value;
            if (implementationType != null && implementationType != handler.GetType())
            {
                // The container created another type than its registration told, so it is not asked again.
                Volatile.Write(ref _implementationType, new StrongBox<Type?>(null));
            }

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

    /// <summary>
    /// Gets the type of the handlers this factory creates when the container can tell it without creating one,
    /// otherwise null. The container is asked once.
    /// </summary>
    internal Type? GetImplementationTypeOrNull()
    {
        var implementationType = Volatile.Read(ref _implementationType);
        if (implementationType == null)
        {
            // Never replaces what GetHandler wrote meanwhile.
            Interlocked.CompareExchange(ref _implementationType, new StrongBox<Type?>(FindImplementationType()), null);
            implementationType = Volatile.Read(ref _implementationType)!;
        }

        return implementationType.Value;
    }

    private Type? FindImplementationType()
    {
        using (var scope = ScopeFactory.CreateScope())
        {
            return scope.ServiceProvider
                .GetService<IServiceImplementationTypeProvider>()?
                .GetImplementationTypeOrNull(HandlerType);
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
