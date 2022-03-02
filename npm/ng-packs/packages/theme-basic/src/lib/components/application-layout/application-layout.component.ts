import {eLayoutType, RouterEvents, SubscriptionService} from '@abp/ng.core';
import { collapseWithMargin, slideFromBottom } from '@abp/ng.theme.shared';
import {AfterViewInit, Component, OnDestroy} from '@angular/core';
import { LayoutService } from '../../services/layout.service';
import {Subscription} from "rxjs";

@Component({
  selector: 'abp-layout-application',
  templateUrl: './application-layout.component.html',
  animations: [slideFromBottom, collapseWithMargin],
  providers: [LayoutService, SubscriptionService],
})
export class ApplicationLayoutComponent implements AfterViewInit , OnDestroy{
  // required for dynamic component
  static type = eLayoutType.application;
  navigationEndSubscription:Subscription
  constructor(public service: LayoutService,
              routerEvents:RouterEvents,
              ) {
    this.navigationEndSubscription = routerEvents.getNavigationEvents('End').subscribe(() => {
      service.isCollapsed = true;
    })
  }

  ngAfterViewInit() {
    this.service.subscribeWindowSize();
  }

  ngOnDestroy(): void {
    this.navigationEndSubscription.unsubscribe()
  }
}
