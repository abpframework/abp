```json
//[doc-seo]
{
    "Description": "Discover how to implement the ABP Checkbox Component for reusable form input, enhancing your applications with ease and efficiency."
}
```

# Checkbox Component

The ABP Checkbox Component is a reusable form input component for the checkbox type.

# Inputs

-   `label`
-   `labelClass (default form-check-label)`
-   `checkboxId`
-   `checkboxReadonly`
-   `checkboxReadonly (default form-check-input)`
-   `checkboxStyle`

# Outputs

-   `checkboxBlur`
-   `checkboxFocus`

# Usage

The ABP Checkbox component (`AbpCheckboxComponent`) is a standalone component. You can import it directly in your component:

```ts
import { Component } from "@angular/core";
import { AbpCheckboxComponent } from "@abp/ng.theme.shared";

@Component({
  selector: 'app-checkbox-demo',
  imports: [AbpCheckboxComponent],
  templateUrl: './checkbox-demo.component.html',
})
export class CheckboxDemoComponent {}
```

Then, the `abp-checkbox` component can be used in your template. See the example below:

```html
<div class="form-check">
  <abp-checkbox label="Yes,I Agree" checkboxId="checkbox-input">
  </abp-checkbox>
</div>
```

See the checkbox input result below:

![abp-checkbox](./images/form-checkbox.png)
