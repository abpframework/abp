# Config State

`ConfigStateService` is a singleton service, i.e. provided in root level of your application, and is actually a façade for interacting with application configuration state in the `Store`.

## Before Use

In order to use the `ConfigStateService` you must inject it in your class as a dependency.

```js
import { ConfigStateService } from '@abp/ng.core';

@Component({
  /* class metadata here */
})
class DemoComponent {
  constructor(private config: ConfigStateService) {}
}
```

You do not have to provide the `ConfigStateService` at module or component/directive level, because it is already **provided in root**.

## Selector Methods

`ConfigStateService` has numerous selector methods which allow you to get a specific configuration or all configurations from the `Store`.

### How to Get All Configurations From the Store

You can use the `getAll` method of `ConfigStateService` to get all of the configuration object from the store. It is used as follows:

```js
// this.config is instance of ConfigStateService

const config = this.config.getAll();
```

### How to Get a Specific Configuration From the Store

You can use the `getOne` method of `ConfigStateService` to get a specific configuration property from the store. For that, the property name should be passed to the method as parameter.

```js
// this.config is instance of ConfigStateService

const currentUser = this.config.getOne("currentUser");
```

On occasion, you will probably want to be more specific than getting just the current user. For example, here is how you can get the `tenantId`:

```js
const tenantId = this.config.getDeep("currentUser.tenantId");
```

or by giving an array of keys as parameter:

```js
const tenantId = this.config.getDeep(["currentUser", "tenantId"]);
```

FYI, `getDeep` is able to do everything `getOne` does. Just keep in mind that `getOne` is slightly faster.

#### Config State Properties

