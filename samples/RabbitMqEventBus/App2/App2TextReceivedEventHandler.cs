using System;
using System.Threading.Tasks;
using SharedModule;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace App2
{
    public class App2TextReceivedEventHandler : IDistributedEventHandler<TextReceivedEventData>, ITransientDependency
    {
        public Task HandleEventAsync(TextReceivedEventData eventData)
        {
            Console.WriteLine("--------> App1 has received the message: " + eventData.ReceivedText.TruncateWithPostfix(32));
            
            return Task.CompletedTask;
        }
    }
}
