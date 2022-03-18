import { AfterViewInit, Component } from '@angular/core';
import { eLayoutType, SubscriptionService } from '@abp/ng.core';
import { LayoutService } from '../../services/layout.service';

@Component({
  selector: 'abp-layout-account',
  templateUrl: './account-layout.component.html',
  providers: [LayoutService, SubscriptionService],
})
export class AccountLayoutComponent implements AfterViewInit {
  // required for dynamic component
  static type = eLayoutType.account;

  authWrapperKey = 'Account.AuthWrapperComponent';

  constructor(public service: LayoutService) {}

  ngAfterViewInit() {
    this.service.subscribeWindowSize();
  }
}
