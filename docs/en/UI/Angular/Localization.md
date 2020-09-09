# Localization

Before you read about _the Localization Pipe_ and _the Localization Service_, you should know about localization keys.

The Localization key format consists of 2 sections which are **Resource Name** and **Key**.
`ResourceName::Key`

> If you do not specify the resource name, it will be `defaultResourceName` which is declared in `environment.ts`

```js
const environment = {
  //...
  localization: {
    defaultResourceName: 'MyProjectName',
  },
};
```

So these two are the same:

```html
<h1>{%{{{ '::Key' | abpLocalization }}}%}</h1>

<h1>{%{{{ 'MyProjectName::Key' | abpLocalization }}}%}</h1>
```

## Using the Localization Pipe

You can use the `abpLocalization` pipe to get localized text as in this example:

```html
<h1>{%{{{ 'Resource::Key' | abpLocalization }}}%}</h1>
```

The pipe will replace the key with the localized text.

You can also specify a default value as shown below:

```html
<h1>{%{{{ { key: 'Resource::Key', defaultValue: 'Default Value' } | abpLocalization }}}%}</h1>
```

To use interpolation, you must give the values for interpolation as pipe parameters, for example:

Localization data is stored in key-value pairs:

```js
{
  //...
  AbpAccount: { // AbpAccount is the resource name
    Key: "Value",
    PagerInfo: "Showing {0} to {1} of {2} entries"
  }
}
```

So we can use this key like this:

```html
<h1>{%{{{ 'AbpAccount::PagerInfo' | abpLocalization:'20':'30':'50' }}}%}</h1>

<!-- Output: Showing 20 to 30 of 50 entries -->
```

### Using the Localization Service

First of all you should import the `LocalizationService` from **@abp/ng.core**

```js
import { LocalizationService } from '@abp/ng.core';

class MyClass {
  constructor(private localizationService: LocalizationService) {}
}
```

After that, you are able to use localization service.

> You can add interpolation parameters as arguments to `instant()` and `get()` methods.

```js
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

```js
this.localizationService.get('Resource::Key');

// with fallback value
this.localizationService.get({ key: 'Resource::Key', defaultValue: 'Default Value' });
```

### Using the Config State

In order to you `getLocalization` method you should import ConfigState.

```js
import { ConfigState } from '@abp/ng.core';
```

Then you can use it as followed:

```js
this.store.selectSnapshot(ConfigState.getLocalization('ResourceName::Key'));
```

`getLocalization` method can be used with both `localization key` and [`LocalizationWithDefault`](https://github.com/abpframework/abp/blob/dev/npm/ng-packs/packages/core/src/lib/models/config.ts#L34) interface.

```js
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

Localization resources are stored in the `localization` property of `ConfigState`.

## RTL Support

As of v2.9 ABP has RTL support. If you are generating a new project with v2.9 and above, everything is set, you do not need to do any changes. If you are migrating your project from an earlier version, please follow the 2 steps below:

#### Step 1. Create Chunks for Bootstrap LTR and RTL

Find [styles configuration in angular.json](https://angular.io/guide/workspace-config#style-script-config) and make sure the chunks in your project has `bootstrap-rtl.min` and `bootstrap-ltr.min` as shown below.

```json
{
  "projects": {
    "MyProjectName": {
      "architect": {
        "build": {
          "options": {
            "styles": [
              {
                "input": "node_modules/@fortawesome/fontawesome-free/css/all.min.css",
                "inject": true,
                "bundleName": "fontawesome-all.min"
              },
              {
                "input": "node_modules/@fortawesome/fontawesome-free/css/v4-shims.min.css",
                "inject": true,
                "bundleName": "fontawesome-v4-shims.min"
              },
              {
                "input": "node_modules/@abp/ng.theme.shared/styles/bootstrap-rtl.min.css",
                "inject": false,
                "bundleName": "bootstrap-rtl.min"
              },
              {
                "input": "node_modules/bootstrap/dist/css/bootstrap.min.css",
                "inject": true,
                "bundleName": "bootstrap-ltr.min"
              },
              "apps/dev-app/src/styles.scss"
            ],
          }
        }
      }
    }
  }
}
```

#### Step 2. Clear Lazy Loaded Fontawesome in AppComponent

If you have created and injected chunks for Fontawesome as seen above, you no longer need the lazy loading in the `AppComponent` which was implemented before v2.9. Simply remove them. The `AppComponent` in the template of the new version looks like this:

```js
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <abp-loader-bar></abp-loader-bar>
    <router-outlet></router-outlet>
  `,
})
export class AppComponent {}
```

## Mapping of Culture Name to Angular Locale File Name

Some of the culture names defined in .NET do not match Angular locales. In such cases, the Angular app throws an error like below at runtime:

![locale-error](./images/locale-error.png)

If you see an error like this, you should pass the `cultureNameLocaleFileMap` property like below to CoreModule's forRoot static method.

```js
// app.module.ts

@NgModule({
  imports: [
    // other imports
     CoreModule.forRoot({
      // other options
      cultureNameLocaleFileMap: { 
        "DotnetCultureName": "AngularLocaleFileName",
        "pt-BR": "pt"  // example
      }
    })
    //...
```

See [all locale files in Angular](https://github.com/angular/angular/tree/master/packages/common/locales).


## See Also

* [Localization in ASP.NET Core](../../Localization.md)

## What's Next?

* [Permission Management](./Permission-Management.md)
