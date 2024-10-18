# Angular UI: Current User

The current user information stored in Config State.

## How to Get a Current User Information Configuration

You can use the `getOne` or `getOne$` method of `ConfigStateService` to get a specific configuration property. For that, the property name should be passed to the method as parameter.

```js
// this.config is an instance of ConfigStateService

const currentUser = this.config.getOne("currentUser");

// or
this.config.getOne$("currentUser").subscribe(currentUser => {
   // use currentUser here
})
```
 
> See the [ConfigStateService](./config-state-service) for more information.
