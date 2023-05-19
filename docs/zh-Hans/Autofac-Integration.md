# 集成 Autofac 

Autofac 是.Net世界中最常用的依赖注入框架之一. 相比.Net Core标准的依赖注入库, 它提供了更多高级特性, 比如动态代理和属性注入.

## 安装 Autofac 

> 所有的启动模板和示例都已经集成了 Autofac. 所以, 多数时候你无需手动安装这个包.

安装 [Volo.Abp.Autofac](https://www.nuget.org/packages/Volo.Abp.Autofac) nuget 包到你的项目 (对于一个多项目应用程序, 建议安装到可执行项目或者Web项目中.)

````
Install-Package Volo.Abp.Autofac
````

然后为你的模块添加 `AbpAutofacModule` 依赖:

```csharp
using Volo.Abp.Modularity;
using Volo.Abp.Autofac;

namespace MyCompany.MyProject
{
    [DependsOn(typeof(AbpAutofacModule))]
    public class MyModule : AbpModule
    {
        //...
    }
}
```

最后, 配置 `AbpApplicationCreationOptions` 用 Autofac 替换默认的依赖注入服务. 根据应用程序类型, 情况有所不同.

### ASP.NET Core 应用程序

如下所示, 在 **Program.cs** 文件中调用 `UseAutofac()`:

````csharp
public class Program
{
    public static int Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    internal static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .UseAutofac(); //Integrate Autofac!
}
````

### 控制台应用程序

如下所示, 在 `AbpApplicationFactory.Create` 中用options调用 `UseAutofac()` 方法:

````csharp
using System;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;

namespace AbpConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var application = AbpApplicationFactory.Create<AppModule>(options =>
            {
                options.UseAutofac(); //Autofac integration
            }))
            {
                //...
            }
        }
    }
}
````

