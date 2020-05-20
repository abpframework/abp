# 对象扩展

ABP框架提供了 **实体扩展系统** 允许你 **添加额外属性** 到已存在的对象 **无需修改相关类**. 它允许你扩展[应用程序依赖模块](Modules/Index.md)实现的功能,尤其是当你要扩展[模块定义的实体](Customizing-Application-Modules-Extending-Entities.md)和[DTO](Customizing-Application-Modules-Overriding-Services.md)时.

> 你自己的对象通常不需要对象扩展系统,因为你可以轻松的添加常规属性到你的类中.

## IHasExtraProperties 接口

这是一个使类可扩展的接口. 它定义了 `Dictionary` 属性:

````csharp
Dictionary<string, object> ExtraProperties { get; }
````

然后你可以使用此字典添加或获取其他属性.

### 基类

默认以下基类实现了 `IHasExtraProperties` 接口:

* 由 `AggregateRoot` 类实现 (参阅 [entities](Entities.md)).
* 由 `ExtensibleEntityDto`, `ExtensibleAuditedEntityDto`... [DTO](Data-Transfer-Objects.md)基类实现.
* 由 `ExtensibleObject` 实现, 它是一个简单的基类,任何类型的对象都可以继承.

如果你的类从这些类继承,那么你的类也是可扩展的,如果没有,你也可以随时手动继承.

### 基本扩展方法

虽然可以直接使用类的 `ExtraProperties` 属性,但建议使用以下扩展方法使用额外属性.

#### SetProperty

用于设置额外属性值:

````csharp
user.SetProperty("Title", "My Title");
user.SetProperty("IsSuperUser", true);
````

`SetProperty` 返回相同的对象, 你可以使用链式编程:

````csharp
user.SetProperty("Title", "My Title")
    .SetProperty("IsSuperUser", true);
````

#### GetProperty

用于读取额外属性的值:

````csharp
var title = user.GetProperty<string>("Title");

if (user.GetProperty<bool>("IsSuperUser"))
{
    //...
}
````

* `GetProperty` 是一个泛型方法,对象类型做为泛型参数.
* 如果未设置给定的属性,则返回默认值 (`int` 的默认值为 `0` ,  `bool` 的默认值是 `false` ... 等).

##### 非基本属性类型

如果你的属性类型不是原始类型(int,bool,枚举,字符串等),你需要使用 `GetProperty` 的非泛型版本,它会返回 `object`.

#### HasProperty

用于检查对象之前是否设置了属性.

#### RemoveProperty

用于从对象中删除属性. 使用此方法代替为属性设置 `null` 值.

### 一些最佳实践

为属性名称使用魔术字符串很危险,因为你很容易输入错误的属性名称-这并不安全;

* 为你的额外属性名称定义一个常量.
* 使用扩展方法轻松设置你的属性.

示例:

````csharp
public static class IdentityUserExtensions
{
    private const string TitlePropertyName = "Title";

    public static void SetTitle(this IdentityUser user, string title)
    {
        user.SetProperty(TitlePropertyName, title);
    }

    public static string GetTitle(this IdentityUser user)
    {
        return user.GetProperty<string>(TitlePropertyName);
    }
}
````

然后, 你可以很容易地设置或获取 `Title` 属性:

````csharp
user.SetTitle("My Title");
var title = user.GetTitle();
````

## Object Extension Manager

你可以为可扩展对象(实现 `IHasExtraProperties`接口)设置任意属性,  `ObjectExtensionManager` 用于显式定义可扩展类的其他属性.

显式定义额外的属性有一些用例:

* 允许控制如何在对象到对象的映射上处理额外的属性 (参阅下面的部分).
* 允许定义属性的元数据. 例如你可以在使用[EF Core](Entity-Framework-Core.md)时将额外的属性映射到数据库中的表字段.

> `ObjectExtensionManager` 实现单例模式 (`ObjectExtensionManager.Instance`) ,你应该在应用程序启动之前定义对象扩展. [应用程序启动模板](Startup-Templates/Application.md) 有一些预定义的静态类,可以安全在内部定义对象扩展.

### AddOrUpdate

`AddOrUpdate` 是定义对象额外属性或更新对象额外属性的主要方法.

