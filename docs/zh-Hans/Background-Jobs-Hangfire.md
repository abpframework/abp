# Hangfire后台作业管理

[Hangfire](https://www.hangfire.io/)是一个高级的后台作业管理. 你可以用ABP框架集成Hangfire代替[默认后台作业管理](Background-Jobs.md). 通过这种方式你可以使用相同的后台作业API,将你的代码独立于Hangfire. 如果你喜欢也可以直接使用Hangfire的API.

> 参阅[后台作业文档](Background-Jobs.md),学习如何使用后台作业系统. 本文只介绍了如何安装和配置Hangfire集成.

## 安装

建议使用[ABP CLI](CLI.md)安装包.

### 使用ABP CLI

在项目的文件夹(.csproj文件)中打开命令行窗口输入以下命令:

````bash
abp add-package Volo.Abp.BackgroundJobs.HangFire
````

### 手动安装

如果你想手动安装;

1. 添加 [Volo.Abp.BackgroundJobs.HangFire](https://www.nuget.org/packages/Volo.Abp.BackgroundJobs.HangFire) NuGet包添加到你的项目:

   ````
   Install-Package Volo.Abp.BackgroundJobs.HangFire
   ````

2. 添加 `AbpBackgroundJobsHangfireModule` 到你的模块的依赖列表:

````csharp
[DependsOn(
    //...other dependencies
    typeof(AbpBackgroundJobsHangfireModule) //Add the new module dependency
    )]
public class YourModule : AbpModule
{
}
````

## 配置

TODO...