# Identity Management Module

Identity module is used to manage roles, users and their permissions, based on the [Microsoft Identity library](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity).

## How to Install

This module comes as pre-installed (as NuGet/NPM packages) when you [create a new solution](https://abp.io/get-started) with the ABP Framework. You can continue to use it as package and get updates easily, or you can include its source code into your solution (see `get-source` [CLI](../CLI.md) command) to develop your custom module.

### The Source Code

The source code of this module can be accessed [here](https://github.com/abpframework/abp/tree/dev/modules/identity). The source code is licensed with [MIT](https://choosealicense.com/licenses/mit/), so you can freely use and customize it.

## Menu Items

This module adds an *Identity management* menu item under the *Administration* menu:

![identity-module-menu](../images/identity-module-menu.png)

The menu items and the related pages are authorized. That means the current user must have the related permissions to make them visible. The `admin` role (and the users with this role - like the `admin` user) already has these permissions. If you want to enable permissions for other roles/users, open the *Permissions* dialog on the *Roles* or *Users* page and check the permissions as shown below:

![identity-module-permissions](../images/identity-module-permissions.png)

See the [Authorization document](../Authorization.md) to understand the permission system.

## Pages

This section introduces the main pages provided by this module.

### Users

This page is used to see the list of users. You can create/edit and delete users, assign users to roles.

![identity-module-users](../images/identity-module-users.png)

A user can have zero or more roles. Users inherit permissions from their roles. In addition, you can assign permissions directly to the users (by clicking the *Actions* button, then selecting the *Permissions*).

### Roles

Roles are used to group permissions assign them to users.

![identity-module-roles](../images/identity-module-roles.png)

Beside the role name, there are two properties of a role:

* `Default`: If a role is marked as "default", then that role is assigned to new users by default when they register to the application themselves (using the [Account Module](Account.md)).
* `Public`: A public role of a user can be seen by other users in the application. This feature has no usage in the Identity module, but provided as a feature that you may want to use in your own application.

## Other Features

This section covers some other features provided by this module which don't have the UI pages.

### Organization Units

Organization Units (OU) can be used to **hierarchically group users and entities**. 

#### OrganizationUnit Entity

An OU is represented by the **OrganizationUnit** entity. The fundamental properties of this entity are:

- **TenantId**: Tenant's Id of this OU. Can be null for host OUs.
- **ParentId**: Parent OU's Id. Can be null if this is a root OU.
- **Code**: A hierarchical string code that is unique for a tenant.
- **DisplayName**: Shown name of the OU.

#### Organization Tree

Since an OU can have a parent, all OUs of a tenant are in a **tree** structure. There are some rules for this tree;

- There can be more than one root (where the `ParentId` is `null`).
- There is a limit for the first-level children count of an OU (because of the fixed OU Code unit length explained below).

#### OU Code

OU code is automatically generated and maintained by the `OrganizationUnitManager` service. It's a string that looks something like this:

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

### Identity Security Log

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

### Options

TODO