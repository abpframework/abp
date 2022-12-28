# ABP Dpar 集成

> 这个文档假设你已经熟悉[Dapr](https://dapr.io/)并且想在你的ABP应用中使用它.

[Dapr](https://dapr.io/) (分布式应用运行时)提供了简化微服务连接的API.它是一个开源项目,主要由微软支持.它也是CNCF(云原生计算基金会)项目,受到社区的信任.

ABP和Dapr有一些相似的特性,如服务到服务通信,分布式消息总线和分布式锁.然而,它们的目的完全不同.ABP的目标是通过提供自以为是的架构并提供必要的基础架构库,可重用模块和工具来正确实现该架构来提供端到端的开发人员体验.另一方面,Dapr的目的是提供一个运行时,将常见的微服务通信模式与应用程序逻辑解耦.

ABP和Dapr可以完美地在同一个应用程序中一起工作.ABP提供了一些包来提供更好的集成,其中Dapr功能与ABP相似.你可以根据[Dapr文档](https://docs.dapr.io/)使用其他Dapr功能,而不需要ABP集成包.

## ABP Dpar 集成包

ABP提供了以下NuGet包用于Dapr集成:

* [Volo.Abp.Dapr](https://www.nuget.org/packages/Volo.Abp.Dapr): 主要的Dapr集成包.所有其他包都依赖于此包.
* [Volo.Abp.Http.Client.Dapr](https://www.nuget.org/packages/Volo.Abp.Http.Client.Dapr): 与Dapr的[服务调用](https://docs.dapr.io/developing-applications/building-blocks/service-invocation/service-invocation-overview/)集成的ABP的[动态](../API/Dynamic-CSharp-API-Clients.md)和[静态](../API/Static-CSharp-API-Clients.md)C# API客户端代理系统集成包.
* [Volo.Abp.EventBus.Dapr](https://www.nuget.org/packages/Volo.Abp.EventBus.Dapr): 使用Dapr的[发布和订阅](https://docs.dapr.io/developing-applications/building-blocks/pubsub/)构建块实现ABP的分布式事件总线.使用此包,你可以发送事件,但不能接收.
* [Volo.Abp.AspNetCore.Mvc.Dapr.EventBus](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.Dapr.EventBus): 提供从Dapr的[发布和订阅](https://docs.dapr.io/developing-applications/building-blocks/pubsub/)构建块接收事件的端点.使用此包发送和接收事件.
* [Volo.Abp.DistributedLocking.Dapr](https://www.nuget.org/packages/Volo.Abp.DistributedLocking.Dapr): 使用Dapr的[分布式锁](https://docs.dapr.io/developing-applications/building-blocks/distributed-lock/)构建块为ABP框架的[分布式锁定](../Distributed-Locking.md)服务.

在以下部分中,我们将看到如何使用这些包在ABP基础解决方案中使用Dapr.

## 基础

### 安装

> 这个部分解释了如何将[Volo.Abp.Dapr](https://www.nuget.org/packages/Volo.Abp.Dapr)添加到你的项目中.如果你使用的是其他Dapr集成包,你可以跳过这个部分,因为这个包会被间接添加.

使用ABP CLI将[Volo.Abp.Dapr](https://www.nuget.org/packages/Volo.Abp.Dapr) NuGet包添加到你的项目中:

* 安装[ABP CLI](https://docs.abp.io/en/abp/latest/CLI)如果你之前没有安装过.
* 在你想要添加`Volo.Abp.Dapr`包的`.csproj`文件所在的目录中打开命令行(终端).
* 运行`abp add-package Volo.Abp.Dapr`命令.

如果你想手动添加,安装 [Volo.Abp.Dapr](https://www.nuget.org/packages/Volo.Abp.Dapr) NuGet包到你的项目中,并在项目内的[ABP模块](../Module-Development-Basics.md)类中添加`[DependsOn(typeof(AbpDaprModule))]`.

### AbpDaprOptions

`AbpDaprOptions` 是配置全局Dapr设置的主要[选项类](../Options.md).**所有设置都是可选的,你大多数情况下不需要配置它们.** 如果你需要,你可以在[模块类](../Module-Development-Basics.md)的`ConfigureServices`方法中配置它:

````csharp
Configure<AbpDaprOptions>(options =>
{
    // ...
});
````

可用的`AbpDaprOptions`类属性:

* `HttpEndpoint` (可选):创建`DaprClient`对象时使用的HTTP端点.如果你没有指定,将使用默认值.
* `GrpcEndpoint` (可选):创建`DaprClient`对象时使用的gRPC端点.如果你没有指定,将使用默认值.
* `DaprApiToken` (可选):应用程序向Dapr发送请求时使用的[Dapr API token](https://docs.dapr.io/operations/security/api-token/).默认情况下,它从`DAPR_API_TOKEN`环境变量中填充(配置后由 Dapr 设置).有关详细信息,请参阅本文档的*安全*部分.
* `AppApiToken` (可选):用于验证来自Dapr的请求的[应用程序API token](https://docs.dapr.io/operations/security/app-api-token/).默认情况下,它从`APP_API_TOKEN`环境变量中填充(配置后由 Dapr 设置).有关详细信息,请参阅本文档的*安全*部分.

或者, 你可以在 `appsettings.json` 文件的 `Dapr` 部分中配置选项.示例:

````csharp
"Dapr": {
  "HttpEndpoint": "http://localhost:3500/"
}
````

### 注入DaprClient

ABP 将 `DaprClient` 类注册到 [依赖注入](../Dependency-Injection.md) 系统中.因此,你可以在需要时注入并使用它:

````csharp
public class MyService : ITransientDependency
{
    private readonly DaprClient _daprClient;

    public MyService(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task DoItAsync()
    {
        // TODO: Use the injected _daprClient object
    }
}
````

注入 `DaprClient` 是在应用程序代码中使用它的推荐方法.当你注入它时,将使用 `IAbpDaprClientFactory` 服务创建它,这会在下一节中将进行说明.

### IAbpDaprClientFactory

`IAbpDaprClientFactory` 可用于创建 `DaprClient` 或 `HttpClient` 对象来执行对 Dapr 的操作.它使用 `AbpDaprOptions`,因此你可以配置设置.

**示例用法:**

````csharp
public class MyService : ITransientDependency
{
    private readonly IAbpDaprClientFactory _daprClientFactory;

    public MyService(IAbpDaprClientFactory daprClientFactory)
    {
        _daprClientFactory = daprClientFactory;
    }

    public async Task DoItAsync()
    {
        // Create a DaprClient object with default options
        DaprClient daprClient = await _daprClientFactory.CreateAsync();
        
        /* Create a DaprClient object with configuring
         * the DaprClientBuilder object */
        DaprClient daprClient2 = await _daprClientFactory
            .CreateAsync(builder =>
            {
                builder.UseDaprApiToken("...");
            });
        
        // Create an HttpClient object
        HttpClient httpClient = await _daprClientFactory
            .CreateHttpClientAsync("target-app-id");
    }
}
````

`CreateHttpClientAsync` 方法还获取可选的 `daprEndpoint` 和 `daprApiToken` 参数.

> ABP使用`IAbpDaprClientFactory`创建Dapr客户端.你也可以在应用程序中使用Dapr API创建客户端对象.推荐使用`IAbpDaprClientFactory`,但不是必需的.

## C# API 客户端代理集成

ABP可以[动态](../API/Dynamic-CSharp-API-Clients.md)或[静态](../API/Static-CSharp-API-Clients.md)生成代理类,以便从Dotnet客户端应用程序调用HTTP API.在分布式系统中使用HTTP API是非常合理的.[Volo.Abp.Http.Client.Dapr](https://www.nuget.org/packages/Volo.Abp.Http.Client.Dapr)包配置了客户端代理系统,因此它使用Dapr的服务调用构建块进行应用程序之间的通信.

### 安装

使用ABP CLI将[Volo.Abp.Http.Client.Dapr](https://www.nuget.org/packages/Volo.Abp.Http.Client.Dapr) NuGet包添加到项目(客户端):

* 安装[ABP CLI](https://docs.abp.io/en/abp/latest/CLI)如果你之前没有安装过.
* 在你想要添加`Volo.Abp.Http.Client.Dapr`包的`.csproj`文件所在的目录中打开命令行(终端).
* 运行`abp add-package Volo.Abp.Http.Client.Dapr`命令.

如果你想手动添加,安装 [Volo.Abp.Http.Client.Dapr](https://www.nuget.org/packages/Volo.Abp.Http.Client.Dapr) NuGet包到你的项目中,并在项目内的[ABP模块](../Module-Development-Basics.md)类中添加`[DependsOn(typeof(AbpHttpClientDaprModule))]`.

### 配置

当你安装了[Volo.Abp.Http.Client.Dapr](https://www.nuget.org/packages/Volo.Abp.Http.Client.Dapr) NuGet 包,所有你需要做的就是在`appsettings.json`或使用`AbpRemoteServiceOptions`[选项类](../Options.md)中配置ABP的远程服务选项.

**示例:**

````csharp
{
  "RemoteServices": {
    "Default": {
      "BaseUrl": "http://dapr-httpapi/"
    }
  }
}
````

`dapr-httpapi` 在这个例子中是你的Dapr配置中服务器应用程序的应用程序ID.

远程服务名称(示例中是`Default`)应该匹配动态客户端代理中`AddHttpClientProxies`调用或静态客户端代理中`AddStaticHttpClientProxies`调用中指定的远程服务名称.如果你的客户端只与一个服务器通信,使用`Default`是可以的.但是,如果你的客户端使用多个服务器,你通常在`RemoteServices`配置中有多个键. 你将远程服务端点配置为Dapr应用程序ID,在你使用ABP的客户端代理系统时它将自动工作并通过Dapr进行HTTP调用,

> 参阅[动态](../API/Dynamic-CSharp-API-Clients.md) 和 [static](../API/Static-CSharp-API-Clients.md)客户端代理文档,了解ABP的客户端代理系统的详细信息.

## 分布式事件总线集成

[ABP的分布式事件总线](../Distributed-Event-Bus.md)系统提供了一个方便的抽象,允许应用程序通过事件异步通信.ABP提供了各种分布式消息系统(如RabbitMQ,Kafka和Azure)的集成包.Dapr也有一个[发布和订阅构建块](https://docs.dapr.io/developing-applications/building-blocks/pubsub/pubsub-overview/),用于相同的目的:分布式消息/事件.

ABP的[Volo.Abp.EventBus.Dapr](https://www.nuget.org/packages/Volo.Abp.EventBus.Dapr)和[Volo.Abp.AspNetCore.Mvc.Dapr.EventBus](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.Dapr.EventBus)包可以使用Dapr基础设施来实现ABP的分布式事件总线.

任何类型的应用程序(例如,控制台或ASP.NET Core应用程序)都可以使用[Volo.Abp.EventBus.Dapr]包通过Dapr发布事件.为了能够接收消息(通过订阅事件),你需要安装[Volo.Abp.AspNetCore.Mvc.Dapr.EventBus]包,并且你的应用程序应该是ASP.NET Core应用程序.

### 安装

如果你的应用程序是ASP.NET Core应用程序并且你想发送和接收事件,你需要按照下面的描述安装[Volo.Abp.AspNetCore.Mvc.Dapr.EventBus]包:

* 安装[ABP CLI](https://docs.abp.io/en/abp/latest/CLI)如果你之前没有安装过.
* 在你想要添加`Volo.Abp.AspNetCore.Mvc.Dapr.EventBus`包的`.csproj`文件所在的目录中打开命令行(终端).
* 运行`abp add-package Volo.Abp.AspNetCore.Mvc.Dapr.EventBus`命令.

如果你想手动添加,安装 [Volo.Abp.AspNetCore.Mvc.Dapr.EventBus](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.Dapr.EventBus) NuGet包到你的项目中,并在项目内的[ABP模块](../Module-Development-Basics.md)类中添加`[DependsOn(typeof(AbpAspNetCoreMvcDaprEventBusModule))]`.

> **如果你安装了[Volo.Abp.AspNetCore.Mvc.Dapr.EventBus](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.Dapr.EventBus)包, 那么你不需要安装[Volo.Abp.EventBus.Dapr](https://www.nuget.org/packages/Volo.Abp.EventBus.Dapr)包,因为它已经由第一个包引用**

如果你的应用程序不是ASP.NET Core应用程序,你不能从Dapr接收事件,至少使用ABP的集成包(如果你想在不同类型的应用程序中接收事件,请参阅[Dapr的文档](https://docs.dapr.io/developing-applications/building-blocks/pubsub/howto-publish-subscribe/)).但是你仍然可以使用[Volo.Abp.EventBus.Dapr](https://www.nuget.org/packages/Volo.Abp.EventBus.Dapr)包发布消息.在这种情况下,请按照下面的步骤将该包安装到你的项目中:

* 安装[ABP CLI](https://docs.abp.io/en/abp/latest/CLI)如果你之前没有安装过.
* 在你想要添加`Volo.Abp.EventBus.Dapr`包的`.csproj`文件所在的目录中打开命令行(终端).
* 运行`abp add-package Volo.Abp.EventBus.Daprs`命令.

如果你想手动添加,安装 [Volo.Abp.Http.Client.Dapr](https://www.nuget.org/packages/Volo.Abp.EventBus.Dapr) NuGet包到你的项目中,并在项目内的[ABP模块](../Module-Development-Basics.md)类中添加`[DependsOn(typeof(AbpEventBusDaprModule))]`.

### 配置

你可以为Dapr配置`AbpDaprEventBusOptions`[选项类](../Options.md):

````csharp
Configure<AbpDaprEventBusOptions>(options =>
{
    options.PubSubName = "pubsub";
});
````

可用的`AbpDaprEventBusOptions`类的属性:

* `PubSubName` (可选): 通过`DaprClient.PublishEventAsync`方法发布消息时的`pubsubName`参数.默认值:`pubsub`.

### ABP订阅端点

ABP提供了以下端点来接收来自Dapr的事件:

* `dapr/subscribe`: Dapr使用此端点从应用程序获取订阅列表.ABP会自动返回所有分布式事件处理程序类和具有`Topic`属性的自定义控制器操作的订阅.
* `api/abp/dapr/event`: 用于接收来自Dapr的所有事件的统一端点.ABP根据主题名称将事件分派给您的事件处理程序.

> **由于ABP会在内部调用`MapSubscribeHandler` 方法,所以你不应该手动调用了.** 如果你想支持[CloudEvents](https://cloudevents.io/)标准,你可以在你的ASP.NET Core管道中使用`app.UseCloudEvents()`中间件.

### 用法

#### ABP的方式

你可以按照[ABP的分布式事件总线文档](../Distributed-Event-Bus.md)来学习如何以ABP的方式发布和订阅事件.你的应用程序代码不需要做任何改变就可以使用Dapr的发布-订阅功能.ABP将自动为你的事件处理程序类(实现`IDistributedEventHandler`接口)订阅Dapr.

ABP提供了 `api/abp/dapr/event`

**示例:使用`IDistributedEventBus`服务发布事件**

````csharp
public class MyService : ITransientDependency
{
    private readonly IDistributedEventBus _distributedEventBus;

    public MyService(IDistributedEventBus distributedEventBus)
    {
        _distributedEventBus = distributedEventBus;
    }

    public async Task DoItAsync()
    {
        await _distributedEventBus.PublishAsync(new StockCountChangedEto
        {
            ProductCode = "AT837234",
            NewStockCount = 42 
        });
    }
}
````

**示例:通过实现`IDistributedEventHandler`接口来订阅事件**

````csharp
public class MyHandler : 
    IDistributedEventHandler<StockCountChangedEto>,
    ITransientDependency
{
    public async Task HandleEventAsync(StockCountChangedEto eventData)
    {
        var productCode = eventData.ProductCode;
        // ...
    }
}
````

参阅[ABP的分布式事件总线文档](../Distributed-Event-Bus.md)来了解细节.

#### 使用Dapr API

在ABP的标准分布式事件总线系统之外,你还可以使用Dapr的API来发布事件.

> 如果你直接使用Dapr API来发布事件,你可能无法从ABP的标准分布式事件总线功能中受益,比如outbox/inbox模式的实现.

**示例:使用`DaprClient`发布事件**

````csharp
public class MyService : ITransientDependency
{
    private readonly DaprClient _daprClient;

    public MyService(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task DoItAsync()
    {
        await _daprClient.PublishEventAsync(
            "pubsub", // pubsub name
            "StockChanged", // topic name 
            new StockCountChangedEto // event data
            {
                ProductCode = "AT837234",
                NewStockCount = 42
            }
        );
    }
}
````

**示例:通过创建ASP.NET Core控制器来订阅事件**

````csharp
public class MyController : AbpController
{
    [HttpPost("/stock-changed")]
    [Topic("pubsub", "StockChanged")]
    public async Task<IActionResult> TestRouteAsync([FromBody] StockCountChangedEto model)
    {
        HttpContext.ValidateDaprAppApiToken();
        
        // Do something with the event
        return Ok();
    }
}
````

`HttpContext.ValidateDaprAppApiToken()` 扩展方法由ABP提供,用于检查请求是否来自Dapr.这是可选的.如果你想启用验证,你应该配置Dapr将App API令牌发送到你的应用程序.如果没有配置,`ValidateDaprAppApiToken()`不会执行任何操作.参阅[Dapr的App API令牌文档](https://docs.dapr.io/operations/security/app-api-token/)了解更多信息.还可以参阅本文档中的**AbpDaprOptions**和**安全**部分.

参阅[Dapr的文档](https://docs.microsoft.com/en-us/dotnet/architecture/dapr-for-net-developers/publish-subscribe)来了解使用Dapr API发送和接收事件的细节.

## 分布式锁

> Dapr的分布式锁功能目前处于Alpha阶段,可能还不稳定.在这一点上,不建议用Dapr来替换ABP的分布式锁.

ABP提供了一个[分布式锁](../Distributed-Locking.md)抽象来控制多个应用程序对共享资源的访问.Dapr也有一个[分布式锁构建块](https://docs.dapr.io/developing-applications/building-blocks/distributed-lock/).[Volo.Abp.DistributedLocking.Dapr](https://www.nuget.org/packages/Volo.Abp.DistributedLocking.Dapr)包使ABP使用Dapr的分布式锁系统.

### 安装

使用ABP CLI将[Volo.Abp.DistributedLocking.Dapr](https://www.nuget.org/packages/Volo.Abp.DistributedLocking.Dapr)NuGet包添加到项目(客户端):

* 安装[ABP CLI](https://docs.abp.io/en/abp/latest/CLI)如果你之前没有安装过.
* 在你想要添加`Volo.Abp.DistributedLocking.Dapr`包的`.csproj`文件所在的目录中打开命令行(终端).
* 运行`abp add-package Volo.Abp.DistributedLocking.Dapr`命令.

如果你想手动添加,安装 [Volo.Abp.DistributedLocking.Dapr](https://www.nuget.org/packages/Volo.Abp.AspNetCore.Mvc.Dapr.EventBus) NuGet包到你的项目中,并在项目内的[ABP模块](../Module-Development-Basics.md)类中添加`[DependsOn(typeof(AbpDistributedLockingDaprModule))]`.

### 配置

你可以在[你的模块](../Module-Development-Basics.md)的`ConfigureServices`方法中使用`AbpDistributedLockDaprOptions`选项类来配置Dapr分布式锁:

````csharp
Configure<AbpDistributedLockDaprOptions>(options =>
{
    options.StoreName = "mystore";
});
````

以下选项可用:

* **`StoreName`** (必需):Dapr使用的存储库名称.锁键名称在同一存储库中范围内.这意味着不同的应用程序可以在不同的存储库中获取相同的锁名称.对于要控制访问的相同资源,请使用相同的存储库名称.
* `Owner` (可选):`DaprClient.Lock`方法使用的`owner`值.如果你不指定,ABP使用一个随机值,这在一般情况下是可以的.
* `DefaultExpirationTimeout` (可选):锁过期后的默认值.默认值:2分钟.

### 用法

你可以注入并使用`IAbpDistributedLock`服务,就像在[分布式锁文档](../Distributed-Locking.md)中解释的那样.

**示例:**

````csharp
public class MyService : ITransientDependency
{
    private readonly IAbpDistributedLock _distributedLock;
    
    public MyService(IAbpDistributedLock distributedLock)
    {
        _distributedLock = distributedLock;
    }
    
    public async Task MyMethodAsync()
    {
        await using (var handle = 
                     await _distributedLock.TryAcquireAsync("MyLockName"))
        {
            if (handle != null)
            {
                // your code that access the shared resource
            }
        }   
    }
}
````

这里有两点关于`TryAcquireAsync`方法我们应该提到,与ABP的标准用法不同:

* `timeout` 参数目前没有使用(即使你指定了它),因为Dapr不支持等待获取锁.
* Dapr 使用过期超时系统(这意味着即使你不通过释放处理程序来释放锁,锁也会在超时后自动释放).但是,ABP的`TryAcquireAsync`方法没有这样的参数.目前,你可以在应用程序中将`AbpDistributedLockDaprOptions.DefaultExpirationTimeout`设置为全局值.

Dapr的分布式锁功能目前处于Alpha阶段,其API是可能会改变的候选者.如果你想要使用它,你可以这样做,但是要准备好未来的变化.目前,我们建议使用ABP的[分布式锁文档](../Distributed-Locking.md)中提到的[DistributedLock](https://github.com/madelson/DistributedLock)库.

## 安全

如果你使用Dapr,你的应用程序中的大部分或全部传入和传出请求都会通过Dapr.Dapr使用两种API令牌来保护应用程序与Dapr之间的通信.

### Dapr API Token

> 这个令牌默认情况下是自动设置的,通常你不需要关心它.

[在Dapr中启用API令牌身份验证](https://docs.dapr.io/operations/security/api-token/)文档描述了Dapr API令牌是什么以及如何配置.如果你想为你的应用程序启用它,请阅读该文档.

如果你启用了Dapr API令牌,你应该在你的应用程序中向Dapr发送该令牌.`AbpDaprOptions`定义了一个`DaprApiToken`属性,作为在你的应用程序中配置Dapr API令牌的中心点.

`DaprApiToken`属性的默认值是从`DAPR_API_TOKEN`环境变量设置的,并且该环境变量是在Dapr运行时设置的.所以,大多数情况下,你不需要在你的应用程序中配置`AbpDaprOptions.DaprApiToken`.但是,如果你需要配置(或覆盖它),你可以在模块类的`ConfigureServices`方法中这样做,如下面的代码块所示:

````csharp
Configure<AbpDaprOptions>(options =>
{
    options.DaprApiToken = "...";
});
````

或者你可以在`appsettings.json`文件中设置它:

````json
"Dapr": {
  "DaprApiToken": "..."
}
````

一旦你设置了它,它就会在你注入`DaprClient`或使用`IAbpDaprClientFactory`时使用.如果你需要在应用程序中使用该值,你可以注入`IDaprApiTokenProvider`并使用其`GetDaprApiToken()`方法.

### App API Token

> 启用App API令牌验证是强烈推荐的.否则,例如,任何客户端都可以直接调用你的事件订阅端点,你的应用程序就像发生了事件一样(如果你的事件订阅端点中没有其他安全策略).

[在Dapr中使用令牌身份验证请求身份验证](https://docs.dapr.io/operations/security/app-api-token/)文档描述了App API令牌是什么以及如何配置.如果你想为你的应用程序启用它,请阅读该文档.

如果你启用了App API令牌,你可以验证它以确保请求来自Dapr.ABP提供了有用的快捷方式来验证它.

**示例:在事件处理HTTP API中验证App API令牌**

````csharp
public class MyController : AbpController
{
    [HttpPost("/stock-changed")]
    [Topic("pubsub", "StockChanged")]
    public async Task<IActionResult> TestRouteAsync([FromBody] StockCountChangedEto model)
    {
        // Validate the App API token!
        HttpContext.ValidateDaprAppApiToken();
        
        // Do something with the event
        return Ok();
    }
}
````

`HttpContext.ValidateDaprAppApiToken()` 是ABP框架提供的扩展方法.如果HTTP头中缺少或错误的令牌,则会抛出`AbpAuthorizationException`(头名称为`dapr-api-token`).你也可以注入`IDaprAppApiTokenValidator`并使用其方法在任何服务中验证令牌(不仅仅是在控制器类中).

你可以配置`AbpDaprOptions.AppApiToken`,如果你想设置(或覆盖)App API令牌值.默认值由`APP_API_TOKEN`环境变量设置.你可以在模块类的`ConfigureServices`方法中这样做,如下面的代码块所示:

````csharp
Configure<AbpDaprOptions>(options =>
{
    options.AppApiToken = "...";
});
````

或者你可以在`appsettings.json`文件中设置它:

````json
"Dapr": {
  "AppApiToken": "..."
}
````

如果你需要在应用程序中使用该值,你可以注入`IDaprApiTokenProvider`并使用其`GetAppApiToken()`方法.

## 另请参阅

* [Dapr for .NET Developers](https://docs.microsoft.com/en-us/dotnet/architecture/dapr-for-net-developers/)
* [Dapr官方文档](https://docs.dapr.io/)
