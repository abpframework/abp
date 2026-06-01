import { eLayoutType, ReplaceableTemplateDirective, SubscriptionService } from '@abp/ng.core';
import { collapseWithMargin, slideFromBottom } from '@abp/ng.theme.shared';
import { AfterViewInit, Component, inject } from '@angular/core';
import { LayoutService } from '../../services/layout.service';
import { NgTemplateOutlet } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { LogoComponent } from '../logo/logo.component';
import { PageAlertContainerComponent } from '../page-alert-container/page-alert-container.component';
import { RoutesComponent } from '../routes/routes.component';
import { NavItemsComponent } from '../nav-items/nav-items.component';

@Component({
  selector: 'abp-layout-application',
  templateUrl: './application-layout.component.html',
  animations: [slideFromBottom, collapseWithMargin],
  providers: [LayoutService, SubscriptionService],
  imports: [
    NgTemplateOutlet,
    LogoComponent,
    PageAlertContainerComponent,
    RoutesComponent,
    NavItemsComponent,
    ReplaceableTemplateDirective,
    RouterOutlet,
  ],
})
export class ApplicationLayoutComponent implements AfterViewInit {
  public readonly service = inject(LayoutService);
  // required for dynamic component
  static type = eLayoutType.application;

  ngAfterViewInit() {
    this.service.subscribeWindowSize();
  }
}
