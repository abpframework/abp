using SharedModule;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace App1
{
    public class App1MessagingService : ITransientDependency
    {
        private readonly IDistributedEventBus _distributedEventBus;

        public App1MessagingService(IDistributedEventBus distributedEventBus)
        {
            _distributedEventBus = distributedEventBus;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("*** Started the APPLICATION 1 ***");
            Console.WriteLine("Write a message and press ENTER to send to the App2.");
            Console.WriteLine("Press ENTER (without writing a message) to stop the application.");

            string message;
            do
            {
                Console.WriteLine();

                message = Console.ReadLine();

                if (!message.IsNullOrEmpty())
                {
                    await _distributedEventBus.PublishAsync(new App1ToApp2TextEventData(message));
                }
                else
                {
                    await _distributedEventBus.PublishAsync(new App1ToApp2TextEventData("App1 is exiting. Bye bye...!"));
                }

            } while (!message.IsNullOrEmpty());
        }
    }
}