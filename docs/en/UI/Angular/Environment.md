# Environment

Every application needs some **environment** variables. In Angular world, this is usually managed by `environment.ts`, `environment.prod.ts` and so on. It is the same for ABP as well.

Current `Environment` configuration holds sub config classes as follows:

```js
export interface Environment {
  apis: Apis;
  application: Application;
  oAuthConfig: AuthConfig;
  production: boolean;
  remoteEnv?: RemoteEnv;
}
```

## Apis

```js
export interface Apis {
  [key: string]: ApiConfig;
  default: ApiConfig;
}

export interface ApiConfig {
  [key: string]: string;
  rootNamespace?: string;
  url: string;
}
```

Api config has to have a default config and it may have some additional ones for different modules.
I.e. you may want to connect to different Apis for different modules.

Take a look at following example

```json
{
  // ...
  "apis": {
    "default": {
      "url": "https://localhost:8080"
    },
    "AbpIdentity": {
      "url": "https://localhost:9090"
    }
  }
  // ...
}
```

When an api from `AbpIdentity` is called, the request will be sent to `"https://localhost:9090"`.
Everything else will be sent to `"https://localhost:8080"`

- `rootNamespace` **(new)** : Root namespace of the related API. e.g. Acme.BookStore

## Application

```js
export interface Application {
  name: string;
  baseUrl?: string;
  logoUrl?: string;
}
```

- `name`: Name of the backend Application. It is also used by `logo.component` if `logoUrl` is not provided.
- `logoUrl`: Url of the application logo. It is used by `logo.component`
- `baseUrl`: [For detailed information](./Multi-Tenancy.md#domain-tenant-resolver)

## AuthConfig

For authentication, we use angular-oauth2-oidc. Please check their [docs](https://github.com/manfredsteyer/angular-oauth2-oidc) out

## RemoteEnvironment

Some applications need to integrate an existing config into the `environment` used throughout the application.
Abp Framework supports this out of box.

To integrate an existing config json into the `environment`, you need to set `remoteEnv`

```js
export type customMergeFn = (
  localEnv: Partial<Config.Environment>,
  remoteEnv: any
) => Config.Environment;

export interface RemoteEnv {
  url: string;
  mergeStrategy: "deepmerge" | "overwrite" | customMergeFn;
  method?: string;
  headers?: ABP.Dictionary<string>;
}
```

- `url` \*: Required. The url to be used to retrieve environment config
- `mergeStrategy` \*: Required. Defines how the local and the remote `environment` json will be merged
  - `deepmerge`: Both local and remote `environment` json will be merged recursively. If both configs have same nested path, the remote `environment` will be prioritized.
  - `overwrite`: Remote `environment` will be used and local environment will be ignored.
  - `customMergeFn`: You can also provide your own merge function as shown in the example. It will take two parameters, `localEnv: Partial<Config.Environment>` and `remoteEnv` and it needs to return a `Config.Environment` object.
- `method`: HTTP method to be used when retrieving environment config. Default: `GET`
- `headers`: If extra headers are needed for the request, it can be set through this field.

## EnvironmentService

` EnvironmentService` is a singleton service, i.e. provided in root level of your application, and keeps the environment in the internal store.

### Before Use

In order to use the `EnvironmentService` you must inject it in your class as a dependency.

```js
import { EnvironmentService } from '@abp/ng.core';

@Component({
  /* class metadata here */
})
class DemoComponent {
  constructor(private environment: EnvironmentService) {}
}
```

You do not have to provide the `EnvironmentService` at module or component/directive level, because it is already **provided in root**.

### Get Methods

`EnvironmentService` has numerous get methods which allow you to get a specific value or all environment object.

Get methods with "$" at the end of the method name (e.g. `getEnvironment$`) return an RxJs stream. The streams are triggered when set or patched the state.

#### How to Get Environment Object

You can use the `getEnvironment` or `getEnvironment$` method of `EnvironmentService` to get all of the environment object. It is used as follows:

```js
// this.environment is instance of EnvironmentService

const environment = this.environment.getEnvironment();

// or
this.environment.getEnvironment$().subscribe((environment) => {
  // use environment here
});
```

#### How to Get API URL

The `getApiUrl` or `getApiUrl$` method is used to get a specific API URL from the environment object. This is how you can use it:

```js
// this.environment is instance of EnvironmentService

const apiUrl = this.environment.getApiUrl();
// environment.apis.default.url

this.environment.getApiUrl$("search").subscribe((searchUrl) => {
  // environment.apis.search.url
});
```

This method returns the `url` of a specific API based on the key given as its only parameter. If there is no key, `'default'` is used.

#### How to Set the Environment

`EnvironmentService` has a method named `setState` which allows you to set the state value.

```js
// this.environment is instance of EnvironmentService

this.environment.setState(newEnvironmentObject);
```

Note that **you do not have to call this method at application initiation**, because the environment variables are already being stored at start.

#### Environment Properties

Please refer to `Environment` type for all the properties. It can be found in the [environment.ts file](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/models/environment.ts#L4).
