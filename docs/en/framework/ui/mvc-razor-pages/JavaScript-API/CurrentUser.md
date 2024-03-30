# ASP.NET Core MVC / Razor Pages UI: JavaScript CurrentUser API

`abp.currentUser` is an object that contains information about the current user of the application.

> This document only explains the JavaScript API. See the [CurrentUser document](../../../CurrentUser.md) to get information about the current user in the server side.

## Authenticated User

If the user was authenticated, this object will be something like below:

````js
{
  isAuthenticated: true,
  id: "34f1f4a7-13cc-4b91-84d1-b91c87afa95f",
  tenantId: null,
  userName: "john",
  name: "John",
  surName: "Nash",
  email: "john.nash@abp.io",
  emailVerified: true,
  phoneNumber: null,
  phoneNumberVerified: false,
  roles: ["moderator","supporter"]
}
````

So, `abp.currentUser.userName` returns `john` in this case.

## Anonymous User

If the user was not authenticated, this object will be something like below:

````js
{
  isAuthenticated: false,
  id: null,
  tenantId: null,
  userName: null,
  name: null,
  surName: null,
  email: null,
  emailVerified: false,
  phoneNumber: null,
  phoneNumberVerified: false,
  roles: []
}
````

You can check `abp.currentUser.isAuthenticated` to understand if the use was authenticated or not.