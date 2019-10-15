/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import { GetAppConfiguration, ConfigState, SetLanguage, SessionState, takeUntilDestroy } from '@abp/ng.core';
import { Component, QueryList, TemplateRef, ViewChild, ViewChildren } from '@angular/core';
import { NgbDropdown } from '@ng-bootstrap/ng-bootstrap';
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
   */
  constructor(store, oauthService) {
    this.store = store;
    this.oauthService = oauthService;
    this.isOpenChangePassword = false;
    this.isOpenProfile = false;
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
        this.navbarRootDropdowns.forEach(
          /**
           * @param {?} item
           * @return {?}
           */
          item => {
            item.close();
          },
        );
        if (window.innerWidth < 768) {
          this.isDropdownChildDynamic = false;
        } else {
          this.isDropdownChildDynamic = true;
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
        debounceTime(250),
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
          '<abp-layout>\n  <ul class="navbar-nav mr-auto">\n    <ng-container\n      *ngFor="let route of visibleRoutes$ | async; trackBy: trackByFn"\n      [ngTemplateOutlet]="route?.children?.length ? dropdownLink : defaultLink"\n      [ngTemplateOutletContext]="{ $implicit: route }"\n    >\n    </ng-container>\n\n    <ng-template #defaultLink let-route>\n      <li class="nav-item" [abpPermission]="route.requiredPolicy">\n        <a class="nav-link" [routerLink]="[route.url]">{{ route.name | abpLocalization }}</a>\n      </li>\n    </ng-template>\n\n    <ng-template #dropdownLink let-route>\n      <li\n        #navbarRootDropdown\n        ngbDropdown\n        [abpPermission]="route.requiredPolicy"\n        [abpVisibility]="routeContainer"\n        class="nav-item dropdown pointer"\n        display="static"\n      >\n        <a ngbDropdownToggle class="nav-link dropdown-toggle pointer" data-toggle="dropdown">\n          {{ route.name | abpLocalization }}\n        </a>\n        <div #routeContainer ngbDropdownMenu class="dropdown-menu dropdown-menu-right">\n          <ng-template\n            #forTemplate\n            ngFor\n            [ngForOf]="route.children"\n            [ngForTrackBy]="trackByFn"\n            [ngForTemplate]="childWrapper"\n          ></ng-template>\n        </div>\n      </li>\n    </ng-template>\n\n    <ng-template #childWrapper let-child>\n      <ng-template\n        [ngTemplateOutlet]="child?.children?.length ? dropdownChild : defaultChild"\n        [ngTemplateOutletContext]="{ $implicit: child }"\n      ></ng-template>\n    </ng-template>\n\n    <ng-template #defaultChild let-child>\n      <div class="dropdown-submenu" [abpPermission]="child.requiredPolicy">\n        <a class="dropdown-item py-2 px-2" [routerLink]="[child.url]">\n          <i *ngIf="child.iconClass" [ngClass]="child.iconClass"></i>\n          {{ child.name | abpLocalization }}</a\n        >\n      </div>\n    </ng-template>\n\n    <ng-template #dropdownChild let-child>\n      <div\n        [abpVisibility]="childrenContainer"\n        class="dropdown-submenu pointer"\n        ngbDropdown\n        [display]="isDropdownChildDynamic ? \'dynamic\' : \'static\'"\n        placement="right-top"\n        [abpPermission]="child.requiredPolicy"\n      >\n        <div ngbDropdownToggle [class.dropdown-toggle]="false" class="pointer">\n          <a\n            abpEllipsis="210px"\n            [abpEllipsisEnabled]="isDropdownChildDynamic"\n            role="button"\n            class="btn d-block text-left py-2 px-2 dropdown-toggle"\n          >\n            <i *ngIf="child.iconClass" [ngClass]="child.iconClass"></i>\n            {{ child.name | abpLocalization }}\n          </a>\n        </div>\n        <div #childrenContainer ngbDropdownMenu class="dropdown-menu dropdown-menu-right">\n          <ng-template\n            ngFor\n            [ngForOf]="child.children"\n            [ngForTrackBy]="trackByFn"\n            [ngForTemplate]="childWrapper"\n          ></ng-template>\n        </div>\n      </div>\n    </ng-template>\n  </ul>\n\n  <ul class="navbar-nav ml-auto">\n    <ng-container\n      *ngFor="let element of rightPartElements; trackBy: trackElementByFn"\n      [ngTemplateOutlet]="element"\n    ></ng-container>\n  </ul>\n</abp-layout>\n\n<ng-template #language>\n  <li class="nav-item dropdown pointer" ngbDropdown>\n    <a ngbDropdownToggle class="nav-link dropdown-toggle text-white pointer" data-toggle="dropdown">\n      {{ defaultLanguage$ | async }}\n    </a>\n    <div ngbDropdownMenu class="dropdown-menu dropdown-menu-right">\n      <a\n        *ngFor="let lang of dropdownLanguages$ | async"\n        class="dropdown-item"\n        (click)="onChangeLang(lang.cultureName)"\n        >{{ lang?.displayName }}</a\n      >\n    </div>\n  </li>\n</ng-template>\n\n<ng-template #currentUser>\n  <li *ngIf="(currentUser$ | async)?.isAuthenticated" class="nav-item dropdown pointer" ngbDropdown>\n    <a ngbDropdownToggle class="nav-link dropdown-toggle text-white pointer" data-toggle="dropdown">\n      {{ (currentUser$ | async)?.userName }}\n    </a>\n    <div ngbDropdownMenu class="dropdown-menu dropdown-menu-right">\n      <a class="dropdown-item pointer" (click)="isOpenChangePassword = true">{{\n        \'AbpUi::ChangePassword\' | abpLocalization\n      }}</a>\n      <a class="dropdown-item pointer" (click)="isOpenProfile = true">{{ \'AbpUi::PersonalInfo\' | abpLocalization }}</a>\n      <a class="dropdown-item pointer" (click)="logout()">{{ \'AbpUi::Logout\' | abpLocalization }}</a>\n    </div>\n  </li>\n\n  <abp-change-password [(visible)]="isOpenChangePassword"></abp-change-password>\n\n  <abp-profile [(visible)]="isOpenProfile"></abp-profile>\n</ng-template>\n',
      },
    ],
  },
];
/** @nocollapse */
ApplicationLayoutComponent.ctorParameters = () => [{ type: Store }, { type: OAuthService }];
ApplicationLayoutComponent.propDecorators = {
  currentUserRef: [{ type: ViewChild, args: ['currentUser', { static: false, read: TemplateRef }] }],
  languageRef: [{ type: ViewChild, args: ['language', { static: false, read: TemplateRef }] }],
  navbarRootDropdowns: [{ type: ViewChildren, args: ['navbarRootDropdown', { read: NgbDropdown }] }],
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
  ApplicationLayoutComponent.prototype.navbarRootDropdowns;
  /** @type {?} */
  ApplicationLayoutComponent.prototype.isOpenChangePassword;
  /** @type {?} */
  ApplicationLayoutComponent.prototype.isOpenProfile;
  /** @type {?} */
  ApplicationLayoutComponent.prototype.isDropdownChildDynamic;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9hcHBsaWNhdGlvbi1sYXlvdXQvYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFHTCxtQkFBbUIsRUFDbkIsV0FBVyxFQUVYLFdBQVcsRUFDWCxZQUFZLEVBQ1osZ0JBQWdCLEVBQ2pCLE1BQU0sY0FBYyxDQUFDO0FBQ3RCLE9BQU8sRUFFTCxTQUFTLEVBRVQsU0FBUyxFQUNULFdBQVcsRUFFWCxTQUFTLEVBQ1QsWUFBWSxFQUNiLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUN6RCxPQUFPLEVBQUUsUUFBUSxFQUFFLFdBQVcsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQzVELE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUNuRCxPQUFPLE9BQU8sTUFBTSxjQUFjLENBQUM7QUFDbkMsT0FBTyxFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDN0MsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDM0QsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBQ3RCLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUVyRCxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBTTNDLE1BQU0sT0FBTywwQkFBMEI7Ozs7O0lBeURyQyxZQUFvQixLQUFZLEVBQVUsWUFBMEI7UUFBaEQsVUFBSyxHQUFMLEtBQUssQ0FB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QTZCOztJQUU3QixtREFBc0I7O0lBRXRCLDREQUFnQzs7SUFzQmhDLHVEQUEyQzs7SUFFM0MsK0NBQW1FOztJQUVuRSxzREFBMkU7Ozs7O0lBRS9ELDJDQUFvQjs7Ozs7SUFBRSxrREFBa0M7Ozs7OztBQWtFdEUsU0FBUyxnQkFBZ0IsQ0FBQyxNQUF1QjtJQUMvQyxPQUFPLE1BQU0sQ0FBQyxNQUFNOzs7OztJQUFDLENBQUMsR0FBRyxFQUFFLEdBQUcsRUFBRSxFQUFFO1FBQ2hDLElBQUksR0FBRyxDQUFDLFNBQVM7WUFBRSxPQUFPLEdBQUcsQ0FBQztRQUU5QixJQUFJLEdBQUcsQ0FBQyxRQUFRLElBQUksR0FBRyxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDdkMsR0FBRyxDQUFDLFFBQVEsR0FBRyxnQkFBZ0IsQ0FBQyxHQUFHLENBQUMsUUFBUSxDQUFDLENBQUM7U0FDL0M7UUFFRCxPQUFPLENBQUMsR0FBRyxHQUFHLEVBQUUsR0FBRyxDQUFDLENBQUM7SUFDdkIsQ0FBQyxHQUFFLEVBQUUsQ0FBQyxDQUFDO0FBQ1QsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7XG4gIEFCUCxcbiAgQXBwbGljYXRpb25Db25maWd1cmF0aW9uLFxuICBHZXRBcHBDb25maWd1cmF0aW9uLFxuICBDb25maWdTdGF0ZSxcbiAgZUxheW91dFR5cGUsXG4gIFNldExhbmd1YWdlLFxuICBTZXNzaW9uU3RhdGUsXG4gIHRha2VVbnRpbERlc3Ryb3lcbn0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7XG4gIEFmdGVyVmlld0luaXQsXG4gIENvbXBvbmVudCxcbiAgT25EZXN0cm95LFxuICBRdWVyeUxpc3QsXG4gIFRlbXBsYXRlUmVmLFxuICBUcmFja0J5RnVuY3Rpb24sXG4gIFZpZXdDaGlsZCxcbiAgVmlld0NoaWxkcmVuXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgTmdiRHJvcGRvd24gfSBmcm9tICdAbmctYm9vdHN0cmFwL25nLWJvb3RzdHJhcCc7XG5pbXBvcnQgeyBOYXZpZ2F0ZSwgUm91dGVyU3RhdGUgfSBmcm9tICdAbmd4cy9yb3V0ZXItcGx1Z2luJztcbmltcG9ydCB7IFNlbGVjdCwgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBPQXV0aFNlcnZpY2UgfSBmcm9tICdhbmd1bGFyLW9hdXRoMi1vaWRjJztcbmltcG9ydCBjb21wYXJlIGZyb20gJ2p1c3QtY29tcGFyZSc7XG5pbXBvcnQgeyBmcm9tRXZlbnQsIE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGRlYm91bmNlVGltZSwgZmlsdGVyLCBtYXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5pbXBvcnQgeyBBZGROYXZpZ2F0aW9uRWxlbWVudCB9IGZyb20gJy4uLy4uL2FjdGlvbnMnO1xuaW1wb3J0IHsgTGF5b3V0IH0gZnJvbSAnLi4vLi4vbW9kZWxzL2xheW91dCc7XG5pbXBvcnQgeyBMYXlvdXRTdGF0ZSB9IGZyb20gJy4uLy4uL3N0YXRlcyc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1sYXlvdXQtYXBwbGljYXRpb24nLFxuICB0ZW1wbGF0ZVVybDogJy4vYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC5odG1sJ1xufSlcbmV4cG9ydCBjbGFzcyBBcHBsaWNhdGlvbkxheW91dENvbXBvbmVudCBpbXBsZW1lbnRzIEFmdGVyVmlld0luaXQsIE9uRGVzdHJveSB7XG4gIC8vIHJlcXVpcmVkIGZvciBkeW5hbWljIGNvbXBvbmVudFxuICBzdGF0aWMgdHlwZSA9IGVMYXlvdXRUeXBlLmFwcGxpY2F0aW9uO1xuXG4gIEBTZWxlY3QoQ29uZmlnU3RhdGUuZ2V0T25lKCdyb3V0ZXMnKSlcbiAgcm91dGVzJDogT2JzZXJ2YWJsZTxBQlAuRnVsbFJvdXRlW10+O1xuXG4gIEBTZWxlY3QoQ29uZmlnU3RhdGUuZ2V0T25lKCdjdXJyZW50VXNlcicpKVxuICBjdXJyZW50VXNlciQ6IE9ic2VydmFibGU8QXBwbGljYXRpb25Db25maWd1cmF0aW9uLkN1cnJlbnRVc2VyPjtcblxuICBAU2VsZWN0KENvbmZpZ1N0YXRlLmdldERlZXAoJ2xvY2FsaXphdGlvbi5sYW5ndWFnZXMnKSlcbiAgbGFuZ3VhZ2VzJDogT2JzZXJ2YWJsZTxBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24uTGFuZ3VhZ2VbXT47XG5cbiAgQFNlbGVjdChMYXlvdXRTdGF0ZS5nZXROYXZpZ2F0aW9uRWxlbWVudHMpXG4gIG5hdkVsZW1lbnRzJDogT2JzZXJ2YWJsZTxMYXlvdXQuTmF2aWdhdGlvbkVsZW1lbnRbXT47XG5cbiAgQFZpZXdDaGlsZCgnY3VycmVudFVzZXInLCB7IHN0YXRpYzogZmFsc2UsIHJlYWQ6IFRlbXBsYXRlUmVmIH0pXG4gIGN1cnJlbnRVc2VyUmVmOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBWaWV3Q2hpbGQoJ2xhbmd1YWdlJywgeyBzdGF0aWM6IGZhbHNlLCByZWFkOiBUZW1wbGF0ZVJlZiB9KVxuICBsYW5ndWFnZVJlZjogVGVtcGxhdGVSZWY8YW55PjtcblxuICBAVmlld0NoaWxkcmVuKCduYXZiYXJSb290RHJvcGRvd24nLCB7IHJlYWQ6IE5nYkRyb3Bkb3duIH0pXG4gIG5hdmJhclJvb3REcm9wZG93bnM6IFF1ZXJ5TGlzdDxOZ2JEcm9wZG93bj47XG5cbiAgaXNPcGVuQ2hhbmdlUGFzc3dvcmQgPSBmYWxzZTtcblxuICBpc09wZW5Qcm9maWxlID0gZmFsc2U7XG5cbiAgaXNEcm9wZG93bkNoaWxkRHluYW1pYzogYm9vbGVhbjtcblxuICBnZXQgdmlzaWJsZVJvdXRlcyQoKTogT2JzZXJ2YWJsZTxBQlAuRnVsbFJvdXRlW10+IHtcbiAgICByZXR1cm4gdGhpcy5yb3V0ZXMkLnBpcGUobWFwKHJvdXRlcyA9PiBnZXRWaXNpYmxlUm91dGVzKHJvdXRlcykpKTtcbiAgfVxuXG4gIGdldCBkZWZhdWx0TGFuZ3VhZ2UkKCk6IE9ic2VydmFibGU8c3RyaW5nPiB7XG4gICAgcmV0dXJuIHRoaXMubGFuZ3VhZ2VzJC5waXBlKFxuICAgICAgbWFwKGxhbmd1YWdlcyA9PiBzbnEoKCkgPT4gbGFuZ3VhZ2VzLmZpbmQobGFuZyA9PiBsYW5nLmN1bHR1cmVOYW1lID09PSB0aGlzLnNlbGVjdGVkTGFuZ0N1bHR1cmUpLmRpc3BsYXlOYW1lKSwgJycpXG4gICAgKTtcbiAgfVxuXG4gIGdldCBkcm9wZG93bkxhbmd1YWdlcyQoKTogT2JzZXJ2YWJsZTxBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24uTGFuZ3VhZ2VbXT4ge1xuICAgIHJldHVybiB0aGlzLmxhbmd1YWdlcyQucGlwZShcbiAgICAgIG1hcChsYW5ndWFnZXMgPT4gc25xKCgpID0+IGxhbmd1YWdlcy5maWx0ZXIobGFuZyA9PiBsYW5nLmN1bHR1cmVOYW1lICE9PSB0aGlzLnNlbGVjdGVkTGFuZ0N1bHR1cmUpKSwgW10pXG4gICAgKTtcbiAgfVxuXG4gIGdldCBzZWxlY3RlZExhbmdDdWx0dXJlKCk6IHN0cmluZyB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoU2Vzc2lvblN0YXRlLmdldExhbmd1YWdlKTtcbiAgfVxuXG4gIHJpZ2h0UGFydEVsZW1lbnRzOiBUZW1wbGF0ZVJlZjxhbnk+W10gPSBbXTtcblxuICB0cmFja0J5Rm46IFRyYWNrQnlGdW5jdGlvbjxBQlAuRnVsbFJvdXRlPiA9IChfLCBpdGVtKSA9PiBpdGVtLm5hbWU7XG5cbiAgdHJhY2tFbGVtZW50QnlGbjogVHJhY2tCeUZ1bmN0aW9uPEFCUC5GdWxsUm91dGU+ID0gKF8sIGVsZW1lbnQpID0+IGVsZW1lbnQ7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUsIHByaXZhdGUgb2F1dGhTZXJ2aWNlOiBPQXV0aFNlcnZpY2UpIHt9XG5cbiAgcHJpdmF0ZSBjaGVja1dpbmRvd1dpZHRoKCkge1xuICAgIHNldFRpbWVvdXQoKCkgPT4ge1xuICAgICAgdGhpcy5uYXZiYXJSb290RHJvcGRvd25zLmZvckVhY2goaXRlbSA9PiB7XG4gICAgICAgIGl0ZW0uY2xvc2UoKTtcbiAgICAgIH0pO1xuICAgICAgaWYgKHdpbmRvdy5pbm5lcldpZHRoIDwgNzY4KSB7XG4gICAgICAgIHRoaXMuaXNEcm9wZG93bkNoaWxkRHluYW1pYyA9IGZhbHNlO1xuICAgICAgfSBlbHNlIHtcbiAgICAgICAgdGhpcy5pc0Ryb3Bkb3duQ2hpbGREeW5hbWljID0gdHJ1ZTtcbiAgICAgIH1cbiAgICB9LCAwKTtcbiAgfVxuXG4gIG5nQWZ0ZXJWaWV3SW5pdCgpIHtcbiAgICBjb25zdCBuYXZpZ2F0aW9ucyA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoTGF5b3V0U3RhdGUuZ2V0TmF2aWdhdGlvbkVsZW1lbnRzKS5tYXAoKHsgbmFtZSB9KSA9PiBuYW1lKTtcblxuICAgIGlmIChuYXZpZ2F0aW9ucy5pbmRleE9mKCdMYW5ndWFnZVJlZicpIDwgMCkge1xuICAgICAgdGhpcy5zdG9yZS5kaXNwYXRjaChcbiAgICAgICAgbmV3IEFkZE5hdmlnYXRpb25FbGVtZW50KFtcbiAgICAgICAgICB7IGVsZW1lbnQ6IHRoaXMubGFuZ3VhZ2VSZWYsIG9yZGVyOiA0LCBuYW1lOiAnTGFuZ3VhZ2VSZWYnIH0sXG4gICAgICAgICAgeyBlbGVtZW50OiB0aGlzLmN1cnJlbnRVc2VyUmVmLCBvcmRlcjogNSwgbmFtZTogJ0N1cnJlbnRVc2VyUmVmJyB9XG4gICAgICAgIF0pXG4gICAgICApO1xuICAgIH1cblxuICAgIHRoaXMubmF2RWxlbWVudHMkXG4gICAgICAucGlwZShcbiAgICAgICAgbWFwKGVsZW1lbnRzID0+IGVsZW1lbnRzLm1hcCgoeyBlbGVtZW50IH0pID0+IGVsZW1lbnQpKSxcbiAgICAgICAgZmlsdGVyKGVsZW1lbnRzID0+ICFjb21wYXJlKGVsZW1lbnRzLCB0aGlzLnJpZ2h0UGFydEVsZW1lbnRzKSksXG4gICAgICAgIHRha2VVbnRpbERlc3Ryb3kodGhpcylcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoZWxlbWVudHMgPT4ge1xuICAgICAgICBzZXRUaW1lb3V0KCgpID0+ICh0aGlzLnJpZ2h0UGFydEVsZW1lbnRzID0gZWxlbWVudHMpLCAwKTtcbiAgICAgIH0pO1xuXG4gICAgdGhpcy5jaGVja1dpbmRvd1dpZHRoKCk7XG5cbiAgICBmcm9tRXZlbnQod2luZG93LCAncmVzaXplJylcbiAgICAgIC5waXBlKFxuICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpLFxuICAgICAgICBkZWJvdW5jZVRpbWUoMjUwKVxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgIHRoaXMuY2hlY2tXaW5kb3dXaWR0aCgpO1xuICAgICAgfSk7XG4gIH1cblxuICBuZ09uRGVzdHJveSgpIHt9XG5cbiAgb25DaGFuZ2VMYW5nKGN1bHR1cmVOYW1lOiBzdHJpbmcpIHtcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBTZXRMYW5ndWFnZShjdWx0dXJlTmFtZSkpO1xuICB9XG5cbiAgbG9nb3V0KCkge1xuICAgIHRoaXMub2F1dGhTZXJ2aWNlLmxvZ091dCgpO1xuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2goXG4gICAgICBuZXcgTmF2aWdhdGUoWycvJ10sIG51bGwsIHtcbiAgICAgICAgc3RhdGU6IHsgcmVkaXJlY3RVcmw6IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoUm91dGVyU3RhdGUpLnN0YXRlLnVybCB9XG4gICAgICB9KVxuICAgICk7XG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgR2V0QXBwQ29uZmlndXJhdGlvbigpKTtcbiAgfVxufVxuXG5mdW5jdGlvbiBnZXRWaXNpYmxlUm91dGVzKHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdKSB7XG4gIHJldHVybiByb3V0ZXMucmVkdWNlKChhY2MsIHZhbCkgPT4ge1xuICAgIGlmICh2YWwuaW52aXNpYmxlKSByZXR1cm4gYWNjO1xuXG4gICAgaWYgKHZhbC5jaGlsZHJlbiAmJiB2YWwuY2hpbGRyZW4ubGVuZ3RoKSB7XG4gICAgICB2YWwuY2hpbGRyZW4gPSBnZXRWaXNpYmxlUm91dGVzKHZhbC5jaGlsZHJlbik7XG4gICAgfVxuXG4gICAgcmV0dXJuIFsuLi5hY2MsIHZhbF07XG4gIH0sIFtdKTtcbn1cbiJdfQ==
