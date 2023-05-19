# Form Input Component

The ABP FormInput Component is a reusable form input component for the text type.

# Inputs
* `label`
* `labelClass (default form-label)`
* `inputPlaceholder`
* `inputReadonly`
* `inputClass (default form-control)`

# Outputs
* `formBlur`
* `formFocus`

# Usage

The ABP FormInput component is a part of the `ThemeSharedModule` module. If you've imported that module into your module, there's no need to import it again. If not, then first import it as shown below:

```ts
import { ThemeSharedModule } from "@abp/ng.theme.shared";
import { FormInputDemoComponent } from "./FomrInputDemoComponent.component";

@NgModule({
  imports: [
   ThemeSharedModule,
   // ...
  ],
  declarations: [FormInputDemoComponent],
})
export class MyFeatureModule {}
```

Then, the `abp-form-input` component can be used. See the example below:

```html
<div class="row">
  <div class="col-4">
    <abp-form-input
	label="AbpAccount::UserNameOrEmailAddress"
	inputId="login-input-user-name-or-email-address"
     ></abp-form-input>
  </div>
</div>
```

See the form input result below:

![abp-form-input](./images/form-input.png)
