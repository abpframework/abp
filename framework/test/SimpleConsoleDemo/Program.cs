using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;

namespace SimpleConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var application = AbpApplicationFactory.Create<MyConsoleModule>(options =>
            {
                options.Configuration.CommandLineArgs = args;
                options.UseAutofac();
            }))
            {
                Console.WriteLine("Initializing the application...");
                application.Initialize();
                Console.WriteLine("Initializing the application... OK");

                Console.WriteLine("Checking configuration...");

                var configuration = application.ServiceProvider.GetRequiredService<IConfiguration>();
                if (configuration["AppSettingKey1"] != "AppSettingValue1")
                {
                    Console.WriteLine("ERROR: Could not read the configuration!");
                    Console.WriteLine("Press ENTER to exit!");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine();
                Console.WriteLine("Checking configuration... OK");

                var writers = application.ServiceProvider.GetServices<IMessageWriter>();
                foreach (var writer in writers)
                {
                    writer.Write();
                }

                Console.WriteLine();
                Console.WriteLine("Press ENTER to exit!");
                Console.ReadLine();
            }
        }
    }

    public class MyConsoleModule : AbpModule
    {
 
    }

    public interface IMessageWriter
    {
        void Write();
    }

    public class ConsoleMessageWriter : IMessageWriter, ITransientDependency
    {
        private readonly MessageSource _messageSource;

        public ConsoleMessageWriter(MessageSource messageSource)
        {
            _messageSource = messageSource;
        }

        public void Write()
        {
            Console.WriteLine(_messageSource.GetMessage());
        }
    }

    public class MessageSource : ITransientDependency
    {
        public string GetMessage()
        {
            return "Hello ABP!";
        }
    }
}