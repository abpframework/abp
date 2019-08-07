import { Component } from '@angular/core';
import { eLayoutType } from '@abp/ng.core';

@Component({
  selector: 'abp-layout-account',
  templateUrl: './layout-account.component.html',
})
export class LayoutAccountComponent {
  // required for dynamic component
  static type = eLayoutType.account;

  isCollapsed: boolean = false;
}
