import {RouterEvents, SubscriptionService} from '@abp/ng.core';
import { ChangeDetectorRef, Injectable, inject } from '@angular/core';
import { fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { eThemeBasicComponents } from '../enums';
import { DOCUMENT } from '@angular/common';

@Injectable()
export class LayoutService {
  private subscription = inject(SubscriptionService);
  private cdRef = inject(ChangeDetectorRef);

  document = inject(DOCUMENT);
  isCollapsed = true;

  smallScreen!: boolean; // do not set true or false

  logoComponentKey = eThemeBasicComponents.Logo;

  routesComponentKey = eThemeBasicComponents.Routes;

  navItemsComponentKey = eThemeBasicComponents.NavItems;

  constructor() {
    const subscription = this.subscription;
    const routerEvents = inject(RouterEvents);

    subscription.addOne(routerEvents.getNavigationEvents("End"),() => {
      this.isCollapsed = true;
    })
  }

  private checkWindowWidth() {
    const isSmallScreen = this.document.defaultView.innerWidth < 992;
    if (isSmallScreen && this.smallScreen === false) {
      this.isCollapsed = false;
      setTimeout(() => {
        this.isCollapsed = true;
      }, 100);
    }
    this.smallScreen = isSmallScreen;
    this.cdRef.detectChanges();
  }

  subscribeWindowSize() {
    this.checkWindowWidth();

    const resize$ = fromEvent(this.document.defaultView, 'resize').pipe(debounceTime(150));
    this.subscription.addOne(resize$, () => this.checkWindowWidth());
  }
}
