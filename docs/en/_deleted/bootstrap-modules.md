# Bootstrap Modules

ABP can be bootstrapped in various applications. like ASP NET Core or Console, WPF, etc.

Modules can be started sync or async, Corresponds to various event methods in the module. We recommend using **async**, Especially if you want to make async calls in the module's methods.

The `PreConfigureServices`, `ConfigureServices`, `PostConfigureServices`, `OnPreApplicationInitialization`, `OnApplicationInitialization`, `OnPostApplicationInitialization` methods of module have the Async version. 

> Async methods automatically call sync methods by default.

If you use async methods in your module, please keep the same sync methods for compatibility.

````csharp
public async override Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
{
    await AsyncMethod();
}

public override void OnApplicationInitialization(ApplicationInitializationContext context)
{
    AsyncHelper.RunSync(() => OnApplicationInitializationAsync(context));
}
````

## ASP NET Core

Bootstrap ABP by using [WebApplication](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.webapplication?view=aspnetcore-6.0) 

````csharp
var builder = WebApplication.CreateBuilder(args);
builder.Host.AddAppSettingsSecretsJson()
    .UseAutofac();

await builder.Services.AddApplicationAsync<MyProjectNameWebModule>();
var app = builder.Build();

await app.InitializeApplicationAsync();
await app.RunAsync();
````

## Console

Bootstrap ABP in Console App.

````csharp
var abpApplication = await AbpApplicationFactory.CreateAsync<MyProjectNameModule>(options =>
{
    options.UseAutofac();
});

await _abpApplication.InitializeAsync();

var helloWorldService = _abpApplication.ServiceProvider.GetRequiredService<HelloWorldService>();

await helloWorldService.SayHelloAsync();
````

Bootstrap ABP by using [HostedService](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-6.0&tabs=visual-studio#ihostedservice-interface) 

````csharp
public class MyHostedService : IHostedService
{
    private IAbpApplicationWithInternalServiceProvider _abpApplication;

    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _hostEnvironment;

    public MyHostedService(IConfiguration configuration, IHostEnvironment hostEnvironment)
    {
        _configuration = configuration;
        _hostEnvironment = hostEnvironment;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _abpApplication = await AbpApplicationFactory.CreateAsync<MyProjectNameModule>(options =>
        {
            options.Services.ReplaceConfiguration(_configuration);
            options.Services.AddSingleton(_hostEnvironment);

            options.UseAutofac();
            options.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
        });

        await _abpApplication.InitializeAsync();

        var helloWorldService = _abpApplication.ServiceProvider.GetRequiredService<HelloWorldService>();

        await helloWorldService.SayHelloAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _abpApplication.ShutdownAsync();
    }
}
````

## WPF

Bootstrap ABP on [OnStartup](https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.onstartup?view=windowsdesktop-6.0) method.

````csharp
public partial class App : Application
{
    private IAbpApplicationWithInternalServiceProvider _abpApplication;

    protected async override void OnStartup(StartupEventArgs e)
    {
        _abpApplication =  await AbpApplicationFactory.CreateAsync<MyProjectNameModule>(options =>
        {
            options.UseAutofac();
            options.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
        });

        await _abpApplication.InitializeAsync();

        _abpApplication.Services.GetRequiredService<MainWindow>()?.Show();
    }

    protected async override void OnExit(ExitEventArgs e)
    {
        await _abpApplication.ShutdownAsync();
    }
}

````

