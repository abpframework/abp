import { eLayoutType, SubscriptionService } from '@abp/ng.core';
import { collapseWithMargin, slideFromBottom } from '@abp/ng.theme.shared';
import { AfterViewInit, Component, OnDestroy } from '@angular/core';
import { fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { eThemeBasicComponents } from '../../enums/components';

@Component({
  selector: 'abp-layout-application',
  templateUrl: './application-layout.component.html',
  animations: [slideFromBottom, collapseWithMargin],
  providers: [SubscriptionService],
})
export class ApplicationLayoutComponent implements AfterViewInit, OnDestroy {
  // required for dynamic component
  static type = eLayoutType.application;

  isCollapsed = true;

  smallScreen: boolean; // do not set true or false

  logoComponentKey = eThemeBasicComponents.Logo;

  routesComponentKey = eThemeBasicComponents.Routes;

  navItemsComponentKey = eThemeBasicComponents.NavItems;

  constructor(private subscription: SubscriptionService) {}

  private checkWindowWidth() {
    setTimeout(() => {
      if (window.innerWidth < 992) {
        if (this.smallScreen === false) {
          this.isCollapsed = false;
          setTimeout(() => {
            this.isCollapsed = true;
          }, 100);
        }
        this.smallScreen = true;
      } else {
        this.smallScreen = false;
      }
    }, 0);
  }

  ngAfterViewInit() {
    this.checkWindowWidth();

    const resize$ = fromEvent(window, 'resize').pipe(debounceTime(150));
    this.subscription.addOne(resize$, () => this.checkWindowWidth());
  }

  ngOnDestroy() {}
}
