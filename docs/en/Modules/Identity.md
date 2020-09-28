# Identity Management Module

Identity module is used to manage [organization units](Organization-Units.md), roles, users and their permissions, based on the Microsoft Identity library.

**See [the source code](https://github.com/abpframework/abp/tree/dev/modules/identity). Documentation will come soon...**


## Identity Security Log

The security log can record some important operations or changes about your account. You can save the security log if needed.

You can inject and use `IdentitySecurityLogManager` or `ISecurityLogManager` to write security logs. It will create a log object by default and fill in some common values, such as `CreationTime`, `ClientIpAddress`, `BrowserInfo`, `current user/tenant`, etc. Of course, you can override them.

```cs
await IdentitySecurityLogManager.SaveAsync(new IdentitySecurityLogContext()
{
	Identity = "IdentityServer";
	Action = "ChangePassword";
});
```

Configure `AbpSecurityLogOptions` to provide the application name for the log or disable this feature. **Enabled** by default.

```cs
Configure<AbpSecurityLogOptions>(options =>
{
	options.ApplicationName = "AbpSecurityTest";
});
```
