import { eLayoutType, takeUntilDestroy } from '@abp/ng.core';
import { collapseWithMargin, slideFromBottom } from '@abp/ng.theme.shared';
import { AfterViewInit, Component, OnDestroy } from '@angular/core';
import { fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { eThemeBasicComponents } from '../../enums/components';

@Component({
  selector: 'abp-layout-application',
  templateUrl: './application-layout.component.html',
  animations: [slideFromBottom, collapseWithMargin],
})
export class ApplicationLayoutComponent implements AfterViewInit, OnDestroy {
  // required for dynamic component
  static type = eLayoutType.application;

  isCollapsed = true;

  smallScreen: boolean; // do not set true or false

  logoComponentKey = eThemeBasicComponents.Logo;

  routesComponentKey = eThemeBasicComponents.Routes;

  navItemsComponentKey = eThemeBasicComponents.NavItems;

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

    fromEvent(window, 'resize')
      .pipe(takeUntilDestroy(this), debounceTime(150))
      .subscribe(() => {
        this.checkWindowWidth();
      });
  }

  ngOnDestroy() {}
}
