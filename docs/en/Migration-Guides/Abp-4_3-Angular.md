# Angular UI v4.3 Migration Guide

## Breaking Changes

### Manage Profile Page

Prior to V4.3, the manage your profile link in the current user dropdown on the top bar redirects to MVC's manage profile page. As of v4.3, the URL will be changed as `/account/manage-profile` to redirect users to the manage profile page in the account module of Angular UI instead of MVC. So you have to install and implement the account module to your Angular project when you update the ABP to v4.3.

#### Account Module Implementation

Install the `@abp/ng.account` NPM package by running the below command:

```bash
npm install @abp/ng.account@next
```

> Make sure v4.3-rc or higher version is installed.

Open the `app.module.ts` and add `AccountConfigModule.forRoot()` to the imports array as shown below:

```js
// app.module.ts

import { AccountConfigModule } from '@abp/ng.account/config';
//...

@NgModule({
  imports: [
    //...
    AccountConfigModule.forRoot()
  ],
  //...
})
export class AppModule {}
```

Open the `app-routing.module.ts` and add the `account` route to `routes` array as follows:

```js
// app-routing.module.ts
const routes: Routes = [
  //...
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(m => m.AccountModule.forLazy()),
  },
  //...
export class AppRoutingModule {}
```

#### Account Module Implementation for Commercial Templates

The pro startup template comes with `@volo/abp.ng.account` package. You should update the package version to v4.3-rc or higher version. The package can be updated by running the following command:

```bash
npm install @volo/abp.ng.account@next
```
> Make sure v4.3-rc or higher version is installed.

`AccountConfigModule` is already imported to `app.module.ts` in the startup template. So no need to import the module to the `AppModule`. If you removed the `AccountConfigModule` from the `AppModule`, you can import it as shown below:

```js
// app.module.ts

import { AccountConfigModule } from '@volo/abp.ng.account/config';
//...

@NgModule({
  imports: [
    //...
    AccountConfigModule.forRoot()
  ],
  //...
})
export class AppModule {}
```

Open the `app-routing.module.ts` and add the `account` route to `routes` array as follows:

```js
// app-routing.module.ts
const routes: Routes = [
  //...
  {
    path: 'account',
    loadChildren: () => import('@volo/abp.ng.account').then(m => m.AccountPublicModule.forLazy()),
  },
  //...
export class AppRoutingModule {}
```