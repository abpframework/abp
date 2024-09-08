# Authorization in Angular UI

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
According to this flow, the user is redirected to an external login page which is built with MVC. So, if you need **to customize the login page**, please follow [this community article](https://abp.io/community/articles/how-to-customize-the-login-page-for-mvc-razor-page-applications-9a40f3cd).

### Resource Owner Password Flow

If you have used the [Angular UI account module](./account-module) in your project, you can switch to the resource owner password flow by changing the OAuth configuration in the _environment.ts_ files as shown below:

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

### Error Filtering

In [AuthFlowStrategy](https://github.com/abpframework/abp/blob/21e70fd66154d4064d03b1a438f20a2e4318715e/npm/ng-packs/packages/oauth/src/lib/strategies/auth-flow-strategy.ts#L24) class, there is a method called `listenToOauthErrors` that listens to `OAuthErrorEvent` errors. This method clears the localStorage for OAuth keys. However, in certain cases, we might want to skip this process. To achieve this, we can use the `AuthErrorFilterService`.
The `AuthErrorFilterService` is an abstract service that needs to be replaced with a custom implementation

> By default, this service is replaced in the `@abp/ng.oauth` package

### Usage

#### 1.Create an auth-filter.provider

```js
import { APP_INITIALIZER, inject } from '@angular/core';
import { AuthErrorFilter, AuthErrorEvent, AuthErrorFilterService } from '@abp/ng.core';
import { eCustomersAuthFilterNames } from '../enums';

export const CUSTOMERS_AUTH_FILTER_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureAuthFilter, multi: true },
];

type Reason = object & { error: { grant_type: string | undefined } };

function configureAuthFilter() {
  const errorFilterService = inject(
    AuthErrorFilterService<AuthErrorFilter<AuthErrorEvent>, AuthErrorEvent>,
  );
  const filter: AuthErrorFilter = {
    id: eCustomersAuthFilterNames.LinkedUser,
    executable: true,
    execute: (event: AuthErrorEvent) => {
      const { reason } = event;
      const {
        error: { grant_type },
      } = <Reason>(reason || {});

      return !!grant_type && grant_type === eCustomersAuthFilterNames.LinkedUser;
    },
  };

  return () => errorFilterService.add(filter);
}
```

- `AuthErrorFilter:` is a model for filter object and it have 3 properties
  - `id:` a unique key in the list for the filter object
  - `executable:` a status for the filter object. If it's false then it won't work, yet it'll stay in the list
  - `execute:` a function that stores the skip logic

#### 2.Add to the FeatureConfigModule

```js
import { ModuleWithProviders, NgModule } from "@angular/core";
import { CUSTOMERS_AUTH_FILTER_PROVIDER } from "./providers/auth-filter.provider";

@NgModule()
export class CustomersConfigModule {
  static forRoot(): ModuleWithProviders<CustomersConfigModule> {
    return {
      ngModule: CustomersConfigModule,
      providers: [CUSTOMERS_AUTH_FILTER_PROVIDER],
    };
  }
}
```

Now it'll skip the clearing of OAuth storage keys for `LinkedUser` grant_type if any `OAuthErrorEvent` occurs

#### Replace with custom implementation

Use the `AbstractAuthErrorFilter<T,E>` class for signs of process.

#### Example

`my-auth-error-filter.service.ts`

```js
import { Injectable, signal } from '@angular/core';
import { MyAuthErrorEvent } from 'angular-my-auth-oidc';
import { AbstractAuthErrorFilter, AuthErrorFilter } from '@abp/ng.core';

@Injectable({ providedIn: 'root' })
export class OAuthErrorFilterService extends AbstractAuthErrorFilter<
  AuthErrorFilter<MyAuthErrorEvent>,
  MyAuthErrorEvent
> {
  protected readonly _filters = signal<Array<AuthErrorFilter<MyAuthErrorEvent>>>([]);
  readonly filters = this._filters.asReadonly();

  get(id: string): AuthErrorFilter<MyAuthErrorEvent> {
    return this._filters().find(({ id: _id }) => _id === id);
  }

  add(filter: AuthErrorFilter<MyAuthErrorEvent>): void {
    this._filters.update(items => [...items, filter]);
  }

  patch(item: Partial<AuthErrorFilter<MyAuthErrorEvent>>): void {
    const _item = this.filters().find(({ id }) => id === item.id);
    if (!_item) {
      return;
    }

    Object.assign(_item, item);
  }

  remove(id: string): void {
    const item = this.filters().find(({ id: _id }) => _id === id);
    if (!item) {
      return;
    }

    this._filters.update(items => items.filter(({ id: _id }) => _id !== id));
  }

  run(event: MyAuthErrorEvent): boolean {
    return this.filters()
      .filter(({ executable }) => !!executable)
      .map(({ execute }) => execute(event))
      .some(item => item);
  }
}

```

## See Also

* [Video tutorials](https://abp.io/video-courses/essentials/authorization)