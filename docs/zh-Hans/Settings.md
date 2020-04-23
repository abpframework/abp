# Settings

[配置系统](Configuration.md) 是在启动时配置应用程序很好的方式. 除了配置之外, ABP提供了另外一种设置和获取应用程序设置的方式.

设置存储在动态数据源(通常是数据库)中的键值对. 设置系统预构建了用户,租户,全局和默认设置方法并且可以进行扩展.

## 定义设置

使用设置之前需要定义它. ABP是 [模块化](Module-Development-Basics.md)的, 不同的模块可以拥有不同的设置. 模块中派生 `SettingDefinitionProvider` 类定义模块内的配置. 示例如下:

````csharp
public class EmailSettingProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition("Smtp.Host", "127.0.0.1"),
            new SettingDefinition("Smtp.Port", "25"),
            new SettingDefinition("Smtp.UserName"),
            new SettingDefinition("Smtp.Password", isEncrypted: true),
            new SettingDefinition("Smtp.EnableSsl", "false")
        );
    }
}
````

ABP会自动发现并注册设置的定义.

### SettingDefinition

`SettingDefinition` 类具有以下属性:

* **Name**: 应用程序中设置的唯一名称. 是**具有约束的唯一属性**, 在应用程序获取/设置此设置的值 (设置名称定义为常量而不是`magic`字符串是个好主意).
* **DefaultValue**: 设置的默认值.
* **DisplayName**: 本地化的字符串,用于在UI上显示名称.
* **Description**: 本地化的字符串,用于在UI上显示描述.
* **IsVisibleToClients**: 布尔值,表示此设置是否在客户端可用. 默认为false,避免意外暴漏内部关键设置.
* **IsInherited**: 布尔值,此设置值是否从其他提供程序继承. 如果没有为请求的提供程序设置设定值,那么默认值是true并回退到下一个提供程序 (参阅设置值提供程序部分了解更多).
* **IsEncrypted**: 布尔值,表示是否在保存值是加密,读取时解密. 在数据库中存储加密的值.
* **Providers**: 限制可用于特定的设置值提供程序(参阅设置值提供程序部分了解更多).
* **Properties**: 设置此值的自定义属性 名称/值 集合,可以在之后的应用程序代码中使用.

### 更改依赖模块的设置定义

在某些情况下,你可能希望更改应用程序/模块所依赖的其他模块中定义的设置的某些属性. 设置定义提供程序可以查询和更新设置定义.

下面的示例中获取了由 [Volo.Abp.Emailing](Emailing.md) 包定义的设置并将其更改:

````csharp
public class MySettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        var smtpHost = context.GetOrNull("Abp.Mailing.Smtp.Host");
        if (smtpHost != null)
        {
            smtpHost.DefaultValue = "mail.mydomain.com";
            smtpHost.DisplayName =
                new LocalizableString(
                    typeof(MyLocalizationResource),
                    "SmtpServer_DisplayName"
                );
        }
    }
}
````

> 使用常量作为设置名称是一种好习惯,ABP的包就是这样做的. `Abp.Mailing.Smtp`设置名称是在`EmailSettingNames`类(在Volo.Abp.Emailing名称空间中)定义的常量.

## 读取设置值

### ISettingProvider

`ISettingProvider` 用于获取指定设置的值或所有设置的值. 示例用法:

````csharp
public class MyService
{
    private readonly ISettingProvider _settingProvider;

    //Inject ISettingProvider in the constructor
    public MyService(ISettingProvider settingProvider)
    {
        _settingProvider = settingProvider;
    }

    public async Task FooAsync()
    {
        //Get a value as string.
        string userName = await _settingProvider.GetOrNullAsync("Smtp.UserName");

        //Get a bool value and fallback to the default value (false) if not set.
        bool enableSsl = await _settingProvider.GetAsync<bool>("Smtp.EnableSsl");

        //Get a bool value and fallback to the provided default value (true) if not set.
        bool enableSsl = await _settingProvider.GetAsync<bool>(
            "Smtp.EnableSsl", defaultValue: true);

        //Get a bool value with the IsTrueAsync shortcut extension method
        bool enableSsl = await _settingProvider.IsTrueAsync("Smtp.EnableSsl");

        //Get an int value or the default value (0) if not set
        int port = (await _settingProvider.GetAsync<int>("Smtp.Port"));

        //Get an int value or null if not provided
        int? port = (await _settingProvider.GetOrNullAsync("Smtp.Port"))?.To<int>();
    }
}
````

> `ISettingProvider` 是非常常用的服务,一些基类中(如`IApplicationService`)已经将其属性注入. 这种情况下可以直接使用`SettingProvider`.

### 在客户端读取设置值

如果允许在客户端显示某个设置,可以使用 JavaScript 代码读取设置值. 示例:

````js
//Gets a value as string.
var language = abp.setting.get('Abp.Localization.DefaultLanguage');

//Gets an integer value.
var requiredLength = abp.setting.getInt('Abp.Identity.Password.RequiredLength');

