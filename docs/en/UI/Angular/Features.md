# Features

You can get the value of a feature on the client-side using the [config state service](./Config-State.md) if it is allowed by the feature definition on the server-side.

> This document explains how to get feature values in an Angular application. See the [Features document](../../Features.md) to learn the feature system.

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

## How to Get a Specific Feature

You can use the `getFeature` method of `ConfigStateService` to get a specific feature from the configuration state. Here is an example:

```js
// this.config is instance of ConfigStateService

const defaultLang = this.config.getFeature("Identity.TwoFactor");
// 'Optional'
```

You can then check the value of the feature to perform your logic. Please note that **feature keys are case-sensitive**.
