## 依赖注入

ABP的依赖注入系统是基于Microsoft的[依赖注入扩展](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)库（Microsoft.Extensions.DependencyInjection nuget包）开发的.因此,它的文档在ABP中也是有效的.

> 虽然ABP框架没有对任何第三方DI提供程序的核心依赖, 但它必须使用一个提供程序来支持动态代理(dynamic proxying)和一些高级特性以便ABP特性能正常工作.启动模板中已安装了Autofac. 更多信息请参阅 [Autofac 集成](Autofac-Integration.md) 文档.

### 模块化

由于ABP是一个模块化框架,因此每个模块都定义它自己的服务并在它自己的单独[模块类](Module-Development-Basics.md)中通过依赖注入进行注册.例:

````C#
public class BlogModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //在此处注入依赖项
    }
}
````

### 依照约定的注册

ABP引入了依照约定的服务注册.依照约定你无需做任何事,它会自动完成.如果要禁用它,你可以通过重写`PreConfigureServices`方法,设置`SkipAutoServiceRegistration`为`true`.

````C#
public class BlogModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        SkipAutoServiceRegistration = true;
    }
}
````

一旦跳过自动注册,你应该手动注册你的服务.在这种情况下,`AddAssemblyOf`扩展方法可以帮助你依照约定注册所有服务.例:

````c#
public class BlogModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        SkipAutoServiceRegistration = true;
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAssemblyOf<BlogModule>();
    }
}
````

以下部分解释了约定和配置.

#### 固有的注册类型

一些特定类型会默认注册到依赖注入.例子:

* 模块类注册为singleton.
* MVC控制器（继承``Controller``或``AbpController``）被注册为transient.
* MVC页面模型（继承``PageModel``或``AbpPageModel``）被注册为transient.
* MVC视图组件（继承``ViewComponent``或``AbpViewComponent``）被注册为transient.
* 应用程序服务（实现``IApplicationService``接口或继承``ApplicationService``类）注册为transient.
* 存储库（实现``IRepository``接口）注册为transient.
* 域服务（实现``IDomainService``接口）注册为transient.

示例:

````C#
public class BlogPostAppService : ApplicationService
{
}
````

``BlogPostAppService`` 由于它是从已知的基类派生的,因此会自动注册为transient生命周期.

#### 依赖接口

如果实现这些接口,则会自动将类注册到依赖注入:

* ``ITransientDependency`` 注册为transient生命周期.
* ``ISingletonDependency`` 注册为singleton生命周期.
* ``IScopedDependency`` 注册为scoped生命周期.

示例:

````C#
public class TaxCalculator : ITransientDependency
{
}
````

``TaxCalculator``因为实现了``ITransientDependency``,所以它会自动注册为transient生命周期.

#### Dependency 特性

配置依赖注入服务的另一种方法是使用``DependencyAttribute``.它具有以下属性:

* ``Lifetime``: 注册的生命周期:Singleton,Transient或Scoped.
* ``TryRegister``: 设置``true``则只注册以前未注册的服务.使用IServiceCollection的TryAdd ... 扩展方法.
* ``ReplaceServices``: 设置``true``则替换之前已经注册过的服务.使用IServiceCollection的Replace扩展方法.

示例:

````C#
[Dependency(ServiceLifetime.Transient, ReplaceServices = true)]
public class TaxCalculator
{

}

````

如果定义了``Lifetime``属性,则``Dependency``特性具有比其他依赖接口更高的优先级. 

#### ExposeServices 特性 

``ExposeServicesAttribute``用于控制相关类提供了什么服务.例: 

````C#
[ExposeServices(typeof(ITaxCalculator))]
public class TaxCalculator: ICalculator, ITaxCalculator, ICanCalculate, ITransientDependency
{

}
````

``TaxCalculator``类只公开``ITaxCalculator``接口.这意味着你只能注入``ITaxCalculator``,但不能注入``TaxCalculator``或``ICalculator``到你的应用程序中.

#### 依照约定公开的服务

如果你未指定要公开的服务,则ABP依照约定公开服务.以上面定义的``TaxCalculator``为例:

* 默认情况下,类本身是公开的.这意味着你可以按``TaxCalculator``类注入它.
* 默认情况下,默认接口是公开的.默认接口是由命名约定确定.在这个例子中,``ICalculator``和``ITaxCalculator``是``TaxCalculator``的默认接口,但``ICanCalculate``不是.

#### 组合到一起

只要有意义,特性和接口是可以组合在一起使用的.

````C#
[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(ITaxCalculator))]
public class TaxCalculator : ITaxCalculator, ITransientDependency
{

}
````

#### 手动注册

在某些情况下,你可能需要向``IServiceCollection``手动注册服务,尤其是在需要使用自定义工厂方法或singleton实例时.在这种情况下,你可以像[Microsoft文档](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)描述的那样直接添加服务.例:

````C#
public class BlogModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //注册一个singleton实例
        context.Services.AddSingleton<TaxCalculator>(new TaxCalculator(taxRatio: 0.18));

        //注册一个从IServiceProvider解析得来的工厂方法
        context.Services.AddScoped<ITaxCalculator>(sp => sp.GetRequiredService<TaxCalculator>());
    }
}
````

### 注入依赖关系

使用已注册的服务有三种常用方法.

#### 构造方法注入

这是将服务注入类的最常用方法.例如:

