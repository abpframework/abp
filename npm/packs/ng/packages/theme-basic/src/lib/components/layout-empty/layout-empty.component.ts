import { Component } from '@angular/core';
import { eLayoutType } from '@abp/ng.core';

@Component({
  selector: 'abp-layout-empty',
  template: `
    Layout-empty
    <router-outlet></router-outlet>
  `,
})
export class LayoutEmptyComponent {
  // required for dynamic component
  static type = eLayoutType.empty;
}
