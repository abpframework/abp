## Authorization in Angular UI

OAuth is preconfigured in Angular application templates. So, when you start a project using the CLI (or Suite, for that matter), authorization already works. ABP Angular UI packages are using [angular-oauth2-oidc library](https://github.com/manfredsteyer/angular-oauth2-oidc#logging-in) for managing OAuth in the Angular client. 
You can find **OAuth configuration** in the _environment.ts_ files.

### Authorization Code Flow 

```js
import { Config } from '@abp/ng.core';

const baseUrl = 'http://localhost:4200';

export const environment = {
  // other options removed for sake of brevity

  oAuthConfig: {
    issuer: 'https://localhost:44305',
    redirectUri: baseUrl,
    clientId: 'MyProjectName_App',
    responseType: 'code',
    scope: 'offline_access MyProjectName',
  },

  // other options removed for sake of brevity
} as Config.Environment;

```

This configuration results in an [OAuth authorization code flow with PKCE](https://tools.ietf.org/html/rfc7636).
According to this flow, the user is redirected to an external login page which is built with MVC. So, if you need **to customize the login page**, please follow [this community article](https://community.abp.io/articles/how-to-customize-the-login-page-for-mvc-razor-page-applications-9a40f3cd).


### Resource Owner Password Flow

If you implemented the [Angular UI account module](./Account-Module) to your project, you can switch the flow to resource owner password flow by changing the OAuth configuration in the _environment.ts_ files as shown below:

```js
import { Config } from '@abp/ng.core';

export const environment = {
  // other options removed for sake of brevity

  oAuthConfig: {
    issuer: 'https://localhost:44305',
    clientId: 'MyProjectName_App',
    dummyClientSecret: '1q2w3e*',
    scope: 'offline_access MyProjectName',
  },

  // other options removed for sake of brevity
} as Config.Environment;
```

According to this flow, the user is redirected to the login page in the account module.

> Note: The resource owner password flow does not support the two-factor authentication for some technical reasons.