````C#
public class TaxAppService : ApplicationService
{
    private readonly ITaxCalculator _taxCalculator;

    public TaxAppService(ITaxCalculator taxCalculator)
    {
        _taxCalculator = taxCalculator;
    }

    public void DoSomething()
    {
        //...使用 _taxCalculator...
    }
}
````

``TaxAppService``在构造方法中得到``ITaxCalculator``.依赖注入系统在运行时自动提供所请求的服务.

构造方法注入是将依赖项注入类的首选方式.这样,除非提供了所有构造方法注入的依赖项,否则无法构造类.因此,该类明确的声明了它必需的服务.

#### 属性注入

Microsoft依赖注入库不支持属性注入.但是,ABP可以与第三方DI提供商（例如[Autofac](https://autofac.org/)）集成,以实现属性注入.例:

````C#
public class MyService : ITransientDependency
{
    public ILogger<MyService> Logger { get; set; }

    public MyService()
    {
        Logger = NullLogger<MyService>.Instance;
    }

    public void DoSomething()
    {
        //...使用 Logger 写日志...
    }
}
````

对于属性注入依赖项,使用公开的setter声明公共属性.这允许DI框架在创建类之后设置它.

属性注入依赖项通常被视为可选依赖项.这意味着没有它们,服务也可以正常工作.``Logger``就是这样的依赖项,``MyService``可以继续工作而无需日志记录.

为了使依赖项成为可选的,我们通常会为依赖项设置默认/后备(fallback)值.在此示例中,NullLogger用作后备.因此,如果DI框架或你在创建``MyService``后未设置Logger属性,则``MyService``依然可以工作但不写日志.

属性注入的一个限制是你不能在构造函数中使用依赖项,因为它是在对象构造之后设置的.

当你想要设计一个默认注入了一些公共服务的基类时,属性注入也很有用.如果你打算使用构造方法注入,那么所有派生类也应该将依赖的服务注入到它们自己的构造方法中,这使得开发更加困难.但是,对于非可选服务使用属性注入要非常小心,因为它使得类的要求难以清楚地看到.

#### 从IServiceProvider解析服务

你可能希望直接从``IServiceProvider``解析服务.在这种情况下,你可以将``IServiceProvider``注入到你的类并使用``GetService``方法,如下所示:

````C#
public class MyService : ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public MyService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void DoSomething()
    {
        var taxCalculator = _serviceProvider.GetService<ITaxCalculator>();
        //...
    }
}
````

#### 释放/处理（Releasing/Disposing）服务

如果你使用了构造函数或属性注入,则无需担心释放服务的资源.但是,如果你从``IServiceProvider``解析了服务,在某些情况下,你可能需要注意释放服务.

ASP.NET Core会在当前HTTP请求结束时释放所有服务,即使你直接从``IServiceProvider``解析了服务（假设你注入了IServiceProvider）.但是,在某些情况下,你可能希望释放/处理手动解析的服务:

* 你的代码在AspNet Core请求之外执行,执行者没有处理服务范围.
* 你只有对根服务提供者的引用.
* 你可能希望立即释放和处理服务（例如,你可能会创建太多具有大量内存占用且不想过度使用内存的服务）.

在任何情况下,你都可以使用这样的`using`代码块来安全地立即释放服务:

````C#
using (var scope = _serviceProvider.CreateScope())
{
    var service1 = scope.ServiceProvider.GetService<IMyService1>();
    var service2 = scope.ServiceProvider.GetService<IMyService2>();
}
````

两个服务在创建的scope被处理时（在using块的末尾）释放.

## 高级特性

### IServiceCollection.OnRegistred 事件

你可能想在注册到依赖注入的每个服务上执行一个操作, 在你的模块的 `PreConfigureServices` 方法中, 使用 `OnRegistred` 方法注册一个回调(callback) , 如下所示:

````csharp
public class AppModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistred(ctx =>
        {
            var type = ctx.ImplementationType;
            //...
        });
    }
}
````

`ImplementationType` 提供了服务类型. 该回调(callback)通常用于向服务添加拦截器. 例如:

````csharp
public class AppModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.OnRegistred(ctx =>
        {
            if (ctx.ImplementationType.IsDefined(typeof(MyLogAttribute), true))
            {
                ctx.Interceptors.TryAdd<MyLogInterceptor>();
            }
        });
    }
}
````

这个示例判断一个服务类是否具有 `MyLogAttribute` 特性, 如果有的话就添加一个 `MyLogInterceptor` 到拦截器集合中.

> 注意, 如果服务类公开了多于一个服务或接口, `OnRegistred` 回调(callback)可能被同一服务类多次调用. 因此, 较安全的方法是使用 `Interceptors.TryAdd` 方法而不是 `Interceptors.Add` 方法. 请参阅动态代理(dynamic proxying)/拦截器 [文档](Dynamic-Proxying-Interceptors.md).

## 第三方提供程序

虽然ABP框架没有对任何第三方DI提供程序的核心依赖, 但它必须使用一个提供程序来支持动态代理(dynamic proxying)和一些高级特性以便ABP特性能正常工作.

启动模板中已安装了Autofac. 更多信息请参阅 [Autofac 集成](Autofac-Integration.md) 文档.


### 请参阅

* [ASP.NET Core依赖注入最佳实践,提示和技巧](https://blog.abp.io/asp-net-core-dependency-injection-best-practices-tips-tricks)
