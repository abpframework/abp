using SharedModule;
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace App2
{
    public class App2MessagingService : ITransientDependency
    {
        private readonly IDistributedEventBus _distributedEventBus;

        public App2MessagingService(IDistributedEventBus distributedEventBus)
        {
            _distributedEventBus = distributedEventBus;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("*** Started the APPLICATION 2 ***");
            Console.WriteLine("Write a message and press ENTER to send to the App1.");
            Console.WriteLine("Press ENTER (without writing a message) to stop the application...");

            string message;
            do
            {
                Console.WriteLine();

                message = Console.ReadLine();

                if (!message.IsNullOrEmpty())
                {
                    await _distributedEventBus.PublishAsync(new App2ToApp1TextEventData(message));
                }
                else
                {
                    await _distributedEventBus.PublishAsync(new App2ToApp1TextEventData("App2 is exiting. Bye bye...!"));
                }

            } while (!message.IsNullOrEmpty());
        }
    }
}