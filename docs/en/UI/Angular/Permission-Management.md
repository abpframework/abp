# Permission Management

A permission is a simple policy that is granted or prohibited for a particular user, role or client. You can read more about [authorization in ABP](../../Authorization.md) document.

You can get permission of authenticated user using `getGrantedPolicy` or `getGrantedPolicy$` method of `PermissionService`.

> ConfigState's getGrantedPolicy selector and ConfigStateService's getGrantedPolicy method deprecated. Use permission service's `getGrantedPolicy$` or `getGrantedPolicy`methods instead 

You can get permission as boolean value:

```js
import { PermissionService } from '@abp/ng.core';

export class YourComponent {
  constructor(private permissionService: PermissionService) {}

  ngOnInit(): void {
    const canCreate = this.permissionService.getGrantedPolicy('AbpIdentity.Roles.Create');
  }
}
```

You may also **combine policy keys** to fine tune your selection:

```js
// this.permissionService is instance of PermissionService

const hasIdentityAndAccountPermission = this.permissionService.getGrantedPolicy(
  "Abp.Identity && Abp.Account"
);

const hasIdentityOrAccountPermission = this.permissionService.getGrantedPolicy(
  "Abp.Identity || Abp.Account"
);
```

Please consider the following **rules** when creating your permission selectors:

- Maximum 2 keys can be combined.
- `&&` operator looks for both keys.
- `||` operator looks for either key.
- Empty string `''` as key will return `true`
- Using an operator without a second key will return `false`

## Permission Directive

You can use the `PermissionDirective` to manage visibility of a DOM Element accordingly to user's permission.

```html
<div *abpPermission="'AbpIdentity.Roles'">
  This content is only visible if the user has 'AbpIdentity.Roles' permission.
</div>
```

As shown above you can remove elements from DOM with `abpPermission` structural directive.

## Permission Guard

You can use `PermissionGuard` if you want to control authenticated user's permission to access to the route during navigation.

* Import the PermissionGuard from @abp/ng.core.
* Add `canActivate: [PermissionGuard]` to your route object.
* Add `requiredPolicy` to the `data` property of your route in your routing module.

```js
import { PermissionGuard } from '@abp/ng.core';
// ...
const routes: Routes = [
  {
    path: 'path',
    component: YourComponent,
    canActivate: [PermissionGuard],
    data: {
        requiredPolicy: 'YourProjectName.YourComponent', // policy key for your component
    },
  },
];
```

Granted Policies are stored in the `auth` property of `ConfigState`.