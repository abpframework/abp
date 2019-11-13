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
var ApplicationLayoutComponent = /** @class */ (function() {
  function ApplicationLayoutComponent(store, oauthService, renderer) {
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
     */ = function(_, item) {
      return item.name;
    };
    this.trackElementByFn
    /**
     * @param {?} _
     * @param {?} element
     * @return {?}
     */ = function(_, element) {
      return element;
    };
  }
  Object.defineProperty(ApplicationLayoutComponent.prototype, 'appInfo', {
    get:
      // do not set true or false
      /**
       * @return {?}
       */
      function() {
        return this.store.selectSnapshot(ConfigState.getApplicationInfo);
      },
    enumerable: true,
    configurable: true,
  });
  Object.defineProperty(ApplicationLayoutComponent.prototype, 'visibleRoutes$', {
    /**
     * @return {?}
     */
    get: function() {
      return this.routes$.pipe(
        map(
          /**
           * @param {?} routes
           * @return {?}
           */
          function(routes) {
            return getVisibleRoutes(routes);
          },
        ),
      );
    },
    enumerable: true,
    configurable: true,
  });
  Object.defineProperty(ApplicationLayoutComponent.prototype, 'defaultLanguage$', {
    /**
     * @return {?}
     */
    get: function() {
      var _this = this;
      return this.languages$.pipe(
        map(
          /**
           * @param {?} languages
           * @return {?}
           */
          function(languages) {
            return snq(
              /**
               * @return {?}
               */
              function() {
                return languages.find(
                  /**
                   * @param {?} lang
                   * @return {?}
                   */
                  function(lang) {
                    return lang.cultureName === _this.selectedLangCulture;
                  },
                ).displayName;
              },
            );
          },
          '',
        ),
      );
    },
    enumerable: true,
    configurable: true,
  });
  Object.defineProperty(ApplicationLayoutComponent.prototype, 'dropdownLanguages$', {
    /**
     * @return {?}
     */
    get: function() {
      var _this = this;
      return this.languages$.pipe(
        map(
          /**
           * @param {?} languages
           * @return {?}
           */
          function(languages) {
            return snq(
              /**
               * @return {?}
               */
              function() {
                return languages.filter(
                  /**
                   * @param {?} lang
                   * @return {?}
                   */
                  function(lang) {
                    return lang.cultureName !== _this.selectedLangCulture;
                  },
                );
              },
            );
          },
          [],
        ),
      );
    },
    enumerable: true,
    configurable: true,
  });
  Object.defineProperty(ApplicationLayoutComponent.prototype, 'selectedLangCulture', {
    /**
     * @return {?}
     */
    get: function() {
      return this.store.selectSnapshot(SessionState.getLanguage);
    },
    enumerable: true,
    configurable: true,
  });
  /**
   * @private
   * @return {?}
   */
  ApplicationLayoutComponent.prototype.checkWindowWidth
  /**
   * @private
   * @return {?}
   */ = function() {
    var _this = this;
    setTimeout(
      /**
       * @return {?}
       */
      function() {
        if (window.innerWidth < 768) {
          _this.isDropdownChildDynamic = false;
          if (_this.smallScreen === false) {
            _this.isCollapsed = false;
            setTimeout(
              /**
               * @return {?}
               */
              function() {
                _this.isCollapsed = true;
              },
              100,
            );
          }
          _this.smallScreen = true;
        } else {
          _this.isDropdownChildDynamic = true;
          _this.smallScreen = false;
        }
      },
      0,
    );
  };
  /**
   * @return {?}
   */
  ApplicationLayoutComponent.prototype.ngAfterViewInit
  /**
   * @return {?}
   */ = function() {
    var _this = this;
    /** @type {?} */
    var navigations = this.store.selectSnapshot(LayoutState.getNavigationElements).map(
      /**
       * @param {?} __0
       * @return {?}
       */
      (function(_a) {
        var name = _a.name;
        return name;
      }),
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
          function(elements) {
            return elements.map(
              /**
               * @param {?} __0
               * @return {?}
               */
              function(_a) {
                var element = _a.element;
                return element;
              },
            );
          },
        ),
        filter(
          /**
           * @param {?} elements
           * @return {?}
           */
          function(elements) {
            return !compare(elements, _this.rightPartElements);
          },
        ),
        takeUntilDestroy(this),
      )
      .subscribe(
        /**
         * @param {?} elements
         * @return {?}
         */
        function(elements) {
          setTimeout(
            /**
             * @return {?}
             */
            function() {
              return (_this.rightPartElements = elements);
            },
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
        function() {
          _this.checkWindowWidth();
        },
      );
  };
  /**
   * @return {?}
   */
  ApplicationLayoutComponent.prototype.ngOnDestroy
  /**
   * @return {?}
   */ = function() {};
  /**
   * @param {?} cultureName
   * @return {?}
   */
  ApplicationLayoutComponent.prototype.onChangeLang
  /**
   * @param {?} cultureName
   * @return {?}
   */ = function(cultureName) {
    this.store.dispatch(new SetLanguage(cultureName));
  };
  /**
   * @return {?}
   */
  ApplicationLayoutComponent.prototype.logout
  /**
   * @return {?}
   */ = function() {
    this.oauthService.logOut();
    this.store.dispatch(
      new Navigate(['/'], null, {
        state: { redirectUrl: this.store.selectSnapshot(RouterState).state.url },
      }),
    );
    this.store.dispatch(new GetAppConfiguration());
  };
  /**
   * @param {?} event
   * @param {?} childrenContainer
   * @return {?}
   */
  ApplicationLayoutComponent.prototype.openChange
  /**
   * @param {?} event
   * @param {?} childrenContainer
   * @return {?}
   */ = function(event, childrenContainer) {
    var _this = this;
    if (!event) {
      Object.keys(childrenContainer.style)
        .filter(
          /**
           * @param {?} key
           * @return {?}
           */
          function(key) {
            return Number.isInteger(+key);
          },
        )
        .forEach(
          /**
           * @param {?} key
           * @return {?}
           */
          function(key) {
            _this.renderer.removeStyle(childrenContainer, childrenContainer.style[key]);
          },
        );
      this.renderer.removeStyle(childrenContainer, 'left');
    }
  };
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
  ApplicationLayoutComponent.ctorParameters = function() {
    return [{ type: Store }, { type: OAuthService }, { type: Renderer2 }];
  };
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
  return ApplicationLayoutComponent;
})();
export { ApplicationLayoutComponent };
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
    function(acc, val) {
      if (val.invisible) return acc;
      if (val.children && val.children.length) {
        val.children = getVisibleRoutes(val.children);
      }
      return tslib_1.__spread(acc, [val]);
    },
    [],
  );
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9hcHBsaWNhdGlvbi1sYXlvdXQvYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFJTCxXQUFXLEVBRVgsbUJBQW1CLEVBQ25CLFlBQVksRUFDWixXQUFXLEVBQ1gsZ0JBQWdCLEdBQ2pCLE1BQU0sY0FBYyxDQUFDO0FBQ3RCLE9BQU8sRUFBRSxrQkFBa0IsRUFBRSxlQUFlLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUMzRSxPQUFPLEVBRUwsU0FBUyxFQUdULFNBQVMsRUFDVCxXQUFXLEVBRVgsU0FBUyxHQUdWLE1BQU0sZUFBZSxDQUFDO0FBRXZCLE9BQU8sRUFBRSxRQUFRLEVBQUUsV0FBVyxFQUFFLE1BQU0scUJBQXFCLENBQUM7QUFDNUQsT0FBTyxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDNUMsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQ25ELE9BQU8sT0FBTyxNQUFNLGNBQWMsQ0FBQztBQUNuQyxPQUFPLEVBQUUsU0FBUyxFQUFFLFVBQVUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUM3QyxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0sRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUMzRCxPQUFPLEdBQUcsTUFBTSxLQUFLLENBQUM7QUFDdEIsT0FBTyxFQUFFLG9CQUFvQixFQUFFLE1BQU0sZUFBZSxDQUFDO0FBRXJELE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFFM0M7SUFrRUUsb0NBQW9CLEtBQVksRUFBVSxZQUEwQixFQUFVLFFBQW1CO1FBQTdFLFVBQUssR0FBTCxLQUFLLENBQU87UUFBVSxp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Z0RBQzREOztJQUU1RCxrREFDcUQ7O0lBRXJELG9EQUNpQzs7SUFFakMsaURBQzhCOztJQUU5Qiw0REFBZ0M7O0lBRWhDLGlEQUFtQjs7SUFFbkIsaURBQXFCOztJQTZCckIsdURBQTJDOztJQUUzQywrQ0FBbUU7O0lBRW5FLHNEQUEyRTs7Ozs7SUFFL0QsMkNBQW9COzs7OztJQUFFLGtEQUFrQzs7Ozs7SUFBRSw4Q0FBMkI7Ozs7OztBQWtGbkcsU0FBUyxnQkFBZ0IsQ0FBQyxNQUF1QjtJQUMvQyxPQUFPLE1BQU0sQ0FBQyxNQUFNOzs7OztJQUFDLFVBQUMsR0FBRyxFQUFFLEdBQUc7UUFDNUIsSUFBSSxHQUFHLENBQUMsU0FBUztZQUFFLE9BQU8sR0FBRyxDQUFDO1FBRTlCLElBQUksR0FBRyxDQUFDLFFBQVEsSUFBSSxHQUFHLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUN2QyxHQUFHLENBQUMsUUFBUSxHQUFHLGdCQUFnQixDQUFDLEdBQUcsQ0FBQyxRQUFRLENBQUMsQ0FBQztTQUMvQztRQUVELHdCQUFXLEdBQUcsR0FBRSxHQUFHLEdBQUU7SUFDdkIsQ0FBQyxHQUFFLEVBQUUsQ0FBQyxDQUFDO0FBQ1QsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7XG4gIEFCUCxcbiAgQXBwbGljYXRpb25Db25maWd1cmF0aW9uLFxuICBDb25maWcsXG4gIENvbmZpZ1N0YXRlLFxuICBlTGF5b3V0VHlwZSxcbiAgR2V0QXBwQ29uZmlndXJhdGlvbixcbiAgU2Vzc2lvblN0YXRlLFxuICBTZXRMYW5ndWFnZSxcbiAgdGFrZVVudGlsRGVzdHJveSxcbn0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IGNvbGxhcHNlV2l0aE1hcmdpbiwgc2xpZGVGcm9tQm90dG9tIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHtcbiAgQWZ0ZXJWaWV3SW5pdCxcbiAgQ29tcG9uZW50LFxuICBPbkRlc3Ryb3ksXG4gIFF1ZXJ5TGlzdCxcbiAgUmVuZGVyZXIyLFxuICBUZW1wbGF0ZVJlZixcbiAgVHJhY2tCeUZ1bmN0aW9uLFxuICBWaWV3Q2hpbGQsXG4gIFZpZXdDaGlsZHJlbixcbiAgRWxlbWVudFJlZixcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBOZ2JEcm9wZG93biB9IGZyb20gJ0BuZy1ib290c3RyYXAvbmctYm9vdHN0cmFwJztcbmltcG9ydCB7IE5hdmlnYXRlLCBSb3V0ZXJTdGF0ZSB9IGZyb20gJ0BuZ3hzL3JvdXRlci1wbHVnaW4nO1xuaW1wb3J0IHsgU2VsZWN0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9BdXRoU2VydmljZSB9IGZyb20gJ2FuZ3VsYXItb2F1dGgyLW9pZGMnO1xuaW1wb3J0IGNvbXBhcmUgZnJvbSAnanVzdC1jb21wYXJlJztcbmltcG9ydCB7IGZyb21FdmVudCwgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgZGVib3VuY2VUaW1lLCBmaWx0ZXIsIG1hcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcbmltcG9ydCB7IEFkZE5hdmlnYXRpb25FbGVtZW50IH0gZnJvbSAnLi4vLi4vYWN0aW9ucyc7XG5pbXBvcnQgeyBMYXlvdXQgfSBmcm9tICcuLi8uLi9tb2RlbHMvbGF5b3V0JztcbmltcG9ydCB7IExheW91dFN0YXRlIH0gZnJvbSAnLi4vLi4vc3RhdGVzJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWxheW91dC1hcHBsaWNhdGlvbicsXG4gIHRlbXBsYXRlVXJsOiAnLi9hcHBsaWNhdGlvbi1sYXlvdXQuY29tcG9uZW50Lmh0bWwnLFxuICBhbmltYXRpb25zOiBbc2xpZGVGcm9tQm90dG9tLCBjb2xsYXBzZVdpdGhNYXJnaW5dLFxufSlcbmV4cG9ydCBjbGFzcyBBcHBsaWNhdGlvbkxheW91dENvbXBvbmVudCBpbXBsZW1lbnRzIEFmdGVyVmlld0luaXQsIE9uRGVzdHJveSB7XG4gIC8vIHJlcXVpcmVkIGZvciBkeW5hbWljIGNvbXBvbmVudFxuICBzdGF0aWMgdHlwZSA9IGVMYXlvdXRUeXBlLmFwcGxpY2F0aW9uO1xuXG4gIEBTZWxlY3QoQ29uZmlnU3RhdGUuZ2V0T25lKCdyb3V0ZXMnKSlcbiAgcm91dGVzJDogT2JzZXJ2YWJsZTxBQlAuRnVsbFJvdXRlW10+O1xuXG4gIEBTZWxlY3QoQ29uZmlnU3RhdGUuZ2V0T25lKCdjdXJyZW50VXNlcicpKVxuICBjdXJyZW50VXNlciQ6IE9ic2VydmFibGU8QXBwbGljYXRpb25Db25maWd1cmF0aW9uLkN1cnJlbnRVc2VyPjtcblxuICBAU2VsZWN0KENvbmZpZ1N0YXRlLmdldERlZXAoJ2xvY2FsaXphdGlvbi5sYW5ndWFnZXMnKSlcbiAgbGFuZ3VhZ2VzJDogT2JzZXJ2YWJsZTxBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24uTGFuZ3VhZ2VbXT47XG5cbiAgQFNlbGVjdChMYXlvdXRTdGF0ZS5nZXROYXZpZ2F0aW9uRWxlbWVudHMpXG4gIG5hdkVsZW1lbnRzJDogT2JzZXJ2YWJsZTxMYXlvdXQuTmF2aWdhdGlvbkVsZW1lbnRbXT47XG5cbiAgQFZpZXdDaGlsZCgnY3VycmVudFVzZXInLCB7IHN0YXRpYzogZmFsc2UsIHJlYWQ6IFRlbXBsYXRlUmVmIH0pXG4gIGN1cnJlbnRVc2VyUmVmOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBWaWV3Q2hpbGQoJ2xhbmd1YWdlJywgeyBzdGF0aWM6IGZhbHNlLCByZWFkOiBUZW1wbGF0ZVJlZiB9KVxuICBsYW5ndWFnZVJlZjogVGVtcGxhdGVSZWY8YW55PjtcblxuICBpc0Ryb3Bkb3duQ2hpbGREeW5hbWljOiBib29sZWFuO1xuXG4gIGlzQ29sbGFwc2VkID0gdHJ1ZTtcblxuICBzbWFsbFNjcmVlbjogYm9vbGVhbjsgLy8gZG8gbm90IHNldCB0cnVlIG9yIGZhbHNlXG5cbiAgZ2V0IGFwcEluZm8oKTogQ29uZmlnLkFwcGxpY2F0aW9uIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRBcHBsaWNhdGlvbkluZm8pO1xuICB9XG5cbiAgZ2V0IHZpc2libGVSb3V0ZXMkKCk6IE9ic2VydmFibGU8QUJQLkZ1bGxSb3V0ZVtdPiB7XG4gICAgcmV0dXJuIHRoaXMucm91dGVzJC5waXBlKG1hcChyb3V0ZXMgPT4gZ2V0VmlzaWJsZVJvdXRlcyhyb3V0ZXMpKSk7XG4gIH1cblxuICBnZXQgZGVmYXVsdExhbmd1YWdlJCgpOiBPYnNlcnZhYmxlPHN0cmluZz4ge1xuICAgIHJldHVybiB0aGlzLmxhbmd1YWdlcyQucGlwZShcbiAgICAgIG1hcChcbiAgICAgICAgbGFuZ3VhZ2VzID0+IHNucSgoKSA9PiBsYW5ndWFnZXMuZmluZChsYW5nID0+IGxhbmcuY3VsdHVyZU5hbWUgPT09IHRoaXMuc2VsZWN0ZWRMYW5nQ3VsdHVyZSkuZGlzcGxheU5hbWUpLFxuICAgICAgICAnJyxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIGdldCBkcm9wZG93bkxhbmd1YWdlcyQoKTogT2JzZXJ2YWJsZTxBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24uTGFuZ3VhZ2VbXT4ge1xuICAgIHJldHVybiB0aGlzLmxhbmd1YWdlcyQucGlwZShcbiAgICAgIG1hcChsYW5ndWFnZXMgPT4gc25xKCgpID0+IGxhbmd1YWdlcy5maWx0ZXIobGFuZyA9PiBsYW5nLmN1bHR1cmVOYW1lICE9PSB0aGlzLnNlbGVjdGVkTGFuZ0N1bHR1cmUpKSwgW10pLFxuICAgICk7XG4gIH1cblxuICBnZXQgc2VsZWN0ZWRMYW5nQ3VsdHVyZSgpOiBzdHJpbmcge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFNlc3Npb25TdGF0ZS5nZXRMYW5ndWFnZSk7XG4gIH1cblxuICByaWdodFBhcnRFbGVtZW50czogVGVtcGxhdGVSZWY8YW55PltdID0gW107XG5cbiAgdHJhY2tCeUZuOiBUcmFja0J5RnVuY3Rpb248QUJQLkZ1bGxSb3V0ZT4gPSAoXywgaXRlbSkgPT4gaXRlbS5uYW1lO1xuXG4gIHRyYWNrRWxlbWVudEJ5Rm46IFRyYWNrQnlGdW5jdGlvbjxBQlAuRnVsbFJvdXRlPiA9IChfLCBlbGVtZW50KSA9PiBlbGVtZW50O1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlLCBwcml2YXRlIG9hdXRoU2VydmljZTogT0F1dGhTZXJ2aWNlLCBwcml2YXRlIHJlbmRlcmVyOiBSZW5kZXJlcjIpIHt9XG5cbiAgcHJpdmF0ZSBjaGVja1dpbmRvd1dpZHRoKCkge1xuICAgIHNldFRpbWVvdXQoKCkgPT4ge1xuICAgICAgaWYgKHdpbmRvdy5pbm5lcldpZHRoIDwgNzY4KSB7XG4gICAgICAgIHRoaXMuaXNEcm9wZG93bkNoaWxkRHluYW1pYyA9IGZhbHNlO1xuICAgICAgICBpZiAodGhpcy5zbWFsbFNjcmVlbiA9PT0gZmFsc2UpIHtcbiAgICAgICAgICB0aGlzLmlzQ29sbGFwc2VkID0gZmFsc2U7XG4gICAgICAgICAgc2V0VGltZW91dCgoKSA9PiB7XG4gICAgICAgICAgICB0aGlzLmlzQ29sbGFwc2VkID0gdHJ1ZTtcbiAgICAgICAgICB9LCAxMDApO1xuICAgICAgICB9XG4gICAgICAgIHRoaXMuc21hbGxTY3JlZW4gPSB0cnVlO1xuICAgICAgfSBlbHNlIHtcbiAgICAgICAgdGhpcy5pc0Ryb3Bkb3duQ2hpbGREeW5hbWljID0gdHJ1ZTtcbiAgICAgICAgdGhpcy5zbWFsbFNjcmVlbiA9IGZhbHNlO1xuICAgICAgfVxuICAgIH0sIDApO1xuICB9XG5cbiAgbmdBZnRlclZpZXdJbml0KCkge1xuICAgIGNvbnN0IG5hdmlnYXRpb25zID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChMYXlvdXRTdGF0ZS5nZXROYXZpZ2F0aW9uRWxlbWVudHMpLm1hcCgoeyBuYW1lIH0pID0+IG5hbWUpO1xuXG4gICAgaWYgKG5hdmlnYXRpb25zLmluZGV4T2YoJ0xhbmd1YWdlUmVmJykgPCAwKSB7XG4gICAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKFxuICAgICAgICBuZXcgQWRkTmF2aWdhdGlvbkVsZW1lbnQoW1xuICAgICAgICAgIHsgZWxlbWVudDogdGhpcy5sYW5ndWFnZVJlZiwgb3JkZXI6IDQsIG5hbWU6ICdMYW5ndWFnZVJlZicgfSxcbiAgICAgICAgICB7IGVsZW1lbnQ6IHRoaXMuY3VycmVudFVzZXJSZWYsIG9yZGVyOiA1LCBuYW1lOiAnQ3VycmVudFVzZXJSZWYnIH0sXG4gICAgICAgIF0pLFxuICAgICAgKTtcbiAgICB9XG5cbiAgICB0aGlzLm5hdkVsZW1lbnRzJFxuICAgICAgLnBpcGUoXG4gICAgICAgIG1hcChlbGVtZW50cyA9PiBlbGVtZW50cy5tYXAoKHsgZWxlbWVudCB9KSA9PiBlbGVtZW50KSksXG4gICAgICAgIGZpbHRlcihlbGVtZW50cyA9PiAhY29tcGFyZShlbGVtZW50cywgdGhpcy5yaWdodFBhcnRFbGVtZW50cykpLFxuICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZShlbGVtZW50cyA9PiB7XG4gICAgICAgIHNldFRpbWVvdXQoKCkgPT4gKHRoaXMucmlnaHRQYXJ0RWxlbWVudHMgPSBlbGVtZW50cyksIDApO1xuICAgICAgfSk7XG5cbiAgICB0aGlzLmNoZWNrV2luZG93V2lkdGgoKTtcblxuICAgIGZyb21FdmVudCh3aW5kb3csICdyZXNpemUnKVxuICAgICAgLnBpcGUoXG4gICAgICAgIHRha2VVbnRpbERlc3Ryb3kodGhpcyksXG4gICAgICAgIGRlYm91bmNlVGltZSgxNTApLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgIHRoaXMuY2hlY2tXaW5kb3dXaWR0aCgpO1xuICAgICAgfSk7XG4gIH1cblxuICBuZ09uRGVzdHJveSgpIHt9XG5cbiAgb25DaGFuZ2VMYW5nKGN1bHR1cmVOYW1lOiBzdHJpbmcpIHtcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBTZXRMYW5ndWFnZShjdWx0dXJlTmFtZSkpO1xuICB9XG5cbiAgbG9nb3V0KCkge1xuICAgIHRoaXMub2F1dGhTZXJ2aWNlLmxvZ091dCgpO1xuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2goXG4gICAgICBuZXcgTmF2aWdhdGUoWycvJ10sIG51bGwsIHtcbiAgICAgICAgc3RhdGU6IHsgcmVkaXJlY3RVcmw6IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoUm91dGVyU3RhdGUpLnN0YXRlLnVybCB9LFxuICAgICAgfSksXG4gICAgKTtcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBHZXRBcHBDb25maWd1cmF0aW9uKCkpO1xuICB9XG5cbiAgb3BlbkNoYW5nZShldmVudDogYm9vbGVhbiwgY2hpbGRyZW5Db250YWluZXI6IEhUTUxEaXZFbGVtZW50KSB7XG4gICAgaWYgKCFldmVudCkge1xuICAgICAgT2JqZWN0LmtleXMoY2hpbGRyZW5Db250YWluZXIuc3R5bGUpXG4gICAgICAgIC5maWx0ZXIoa2V5ID0+IE51bWJlci5pc0ludGVnZXIoK2tleSkpXG4gICAgICAgIC5mb3JFYWNoKGtleSA9PiB7XG4gICAgICAgICAgdGhpcy5yZW5kZXJlci5yZW1vdmVTdHlsZShjaGlsZHJlbkNvbnRhaW5lciwgY2hpbGRyZW5Db250YWluZXIuc3R5bGVba2V5XSk7XG4gICAgICAgIH0pO1xuICAgICAgdGhpcy5yZW5kZXJlci5yZW1vdmVTdHlsZShjaGlsZHJlbkNvbnRhaW5lciwgJ2xlZnQnKTtcbiAgICB9XG4gIH1cbn1cblxuZnVuY3Rpb24gZ2V0VmlzaWJsZVJvdXRlcyhyb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSkge1xuICByZXR1cm4gcm91dGVzLnJlZHVjZSgoYWNjLCB2YWwpID0+IHtcbiAgICBpZiAodmFsLmludmlzaWJsZSkgcmV0dXJuIGFjYztcblxuICAgIGlmICh2YWwuY2hpbGRyZW4gJiYgdmFsLmNoaWxkcmVuLmxlbmd0aCkge1xuICAgICAgdmFsLmNoaWxkcmVuID0gZ2V0VmlzaWJsZVJvdXRlcyh2YWwuY2hpbGRyZW4pO1xuICAgIH1cblxuICAgIHJldHVybiBbLi4uYWNjLCB2YWxdO1xuICB9LCBbXSk7XG59XG4iXX0=
