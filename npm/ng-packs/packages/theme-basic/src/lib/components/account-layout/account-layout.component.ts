import { AfterViewInit, Component } from '@angular/core';
import { eLayoutType, ReplaceableTemplateDirective, SubscriptionService } from '@abp/ng.core';
import { LayoutService } from '../../services/layout.service';
import { CommonModule } from '@angular/common';
import { LogoComponent } from '../logo/logo.component';
import { RoutesComponent } from '../routes/routes.component';
import { NavItemsComponent } from '../nav-items/nav-items.component';
import { AuthWrapperComponent } from './auth-wrapper/auth-wrapper.component';
import { PageAlertContainerComponent } from '../page-alert-container/page-alert-container.component';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'abp-layout-account',
  templateUrl: './account-layout.component.html',
  providers: [LayoutService, SubscriptionService],
  imports: [
    CommonModule,
    LogoComponent,
    RoutesComponent,
    NavItemsComponent,
    AuthWrapperComponent,
    PageAlertContainerComponent,
    ReplaceableTemplateDirective,
    RouterModule,
  ],
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
