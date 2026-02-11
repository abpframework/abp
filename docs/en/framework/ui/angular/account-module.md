```json
//[doc-seo]
{
    "Description": "Learn how to integrate the Angular UI Account Module into your project for seamless user authentication with login, registration, and account management features."
}
```

# Angular UI Account Module

Angular UI account module is available as of v4.3. It contains some pages (login, register, My account, etc.).

If you add the account module to your project;

- "My account" link in the current user dropdown on the top bar will redirect the user to a page in the account module.
- You can switch the authentication flow to the resource owner password flow.

## Account Module Implementation

Install the `@abp/ng.account` NPM package by running the below command:

```bash
npm install @abp/ng.account
```

> Make sure v4.3 or higher version is installed.

Open the `app.config.ts` and add `provideAccountConfig()` to the providers array as shown below:

```js
// app.config.ts

import { provideAccountConfig } from "@abp/ng.account/config";
// ...

export const appConfig: ApplicationConfig = {
  providers: [
    // ...
    provideAccountConfig(),
    // ...
  ],
};
```

Open the `app.routes.ts` and add the `account` route to `APP_ROUTES` array as follows:

```js
// app.routes.ts
export const APP_ROUTES: Routes = [
  //...
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(c => c.createRoutes()),
  },
  //...
];
```

## Account Public Module Implementation for Commercial Templates

The pro startup template comes with `@volo/abp.ng.account` package. You should update the package version to v4.3 or higher version. The package can be updated by running the following command:

```bash
npm install @volo/abp.ng.account
```

> Make sure v4.3 or higher version is installed.

Open the `app.config.ts` and add `provideAccountPublicConfig()` to the providers array as shown below:

> Ensure that the `Account Layout Provider` has been added if you are using the Lepton X theme. If you miss the step, you will get an error message that says `Account layout not found. Please check your configuration. If you are using LeptonX, please make sure you have added "provideAccountLayout()" to your app configuration.` Otherwise, you can skip adding the `provideAccountLayout()` step.

```js
// app.config.ts

import { provideAccountPublicConfig } from "@volo/abp.ng.account/public/config";
// if you are using or want to use Lepton X, you should add provideAccountLayout
// import { provideAccountLayout } from '@volosoft/abp.ng.theme.lepton-x/account'

//...

export const appConfig: ApplicationConfig = {
  providers: [
    // ...
    provideAccountPublicConfig(),
    provideAccountLayout() // Only for Lepton X
    // ...
  ],
};
```

Open the `app.routes.ts` and add the `account` route to `APP_ROUTES` array as follows:

```js
// app.routes.ts

export const APP_ROUTES: Routes = [
  //...
  {
    path: 'account',
    loadChildren: () => import('@volo/abp.ng.account/public').then(c => c.createRoutes()),
  },
  //...
];
```

## My Account Page

Before v4.3, the "My account" link in the current user dropdown on the top bar redirected the user to MVC's profile management page. As of v4.3, if you added the account module to your project, the same link will land on a page in the Angular UI account module instead.

## Personal Info Page Confirm Message

When the user changes their own data on the personal settings tab in My Account, The data can not update the CurrentUser key of Application-Configuration. The information of the user is stored in claims. The only way to apply this information to the CurrentUser of Application-Configuration is user should log out and log in. When the Refresh-Token feature is implemented, it will be fixed. So We've added a confirmation alert.

If you want to disable these warning, You should set `isPersonalSettingsChangedConfirmationActive` false

```js
// app.routes.ts
export const APP_ROUTES: Routes = [
  //...
  {
    path: 'account',
    loadChildren: () => import('@volo/abp.ng.account/public').then(c => c.create({ isPersonalSettingsChangedConfirmationActive:false })),
  },
  //...
];
```

## Security Logs Page [COMMERCIAL]

Before v4.3, the "Security Logs" link in the current user dropdown on the top bar redirected the user to MVC's security logs page. As of v4.3, if you added the account module to your project, the same link will land on a page in the Angular UI account public module instead.

## Resource Owner Password Flow

OAuth is preconfigured as authorization code flow in Angular application templates by default. If you added the account module to your project, you can switch the flow to resource owner password flow by changing the OAuth configuration in the _environment.ts_ files as shown below:

```js
import { Config } from '@abp/ng.core';

export const environment = {
  // other options removed for sake of brevity

  oAuthConfig: {
    issuer: 'https://localhost:44305', // AuthServer url
    clientId: 'MyProjectName_App',
    dummyClientSecret: '1q2w3e*',
    scope: 'offline_access MyProjectName',
  },

  // other options removed for sake of brevity
} as Config.Environment;
```

See the [Authorization in Angular UI](./authorization.md) document for more details.
