using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TickerQ.Dashboard.DependencyInjection;
using TickerQ.DependencyInjection;
using TickerQ.Utilities;
using TickerQ.Utilities.Enums;
using TickerQ.Utilities.Interfaces.Managers;
using TickerQ.Utilities.Base;
using TickerQ.Utilities.Entities;
using Volo.Abp.AspNetCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs.DemoApp.Shared;
using Volo.Abp.BackgroundJobs.DemoApp.Shared.Jobs;
using Volo.Abp.BackgroundJobs.TickerQ;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.BackgroundWorkers.TickerQ;
using Volo.Abp.Modularity;
using Volo.Abp.TickerQ;

namespace Volo.Abp.BackgroundJobs.DemoApp.TickerQ;

[DependsOn(
    typeof(AbpBackgroundJobsTickerQModule),
    typeof(AbpBackgroundWorkersTickerQModule),
    typeof(DemoAppSharedModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreModule)
)]
public class DemoAppTickerQModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddTickerQ(options =>
        {
            options.ConfigureScheduler(scheduler =>
            {
                scheduler.FallbackIntervalChecker = TimeSpan.FromSeconds(30);
            });

            options.AddDashboard(x =>
            {
                x.SetBasePath("/tickerq-dashboard");
            });
        });

        Configure<AbpBackgroundJobsTickerQOptions>(options =>
        {
            options.AddConfiguration<WriteToConsoleGreenJob>(new AbpBackgroundJobsTimeTickerConfiguration()
            {
                Retries = 3,
                RetryIntervals = new[] {30, 60, 120}, // Retry after 30s, 60s, then 2min,
                Priority = TickerTaskPriority.High
            });

            options.AddConfiguration<WriteToConsoleYellowJob>(new AbpBackgroundJobsTimeTickerConfiguration()
            {
                Retries = 5,
                RetryIntervals = new[] {30, 60, 120}, // Retry after 30s, 60s, then 2min
            });
        });

        Configure<AbpBackgroundWorkersTickerQOptions>(options =>
        {
            options.AddConfiguration<MyBackgroundWorker>(new AbpBackgroundWorkersCronTickerConfiguration()
            {
                Retries = 3,
                RetryIntervals = new[] {30, 60, 120}, // Retry after 30s, 60s, then 2min,
                Priority = TickerTaskPriority.High
            });
        });
    }

    public override Task OnPreApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var abpTickerQFunctionProvider = context.ServiceProvider.GetRequiredService<AbpTickerQFunctionProvider>();
        abpTickerQFunctionProvider.AddFunction(nameof(CleanupJobs), async (cancellationToken, serviceProvider, tickerFunctionContext) =>
        {
            var service = new CleanupJobs();
            var request = await TickerRequestProvider.GetRequestAsync<string>(tickerFunctionContext, cancellationToken);
            var genericContext = new TickerFunctionContext<string>(tickerFunctionContext, request);
            await service.CleanupLogsAsync(genericContext, cancellationToken);
        }, TickerTaskPriority.Normal);
        abpTickerQFunctionProvider.RequestTypes.TryAdd(nameof(CleanupJobs), (typeof(string).FullName, typeof(string)));
        return Task.CompletedTask;
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        var backgroundWorkerManager = context.ServiceProvider.GetRequiredService<IBackgroundWorkerManager>();
        await backgroundWorkerManager.AddAsync(context.ServiceProvider.GetRequiredService<MyBackgroundWorker>());

        var app = context.GetApplicationBuilder();

        context.GetHost().UseAbpTickerQ();

        var timeTickerManager = context.ServiceProvider.GetRequiredService<ITimeTickerManager<TimeTickerEntity>>();
        await timeTickerManager.AddAsync(new TimeTickerEntity
        {
            Function = nameof(CleanupJobs),
            ExecutionTime = DateTime.UtcNow.AddSeconds(5),
            Request = TickerHelper.CreateTickerRequest<string>("cleanup_example_file.txt"),
            Retries = 3,
            RetryIntervals = new[] { 30, 60, 120 }, // Retry after 30s, 60s, then 2min
        });

        var cronTickerManager = context.ServiceProvider.GetRequiredService<ICronTickerManager<CronTickerEntity>>();
        await cronTickerManager.AddAsync(new CronTickerEntity
        {
            Function = nameof(CleanupJobs),
            Expression = "* * * * *", // Every minute
            Request = TickerHelper.CreateTickerRequest<string>("cleanup_example_file.txt"),
            Retries = 2,
            RetryIntervals = new[] { 60, 300 }
        });

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async httpContext =>
            {
                httpContext.Response.Redirect("/tickerq-dashboard", true);
            });
        });

        await CancelableBackgroundJobAsync(context.ServiceProvider);
    }

    private async Task CancelableBackgroundJobAsync(IServiceProvider serviceProvider)
    {
        var backgroundJobManager = serviceProvider.GetRequiredService<IBackgroundJobManager>();
        var jobId = await backgroundJobManager.EnqueueAsync(new LongRunningJobArgs { Value = "test-cancel-job" });
        await backgroundJobManager.EnqueueAsync(new LongRunningJobArgs { Value = "test-3" });

        await Task.Delay(1000);

        var timeTickerManager = serviceProvider.GetRequiredService<ITimeTickerManager<TimeTickerEntity>>();
        var result = await timeTickerManager.DeleteAsync(Guid.Parse(jobId));
    }
}
