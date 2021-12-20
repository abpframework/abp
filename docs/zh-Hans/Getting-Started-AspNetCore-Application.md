# 在AspNet Core MVC Web Application中使用ABP

本教程将介绍如何开始以最少的依赖关系开始使用ABP开发.

通常情况下你希望从 **[启动模板](Getting-Started-AspNetCore-MVC-Template.md)** 开始.

## 创建一个新项目

1. 使用Visual Studio 2022 (17.0.0+)创建一个新的ASPNET Core Web应用程序:

![](images/create-new-aspnet-core-application-v2.png)

2. 配置新的项目:

![](images/select-empty-web-application-v2.png)

3. 完成创建:

![](images/create-aspnet-core-application.png)


## 安装 Volo.Abp.AspNetCore.Mvc 包

Volo.Abp.AspNetCore.Mvc是ABP集成AspNet Core MVC的包,请安装它到你项目中:

````
Install-Package Volo.Abp.AspNetCore.Mvc
````

## 创建ABP模块

ABP是一个模块化框架,它需要一个**启动 (根) 模块**继承自``AbpModule``:

````C#
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace BasicAspNetCoreApplication
{
    [DependsOn(typeof(AbpAspNetCoreMvcModule))]
    public class AppModule : AbpModule
    {
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseConfiguredEndpoints();
        }
    }
}
````

``AppModule`` 是应用程序启动模块的好名称.

ABP的包定义了模块类,模块可以依赖其它模块.在上面的代码中 ``AppModule`` 依赖于 ``AbpAspNetCoreMvcModule`` (由[Volo.Abp.AspNetCore.Mvc](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc)包定义). 安装新的ABP的包后添加``DependsOn``特性是很常见的做法.

我们在此模块类中配置ASP.NET Core管道,而不是Startup类中.

### 启动类

接下来修改启动类集成到ABP模块系统:

````C#
using BasicAspNetCoreApplication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ReplaceConfiguration(builder.Configuration);

builder.Services.AddApplication<AppModule>();

var app = builder.Build();

app.InitializeApplication();

app.Run();

````

``services.AddApplication<AppModule>()``添加了从``AppModule``中启动的所有模块中定义的所有服务.

``app.InitializeApplication()`` 初始化并启动应用程序.

## 运行应用程序!

启动该应用,它将按预期运行.

## 使用 Autofac 依赖注入框架

虽然AspNet Core的依赖注入(DI)系统适用于基本要求,但[Autofac](https://autofac.org/)提供了属性注入和方法拦截等高级功能,这些功能是ABP执行高级应用程序框架功能所必需的.

用Autofac取代AspNet Core的DI系统并集成到ABP非常简单.

1. 安装 [Volo.Abp.Autofac](https://www.nuget.org/packages/Volo.Abp.Autofac) 包

````
Install-Package Volo.Abp.Autofac
````

2. 添加 ``AbpAutofacModule`` 依赖

````C#
[DependsOn(typeof(AbpAspNetCoreMvcModule))]
[DependsOn(typeof(AbpAutofacModule))] // 在模块上添加依赖AbpAutofacModule
public class AppModule : AbpModule
{
    ...
}
````

3. 修改``Program.cs``以使用Autofac:

````C#
using BasicAspNetCoreApplication;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseAutofac();  //Add this line

builder.Services.ReplaceConfiguration(builder.Configuration);

builder.Services.AddApplication<AppModule>();

var app = builder.Build();

app.InitializeApplication();

app.Run();
````

## 源码

从[此处](https://github.com/abpframework/abp-samples/tree/master/BasicAspNetCoreApplication)获取本教程中创建的示例项目的源代码.