示例: 为 `IdentityUser` 实体定义额外属性:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdate<IdentityUser>(options =>
        {
            options.AddOrUpdateProperty<string>("SocialSecurityNumber");
            options.AddOrUpdateProperty<bool>("IsSuperUser");
        }
    );
````

### AddOrUpdateProperty

虽然可以如上所示使用 `AddOrUpdateProperty`, 但如果要定义单个额外的属性,也可以使用快捷的扩展方法:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUser, string>("SocialSecurityNumber");
````

有时将单个额外属性定义为多种类型是可行的. 你可以使用以下代码,而不是一个一个地定义:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<string>(
        new[]
        {
            typeof(IdentityUserDto),
            typeof(IdentityUserCreateDto),
            typeof(IdentityUserUpdateDto)
        },
        "SocialSecurityNumber"
    );
````

#### 属性配置

`AddOrUpdateProperty` 还可以为属性定义执行其他配置的操作.

Example:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUser, string>(
        "SocialSecurityNumber",
        options =>
        {
            options.CheckPairDefinitionOnMapping = false;
        });
````

> 参阅 "对象到对象映射" 部分了解 `CheckPairDefinitionOnMapping` 选项.

`options` 有一个名为 `Configuration` 的字典,该字典存储对象扩展定义甚至可以扩展. EF Core使用它来将其他属性映射到数据库中的表字段. 请参阅[扩展实体文档](Customizing-Application-Modules-Extending-Entities.md).

#### 默认值

自动为新属性设置默认值,默认值是属性类型的自然默认值,例如: `string`: `null` , `bool`: `false` 或 `int`: `0`.

有两种方法可以覆盖默认值:

##### DefaultValue 选项

`DefaultValue` 选项可以设置任何值:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUser, int>(
        "MyIntProperty",
        options =>
        {
            options.DefaultValue = 42;
        });
````

##### DefaultValueFactory 选项

`DefaultValueFactory` 可以设置返回默认值的函数:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUser, DateTime>(
        "MyDateTimeProperty",
        options =>
        {
            options.DefaultValueFactory = () => DateTime.Now;
        });
````

`options.DefaultValueFactory` 比 `options.DefaultValue` 优先级要高.

> 提示: 只有在默认值可能发生变化时(如示例中的`DateTime.Now;`) 才使用 `DefaultValueFactory`,如果是一个常量请使用 `DefaultValue` 选项.

#### CheckPairDefinitionOnMapping

控制在映射两个可扩展对象时如何检查属性定义. 请参阅*对象到对象映射*部分,了解 `CheckPairDefinitionOnMapping` 选项.

## Validation

你可能要为你定义的额外属性添加一些 **验证规则**. `AddOrUpdateProperty` 方法选项允许进行验证的方法有两种:

1. 你可以为属性添加 **数据注解 attributes**.
2. 你可以给定一个action(代码块)执行 **自定义验证**.

当你在**自动验证**的方法(例如:控制器操作,页面处理程序方法,应用程序服务方法...)中使用对象时,验证会工作. 因此,每当扩展对象被验证时,所有额外的属性都会被验证.

### 数据注解 Attributes

所有标准的数据注解Attributes对于额外属性都是有效的. 例:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUserCreateDto, string>(
        "SocialSecurityNumber",
        options =>
        {
            options.Attributes.Add(new RequiredAttribute());
            options.Attributes.Add(
                new StringLengthAttribute(32) {
                    MinimumLength = 6 
                }
            );
        });
````

使用以上配置,如果没有提供有效的 `SocialSecurityNumber` 值, `IdentityUserCreateDto` 对象将是无效的.

### 自定义验证

如果需要,可以添加一个自定义action验证额外属性. 例:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUserCreateDto, string>(
        "SocialSecurityNumber",
        options =>
        {
            options.Validators.Add(context =>
            {
                var socialSecurityNumber = context.Value as string;

                if (socialSecurityNumber == null ||
                    socialSecurityNumber.StartsWith("X"))
                {
                    context.ValidationErrors.Add(
                        new ValidationResult(
                            "Invalid social security number: " + socialSecurityNumber,
                            new[] { "SocialSecurityNumber" }
                        )
                    );
                }
            });
        });
