# Angular UI: Current User

Current user information store in Config State.

### How to Get a Current User Information Configuration

You can use the `getOne` or `getOne$` method of `ConfigStateService` to get a specific configuration property. For that, the property name should be passed to the method as parameter.

```js
// this.config is instance of ConfigStateService

const currentUser = this.config.getOne("currentUser");

// or
this.config.getOne$("currentUser").subscribe(currentUser => {
   // use currentUser here
})
```
 
> See the [ConfigStateService](./Config-State-Service) for more information.
