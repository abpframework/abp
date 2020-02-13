# Background Jobs

## Introduction

Background jobs are used to queue some tasks to be executed in the background. You may need background jobs for several reasons. Here are some examples:

- To perform **long-running tasks** without having the users wait. For example, a user presses a 'report' button to start a long-running reporting job. You add this job to the **queue** and send the report's result to your user via email when it's completed.
- To create **re-trying** and **persistent tasks** to **guarantee** that a code will be **successfully executed**. For example, you can send emails in a background job to overcome **temporary failures** and **guarantee** that it eventually will be sent. That way users do not wait while sending emails.

Background jobs are **persistent** that means they will be **re-tried** and **executed** later even if your application crashes.

## Abstraction Package

ABP provides an **abstraction** module and **several implementations** for background jobs. It has a built-in/default implementation as well as Hangfire and RabbitMQ integrations.

`Volo.Abp.BackgroundJobs.Abstractions` nuget package provides needed services to create background jobs and queue background job items. If your module only depend on this package, it can be independent from the actual implementation/integration.

> `Volo.Abp.BackgroundJobs.Abstractions` package is installed to the startup templates by default.

### Create a Background Job

A background job is a class that implements the `IBackgroundJob<TArgs>` interface or derives from the `BackgroundJob<TArgs>` class. `TArgs` is a simple plain C# class to store the job data.

This example is used to send emails in background. First, define a class to store arguments of the background job:

````csharp
public class EmailSendingArgs
{
    public string EmailAddress { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}
````

Then create a background job class that uses an `EmailSendingArgs` object to send an email:

````csharp
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing;

namespace MyProject
{
    public class EmailSendingJob : BackgroundJob<EmailSendingArgs>, ITransientDependency
    {
        private readonly IEmailSender _emailSender;

        public EmailSendingJob(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public override void Execute(EmailSendingArgs args)
        {
            _emailSender.Send(
                args.EmailAddress,
                args.Subject,
                args.Body
            );
        }
    }
}
````

This job simply uses `IEmailSender` to send emails (see [email sending document](Emailing.md)).

#### Exception Handling

A background job should not hide exceptions. If it throws an exception, the background job is automatically re-tried after a calculated waiting time. Hide exceptions only if you don't want to re-run the background job for the current argument.

### Queue a Job Item

Now, you can queue an email sending job using the `IBackgroundJobManager` service:

````csharp
public class RegistrationService : ApplicationService
{
    private readonly IBackgroundJobManager _backgroundJobManager;

    public RegistrationService(IBackgroundJobManager backgroundJobManager)
    {
        _backgroundJobManager = backgroundJobManager;
    }

    public async Task RegisterAsync(string userName, string emailAddress, string password)
    {
        //TODO: Create new user in the database...

        await _backgroundJobManager.EnqueueAsync(
            new EmailSendingArgs
            {
                EmailAddress = emailAddress,
                Subject = "You've successfully registered!",
                Body = "..."
            }
        );
    }
}
````

Just injected `IBackgroundJobManager` service and used its `EnqueueAsync` method to add a new job to the queue.

Enqueue method gets some optional arguments to control the background job:

* **priority** is used to control priority of the job item. It gets an `BackgroundJobPriority` enum which has `Low`, `BelowNormal`, `Normal` (default), `AboveNormal` and `Hight` fields.
* **delay** is used to wait a while (`TimeSpan`) before first try.

### Disable Job Execution

You may want to disable background job execution for your application. This is generally needed if you want to execute background jobs in another process and disable it for the current process.

Use `AbpBackgroundJobOptions` to configure the job execution:

````csharp
[DependsOn(typeof(AbpBackgroundJobsModule))]
public class MyModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options =>
        {
            options.IsJobExecutionEnabled = false; //Disables job execution
        });
    }
}
````

> Default background manager (see below) does not support multiple processes execute the same job queue. So, if you have multiple running instance of your application and you are using the default background job manager, you should only enable one application instance process the job queue.

## Default Background Job Manager

ABP framework includes a simple `IBackgroundJobManager` implementation that;

- Works as **FIFO** in a **single thread**.
- **Retries** job execution until the job **successfully runs** or **timeouts**. Default timeout is 2 days for a job. Logs all exceptions.
- **Deletes** a job from the store (database) when it's successfully executed. If it's timed out, it sets it as **abandoned** and leaves it in the database.
- **Increasingly waits between retries** for a job. It waits 1 minute for the first retry, 2 minutes for the second retry, 4 minutes for the third retry and so on.
- **Polls** the store for jobs in fixed intervals. It queries jobs, ordering by priority (asc) and then by try count (asc).

> `Volo.Abp.BackgroundJobs` nuget package contains the default background job manager and it is installed to the startup templates by default.

### Configuration

Use `AbpBackgroundJobWorkerOptions` in your [module class](Module-Development-Basics.md) to configure the default background job manager. The example below changes the timeout duration for background jobs:

````csharp
[DependsOn(typeof(AbpBackgroundJobsModule))]
public class MyModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobWorkerOptions>(options =>
        {
            options.DefaultTimeout = 864000; //10 days (as seconds)
        });
    }
}
````

### Data Store

The default background job manager needs a data store to save and read jobs. It defines `IBackgroundJobStore` as an abstraction. So, you can replace the implementation if you want.

Background Jobs module implements `IBackgroundJobStore` using various data access providers. See its own [documentation](Modules/Background-Jobs.md).

> Background Jobs module is already installed to the startup templates by default and it works based on your ORM/data access choice.

## Integrations

Background job system is extensible and you can change the default background job manager with your own implementation or on of the pre-built integrations.

See pre-built job manager alternatives:

* [Hangfire Background Job Manager](Background-Jobs-Hangfire.md)
* [RabbitMQ Background Job Manager](Background-Jobs-RabbitMq.md)