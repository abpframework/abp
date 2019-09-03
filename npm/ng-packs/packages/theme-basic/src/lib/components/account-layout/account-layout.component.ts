import { Component } from '@angular/core';
import { eLayoutType } from '@abp/ng.core';

@Component({
  selector: 'abp-layout-account',
  templateUrl: './account-layout.component.html',
})
export class AccountLayoutComponent {
  // required for dynamic component
  static type = eLayoutType.account;
}
