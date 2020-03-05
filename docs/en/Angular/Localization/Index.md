## Localization in Angular Projects

There are three ways to use localization in your project:

- Via [localization pipe](#using-the-localization-pipe) in your component's template
- Via [localization service](#using-the-localization-service) in your TypeScript files.
- Via [the Config State](#using-the-config-state)

Before you read about _the Localization Pipe_ and _the Localization Service_, you should know about localization keys.

The Localization key format consists of 2 sections which are **Resource Name** and **Key**.
`ResourceName::Key`

> If you do not specify the resource name, it will be `defaultResourceName` which is declared in _environment.ts_

```ts
const environment = {
  //...
  localization: {
    defaultResourceName: 'MyProjectName',
  },
};
```

So this two are the same:

```html
<h1>{{ '::Key' | abpLocalization }}</h1>

<h1>{{ 'MyProjectName::Key' | abpLocalization }}</h1>
```

### Using the Localization Pipe

You can use the `abpLocalization` pipe to get localized text as in this example:

```html
<h1>{{ 'Resource::Key' | abpLocalization }}</h1>
```

The pipe will replace the key with the localized text.

You can also specify a default value as shown below:

```html
<h1>{{ { key: 'Resource::Key', defaultValue: 'Default Value' } | abpLocalization }}</h1>
```

To use interpolation, you must give the values for interpolation as pipe parameters, for example:

Localization data is stored in key-value pairs:

```js
{
  //...
  AbpAccount: { // This is the resource name
    Key: "Value",
    PagerInfo: "Showing {0} to {1} of {2} entries"
  }
}
```

So we can use this key like this:

```html
<h1>{{ 'AbpAccount::PagerInfo' | abpLocalization:'20':'30':'50' }}</h1>

<!-- Output: Showing 20 to 30 of 50 entries -->
```

### Using the Localization Service

First of all you should import the `LocalizationService` from **@abp/ng.core**

```ts
import { LocalizationService } from '@abp/ng.core';

class MyClass {
  constructor(private localizationService: LocalizationService) {}
}
```

After that, you are able to use localization service.

> You can add interpolation parameters as arguments to `instant()` and `get()` methods.

```ts
this.localizationService.instant('AbpIdentity::UserDeletionConfirmation', 'John');

// with fallback value
this.localizationService.instant(
  { key: 'AbpIdentity::UserDeletionConfirmation', defaultValue: 'Default Value' },
  'John',
);

// Output
// User 'John' will be deleted. Do you confirm that?
```

To get a localized text as [_Observable_](https://rxjs.dev/guide/observable) use `get` method instead of `instant`:

```ts
this.localizationService.get('Resource::Key');

// with fallback value
this.localizationService.get({ key: 'Resource::Key', defaultValue: 'Default Value' });
```

### Using the Config State

In order to you `getLocalization` method you should import ConfigState.

```ts
import { ConfigState } from '@abp/ng.core';
```

Then you can use it as followed:

```ts
this.store.selectSnapshot(ConfigState.getLocalization('ResourceName::Key'));
```

`getLocalization` method can be used with both `localization key` and [`LocalizationWithDefault`](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/models/config.ts#L34) interface.

```ts
this.store.selectSnapshot(
  ConfigState.getLocalization(
    {
      key: 'AbpIdentity::UserDeletionConfirmation',
      defaultValue: 'Default Value',
    },
    'John',
  ),
);
```

---

Localization resources are stored in the `localization` property of `ConfigState`.
