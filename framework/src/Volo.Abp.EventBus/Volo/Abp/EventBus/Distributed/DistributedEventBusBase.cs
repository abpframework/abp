using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Volo.Abp.EventBus.Distributed
{
    public abstract class DistributedEventBusBase : EventBusBase, IDistributedEventBus
    {
        protected AbpDistributedEventBusOptions AbpDistributedEventBusOptions { get; }

        protected DistributedEventBusBase(
            IServiceScopeFactory serviceScopeFactory,
            ICurrentTenant currentTenant, 
            IUnitOfWorkManager unitOfWorkManager,
            IEventErrorHandler errorHandler,
            IOptions<AbpDistributedEventBusOptions> abpDistributedEventBusOptions
            ) : base(
            serviceScopeFactory, 
            currentTenant,
            unitOfWorkManager,
            errorHandler)
        {
            AbpDistributedEventBusOptions = abpDistributedEventBusOptions.Value;
        }

        public IDisposable Subscribe<TEvent>(IDistributedEventHandler<TEvent> handler) where TEvent : class
        {
            return Subscribe(typeof(TEvent), handler);
        }

        public Task PublishAsync<TEvent>(
            TEvent eventData,
            bool onUnitOfWorkComplete = true,
            bool useOutbox = true)
            where TEvent : class
        {
            return PublishAsync(typeof(TEvent), eventData, onUnitOfWorkComplete, useOutbox);
        }

        public async Task PublishAsync(
            Type eventType,
            object eventData,
            bool onUnitOfWorkComplete = true,
            bool useOutbox = true)
        {
            if (onUnitOfWorkComplete && UnitOfWorkManager.Current != null)
            {
                AddToUnitOfWork(
                    UnitOfWorkManager.Current,
                    new UnitOfWorkEventRecord(eventType, eventData, EventOrderGenerator.GetNext(), useOutbox)
                );
                return;
            }
            
            if (useOutbox)
            {
                if (await AddToOutboxAsync(eventType, eventData))
                {
                    return;
                }
            }

            await PublishToEventBusAsync(eventType, eventData);
        }
        
        private async Task<bool> AddToOutboxAsync(Type eventType, object eventData)
        {
            return false;
        }
    }
}