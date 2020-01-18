import { Component } from '@angular/core';
import { eLayoutType } from '@abp/ng.core';

@Component({
  selector: 'abp-layout-account',
  template: `
    <router-outlet></router-outlet>
    <abp-confirmation></abp-confirmation>
    <abp-toast-container right="30px" bottom="30px"></abp-toast-container>
  `,
})
export class AccountLayoutComponent {
  // required for dynamic component
  static type = eLayoutType.account;
}
