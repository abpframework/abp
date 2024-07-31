# ASP.NET Core MVC / Razor Pages UI: JavaScript Events API

`abp.event` object is a simple service that is used to publish and subscribe to global events **in the browser**.

> This API is not related to server side local or distributed events. It works in the browser boundaries to make the UI components (code parts) communicate in a loosely coupled way.

## Basic Usage

### Publishing Events

Use `abp.event.trigger` to publish events.

**Example: Publish a *Basket Updated* event**

````js
abp.event.trigger('basketUpdated');
````

This will trigger all the subscribed callbacks.

### Subscribing to the Events

Use `abp.event.on` to subscribe to events.

**Example: Consume the *Basket Updated* event**

````js
abp.event.on('basketUpdated', function() {
  console.log('Handled the basketUpdated event...');
});
````

You start to get events after you subscribe to the event.

### Unsubscribing from the Events

If you need to unsubscribe from a pre-subscribed event, you can use the `abp.event.off(eventName, callback)` function. In this case, you have the callback as a separate function declaration.

**Example: Subscribe & Unsubscribe**

````js
function onBasketUpdated() {
  console.log('Handled the basketUpdated event...');
}

//Subscribe
abp.event.on('basketUpdated', onBasketUpdated);

//Unsubscribe
abp.event.off('basketUpdated', onBasketUpdated);
````

You don't get events after you unsubscribe from the event.

## Event Arguments

You can pass arguments (of any count) to the `trigger` method and get them in the subscription callback.

**Example: Add the basket as the event argument**

````js
//Subscribe to the event
abp.event.on('basketUpdated', function(basket) {
  console.log('The new basket object: ');
  console.log(basket);
});

//Trigger the event
abp.event.trigger('basketUpdated', {
  items: [
    {
      "productId": "123",
      "count": 2
    },
    {
      "productId": "832",
      "count": 1
    }
  ]
});
````

### Multiple Arguments

If you want to pass multiple arguments, you can pass like `abp.event.on('basketUpdated', arg0, arg1, agr2)`. Then you can add the same argument list to the callback function on the subscriber side.

> **Tip:** Alternatively, you can send a single object that has a separate field for each argument. This makes easier to extend/change the event arguments in the future without breaking the subscribers.