````

`context.ServiceProvider` 可以解析服务.

除了为单个属性添加自定义验证逻辑外,还可以添加在对象级执行的自定义验证逻辑. 例:

````csharp
ObjectExtensionManager.Instance
.AddOrUpdate<IdentityUserCreateDto>(objConfig =>
{
    //Define two properties with their own validation rules
    
    objConfig.AddOrUpdateProperty<string>("Password", propertyConfig =>
    {
        propertyConfig.Attributes.Add(new RequiredAttribute());
    });

    objConfig.AddOrUpdateProperty<string>("PasswordRepeat", propertyConfig =>
    {
        propertyConfig.Attributes.Add(new RequiredAttribute());
    });

    //Write a common validation logic works on multiple properties
    
    objConfig.Validators.Add(context =>
    {
        if (context.ValidatingObject.GetProperty<string>("Password") !=
            context.ValidatingObject.GetProperty<string>("PasswordRepeat"))
        {
            context.ValidationErrors.Add(
                new ValidationResult(
                    "Please repeat the same password!",
                    new[] { "Password", "PasswordRepeat" }
                )
            );
        }
    });
});
````

## 对象到对象映射

假设你已向可扩展的实体对象添加了额外的属性并使用了自动[对象到对象的映射](Object-To-Object-Mapping.md)将该实体映射到可扩展的DTO类. 在这种情况下你需要格外小心,因为额外属性可能包含**敏感数据**,这些数据对于客户端不可用.

本节提供了一些**好的做法**,可以控制对象映射的额外属性.

### MapExtraPropertiesTo

`MapExtraPropertiesTo` 是ABP框架提供的扩展方法,用于以受控方式将额外的属性从一个对象复制到另一个对象. 示例:

````csharp
identityUser.MapExtraPropertiesTo(identityUserDto);
````

`MapExtraPropertiesTo` 需要在**两侧**(本例中是`IdentityUser` 和 `IdentityUserDto`)**定义属性**. 以将值复制到目标对象. 否则即使源对象(在此示例中为 `identityUser` )中确实存在该值,它也不会复制. 有一些重载此限制的方法.

#### MappingPropertyDefinitionChecks

`MapExtraPropertiesTo` 获取一个附加参数来控制单个映射操作的定义检查:

````csharp
identityUser.MapExtraPropertiesTo(
    identityUserDto,
    MappingPropertyDefinitionChecks.None
);
````

> 要小心,因为 `MappingPropertyDefinitionChecks.None` 会复制所有的额外属性而不进行任何检查. `MappingPropertyDefinitionChecks` 枚举还有其他成员.

如果要完全禁用属性的定义检查,可以在定义额外的属性(或更新现有定义)时进行,如下所示:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUser, string>(
        "SocialSecurityNumber",
        options =>
        {
            options.CheckPairDefinitionOnMapping = false;
        });
````

#### 忽略属性

你可能要在映射操作忽略某些属性:

````csharp
identityUser.MapExtraPropertiesTo(
    identityUserDto,
    ignoredProperties: new[] {"MySensitiveProp"}
);
````

忽略的属性不会复制到目标对象.

#### AutoMapper集成

如果你使用的是[AutoMapper](https://automapper.org/)库,ABP框架还提供了一种扩展方法来利用上面定义的 `MapExtraPropertiesTo` 方法.

你可以在映射配置文件中使用 `MapExtraProperties()` 方法.

````csharp
public class MyProfile : Profile
{
    public MyProfile()
    {
        CreateMap<IdentityUser, IdentityUserDto>()
            .MapExtraProperties();
    }
}
````

它与 `MapExtraPropertiesTo()` 方法具有相同的参数.

## Entity Framework Core 数据库映射

如果你使用的是EF Core,可以将额外的属性映射到数据库中的表字段. 例:

````csharp
ObjectExtensionManager.Instance
    .AddOrUpdateProperty<IdentityUser, string>(
        "SocialSecurityNumber",
        options =>
        {
            options.MapEfCore(b => b.HasMaxLength(32));
        }
    );
````

参阅 [Entity Framework Core 集成文档](Entity-Framework-Core.md) 了解更多内容.