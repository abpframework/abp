using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Volo.Abp;

namespace MyCompanyName.MyProjectName;

public class MyProjectNameHostedService : IHostedService
{
    private readonly IAbpApplicationWithExternalServiceProvider _application;
    private readonly IServiceProvider _serviceProvider;
    private readonly HelloWorldService _helloWorldService;

    public MyProjectNameHostedService(
        IAbpApplicationWithExternalServiceProvider application,
        IServiceProvider serviceProvider,
        HelloWorldService helloWorldService)
    {
        _application = application;
        _serviceProvider = serviceProvider;
        _helloWorldService = helloWorldService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _application.Initialize(_serviceProvider);

        _helloWorldService.SayHello();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _application.Shutdown();

        return Task.CompletedTask;
    }
}
