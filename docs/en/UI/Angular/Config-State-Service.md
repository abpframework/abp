# Config State Service

`ConfigStateService` is a singleton service, i.e. provided in root level of your application, and keeps the application configuration response in the internal store.

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

## Get Methods

`ConfigStateService` has numerous get methods which allow you to get a specific configuration or all configurations.

Get methods with "$" at the end of the method name (e.g. `getAll$`) return an RxJs stream. The streams are triggered when set or patched the state.

### How to Get All Configurations

You can use the `getAll` or `getAll$` method of `ConfigStateService` to get all of the applcation configuration response object. It is used as follows:

```js
// this.config is instance of ConfigStateService

const config = this.config.getAll();

// or
this.config.getAll$().subscribe(config => {
   // use config here
})
```

### How to Get a Specific Configuration

You can use the `getOne` or `getOne$` method of `ConfigStateService` to get a specific configuration property. For that, the property name should be passed to the method as parameter.

```js
// this.config is instance of ConfigStateService

const currentUser = this.config.getOne("currentUser");

// or
this.config.getOne$("currentUser").subscribe(currentUser => {
   // use currentUser here
})
```

On occasion, you will probably want to be more specific than getting just the current user. For example, here is how you can get the `tenantId`:

```js
const tenantId = this.config.getDeep("currentUser.tenantId");

// or
this.config.getDeep$("currentUser.tenantId").subscribe(tenantId => {
   // use tenantId here
})
```

or by giving an array of keys as parameter:

```js
const tenantId = this.config.getDeep(["currentUser", "tenantId"]);
```

FYI, `getDeep` is able to do everything `getOne` does. Just keep in mind that `getOne` is slightly faster.

### How to Get a Feature

You can use the `getFeature` or `getFeature$` method of `ConfigStateService` to get a feature value. For that, the feature name should be passed to the method as parameter.

```js
// this.config is instance of ConfigStateService

const enableLdapLogin = this.configStateService.getFeature("Account.EnableLdapLogin");

// or
this.config.getFeature$("Account.EnableLdapLogin").subscribe(enableLdapLogin => {
   // use enableLdapLogin here
})
```

> For more information, see the [features document](./Features).

### How to Get a Setting

You can use the `getSetting` or `getSetting$` method of `ConfigStateService` to get a setting. For that, the setting name should be passed to the method as parameter.

```js
// this.config is instance of ConfigStateService

const twoFactorBehaviour = this.configStateService.getSetting("Abp.Identity.TwoFactor.Behaviour");

// or
this.config.getSetting$("Abp.Identity.TwoFactor.Behaviour").subscribe(twoFactorBehaviour => {
   // use twoFactorBehaviour here
})
```

> For more information, see the [settings document](./Settings).

#### State Properties

Please refer to `ApplicationConfiguration.Response` type for all the properties you can get with `getOne` and `getDeep`. It can be found in the [application-configuration.ts file](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/models/application-configuration.ts#L4).


## Set State

`ConfigStateService` has a method named `setState` which allow you to set the state value.

You can get the application configuration response and set the `ConfigStateService` state value as shown below:

```js
import {ApplicationConfigurationService, ConfigStateService} from '@abp/ng.core';

constructor(private applicationConfigurationService: ApplicationConfigurationService, private config: ConfigStateService) {
  this.applicationConfigurationService.getConfiguration().subscribe(config => {
    this.config.setState(config);
  })
}
```

## See Also

- [Settings](./Settings.md)
- [Features](./Features.md)
