import { __spread, __assign, __decorate, __metadata, __extends } from 'tslib';
import {
  ConfigState,
  SessionState,
  takeUntilDestroy,
  SetLanguage,
  GetAppConfiguration,
  LazyLoadService,
  CoreModule,
} from '@abp/ng.core';
import { slideFromBottom, collapseWithMargin, ThemeSharedModule } from '@abp/ng.theme.shared';
import {
  Component,
  Renderer2,
  ViewChild,
  TemplateRef,
  ChangeDetectionStrategy,
  ViewEncapsulation,
  Injectable,
  ɵɵdefineInjectable,
  ɵɵinject,
  NgModule,
} from '@angular/core';
import { NgbCollapseModule, NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { ValidationErrorComponent as ValidationErrorComponent$1, NgxValidateCoreModule } from '@ngx-validate/core';
import { Action, Selector, State, Store, Select, NgxsModule } from '@ngxs/store';
import { ToastModule } from 'primeng/toast';
import { Navigate, RouterState } from '@ngxs/router-plugin';
import { OAuthService } from 'angular-oauth2-oidc';
import compare from 'just-compare';
import { fromEvent, Observable } from 'rxjs';
import { map, filter, debounceTime } from 'rxjs/operators';
import snq from 'snq';

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var AccountLayoutComponent = /** @class */ (function() {
  function AccountLayoutComponent() {}
  // required for dynamic component
  AccountLayoutComponent.type = 'account' /* account */;
  AccountLayoutComponent.decorators = [
    {
      type: Component,
      args: [
        {
          selector: 'abp-layout-account',
          template:
            '\n    <router-outlet></router-outlet>\n    <abp-confirmation></abp-confirmation>\n    <abp-toast></abp-toast>\n  ',
        },
      ],
    },
  ];
  return AccountLayoutComponent;
})();
if (false) {
  /** @type {?} */
  AccountLayoutComponent.type;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var AddNavigationElement = /** @class */ (function() {
  function AddNavigationElement(payload) {
    this.payload = payload;
  }
  AddNavigationElement.type = '[Layout] Add Navigation Element';
  return AddNavigationElement;
})();
if (false) {
  /** @type {?} */
  AddNavigationElement.type;
  /** @type {?} */
  AddNavigationElement.prototype.payload;
}
var RemoveNavigationElementByName = /** @class */ (function() {
  function RemoveNavigationElementByName(name) {
    this.name = name;
  }
  RemoveNavigationElementByName.type = '[Layout] Remove Navigation ElementByName';
  return RemoveNavigationElementByName;
})();
if (false) {
  /** @type {?} */
  RemoveNavigationElementByName.type;
  /** @type {?} */
  RemoveNavigationElementByName.prototype.name;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var LayoutState = /** @class */ (function() {
  function LayoutState() {}
  /**
   * @param {?} __0
   * @return {?}
   */
  LayoutState.getNavigationElements
  /**
   * @param {?} __0
   * @return {?}
   */ = function(_a) {
    var navigationElements = _a.navigationElements;
    return navigationElements;
  };
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  LayoutState.prototype.layoutAddAction
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */ = function(_a, _b) {
    var getState = _a.getState,
      patchState = _a.patchState;
    var _c = _b.payload,
      payload = _c === void 0 ? [] : _c;
    var navigationElements = getState().navigationElements;
    if (!Array.isArray(payload)) {
      payload = [payload];
    }
    if (navigationElements.length) {
      payload = snq(
        /**
         * @return {?}
         */
        function() {
          return /** @type {?} */ (payload).filter(
            /**
             * @param {?} __0
             * @return {?}
             */
            function(_a) {
              var name = _a.name;
              return (
                navigationElements.findIndex(
                  /**
                   * @param {?} nav
                   * @return {?}
                   */
                  function(nav) {
                    return nav.name === name;
                  },
                ) < 0
              );
            },
          );
        },
        [],
      );
    }
    if (!payload.length) return;
    navigationElements = __spread(navigationElements, payload)
      .map(
        /**
         * @param {?} element
         * @return {?}
         */
        function(element) {
          return __assign({}, element, { order: element.order || 99 });
        },
      )
      .sort(
        /**
         * @param {?} a
         * @param {?} b
         * @return {?}
         */
        function(a, b) {
          return a.order - b.order;
        },
      );
    return patchState({
      navigationElements: navigationElements,
    });
  };
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  LayoutState.prototype.layoutRemoveAction
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */ = function(_a, _b) {
    var getState = _a.getState,
      patchState = _a.patchState;
    var name = _b.name;
    var navigationElements = getState().navigationElements;
    /** @type {?} */
    var index = navigationElements.findIndex(
      /**
       * @param {?} element
       * @return {?}
       */
      (function(element) {
        return element.name === name;
      }),
    );
    if (index > -1) {
      navigationElements = navigationElements.splice(index, 1);
    }
    return patchState({
      navigationElements: navigationElements,
    });
  };
  __decorate(
    [
      Action(AddNavigationElement),
      __metadata('design:type', Function),
      __metadata('design:paramtypes', [Object, AddNavigationElement]),
      __metadata('design:returntype', void 0),
    ],
    LayoutState.prototype,
    'layoutAddAction',
    null,
  );
  __decorate(
    [
      Action(RemoveNavigationElementByName),
      __metadata('design:type', Function),
      __metadata('design:paramtypes', [Object, RemoveNavigationElementByName]),
      __metadata('design:returntype', void 0),
    ],
    LayoutState.prototype,
    'layoutRemoveAction',
    null,
  );
  __decorate(
    [
      Selector(),
      __metadata('design:type', Function),
      __metadata('design:paramtypes', [Object]),
      __metadata('design:returntype', Array),
    ],
    LayoutState,
    'getNavigationElements',
    null,
  );
  LayoutState = __decorate(
    [
      State({
        name: 'LayoutState',
        defaults: /** @type {?} */ ({ navigationElements: [] }),
      }),
    ],
    LayoutState,
  );
  return LayoutState;
})();

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
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
  __decorate(
    [Select(ConfigState.getOne('routes')), __metadata('design:type', Observable)],
    ApplicationLayoutComponent.prototype,
    'routes$',
    void 0,
  );
  __decorate(
    [Select(ConfigState.getOne('currentUser')), __metadata('design:type', Observable)],
    ApplicationLayoutComponent.prototype,
    'currentUser$',
    void 0,
  );
  __decorate(
    [Select(ConfigState.getDeep('localization.languages')), __metadata('design:type', Observable)],
    ApplicationLayoutComponent.prototype,
    'languages$',
    void 0,
  );
  __decorate(
    [Select(LayoutState.getNavigationElements), __metadata('design:type', Observable)],
    ApplicationLayoutComponent.prototype,
    'navElements$',
    void 0,
  );
  return ApplicationLayoutComponent;
})();
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
      return __spread(acc, [val]);
    },
    [],
  );
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var EmptyLayoutComponent = /** @class */ (function() {
  function EmptyLayoutComponent() {}
  EmptyLayoutComponent.type = 'empty' /* empty */;
  EmptyLayoutComponent.decorators = [
    {
      type: Component,
      args: [
        {
          selector: 'abp-layout-empty',
          template:
            '\n    <router-outlet></router-outlet>\n    <abp-confirmation></abp-confirmation>\n    <abp-toast></abp-toast>\n  ',
        },
      ],
    },
  ];
  return EmptyLayoutComponent;
})();
if (false) {
  /** @type {?} */
  EmptyLayoutComponent.type;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var ValidationErrorComponent = /** @class */ (function(_super) {
  __extends(ValidationErrorComponent, _super);
  function ValidationErrorComponent() {
    return (_super !== null && _super.apply(this, arguments)) || this;
  }
  Object.defineProperty(ValidationErrorComponent.prototype, 'abpErrors', {
    /**
     * @return {?}
     */
    get: function() {
      if (!this.errors || !this.errors.length) return [];
      return this.errors.map(
        /**
         * @param {?} error
         * @return {?}
         */
        function(error) {
          if (!error.message) return error;
          /** @type {?} */
          var index = error.message.indexOf('[');
          if (index > -1) {
            return __assign({}, error, {
              message: error.message.slice(0, index),
              interpoliteParams: error.message.slice(index + 1, error.message.length - 1).split(','),
            });
          }
          return error;
        },
      );
    },
    enumerable: true,
    configurable: true,
  });
  ValidationErrorComponent.decorators = [
    {
      type: Component,
      args: [
        {
          selector: 'abp-validation-error',
          template:
            '\n    <div class="invalid-feedback" *ngFor="let error of abpErrors; trackBy: trackByFn">\n      {{ error.message | abpLocalization: error.interpoliteParams }}\n    </div>\n  ',
          changeDetection: ChangeDetectionStrategy.OnPush,
          encapsulation: ViewEncapsulation.None,
        },
      ],
    },
  ];
  return ValidationErrorComponent;
})(ValidationErrorComponent$1);

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var styles =
  '\n.content-header-title {\n    font-size: 24px;\n}\n\n.entry-row {\n    margin-bottom: 15px;\n}\n\n#main-navbar-tools a.dropdown-toggle {\n    text-decoration: none;\n    color: #fff;\n}\n\n.navbar .dropdown-submenu {\n    position: relative;\n}\n.navbar .dropdown-menu {\n    margin: 0;\n    padding: 0;\n}\n    .navbar .dropdown-menu a {\n        font-size: .9em;\n        padding: 10px 15px;\n        display: block;\n        min-width: 210px;\n        text-align: left;\n        border-radius: 0.25rem;\n        min-height: 44px;\n    }\n.navbar .dropdown-submenu a::after {\n    transform: rotate(-90deg);\n    position: absolute;\n    right: 16px;\n    top: 18px;\n}\n.navbar .dropdown-submenu .dropdown-menu {\n    top: 0;\n    left: 100%;\n}\n\n.card-header .btn {\n    padding: 2px 6px;\n}\n.card-header h5 {\n    margin: 0;\n}\n.container > .card {\n    box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075) !important;\n}\n\n@media screen and (min-width: 768px) {\n    .navbar .dropdown:hover > .dropdown-menu {\n        display: block;\n    }\n\n    .navbar .dropdown-submenu:hover > .dropdown-menu {\n        display: block;\n    }\n}\n.input-validation-error {\n    border-color: #dc3545;\n}\n.field-validation-error {\n    font-size: 0.8em;\n}\n\n\n.abp-main-nav-dropdown {\n  margin-top: -50%;\n}\n';

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var InitialService = /** @class */ (function() {
  function InitialService(lazyLoadService) {
    this.lazyLoadService = lazyLoadService;
    this.appendStyle().subscribe();
  }
  /**
   * @return {?}
   */
  InitialService.prototype.appendStyle
  /**
   * @return {?}
   */ = function() {
    return this.lazyLoadService.load(null, 'style', styles, 'head', 'afterbegin');
  };
  InitialService.decorators = [{ type: Injectable, args: [{ providedIn: 'root' }] }];
  /** @nocollapse */
  InitialService.ctorParameters = function() {
    return [{ type: LazyLoadService }];
  };
  /** @nocollapse */ InitialService.ngInjectableDef = ɵɵdefineInjectable({
    factory: function InitialService_Factory() {
      return new InitialService(ɵɵinject(LazyLoadService));
    },
    token: InitialService,
    providedIn: 'root',
  });
  return InitialService;
})();
if (false) {
  /**
   * @type {?}
   * @private
   */
  InitialService.prototype.lazyLoadService;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/** @type {?} */
var LAYOUTS = [ApplicationLayoutComponent, AccountLayoutComponent, EmptyLayoutComponent];
var ThemeBasicModule = /** @class */ (function() {
  function ThemeBasicModule(initialService) {
    this.initialService = initialService;
  }
  ThemeBasicModule.decorators = [
    {
      type: NgModule,
      args: [
        {
          declarations: __spread(LAYOUTS, [ValidationErrorComponent]),
          imports: [
            CoreModule,
            ThemeSharedModule,
            NgbCollapseModule,
            NgbDropdownModule,
            ToastModule,
            NgxValidateCoreModule,
            NgxsModule.forFeature([LayoutState]),
            NgxValidateCoreModule.forRoot({
              targetSelector: '.form-group',
              blueprints: {
                email: 'AbpAccount::ThisFieldIsNotAValidEmailAddress.',
                max: 'AbpAccount::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]',
                maxlength: 'AbpAccount::ThisFieldMustBeAStringWithAMaximumLengthOf{1}[{{ requiredLength }}]',
                min: 'AbpAccount::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]',
                minlength: 'AbpAccount::ThisFieldMustBeAStringOrArrayTypeWithAMinimumLengthOf[{{ min }},{{ max }}]',
                required: 'AbpAccount::ThisFieldIsRequired.',
                passwordMismatch: 'AbpIdentity::Identity.PasswordConfirmationFailed',
              },
              errorTemplate: ValidationErrorComponent,
            }),
          ],
          exports: __spread(LAYOUTS),
          entryComponents: __spread(LAYOUTS, [ValidationErrorComponent]),
        },
      ],
    },
  ];
  /** @nocollapse */
  ThemeBasicModule.ctorParameters = function() {
    return [{ type: InitialService }];
  };
  return ThemeBasicModule;
})();
if (false) {
  /**
   * @type {?}
   * @private
   */
  ThemeBasicModule.prototype.initialService;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var Layout;
(function(Layout) {
  /**
   * @record
   */
  function State() {}
  Layout.State = State;
  if (false) {
    /** @type {?} */
    State.prototype.navigationElements;
  }
  /**
   * @record
   */
  function NavigationElement() {}
  Layout.NavigationElement = NavigationElement;
  if (false) {
    /** @type {?} */
    NavigationElement.prototype.name;
    /** @type {?} */
    NavigationElement.prototype.element;
    /** @type {?|undefined} */
    NavigationElement.prototype.order;
  }
})(Layout || (Layout = {}));

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

export {
  AccountLayoutComponent,
  AddNavigationElement,
  ApplicationLayoutComponent,
  EmptyLayoutComponent,
  LAYOUTS,
  LayoutState,
  RemoveNavigationElementByName,
  ThemeBasicModule,
  ValidationErrorComponent,
  ApplicationLayoutComponent as ɵa,
  LayoutState as ɵb,
  AccountLayoutComponent as ɵc,
  EmptyLayoutComponent as ɵd,
  ValidationErrorComponent as ɵe,
  LayoutState as ɵf,
  AddNavigationElement as ɵg,
  RemoveNavigationElementByName as ɵh,
  InitialService as ɵj,
};
//# sourceMappingURL=abp-ng.theme.basic.js.map
