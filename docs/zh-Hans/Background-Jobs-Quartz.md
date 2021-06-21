# Quartz 后台作业管理

[Quartz](https://www.quartz-scheduler.net/)是一个高级的作业管理. 你可以用ABP框架集成Quartz代替[默认后台作业管理](Background-Jobs.md). 通过这种方式你可以使用相同的后台作业API,将你的代码独立于Quartz. 如果你喜欢也可以直接使用Quartz的API.

> 参阅[后台作业文档](Background-Jobs.md),学习如何使用后台作业系统. 本文只介绍了如何安装和配置Quartz集成.

## 安装

建议使用[ABP CLI](CLI.md)安装包.

### 使用ABP CLI

在项目的文件夹(.csproj文件)中打开命令行窗口输入以下命令:

````bash
abp add-package Volo.Abp.BackgroundJobs.Quartz
````

### 手动安装

如果你想手动安装;

1. 添加 [Volo.Abp.BackgroundJobs.Quartz](https://www.nuget.org/packages/Volo.Abp.BackgroundJobs.Quartz) NuGet包添加到你的项目:

   ````
   Install-Package Volo.Abp.BackgroundJobs.Quartz
   ````

2. 添加 `AbpBackgroundJobsQuartzModule` 到你的模块的依赖列表:

````csharp
[DependsOn(
    //...other dependencies
    typeof(AbpBackgroundJobsQuartzModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
}
````

## 配置

Quartz是一个可配置的类库,对此ABP框架提供了 `AbpQuartzOptions`. 你可以在模块预配置此选项,ABP在初始化Quartz模块时将使用它. 例:

````csharp
[DependsOn(
    //...other dependencies
    typeof(AbpBackgroundJobsQuartzModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        PreConfigure<AbpQuartzOptions>(options =>
        {
            options.Properties = new NameValueCollection
            {
                ["quartz.jobStore.dataSource"] = "BackgroundJobsDemoApp",
                ["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz",
                ["quartz.jobStore.tablePrefix"] = "QRTZ_",
                ["quartz.serializer.type"] = "json",
                ["quartz.dataSource.BackgroundJobsDemoApp.connectionString"] = configuration.GetConnectionString("Quartz"),
                ["quartz.dataSource.BackgroundJobsDemoApp.provider"] = "SqlServer",
                ["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz",
            };
        });
    }
}
````

从ABP3.1版本开始,我们在 `AbpQuartzOptions` 添加了 `Configurator` 用于配置Quartz. 例:

````csharp
[DependsOn(
    //...other dependencies
    typeof(AbpBackgroundJobsQuartzModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        PreConfigure<AbpQuartzOptions>(options =>
        {
            options.Configurator = configure =>
            {
                configure.UsePersistentStore(storeOptions =>
                {
                    storeOptions.UseProperties = true;
                    storeOptions.UseJsonSerializer();
                    storeOptions.UseSqlServer(configuration.GetConnectionString("Quartz"));
                    storeOptions.UseClustering(c =>
                    {
                        c.CheckinMisfireThreshold = TimeSpan.FromSeconds(20);
                        c.CheckinInterval = TimeSpan.FromSeconds(10);
                    });
                });
            };
        });
    }
}
````

> 你可以选择你喜爱的方式来配置Quaratz.

Quartz**默认**将作业与调度信息存储在**内存**中,示例中我们使用[选项模式](Options.md)的预配置将其更改为存储到数据库中. 有关Quartz的更多配置请参阅[Quartz文档](https://www.quartz-scheduler.net/).

## 异常处理

### 默认异常处理策略

当后台作业发生异常时ABP提供了**默认**异常处理策略,它会为你的作业重试3次,每次间隔3秒. 你可以通过 `AbpBackgroundJobQuartzOptions` 更改默认重试次数与间隔时间:

```csharp
[DependsOn(
    //...other dependencies
    typeof(AbpBackgroundJobsQuartzModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobQuartzOptions>(options =>
        {
            options.RetryCount = 1;
            options.RetryIntervalMillisecond = 1000;
        });
    }
}
```

### 自定义异常处理策略

你可以通过 `AbpBackgroundJobQuartzOptions` 选项自定义异常处理策略:

```csharp
[DependsOn(
    //...other dependencies
    typeof(AbpBackgroundJobsQuartzModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpBackgroundJobQuartzOptions>(options =>
        {
            options.RetryStrategy = async (retryIndex, executionContext, exception) =>
            {
                // customize exception handling
            };
        });
    }
}
```