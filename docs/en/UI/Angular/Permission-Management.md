# Permission Management

A permission is a simple policy that is granted or prohibited for a particular user, role or client. You can read more about [authorization in ABP](../../Authorization.md) document.

You can get permission of authenticated user using `getGrantedPolicy` selector of `ConfigState`.

You can get permission as boolean value from store:

```js
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';

export class YourComponent {
  constructor(private store: Store) {}

  ngOnInit(): void {
    const canCreate = this.store.selectSnapshot(ConfigState.getGrantedPolicy('AbpIdentity.Roles.Create'));
  }

  // ...
}
```

Or you can get it via `ConfigStateService`:

```js
import { ConfigStateService } from '../services/config-state.service';

export class YourComponent {
  constructor(private configStateService: ConfigStateService) {}

  ngOnInit(): void {
    const canCreate = this.configStateService.getGrantedPolicy('AbpIdentity.Roles.Create');
  }

  // ...
}
```

## Permission Directive

You can use the `PermissionDirective` to manage visibility of a DOM Element accordingly to user's permission.

```html
<div *abpPermission="AbpIdentity.Roles">
  This content is only visible if the user has 'AbpIdentity.Roles' permission.
</div>
```

As shown above you can remove elements from DOM with `abpPermission` structural directive.

The directive can also be used as an attribute directive but we recommend to you to use it as a structural directive.

## Permission Guard

You can use `PermissionGuard` if you want to control authenticated user's permission to access to the route during navigation.

Add `requiredPolicy` to the `routes` property in your routing module.

```js
const routes: Routes = [
  {
    path: 'path',
    component: YourComponent,
    canActivate: [PermissionGuard],
    data: {
      routes: {
        requiredPolicy: 'AbpIdentity.Roles.Create',
      },
    },
  },
];
```

Granted Policies are stored in the `auth` property of `ConfigState`.

## What's Next?

- [Confirmation Popup](./Confirmation-Service.md)