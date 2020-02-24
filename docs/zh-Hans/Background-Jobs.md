# 后台作业

## 介绍

后台作业用来在后台里执行应用里的一些任务, 出于几个原因, 你可能需要后台工作, 以下是一些例子:

- 为执行**长时间运行的任务**而用户无需等待, 例如:用户按了一下"报告"按钮开始一个长时间运行的报告任务, 你把这个任务添加到**队列**里,并在完成后通过电子邮件将报告的结果发送给你的用户.
- 创建**可重试**和**持久的任务**以**确保**代码将**成功执行**. 例如, 你可以在后台作业中发送电子邮件以克服**临时故障**并**保证**最终发送. 这样用户不需要在发送电子邮件时等待.

后台作业是**持久性的**这意味着即使你的应用程序崩溃了, 后台左右也会在稍后**重试**并**执行**.

ABP为后台作业提供了一个**抽象**模块和几个后台作业**实现**. 它具有内置/默认的实现以及与Hangfire和RabbitMQ的集成.

## 抽象模块

ABP为后台作业提供了一个 **abstraction** 模块和 **多个实现**. 它有一个内置/默认实现以及Hangfire与RabbitMQ集成.

`Volo.Abp.BackgroundJobs.Abstractions` nuget package 提供了创建后台作业和队列作业所需要的服务. 如果你的模块只依赖这个包,那么它可以独立于其实现/集成.

> `Volo.Abp.BackgroundJobs.Abstractions` package 默认在启动模板中已经安装.

### 创建后台作业

后台作业是一个实现`IBackgroundJob<TArgs>`接口或继承自`BackgroundJob<TArgs>`类的类.`TArgs`是一个简单的C#类, 用于存储作业数据.

在示例中使用后台作业发送电子邮件,首先定义一个类来存储后台作业的参数

````csharp
public class EmailSendingArgs
{
    public string EmailAddress { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}
````

然后创建后台作业类,它使用 `EmailSendingArgs` 对象发送电子邮件:

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

这个作业简单的使用了 `IEmailSender` 发送电子邮件 (请参阅 [邮件发送文档](Emailing.md)).

#### 异常处理

后台作业不应该隐藏异常. 如果它抛出一个异常, 在稍后后台作业将会自动重试. 只有在你不想为当前参数重新运行后台作业时才隐藏异常.

### 队列作业

现在, 你可以使用 `IBackgroundJobManager` 服务向队列中添加一个发送电子邮件作业:

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
        //TODO: 创建一个新用户到数据库中...

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

刚才我们注入 `IBackgroundJobManager` 服务了并且使用它的 `EnqueueAsync` 方法添加一个新的作业到队列中.

Enqueue方法接收一些可选参数用于控制后台作业:

* **priority** 用于控制作业项的优先级. 它接收一个 `BackgroundJobPriority` 类型的枚举,它有 `Low`, `BelowNormal`, `Normal` (默认), `AboveNormal` 和 `Hight` 字段.
* **delay** 用于作业第一次重试之前的等待时间 (`TimeSpan`)类型.

### 禁用作业执行

你可能希望在你的应用程序中禁用后台作业执行. 如果你希望在另一个进程中执行后台作业并在当前进程中禁用它,通常可以使用以下命令.

使用 `AbpBackgroundJobOptions` 配置作业执行:

````csharp
[DependsOn(typeof(AbpBackgroundJobsModule))]
public class MyModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobOptions>(options =>
        {
            options.IsJobExecutionEnabled = false; //禁用作业执行
        });
    }
}
````

> 默认后台管理器(见下文)不支持多进程执行相同的作业队列. 所以, 如果你的应用程序中有多个正在运行的实现,并且使用的是默认的后台管理器, 你应该只在一个应用程序实例进程中启用作业队列.

## 默认后台作业管理器

ABP framework 包含一个简单的 `IBackgroundJobManager` 实现;

- 在**单线程**中**FIFO(先入先出)**.
- **重试**作业执行直到作业**执行成功**或**超时**. 默认作业超时时间是2天. 记录所有异常 .
- 作业执行成功时从存储中(数据库)**删除**作业. 如果超时, 作业会在数据库中被设置为**abandoned**.
- 作业的**重试等待时间会越来越长**. 作业第一次重试等待1分钟, 第二次重试等待2分钟, 第三次重试等待4分钟,以此类推.
- 以固定的时间间隔轮询存储中的作业. 查询作业, 按优先级排序(asc)然后按尝试次数排序(asc).

> `Volo.Abp.BackgroundJobs` nuget package 包含默认的后台作业管理器并且在默认在启动模板中已经安装.

### 配置

在你的[模块类](Module-Development-Basics.md)中使用 `AbpBackgroundJobWorkerOptions` 配置默认作业管理器.
示例中更改后台作业的的超时时间:

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

### 数据存储

默认的后台作业管理器需要数据存储用来保存和读取作业. 它将 `IBackgroundJobStore` 定义为抽象的. 所以, 如果你想要的话你可以替换它的实现.

后台作业模块使用各种数据访问提供程序实现 `IBackgroundJobStore`. 参阅 [后台工作模块文档](Modules/Background-Jobs.md).

> 默认情况下,后台作业模块已经安装到启动模板中,它基于你的ORM/数据访问选项.

## 集成

后台作业系统是可扩展的,你可以使用自己的实现或预先构建的集成更改默认后台作业管理器.

请参阅预构建的作业管理器备选方案:

* [Hangfire 后台作业管理器](Background-Jobs-Hangfire.md)
* [RabbitMQ 后台作业管理器](Background-Jobs-RabbitMq.md)