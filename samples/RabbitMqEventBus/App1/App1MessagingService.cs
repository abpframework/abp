using System;
using SharedModule;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus;
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

        public void Run()
        {
            Console.WriteLine("*** Started the APPLICATION 1 ***");
            Console.WriteLine("Write a message and press ENTER to send to the App2.");
            Console.WriteLine("Press ENTER (without writing a message) to stop the application.");

            string message;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Send message to App2: ");

                message = Console.ReadLine();

                if (!message.IsNullOrEmpty())
                {
                    _distributedEventBus.Publish(new App1ToApp2TextEventData(message));
                }
                else
                {
                    _distributedEventBus.Publish(new App1ToApp2TextEventData("App1 is exiting. Bye bye...!"));
                }

            } while (!message.IsNullOrEmpty());
        }
    }
}