import { AfterViewInit, Component, inject } from '@angular/core';
import { eLayoutType, ReplaceableTemplateDirective, SubscriptionService } from '@abp/ng.core';
import { LayoutService } from '../../services/layout.service';
import { NgTemplateOutlet } from '@angular/common';
import { LogoComponent } from '../logo/logo.component';
import { RoutesComponent } from '../routes/routes.component';
import { NavItemsComponent } from '../nav-items/nav-items.component';
import { AuthWrapperComponent } from './auth-wrapper/auth-wrapper.component';
import { PageAlertContainerComponent } from '../page-alert-container/page-alert-container.component';
import { RouterOutlet } from '@angular/router';
import { collapseWithMargin } from '@abp/ng.theme.shared';

@Component({
  selector: 'abp-layout-account',
  templateUrl: './account-layout.component.html',
  animations: [collapseWithMargin],
  providers: [LayoutService, SubscriptionService],
  imports: [
    NgTemplateOutlet,
    LogoComponent,
    RoutesComponent,
    NavItemsComponent,
    AuthWrapperComponent,
    PageAlertContainerComponent,
    ReplaceableTemplateDirective,
    RouterOutlet,
  ],
})
export class AccountLayoutComponent implements AfterViewInit {
  service = inject(LayoutService);

  // required for dynamic component
  static type = eLayoutType.account;

  authWrapperKey = 'Account.AuthWrapperComponent';

  ngAfterViewInit() {
    this.service.subscribeWindowSize();
  }
}
