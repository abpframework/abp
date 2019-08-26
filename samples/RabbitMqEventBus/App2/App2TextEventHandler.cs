using System;
using System.Threading.Tasks;
using SharedModule;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace App2
{
    /// <summary>
    /// Used to listen messages sent to App2 by App1.
    /// </summary>
    public class App2TextEventHandler : IDistributedEventHandler<App1ToApp2TextEventData>, ITransientDependency
    {
        private readonly IDistributedEventBus _distributedEventBus;

        public App2TextEventHandler(IDistributedEventBus distributedEventBus)
        {
            _distributedEventBus = distributedEventBus;
        }

        public async Task HandleEventAsync(App1ToApp2TextEventData eventData)
        {
            Console.WriteLine("************************ INCOMING MESSAGE ****************************");
            Console.WriteLine(eventData.TextMessage);
            Console.WriteLine("**********************************************************************");
            Console.WriteLine();

            await _distributedEventBus.PublishAsync(new App2TextReceivedEventData(eventData.TextMessage));
        }
    }
}
