# Form Validation

Reactive forms in ABP Angular UI are validated by [ngx-validate](https://www.npmjs.com/package/@ngx-validate/core) and helper texts are shown automatically based on validation rules and error blueprints. You do not have to add any elements or components to your templates. The library handles that for you. Here is how the experience is:

<img alt="The ngx-validate library validates an Angular reactive form and an error text appears under each wrong input based on the validation rule and the error blueprint." src="./images/form-validation---error-display-user-experience.gif" width="990px" style="max-width:100%">

## How to Add New Error Messages

You can add a new error message by providing the `VALIDATION_BLUEPRINTS` injection token from your root module.

```js
import { VALIDATION_BLUEPRINTS } from "@ngx-validate/core";

@NgModule({
  // rest of the module metadata

  providers: [
    // other providers
    {
      provide: VALIDATION_BLUEPRINTS,
      useValue: {
        uniqueUsername: "::AlreadyExists[{{ username }}]",
      },
    },
  ],
})
export class AppModule {}
```

When a [validator](https://angular.io/guide/form-validation#defining-custom-validators) or an [async validator](https://angular.io/guide/form-validation#creating-asynchronous-validators) returns an error with the key given to the error blueprints (`uniqueUsername` here), the validation library will be able to display an error message after localizing according to the given key and interpolation params. The result will look like this:

<img alt="An already taken username is entered while creating new user and a custom error message appears under the input after validation." src="./images/form-validation---new-error-message.gif" width="990px" style="max-width:100%">

In this example;

- Localization key is `::AlreadyExists`.
- The interpolation param is `username`.
- Localization resource is defined as `"AlreadyExists": "Sorry, “{0}” already exists."`.
- And the validator should return `{ uniqueUsername: { username: "admin" } }` as the error object.

## How to Change Existing Error Messages

You can overwrite an existing error message by providing `VALIDATION_BLUEPRINTS` injection token from your root module. Let's imagine you have a custom localization resource for required inputs.

```json
"RequiredInput": "Oops! We need this input."
```

To use this instead of the built-in required input message, all you need to do is the following.

```js
import { VALIDATION_BLUEPRINTS } from "@ngx-validate/core";

@NgModule({
  // rest of the module metadata

  providers: [
    // other providers
    {
      provide: VALIDATION_BLUEPRINTS,
      useValue: {
        required: "::RequiredInput",
      },
    },
  ],
})
export class AppModule {}
```

The error message will look like this:

<img alt="A required field is cleared and the custom error message appears under the input." src="./images/form-validation---overwrite-error-message.gif" width="990px" style="max-width:100%">

## What's Next?

- [Settings](./Settings.md)
