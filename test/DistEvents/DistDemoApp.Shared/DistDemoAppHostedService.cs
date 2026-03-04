using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Volo.Abp;

namespace DistDemoApp
{
    public class DistDemoAppHostedService : IHostedService
    {
        private readonly IAbpApplicationWithExternalServiceProvider _application;
        private readonly IServiceProvider _serviceProvider;
        private readonly DemoService _demoService;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public DistDemoAppHostedService(
            IAbpApplicationWithExternalServiceProvider application,
            IServiceProvider serviceProvider,
            DemoService demoService,
            IHostApplicationLifetime hostApplicationLifetime)
        {
            _application = application;
            _serviceProvider = serviceProvider;
            _demoService = demoService;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _demoService.CreateTodoItemAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _application.Shutdown();

            return Task.CompletedTask;
        }
    }
}
