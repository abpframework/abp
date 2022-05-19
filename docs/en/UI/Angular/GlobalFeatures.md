# Angular: Global Features API

`ConfigStateService.getGlobalFeatures` API allows you to get the enabled features of the [Global Features](../../Global-Features.md) in the client side.

> This document only explains the JavaScript API. See the [Global Features](../../Global-Features.md) document to understand the ABP Global Features system.

## Usage

````js

import { ConfigStateService } from '@abp/ng.core';

@Component({
  /* class metadata here */
})
class DemoComponent {
  constructor(private config: ConfigStateService) {}
}

// Gets all enabled global features.
const getGlobalFeatures = this.config.getGlobalFeatures ();

// { enabledFeatures: [ 'Shopping.Payment', 'Ecommerce.Subscription' ] }

// or
this.config.getGlobalFeatures$().subscribe(getGlobalFeatures => {
   // use getGlobalFeatures here
})

// Check the global feature is enabled
this.config.getGlobalFeatureIsEnabled('Ecommerce.Subscription')

true

> this.config.getGlobalFeatureIsEnabled('My.Subscription')

false

// or
this.config.getGlobalFeatureIsEnabled$('Ecommerce.Subscription').subscribe((isEnabled:boolean) => {
   // use isEnabled here
})