//Gets a boolean value.
var requireDigit = abp.setting.getBoolean('Abp.Identity.Password.RequireDigit');
````

使用 `abp.setting.values` 可以读取所有设置值的字典.

## 设置值提供程序

设置系统是可扩展的, 你可以定义设置值提供程序扩展它,根据任何条件从任何来源获取设置值.

`ISettingProvider` 使用设置值提供程序来获取设置值. 如果值提供程序无法获取设置值,则会回退到下一个值提供程序.

有五个预构建设置值提供程序按以下顺序注册:

* `DefaultValueSettingValueProvider`: 从设置定义的默认值中获取值(参见上面的SettingDefinition部分).
* `ConfigurationSettingValueProvider`: 从[IConfiguration服务](Configuration.md)中获取值.
* `GlobalSettingValueProvider`: 获取设置的全局(系统范围)值.
* `TenantSettingValueProvider`: 获取当前租户的设置值(参阅 [多租户](Multi-Tenancy.md)文档).
* `UserSettingValueProvider`: 获取当前用户的设置值(参阅 [当前用户](CurrentUser.md) 文档).

> 设置回退系统从底部 (用户) 到 (默认) 方向起用用.

全局,租户和用户设置值提供程序使用 `ISettingStore` 从数据源读取值(参见下面的小节).

### 在应用程序配置中设置值

上一节提到 `ConfigurationSettingValueProvider` 从 `IConfiguration` 服务中读取设置, 该服务默认从 `appsettings.json` 中读取值. 所以在  `appsettings.json` 文件中配置设置值是最简单的方式.

例如你可以像以下方式一样配置 [IEmailSender](Emailing.md) 设置:

````json
{
  "Settings": {
    "Abp.Mailing.DefaultFromAddress": "noreply@mydomain.com",
    "Abp.Mailing.DefaultFromDisplayName": "My Application",
    "Abp.Mailing.Smtp.Host": "mail.mydomain.com",
    "Abp.Mailing.Smtp.Port": "547",
    "Abp.Mailing.Smtp.UserName": "myusername",
    "Abp.Mailing.Smtp.Password": "mySecretPassW00rd",
    "Abp.Mailing.Smtp.EnableSsl": "True"
  }
}
````

设置值应该在 `Settings` 部分配置,如本例所示.

> `IConfiguration`是.NET Core的服务,它不仅可以从 `appsettings.json` 中读取值,还可以从环境,用户机密...等中读取值. 有关更多信息请参阅[微软文档](https://docs.microsoft.com/zh-cn/aspnet/core/fundamentals/configuration/).

### 自定义设置值提供程序

扩展设置系统的方式是定义一个派生自 `SettingValueProvider` 的类. 示例:

````csharp
public class CustomSettingValueProvider : SettingValueProvider
{
    public override string Name => "Custom";

    public CustomSettingValueProvider(ISettingStore settingStore) 
        : base(settingStore)
    {
    }

    public override Task<string> GetOrNullAsync(SettingDefinition setting)
    {
        /* Return the setting value or null
           Use the SettingStore or another data source */
    }
}
````

> 或者你直接可以实现 `ISettingValueProvider` 接口. 这时需要记得将其注册到 [依赖注入](Dependency-Injection.md).

每一个提供程序都应该具有唯一的名称 (这里的名称是 "Custom" ). 内置提供程序使用给定的名称:

* `DefaultValueSettingValueProvider`: "**D**".
* `ConfigurationSettingValueProvider`: "**C**".
* `GlobalSettingValueProvider`: "**G**".
* `TenantSettingValueProvider`: "**T**".
* `UserSettingValueProvider`: "**U**".

最好使用一个字母的名称来减少数据库中的数据大小(提供者名称在每行中重复).

定义自定义设置值提供程序后,需要将其显式注册到 `AbpSettingOptions`:

````csharp
Configure<AbpSettingOptions>(options =>
{
    options.ValueProviders.Add<CustomSettingValueProvider>();
});
````

本示例将其添加到最后一项,因此它将成为`ISettingProvider`使用的第一个值提供程序. 你也可以将其添加到`options.ValueProviders`列表的另一个位置.

### ISettingStore

尽管设置值提供程序可以自由使用任何来源来获取设置值,但 `ISettingStore` 服务是设置值的默认来源. 全局,租户和用户设置值提供者都使用它.

## ISettingEncryptionService

`ISettingEncryptionService` 用于在设置定义的 `isencryption` 属性设置为 `true` 时加密/解密设置值.

你可以在依赖项入系统中替换此服务,自定义实现加密/解密过程. 默认实现 `StringEncryptionService` 使用AES算法(参见字符串[加密文档](String-Encryption.md)学习更多).

## 设置管理模块

设置系统核心是相当独立的,不做任何关于如何管理(更改)设置值的假设. 默认的`ISettingStore`实现也是`NullSettingStore`,它为所有设置值返回null.

设置管理模块通过管理数据库中的设置值来完成逻辑(实现`ISettingStore`).有关更多信息参阅[设置管理模块](Modules/Setting-Management.md)学习更多.
