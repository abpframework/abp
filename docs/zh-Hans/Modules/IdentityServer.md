# 身份服务器模块

身份服务器模块提供了一个 [IdentityServer](https://github.com/IdentityServer/IdentityServer4) (IDS) 的完全集成, 该框架提供高级身份验证功能, 如单点登录和API访问控制.此模块将客户端,资源以及其他 IDS 相关的对象保存到数据库中.

## 如何安装

当你使用 ABP 框架 [创建一个新的解决方案](https://abp.io/get-started) 时, 此模块将被预安装(作为 NuGet/NPM 包).你可以继续用其作为包并轻松地获取更新, 也可以将其源代码包含在解决方案中(请参阅 `get-source` [CLI](../CLI.md))以开发自定义模块.

### 源代码

可以 [在此处](https://github.com/abpframework/abp/tree/dev/modules/identityserver) 访问源代码.源代码使用 [MIT](https://choosealicense.com/licenses/mit/) 许可, 所以你可以免费使用和自定义它.

## 用户界面

此模块使用了领域逻辑和数据库集成, 但没有提供任何 UI.如果你需要动态添加客户端和资源, 管理 UI 是非常有用的.在这种情况下, 你可以自己构建管理 UI, 或者考虑购买为此模块提供了管理 UI 的 [ABP 商业版](https://commercial.abp.io/).

## 与其他模块的关系

此模块基于 [身份模块](Identity.md) 并且[账户模块](Account.md) 有一个 [集成包](https://www.nuget.org/packages/Volo.Abp.Account.Web.IdentityServer).

## 选项

### AbpIdentityServerBuilderOptions

`AbpIdentityServerBuilderOptions` 在你的身份服务器 [模块](https://docs.abp.io/zh-Hans/abp/latest/Module-Development-Basics) 中的 `PreConfigureServices` 方法中配置.例如:

````csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<AbpIdentityServerBuilderOptions>(builder =>
    {
        //Set options here...
    });
}
````

`AbpIdentityServerBuilderOptions` 属性:

* `UpdateJwtSecurityTokenHandlerDefaultInboundClaimTypeMap` (默认值:true):更新 `JwtSecurityTokenHandler.DefaultInboundClaimTypeMap` 使其与身份服务器声明兼容.
* `UpdateAbpClaimTypes` (默认值:true):更新 `AbpClaimTypes` 与身份服务器声明兼容.
* `IntegrateToAspNetIdentity` (默认值:true):集成到 ASP.NET Identity.
* `AddDeveloperSigningCredential` (默认值:true):设置为 false 禁止调用 IIdentityServerBuilder 中的 `AddDeveloperSigningCredential()`.

`IIdentityServerBuilder` 可以在你的身份服务器 [模块](https://docs.abp.io/zh-Hans/abp/latest/Module-Development-Basics) 中的 `PreConfigureServices` 方法中配置.例如:

````csharp
public override void PreConfigureServices(ServiceConfigurationContext context)
{
    PreConfigure<IIdentityServerBuilder>(builder =>
    {
        builder.AddSigningCredential(...);
    });
}
````

## 内部结构

### 领域层

#### 聚合

##### API 资源

需要 API 资源来允许客户端请求访问令牌.

* `ApiResource` (聚合根):表示系统中的 API 资源.
  * `ApiSecret` (集合):API 资源的密钥.
  * `ApiScope` (集合):API 资源的作用域.
  * `ApiResourceClaim` (集合):API 资源的声明.

##### 客户端

客户端表示可以从你的身份服务器请求令牌的应用程序.

* `Client` (聚合根):表示一个身份服务器的客户端应用程序.
  * `ClientScope` (集合):客户端的作用域.
  * `ClientSecret` (集合):客户端的密钥.
  * `ClientGrantType` (集合):客户端的授权类型.
  * `ClientCorsOrigin` (集合):客户端的 CORS 源.
  * `ClientRedirectUri` (集合):客户端的重定向 URIs.
  * `ClientPostLogoutRedirectUri` (集合):客户端的登出重定向 URIs.
  * `ClientIdPRestriction` (集合):客户端的提供程序约束.
  * `ClientClaim` (集合):客户端的声明.
  * `ClientProperty` (集合):客户端的自定义属性.

##### 持续化授权

持续化授权存储了授权码,刷新令牌和用户准许.

* `PersistedGrant` (聚合根):表示为身份服务器持续化授权.

##### 身份资源

身份资源是用户的用户 ID ,名称或邮件地址等数据.

* `IdentityResource` (聚合根):表示与身份服务器的身份资源.
    * `IdentityClaim` (集合):身份资源的声明.

#### 仓储

为此模块定义了以下自定义仓储:

* `IApiResourceRepository`
* `IClientRepository`
* `IPersistentGrantRepository`
* `IIdentityResourceRepository`

#### 领域服务

此模块不包含任何领域服务, 但重写了下面的服务;

* `AbpProfileService` (当 `AbpIdentityServerBuilderOptions.IntegrateToAspNetIdentity` 为 true 时使用)
* `AbpClaimsService`
* `AbpCorsPolicyService`

### 设置

此模块未定义任何设置.

### 应用层

#### 应用服务

* `ApiResourceAppService` (实现 `IApiResourceAppService`):实现了 API 资源管理 UI 的用例.
* `IdentityServerClaimTypeAppService` (实现 `IIdentityServerClaimTypeAppService`):用于获取声明列表.
* `ApiResourceAppService` (实现 `IApiResourceAppService`):实现了 API 管理资源 UI 的用例.
* `IdentityResourceAppService` (实现 `IIdentityResourceAppService`):实现了身份资源管理 UI 的用例.

### 数据库提供程序

#### 公共

##### 表/集合 前缀 & 架构

所有表/集合都使用 `IdentityServer` 作为默认前缀.如果你需要改变表的前缀或设置一个架构名称(如果你的数据库提供程序支持), 请设置 `AbpIdentityServerDbProperties` 类的静态属性.

##### 连接字符串

此模块使用 `AbpIdentityServer` 作为连接字符串的名称.如果你没有用这个名称定义连接字符串, 它将回退到 `Default` 连接字符串.

有关详细信息, 请参阅 [连接字符串](https://docs.abp.io/zh-Hans/abp/latest/Connection-Strings) 文档.

#### EF Core

##### 表

* **IdentityServerApiResources**
  * IdentityServerApiSecrets
  * IdentityServerApiScopes
    * IdentityServerApiScopeClaims
  * IdentityServerApiClaims
* **IdentityServerClients**
  * IdentityServerClientScopes
  * IdentityServerClientSecrets
  * IdentityServerClientGrantTypes
  * IdentityServerClientCorsOrigins
  * IdentityServerClientRedirectUris
  * IdentityServerClientPostLogoutRedirectUris
  * IdentityServerClientIdPRestrictions
  * IdentityServerClientClaims
  * IdentityServerClientProperties
* **IdentityServerPersistedGrants**
* **IdentityServerIdentityResources**
  * IdentityServerIdentityClaims

#### MongoDB

##### 集合

* **IdentityServerApiResources**
* **IdentityServerClients**
* **IdentityServerPersistedGrants**
* **IdentityServerIdentityResources**
