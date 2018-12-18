using System;
using System.Threading.Tasks;
using SharedModule;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace App1
{
    /// <summary>
    /// Used to know when App2 has received a message sent by App1.
    /// </summary>
    public class App1TextReceivedEventHandler : IDistributedEventHandler<App2TextReceivedEventData>, ITransientDependency
    {
        public Task HandleEventAsync(App2TextReceivedEventData eventData)
        {
            Console.WriteLine("--------> App2 has received the message: " + eventData.ReceivedText.TruncateWithPostfix(32));
            
            return Task.CompletedTask;
        }
    }
}
