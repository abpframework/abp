```json
//[doc-seo]
{
    "Description": "Learn how to implement the `LoadingDirective` in your ABP Framework app to enhance user experience with loading spinners in your UI."
}
```

# Loading Directive


You may want to block a part of the UI and show a spinner for a while; the `LoadingDirective` directive makes this for you. `LoadingDirective` has been exposed by the `@abp/ng.theme.shared` package.


## Getting Started

In order to use the `LoadingDirective` in an HTML template, it should be imported into your component like this:

```js
// ...
import { LoadingDirective } from '@abp/ng.theme.shared';

@Component({
  // ...
  imports: [ LoadingDirective ]
  // ...
})
export class SampleComponent {}
```


## Usage

The `LoadingDirective` is easy to use. The directive's selector is **`abpLoading`**. By adding the `abpLoading` attribute to an HTML element, you can activate the `LoadingDirective` for the HTML element when the value is true.

See an example usage:

```html
<div [abpLoading]="true">
    Lorem ipsum dolor sit, amet consectetur adipisicing elit. Laboriosam commodi quae aspernatur,
    corporis velit et suscipit id consequuntur amet minima expedita cum reiciendis dolorum
    cupiditate? Voluptas eaque voluptatum odio deleniti quo vel illum nemo accusamus nulla ratione
    impedit dolorum expedita necessitatibus fugiat ullam beatae, optio eum cupiditate ducimus
    architecto.
</div>
```


The `abpLoading` attribute has been added to the `<div>` element that contains very a long text inside to activate the `LoadingDirective`.

See the result:

![Loading directive result](./images/abp-loading.png)
