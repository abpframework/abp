# Blazor UI: Current Tenant

`ICurrentTenant` service can be used to get information about the current tenant in a [multi-tenant](../../Multi-Tenancy.md) application. `ICurrentTenant` defines the following properties;

* `Id` (`Guid`): Id of the current tenant. Can be `null` if the current user is a host user or the tenant could not be determined.
* `Name` (`string`): Name of the current tenant. Can be `null` if the current user is a host user or the tenant could not be determined.
* `IsAvailable` (`bool`): Returns `true` if the `Id` is not `null`.

**Example: Show the current tenant name on a page**

````csharp
@page "/"
@using Volo.Abp.MultiTenancy
@inject ICurrentTenant CurrentTenant
@if (CurrentTenant.IsAvailable)
{
    <p>Current tenant name: @CurrentTenant.Name</p>
}
````

## See Also

* [Multi-Tenancy](../../Multi-Tenancy.md)