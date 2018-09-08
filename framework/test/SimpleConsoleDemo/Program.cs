using System;
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
                
            }))
            {
                application.Initialize();

                Console.WriteLine("ABP initialized... Press ENTER to exit!");

                var writers = application.ServiceProvider.GetServices<IMessageWriter>();
                foreach (var writer in writers)
                {
                    writer.Write();
                }

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