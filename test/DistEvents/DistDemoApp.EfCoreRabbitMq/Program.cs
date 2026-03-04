using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Volo.Abp;
using Volo.Abp.Threading;

namespace DistDemoApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (var application = AbpApplicationFactory.Create<DistDemoAppEfCoreRabbitMqModule>(options =>
                   {
                       options.UseAutofac();
                       options.Services.AddSerilog((_, _) =>
                       {
                       });
                       options.Services.AddLogging(c => c.AddSerilog());
                   }))
            {
                Log.Information("Starting DistDemoApp.EfCoreRabbitMq.");
                application.Initialize();

                AsyncHelper.RunSync(
                    () => application
                        .ServiceProvider
                        .GetRequiredService<DemoService>().CreateTodoItemAsync()
                );

                application.Shutdown(); 
            }
        }
    }
}
