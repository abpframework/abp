# Quartz 后台工作者管理

[Quartz](https://www.quartz-scheduler.net/)是一个高级的后台工作者管理. 你可以用ABP框架集成Quartz代替[默认后台工作者管理](Background-Workers.md). ABP简单的集成了Quartz.

## 安装

建议使用[ABP CLI](CLI.md)安装包.

### 使用ABP CLI

在项目的文件夹(.csproj文件)中打开命令行窗口输入以下命令:

````bash
abp add-package Volo.Abp.BackgroundWorkers.Quartz
````

### 手动安装

如果你想手动安装;

1. 添加 [Volo.Abp.BackgroundWorkers.Quartz](https://www.nuget.org/packages/Volo.Abp.BackgroundWorkers.Quartz) NuGet包添加到你的项目:

   ````
   Install-Package Volo.Abp.BackgroundWorkers.Quartz
   ````

2. 添加 `AbpBackgroundWorkersQuartzModule` 到你的模块的依赖列表:

````csharp
[DependsOn(
    //...other dependencies
    typeof(AbpBackgroundWorkersQuartzModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
}
````

> Quartz后台工作者集成提供了 `QuartzPeriodicBackgroundWorkerAdapter` 来适配 `PeriodicBackgroundWorkerBase` 和 `AsyncPeriodicBackgroundWorkerBase` 派生类. 所以你依然可以按照[后台工作者文档](Background-Workers.md)来定义后台作业.
> `BackgroundJobWorker` 每5秒检查待执行作业,但是长时间的作业不会阻塞quartz. 所以安装Quartz后台工作者集成后,你同时需要安装[Quartz后台作业](Background-Jobs-Quartz.md)或[Hangfire后台作业](Background-Jobs-Hangfire.md)以避免重复执行作业.

## 配置

参阅[配置](Background-Jobs-Quartz.md#配置).

## 创建后台工作者

后台工作者是一个继承自 `QuartzBackgroundWorkerBase` 基类的类. 一个简单的工作者如下所示:

```` csharp
public class MyLogWorker : QuartzBackgroundWorkerBase
{
    public MyLogWorker()
    {
        JobDetail = JobBuilder.Create<MyLogWorker>().WithIdentity(nameof(MyLogWorker)).Build();
        Trigger = TriggerBuilder.Create().WithIdentity(nameof(MyLogWorker)).StartNow().Build();
    }

    public override Task Execute(IJobExecutionContext context)
    {
        Logger.LogInformation("Executed MyLogWorker..!");
        return Task.CompletedTask;
    }
}
````

示例中我们重写了 `Execute` 方法写入日志. 后台工作者默认是**单例**. 如果你需要,也可以实现[依赖接口](Dependency-Injection.md#依赖接口)将其注册为其他的生命周期.

> 提示: 为后台工作者添加标识是最佳实践,Quartz根据标识区分作业. 如果未指定标识会重复添加工作者到Quartz.

## 添加到BackgroundWorkerManager

默认后台工作者会在应用程序启动时**自动**添加到 `BackgroundWorkerManager`,如果你想要手动添加,可以将 `AutoRegister` 属性值设置为 `false`:

```` csharp
public class MyLogWorker : QuartzBackgroundWorkerBase
{
    public MyLogWorker()
    {
        AutoRegister = false;
        JobDetail = JobBuilder.Create<MyLogWorker>().WithIdentity(nameof(MyLogWorker)).Build();
        Trigger = TriggerBuilder.Create().WithIdentity(nameof(MyLogWorker)).StartNow().Build();
    }

    public override Task Execute(IJobExecutionContext context)
    {
        Logger.LogInformation("Executed MyLogWorker..!");
        return Task.CompletedTask;
    }
}
````

尽管你可以使用 `AutoRegister` 跳过自动添加,但如果你想要全局禁用这样会比较繁琐. 你可以通过 `AbpBackgroundWorkerQuartzOptions` 选项全局禁用:

```csharp
[DependsOn(
    //...other dependencies
    typeof(AbpBackgroundWorkersQuartzModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundWorkerQuartzOptions>(options =>
        {
            options.IsAutoRegisterEnabled = false;
        });
    }
}
```

## 高级主题

### 自定义ScheduleJob

例如你有一个每10分钟执行一次的工作者,但由于服务器不可用30分钟导致工作者错过了3次执行,你想要在服务器恢复正常后执行所有错过的执行. 你应该这样定义你的工作者:

```csharp
public class MyLogWorker : QuartzBackgroundWorkerBase
{
    public MyLogWorker()
    {
        JobDetail = JobBuilder.Create<MyLogWorker>().WithIdentity(nameof(MyLogWorker)).Build();
        Trigger = TriggerBuilder.Create().WithIdentity(nameof(MyLogWorker)).WithSimpleSchedule(s=>s.WithIntervalInMinutes(1).RepeatForever().WithMisfireHandlingInstructionIgnoreMisfires()).Build();

        ScheduleJob = async scheduler =>
        {
            if (!await scheduler.CheckExists(JobDetail.Key))
            {
                await scheduler.ScheduleJob(JobDetail, Trigger);
            }
        };
    }

    public override Task Execute(IJobExecutionContext context)
    {
        Logger.LogInformation("Executed MyLogWorker..!");
        return Task.CompletedTask;
    }
}
```

在示例中我们定义了工作者执行间隔为10分钟,并且设置 `WithMisfireHandlingInstructionIgnoreMisfires` ,另外自定义 `ScheduleJob` 仅当工作者不存在时向quartz添加调度作业.

## 更多

参阅Quartz[文档](https://www.quartz-scheduler.net/documentation/index.html)了解更多信息.