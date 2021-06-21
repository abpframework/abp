# 后台工作者

## 介绍

背景工人在应用简单独立的线程在后台运行.一般来说,他们定期运行,以执行一些任务.例子;
后台工作者在应用程序后台运行的简单的独立线程,一般来说它们定期运行执行一些任务.例如;

* 后台工作者可以定期**删除过时的日志**.
* 后台工作者可以定期检查**不活跃的用户**并且向其**发送邮件**使用户继续使用你的应用程序.

## 创建一个后台工作者

后台工作者应该直接或间接的继承 `IBackgroundWorker` 接口.

> 后台工作者是[单例](Dependency-Injection.md)的. 所以实例化运行你的工作者类的单个实例.

### BackgroundWorkerBase

`BackgroundWorkerBase` 是创建后台工作者的简单方法.

````csharp
public class MyWorker : BackgroundWorkerBase
{
    public override Task StartAsync(CancellationToken cancellationToken = default)
    {
        //...
    }

    public override Task StopAsync(CancellationToken cancellationToken = default)
    {
        //...
    }
}
````

`StartAsync` 开始你的工作者(在应用程序启动时),`StopAsync` 停止它(在应用程序关闭时).

> 你可以直接实现 `IBackgroundWorker`, 但 `BackgroundWorkerBase` 提供了一些像 `Logger` 的常用属性.

### AsyncPeriodicBackgroundWorkerBase

假设我们要设置用户为不活跃用户(如果用户最近30天未登录应用程序).`AsyncPeriodicBackgroundWorkerBase` 类简化了创建定期工作者的过程,我们在下面的示例中使用它:

````csharp
public class PassiveUserCheckerWorker : AsyncPeriodicBackgroundWorkerBase
{
    public PassiveUserCheckerWorker(
            AbpTimer timer,
            IServiceScopeFactory serviceScopeFactory
        ) : base(
            timer, 
            serviceScopeFactory)
    {
        Timer.Period = 600000; //10 minutes
    }

    protected async override Task DoWorkAsync(
        PeriodicBackgroundWorkerContext workerContext)
    {
        Logger.LogInformation("Starting: Setting status of inactive users...");

        //Resolve dependencies
        var userRepository = workerContext
            .ServiceProvider
            .GetRequiredService<IUserRepository>();

        //Do the work
        await userRepository.UpdateInactiveUserStatusesAsync();

        Logger.LogInformation("Completed: Setting status of inactive users...");
    }
}
````

* `AsyncPeriodicBackgroundWorkerBase` 使用 `AbpTimer`(线程安全定时器)对象来确定**时间段**. 我们可以在构造函数中设置了`Period` 属性.
* 它需要实现 `DoWorkAsync` 方法**执行**定期任务.
* 最好使用 `PeriodicBackgroundWorkerContext` **解析依赖** 而不是构造函数. 因为 `AsyncPeriodicBackgroundWorkerBase` 使用 `IServiceScope` 在你的任务执行结束时会对其 **disposed**.
* `AsyncPeriodicBackgroundWorkerBase` **捕获并记录** 由 `DoWorkAsync` 方法抛出的 **异常**.

## 注册后台工作者

创建一个后台工作者后,你应该将其添加到 `IBackgroundWorkerManager`. 最常见的地方是模块类的 `OnApplicationInitialization` 方法:

````csharp
[DependsOn(typeof(AbpBackgroundWorkersModule))]
public class MyModule : AbpModule
{
    public override void OnApplicationInitialization(
        ApplicationInitializationContext context)
    {
        context.AddBackgroundWorker<PassiveUserCheckerWorker>();
    }
}
````

`context.AddBackgroundWorker(...)` 是以下代码的简化扩展方法:

````csharp
context.ServiceProvider
    .GetRequiredService<IBackgroundWorkerManager>()
    .Add(
        context
            .ServiceProvider
            .GetRequiredService<PassiveUserCheckerWorker>()
    );
````

所以,它解析了给定的后台工作者并添加到 `IBackgroundWorkerManager`.

如果我们通常在 `OnApplicationInitialization` 添加工作者,但并不是强制的. 你可以在应用程序的任何地方注入 `IBackgroundWorkerManager` 并在运行时添加工作者. 在你的应用程序关闭时Background worker manager会释放所有已注册的后台工作者.

## Options

`AbpBackgroundWorkerOptions` 是用于设置后台工作者的选择. 目前只有一个选项:

* `IsEnabled` (默认值: true): 用于为你的应用程序启动或禁用后台工作者系统.

## 让应用程序始终运行

后台工作者只有在你的应用程序运行时才会工作. 如果你将后台作业托管在web应用程序中(这是默认行为),那么你应该确保你的web应用程序被配置为始终运行. 否则只有在你的应用程序正在运行时后台作业才会工作.

## 在集群运行

如果你在集群环境中运行同时运行应用程序的多个实现,这种情况下要小心,每个应用程序都运行相同的后台工作者,如果你的工作者在相同的资源上运行(例如处理相同的数据),那么可能会产生冲突.

如果这对你的工作者是一个问题,你有两个选项:

* 使用上面提到的 `AbpBackgroundWorkerOptions` 禁用其他的后台工作者系统,只保留一个实例.
* 所有的应用程序都禁用后台工作者系统,创建一个特殊的应用程序在一个服务上运行执行工作者.

## Quartz 集成

ABP框架的后台工作者系统可以很好的执行周期任务. 但是你可能需要使用更高级的任务调度,像[Quartz](https://www.quartz-scheduler.net/). 参阅社区贡献的[Quartz集成](Background-Workers-Quartz.md)

## 另请参阅

* [后台工作者的Quartz集成](Background-Workers-Quartz.md)
* [后台作业](Background-Jobs.md)