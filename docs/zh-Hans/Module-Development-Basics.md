## 模块化

### 介绍

ABP本身是一个包含许多nuget包的模块化框架.它还提供了一个完整的基础架构来开发你自己的具有实体, 服务, 数据库集成, API, UI组件等等功能的应用程序模块.

### 模块类

每个模块都应该定义一个模块类.定义模块类的最简单方法是创建一个派生自``AbpModule``的类,如下所示:

````C#
public class BlogModule : AbpModule
{
            
}

````

#### 配置依赖注入和其他模块

##### ConfigureServices方法

``ConfigureServices``是将你的服务添加到依赖注入系统并配置其他模块的主要方法.例:

````C#
public class BlogModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //...
    }
}
````

你可以按照Microsoft的[文档](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)中的说明逐个注册依赖项.但ABP有一个**依照约定的依赖注册系统**,可以自动注册程序集中的所有服务.有关依赖项注入系统的更多信息,请参阅[依赖项注入](Dependency-Injection.md)文档.

你也可以通过这种方式配置其他服务和模块.例:

````C#
public class BlogModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //为应用程序配置默认的连接字符串
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = "......";
        });
    }
}
````
有关配置系统的更多信息,请参阅配置（TODO:link）文档.

##### 配置服务前和后

``AbpModule``类还定义了``PreConfigureServices``和``PostConfigureServices``方法用来在``ConfigureServices``之前或之后覆盖和编写你的代码.请注意,在这些方法中编写的代码将在所有其他模块的``ConfigureServices``方法之前/之后执行.

#### 应用程序初始化

一旦配置了所有模块的所有服务,应用程序就会通过初始化所有模块来启动.在此阶段,你可以从``IServiceProvider``中获取服务,因为这时它已准备就绪且可用.

##### OnApplicationInitialization方法

你可以在启动应用程序时覆盖``OnApplicationInitialization``方法来执行代码.例:

````C#
public class BlogModule : AbpModule
{
    //...

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var myService = context.ServiceProvider.GetService<MyService>();
        myService.DoSomething();
    }
}
````

``OnApplicationInitialization``通常由启动模块用于构建ASP.NET Core应用程序的中间件管道.例:

````C#
[DependsOn(typeof(AbpAspNetCoreMvcModule))]
public class AppModule : AbpModule
{
    //...

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseMvcWithDefaultRoute();
    }
}
````

如果模块需要,你还可以执行启动逻辑

##### 应用程序初始化前和后

``AbpModule``类还定义了``OnPreApplicationInitialization``和``OnPostApplicationInitialization``方法用来在``OnApplicationInitialization``之前或之后覆盖和编写你的代码.请注意,在这些方法中编写的代码将在所有其他模块的``OnApplicationInitialization``方法之前/之后执行.

#### 应用程序关闭

最后,如果要在应用程序关闭时执行某些代码,你可以覆盖``OnApplicationShutdown``方法.

### 模块依赖

在模块化应用程序中,一个模块依赖于另一个或几个模块并不罕见.如果一个Abp模块依赖于另一个模块,它必须声明``[DependsOn]``特性,如下所示:

````C#
[DependsOn(typeof(AbpAspNetCoreMvcModule))]
[DependsOn(typeof(AbpAutofacModule))]
public class BlogModule
{
    //...
}
````

你可以根据需要使用多个``DependsOn``特性或将多个模块类型传递给单个``DependsOn``特性.

依赖模块可能依赖于另一个模块,但你只需要定义直接依赖项.ABP在启动时会调查应用程序的依赖关系,并以正确的顺序初始化/关闭模块.

## 框架模块 vs 应用程序模块

**模块分为两种类型.** 这两种类型并没有任何结构上的区别,只是按功能和用途分类:

- **框架模块**: 这些是**框架的核心模块** 如缓存, 邮件, 主题, 安全, 序列化, 验证, EF Core集成, MongoDB集成... 等. 它们没有应用/业务功能,它们提供了日常开发经常用到的通用基础设施,集成和抽象.
- **应用程序模块**: 这些模块实现了 **特定的应用/业务功能** 像博客, 文档管理, 身份管理, 租户管理... 等等. 它们通常有自己的实体,服务,API和UI组件. 请参阅 [预构建的应用程序模块](Modules/Index.md).
