# Angular UI 3.3 to 4.0 Migration Guide

## Angular v11

The new ABP Angular UI is based on Angular v11 and TypeScript v4. The difference between v10 and v11 is non-breaking so you do not have to update right away but it is recommended. Nevertheless, ABP modules will keep working with Angular v10. Therefore, if your project is Angular v10, you do not need to update to Angular 11. The update is usually very easy though.

You can read more about Angular v11 [here](https://blog.angular.io/version-11-of-angular-now-available-74721b7952f7)

## **Breaking Changes**

### **Localization**

Prior to ABP 4.x, we'd handled what locale files of Angular should be created to load them lazily. However, this made it impossible to add new locale files (to be lazily loaded) for our users. With ABP 4.x, we enabled an option to pass a function to `CoreModule`.

The quickest solution is as follows:

```typescript
// app.module.ts

import { registerLocale } from '@abp/ng.core/locale';
// or 
// import { registerLocale } from '@volo/abp.ng.language-management/locale';
// if you have commercial license


@NgModule({
imports: [
  // ...
  CoreModule.forRoot({
    // ...other options,
    registerLocaleFn: registerLocale()
  }),
  //...
]
export class AppModule {}
```

You can find the related issue [here](https://github.com/abpframework/abp/issues/6066)
Also, please refer to [the docs](https://docs.abp.io/en/abp/latest/UI/Angular/Localization#registering-a-new-locale) for more information.

### **Removed the Angular Account Module Public UI**

With ABP 4.x, we have retired `@abp/ng.account`, it is no longer a part of our framework. There won't be any newer versions of this package as well. Therefore, you can delete anything related to this package. 
There should be a config in `app-routing.module` for path `account` and `AccountConfigModule` import in `app.module` 

However, if you are using the commercial version of this package, a.k.a `@volo/abp.ng.account`, this package will continue to exist because it contains `AccountAdminModule` which is still being maintained and developed. You only need to delete the route config from `app-routing.module`

You can find the related issue [here](https://github.com/abpframework/abp/issues/5652)

Angular UI is using the Authorization Code Flow to authenticate since version 3.1.0 by default. Starting from version 4.0, this is becoming the only option, because it is the recommended way of authenticating SPAs.

If you haven't done it yet, see [this post](https://blog.abp.io/abp/ABP-Framework-v3.1-RC-Has-Been-Released) to change the authentication of your application.

### State Management

In the ABP Angular UI, we've been using `NGXS` for state management. However, we've decided that the Angular UI should be agnostic with regard to state management. Our users should be able to handle the state in any way they prefer. They should be able to use any library other than `NGXS` or no library at all. That's why we have created our internal store in version 3.2. It is a simple utility class that employs `BehaviorSubject` internally. 

You can examine it [here](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/utils/internal-store-utils.ts)

With version 4.0, we will keep utilizing our `InternalStore` instead of `@ngxs/store` in our services and move away from `@ngxs/store`. We plan to remove any dependency of `NGXS` by version 5.0. 

With this in mind, we've already deprecated some services and implemented some breaking changes.

#### Removed the `SessionState`

Use `SessionStateService` instead of the `SessionState`. See [this issue](https://github.com/abpframework/abp/issues/5606) for details.

#### Deprecated the `ConfigState`

`ConfigState` is now deprecated and should not be used.

`ConfigState` reference removed from `CoreModule`. If you want to use the `ConfigState` (not recommended), you should pass the state to `NgxsModule` as shown below:

```typescript
//app.module.ts

import { ConfigState } from '@abp/ng.core';

// ...

imports: [ 
   NgxsModule.forRoot([ConfigState]),
// ...
```

Moving away from the global store, we create small services with a single responsibility. There are two new services available in version 4.0 which are `EnvironmentService` and `PermissionService`.

See [the related issue](https://github.com/abpframework/abp/issues/6154)

Please refer to the following docs for detail information and examples
- [`ConfigStateService`](../UI/Angular/Config-State-Service)
- [`EnvironmentService`](../UI/Angular/Environment#EnvironmentService)
- [`PermissionService`](../UI/Angular/Permission-Management#)

### Deprecated Interfaces

  Some interfaces have long been marked as deprecated and now they are removed.

- Removed replaceable components state.
- Removed legacy identity types and service.
- Removed legacy tenant management types and service.
- Removed legacy feature management types and services.
- Removed legacy permission management types and service.

### Deprecated commercial interfaces
- Removed legacy audit logging types and services.
- Removed legacy identity types and services.
- Removed legacy language management types and services.
- Removed legacy saas types and services.

### Identity Server [COMMERCIAL]

With the new version of Identity Server, there happened some breaking changes in the backend (also in the database). We've implemented those in the Angular UI.
If you are just using the package `@volo/abp.ng.identity-server` as is, you will not need to do anything. 
However, there are a couple of breaking changes we need to mention.

- As we have stated above, we want to remove the dependency of `NGXS`. Thus, we have deleted all of the actions defined in `identity-server.actions`. Those actions are not needed anymore and the state is managed locally. With the actions gone, `IdentityServerStateService` became unused and got deleted as well. 

- `ApiScope` is also available as a new entity (It was part of `ApiResource` before). It provides tokens for entity prop, entity actions, toolbar, edit and create form contributors like the existing ones which are `Client`, `IdentityResource` and `ApiResource`

- There were some deprecated interfaces within the `IdentityServer` namespace. Those are no longer being used, instead, their replacements were generated by `ABP Cli` using the `generate-proxy` command. 
