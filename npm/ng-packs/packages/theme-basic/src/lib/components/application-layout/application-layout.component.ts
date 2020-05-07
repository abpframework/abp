import { eLayoutType, takeUntilDestroy } from '@abp/ng.core';
import { collapseWithMargin, slideFromBottom } from '@abp/ng.theme.shared';
import { AfterViewInit, Component, OnDestroy } from '@angular/core';
import { Store } from '@ngxs/store';
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

  isDropdownChildDynamic: boolean;

  isCollapsed = true;

  smallScreen: boolean; // do not set true or false

  logoComponentKey = eThemeBasicComponents.Logo;

  routesComponentKey = eThemeBasicComponents.Routes;

  navItemsComponentKey = eThemeBasicComponents.NavItems;

  constructor(private store: Store) {}

  private checkWindowWidth() {
    setTimeout(() => {
      if (window.innerWidth < 768) {
        this.isDropdownChildDynamic = false;
        if (this.smallScreen === false) {
          this.isCollapsed = false;
          setTimeout(() => {
            this.isCollapsed = true;
          }, 100);
        }
        this.smallScreen = true;
      } else {
        this.isDropdownChildDynamic = true;
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
