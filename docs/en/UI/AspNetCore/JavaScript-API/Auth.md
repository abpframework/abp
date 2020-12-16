# ASP.NET Core MVC / Razor Pages UI: JavaScript Auth API

Auth API allows you to check permissions (policies) for the current user in the client side. In this way, you can conditionally show/hide UI parts or perform your client side logic based on the current permissions.

> This document only explains the JavaScript API. See the [authorization document](../../../Authorization.md) to understand the ABP authorization & permission system.

## Basic Usage

`abp.auth.isGranted(...)` function is used to check if a permission/policy has granted or not:

````js
if (abp.auth.isGranted('DeleteUsers')) {
  //TODO: Delete the user
} else {
  alert("You don't have permission to delete a user!");
}
````

## Other Fields & Functions

* ` abp.auth.isAnyGranted(...)`: Gets one or more permission/policy names and returns `true` if at least one of them has granted.
* `abp.auth.areAllGranted(...)`: Gets one or more permission/policy names and returns `true` if all of them of them have granted.
* `abp.auth.policies`: This is an object where its keys are the permission/policy names. You can find all permission/policy names here.
* `abp.auth.grantedPolicies`: This is an object where its keys are the permission/policy names. You can find the granted permission/policy names here.