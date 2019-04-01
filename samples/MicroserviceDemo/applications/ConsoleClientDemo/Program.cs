using Serilog;
using Serilog.Events;
using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Threading;

namespace ConsoleClientDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            InitializeSerilog();

            Log.Information("Starting ConsoleClientDemo...");

            try
            {
                using (var application = AbpApplicationFactory.Create<ConsoleClientDemoModule>(options =>
                    {
                        options.Services.AddLogging(loggingBuilder =>
                        {
                            loggingBuilder.AddSerilog(dispose: true);
                        });
                    }))
                {
                    application.Initialize();

                    var demo = application.ServiceProvider.GetRequiredService<ClientDemoService>();
                    AsyncHelper.RunSync(() => demo.RunAsync());

                    Console.WriteLine("Press ENTER to stop application...");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                throw;
            }
        }

        private static void InitializeSerilog()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File("Logs/logs.txt")
                .CreateLogger();
        }
    }
}
