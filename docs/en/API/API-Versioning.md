# API Versioning System

ABP Framework integrates the [ASPNET-API-Versioning](https://github.com/dotnet/aspnet-api-versioning/wiki) feature and adapts to C# and JavaScript Static Client Proxies and [Auto API Controller](API/Auto-API-Controllers.md).


## Enable API Versioning

```cs
public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddAbpApiVersioning(options =>
    {
        // Show neutral/versionless APIs.
        options.UseApiBehavior = false;

        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
    });

    Configure<AbpAspNetCoreMvcOptions>(options =>
    {
        options.ChangeControllerModelApiExplorerGroupName = false;
    });
}
```

## C# and JavaScript Static Client Proxies

This feature does not compatible with [URL Path Versioning](https://github.com/dotnet/aspnet-api-versioning/wiki/Versioning-via-the-URL-Path), we suggest to use [Versioning-via-the-Query-String](https://github.com/dotnet/aspnet-api-versioning/wiki/Versioning-via-the-Query-String).

### Example

**Application Services:**
```cs
public interface IBookAppService : IApplicationService
{
    Task<BookDto> GetAsync();
}

public interface IBookV2AppService : IApplicationService
{
    Task<BookDto> GetAsync();

    Task<BookDto> GetAsync(string isbn);
}
```

**HttpApi Controllers:**
```cs
[Area(BookStoreRemoteServiceConsts.ModuleName)]
[RemoteService(Name = BookStoreRemoteServiceConsts.RemoteServiceName)]
[ApiVersion("1.0", Deprecated = true)]
[ApiController]
[ControllerName("Book")]
[Route("api/BookStore/Book")]
public class BookController : BookStoreController, IBookAppService
{
    private readonly IBookAppService _bookAppService;

    public BookController(IBookAppService bookAppService)
    {
        _bookAppService = bookAppService;
    }

    [HttpGet]
    public async Task<BookDto> GetAsync()
    {
        return await _bookAppService.GetAsync();
    }
}

[Area(BookStoreRemoteServiceConsts.ModuleName)]
[RemoteService(Name = BookStoreRemoteServiceConsts.RemoteServiceName)]
[ApiVersion("2.0")]
[ApiController]
[ControllerName("Book")]
[Route("api/BookStore/Book")]
public class BookV2Controller : BookStoreController, IBookV2AppService
{
    private readonly IBookV2AppService _bookAppService;

    public BookV2Controller(IBookV2AppService bookAppService)
    {
        _bookAppService = bookAppService;
    }

    [HttpGet]
    public async Task<BookDto> GetAsync()
    {
        return await _bookAppService.GetAsync();
    }

    [HttpGet]
    [Route("{isbn}")]
    public async Task<BookDto> GetAsync(string isbn)
    {
        return await _bookAppService.GetAsync(isbn);
    }
}
```

**Generated CS and JS proxies:**

```cs
[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IBookAppService), typeof(BookClientProxy))]
public partial class BookClientProxy : ClientProxyBase<IBookAppService>, IBookAppService
{
    public virtual async Task<BookDto> GetAsync()
    {
        return await RequestAsync<BookDto>(nameof(GetAsync));
    }
}

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IBookV2AppService), typeof(BookV2ClientProxy))]
public partial class BookV2ClientProxy : ClientProxyBase<IBookV2AppService>, IBookV2AppService
{
    public virtual async Task<BookDto> GetAsync()
    {
        return await RequestAsync<BookDto>(nameof(GetAsync));
    }

    public virtual async Task<BookDto> GetAsync(string isbn)
    {
        return await RequestAsync<BookDto>(nameof(GetAsync), new ClientProxyRequestTypeValue
        {
            { typeof(string), isbn }
        });
    }
}
```


```js
// controller bookStore.books.book

(function(){

abp.utils.createNamespace(window, 'bookStore.books.book');

bookStore.books.book.get = function(api_version, ajaxParams) {
    var api_version = api_version ? api_version : '1.0';
    return abp.ajax($.extend(true, {
    url: abp.appPath + 'api/BookStore/Book' + abp.utils.buildQueryString([{ name: 'api-version', value: api_version }]) + '',
    type: 'GET'
    }, ajaxParams));
};

})();

// controller bookStore.books.bookV2

(function(){

abp.utils.createNamespace(window, 'bookStore.books.bookV2');

bookStore.books.bookV2.get = function(api_version, ajaxParams) {
    var api_version = api_version ? api_version : '2.0';
    return abp.ajax($.extend(true, {
    url: abp.appPath + 'api/BookStore/Book' + abp.utils.buildQueryString([{ name: 'api-version', value: api_version }]) + '',
    type: 'GET'
    }, ajaxParams));
};

bookStore.books.bookV2.getAsyncByIsbn = function(isbn, api_version, ajaxParams) {
    var api_version = api_version ? api_version : '2.0';
    return abp.ajax($.extend(true, {
    url: abp.appPath + 'api/BookStore/Book/' + isbn + '' + abp.utils.buildQueryString([{ name: 'api-version', value: api_version }]) + '',
    type: 'GET'
    }, ajaxParams));
};

})();
```


## Changing version manually

If an application service class supports multiple versions. You can inject `ICurrentApiVersionInfo` to switch versions in C#.

```cs
var currentApiVersionInfo = _abpApplication.ServiceProvider.GetRequiredService<ICurrentApiVersionInfo>();
var bookV4AppService = _abpApplication.ServiceProvider.GetRequiredService<IBookV4AppService>();
using (currentApiVersionInfo.Change(new ApiVersionInfo(ParameterBindingSources.Query, "4.0")))
{
    book = await bookV4AppService.GetAsync();
    logger.LogWarning(book.Title);
    logger.LogWarning(book.ISBN);
}

using (currentApiVersionInfo.Change(new ApiVersionInfo(ParameterBindingSources.Query, "4.1")))
{
    book = await bookV4AppService.GetAsync();
    logger.LogWarning(book.Title);
    logger.LogWarning(book.ISBN);
}
```

We have made a default version in the JS proxy. Of course, you can also manually change the version.

```js

bookStore.books.bookV4.get("4.0") // Manually change the version.
//Title: Mastering ABP Framework V4.0

bookStore.books.bookV4.get() // The latest supported version is used by default.
//Title: Mastering ABP Framework V4.1
```

## Auto API Controller

```cs
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<AbpAspNetCoreMvcOptions>(options =>
    {
        //2.0 Version
        options.ConventionalControllers.Create(typeof(BookStoreWebAppModule).Assembly, opts =>
        {
            opts.TypePredicate = t => t.Namespace == typeof(BookStore.Controllers.ConventionalControllers.v2.TodoAppService).Namespace;
            opts.ApiVersions.Add(new ApiVersion(2, 0));
        });

        //1.0 Compatibility version
        options.ConventionalControllers.Create(typeof(BookStoreWebAppModule).Assembly, opts =>
        {
            opts.TypePredicate = t => t.Namespace == typeof(BookStore.Controllers.ConventionalControllers.v1.TodoAppService).Namespace;
            opts.ApiVersions.Add(new ApiVersion(1, 0));
        });
    });
}

public override void ConfigureServices(ServiceConfigurationContext context)
{
    var preActions = context.Services.GetPreConfigureActions<AbpAspNetCoreMvcOptions>();
    Configure<AbpAspNetCoreMvcOptions>(options =>
    {
        preActions.Configure(options);
    });

    context.Services.AddAbpApiVersioning(options =>
    {
        // Show neutral/versionless APIs.
        options.UseApiBehavior = false;

        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;

        options.ConfigureAbp(preActions.Configure());
    });

    Configure<AbpAspNetCoreMvcOptions>(options =>
    {
        options.ChangeControllerModelApiExplorerGroupName = false;
    });
}
```

## Swagger/VersionedApiExplorer

```cs

public override void ConfigureServices(ServiceConfigurationContext context)
{
    context.Services.AddAbpApiVersioning(options =>
    {
        // Show neutral/versionless APIs.
        options.UseApiBehavior = false;

        options.ReportApiVersions = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
    });

    context.Services.AddVersionedApiExplorer(
        options =>
        {
            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            options.GroupNameFormat = "'v'VVV";

            // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
            // can also be used to control the format of the API version in route templates
            options.SubstituteApiVersionInUrl = true;
        });

    context.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

    context.Services.AddAbpSwaggerGen(
        options =>
        {
            // add a custom operation filter which sets default values
            options.OperationFilter<SwaggerDefaultValues>();

            options.CustomSchemaIds(type => type.FullName);
        });

    Configure<AbpAspNetCoreMvcOptions>(options =>
    {
        options.ChangeControllerModelApiExplorerGroupName = false;
    });
}

public override void OnApplicationInitialization(ApplicationInitializationContext context)
{
    var app = context.GetApplicationBuilder();
    var env = context.GetEnvironment();

    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseErrorPage();
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAbpRequestLocalization();

    app.UseSwagger();
    app.UseSwaggerUI(
        options =>
        {
            var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
            // build a swagger endpoint for each discovered API version
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
        });

    app.UseConfiguredEndpoints();
}
```

## Custom multi-version API controller

ABP Framework will not affect to your APIs, you can freely implement your APIs according to the Microsoft's documentation.

Further information, see https://github.com/dotnet/aspnet-api-versioning/wiki


## Sample source code

Follow the link below to get the sample's complete source-code
https://github.com/abpframework/abp-samples/tree/master/Api-Versioning
