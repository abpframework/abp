# ASP.NET Core MVC / Razor Pages UI: JavaScript Global Features API

`abp.globalFeatures` API allows you to get the enabled features of the [Global Features](../../../Global-Features.md) in the client side.

> This document only explains the JavaScript API. See the [Global Features](../../../Global-Features.md) document to understand the ABP Global Features system.

## Usage

````js
> abp.globalFeatures.enabledFeatures

[ 'Shopping.Payment', 'Ecommerce.Subscription' ]

> abp.globalFeatures.moduleEnabledFeatures

{ Ecommerce }

> abp.globalFeatures.moduleEnabledFeatures.Ecommerce

[ 'Ecommerce.Subscription', 'Ecommerce.Invoice' ]
````
