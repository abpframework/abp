# Blazor UI: Settings

Blazor applications can reuse the same `ISettingProvider` service that is explained in the [settings document](../../Settings.md).

## ISettingProvider

`ISettingProvider` is used to get the value of a setting or get the values of all the settings.

**Example usages in a simple service**

````csharp
public class MyService : ITransientDependency
{
    private readonly ISettingProvider _settingProvider;

    //Inject ISettingProvider in the constructor
    public MyService(ISettingProvider settingProvider)
    {
        _settingProvider = settingProvider;
    }

    public async Task FooAsync()
    {
        //Get a value as string.
        string setting1 = await _settingProvider.GetOrNullAsync("MySettingName");

        //Get a bool value and fallback to the default value (false) if not set.
        bool setting2 = await _settingProvider.GetAsync<bool>("MyBoolSettingName");

        //Get a bool value and fallback to the provided default value (true) if not set.
        bool setting3 = await _settingProvider.GetAsync<bool>(
            "MyBoolSettingName", defaultValue: true);
        
        //Get a bool value with the IsTrueAsync shortcut extension method
        bool setting4 = await _settingProvider.IsTrueAsync("MyBoolSettingName");
        
        //Get an int value or the default value (0) if not set
        int setting5 = (await _settingProvider.GetAsync<int>("MyIntegerSettingName"));

        //Get an int value or null if not provided
        int? setting6 = (await _settingProvider
                         .GetOrNullAsync("MyIntegerSettingName"))?.To<int>();
    }
}
````

**Example usage in a Razor Component**

````csharp
@page "/"
@using Volo.Abp.Settings
@inject ISettingProvider SettingProvider
@code {
    protected override async Task OnInitializedAsync()
    {
        bool settingValue = await SettingProvider.GetAsync<bool>("MyBoolSettingName");
    }
}
````

## See Also

* [Settings](../../Settings.md)