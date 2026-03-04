using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Volo.Abp;
using Volo.Abp.Threading;

namespace DistDemoApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
//             Log.Logger = new LoggerConfiguration()
// #if DEBUG
//                 .MinimumLevel.Debug()
// #else
//                 .MinimumLevel.Information()
// #endif
//                 .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
//                 .Enrich.FromLogContext()
//                 .WriteTo.Async(c => c.File("Logs/logs.txt"))
//                 .WriteTo.Async(c => c.Console())
//                 .CreateLogger();
//
//             try
//             {
//                 Log.Information("Starting console host.");
//                 await CreateHostBuilder(args).RunConsoleAsync();
//                 return 0;
//             }
//             catch (Exception ex)
//             {
//                 Log.Fatal(ex, "Host terminated unexpectedly!");
//                 return 1;
//             }
//             finally
//             {
//                 Log.CloseAndFlush();
//             }

            using (var application = AbpApplicationFactory.Create<DistDemoAppMongoDbRebusModule>(options =>
                   {
                       options.UseAutofac();
                       options.Services.AddSerilog((serviceProvider, c) =>
                       {
                           // c.Enrich.FromLogContext()
                           //     .WriteTo.Async(c => c.File("Logs/logs.txt"))
                           //     .WriteTo.Async(c => c.Console())
                           //      .WriteTo.AbpStudio(serviceProvider);
                       });
                       options.Services.AddLogging(c => c.AddSerilog());
                   }))
            {
                Log.Information("Starting Volo.AbpIo.DbMigrator.");
                
                application.Initialize();

                AsyncHelper.RunSync(
                    () => application
                        .ServiceProvider
                        .GetRequiredService<DemoService>().CreateTodoItemAsync()
                );

                application.Shutdown(); 
            }

        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseAutofac()
                .UseSerilog()
                .ConfigureAppConfiguration((context, config) =>
                {
                    //setup your additional configuration sources
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddApplication<DistDemoAppMongoDbRebusModule>();
                });
    }
}
