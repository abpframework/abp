## Creating a Settings Tab

There are several settings tabs from different modules. You can add custom settings tabs to your project in 3 steps.

1. Create a Component

```js
import { Select } from '@ngxs/store';
import { Component } from '@angular/core';

@Component({
  selector: 'app-your-custom-settings',
  template: `
    your-custom-settings works! mySetting: {%{{ mySetting$ | async }}%}
  `,
})
export class YourCustomSettingsComponent {
  @Select(ConfigState.getSetting('MyProjectName.MySetting1')) // Gets a setting. MyProjectName.MySetting1 is a setting key.
  mySetting$: Observable<string>; // The selected setting is set to the mySetting variable as Observable.
}
```

2. Add the `YourCustomSettingsComponent` to `declarations` and the `entryComponents` arrays in the `AppModule`.

3. Open the `app.component.ts` and add the below content to the `ngOnInit`

```js
import { addSettingTab } from '@abp/ng.theme.shared';
// ...

ngOnInit() {
  addSettingTab({
    component: YourCustomSettingsComponent,
    name: 'Type here the setting tab title (you can type a localization key, e.g: AbpAccount::Login',
    order: 4,
    requiredPolicy: 'type here a policy key'
  });
}
```

Open the `setting-management` page to see the changes:

![Custom Settings Tab](./images/custom-settings.png)
