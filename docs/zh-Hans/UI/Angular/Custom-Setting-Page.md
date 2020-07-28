# 自定义设置页面

不同的模块提供它们的设置选项卡. 你可以通过3个步骤在项目中自定义设置页面.

1. 使用以下命令创建一个组件

```bash
yarn ng generate component my-settings
```

2. 打开 `app.component.ts` 做以下修改:

```js
import { Component } from '@angular/core';
import { SettingTabsService } from '@abp/ng.core'; // imported SettingTabsService
import { MySettingsComponent } from './my-settings/my-settings.component'; // imported MySettingsComponent

@Component(/* component metadata */)
export class AppComponent {
  constructor(private settingTabs: SettingTabsService) // injected MySettingsComponent
  {
    // added below
    settingTabs.add([
      {
        name: 'MySettings',
        order: 1,
        requiredPolicy: 'policy key here',
        component: MySettingsComponent,
      },
    ]);
  }
}
```

导航到 `/setting-management` 路由你会看到以下变化:

![Custom Settings Tab](./images/custom-settings.png)

## 下一步是什么?

- [懒加载 Scripts 与 Styles](./Lazy-Load-Service.md)
