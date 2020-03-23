# 自定义应用模块: 重写服务

你可能想要**更改**依赖模块的**行为(业务逻辑)**. 在这种情况下,你可以使用[依赖注入](Dependency-Injection.md)的能力替换服务,控制器甚至页面模型到你自己的实现.

注册到依赖注入的任何类,包括ABP框架的服务都可以被**替换**.

你可以根据自己的需求使用不同的选项,下面的章节中将介绍这些选项.

> 请注意,某些服务方法可能不是virtual,你可能无法override,我们会通过设计将其virtual,如果你发现任何方法不可以被覆盖,请[创建一个issue](https://github.com/abpframework/abp/issues/new)或者你直接修改后并发送**pull request**到GitHub.

## 替换接口

如果给定的服务定义了接口,像 `IdentityUserAppService` 类实现了 `IIdentityUserAppService` 接口,你可以为这个接口创建自己的实现并且替换当前的实现. 例如:

````csharp
public class MyIdentityUserAppService : IIdentityUserAppService, ITransientDependency
{
    //...
}
````

`MyIdentityUserAppService` 通过命名约定替换了 `IIdentityUserAppService` 的当前实现. 如果你的类名不匹配,你需要手动公开服务接口:

````csharp
[ExposeServices(typeof(IIdentityUserAppService))]
public class TestAppService : IIdentityUserAppService, ITransientDependency
{
    //...
}
````

依赖注入系统允许为一个接口注册多个服务. 注入接口时会解析最后一个注入的服务. 显式的替换服务是一个好习惯.

示例:

````csharp
[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IIdentityUserAppService))]
public class TestAppService : IIdentityUserAppService, ITransientDependency
{
    //...
}
````

使用这种方法, `IIdentityUserAppService` 接口将只会有一个实现. 也可以使用以下方法替换服务:

````csharp
context.Services.Replace(
    ServiceDescriptor.Transient<IIdentityUserAppService, MyIdentityUserAppService>()
);
````

你可以在[模块](Module-Development-Basics.md)类的 `ConfigureServices` 方法编写替换服务代码.

## 重写一个服务类

大多数情况下,你会仅想改变服务当前实现的一个或几个方法. 重新实现完整的接口变的繁琐,更好的方法是继承原始类并重写方法。

### 示例: 重写服务方法

````csharp
[Dependency(ReplaceServices = true)]
public class MyIdentityUserAppService : IdentityUserAppService
{
    //...
    public MyIdentityUserAppService(
        IdentityUserManager userManager,
        IIdentityUserRepository userRepository,
        IGuidGenerator guidGenerator
    ) : base(
        userManager,
        userRepository,
        guidGenerator)
    {
    }

    public override async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
    {
        if (input.PhoneNumber.IsNullOrWhiteSpace())
        {
            throw new AbpValidationException(
                "Phone number is required for new users!",
                new List<ValidationResult>
                {
                    new ValidationResult(
                        "Phone number can not be empty!",
                        new []{"PhoneNumber"}
                    )
                }
            );        }

        return await base.CreateAsync(input);
    }
}
````

示例中**重写**了 `IdentityUserAppService` [应用程序](Application-Services.md) `CreateAsync` 方法检查手机号码. 然后调用了基类方法继续**基本业务逻辑**. 通过这种方法你可以在基本业务逻辑**之前**和**之后**执行其他业务逻辑.

你也可以完全**重写**整个业务逻辑去创建用户,而不是调用基类方法.

### 示例: 重写领域服务

````csharp
[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IdentityUserManager))]
public class MyIdentityUserManager : IdentityUserManager
{
    public MyIdentityUserManager(
        IdentityUserStore store, 
        IOptions<IdentityOptions> optionsAccessor, 
        IPasswordHasher<IdentityUser> passwordHasher,
        IEnumerable<IUserValidator<IdentityUser>> userValidators, 
        IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators, 
        ILookupNormalizer keyNormalizer, 
        IdentityErrorDescriber errors, 
        IServiceProvider services, 
        ILogger<IdentityUserManager> logger, 
        ICancellationTokenProvider cancellationTokenProvider
        ) : base(
            store, 
            optionsAccessor, 
            passwordHasher, 
            userValidators, 
            passwordValidators, 
            keyNormalizer, 
            errors, 
            services, 
            logger, 
            cancellationTokenProvider)
    {
    }

    public override async Task<IdentityResult> CreateAsync(IdentityUser user)
    {
        if (user.PhoneNumber.IsNullOrWhiteSpace())
        {
            throw new AbpValidationException(
                "Phone number is required for new users!",
                new List<ValidationResult>
                {
                    new ValidationResult(
                        "Phone number can not be empty!",
                        new []{"PhoneNumber"}
                    )
                }
            );
        }

        return await base.CreateAsync(user);
    }
}
````

示例中类继承了 `IdentityUserManager` [领域服务](Domain-Services.md),并且重写了 `CreateAsync` 方法进行了与之前相同的手机号码检查. 结果也是一样的,但是这次我们在领域服务实现了它,假设这是我们系统的**核心领域逻辑**.

> 这里需要 `[ExposeServices(typeof(IdentityUserManager))]`  attribute,因为 `IdentityUserManager` 没有定义接口 (像 `IIdentityUserManager`) ,依赖注入系统并不会按照约定公开继承类的服务(如已实现的接口).

参阅[本地化系统](Localization.md)了解如何自定义错误消息.

### 重写其他服务

控制器,框架服务,视图组件类以及其他类型注册到依赖注入的类都可以像上面的示例那样被重写.

## 如何找到服务?

[模块文档](Modules/Index.md) 包含了定义的主要服务列表. 另外 你也可以查看[源码](https://github.com/abpframework/abp/tree/dev/modules)找到所有的服务.