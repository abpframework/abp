# 设置管理模块

设置管理模块实现了 `ISettingStore` (参阅 [设置系统](../Settings.md)) 将设置值存储在数据库中, 并提供 `ISettingManager` 管理 (更改) 数据库中设置值的功能.

> [启动模板](../Startup-Templates/Index.md)默认安装并配置了设置管理模块. 大部分情况下你不需要手动的添加该到模块到应用程序中.

## ISettingManager

`ISettingManager` 用于获取和设定设置值. 示例:

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

你可以从不同的设置值提供程序中(默认,全局,用户,租户...等)中获取或设定设置值.

> 如果只需要读取设置值,建议使用 `ISettingProvider` 而不是`ISettingManager`,因为它实现了缓存并支持所有部署场景. 如果要创建设置管理UI,可以使用ISettingManager.

### Setting Cache

设置值缓存在 [分布式缓存](../Caching.md) 系统中. 建议始终使用 `ISettingManager` 更改设置值.

## Setting Management Providers

设置管理模块是可扩展的,像[设置系统](../Settings.md)一样.  你可以通过自定义设置管理提供程序进行扩展. 有5个预构建的设置管理程序程序按以下顺序注册:

* `DefaultValueSettingManagementProvider`: 从设置定义的默认值中获取值,由于默认值是硬编码在设置定义上的,所以无法更改默认值.
* `ConfigurationSettingManagementProvider`:从 [IConfiguration 服务](../Configuration.md)中获取值. 由于无法在运行时更改配置值,所以无法更改配置值.
* `GlobalSettingManagementProvider`: 获取或设定设置的全局 (系统范围)值.
* `TenantSettingManagementProvider`: 获取或设定租户的设置值.
* `UserSettingManagementProvider`: 获取或设定用户的设置值.

`ISettingManager` 在 `get/set` 方法中使用设置管理提供程序. 通常每个设置程序提供程序都在 `ISettingManagement` 服务上定义了模块方法 (比如用户设置管理程序提供定义了 `SetForUserAsync` 方法).