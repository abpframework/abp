# Permission Management

A permission is a simple policy that is granted or prohibited for a particular user, role or client. You can read more about [authorization in ABP](../../Authorization.md) document.

You can get permission of authenticated user using `getGrantedPolicy` selector of `ConfigState`.

You can get permission as boolean value:

```js
import { ConfigStateService } from '@abp/ng.core';

export class YourComponent {
  constructor(private config: ConfigStateService) {}

  ngOnInit(): void {
    const canCreate = this.config.getGrantedPolicy('AbpIdentity.Roles.Create');
  }
}
```

## Permission Directive

You can use the `PermissionDirective` to manage visibility of a DOM Element accordingly to user's permission.

```html
<div *abpPermission="'AbpIdentity.Roles'">
  This content is only visible if the user has 'AbpIdentity.Roles' permission.
</div>
```

As shown above you can remove elements from DOM with `abpPermission` structural directive.

The directive can also be used as an attribute directive but we recommend to you to use it as a structural directive.

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

## What's Next?

* [Multi Tenancy](./Multi-Tenancy.md)