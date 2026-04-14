# TickerQ Background Worker Manager

[TickerQ](https://tickerq.net/) is a fast, reflection-free background task scheduler for .NET — built with source generators, EF Core integration, cron + time-based execution, and a real-time dashboard. You can integrate TickerQ with the ABP to use it instead of the [default background worker manager](../background-workers).

## Installation

It is suggested to use the [ABP CLI](../../../cli) to install this package.

### Using the ABP CLI

Open a command line window in the folder of the project (.csproj file) and type the following command:

````bash
abp add-package Volo.Abp.BackgroundWorkers.TickerQ
````

>  If you haven't done it yet, you first need to install the [ABP CLI](../../../cli). For other installation options, see [the package description page](https://abp.io/package-detail/Volo.Abp.BackgroundWorkers.TickerQ).

## Configuration

### AddTickerQ

You can call the `AddTickerQ` extension method in the `ConfigureServices` method of your module to configure TickerQ services:

> This is optional. ABP will automatically register TickerQ services.

```csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
	context.Services.AddTickerQ(x =>
	{
		// Configure TickerQ options here
	});
}
```

### UseAbpTickerQ

You need to call the `UseAbpTickerQ` extension method in the `OnApplicationInitialization` method of your module:

```csharp
public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
{
    // (default: TickerQStartMode.Immediate)
    context.GetHost().UseAbpTickerQ(qStartMode: ...);
}
```

### AbpBackgroundWorkersTickerQOptions

You can configure the `CronTicker` properties for specific jobs. For example, Change `Priority`, `Retries` and `RetryIntervals` properties:

```csharp
Configure<AbpBackgroundWorkersTickerQOptions>(options =>
{
	options.AddConfiguration<MyBackgroundWorker>(new AbpBackgroundWorkersCronTickerConfiguration()
	{
		Retries = 3,
		RetryIntervals = new[] {30, 60, 120}, // Retry after 30s, 60s, then 2min,
		Priority = TickerTaskPriority.High
	});
});
```

### Add your own TickerQ Background Worker Definitions

ABP will handle the TickerQ job definitions by `AbpTickerQFunctionProvider` service. You shouldn't use `TickerFunction` to add your own job definitions. You can inject and use the `AbpTickerQFunctionProvider` to add your own definitions and use `ITimeTickerManager<TimeTickerEntity>` or `ICronTickerManager<CronTickerEntity>` to manage the jobs.

For example, you can add a `CleanupJobs` job definition in the `OnPreApplicationInitializationAsync` method of your module:

```csharp
public class CleanupJobs
{
    public async Task CleanupLogsAsync(TickerFunctionContext<string> tickerContext, CancellationToken cancellationToken)
    {
        var logFileName = tickerContext.Request;
        Console.WriteLine($"Cleaning up log file: {logFileName} at {DateTime.Now}");
    }
}
```

```csharp
public override Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
{
	var abpTickerQFunctionProvider = context.ServiceProvider.GetRequiredService<AbpTickerQFunctionProvider>();
	abpTickerQFunctionProvider.AddFunction(nameof(CleanupJobs), async (cancellationToken, serviceProvider, tickerFunctionContext) =>
	{
		var service = new CleanupJobs(); // Or get it from the serviceProvider
		var request = await TickerRequestProvider.GetRequestAsync<string>(tickerFunctionContext, cancellationToken);
		var genericContext = new TickerFunctionContext<string>(tickerFunctionContext, request);
		await service.CleanupLogsAsync(genericContext, cancellationToken);
	}, TickerTaskPriority.Normal);
	abpTickerQFunctionProvider.RequestTypes.TryAdd(nameof(CleanupJobs), (typeof(string).FullName, typeof(string)));
	return Task.CompletedTask;
}
```

And then you can add a job by using the `ICronTickerManager<CronTickerEntity>`:

```csharp
var cronTickerManager = context.ServiceProvider.GetRequiredService<ICronTickerManager<CronTickerEntity>>();
await cronTickerManager.AddAsync(new CronTickerEntity
{
	Function = nameof(CleanupJobs),
	Expression = "0 */6 * * *", // Every 6 hours
	Request = TickerHelper.CreateTickerRequest<string>("cleanup_example_file.txt"),
	Retries = 2,
	RetryIntervals = new[] { 60, 300 }
});
```

You can specify a cron expression instead of using `ICronTickerManager<CronTickerEntity>` to add a worker:

```csharp
abpTickerQFunctionProvider.AddFunction(nameof(CleanupJobs), async (cancellationToken, serviceProvider, tickerFunctionContext) =>
{
	var service = new CleanupJobs();
	var request = await TickerRequestProvider.GetRequestAsync<string>(tickerFunctionContext, cancellationToken);
	var genericContext = new TickerFunctionContext<string>(tickerFunctionContext, request);
	await service.CleanupLogsAsync(genericContext, cancellationToken);
}, TickerTaskPriority.Normal);
```
