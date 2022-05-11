# Blazor UI: Authentication

````json
//[doc-params]
{
    "UI": ["Blazor", "BlazorServer"]
}
````

The [application startup template](../../Startup-Templates/Application.md) is properly configured to use OpenId Connect to authenticate the user;

{{if UI == "BlazorServer"}}
* When the Blazor application needs to authenticate, it is redirected to the login page built with Razor Pages UI in the same application.
* Users can enter username & password to login if they already have an account. If not, they can use the register form to create a new user. They can also use forgot password and other features.
* Finally, they are redirected back to the Blazor application to complete the login process.

{{end}}

{{if UI == "Blazor"}}
* When the Blazor application needs to authenticate, it is redirected to the server side.
* Users can enter username & password to login if they already have an account. If not, they can use the register form to create a new user. They can also use forgot password and other features.,

* Finally, they are redirected back to the Blazor application to complete the login process.

This is a typical and recommended approach to implement authentication in Single-Page Applications. The client side configuration is done in the startup template, so you can change it.
{{end}}

See the [Blazor Security document](https://docs.microsoft.com/en-us/aspnet/core/blazor/security) to understand and customize the authentication process.