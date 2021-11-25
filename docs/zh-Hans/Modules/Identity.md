# 身份管理模块

身份模块基于Microsoft Identity库用于管理[组织单元](Organization-Units.md), 角色, 用户和他们的权限.

> 参阅 [源码](https://github.com/abpframework/abp/tree/dev/modules/identity). 文档很快会被完善.

## Identity安全日志

安全日志可以记录账户的一些重要的操作或者改动， 你可以在在一些功能中保存安全日志.

你可以注入和使用 `IdentitySecurityLogManager` 或 `ISecurityLogManager` 来保存安全日志. 默认它会创建一个安全日志对象并填充常用的值. 如 `CreationTime`, `ClientIpAddress`, `BrowserInfo`, `current user/tenant`等等. 当然你可以自定义这些值.

```cs
await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
{
	Identity = "IdentityServer";
	Action = "ChangePassword";
});
```

通过配置 `AbpSecurityLogOptions` 来提供应用程序的名称或者禁用安全日志功能. 默认是**启用**状态.

```cs
Configure<AbpSecurityLogOptions>(options =>
{
	options.ApplicationName = "AbpSecurityTest";
});
```
