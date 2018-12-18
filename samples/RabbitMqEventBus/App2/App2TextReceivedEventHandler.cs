using System;
using System.Threading.Tasks;
using SharedModule;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace App2
{
    /// <summary>
    /// Used to know when App1 has received a message sent by App2.
    /// </summary>
    public class App2TextReceivedEventHandler : IDistributedEventHandler<App1TextReceivedEventData>, ITransientDependency
    {
        public Task HandleEventAsync(App1TextReceivedEventData eventData)
        {
            Console.WriteLine("--------> App1 has received the message: " + eventData.ReceivedText.TruncateWithPostfix(32));
            Console.WriteLine();

            return Task.CompletedTask;
        }
    }
}
