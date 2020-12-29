# Blazor UI: Authorization

Blazor applications can use the same authorization system and permissions defined in the server side.

> This document is only for authorizing on the Blazor UI. See the [Server Side Authorization](../../Authorization.md) to learn how to define permissions and control the authorization system.

## Basic Usage

> ABP Framework is **100% compatible** with the Authorization infrastructure provided by the Blazor. See the [Blazor Security Document](https://docs.microsoft.com/en-us/aspnet/core/blazor/security/) to learn all authorization options. This section **only shows some common scenarios**.

### Authorize Attribute

`[Authorize]` attribute can be used to show a page only to the authenticated users.

````csharp
@page "/"
@attribute [Authorize]

You can only see this if you're signed in.
````

The `[Authorize]` attribute also supports role-based or policy-based authorization. For example, you can check permissions defined in the server side:

````csharp
@page "/"
@attribute [Authorize("MyPermission")]

You can only see this if you have the necessary permission.
````

### AuthorizeView

`AuthorizeView` component can be used in a page/component to conditionally render a part of the content:

````html
<AuthorizeView Policy="MyPermission">
    <p>You can only see this if you satisfy the "MyPermission" policy.</p>
</AuthorizeView>
````

### IAuthorizationService

`IAuthorizationService` can be injected and used to programmatically check permissions:

````csharp
public partial class Index
{
    protected override async Task OnInitializedAsync()
    {
        if (await AuthorizationService.IsGrantedAsync("MyPermission"))
        {
            //...
        }
    }
}
````

If your component directly or indirectly inherits from the `AbpComponentBase`, `AuthorizationService` becomes pre-injected and ready to use. If not, you can always [inject](../../Dependency-Injection.md) the `IAuthorizationService` yourself.

`IAuthorizationService` can also be used in the view side where `AuthorizeView` component is not enough.

There are some useful extension methods for the `IAuthorizationService`:

* `IsGrantedAsync` simply returns `true` or `false` for the given policy/permission.
* `CheckAsync` checks and throws `AbpAuthorizationException` if given policy/permission hasn't granted. You don't have to handle these kind of exceptions since ABP Framework automatically [handles errors](Error-Handling.md).
* `AuthorizeAsync` returns `AuthorizationResult` as the standard way provided by the ASP.NET Core authorization system.

> See the [Blazor Security Document](https://docs.microsoft.com/en-us/aspnet/core/blazor/security/) to learn all authorization options

## See Also

* [Authorization](../../Authorization.md) (server side)
* [Blazor Security](https://docs.microsoft.com/en-us/aspnet/core/blazor/security/) (Microsoft documentation)
* [ICurrentUser Service](CurrentUser.md)

