using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Volo.Abp;

namespace MyCompanyName.MyProjectName
{
    public class MyProjectNameHostedService : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            using (var application = AbpApplicationFactory.Create<MyProjectNameModule>(options =>
            {
                options.UseAutofac(); //Autofac integration
                options.Services.AddLogging(c => c.AddSerilog());
            }))
            {
                application.Initialize();

                //Resolve a service and use it
                var helloWorldService = application.ServiceProvider.GetService<HelloWorldService>();
                helloWorldService.SayHello();

                application.Shutdown();
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
