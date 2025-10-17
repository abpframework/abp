```json
//[doc-seo]
{
    "Description": "Learn how to manage user permissions in ABP Framework with practical examples and methods for effective authorization control."
}
```

# Permission Management

A permission is a simple policy that is granted or prohibited for a particular user, role or client. You can read more about [authorization in ABP](../../fundamentals/authorization.md) document.

You can get permission of authenticated user using `getGrantedPolicy` or `getGrantedPolicy$` method of `PermissionService`.

You can get permission as boolean value:

```js
import { PermissionService } from '@abp/ng.core';
import { inject } from '@angular/core';

export class YourComponent {
  private permissionService = inject(PermissionService);

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

You can use `permissionGuard` if you want to control authenticated user's permission to access to the route during navigation.

* Import the permissionGuard from @abp/ng.core.
* Add `canActivate: [permissionGuard]` to your route object.
* Add `requiredPolicy` to the `data` property of your route in your routing module.

```js
import { permissionGuard } from '@abp/ng.core';
// ...
const routes: Routes = [
  {
    path: 'path',
    component: YourComponent,
    canActivate: [permissionGuard],
    data: {
        requiredPolicy: 'YourProjectName.YourComponent', // policy key for your component
    },
  },
];
```

## Customization

In some cases, a custom permission management may be needed. All you need to do is to replace the service with your own. Here is how to achieve this:

- First, create a service of your own. Let's call it `CustomPermissionService` and extend `PermissionService` from `@abp/ng.core` as follows:

```js
import { ConfigStateService, PermissionService } from '@abp/ng.core';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class CustomPermissionService extends PermissionService {
  constructor() {
    super(inject(ConfigStateService));
  }

  // This is an example to show how to override the methods
  getGrantedPolicy$(key: string) {
    return super.getGrantedPolicy$(key);
  }
}
```

- Then, in `app.config.ts`, provide this service as follows: 

```js
export const appConfig: ApplicationConfig = {
  providers: [
    // ...
    {
      provide: PermissionService,
      useExisting: CustomPermissionService,
    },
  ],
};
```

That's it. Now, when a directive/guard asks for `PermissionService` from angular, it will inject your service.
