import {RouterEvents, SubscriptionService} from '@abp/ng.core';
import { ChangeDetectorRef, Injectable } from '@angular/core';
import { fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { eThemeBasicComponents } from '../enums';

@Injectable()
export class LayoutService {
  isCollapsed = true;

  smallScreen!: boolean; // do not set true or false

  logoComponentKey = eThemeBasicComponents.Logo;

  routesComponentKey = eThemeBasicComponents.Routes;

  navItemsComponentKey = eThemeBasicComponents.NavItems;

  constructor(private subscription: SubscriptionService,
              private cdRef: ChangeDetectorRef,
              routerEvents:RouterEvents) {
    subscription.addOne(routerEvents.getNavigationEvents("End"),() => {
      this.isCollapsed = true;
    })
  }

  private checkWindowWidth() {
    const isSmallScreen = window.innerWidth < 992;
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

    const resize$ = fromEvent(window, 'resize').pipe(debounceTime(150));
    this.subscription.addOne(resize$, () => this.checkWindowWidth());
  }
}
