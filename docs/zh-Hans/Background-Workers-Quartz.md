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

### 配置

参阅[配置](Background-Jobs-Quartz.md#配置).

### 创建后台工作者

后台工作者是一个继承自 `QuartzBackgroundWorkerBase` 基类的类. 一个简单的工作者如下所示:

```` csharp
public class MyLogWorker : QuartzBackgroundWorkerBase
{
    public MyLogWorker()
    {
        JobDetail = JobBuilder.Create<MyLogWorker>().Build();
        Trigger = TriggerBuilder.Create().StartNow().Build();
    }

    public override Task Execute(IJobExecutionContext context)
    {
        Logger.LogInformation("Executed MyLogWorker..!");
        return Task.CompletedTask;
    }
}
````

示例中我们重写了 `Execute` 方法写入日志. 后台工作者默认是**单例**. 如果你需要,也可以实现[依赖接口](Dependency-Injection.md#依赖接口)将其注册为其他的生命周期.

### 更多

参阅Quartz[文档](https://www.quartz-scheduler.net/documentation/index.html)了解更多信息.