Please refer to `Config.State` type for all the properties you can get with `getOne` and `getDeep`. It can be found in the [config.ts file](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/models/config.ts#L7).

### How to Get the Application Information From the Store

The `getApplicationInfo` method is used to get the application information from the environment variables stored as the config state. This is how you can use it:

```js
// this.config is instance of ConfigStateService

const appInfo = this.config.getApplicationInfo();
```

This method never returns `undefined` or `null` and returns an empty object literal (`{}`) instead. In other words, you will never get an error when referring to the properties of `appInfo` above.

#### Application Information Properties

Please refer to `Config.Application` type for all the properties you can get with `getApplicationInfo`. It can be found in the [config.ts file](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/models/config.ts#L21).

### How to Get API URL From the Store

The `getApplicationInfo` method is used to get a specific API URL from the environment variables stored as the config state. This is how you can use it:

```js
// this.config is instance of ConfigStateService

const apiUrl = this.config.getApiUrl();
// environment.apis.default.url

const searchUrl = this.config.getApiUrl("search");
// environment.apis.search.url
```

This method returns the `url` of a specific API based on the key given as its only parameter. If there is no key, `'default'` is used.

### How to Get All Settings From the Store

You can use the `getSettings` method of `ConfigStateService` to get all of the settings object from the configuration state. Here is how you get all settings:

```js
// this.config is instance of ConfigStateService

const settings = this.config.getSettings();
```

In addition, the method lets you search settings by **passing a keyword** to it.

```js
const localizationSettings = this.config.getSettings("Localization");
/*
{
	'Abp.Localization.DefaultLanguage': 'en'
}
*/
```

Beware though, **settings search is case sensitive**.

### How to Get a Specific Setting From the Store

You can use the `getSetting` method of `ConfigStateService` to get a specific setting from the configuration state. Here is an example:

```js
// this.config is instance of ConfigStateService

const defaultLang = this.config.getSetting("Abp.Localization.DefaultLanguage");
// 'en'
```

### How to Get a Specific Permission From the Store

You can use the `getGrantedPolicy` method of `ConfigStateService` to get a specific permission from the configuration state. For that, you should pass a policy key as parameter to the method.

```js
// this.config is instance of ConfigStateService

const hasIdentityPermission = this.config.getGrantedPolicy("Abp.Identity");
// true
```

You may also **combine policy keys** to fine tune your selection:

```js
// this.config is instance of ConfigStateService

const hasIdentityAndAccountPermission = this.config.getGrantedPolicy(
  "Abp.Identity && Abp.Account"
);
// false

const hasIdentityOrAccountPermission = this.config.getGrantedPolicy(
  "Abp.Identity || Abp.Account"
);
// true
```

Please consider the following **rules** when creating your permission selectors:

- Maximum 2 keys can be combined.
- `&&` operator looks for both keys.
- `||` operator looks for either key.
- Empty string `''` as key will return `true`
- Using an operator without a second key will return `false`

### How to Get Translations From the Store

The `getLocalization` method of `ConfigStateService` is used for translations. Here are some examples:

```js
// this.config is instance of ConfigStateService

const identity = this.config.getLocalization("AbpIdentity::Identity");
// 'identity'

const notFound = this.config.getLocalization("AbpIdentity::IDENTITY");
// 'AbpIdentity::IDENTITY'

const defaultValue = this.config.getLocalization({
  key: "AbpIdentity::IDENTITY",
  defaultValue: "IDENTITY"
});
// 'IDENTITY'
```

Please check out the [localization documentation](./Localization.md) for details.

## Dispatch Methods

`ConfigStateService` has several dispatch methods which allow you to conveniently dispatch predefined actions to the `Store`.

### How to Get Application Configuration From Server

The `dispatchGetAppConfiguration` triggers a request to an endpoint that responds with the application state and then places this response to the `Store` as configuration state.

```js
// this.config is instance of ConfigStateService

this.config.dispatchGetAppConfiguration();
// returns a state stream which emits after dispatch action is complete
```

Note that **you do not have to call this method at application initiation**, because the application configuration is already being received from the server at start.

### How to Patch Route Configuration

The `dispatchPatchRouteByName` finds a route by its name and replaces its configuration in the `Store` with the new configuration passed as the second parameter.

```js
// this.config is instance of ConfigStateService

const newRouteConfig: Partial<ABP.Route> = {
  name: "Home",
  path: "home",
  children: [
    {
      name: "Dashboard",
      path: "dashboard"
    }
  ]
};

this.config.dispatchPatchRouteByName("::Menu:Home", newRouteConfig);
// returns a state stream which emits after dispatch action is complete
```

### How to Add a New Route Configuration

The `dispatchAddRoute` adds a new route to the configuration state in the `Store`. For this, the route config should be passed as the parameter of the method.

```js
// this.config is instance of ConfigStateService

const newRoute: ABP.Route = {
  name: "My New Page",
  iconClass: "fa fa-dashboard",
  path: "page",
  invisible: false,
  order: 2,
  requiredPolicy: "MyProjectName.MyNewPage"
};

this.config.dispatchAddRoute(newRoute);
// returns a state stream which emits after dispatch action is complete
```

The `newRoute` will be placed as at root level, i.e. without any parent routes and its url will be stored as `'/path'`.

If you want **to add a child route, you can do this:**

```js
import { eIdentityRouteNames } from '@abp/ng.identity';

// this.config is instance of ConfigStateService

const newRoute: ABP.Route = {
  parentName: eIdentityRouteNames.IdentityManagement,
  name: "My New Page",
  iconClass: "fa fa-dashboard",
  path: "page",
  invisible: false,
  order: 2,
  requiredPolicy: "MyProjectName.MyNewPage"
};

this.config.dispatchAddRoute(newRoute);
// returns a state stream which emits after dispatch action is complete
```

The `newRoute` will then be placed as a child of the parent route named `eIdentityRouteNames.IdentityManagement` and its url will be set as `'/identity/page'`.

#### Route Configuration Properties

Please refer to `ABP.Route` type for all the properties you can pass to `dispatchSetEnvironment` in its parameter. It can be found in the [common.ts file](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/models/common.ts#L27).

### How to Set the Environment

The `dispatchSetEnvironment` places environment variables passed to it in the `Store` under the configuration state. Here is how it is used:

```js
// this.config is instance of ConfigStateService

this.config.dispatchSetEnvironment({
  /* environment properties here */
});
// returns a state stream which emits after dispatch action is complete
```

Note that **you do not have to call this method at application initiation**, because the environment variables are already being stored at start.

#### Environment Properties

Please refer to `Config.Environment` type for all the properties you can pass to `dispatchSetEnvironment` as parameter. It can be found in the [config.ts file](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/models/config.ts#L13).

## What's Next?

- [Modifying the Menu](./Modifying-the-Menu.md)