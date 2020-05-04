# Custom Setting Page

There are several settings tabs from different modules. You can add custom settings page to your project in 3 steps.

1. Create a Component

```js
import { Select } from '@ngxs/store';
import { Component } from '@angular/core';

@Component({
  selector: 'app-your-custom-settings',
  template: `
    custom-settings works! 
  `,
})
export class YourCustomSettingsComponent {
  // Your component logic
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

Navigate to `/setting-management` route to see the changes:

![Custom Settings Tab](./images/custom-settings.png)

## What's Next?

- [Lazy Loading Scripts & Styles](./Lazy-Load-Service.md)
