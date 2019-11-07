/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import { ConfigState, GetAppConfiguration, SessionState, SetLanguage, takeUntilDestroy } from '@abp/ng.core';
import { collapseWithMargin, slideFromBottom } from '@abp/ng.theme.shared';
import { Component, Renderer2, TemplateRef, ViewChild } from '@angular/core';
import { Navigate, RouterState } from '@ngxs/router-plugin';
import { Select, Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import compare from 'just-compare';
import { fromEvent, Observable } from 'rxjs';
import { debounceTime, filter, map } from 'rxjs/operators';
import snq from 'snq';
import { AddNavigationElement } from '../../actions';
import { LayoutState } from '../../states';
export class ApplicationLayoutComponent {
  /**
   * @param {?} store
   * @param {?} oauthService
   * @param {?} renderer
   */
  constructor(store, oauthService, renderer) {
    this.store = store;
    this.oauthService = oauthService;
    this.renderer = renderer;
    this.isCollapsed = true;
    this.rightPartElements = [];
    this.trackByFn
    /**
     * @param {?} _
     * @param {?} item
     * @return {?}
     */ = (_, item) => item.name;
    this.trackElementByFn
    /**
     * @param {?} _
     * @param {?} element
     * @return {?}
     */ = (_, element) => element;
  }
  // do not set true or false
  /**
   * @return {?}
   */
  get appInfo() {
    return this.store.selectSnapshot(ConfigState.getApplicationInfo);
  }
  /**
   * @return {?}
   */
  get visibleRoutes$() {
    return this.routes$.pipe(
      map(
        /**
         * @param {?} routes
         * @return {?}
         */
        routes => getVisibleRoutes(routes),
      ),
    );
  }
  /**
   * @return {?}
   */
  get defaultLanguage$() {
    return this.languages$.pipe(
      map(
        /**
         * @param {?} languages
         * @return {?}
         */
        languages =>
          snq(
            /**
             * @return {?}
             */
            () =>
              languages.find(
                /**
                 * @param {?} lang
                 * @return {?}
                 */
                lang => lang.cultureName === this.selectedLangCulture,
              ).displayName,
          ),
        '',
      ),
    );
  }
  /**
   * @return {?}
   */
  get dropdownLanguages$() {
    return this.languages$.pipe(
      map(
        /**
         * @param {?} languages
         * @return {?}
         */
        languages =>
          snq(
            /**
             * @return {?}
             */
            () =>
              languages.filter(
                /**
                 * @param {?} lang
                 * @return {?}
                 */
                lang => lang.cultureName !== this.selectedLangCulture,
              ),
          ),
        [],
      ),
    );
  }
  /**
   * @return {?}
   */
  get selectedLangCulture() {
    return this.store.selectSnapshot(SessionState.getLanguage);
  }
  /**
   * @private
   * @return {?}
   */
  checkWindowWidth() {
    setTimeout(
      /**
       * @return {?}
       */
      () => {
        if (window.innerWidth < 768) {
          this.isDropdownChildDynamic = false;
          if (this.smallScreen === false) {
            this.isCollapsed = false;
            setTimeout(
              /**
               * @return {?}
               */
              () => {
                this.isCollapsed = true;
              },
              100,
            );
          }
          this.smallScreen = true;
        } else {
          this.isDropdownChildDynamic = true;
          this.smallScreen = false;
        }
      },
      0,
    );
  }
  /**
   * @return {?}
   */
  ngAfterViewInit() {
    /** @type {?} */
    const navigations = this.store.selectSnapshot(LayoutState.getNavigationElements).map(
      /**
       * @param {?} __0
       * @return {?}
       */
      (({ name }) => name),
    );
    if (navigations.indexOf('LanguageRef') < 0) {
      this.store.dispatch(
        new AddNavigationElement([
          { element: this.languageRef, order: 4, name: 'LanguageRef' },
          { element: this.currentUserRef, order: 5, name: 'CurrentUserRef' },
        ]),
      );
    }
    this.navElements$
      .pipe(
        map(
          /**
           * @param {?} elements
           * @return {?}
           */
          elements =>
            elements.map(
              /**
               * @param {?} __0
               * @return {?}
               */
              ({ element }) => element,
            ),
        ),
        filter(
          /**
           * @param {?} elements
           * @return {?}
           */
          elements => !compare(elements, this.rightPartElements),
        ),
        takeUntilDestroy(this),
      )
      .subscribe(
        /**
         * @param {?} elements
         * @return {?}
         */
        elements => {
          setTimeout(
            /**
             * @return {?}
             */
            () => (this.rightPartElements = elements),
            0,
          );
        },
      );
    this.checkWindowWidth();
    fromEvent(window, 'resize')
      .pipe(
        takeUntilDestroy(this),
        debounceTime(150),
      )
      .subscribe(
        /**
         * @return {?}
         */
        () => {
          this.checkWindowWidth();
        },
      );
  }
  /**
   * @return {?}
   */
  ngOnDestroy() {}
  /**
   * @param {?} cultureName
   * @return {?}
   */
  onChangeLang(cultureName) {
    this.store.dispatch(new SetLanguage(cultureName));
  }
  /**
   * @return {?}
   */
  logout() {
    this.oauthService.logOut();
    this.store.dispatch(
      new Navigate(['/'], null, {
        state: { redirectUrl: this.store.selectSnapshot(RouterState).state.url },
      }),
    );
    this.store.dispatch(new GetAppConfiguration());
  }
  /**
   * @param {?} event
   * @param {?} childrenContainer
   * @return {?}
   */
  openChange(event, childrenContainer) {
    if (!event) {
      Object.keys(childrenContainer.style)
        .filter(
          /**
           * @param {?} key
           * @return {?}
           */
          key => Number.isInteger(+key),
        )
        .forEach(
          /**
           * @param {?} key
           * @return {?}
           */
          key => {
            this.renderer.removeStyle(childrenContainer, childrenContainer.style[key]);
          },
        );
      this.renderer.removeStyle(childrenContainer, 'left');
    }
  }
}
// required for dynamic component
ApplicationLayoutComponent.type = 'application' /* application */;
ApplicationLayoutComponent.decorators = [
  {
    type: Component,
    args: [
      {
        selector: 'abp-layout-application',
        template:
          '<nav\n  class="navbar navbar-expand-md navbar-dark bg-dark shadow-sm flex-column flex-md-row mb-4"\n  id="main-navbar"\n  style="min-height: 4rem;"\n>\n  <div class="container ">\n    <a class="navbar-brand" routerLink="/">\n      <img *ngIf="appInfo.logoUrl; else appName" [src]="appInfo.logoUrl" [alt]="appInfo.name" />\n    </a>\n    <button\n      class="navbar-toggler"\n      type="button"\n      [attr.aria-expanded]="!isCollapsed"\n      (click)="isCollapsed = !isCollapsed"\n    >\n      <span class="navbar-toggler-icon"></span>\n    </button>\n    <div class="navbar-collapse" id="main-navbar-collapse">\n      <ng-container *ngTemplateOutlet="!smallScreen ? navigations : null"></ng-container>\n\n      <div *ngIf="smallScreen" class="overflow-hidden" [@collapseWithMargin]="isCollapsed ? \'collapsed\' : \'expanded\'">\n        <ng-container *ngTemplateOutlet="navigations"></ng-container>\n      </div>\n\n      <ng-template #navigations>\n        <ul class="navbar-nav mx-auto">\n          <ng-container\n            *ngFor="let route of visibleRoutes$ | async; trackBy: trackByFn"\n            [ngTemplateOutlet]="route?.children?.length ? dropdownLink : defaultLink"\n            [ngTemplateOutletContext]="{ $implicit: route }"\n          >\n          </ng-container>\n\n          <ng-template #defaultLink let-route>\n            <li class="nav-item" [abpPermission]="route.requiredPolicy">\n              <a class="nav-link" [routerLink]="[route.url]">{{ route.name | abpLocalization }}</a>\n            </li>\n          </ng-template>\n\n          <ng-template #dropdownLink let-route>\n            <li\n              #navbarRootDropdown\n              [abpPermission]="route.requiredPolicy"\n              [abpVisibility]="routeContainer"\n              class="nav-item dropdown"\n              display="static"\n              (click)="\n                navbarRootDropdown.expand ? (navbarRootDropdown.expand = false) : (navbarRootDropdown.expand = true)\n              "\n            >\n              <a\n                class="nav-link dropdown-toggle"\n                data-toggle="dropdown"\n                aria-haspopup="true"\n                aria-expanded="false"\n                href="javascript:void(0)"\n              >\n                <i *ngIf="route.iconClass" [ngClass]="route.iconClass"></i> {{ route.name | abpLocalization }}\n              </a>\n              <div\n                #routeContainer\n                class="dropdown-menu border-0 shadow-sm"\n                [class.d-block]="smallScreen"\n                [class.overflow-hidden]="smallScreen"\n                (click)="$event.preventDefault(); $event.stopPropagation()"\n              >\n                <div\n                  class="abp-collapsed abp-main-nav-dropdown"\n                  [class.expanded]="smallScreen ? navbarRootDropdown.expand : true"\n                >\n                  <ng-template\n                    #forTemplate\n                    ngFor\n                    [ngForOf]="route.children"\n                    [ngForTrackBy]="trackByFn"\n                    [ngForTemplate]="childWrapper"\n                  ></ng-template>\n                </div>\n              </div>\n            </li>\n          </ng-template>\n\n          <ng-template #childWrapper let-child>\n            <ng-template\n              [ngTemplateOutlet]="child?.children?.length ? dropdownChild : defaultChild"\n              [ngTemplateOutletContext]="{ $implicit: child }"\n            ></ng-template>\n          </ng-template>\n\n          <ng-template #defaultChild let-child>\n            <div class="dropdown-submenu" [abpPermission]="child.requiredPolicy">\n              <a class="dropdown-item" [routerLink]="[child.url]">\n                <i *ngIf="child.iconClass" [ngClass]="child.iconClass"></i>\n                {{ child.name | abpLocalization }}</a\n              >\n            </div>\n          </ng-template>\n\n          <ng-template #dropdownChild let-child>\n            <div\n              [abpVisibility]="childrenContainer"\n              class="dropdown-submenu"\n              ngbDropdown\n              #dropdownSubmenu="ngbDropdown"\n              [display]="isDropdownChildDynamic ? \'dynamic\' : \'static\'"\n              placement="right-top"\n              [autoClose]="true"\n              [abpPermission]="child.requiredPolicy"\n              (openChange)="openChange($event, childrenContainer)"\n            >\n              <div ngbDropdownToggle [class.dropdown-toggle]="false">\n                <a\n                  abpEllipsis="210px"\n                  [abpEllipsisEnabled]="isDropdownChildDynamic"\n                  role="button"\n                  class="btn d-block text-left dropdown-toggle"\n                >\n                  <i *ngIf="child.iconClass" [ngClass]="child.iconClass"></i>\n                  {{ child.name | abpLocalization }}\n                </a>\n              </div>\n              <div\n                #childrenContainer\n                class="dropdown-menu border-0 shadow-sm"\n                [class.d-block]="smallScreen"\n                [class.overflow-hidden]="smallScreen"\n              >\n                <div\n                  class="abp-collapsed abp-main-nav-dropdown"\n                  [class.expanded]="smallScreen ? dropdownSubmenu.isOpen() : true"\n                >\n                  <ng-template\n                    ngFor\n                    [ngForOf]="child.children"\n                    [ngForTrackBy]="trackByFn"\n                    [ngForTemplate]="childWrapper"\n                  ></ng-template>\n                </div>\n              </div>\n            </div>\n          </ng-template>\n        </ul>\n\n        <ul class="navbar-nav">\n          <ng-container\n            *ngFor="let element of rightPartElements; trackBy: trackElementByFn"\n            [ngTemplateOutlet]="element"\n          ></ng-container>\n        </ul>\n      </ng-template>\n    </div>\n  </div>\n</nav>\n\n<div [@slideFromBottom]="outlet && outlet.activatedRoute && outlet.activatedRoute.routeConfig.path" class="container">\n  <router-outlet #outlet="outlet"></router-outlet>\n</div>\n\n<abp-confirmation></abp-confirmation>\n<abp-toast></abp-toast>\n\n<ng-template #appName>\n  {{ appInfo.name }}\n</ng-template>\n\n<ng-template #language>\n  <li class="nav-item">\n    <div class="dropdown" ngbDropdown #languageDropdown="ngbDropdown" display="static">\n      <a\n        ngbDropdownToggle\n        class="nav-link"\n        href="javascript:void(0)"\n        role="button"\n        id="dropdownMenuLink"\n        data-toggle="dropdown"\n        aria-haspopup="true"\n        aria-expanded="false"\n      >\n        {{ defaultLanguage$ | async }}\n      </a>\n      <div\n        class="dropdown-menu dropdown-menu-right border-0 shadow-sm"\n        aria-labelledby="dropdownMenuLink"\n        [class.d-block]="smallScreen"\n        [class.overflow-hidden]="smallScreen"\n      >\n        <div\n          class="abp-collapsed abp-main-nav-dropdown"\n          [class.expanded]="smallScreen ? languageDropdown.isOpen() : true"\n        >\n          <a\n            *ngFor="let lang of dropdownLanguages$ | async"\n            href="javascript:void(0)"\n            class="dropdown-item"\n            (click)="onChangeLang(lang.cultureName)"\n            >{{ lang?.displayName }}</a\n          >\n        </div>\n      </div>\n    </div>\n  </li>\n</ng-template>\n\n<ng-template #currentUser>\n  <li *ngIf="(currentUser$ | async)?.isAuthenticated" class="nav-item">\n    <div ngbDropdown class="dropdown" #currentUserDropdown="ngbDropdown" display="static">\n      <a\n        ngbDropdownToggle\n        class="nav-link"\n        href="javascript:void(0)"\n        role="button"\n        id="dropdownMenuLink"\n        data-toggle="dropdown"\n        aria-haspopup="true"\n        aria-expanded="false"\n      >\n        {{ (currentUser$ | async)?.userName }}\n      </a>\n      <div\n        class="dropdown-menu dropdown-menu-right border-0 shadow-sm"\n        aria-labelledby="dropdownMenuLink"\n        [class.overflow-hidden]="smallScreen"\n        [class.d-block]="smallScreen"\n      >\n        <div\n          class="abp-collapsed abp-main-nav-dropdown"\n          [class.expanded]="smallScreen ? currentUserDropdown.isOpen() : true"\n        >\n          <a class="dropdown-item" routerLink="/account/manage-profile">{{\n            \'AbpAccount::ManageYourProfile\' | abpLocalization\n          }}</a>\n          <a class="dropdown-item" href="javascript:void(0)" (click)="logout()">{{\n            \'AbpUi::Logout\' | abpLocalization\n          }}</a>\n        </div>\n      </div>\n    </div>\n  </li>\n</ng-template>\n',
        animations: [slideFromBottom, collapseWithMargin],
      },
    ],
  },
];
/** @nocollapse */
ApplicationLayoutComponent.ctorParameters = () => [{ type: Store }, { type: OAuthService }, { type: Renderer2 }];
ApplicationLayoutComponent.propDecorators = {
  currentUserRef: [{ type: ViewChild, args: ['currentUser', { static: false, read: TemplateRef }] }],
  languageRef: [{ type: ViewChild, args: ['language', { static: false, read: TemplateRef }] }],
};
tslib_1.__decorate(
  [Select(ConfigState.getOne('routes')), tslib_1.__metadata('design:type', Observable)],
  ApplicationLayoutComponent.prototype,
  'routes$',
  void 0,
);
tslib_1.__decorate(
  [Select(ConfigState.getOne('currentUser')), tslib_1.__metadata('design:type', Observable)],
  ApplicationLayoutComponent.prototype,
  'currentUser$',
  void 0,
);
tslib_1.__decorate(
  [Select(ConfigState.getDeep('localization.languages')), tslib_1.__metadata('design:type', Observable)],
  ApplicationLayoutComponent.prototype,
  'languages$',
  void 0,
);
tslib_1.__decorate(
  [Select(LayoutState.getNavigationElements), tslib_1.__metadata('design:type', Observable)],
  ApplicationLayoutComponent.prototype,
  'navElements$',
  void 0,
);
if (false) {
  /** @type {?} */
  ApplicationLayoutComponent.type;
  /** @type {?} */
  ApplicationLayoutComponent.prototype.routes$;
  /** @type {?} */
  ApplicationLayoutComponent.prototype.currentUser$;
  /** @type {?} */
  ApplicationLayoutComponent.prototype.languages$;
  /** @type {?} */
  ApplicationLayoutComponent.prototype.navElements$;
  /** @type {?} */
  ApplicationLayoutComponent.prototype.currentUserRef;
  /** @type {?} */
  ApplicationLayoutComponent.prototype.languageRef;
  /** @type {?} */
  ApplicationLayoutComponent.prototype.isDropdownChildDynamic;
  /** @type {?} */
  ApplicationLayoutComponent.prototype.isCollapsed;
  /** @type {?} */
  ApplicationLayoutComponent.prototype.smallScreen;
  /** @type {?} */
  ApplicationLayoutComponent.prototype.rightPartElements;
  /** @type {?} */
  ApplicationLayoutComponent.prototype.trackByFn;
  /** @type {?} */
  ApplicationLayoutComponent.prototype.trackElementByFn;
  /**
   * @type {?}
   * @private
   */
  ApplicationLayoutComponent.prototype.store;
  /**
   * @type {?}
   * @private
   */
  ApplicationLayoutComponent.prototype.oauthService;
  /**
   * @type {?}
   * @private
   */
  ApplicationLayoutComponent.prototype.renderer;
}
/**
 * @param {?} routes
 * @return {?}
 */
function getVisibleRoutes(routes) {
  return routes.reduce(
    /**
     * @param {?} acc
     * @param {?} val
     * @return {?}
     */
    (acc, val) => {
      if (val.invisible) return acc;
      if (val.children && val.children.length) {
        val.children = getVisibleRoutes(val.children);
      }
      return [...acc, val];
    },
    [],
  );
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9hcHBsaWNhdGlvbi1sYXlvdXQvYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFJTCxXQUFXLEVBRVgsbUJBQW1CLEVBQ25CLFlBQVksRUFDWixXQUFXLEVBQ1gsZ0JBQWdCLEdBQ2pCLE1BQU0sY0FBYyxDQUFDO0FBQ3RCLE9BQU8sRUFBRSxrQkFBa0IsRUFBRSxlQUFlLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUMzRSxPQUFPLEVBRUwsU0FBUyxFQUdULFNBQVMsRUFDVCxXQUFXLEVBRVgsU0FBUyxHQUdWLE1BQU0sZUFBZSxDQUFDO0FBRXZCLE9BQU8sRUFBRSxRQUFRLEVBQUUsV0FBVyxFQUFFLE1BQU0scUJBQXFCLENBQUM7QUFDNUQsT0FBTyxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDNUMsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQ25ELE9BQU8sT0FBTyxNQUFNLGNBQWMsQ0FBQztBQUNuQyxPQUFPLEVBQUUsU0FBUyxFQUFFLFVBQVUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUM3QyxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0sRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUMzRCxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFDdEIsT0FBTyxFQUFFLG9CQUFvQixFQUFFLE1BQU0sZUFBZSxDQUFDO0FBRXJELE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFPM0MsTUFBTSxPQUFPLDBCQUEwQjs7Ozs7O0lBNkRyQyxZQUFvQixLQUFZLEVBQVUsWUFBMEIsRUFBVSxRQUFtQjtRQUE3RSxVQUFL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QyxTQUFTO1lBQUUsT0FBTyxHQUFHLENBQUM7UUFFOUIsSUFBSSxHQUFHLENBQUMsUUFBUSxJQUFJLEdBQUcsQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO1lBQ3ZDLEdBQUcsQ0FBQyxRQUFRLEdBQUcsZ0JBQWdCLENBQUMsR0FBRyxDQUFDLFFBQVEsQ0FBQyxDQUFDO1NBQy9DO1FBRUQsT0FBTyxDQUFDLEdBQUcsR0FBRyxFQUFFLEdBQUcsQ0FBQyxDQUFDO0lBQ3ZCLENBQUMsR0FBRSxFQUFFLENBQUMsQ0FBQztBQUNULENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQge1xuICBBQlAsXG4gIEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbixcbiAgQ29uZmlnLFxuICBDb25maWdTdGF0ZSxcbiAgZUxheW91dFR5cGUsXG4gIEdldEFwcENvbmZpZ3VyYXRpb24sXG4gIFNlc3Npb25TdGF0ZSxcbiAgU2V0TGFuZ3VhZ2UsXG4gIHRha2VVbnRpbERlc3Ryb3ksXG59IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBjb2xsYXBzZVdpdGhNYXJnaW4sIHNsaWRlRnJvbUJvdHRvbSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7XG4gIEFmdGVyVmlld0luaXQsXG4gIENvbXBvbmVudCxcbiAgT25EZXN0cm95LFxuICBRdWVyeUxpc3QsXG4gIFJlbmRlcmVyMixcbiAgVGVtcGxhdGVSZWYsXG4gIFRyYWNrQnlGdW5jdGlvbixcbiAgVmlld0NoaWxkLFxuICBWaWV3Q2hpbGRyZW4sXG4gIEVsZW1lbnRSZWYsXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgTmdiRHJvcGRvd24gfSBmcm9tICdAbmctYm9vdHN0cmFwL25nLWJvb3RzdHJhcCc7XG5pbXBvcnQgeyBOYXZpZ2F0ZSwgUm91dGVyU3RhdGUgfSBmcm9tICdAbmd4cy9yb3V0ZXItcGx1Z2luJztcbmltcG9ydCB7IFNlbGVjdCwgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBPQXV0aFNlcnZpY2UgfSBmcm9tICdhbmd1bGFyLW9hdXRoMi1vaWRjJztcbmltcG9ydCBjb21wYXJlIGZyb20gJ2p1c3QtY29tcGFyZSc7XG5pbXBvcnQgeyBmcm9tRXZlbnQsIE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGRlYm91bmNlVGltZSwgZmlsdGVyLCBtYXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5pbXBvcnQgeyBBZGROYXZpZ2F0aW9uRWxlbWVudCB9IGZyb20gJy4uLy4uL2FjdGlvbnMnO1xuaW1wb3J0IHsgTGF5b3V0IH0gZnJvbSAnLi4vLi4vbW9kZWxzL2xheW91dCc7XG5pbXBvcnQgeyBMYXlvdXRTdGF0ZSB9IGZyb20gJy4uLy4uL3N0YXRlcyc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1sYXlvdXQtYXBwbGljYXRpb24nLFxuICB0ZW1wbGF0ZVVybDogJy4vYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC5odG1sJyxcbiAgYW5pbWF0aW9uczogW3NsaWRlRnJvbUJvdHRvbSwgY29sbGFwc2VXaXRoTWFyZ2luXSxcbn0pXG5leHBvcnQgY2xhc3MgQXBwbGljYXRpb25MYXlvdXRDb21wb25lbnQgaW1wbGVtZW50cyBBZnRlclZpZXdJbml0LCBPbkRlc3Ryb3kge1xuICAvLyByZXF1aXJlZCBmb3IgZHluYW1pYyBjb21wb25lbnRcbiAgc3RhdGljIHR5cGUgPSBlTGF5b3V0VHlwZS5hcHBsaWNhdGlvbjtcblxuICBAU2VsZWN0KENvbmZpZ1N0YXRlLmdldE9uZSgncm91dGVzJykpXG4gIHJvdXRlcyQ6IE9ic2VydmFibGU8QUJQLkZ1bGxSb3V0ZVtdPjtcblxuICBAU2VsZWN0KENvbmZpZ1N0YXRlLmdldE9uZSgnY3VycmVudFVzZXInKSlcbiAgY3VycmVudFVzZXIkOiBPYnNlcnZhYmxlPEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbi5DdXJyZW50VXNlcj47XG5cbiAgQFNlbGVjdChDb25maWdTdGF0ZS5nZXREZWVwKCdsb2NhbGl6YXRpb24ubGFuZ3VhZ2VzJykpXG4gIGxhbmd1YWdlcyQ6IE9ic2VydmFibGU8QXBwbGljYXRpb25Db25maWd1cmF0aW9uLkxhbmd1YWdlW10+O1xuXG4gIEBTZWxlY3QoTGF5b3V0U3RhdGUuZ2V0TmF2aWdhdGlvbkVsZW1lbnRzKVxuICBuYXZFbGVtZW50cyQ6IE9ic2VydmFibGU8TGF5b3V0Lk5hdmlnYXRpb25FbGVtZW50W10+O1xuXG4gIEBWaWV3Q2hpbGQoJ2N1cnJlbnRVc2VyJywgeyBzdGF0aWM6IGZhbHNlLCByZWFkOiBUZW1wbGF0ZVJlZiB9KVxuICBjdXJyZW50VXNlclJlZjogVGVtcGxhdGVSZWY8YW55PjtcblxuICBAVmlld0NoaWxkKCdsYW5ndWFnZScsIHsgc3RhdGljOiBmYWxzZSwgcmVhZDogVGVtcGxhdGVSZWYgfSlcbiAgbGFuZ3VhZ2VSZWY6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgaXNEcm9wZG93bkNoaWxkRHluYW1pYzogYm9vbGVhbjtcblxuICBpc0NvbGxhcHNlZCA9IHRydWU7XG5cbiAgc21hbGxTY3JlZW46IGJvb2xlYW47IC8vIGRvIG5vdCBzZXQgdHJ1ZSBvciBmYWxzZVxuXG4gIGdldCBhcHBJbmZvKCk6IENvbmZpZy5BcHBsaWNhdGlvbiB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0QXBwbGljYXRpb25JbmZvKTtcbiAgfVxuXG4gIGdldCB2aXNpYmxlUm91dGVzJCgpOiBPYnNlcnZhYmxlPEFCUC5GdWxsUm91dGVbXT4ge1xuICAgIHJldHVybiB0aGlzLnJvdXRlcyQucGlwZShtYXAocm91dGVzID0+IGdldFZpc2libGVSb3V0ZXMocm91dGVzKSkpO1xuICB9XG5cbiAgZ2V0IGRlZmF1bHRMYW5ndWFnZSQoKTogT2JzZXJ2YWJsZTxzdHJpbmc+IHtcbiAgICByZXR1cm4gdGhpcy5sYW5ndWFnZXMkLnBpcGUoXG4gICAgICBtYXAoXG4gICAgICAgIGxhbmd1YWdlcyA9PiBzbnEoKCkgPT4gbGFuZ3VhZ2VzLmZpbmQobGFuZyA9PiBsYW5nLmN1bHR1cmVOYW1lID09PSB0aGlzLnNlbGVjdGVkTGFuZ0N1bHR1cmUpLmRpc3BsYXlOYW1lKSxcbiAgICAgICAgJycsXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBnZXQgZHJvcGRvd25MYW5ndWFnZXMkKCk6IE9ic2VydmFibGU8QXBwbGljYXRpb25Db25maWd1cmF0aW9uLkxhbmd1YWdlW10+IHtcbiAgICByZXR1cm4gdGhpcy5sYW5ndWFnZXMkLnBpcGUoXG4gICAgICBtYXAobGFuZ3VhZ2VzID0+IHNucSgoKSA9PiBsYW5ndWFnZXMuZmlsdGVyKGxhbmcgPT4gbGFuZy5jdWx0dXJlTmFtZSAhPT0gdGhpcy5zZWxlY3RlZExhbmdDdWx0dXJlKSksIFtdKSxcbiAgICApO1xuICB9XG5cbiAgZ2V0IHNlbGVjdGVkTGFuZ0N1bHR1cmUoKTogc3RyaW5nIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChTZXNzaW9uU3RhdGUuZ2V0TGFuZ3VhZ2UpO1xuICB9XG5cbiAgcmlnaHRQYXJ0RWxlbWVudHM6IFRlbXBsYXRlUmVmPGFueT5bXSA9IFtdO1xuXG4gIHRyYWNrQnlGbjogVHJhY2tCeUZ1bmN0aW9uPEFCUC5GdWxsUm91dGU+ID0gKF8sIGl0ZW0pID0+IGl0ZW0ubmFtZTtcblxuICB0cmFja0VsZW1lbnRCeUZuOiBUcmFja0J5RnVuY3Rpb248QUJQLkZ1bGxSb3V0ZT4gPSAoXywgZWxlbWVudCkgPT4gZWxlbWVudDtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSwgcHJpdmF0ZSBvYXV0aFNlcnZpY2U6IE9BdXRoU2VydmljZSwgcHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyKSB7fVxuXG4gIHByaXZhdGUgY2hlY2tXaW5kb3dXaWR0aCgpIHtcbiAgICBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgIGlmICh3aW5kb3cuaW5uZXJXaWR0aCA8IDc2OCkge1xuICAgICAgICB0aGlzLmlzRHJvcGRvd25DaGlsZER5bmFtaWMgPSBmYWxzZTtcbiAgICAgICAgaWYgKHRoaXMuc21hbGxTY3JlZW4gPT09IGZhbHNlKSB7XG4gICAgICAgICAgdGhpcy5pc0NvbGxhcHNlZCA9IGZhbHNlO1xuICAgICAgICAgIHNldFRpbWVvdXQoKCkgPT4ge1xuICAgICAgICAgICAgdGhpcy5pc0NvbGxhcHNlZCA9IHRydWU7XG4gICAgICAgICAgfSwgMTAwKTtcbiAgICAgICAgfVxuICAgICAgICB0aGlzLnNtYWxsU2NyZWVuID0gdHJ1ZTtcbiAgICAgIH0gZWxzZSB7XG4gICAgICAgIHRoaXMuaXNEcm9wZG93bkNoaWxkRHluYW1pYyA9IHRydWU7XG4gICAgICAgIHRoaXMuc21hbGxTY3JlZW4gPSBmYWxzZTtcbiAgICAgIH1cbiAgICB9LCAwKTtcbiAgfVxuXG4gIG5nQWZ0ZXJWaWV3SW5pdCgpIHtcbiAgICBjb25zdCBuYXZpZ2F0aW9ucyA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoTGF5b3V0U3RhdGUuZ2V0TmF2aWdhdGlvbkVsZW1lbnRzKS5tYXAoKHsgbmFtZSB9KSA9PiBuYW1lKTtcblxuICAgIGlmIChuYXZpZ2F0aW9ucy5pbmRleE9mKCdMYW5ndWFnZVJlZicpIDwgMCkge1xuICAgICAgdGhpcy5zdG9yZS5kaXNwYXRjaChcbiAgICAgICAgbmV3IEFkZE5hdmlnYXRpb25FbGVtZW50KFtcbiAgICAgICAgICB7IGVsZW1lbnQ6IHRoaXMubGFuZ3VhZ2VSZWYsIG9yZGVyOiA0LCBuYW1lOiAnTGFuZ3VhZ2VSZWYnIH0sXG4gICAgICAgICAgeyBlbGVtZW50OiB0aGlzLmN1cnJlbnRVc2VyUmVmLCBvcmRlcjogNSwgbmFtZTogJ0N1cnJlbnRVc2VyUmVmJyB9LFxuICAgICAgICBdKSxcbiAgICAgICk7XG4gICAgfVxuXG4gICAgdGhpcy5uYXZFbGVtZW50cyRcbiAgICAgIC5waXBlKFxuICAgICAgICBtYXAoZWxlbWVudHMgPT4gZWxlbWVudHMubWFwKCh7IGVsZW1lbnQgfSkgPT4gZWxlbWVudCkpLFxuICAgICAgICBmaWx0ZXIoZWxlbWVudHMgPT4gIWNvbXBhcmUoZWxlbWVudHMsIHRoaXMucmlnaHRQYXJ0RWxlbWVudHMpKSxcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoZWxlbWVudHMgPT4ge1xuICAgICAgICBzZXRUaW1lb3V0KCgpID0+ICh0aGlzLnJpZ2h0UGFydEVsZW1lbnRzID0gZWxlbWVudHMpLCAwKTtcbiAgICAgIH0pO1xuXG4gICAgdGhpcy5jaGVja1dpbmRvd1dpZHRoKCk7XG5cbiAgICBmcm9tRXZlbnQod2luZG93LCAncmVzaXplJylcbiAgICAgIC5waXBlKFxuICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpLFxuICAgICAgICBkZWJvdW5jZVRpbWUoMTUwKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICB0aGlzLmNoZWNrV2luZG93V2lkdGgoKTtcbiAgICAgIH0pO1xuICB9XG5cbiAgbmdPbkRlc3Ryb3koKSB7fVxuXG4gIG9uQ2hhbmdlTGFuZyhjdWx0dXJlTmFtZTogc3RyaW5nKSB7XG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgU2V0TGFuZ3VhZ2UoY3VsdHVyZU5hbWUpKTtcbiAgfVxuXG4gIGxvZ291dCgpIHtcbiAgICB0aGlzLm9hdXRoU2VydmljZS5sb2dPdXQoKTtcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKFxuICAgICAgbmV3IE5hdmlnYXRlKFsnLyddLCBudWxsLCB7XG4gICAgICAgIHN0YXRlOiB7IHJlZGlyZWN0VXJsOiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFJvdXRlclN0YXRlKS5zdGF0ZS51cmwgfSxcbiAgICAgIH0pLFxuICAgICk7XG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgR2V0QXBwQ29uZmlndXJhdGlvbigpKTtcbiAgfVxuXG4gIG9wZW5DaGFuZ2UoZXZlbnQ6IGJvb2xlYW4sIGNoaWxkcmVuQ29udGFpbmVyOiBIVE1MRGl2RWxlbWVudCkge1xuICAgIGlmICghZXZlbnQpIHtcbiAgICAgIE9iamVjdC5rZXlzKGNoaWxkcmVuQ29udGFpbmVyLnN0eWxlKVxuICAgICAgICAuZmlsdGVyKGtleSA9PiBOdW1iZXIuaXNJbnRlZ2VyKCtrZXkpKVxuICAgICAgICAuZm9yRWFjaChrZXkgPT4ge1xuICAgICAgICAgIHRoaXMucmVuZGVyZXIucmVtb3ZlU3R5bGUoY2hpbGRyZW5Db250YWluZXIsIGNoaWxkcmVuQ29udGFpbmVyLnN0eWxlW2tleV0pO1xuICAgICAgICB9KTtcbiAgICAgIHRoaXMucmVuZGVyZXIucmVtb3ZlU3R5bGUoY2hpbGRyZW5Db250YWluZXIsICdsZWZ0Jyk7XG4gICAgfVxuICB9XG59XG5cbmZ1bmN0aW9uIGdldFZpc2libGVSb3V0ZXMocm91dGVzOiBBQlAuRnVsbFJvdXRlW10pIHtcbiAgcmV0dXJuIHJvdXRlcy5yZWR1Y2UoKGFjYywgdmFsKSA9PiB7XG4gICAgaWYgKHZhbC5pbnZpc2libGUpIHJldHVybiBhY2M7XG5cbiAgICBpZiAodmFsLmNoaWxkcmVuICYmIHZhbC5jaGlsZHJlbi5sZW5ndGgpIHtcbiAgICAgIHZhbC5jaGlsZHJlbiA9IGdldFZpc2libGVSb3V0ZXModmFsLmNoaWxkcmVuKTtcbiAgICB9XG5cbiAgICByZXR1cm4gWy4uLmFjYywgdmFsXTtcbiAgfSwgW10pO1xufVxuIl19
