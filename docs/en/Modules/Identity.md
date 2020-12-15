# Identity Management Module

Identity module is used to manage organization units, roles, users and their permissions, based on the Microsoft Identity library.

> **See [the source code](https://github.com/abpframework/abp/tree/dev/modules/identity). Documentation will come soon...**


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

## Organization Unit Management

Organization units (OU) is a part of **Identity Module** and can be used to **hierarchically group users and entities**. 

### OrganizationUnit Entity

An OU is represented by the **OrganizationUnit** entity. The fundamental properties of this entity are:

- **TenantId**: Tenant's Id of this OU. Can be null for host OUs.
- **ParentId**: Parent OU's Id. Can be null if this is a root OU.
- **Code**: A hierarchical string code that is unique for a tenant.
- **DisplayName**: Shown name of the OU.

The OrganizationUnit entity's primary key (Id) is a **Guid** type and it derives from the [**FullAuditedAggregateRoot**](../Entities.md) class.

#### Organization Tree

Since an OU can have a parent, all OUs of a tenant are in a **tree** structure. There are some rules for this tree;

- There can be more than one root (where the `ParentId` is `null`).
- There is a limit for the first-level children count of an OU (because of the fixed OU Code unit length explained below).

#### OU Code

OU code is automatically generated and maintained by the OrganizationUnit Manager. It's a string that looks something like this:

"**00001.00042.00005**"

This code can be used to easily query the database for all the children of an OU (recursively). There are some rules for this code:

- It must be **unique** for a [tenant](../Multi-Tenancy.md).
- All the children of the same OU have codes that **start with the parent OU's code**.
- It's **fixed length** and based on the level of the OU in the tree, as shown in the sample.
- While the OU code is unique, it can be **changeable** if you move an OU.
- You must reference an OU by Id, not Code.

### OrganizationUnit Manager

The **OrganizationUnitManager** class can be [injected](../Dependency-Injection.md) and used to manage OUs. Common use cases are:

- Create, Update or Delete an OU
- Move an OU in the OU tree.
- Getting information about the OU tree and its items.

#### Multi-Tenancy

The `OrganizationUnitManager` is designed to work for a **single tenant** at a time. It works for the **current tenant** by default.