# Settings

You can get settings on the client-side using the [config state service](./Config-State.md) if they are allowed by their setting definition on the server-side.

> This document only explains how settings work in the Angular UI projects. See the [settings document](../../Settings.md) to understand the ABP setting system.

## Before Use

To use the `ConfigStateService`, you must inject it in your class as a dependency. You do not have to provide the service explicitly, because it is already **provided in root**.

```js
import { ConfigStateService } from '@abp/ng.core';

@Component({
  /* class metadata here */
})
class DemoComponent {
  constructor(private config: ConfigStateService) {}
}
```

## How to Get a Specific Setting

You can use the `getSetting` method of `ConfigStateService` to get a specific setting from the configuration state. Here is an example:

```js
// this.config is instance of ConfigStateService

const defaultLang = this.config.getSetting("Abp.Localization.DefaultLanguage");
// 'en'
```

### How to Get All Settings From the Store

You can use the `getSettings` method of `ConfigStateService` to obtain all settings as an object where the object properties are setting names and property values are setting values.

```js
// this.config is instance of ConfigStateService

const settings = this.config.getSettings();
// all settings as a key value pair
```

Additionally, the method lets you search settings by **passing a keyword** to it.

```js
const localizationSettings = this.config.getSettings("Localization");
/*
{
	'Abp.Localization.DefaultLanguage': 'en'
}
*/
```

Beware though, **settings search is case-sensitive**.
