# Blazor UI: Routing

Blazor has its own [routing system](https://docs.microsoft.com/en-us/aspnet/core/blazor/fundamentals/routing) and you can use it in your applications. ABP doesn't add any new feature to it, except one small improvement for the [modular development](../../Module-Development-Basics.md).

## AbpRouterOptions

Blazor `Router` component requires to define `AdditionalAssemblies` when you have components in assemblies/projects other than the main application's entrance assembly. So, if you want to create razor class libraries as ABP modules, you typically want to add the module's assembly to the `AdditionalAssemblies`. In this case, you need to add your module's assembly to the `AbpRouterOptions`.

**Example**

````csharp
Configure<AbpRouterOptions>(options =>
{
    options.AdditionalAssemblies.Add(typeof(MyBlazorModule).Assembly);
});
````

Write this code in the `ConfigureServices` method of your [module](../../Module-Development-Basics.md).

`AbpRouterOptions` has another property, `AppAssembly`, which should be the entrance assembly of the application and typically set in the final application's module. If you've created your solution with the [application startup template](../../Startup-Templates/Application.md), it is already configured for you.

## See Also

* [Blazor Routing](https://docs.microsoft.com/en-us/aspnet/core/blazor/fundamentals/routing) (Microsoft Documentation)