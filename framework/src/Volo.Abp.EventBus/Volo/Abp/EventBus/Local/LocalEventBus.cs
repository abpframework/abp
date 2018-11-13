using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Collections;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Reflection;
using Volo.Abp.Threading;

namespace Volo.Abp.EventBus.Local
{
    /// <summary>
    /// Implements EventBus as Singleton pattern.
    /// </summary>
    [ExposeServices(typeof(ILocalEventBus), typeof(LocalEventBus))]
    public class LocalEventBus : ILocalEventBus, ISingletonDependency
    {
        /// <summary>
        /// Reference to the Logger.
        /// </summary>
        public ILogger<LocalEventBus> Logger { get; set; }

        protected EventBusOptions Options { get; }

        protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }

        protected IServiceProvider ServiceProvider { get; }

        public LocalEventBus(
            IOptions<EventBusOptions> options,
            IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Options = options.Value;
            Logger = NullLogger<LocalEventBus>.Instance;

            HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
            Subscribe(Options.Handlers);
        }

        public virtual void Subscribe(ITypeList<IEventHandler> handlers)
        {
            foreach (var handler in handlers)
            {
                var interfaces = handler.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    if (!typeof(IEventHandler).GetTypeInfo().IsAssignableFrom(@interface))
                    {
                        continue;
                    }

                    var genericArgs = @interface.GetGenericArguments();
                    if (genericArgs.Length == 1)
                    {
                        Subscribe(genericArgs[0], new IocEventHandlerFactory(ServiceProvider, handler));
                    }
                }
            }
        }

        /// <inheritdoc/>
        public IDisposable Subscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class
        {
            return Subscribe(typeof(TEvent), new ActionEventHandler<TEvent>(action));
        }

        /// <inheritdoc/>
        public IDisposable Subscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : class
        {
            return Subscribe(typeof(TEvent), handler);
        }

        /// <inheritdoc/>
        public IDisposable Subscribe<TEvent, THandler>()
            where TEvent : class
            where THandler : IEventHandler, new()
        {
            return Subscribe(typeof(TEvent), new TransientEventHandlerFactory<THandler>());
        }

        /// <inheritdoc/>
        public IDisposable Subscribe(Type eventType, IEventHandler handler)
        {
            return Subscribe(eventType, new SingleInstanceHandlerFactory(handler));
        }

        /// <inheritdoc/>
        public IDisposable Subscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class
        {
            return Subscribe(typeof(TEvent), factory);
        }

        /// <inheritdoc/>
        public IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
        {
            GetOrCreateHandlerFactories(eventType)
                .Locking(factories => factories.Add(factory));

            return new EventHandlerFactoryUnregistrar(this, eventType, factory);
        }

        /// <inheritdoc/>
        public void Unsubscribe<TEvent>(Func<TEvent, Task> action) where TEvent : class
        {
            Check.NotNull(action, nameof(action));

            GetOrCreateHandlerFactories(typeof(TEvent))
                .Locking(factories =>
                {
                    factories.RemoveAll(
                        factory =>
                        {
                            var singleInstanceFactory = factory as SingleInstanceHandlerFactory;
                            if (singleInstanceFactory == null)
                            {
                                return false;
                            }

                            var actionHandler = singleInstanceFactory.HandlerInstance as ActionEventHandler<TEvent>;
                            if (actionHandler == null)
                            {
                                return false;
                            }

                            return actionHandler.Action == action;
                        });
                });
        }

        /// <inheritdoc/>
        public void Unsubscribe<TEvent>(IEventHandler<TEvent> handler) where TEvent : class
        {
            Unsubscribe(typeof(TEvent), handler);
        }

        /// <inheritdoc/>
        public void Unsubscribe(Type eventType, IEventHandler handler)
        {
            GetOrCreateHandlerFactories(eventType)
                .Locking(factories =>
                {
                    factories.RemoveAll(
                        factory =>
                            factory is SingleInstanceHandlerFactory &&
                            (factory as SingleInstanceHandlerFactory).HandlerInstance == handler
                    );
                });
        }

        /// <inheritdoc/>
        public void Unsubscribe<TEvent>(IEventHandlerFactory factory) where TEvent : class
        {
            Unsubscribe(typeof(TEvent), factory);
        }

        /// <inheritdoc/>
        public void Unsubscribe(Type eventType, IEventHandlerFactory factory)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));
        }

        /// <inheritdoc/>
        public void UnsubscribeAll<TEvent>() where TEvent : class
        {
            UnsubscribeAll(typeof(TEvent));
        }

        /// <inheritdoc/>
        public void UnsubscribeAll(Type eventType)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Clear());
        }

        /// <inheritdoc/>
        public Task PublishAsync<TEvent>(TEvent eventData) where TEvent : class
        {
            return PublishAsync(typeof(TEvent), eventData);
        }

        /// <inheritdoc/>
        public async Task PublishAsync(Type eventType, object eventData)
        {
            //TODO: Aggregate all exceptions (including the recursive call)!

            var exceptions = new List<Exception>();

            await new SynchronizationContextRemover();

            foreach (var handlerFactories in GetHandlerFactories(eventType))
            {
                foreach (var handlerFactory in handlerFactories.EventHandlerFactories)
                {
                    await TriggerAsyncHandlingException(handlerFactory, handlerFactories.EventType, eventData, exceptions);
                }
            }

            //Implements generic argument inheritance. See IEventDataWithInheritableGenericArgument
            if (eventType.GetTypeInfo().IsGenericType &&
                eventType.GetGenericArguments().Length == 1 &&
                typeof(IEventDataWithInheritableGenericArgument).IsAssignableFrom(eventType))
            {
                var genericArg = eventType.GetGenericArguments()[0];
                var baseArg = genericArg.GetTypeInfo().BaseType;
                if (baseArg != null)
                {
                    var baseEventType = eventType.GetGenericTypeDefinition().MakeGenericType(baseArg);
                    var constructorArgs = ((IEventDataWithInheritableGenericArgument)eventData).GetConstructorArgs();
                    var baseEventData = Activator.CreateInstance(baseEventType, constructorArgs);
                    await PublishAsync(baseEventType, baseEventData);
                }
            }

            if (exceptions.Any())
            {
                if (exceptions.Count == 1)
                {
                    exceptions[0].ReThrow();
                }

                throw new AggregateException("More than one error has occurred while triggering the event: " + eventType, exceptions);
            }
        }

        private async Task TriggerAsyncHandlingException(IEventHandlerFactory asyncHandlerFactory, Type eventType, object eventData, List<Exception> exceptions)
        {
            using (var eventHandlerWrapper = asyncHandlerFactory.GetHandler())
            {
                try
                {
                    var handlerType = eventHandlerWrapper.EventHandler.GetType();

                    if (ReflectionHelper.IsAssignableToGenericType(
                        handlerType,
                        typeof(IEventHandler<>)))
                    {
                        var method = typeof(IEventHandler<>) //TODO: to a static field
                            .MakeGenericType(eventType)
                            .GetMethod(
                                nameof(IEventHandler<object>.HandleEventAsync),
                                new[] {eventType}
                            );

                        await (Task)method.Invoke(eventHandlerWrapper.EventHandler, new[] { eventData });
                    }
                    else if (ReflectionHelper.IsAssignableToGenericType(
                        handlerType,
                        typeof(IDistributedEventHandler<>)))
                    {
                        var method = typeof(IDistributedEventHandler<>) //TODO: to a static field
                            .MakeGenericType(eventType)
                            .GetMethod(
                                nameof(IDistributedEventHandler<object>.HandleEventAsync),
                                new[] {eventType}
                            );

                        await (Task)method.Invoke(eventHandlerWrapper.EventHandler, new[] { eventData });
                    }
                    else
                    {
                        throw new AbpException("The object instance is not an event handler. Object type: " + handlerType.AssemblyQualifiedName);
                    }
                }
                catch (TargetInvocationException ex)
                {
                    exceptions.Add(ex.InnerException);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
        }

        public virtual IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
        {
            var handlerFactoryList = new List<EventTypeWithEventHandlerFactories>();

            foreach (var handlerFactory in HandlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
            {
                handlerFactoryList.Add(new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value));
            }

            return handlerFactoryList.ToArray();
        }

        private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
        {
            return HandlerFactories.GetOrAdd(eventType, (type) => new List<IEventHandlerFactory>());
        }

        private static bool ShouldTriggerEventForHandler(Type targetEventType, Type handlerEventType)
        {
            //Should trigger same type
            if (handlerEventType == targetEventType)
            {
                return true;
            }

            //Should trigger for inherited types
            if (handlerEventType.IsAssignableFrom(targetEventType))
            {
                return true;
            }

            return false;
        }

        public class EventTypeWithEventHandlerFactories
        {
            public Type EventType { get; }

            public List<IEventHandlerFactory> EventHandlerFactories { get; }

            public EventTypeWithEventHandlerFactories(Type eventType, List<IEventHandlerFactory> eventHandlerFactories)
            {
                EventType = eventType;
                EventHandlerFactories = eventHandlerFactories;
            }
        }

        // Reference from
        // https://blogs.msdn.microsoft.com/benwilli/2017/02/09/an-alternative-to-configureawaitfalse-everywhere/
        private struct SynchronizationContextRemover : INotifyCompletion
        {
            public bool IsCompleted
            {
                get { return SynchronizationContext.Current == null; }
            }

            public void OnCompleted(Action continuation)
            {
                var prevContext = SynchronizationContext.Current;
                try
                {
                    SynchronizationContext.SetSynchronizationContext(null);
                    continuation();
                }
                finally
                {
                    SynchronizationContext.SetSynchronizationContext(prevContext);
                }
            }

            public SynchronizationContextRemover GetAwaiter()
            {
                return this;
            }

            public void GetResult()
            {
            }
        }
    }
}