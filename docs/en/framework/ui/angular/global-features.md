# Angular: Global Features API

The `ConfigStateService.getGlobalFeatures` API allows you to get the enabled features of the [Global Features](../../infrastructure/global-features.md) on the client side.

> This document only explains the JavaScript API. See the [Global Features](../../infrastructure/global-features.md) document to understand the ABP Global Features system.

## Usage

````js

import { ConfigStateService } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';

@Component({
  /* class metadata here */
})
class DemoComponent implements OnInit {
  constructor(private config: ConfigStateService) {}
 
  ngOnInit(): void {
    // Gets all enabled global features.
    const getGlobalFeatures = this.config.getGlobalFeatures();

    //Example result is: `{ enabledFeatures: [ 'Shopping.Payment', 'Ecommerce.Subscription' ] }`

    // or
    this.config.getGlobalFeatures$().subscribe(getGlobalFeatures => {
       // use getGlobalFeatures here
    })

    // Check the global feature is enabled
    this.config.getGlobalFeatureIsEnabled('Ecommerce.Subscription')

    //Example result is `true`

    this.config.getGlobalFeatureIsEnabled('My.Subscription')

     //Example result is `false`

    // or
    this.config.getGlobalFeatureIsEnabled$('Ecommerce.Subscription').subscribe((isEnabled:boolean) => {
       // use isEnabled here
    })
  }
}


