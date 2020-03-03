## Permission Management in Angular Projects

To get permission of authenticated user you can use `getGrantedPolicy` method of `ConfigState`.

You can use it as store selector:

```ts
this.store.selectSnapshot(ConfigState.getGrantedPolicy('AbpIdentity.Roles.Create'));
```

Or you can use it via `ConfigStateService`:

```ts
this.configStateService.getGrantedPolicy('AbpIdentity.Roles.Create');
```

### Permission Directive

You can use the `PermissionDirective` to manage visibility of a DOM Element accordingly to user's permission.

```html
<div *abpPermission="Policy Key">
  This content is only visible if the user has 'Policy Key' permission.
</div>
```

As shown above you can remove elements from DOM with structural abpPermission directive.

The directive can also be used as an attribute directive but we recommend to you to use it as a structural directive.

### Permission Guard

Use can use `PermissionGuard` if you want to control authenticated user's permission before navigating to the route.

Add `requiredPolicy` to the data of the route in your routing module.

```ts
const routes: Routes = [
  {
    path: 'path',
    component: YourComponent,
    canActivate: [PermissionGuard],
    data: {
      requiredPolicy: 'AbpIdentity.Roles.Create',
    },
  },
];
```

---

Policies and Granted Policies are stored in the `auth` property of `ConfigState`.
