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
      "url": "https://localhost:8080",
    },
    "AbpIdentity": {
      "url": "https://localhost:9090",
    }
  },
  // ...
}
```

When an api from `AbpIdentity` is called, the request will be sent to `"https://localhost:9090"`. 
Everything else will be sent to `"https://localhost:8080"`

* `rootNamespace` **(new)** : Root namespace of the related API. e.g. Acme.BookStore

## Application

```js
 export interface Application {
  name: string;
  baseUrl?: string;
  logoUrl?: string;
}
```

* `name`: Name of the backend Application. It is also used by `logo.component` if `logoUrl` is not provided.
* `logoUrl`: Url of the application logo. It is used by `logo.component`
* `baseUrl`: [For detailed information](./Multi-Tenancy.md#domain-tenant-resolver)


## AuthConfig

For authentication, we use angular-oauth2-oidc. Please check their [docs](https://github.com/manfredsteyer/angular-oauth2-oidc) out

## RemoteEnvironment

Some applications need to integrate an existing config into the `environment` used throughout the application. 
Abp Framework supports this out of box.

To integrate an existing config json into the `environment`, you need to set `remoteEnv`

```js
export type customMergeFn = (
  localEnv: Partial<Config.Environment>,
  remoteEnv: any,
) => Config.Environment;

export interface RemoteEnv {
  url: string;
  mergeStrategy: 'deepmerge' | 'overwrite' | customMergeFn;
  method?: string;
  headers?: ABP.Dictionary<string>;
}
```

* `url` *: Required. The url to be used to retrieve environment config
* `mergeStrategy` *: Required. Defines how the local and the remote `environment` json will be merged
  * `deepmerge`: Both local and remote `environment` json will be merged recursively. If both configs have same nested path, the remote `environment` will be prioritized. 
  * `overwrite`: Remote `environment` will be used and local environment will be ignored.
  * `customMergeFn`: You can also provide your own merge function as shown in the example. It will take two parameters, `localEnv: Partial<Config.Environment>` and `remoteEnv` and it needs to return a `Config.Environment` object.
* `method`: HTTP method to be used when retrieving environment config. Default: `GET`
* `headers`: If extra headers are needed for the request, it can be set through this field.


## What's Next?

- [About Feature Libraries](./Feature-Libraries.md)
