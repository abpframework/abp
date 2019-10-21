using System;
using System.Threading.Tasks;
using SharedModule;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace App1
{
    /// <summary>
    /// Used to listen messages sent to App2 by App1.
    /// </summary>
    public class App1TextEventHandler : IDistributedEventHandler<App2ToApp1TextEventData>, ITransientDependency
    {
        private readonly IDistributedEventBus _distributedEventBus;

        public App1TextEventHandler(IDistributedEventBus distributedEventBus)
        {
            _distributedEventBus = distributedEventBus;
        }

        public async Task HandleEventAsync(App2ToApp1TextEventData eventData)
        {
            Console.WriteLine("************************ INCOMING MESSAGE ****************************");
            Console.WriteLine(eventData.TextMessage);
            Console.WriteLine("**********************************************************************");
            Console.WriteLine();

           await _distributedEventBus.PublishAsync(new App1TextReceivedEventData(eventData.TextMessage));
        }
    }
}
