# Dinamik C# API Client

ABP dinamik olarak C# API client proxy leriyle uzak HTTP servis (RESP API) lerine gönderebilir. Bu şekilde, `HttpClient` ve düşük seviye HTTP özellikleriyle ile uğraşmanıza gerek kalmadan uzak bağlantıya istek gönderip cevap alabilirsiniz.

## Servis Interface

Sizin servis/controller ınız sunucu ile client arasında paylaşım sağlayan bir interface ile entegre olmalıdır. Yani, bir paylaşımlı kütüphane projesi oluşturup servis interface tanımlayalım. Örnek :


````csharp
public interface IBookAppService : IApplicationService
{
    Task<List<BookDto>> GetListAsync();
}
````

Sizin interface iniz `IRemoteService` ile entegre edilmeli ki otomatik olarak bulunsun. `IApplicationService` miras aldığı `IRemoteService` olduğundan beridir, `IBookAppService` şartı sağlar.

Bu class 'ı sizin servis uygulamanızda oluşturunuz. [Otomatik API controller sistemi](Auto-API-Controllers.md)ni de kullanabilir, REST API endpoint noktalarını görebilirsiniz.

## Client Proxy Oluşumu

Öncelikle [Volo.Abp.Http.Client](https://www.nuget.org/packages/Volo.Abp.Http.Client) nuget paketini client projenize ekleyiniz.

````
Install-Package Volo.Abp.Http.Client
````

Daha sonra `AbpHttpClientModule` bağımlılığını modülünüze ekleyiniz:

````csharp
[DependsOn(typeof(AbpHttpClientModule))] //bağımlıyı ekleyiniz
public class MyClientAppModule : AbpModule
{
}
````

Cient proxyleri oluşturmaya hazır haldedir. Örnek:

````csharp
[DependsOn(
    typeof(AbpHttpClientModule), //client proxy oluşturmak için
    typeof(BookStoreApplicationModule) //uygulama servis interfaceleri içerir
    )]
public class MyClientAppModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        //Create dynamic client proxies
        context.Services.AddHttpClientProxies(
            typeof(BookStoreApplicationModule).Assembly
        );
    }
}
````

`AddHttpClientProxies` metodu assembly leri getirip, verilen assembly deki tüm servis interfacelerini bulur. Proxy class larını kayıt edip oluşturur.

### Endpoint Configuration

`appsettings.json` dosyasında ki `RemoteServices` alanı, uzak servisler için ön ayar olarak kullanılır. Basit ayarlama aşşağıda gösterilmiştir:

````
{
  "RemoteServices": {
    "Default": {
      "BaseUrl": "http://localhost:53929/"
    } 
  } 
}
````

"AbpRemoteServiceOptions" alanı için detayları aşşağıda görebilirsiniz.

## Kullanış

Kullanımı basittir. Client uygulamasında servis interfaceini inject edip kullanabilirsiniz:

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

Bu örnek yukarıda tanımlanan `IBookAppService` servis interfasi inject eder. Dinamik client proxy entegrasyonu herhangi bir zaman method çağrıldığı zaman HTTP requesti oluşturur.

### IHttpClientProxy Interface

Client proxy kullanımında `IBookAppService` ı inject edebildiğiniz gibi, daha farklı kullanımlar için `IHttpClientProxy<IBookAppService>` ıda kullanabilirsiniz. Bu şekilde `IHttpClientProxy<T>` interface inin `Service` özelliğini kullanırsınız.

## Yapılandırma

### AbpRemoteServiceOptions

Varsayılan olarak `AbpRemoteServiceOptions` otomatik olarak `appsettings.json` dan değerleri alır. Alternatif olarak `Configure` metodunu kullanıp bu ayarlamayı ezebilirsiniz.

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

### Çoklu Uzak Servis Bağlantısı

Yukarıda ki ayarlama "Varsayılan" uzak bağlantı içindir. Sizin farklı bağlantı noktalarınız olabilir (microservis yaklaşımı gibi her microservisin farklı bağlantı noktası olur). Böyle bir senaryoda, farklı endpointleri yapılandırma dosyanıza ekleyebilirsiniz. 

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
Uzak servis adı için `AddHttpClientProxies` methodu farklı ek parametreler alabilir. Örneğin:

````csharp
context.Services.AddHttpClientProxies(
    typeof(BookStoreApplicationModule).Assembly,
    remoteServiceName: "BookStore"
);
````
`remoteServiceName` parametresi `AbpRemoteServiceOptions` da tanımlı uzak bağlantı noktası ile eşleşir. Eğer `BookStore` uzak bağlantı noktası tanımlı değilse `Default` uzak bağlantı noktası döner.

### As Default Services
Ne zaman `IBookAppService` proxy si oluşturursanız, siz direk olarak `IBookAppService` nı inject ederek proxy clientlerinin (kullanım alanında gösterildiği gibi) kullanımına sunarsınız. Değeri `asDefaultServices: false` olarak atarsanız, `AddHttpClientProxies` metodunun bu özelliğini devre dışı bırakabilirsiniz.

````csharp
context.Services.AddHttpClientProxies(
    typeof(BookStoreApplicationModule).Assembly,
    asDefaultServices: false
);
````

Sizin uygulamanızda  `asDefaultServices: false` şeklinde kullanım belki uygulamanız hali hazırda servis ile entegre edilmişse ve siz ovverride/değişim 'i client proxy nize istemiyorsanız ihtiyaç olabilir.

> Eğer `asDefaultServices` devre dışı ederseniz, sadece `IHttpClientProxy<T>` interfaceini client proxyleri için kullanabilirsiniz (yukarıda alanda göstermiştir)