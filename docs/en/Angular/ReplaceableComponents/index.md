# Component Replacement in Angular

You can replace some ABP components with your custom components.

The reason that you **can replace** but **cannot customize** default ABP components is disabling or changing a part of that component can cause problems. So we named those components as _Replaceable Components_.

## How to Use Replaceable Components

Create a new component that you want to use instead of an ABP component. Add that component to `declarations` and `entryComponents` in the `AppModule`.

Then, open the `app.component.ts` and dispatch the `AddReplaceableComponent` action to replace your component with an ABP component as shown below:

```ts
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

![Example Usage](./images/inaction.gif)

## Available Replaceable Components

| Component key                                      | Description                                                                                                     |
| -------------------------------------------------- | --------------------------------------------------------------------------------------------------------------- |
| Account.LoginComponent                             | Login page                                                                                                      |
| Account.RegisterComponent                          | Register page                                                                                                   |
| Account.ManageProfileComponent                     | Manage Profile page                                                                                             |
| Account.ForgotPasswordComponent                    | Forgot password page                                                                                            |
| Account.ResetPasswordComponent                     | Reset password page                                                                                             |
| Account.AccountComponent                           | The component that wraps register, login, forgot password, and reset password pages. Contains `<router-outlet>` |
| Account.ChangePasswordComponent                    | Change password form in manage profile page                                                                     |
| Account.PersonalSettingsComponent                  | Personal settings form in manage profile page                                                                   |
| Account.TenantBoxComponent                         | Tenant changing box in account component                                                                        |
| AuditLogging.AuditLogsComponent                    | Audit logs page                                                                                                 |
| Identity.UsersComponent                            | Users page                                                                                                      |
| Identity.RolesComponent                            | Roles page                                                                                                      |
| Identity.ClaimsComponent                           | Claim types page                                                                                                |
| IdentityServer.IdentityResource                    | Identity resources page                                                                                         |
| IdentityServer.Client                              | Clients page                                                                                                    |
| IdentityServer.ApiResource                         | Api resources page                                                                                              |
| LanguageManagement.Languages                       | Languages page                                                                                                  |
| LanguageManagement.LanguageTexts                   | Language texts page                                                                                             |
| Saas.TenantsComponent                              | Tenants page                                                                                                    |
| Saas.EditionsComponent                             | Editions page                                                                                                   |
| FeatureManagement.FeatureManagementComponent       | Features modal                                                                                                  |
| PermissionManagement.PermissionManagementComponent | Permissions modal                                                                                               |
| SettingManagement.SettingManagementComponent       | Setting Management page                                                                                         |
