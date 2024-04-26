# Blazor UI: Authentication

````json
//[doc-params]
{
    "UI": ["Blazor", "BlazorServer"]
}
````

The [application startup template](../../../solution-templates/layered-web-application) is properly configured to use OpenId Connect to authenticate the user;

{{if UI == "BlazorServer"}}
The Blazor Server application UI is actually a hybrid application that is combined with the MVC UI, and uses the login page provided by the MVC UI. When users enter a page that requires login, they are redirected to the `/Account/Login` page. Once they complete the login process, they are returned back to the application's UI. The login page also contains features like registration, password recovery, etc.

{{end}}

{{if UI == "Blazor"}}
* When the Blazor application needs to authenticate, it is redirected to the server side.
* Users can enter username & password to login if they already have an account. If not, they can use the register form to create a new user. They can also use forgot password and other features. The server side uses OpenIddict to handle the authentication.
* Finally, they are redirected back to the Blazor application to complete the login process.

This is a typical and recommended approach to implement authentication in Single-Page Applications. The client side configuration is done in the startup template, so you can change it.


See the [Blazor Security document](https://docs.microsoft.com/en-us/aspnet/core/blazor/security) to understand and customize the authentication process.

{{end}}