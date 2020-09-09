# 当前用户

在Web应用程序中检索有关已登录用户的信息是很常见的. 当前用户是与Web应用程序中的当前请求相关的活动用户.

## ICurrentUser

`ICurrentUser` 是主要的服务,用于获取有关当前活动的用户信息.

示例: [注入](Dependency-Injection.md) `ICurrentUser` 到服务中:

````csharp
using System;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Users;

namespace AbpDemo
{
    public class MyService : ITransientDependency
    {
        private readonly ICurrentUser _currentUser;

        public MyService(ICurrentUser currentUser)
        {
            _currentUser = currentUser;
        }
        
        public void Foo()
        {
            Guid? userId = _currentUser.Id;
        }
    }
}
````

公共基类已经将此服务作为基本属性注入. 例如你可以直接在[应用服务](Application-Services.md)中使用 `CurrentUser` 属性:

````csharp
using System;
using Volo.Abp.Application.Services;

namespace AbpDemo
{
    public class MyAppService : ApplicationService
    {
        public void Foo()
        {
            Guid? userId = CurrentUser.Id;
        }
    }
}
````

### 属性

以下是 `ICurrentUser` 接口的基本属性:

* **IsAuthenticated** 如果当前用户已登录(已认证),则返回 `true`. 如果用户尚未登录,则 `Id` 和 `UserName` 将返回 `null`.
* **Id** (Guid?): 当前用户的Id,如果用户未登录,返回 `null`.
* **UserName** (string): 当前用户的用户名称. 如果用户未登录,返回 `null`.
* **TenantId** (Guid?): 当前用户的租户Id. 对于[多租户](Multi-Tenancy.md) 应用程序很有用. 如果当前用户未分配给租户,返回 `null`.
* **Email** (string): 当前用户的电子邮件地址. 如果当前用户尚未登录或未设置电子邮件地址,返回 `null`.
* **EmailVerified** (bool): 如果当前用户的电子邮件地址已经过验证,返回 `true`.
* **PhoneNumber** (string): 当前用户的电话号码. 如果当前用户尚未登录或未设置电话号码,返回 `null`.
* **PhoneNumberVerified** (bool): 如果当前用户的电话号码已经过验证,返回 `true`.
* **Roles** (string[]): 当前用户的角色. 返回当前用户角色名称的字符串数组.

### Methods

`ICurrentUser` 是在 `ICurrentPrincipalAccessor` 上实现的(请参阅以下部分),并可以处理声明. 实际上所有上述属性都是从当前经过身份验证的用户的声明中检索的.

如果你有自定义声明或获取其他非常见声明类型, `ICurrentUser` 有一些直接使用声明的方法.

* **FindClaim**: 获取给定名称的声明,如果未找到返回 `null`.
* **FindClaims**: 获取具有给定名称的所有声明(允许具有相同名称的多个声明值).
* **GetAllClaims**: 获取所有声明.
* **IsInRole**: 一种检查当前用户是否在指定角色中的简化方法.

除了这些标准方法,还有一些扩展方法:

* **FindClaimValue**: 获取具有给定名称的声明的值,如果未找到返回 `null`. 它有一个泛型重载将值强制转换为特定类型.
* **GetId**: 返回当前用户的 `Id`. 如果当前用户没有登录它会抛出一个异常(而不是返回`null`). 仅在你确定用户已经在你的代码上下文中进行了身份验证时才使用此选项.

### 验证和授权

`ICurrentUser` 的工作方式与用户的身份验证或授权方式无关. 它可以与使用当前主体的任何身份验证系统无缝地配合使用(请参阅下面的部分).

## ICurrentPrincipalAccessor

`ICurrentPrincipalAccessor` 是当需要当前用户的Principal时使用的服务(由ABP框架和你的应用程序代码使用).

对于Web应用程序, 它获取当前 `HttpContext` 的 `User` 属性,对于非Web应用程序它将返回 `Thread.CurrentPrincipal`.

> 通常你不需要这种低级别的 `ICurrentPrincipalAccessor` 服务,直接使用上述的 `ICurrentUser` 即可.

### 基本用法

你可以注入 `ICurrentPrincipalAccessor` 并且使用 `Principal` 属性获取当前principal:

````csharp
public class MyService : ITransientDependency
{
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

    public MyService(ICurrentPrincipalAccessor currentPrincipalAccessor)
    {
        _currentPrincipalAccessor = currentPrincipalAccessor;
    }
    
    public void Foo()
    {
        var allClaims = _currentPrincipalAccessor.Principal.Claims.ToList();
        //...
    }
}
````

### 更改当前Principal

除了某些高级场景外,你不需要设置或更改当前Principal. 如果需要可以使用 `ICurrentPrincipalAccessor` 的 `Change` 方法. 它接受一个 `ClaimsPrincipal` 对象并使其成为作用域的"当前"对象.

示例:

````csharp
public class MyAppService : ApplicationService
{
    private readonly ICurrentPrincipalAccessor _currentPrincipalAccessor;

    public MyAppService(ICurrentPrincipalAccessor currentPrincipalAccessor)
    {
        _currentPrincipalAccessor = currentPrincipalAccessor;
    }

    public void Foo()
    {
        var newPrincipal = new ClaimsPrincipal(
            new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(AbpClaimTypes.UserId, Guid.NewGuid().ToString()),
                    new Claim(AbpClaimTypes.UserName, "john"),
                    new Claim("MyCustomCliam", "42")
                }
            )
        );

        using (_currentPrincipalAccessor.Change(newPrincipal))
        {
            var userName = CurrentUser.UserName; //returns "john"
            //...
        }
    }
}
````

始终在 `using` 语句中使用 `Change` 方法,在 `using` 范围结束后它将恢复为原始值.

这可以是一种模拟用户登录的应用程序代码范围的方法,但是请尝试谨慎使用它.

## AbpClaimTypes

`AbpClaimTypes` 是一个静态类它定义了标准声明的名称被ABP框架使用.

* `UserName`, `UserId`, `Role` 和 `Email` 属性的默认值是通常[System.Security.Claims.ClaimTypes](https://docs.microsoft.com/en-us/dotnet/api/system.security.claims.claimtypes)类设置的, 但你可以改变它们.

* 其他属性,如 `EmailVerified`, `PhoneNumber`, `TenantId` ...是由ABP框架通过尽可能遵循标准名称来定义的.

建议使用这个类的属性来代替声明名称的魔术字符串.
