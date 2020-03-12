# Component Replacement

You can replace some ABP components with your custom components.

The reason that you **can replace** but **cannot customize** default ABP components is disabling or changing a part of that component can cause problems. So we named those components as _Replaceable Components_.

## How to Replace a Component

Create a new component that you want to use instead of an ABP component. Add that component to `declarations` and `entryComponents` in the `AppModule`.

Then, open the `app.component.ts` and dispatch the `AddReplaceableComponent` action to replace your component with an ABP component as shown below:

```js
import { ..., AddReplaceableComponent } from '@abp/ng.core';
export class AppComponent {
  constructor(..., private store: Store) {}

  ngOnInit() {
    this.store.dispatch(
      new AddReplaceableComponent({
        component: YourNewRoleComponent,
        key: 'Identity.RolesComponent',
      }),
    );
    //...
  }
}
```

![Example Usage](./images/component-replacement.gif)

## Available Replaceable Components

| Component key                                      | Description                                   |
| -------------------------------------------------- | --------------------------------------------- |
| Account.LoginComponent                             | Login page                                    |
| Account.RegisterComponent                          | Register page                                 |
| Account.ManageProfileComponent                     | Manage Profile page                           |
| Account.AuthWrapperComponent                       | This component wraps register and login pages |
| Account.ChangePasswordComponent                    | Change password form                          |
| Account.PersonalSettingsComponent                  | Personal settings form                        |
| Account.TenantBoxComponentInputs                   | Tenant changing box                           |
| FeatureManagement.FeatureManagementComponent       | Features modal                                |
| Identity.UsersComponent                            | Users page                                    |
| Identity.RolesComponent                            | Roles page                                    |
| PermissionManagement.PermissionManagementComponent | Permissions modal                             |
| SettingManagement.SettingManagementComponent       | Setting Management page                       |
| TenantManagement.TenantsComponent                  | Tenants page                                  |

## What's Next?

- [Custom Setting Page](./Custom-Setting-Page.md)
