# Angular UI Account Module

Angular UI account module is available as of v4.3. It contains some pages (login, register, manage your profile, etc.).

If you add the account module to your project;

- "Manage your profile" link in the current user dropdown on the top bar will redirect the user to a page in the account module.
- You can switch the authentication flow to the resource owner password flow.


### Account Module Implementation

Install the `@abp/ng.account` NPM package by running the below command:

```bash
npm install @abp/ng.account
```

> Make sure v4.3 or higher version is installed.

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

### Account Public Module Implementation for Commercial Templates

The pro startup template comes with `@volo/abp.ng.account` package. You should update the package version to v4.3 or higher version. The package can be updated by running the following command:

```bash
npm install @volo/abp.ng.account
```
> Make sure v4.3 or higher version is installed.

Open the `app.module.ts` and add `AccountPublicConfigModule.forRoot()` to the imports array as shown below:

```js
// app.module.ts

import { AccountPublicConfigModule } from '@volo/abp.ng.account/public/config';
//...

@NgModule({
  imports: [
    //...
    AccountPublicConfigModule.forRoot()
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
    loadChildren: () => import('@volo/abp.ng.account/public').then(m => m.AccountPublicModule.forLazy()),
  },
  //...
export class AppRoutingModule {}
```

### Manage Profile Page

Before v4.3, the "Manage Your Profile" link in the current user dropdown on the top bar redirected the user to MVC's profile management page. As of v4.3, if you added the account module to your project, the same link will land on a page in the Angular UI account module instead.

### My Security Logs Page [COMMERCIAL]

Before v4.3, the "My Security Logs" link in the current user dropdown on the top bar redirected the user to MVC's my security logs page. As of v4.3, if you added the account module to your project, the same link will land on a page in the Angular UI account public module instead.

### Resource Owner Password Flow

OAuth is preconfigured as authorization code flow in Angular application templates by default. If you added the account module to your project, you can switch the flow to resource owner password flow by changing the OAuth configuration in the _environment.ts_ files as shown below:

```js
import { Config } from '@abp/ng.core';

export const environment = {
  // other options removed for sake of brevity

  oAuthConfig: {
    issuer: 'https://localhost:44305', // IdentityServer url
    clientId: 'MyProjectName_App',
    dummyClientSecret: '1q2w3e*',
    scope: 'offline_access MyProjectName',
  },

  // other options removed for sake of brevity
} as Config.Environment;
```

> Note: The resource owner password flow does not support the two-factor authentication for some technical reasons.

See the [Authorization in Angular UI](./Authorization.md) document for more details.
