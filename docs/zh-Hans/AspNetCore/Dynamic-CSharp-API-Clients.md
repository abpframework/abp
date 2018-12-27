# 动态 C# API 客户端

ABP可以自动创建C# API 客户端代理来调用远程HTTP服务(REST APIS).通过这种方式,你不需要通过 `HttpClient` 或者其他低级的HTTP功能调用远程服务并获取数据.

## 服务接口

你的service或controller需要实现一个在服务端和客户端共享的接口.因此,首先需要在一个共享的类库项目中定义一个服务接口.例如:

````csharp
public interface IBookService : IApplicationService
{
    Task<List<BookDto>> GetListAsync();
}
````

你的接口需要实现`IRemoteService`接口.由于`IApplicationService`继承自`IRemoteService`接口.所以`IBookService`完全满足这个条件.

在你的服务中实现这个类,你可以使用[Auto API Controller](Auto-API-Controllers.md)将你的服务暴漏为一个REST API 端点.

## 客户端代理生成

首先,将[Volo.Abp.Http.Client](https://www.nuget.org/packages/Volo.Abp.Http.Client) nuget包添加到你的客户端项目中:

````
Install-Package Volo.Abp.Http.Client
````

然后给你的模块添加`AbpHttpClientModule`依赖:

````csharp
[DependsOn(typeof(AbpHttpClientModule))] //添加依赖
public class MyClientAppModule : AbpModule
{
}
````

现在,已经可以创建客户端代理了.例如:

````csharp
[DependsOn(
    typeof(AbpHttpClientModule), //用来创建客户端代理
    typeof(BookStoreApplicationModule) //包含应用服务接口
    )]
public class MyClientAppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //配置远程端点
        context.Services.Configure<RemoteServiceOptions>(options =>
        {
            options.RemoteServices.Default =
                new RemoteServiceConfiguration("http://localhost:53929/");
        });

        //创建动态客户端代理
        context.Services.AddHttpClientProxies(
            typeof(BookStoreApplicationModule).Assembly
        );
    }
}
````

`RemoteServiceOptions`被用来为远程服务配置端点(本例设置了默认的端点,当然你也可以拥有不同的服务端点供不同的客户端使用.参考本文"多个远程服务端点"小节).

`AddHttpClientproxies`方法获得一个程序集,找到这个程序集中所有的服务接口,创建并注册代理类.

## 使用

可以很直接地使用.只需要在你的客户端程序中注入服务接口:

````csharp
public class MyService : ITransientDependency
{
    private readonly IBookService _bookService;

    public MyService(IBookService bookService)
    {
        _bookService = bookService;
    }

    public async Task DoIt()
    {
        var books = await _bookService.GetListAsync();
        foreach (var book in books)
        {
            Console.WriteLine($"[BOOK {book.Id}] Name={book.Name}");
        }
    }
}
````

本例注入了上面定义的`IBookService`服务接口.当客户端调用服务方法的时候动态客户端代理就会创建一个HTTP调用.

## 详细配置

### RemoteServiceOptions

你可以像上面展示的那样配置`RemoteServiceOptions`.也可以从`appsettings.json`文件中读取.在你的`appsettings.json`文件中添加`RemoteServices`节点:

````json
{
  "RemoteServices": {
    "Default": {
      "BaseUrl": "http://localhost:53929/"
    } 
  } 
}
````

然后你可以像下面这样将`IConfigurationRoot`实例直接传递到`Configure<RemoteServiceOptions>()`方法中:

````csharp
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

context.Services.Configure<RemoteServiceOptions>(configuration);
````

这种方式对于不修改代码来改变配置是非常有用的.

#### 多个远程服务端点

上面的例子已经配置了"Default"远程服务端点.你可能需要为不同的服务创建不同的端点.(就像在微服务方法中一样,每个微服务具有不同的端点).在这种情况下,你可以在你的配置文件中添加其他的端点:

````json
{
  "RemoteServices": {
    "Default": {
      "BaseUrl": "http://localhost:53929/"
    },
    "BookStore": {
      "BaseUrl": "http://localhost:48392/"
    } 
  } 
}
````

下一节学习如何使用这个新的端点.

### AddHttpClientProxies方法

`AddHttpClientProxies`方法有一个可选的参数来定义远程服务的名字:

````csharp
context.Services.AddHttpClientProxies(
    typeof(BookStoreApplicationModule).Assembly,
    remoteServiceName: "BookStore"
);
````

`remoteServiceName`参数会匹配通过`RemoteServiceOptions`配置的服务端点.如果`BookStore`端点没有定义就会使用默认的`Default`端点.