# Card Component

ABP Card component is a wrapper component for bootstrap card class.

## Usage

ABP Card component is a built-in `ThemeSharedModule` component. If you imported shared module into your feature module, you don't need to do anything. 

```ts
// my-feature.module.ts

import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { CardDemoComponent } from './chart-demo.component';

@NgModule({
  imports: [
    SharedModule,
    // ...
  ],
  declarations: [CardDemoComponent],
  // ...
})
export class MyFeatureModule {}

```

Then, `abp-card` component can be used. See example below:
```ts

// card-demo.component.ts

import { Component } from '@angular/core';

@Component({
  selector: 'app-card-demo',
  template: ` 
    <abp-card [cardStyle]="{width: '18rem'}">
      <abp-card-body>
        <abp-card-title>Lorem Ipsum</abp-card-title>
        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla commodo condimentum ligula, sed varius nibh eleifend sit amet. Maecenas facilisis vel arcu nec maximus.</abp-card-body>
    </abp-card> 
  `,
})
export class CardDemoComponent { }
```

See the result below:

![abp-card-component](./images/abp-card-component.png)

As you can see in above example, you can customize your card component's style with `cardStyle` input. Also you can add your custom classes with `cardClass` input.