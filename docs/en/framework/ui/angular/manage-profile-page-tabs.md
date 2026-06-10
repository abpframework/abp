```json
//[doc-seo]
{
    "Description": "Learn to manage profile page tabs in ABP Framework using `ManageProfileTabsService` for adding, removing, or editing tabs effortlessly."
}
```

# Manage Profile Page Tabs

![manage profile page](./images/manage-profile-page.png)

The tabs in the manage profile page can be managed via `ManageProfileTabsService` which is exposed by the `@volo/abp.ng.account/public/config` package. You can add, remove, or edit a tab with using this service.

See the example below, covers all features:

```ts
// manage-profile-tabs.provider.ts

import { Component, inject, provideAppInitializer } from "@angular/core";
import { TwoFactorTabComponent } from "@volo/abp.ng.account/public";
import {
  eAccountManageProfileTabNames,
  ManageProfileTabsService,
} from "@volo/abp.ng.account/public/config";

@Component({
  selector: "abp-my-awesome-tab",
  template: `My Awesome Tab`,
})
class MyAwesomeTabComponent {}

export const MANAGE_PROFILE_TAB_PROVIDER = [
  provideAppInitializer(() => {
    configureManageProfileTabs();
  }),
];

export function configureManageProfileTabs() {
  const tabs = inject(ManageProfileTabsService);
  tabs.add([
    {
      name: "::MyAwesomeTab", // supports localization keys
      order: 5,
      component: MyAwesomeTabComponent,
    },
  ]);

  tabs.patch(eAccountManageProfileTabNames.TwoFactor, {
    name: "Two factor authentication",
    component: TwoFactorTabComponent,
  });

  tabs.remove([eAccountManageProfileTabNames.ProfilePicture]);
}
```

```ts
//app.config.ts

import { MANAGE_PROFILE_TAB_PROVIDER } from "./manage-profile-tabs.provider";

export const appConfig: ApplicationConfig = {
  providers: [
    // ...
    MANAGE_PROFILE_TAB_PROVIDER,
  ],
};
```

What we have done above;

- Created the `manage-profile-page-tabs.provider.ts`.
- Determined the `configureManageProfileTabs` function to perform manage profile tabs actions.
  - Added a new tab labeled "My awesome tab".
  - Renamed the "Two factor" tab label.
  - Removed the "Profile picture" tab.
- Determined the `MANAGE_PROFILE_TAB_PROVIDER` to be able to run the `configureManageProfileTabs` function on initialization.
- Registered the `MANAGE_PROFILE_TAB_PROVIDER` to the `appConfig` providers.

See the result:

![my awesome tab](./images/manage-profile-page-new-tab.png)
