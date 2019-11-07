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
import { __decorate, __metadata } from 'tslib';
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
class AccountLayoutComponent {}
// required for dynamic component
AccountLayoutComponent.type = 'account' /* account */;
AccountLayoutComponent.decorators = [
  {
    type: Component,
    args: [
      {
        selector: 'abp-layout-account',
        template: `
    <router-outlet></router-outlet>
    <abp-confirmation></abp-confirmation>
    <abp-toast></abp-toast>
  `,
      },
    ],
  },
];
if (false) {
  /** @type {?} */
  AccountLayoutComponent.type;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class AddNavigationElement {
  /**
   * @param {?} payload
   */
  constructor(payload) {
    this.payload = payload;
  }
}
AddNavigationElement.type = '[Layout] Add Navigation Element';
if (false) {
  /** @type {?} */
  AddNavigationElement.type;
  /** @type {?} */
  AddNavigationElement.prototype.payload;
}
class RemoveNavigationElementByName {
  /**
   * @param {?} name
   */
  constructor(name) {
    this.name = name;
  }
}
RemoveNavigationElementByName.type = '[Layout] Remove Navigation ElementByName';
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
let LayoutState = class LayoutState {
  /**
   * @param {?} __0
   * @return {?}
   */
  static getNavigationElements({ navigationElements }) {
    return navigationElements;
  }
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  layoutAddAction({ getState, patchState }, { payload = [] }) {
    let { navigationElements } = getState();
    if (!Array.isArray(payload)) {
      payload = [payload];
    }
    if (navigationElements.length) {
      payload = snq(
        /**
         * @return {?}
         */
        () =>
          /** @type {?} */ (payload).filter(
            /**
             * @param {?} __0
             * @return {?}
             */
            ({ name }) =>
              navigationElements.findIndex(
                /**
                 * @param {?} nav
                 * @return {?}
                 */
                nav => nav.name === name,
              ) < 0,
          ),
        [],
      );
    }
    if (!payload.length) return;
    navigationElements = [...navigationElements, ...payload]
      .map(
        /**
         * @param {?} element
         * @return {?}
         */
        element => Object.assign({}, element, { order: element.order || 99 }),
      )
      .sort(
        /**
         * @param {?} a
         * @param {?} b
         * @return {?}
         */
        (a, b) => a.order - b.order,
      );
    return patchState({
      navigationElements,
    });
  }
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  layoutRemoveAction({ getState, patchState }, { name }) {
    let { navigationElements } = getState();
    /** @type {?} */
    const index = navigationElements.findIndex(
      /**
       * @param {?} element
       * @return {?}
       */
      (element => element.name === name),
    );
    if (index > -1) {
      navigationElements = navigationElements.splice(index, 1);
    }
    return patchState({
      navigationElements,
    });
  }
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

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class ApplicationLayoutComponent {
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

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class EmptyLayoutComponent {}
EmptyLayoutComponent.type = 'empty' /* empty */;
EmptyLayoutComponent.decorators = [
  {
    type: Component,
    args: [
      {
        selector: 'abp-layout-empty',
        template: `
    <router-outlet></router-outlet>
    <abp-confirmation></abp-confirmation>
    <abp-toast></abp-toast>
  `,
      },
    ],
  },
];
if (false) {
  /** @type {?} */
  EmptyLayoutComponent.type;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class ValidationErrorComponent extends ValidationErrorComponent$1 {
  /**
   * @return {?}
   */
  get abpErrors() {
    if (!this.errors || !this.errors.length) return [];
    return this.errors.map(
      /**
       * @param {?} error
       * @return {?}
       */
      error => {
        if (!error.message) return error;
        /** @type {?} */
        const index = error.message.indexOf('[');
        if (index > -1) {
          return Object.assign({}, error, {
            message: error.message.slice(0, index),
            interpoliteParams: error.message.slice(index + 1, error.message.length - 1).split(','),
          });
        }
        return error;
      },
    );
  }
}
ValidationErrorComponent.decorators = [
  {
    type: Component,
    args: [
      {
        selector: 'abp-validation-error',
        template: `
    <div class="invalid-feedback" *ngFor="let error of abpErrors; trackBy: trackByFn">
      {{ error.message | abpLocalization: error.interpoliteParams }}
    </div>
  `,
        changeDetection: ChangeDetectionStrategy.OnPush,
        encapsulation: ViewEncapsulation.None,
      },
    ],
  },
];

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var styles = `
.content-header-title {
    font-size: 24px;
}

.entry-row {
    margin-bottom: 15px;
}

#main-navbar-tools a.dropdown-toggle {
    text-decoration: none;
    color: #fff;
}

.navbar .dropdown-submenu {
    position: relative;
}
.navbar .dropdown-menu {
    margin: 0;
    padding: 0;
}
    .navbar .dropdown-menu a {
        font-size: .9em;
        padding: 10px 15px;
        display: block;
        min-width: 210px;
        text-align: left;
        border-radius: 0.25rem;
        min-height: 44px;
    }
.navbar .dropdown-submenu a::after {
    transform: rotate(-90deg);
    position: absolute;
    right: 16px;
    top: 18px;
}
.navbar .dropdown-submenu .dropdown-menu {
    top: 0;
    left: 100%;
}

.card-header .btn {
    padding: 2px 6px;
}
.card-header h5 {
    margin: 0;
}
.container > .card {
    box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075) !important;
}

@media screen and (min-width: 768px) {
    .navbar .dropdown:hover > .dropdown-menu {
        display: block;
    }

    .navbar .dropdown-submenu:hover > .dropdown-menu {
        display: block;
    }
}
.input-validation-error {
    border-color: #dc3545;
}
.field-validation-error {
    font-size: 0.8em;
}


.abp-main-nav-dropdown {
  margin-top: -50%;
}
`;

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class InitialService {
  /**
   * @param {?} lazyLoadService
   */
  constructor(lazyLoadService) {
    this.lazyLoadService = lazyLoadService;
    this.appendStyle().subscribe();
  }
  /**
   * @return {?}
   */
  appendStyle() {
    return this.lazyLoadService.load(null, 'style', styles, 'head', 'afterbegin');
  }
}
InitialService.decorators = [{ type: Injectable, args: [{ providedIn: 'root' }] }];
/** @nocollapse */
InitialService.ctorParameters = () => [{ type: LazyLoadService }];
/** @nocollapse */ InitialService.ngInjectableDef = ɵɵdefineInjectable({
  factory: function InitialService_Factory() {
    return new InitialService(ɵɵinject(LazyLoadService));
  },
  token: InitialService,
  providedIn: 'root',
});
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
const LAYOUTS = [ApplicationLayoutComponent, AccountLayoutComponent, EmptyLayoutComponent];
class ThemeBasicModule {
  /**
   * @param {?} initialService
   */
  constructor(initialService) {
    this.initialService = initialService;
  }
}
ThemeBasicModule.decorators = [
  {
    type: NgModule,
    args: [
      {
        declarations: [...LAYOUTS, ValidationErrorComponent],
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
        exports: [...LAYOUTS],
        entryComponents: [...LAYOUTS, ValidationErrorComponent],
      },
    ],
  },
];
/** @nocollapse */
ThemeBasicModule.ctorParameters = () => [{ type: InitialService }];
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
