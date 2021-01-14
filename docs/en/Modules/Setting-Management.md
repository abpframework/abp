# Setting Management Module

Setting Management Module implements the `ISettingStore` (see [the setting system](../Settings.md)) to store the setting values in a database and provides the `ISettingManager` to manage (change) the setting values in the database.

> Setting Management module is already installed and configured for [the startup templates](../Startup-Templates/Index.md). So, most of the times you don't need to manually add this module to your application.

## ISettingManager

`ISettingManager` is used to get and set the values for the settings. Examples:

````csharp
using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SettingManagement;

namespace Demo
{
    public class MyService : ITransientDependency
    {
        private readonly ISettingManager _settingManager;

        //Inject ISettingManager service
        public MyService(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        public async Task FooAsync()
        {
            Guid user1Id = ...;
            Guid tenant1Id = ...;

            //Get/set a setting value for the current user or the specified user
            
            string layoutType1 =
                await _settingManager.GetOrNullForCurrentUserAsync("App.UI.LayoutType");
            string layoutType2 =
                await _settingManager.GetOrNullForUserAsync("App.UI.LayoutType", user1Id);

            await _settingManager.SetForCurrentUserAsync("App.UI.LayoutType", "LeftMenu");
            await _settingManager.SetForUserAsync(user1Id, "App.UI.LayoutType", "LeftMenu");

            //Get/set a setting value for the current tenant or the specified tenant
            
            string layoutType3 =
                await _settingManager.GetOrNullForCurrentTenantAsync("App.UI.LayoutType");
            string layoutType4 =
                await _settingManager.GetOrNullForTenantAsync("App.UI.LayoutType", tenant1Id);
            
            await _settingManager.SetForCurrentTenantAsync("App.UI.LayoutType", "LeftMenu");
            await _settingManager.SetForTenantAsync(tenant1Id, "App.UI.LayoutType", "LeftMenu");

            //Get/set a global and default setting value
            
            string layoutType5 =
                await _settingManager.GetOrNullGlobalAsync("App.UI.LayoutType");
            string layoutType6 =
                await _settingManager.GetOrNullDefaultAsync("App.UI.LayoutType");

            await _settingManager.SetGlobalAsync("App.UI.LayoutType", "TopMenu");
        }
    }
}

````

So, you can get or set a setting value for different setting value providers (Default, Global, User, Tenant... etc).

> Use the `ISettingProvider` instead of the `ISettingManager` if you only need to read the setting values, because it implements caching and supports all deployment scenarios. You can use the `ISettingManager` if you are creating a setting management UI.

### Setting Cache

Setting values are cached using the [distributed cache](../Caching.md) system. Always use the `ISettingManager` to change the setting values which manages the cache for you.

## Setting Management Providers

Setting Management module is extensible, just like the [setting system](../Settings.md).  You can extend it by defining setting management providers. There are 5 pre-built setting management providers registered it the following order:

* `DefaultValueSettingManagementProvider`: Gets the value from the default value of the setting definition. It can not set the default value since default values are hard-coded on the setting definition.
* `ConfigurationSettingManagementProvider`: Gets the value from the [IConfiguration service](../Configuration.md). It can not set the configuration value because it is not possible to change the configuration values on runtime.
* `GlobalSettingManagementProvider`: Gets or sets the global (system-wide) value for a setting.
* `TenantSettingManagementProvider`: Gets or sets the setting value for a tenant.
* `UserSettingManagementProvider`: Gets the setting value for a user.

`ISettingManager` uses the setting management providers on get/set methods. Typically, every setting management provider defines extension methods on the `ISettingManagement` service (like `SetForUserAsync` defined by the user setting management provider).