# 动态 C# API 客户端

ABP可以自动创建C# API 客户端代理来调用远程HTTP服务(REST APIS).通过这种方式,你不需要通过 `HttpClient` 或者其他低级的HTTP功能调用远程服务并获取数据.

## 服务接口

你的service或controller需要实现一个在服务端和客户端共享的接口.因此,首先需要在一个共享的类库项目中定义一个服务接口.例如:

````csharp
public interface IBookAppService : IApplicationService
{
    Task<List<BookDto>> GetListAsync();
}
````

为了能自动被发现,你的接口需要实现`IRemoteService`接口.由于`IApplicationService`继承自`IRemoteService`接口.所以`IBookAppService`完全满足这个条件.

在你的服务中实现这个类,你可以使用[auto API controller system](Auto-API-Controllers.md)将你的服务暴漏为一个REST API 端点.

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
    typeof(BookStoreApplicationContractsModule) //包含应用服务接口
    )]
public class MyClientAppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //创建动态客户端代理
        context.Services.AddHttpClientProxies(
            typeof(BookStoreApplicationContractsModule).Assembly
        );
    }
}
````

`AddHttpClientproxies`方法获得一个程序集,找到这个程序集中所有的服务接口,创建并注册代理类.

### Endpoint配置

`appsettings.json`文件中的`RemoteServices`节点被用来设置默认的服务地址.下面是最简单的配置:

````
{
  "RemoteServices": {
    "Default": {
      "BaseUrl": "http://localhost:53929/"
    } 
  } 
}
````

查看下面的"AbpRemoteServiceOptions"章节获取更多详细配置.

## 使用

可以很直接地使用.只需要在你的客户端程序中注入服务接口:

````csharp
public class MyService : ITransientDependency
{
    private readonly IBookAppService _bookService;

    public MyService(IBookAppService bookService)
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

本例注入了上面定义的`IBookAppService`服务接口.当客户端调用服务方法的时候动态客户端代理就会创建一个HTTP调用.

### IHttpClientProxy接口

你可以像上面那样注入`IBookAppService`来使用客户端代理,也可以注入`IHttpClientProxy<IBookAppService>`获取更多明确的用法.这种情况下你可以使用`IHttpClientProxy<T>`接口的`Service`属性.

## 配置

### AbpRemoteServiceOptions

默认情况下`AbpRemoteServiceOptions`从`appsettings.json`获取.或者,你可以使用`Configure`方法来设置或重写它.如:

````csharp
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.Configure<AbpRemoteServiceOptions>(options =>
    {
        options.RemoteServices.Default =
            new RemoteServiceConfiguration("http://localhost:53929/");
    });
    
    //...
}
````

### 多个远程服务端点

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

`AddHttpClientProxies`方法有一个可选的参数来定义远程服务的名字:

````csharp
context.Services.AddHttpClientProxies(
    typeof(BookStoreApplicationContractsModule).Assembly,
    remoteServiceConfigurationName: "BookStore"
);
````

`remoteServiceConfigurationName`参数会匹配通过`AbpRemoteServiceOptions`配置的服务端点.如果`BookStore`端点没有定义就会使用默认的`Default`端点.

### 作为默认服务

当你为`IBookAppService`创建了一个服务代理,你可以直接注入`IBookAppService`来使用代理客户端(像上面章节中将的那样).你可以传递`asDefaultService:false`到`AddHttpClientProxies`方法来禁用此功能.

````csharp
context.Services.AddHttpClientProxies(
    typeof(BookStoreApplicationContractsModule).Assembly,
    asDefaultServices: false
);
````

如果你的程序中已经有一个服务的实现并且你不想用你的客户端代理重写或替换其他的实现,就需要使用`asDefaultServices:false`

> 如果你禁用了`asDefaultService`,你只能使用`IHttpClientProxy<T>`接口去使用客户端代理.(参见上面的相关章节).