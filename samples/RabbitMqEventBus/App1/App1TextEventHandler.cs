using System;
using System.Threading.Tasks;
using SharedModule;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace App2
{
    public class App1TextEventHandler : IDistributedEventHandler<TextEventData>, ITransientDependency
    {
        private readonly IDistributedEventBus _distributedEventBus;

        public App1TextEventHandler(IDistributedEventBus distributedEventBus)
        {
            _distributedEventBus = distributedEventBus;
        }

        public Task HandleEventAsync(TextEventData eventData)
        {
            Console.WriteLine("************************ INCOMING MESSAGE ****************************");
            Console.WriteLine(eventData.TextMessage);
            Console.WriteLine("**********************************************************************");

            _distributedEventBus.PublishAsync(
                new TextReceivedEventData
                {
                    ReceivedText = eventData.TextMessage
                }
            );

            return Task.CompletedTask;
        }
    }
}
