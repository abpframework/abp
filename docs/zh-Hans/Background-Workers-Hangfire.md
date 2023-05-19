# Hangfire Background Worker Manager

[Hangfire](https://https://www.hangfire.io/)是一个高级的后台工作者管理. 你可以用ABP框架Hangfire集成代替[默认后台工作者管理](Background-Workers.md).

主要优点是你可以使用相同的服务器群来管理你的后台作业和工作线程以及利用 Hangfire 提供的[Recurring Jobs](https://docs.hangfire.io/en/latest/background-methods/performing-recurrent-tasks.html?highlight=recurring)高级调度功能.

## 安装

建议使用[ABP CLI](CLI.md)安装包.


### 使用ABP CLI

在项目的文件夹(.csproj文件)中打开命令行窗口输入以下命令:

````bash
abp add-package Volo.Abp.BackgroundWorkers.Hangfire
````

### 手动安装

如果你想手动安装;

1. 添加 [Volo.Abp.BackgroundWorkers.Hangfire](https://www.nuget.org/packages/Volo.Abp.BackgroundWorkers.Hangfire) NuGet包添加到你的项目:

   ````
   Install-Package Volo.Abp.BackgroundWorkers.Hangfire
   ````

2. 添加 `AbpBackgroundWorkersHangfireModule` 到你的模块的依赖列表:

````csharp
[DependsOn(
    //...other dependencies
    typeof(AbpBackgroundWorkersHangfireModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
}
````

> Hangfire后台工作者集成提供了 `HangfirePeriodicBackgroundWorkerAdapter` 来适配 `PeriodicBackgroundWorkerBase` 和 `AsyncPeriodicBackgroundWorkerBase` 派生类. 所以你依然可以按照[后台工作者文档](Background-Workers.md)来定义后台作业.

## 配置

你可以安装任何Hangfire存储. 最常用的是SQL Server(参阅[Hangfire.SqlServer](https://www.nuget.org/packages/Hangfire.SqlServer)NuGet包).

当你安装NuGet包后,你需要为你的项目配置Hangfire.

1.首先, 我们需要更改 `Module` 类 (例如: `<YourProjectName>HttpApiHostModule`) 的 `ConfigureServices` 方法去配置Hangfire存储和连接字符串:

````csharp
  public override void ConfigureServices(ServiceConfigurationContext context)
  {
      var configuration = context.Services.GetConfiguration();
      var hostingEnvironment = context.Services.GetHostingEnvironment();

      //... other configarations.

      ConfigureHangfire(context, configuration);
  }

  private void ConfigureHangfire(ServiceConfigurationContext context, IConfiguration configuration)
  {
      context.Services.AddHangfire(config =>
      {
          config.UseSqlServerStorage(configuration.GetConnectionString("Default"));
      });
  }
````

> 你必须为Hangfire配置一个存储

## 创建后台工作者

`HangfireBackgroundWorkerBase` 是创建一个后台工作者简单的方法.

```` csharp
public class MyLogWorker : HangfireBackgroundWorkerBase
{
    public MyLogWorker()
    {
        RecurringJobId = nameof(MyLogWorker);
        CronExpression = Cron.Daily();
    }

    public override Task DoWorkAsync(CancellationToken cancellationToken = default)
    {
        Logger.LogInformation("Executed MyLogWorker..!");
        return Task.CompletedTask;
    }
}
````

* **RecurringJobId** 是一个可选参数, 参阅 [Hangfire文档](https://docs.hangfire.io/en/latest/background-methods/performing-recurrent-tasks.html)
* **CronExpression** 是CRON表达式, 参阅 [CRON 表达式](https://en.wikipedia.org/wiki/Cron#CRON_expression)

> 你可以直接实现 `IHangfireBackgroundWorker`, 但是 `HangfireBackgroundWorkerBase` 提供了一些有用的属性,例如 `Logger`.

### UnitOfWork

```csharp
public class MyLogWorker : HangfireBackgroundWorkerBase, IMyLogWorker
{
    public MyLogWorker()
    {
        RecurringJobId = nameof(MyLogWorker);
        CronExpression = Cron.Daily();
    }

    public override Task DoWorkAsync(CancellationToken cancellationToken = default)
    {
        using (var uow = LazyServiceProvider.LazyGetRequiredService<IUnitOfWorkManager>().Begin())
        {
            Logger.LogInformation("Executed MyLogWorker..!");
            return Task.CompletedTask;
        }
    }
}
```

## 注册到后台工作者管理器

创建一个后台工作者后, 你应该添加到 `IBackgroundWorkerManager`, 最常用的地方是在你模块类的 `OnApplicationInitializationAsync` 方法中:

```` csharp
[DependsOn(typeof(AbpBackgroundWorkersModule))]
public class MyModule : AbpModule
{
    public override async Task OnApplicationInitializationAsync(
        ApplicationInitializationContext context)
    {
        await context.AddBackgroundWorkerAsync<MyLogWorker>();
    }
}
````

`context.AddBackgroundWorkerAsync(...)` 是一个是以下代码快捷的扩展方法:

```` csharp
context.ServiceProvider
    .GetRequiredService<IBackgroundWorkerManager>()
    .AddAsync(
        context
            .ServiceProvider
            .GetRequiredService<MyLogWorker>()
    );
````

它解析给定的后台工作者并添加到 `IBackgroundWorkerManager`.

虽然我们通常在 `OnApplicationInitializationAsync` 中添加后台工作者, 但对此没有限制. 你可以在任何地方注入 `IBackgroundWorkerManager` 并在运行时添加后台工作者.
