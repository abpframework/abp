# TickerQ Background Job Manager

[TickerQ](https://tickerq.net/) is a fast, reflection-free background task scheduler for .NET — built with source generators, EF Core integration, cron + time-based execution, and a real-time dashboard. You can integrate TickerQ with the ABP to use it instead of the [default background job manager](../background-jobs). In this way, you can use the same background job API for TickerQ and your code will be independent of TickerQ. If you like, you can directly use TickerQ's API, too.

> See the [background jobs document](../background-jobs) to learn how to use the background job system. This document only shows how to install and configure the TickerQ integration.

## Installation

It is suggested to use the [ABP CLI](../../../cli) to install this package.

### Using the ABP CLI

Open a command line window in the folder of the project (.csproj file) and type the following command:

````bash
abp add-package Volo.Abp.BackgroundJobs.TickerQ
````

>  If you haven't done it yet, you first need to install the [ABP CLI](../../../cli). For other installation options, see [the package description page](https://abp.io/package-detail/Volo.Abp.BackgroundJobs.TickerQ).

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

You need to call the `UseAbpTickerQ` extension method instead of `AddTickerQ` in the `OnApplicationInitialization` method of your module:

```csharp
// (default: TickerQStartMode.Immediate)
app.UseAbpTickerQ(startMode: ...);
```

### AbpBackgroundJobsTickerQOptions

You can configure the `TimeTicker` properties for specific jobs. For example, you can change `Priority`, `Retries` and `RetryIntervals` properties as shown below:

```csharp
Configure<AbpBackgroundJobsTickerQOptions>(options =>
{
	options.AddJobConfiguration<MyBackgroundJob>(new AbpBackgroundJobsTimeTickerConfiguration()
	{
		Retries = 3,
		RetryIntervals = new[] {30, 60, 120}, // Retry after 30s, 60s, then 2min
		Priority = TickerTaskPriority.High

		// Optional batching
		//BatchParent = Guid.Parse("...."),
		//BatchRunCondition = BatchRunCondition.OnSuccess
	});

	options.AddJobConfiguration<MyBackgroundJob2>(new AbpBackgroundJobsTimeTickerConfiguration()
	{
		Retries = 5,
		RetryIntervals = new[] {30, 60, 120}, // Retry after 30s, 60s, then 2min
		Priority = TickerTaskPriority.Normal
	});
});
```

### Add your own TickerQ Background Jobs Definitions

ABP will handle the TickerQ job definitions by `AbpTickerQFunctionProvider` service. You shouldn't use `TickerFunction` to add your own job definitions. You can inject and use the `AbpTickerQFunctionProvider` to add your own definitions and use `ITimeTickerManager<TimeTicker>` or `ICronTickerManager<CronTicker>` to manage the jobs.

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
	abpTickerQFunctionProvider.Functions.TryAdd(nameof(CleanupJobs), (string.Empty, TickerTaskPriority.Normal, new TickerFunctionDelegate(async (cancellationToken, serviceProvider, tickerFunctionContext) =>
	{
		var service = new CleanupJobs(); // Or get it from the serviceProvider
		var request = await TickerRequestProvider.GetRequestAsync<string>(serviceProvider,  tickerFunctionContext.Id, tickerFunctionContext.Type);
		var genericContext = new TickerFunctionContext<string>(tickerFunctionContext, request);
		await service.CleanupLogsAsync(genericContext, cancellationToken);
	})));
	abpTickerQFunctionProvider.RequestTypes.TryAdd(nameof(CleanupJobs), (typeof(string).FullName, typeof(string)));
	return Task.CompletedTask;
}
```

And then you can add a job by using the `ITimeTickerManager<TimeTicker>`:

```csharp
var timeTickerManager = context.ServiceProvider.GetRequiredService<ITimeTickerManager<TimeTicker>>();
await timeTickerManager.AddAsync(new TimeTicker
{
	Function = nameof(CleanupJobs),
	ExecutionTime = DateTime.UtcNow.AddSeconds(5),
	Request = TickerHelper.CreateTickerRequest<string>("cleanup_example_file.txt"),
	Retries = 3,
	RetryIntervals = new[] { 30, 60, 120 }, // Retry after 30s, 60s, then 2min
});
```

### TickerQ Dashboard and EF Core Integration

You can install the [TickerQ dashboard](https://tickerq.net/setup/dashboard.html) and [Entity Framework Core](https://tickerq.net/setup/tickerq-ef-core.html) integration by its documentation. There is no specific configuration needed for the ABP integration.

