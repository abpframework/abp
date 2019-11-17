import { __spread, __assign, __decorate, __metadata, __extends } from 'tslib';
import { ConfigState, SessionState, takeUntilDestroy, SetLanguage, GetAppConfiguration, LazyLoadService, CoreModule } from '@abp/ng.core';
import { slideFromBottom, collapseWithMargin, ThemeSharedModule } from '@abp/ng.theme.shared';
import { Component, Renderer2, ViewChild, TemplateRef, ChangeDetectionStrategy, ViewEncapsulation, Injectable, ɵɵdefineInjectable, ɵɵinject, NgModule } from '@angular/core';
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
 * Generated from: lib/components/account-layout/account-layout.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var AccountLayoutComponent = /** @class */ (function () {
    function AccountLayoutComponent() {
    }
    // required for dynamic component
    AccountLayoutComponent.type = "account" /* account */;
    AccountLayoutComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-layout-account',
                    template: "\n    <router-outlet></router-outlet>\n    <abp-confirmation></abp-confirmation>\n    <abp-toast></abp-toast>\n  "
                }] }
    ];
    return AccountLayoutComponent;
}());
if (false) {
    /** @type {?} */
    AccountLayoutComponent.type;
}

/**
 * @fileoverview added by tsickle
 * Generated from: lib/actions/layout.actions.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var AddNavigationElement = /** @class */ (function () {
    function AddNavigationElement(payload) {
        this.payload = payload;
    }
    AddNavigationElement.type = '[Layout] Add Navigation Element';
    return AddNavigationElement;
}());
if (false) {
    /** @type {?} */
    AddNavigationElement.type;
    /** @type {?} */
    AddNavigationElement.prototype.payload;
}
var RemoveNavigationElementByName = /** @class */ (function () {
    function RemoveNavigationElementByName(name) {
        this.name = name;
    }
    RemoveNavigationElementByName.type = '[Layout] Remove Navigation ElementByName';
    return RemoveNavigationElementByName;
}());
if (false) {
    /** @type {?} */
    RemoveNavigationElementByName.type;
    /** @type {?} */
    RemoveNavigationElementByName.prototype.name;
}

/**
 * @fileoverview added by tsickle
 * Generated from: lib/actions/index.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * Generated from: lib/states/layout.state.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var LayoutState = /** @class */ (function () {
    function LayoutState() {
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    LayoutState.getNavigationElements = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var navigationElements = _a.navigationElements;
        return navigationElements;
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    LayoutState.prototype.layoutAddAction = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var getState = _a.getState, patchState = _a.patchState;
        var _c = _b.payload, payload = _c === void 0 ? [] : _c;
        var navigationElements = getState().navigationElements;
        if (!Array.isArray(payload)) {
            payload = [payload];
        }
        if (navigationElements.length) {
            payload = snq((/**
             * @return {?}
             */
            function () {
                return ((/** @type {?} */ (payload))).filter((/**
                 * @param {?} __0
                 * @return {?}
                 */
                function (_a) {
                    var name = _a.name;
                    return navigationElements.findIndex((/**
                     * @param {?} nav
                     * @return {?}
                     */
                    function (nav) { return nav.name === name; })) < 0;
                }));
            }), []);
        }
        if (!payload.length)
            return;
        navigationElements = __spread(navigationElements, payload).map((/**
         * @param {?} element
         * @return {?}
         */
        function (element) { return (__assign({}, element, { order: element.order || 99 })); }))
            .sort((/**
         * @param {?} a
         * @param {?} b
         * @return {?}
         */
        function (a, b) { return a.order - b.order; }));
        return patchState({
            navigationElements: navigationElements,
        });
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    LayoutState.prototype.layoutRemoveAction = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var getState = _a.getState, patchState = _a.patchState;
        var name = _b.name;
        var navigationElements = getState().navigationElements;
        /** @type {?} */
        var index = navigationElements.findIndex((/**
         * @param {?} element
         * @return {?}
         */
        function (element) { return element.name === name; }));
        if (index > -1) {
            navigationElements = navigationElements.splice(index, 1);
        }
        return patchState({
            navigationElements: navigationElements,
        });
    };
    __decorate([
        Action(AddNavigationElement),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object, AddNavigationElement]),
        __metadata("design:returntype", void 0)
    ], LayoutState.prototype, "layoutAddAction", null);
    __decorate([
        Action(RemoveNavigationElementByName),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object, RemoveNavigationElementByName]),
        __metadata("design:returntype", void 0)
    ], LayoutState.prototype, "layoutRemoveAction", null);
    __decorate([
        Selector(),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object]),
        __metadata("design:returntype", Array)
    ], LayoutState, "getNavigationElements", null);
    LayoutState = __decorate([
        State({
            name: 'LayoutState',
            defaults: (/** @type {?} */ ({ navigationElements: [] })),
        })
    ], LayoutState);
    return LayoutState;
}());

/**
 * @fileoverview added by tsickle
 * Generated from: lib/states/index.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/application-layout/application-layout.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var ApplicationLayoutComponent = /** @class */ (function () {
    function ApplicationLayoutComponent(store, oauthService, renderer) {
        this.store = store;
        this.oauthService = oauthService;
        this.renderer = renderer;
        this.isCollapsed = true;
        this.rightPartElements = [];
        this.trackByFn = (/**
         * @param {?} _
         * @param {?} item
         * @return {?}
         */
        function (_, item) { return item.name; });
        this.trackElementByFn = (/**
         * @param {?} _
         * @param {?} element
         * @return {?}
         */
        function (_, element) { return element; });
    }
    Object.defineProperty(ApplicationLayoutComponent.prototype, "appInfo", {
        get: 
        // do not set true or false
        /**
         * @return {?}
         */
        function () {
            return this.store.selectSnapshot(ConfigState.getApplicationInfo);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ApplicationLayoutComponent.prototype, "visibleRoutes$", {
        get: /**
         * @return {?}
         */
        function () {
            return this.routes$.pipe(map((/**
             * @param {?} routes
             * @return {?}
             */
            function (routes) { return getVisibleRoutes(routes); })));
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ApplicationLayoutComponent.prototype, "defaultLanguage$", {
        get: /**
         * @return {?}
         */
        function () {
            var _this = this;
            return this.languages$.pipe(map((/**
             * @param {?} languages
             * @return {?}
             */
            function (languages) { return snq((/**
             * @return {?}
             */
            function () { return languages.find((/**
             * @param {?} lang
             * @return {?}
             */
            function (lang) { return lang.cultureName === _this.selectedLangCulture; })).displayName; })); }), ''));
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ApplicationLayoutComponent.prototype, "dropdownLanguages$", {
        get: /**
         * @return {?}
         */
        function () {
            var _this = this;
            return this.languages$.pipe(map((/**
             * @param {?} languages
             * @return {?}
             */
            function (languages) { return snq((/**
             * @return {?}
             */
            function () { return languages.filter((/**
             * @param {?} lang
             * @return {?}
             */
            function (lang) { return lang.cultureName !== _this.selectedLangCulture; })); })); }), []));
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ApplicationLayoutComponent.prototype, "selectedLangCulture", {
        get: /**
         * @return {?}
         */
        function () {
            return this.store.selectSnapshot(SessionState.getLanguage);
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @private
     * @return {?}
     */
    ApplicationLayoutComponent.prototype.checkWindowWidth = /**
     * @private
     * @return {?}
     */
    function () {
        var _this = this;
        setTimeout((/**
         * @return {?}
         */
        function () {
            if (window.innerWidth < 768) {
                _this.isDropdownChildDynamic = false;
                if (_this.smallScreen === false) {
                    _this.isCollapsed = false;
                    setTimeout((/**
                     * @return {?}
                     */
                    function () {
                        _this.isCollapsed = true;
                    }), 100);
                }
                _this.smallScreen = true;
            }
            else {
                _this.isDropdownChildDynamic = true;
                _this.smallScreen = false;
            }
        }), 0);
    };
    /**
     * @return {?}
     */
    ApplicationLayoutComponent.prototype.ngAfterViewInit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        /** @type {?} */
        var navigations = this.store.selectSnapshot(LayoutState.getNavigationElements).map((/**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var name = _a.name;
            return name;
        }));
        if (navigations.indexOf('LanguageRef') < 0) {
            this.store.dispatch(new AddNavigationElement([
                { element: this.languageRef, order: 4, name: 'LanguageRef' },
                { element: this.currentUserRef, order: 5, name: 'CurrentUserRef' },
            ]));
        }
        this.navElements$
            .pipe(map((/**
         * @param {?} elements
         * @return {?}
         */
        function (elements) { return elements.map((/**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var element = _a.element;
            return element;
        })); })), filter((/**
         * @param {?} elements
         * @return {?}
         */
        function (elements) { return !compare(elements, _this.rightPartElements); })), takeUntilDestroy(this))
            .subscribe((/**
         * @param {?} elements
         * @return {?}
         */
        function (elements) {
            setTimeout((/**
             * @return {?}
             */
            function () { return (_this.rightPartElements = elements); }), 0);
        }));
        this.checkWindowWidth();
        fromEvent(window, 'resize')
            .pipe(takeUntilDestroy(this), debounceTime(150))
            .subscribe((/**
         * @return {?}
         */
        function () {
            _this.checkWindowWidth();
        }));
    };
    /**
     * @return {?}
     */
    ApplicationLayoutComponent.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () { };
    /**
     * @param {?} cultureName
     * @return {?}
     */
    ApplicationLayoutComponent.prototype.onChangeLang = /**
     * @param {?} cultureName
     * @return {?}
     */
    function (cultureName) {
        this.store.dispatch(new SetLanguage(cultureName));
    };
    /**
     * @return {?}
     */
    ApplicationLayoutComponent.prototype.logout = /**
     * @return {?}
     */
    function () {
        this.oauthService.logOut();
        this.store.dispatch(new Navigate(['/'], null, {
            state: { redirectUrl: this.store.selectSnapshot(RouterState).state.url },
        }));
        this.store.dispatch(new GetAppConfiguration());
    };
    /**
     * @param {?} event
     * @param {?} childrenContainer
     * @return {?}
     */
    ApplicationLayoutComponent.prototype.openChange = /**
     * @param {?} event
     * @param {?} childrenContainer
     * @return {?}
     */
    function (event, childrenContainer) {
        var _this = this;
        if (!event) {
            Object.keys(childrenContainer.style)
                .filter((/**
             * @param {?} key
             * @return {?}
             */
            function (key) { return Number.isInteger(+key); }))
                .forEach((/**
             * @param {?} key
             * @return {?}
             */
            function (key) {
                _this.renderer.removeStyle(childrenContainer, childrenContainer.style[key]);
            }));
            this.renderer.removeStyle(childrenContainer, 'left');
        }
    };
    // required for dynamic component
    ApplicationLayoutComponent.type = "application" /* application */;
    ApplicationLayoutComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-layout-application',
                    template: "<nav\r\n  class=\"navbar navbar-expand-md navbar-dark bg-dark shadow-sm flex-column flex-md-row mb-4\"\r\n  id=\"main-navbar\"\r\n  style=\"min-height: 4rem;\"\r\n>\r\n  <div class=\"container \">\r\n    <a class=\"navbar-brand\" routerLink=\"/\">\r\n      <img *ngIf=\"appInfo.logoUrl; else appName\" [src]=\"appInfo.logoUrl\" [alt]=\"appInfo.name\" />\r\n    </a>\r\n    <button\r\n      class=\"navbar-toggler\"\r\n      type=\"button\"\r\n      [attr.aria-expanded]=\"!isCollapsed\"\r\n      (click)=\"isCollapsed = !isCollapsed\"\r\n    >\r\n      <span class=\"navbar-toggler-icon\"></span>\r\n    </button>\r\n    <div class=\"navbar-collapse\" [class.overflow-hidden]=\"smallScreen\" id=\"main-navbar-collapse\">\r\n      <ng-container *ngTemplateOutlet=\"!smallScreen ? navigations : null\"></ng-container>\r\n\r\n      <div *ngIf=\"smallScreen\" [@collapseWithMargin]=\"isCollapsed ? 'collapsed' : 'expanded'\">\r\n        <ng-container *ngTemplateOutlet=\"navigations\"></ng-container>\r\n      </div>\r\n\r\n      <ng-template #navigations>\r\n        <ul class=\"navbar-nav mx-auto\">\r\n          <ng-container\r\n            *ngFor=\"let route of visibleRoutes$ | async; trackBy: trackByFn\"\r\n            [ngTemplateOutlet]=\"route?.children?.length ? dropdownLink : defaultLink\"\r\n            [ngTemplateOutletContext]=\"{ $implicit: route }\"\r\n          >\r\n          </ng-container>\r\n\r\n          <ng-template #defaultLink let-route>\r\n            <li class=\"nav-item\" [abpPermission]=\"route.requiredPolicy\">\r\n              <a class=\"nav-link\" [routerLink]=\"[route.url]\"\r\n                ><i *ngIf=\"route.iconClass\" [ngClass]=\"route.iconClass\"></i> {{ route.name | abpLocalization }}</a\r\n              >\r\n            </li>\r\n          </ng-template>\r\n\r\n          <ng-template #dropdownLink let-route>\r\n            <li\r\n              #navbarRootDropdown\r\n              [abpPermission]=\"route.requiredPolicy\"\r\n              [abpVisibility]=\"routeContainer\"\r\n              class=\"nav-item dropdown\"\r\n              display=\"static\"\r\n              (click)=\"\r\n                navbarRootDropdown.expand ? (navbarRootDropdown.expand = false) : (navbarRootDropdown.expand = true)\r\n              \"\r\n            >\r\n              <a\r\n                class=\"nav-link dropdown-toggle\"\r\n                data-toggle=\"dropdown\"\r\n                aria-haspopup=\"true\"\r\n                aria-expanded=\"false\"\r\n                href=\"javascript:void(0)\"\r\n              >\r\n                <i *ngIf=\"route.iconClass\" [ngClass]=\"route.iconClass\"></i> {{ route.name | abpLocalization }}\r\n              </a>\r\n              <div\r\n                #routeContainer\r\n                class=\"dropdown-menu border-0 shadow-sm\"\r\n                (click)=\"$event.preventDefault(); $event.stopPropagation()\"\r\n                [class.abp-collapsed-height]=\"smallScreen\"\r\n                [class.d-block]=\"smallScreen\"\r\n                [class.abp-mh-25]=\"smallScreen && navbarRootDropdown.expand\"\r\n              >\r\n                <ng-template\r\n                  #forTemplate\r\n                  ngFor\r\n                  [ngForOf]=\"route.children\"\r\n                  [ngForTrackBy]=\"trackByFn\"\r\n                  [ngForTemplate]=\"childWrapper\"\r\n                ></ng-template>\r\n              </div>\r\n            </li>\r\n          </ng-template>\r\n\r\n          <ng-template #childWrapper let-child>\r\n            <ng-template\r\n              [ngTemplateOutlet]=\"child?.children?.length ? dropdownChild : defaultChild\"\r\n              [ngTemplateOutletContext]=\"{ $implicit: child }\"\r\n            ></ng-template>\r\n          </ng-template>\r\n\r\n          <ng-template #defaultChild let-child>\r\n            <div class=\"dropdown-submenu\" [abpPermission]=\"child.requiredPolicy\">\r\n              <a class=\"dropdown-item\" [routerLink]=\"[child.url]\">\r\n                <i *ngIf=\"child.iconClass\" [ngClass]=\"child.iconClass\"></i>\r\n                {{ child.name | abpLocalization }}</a\r\n              >\r\n            </div>\r\n          </ng-template>\r\n\r\n          <ng-template #dropdownChild let-child>\r\n            <div\r\n              [abpVisibility]=\"childrenContainer\"\r\n              class=\"dropdown-submenu\"\r\n              ngbDropdown\r\n              #dropdownSubmenu=\"ngbDropdown\"\r\n              [display]=\"isDropdownChildDynamic ? 'dynamic' : 'static'\"\r\n              placement=\"right-top\"\r\n              [autoClose]=\"true\"\r\n              [abpPermission]=\"child.requiredPolicy\"\r\n              (openChange)=\"openChange($event, childrenContainer)\"\r\n            >\r\n              <div ngbDropdownToggle [class.dropdown-toggle]=\"false\">\r\n                <a\r\n                  abpEllipsis=\"210px\"\r\n                  [abpEllipsisEnabled]=\"isDropdownChildDynamic\"\r\n                  role=\"button\"\r\n                  class=\"btn d-block text-left dropdown-toggle\"\r\n                >\r\n                  <i *ngIf=\"child.iconClass\" [ngClass]=\"child.iconClass\"></i>\r\n                  {{ child.name | abpLocalization }}\r\n                </a>\r\n              </div>\r\n              <div\r\n                #childrenContainer\r\n                class=\"dropdown-menu border-0 shadow-sm\"\r\n                [class.abp-collapsed-height]=\"smallScreen\"\r\n                [class.d-block]=\"smallScreen\"\r\n                [class.abp-mh-25]=\"smallScreen && dropdownSubmenu.isOpen()\"\r\n              >\r\n                <ng-template\r\n                  ngFor\r\n                  [ngForOf]=\"child.children\"\r\n                  [ngForTrackBy]=\"trackByFn\"\r\n                  [ngForTemplate]=\"childWrapper\"\r\n                ></ng-template>\r\n              </div>\r\n            </div>\r\n          </ng-template>\r\n        </ul>\r\n\r\n        <ul class=\"navbar-nav\">\r\n          <ng-container\r\n            *ngFor=\"let element of rightPartElements; trackBy: trackElementByFn\"\r\n            [ngTemplateOutlet]=\"element\"\r\n          ></ng-container>\r\n        </ul>\r\n      </ng-template>\r\n    </div>\r\n  </div>\r\n</nav>\r\n\r\n<div [@slideFromBottom]=\"outlet && outlet.activatedRoute && outlet.activatedRoute.routeConfig.path\" class=\"container\">\r\n  <router-outlet #outlet=\"outlet\"></router-outlet>\r\n</div>\r\n\r\n<abp-confirmation></abp-confirmation>\r\n<abp-toast></abp-toast>\r\n\r\n<ng-template #appName>\r\n  {{ appInfo.name }}\r\n</ng-template>\r\n\r\n<ng-template #language>\r\n  <li class=\"nav-item\">\r\n    <div class=\"dropdown\" ngbDropdown #languageDropdown=\"ngbDropdown\" display=\"static\">\r\n      <a\r\n        ngbDropdownToggle\r\n        class=\"nav-link\"\r\n        href=\"javascript:void(0)\"\r\n        role=\"button\"\r\n        id=\"dropdownMenuLink\"\r\n        data-toggle=\"dropdown\"\r\n        aria-haspopup=\"true\"\r\n        aria-expanded=\"false\"\r\n      >\r\n        {{ defaultLanguage$ | async }}\r\n      </a>\r\n      <div\r\n        class=\"dropdown-menu dropdown-menu-right border-0 shadow-sm\"\r\n        aria-labelledby=\"dropdownMenuLink\"\r\n        [class.abp-collapsed-height]=\"smallScreen\"\r\n        [class.d-block]=\"smallScreen\"\r\n        [class.abp-mh-25]=\"smallScreen && languageDropdown.isOpen()\"\r\n      >\r\n        <a\r\n          *ngFor=\"let lang of dropdownLanguages$ | async\"\r\n          href=\"javascript:void(0)\"\r\n          class=\"dropdown-item\"\r\n          (click)=\"onChangeLang(lang.cultureName)\"\r\n          >{{ lang?.displayName }}</a\r\n        >\r\n      </div>\r\n    </div>\r\n  </li>\r\n</ng-template>\r\n\r\n<ng-template #currentUser>\r\n  <li *ngIf=\"(currentUser$ | async)?.isAuthenticated\" class=\"nav-item\">\r\n    <div ngbDropdown class=\"dropdown\" #currentUserDropdown=\"ngbDropdown\" display=\"static\">\r\n      <a\r\n        ngbDropdownToggle\r\n        class=\"nav-link\"\r\n        href=\"javascript:void(0)\"\r\n        role=\"button\"\r\n        id=\"dropdownMenuLink\"\r\n        data-toggle=\"dropdown\"\r\n        aria-haspopup=\"true\"\r\n        aria-expanded=\"false\"\r\n      >\r\n        {{ (currentUser$ | async)?.userName }}\r\n      </a>\r\n      <div\r\n        class=\"dropdown-menu dropdown-menu-right border-0 shadow-sm\"\r\n        aria-labelledby=\"dropdownMenuLink\"\r\n        [class.abp-collapsed-height]=\"smallScreen\"\r\n        [class.d-block]=\"smallScreen\"\r\n        [class.abp-mh-25]=\"smallScreen && currentUserDropdown.isOpen()\"\r\n      >\r\n        <a class=\"dropdown-item\" routerLink=\"/account/manage-profile\">{{\r\n          'AbpAccount::ManageYourProfile' | abpLocalization\r\n        }}</a>\r\n        <a class=\"dropdown-item\" href=\"javascript:void(0)\" (click)=\"logout()\">{{\r\n          'AbpUi::Logout' | abpLocalization\r\n        }}</a>\r\n      </div>\r\n    </div>\r\n  </li>\r\n</ng-template>\r\n",
                    animations: [slideFromBottom, collapseWithMargin]
                }] }
    ];
    /** @nocollapse */
    ApplicationLayoutComponent.ctorParameters = function () { return [
        { type: Store },
        { type: OAuthService },
        { type: Renderer2 }
    ]; };
    ApplicationLayoutComponent.propDecorators = {
        currentUserRef: [{ type: ViewChild, args: ['currentUser', { static: false, read: TemplateRef },] }],
        languageRef: [{ type: ViewChild, args: ['language', { static: false, read: TemplateRef },] }]
    };
    __decorate([
        Select(ConfigState.getOne('routes')),
        __metadata("design:type", Observable)
    ], ApplicationLayoutComponent.prototype, "routes$", void 0);
    __decorate([
        Select(ConfigState.getOne('currentUser')),
        __metadata("design:type", Observable)
    ], ApplicationLayoutComponent.prototype, "currentUser$", void 0);
    __decorate([
        Select(ConfigState.getDeep('localization.languages')),
        __metadata("design:type", Observable)
    ], ApplicationLayoutComponent.prototype, "languages$", void 0);
    __decorate([
        Select(LayoutState.getNavigationElements),
        __metadata("design:type", Observable)
    ], ApplicationLayoutComponent.prototype, "navElements$", void 0);
    return ApplicationLayoutComponent;
}());
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
    return routes.reduce((/**
     * @param {?} acc
     * @param {?} val
     * @return {?}
     */
    function (acc, val) {
        if (val.invisible)
            return acc;
        if (val.children && val.children.length) {
            val.children = getVisibleRoutes(val.children);
        }
        return __spread(acc, [val]);
    }), []);
}

/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/empty-layout/empty-layout.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var EmptyLayoutComponent = /** @class */ (function () {
    function EmptyLayoutComponent() {
    }
    EmptyLayoutComponent.type = "empty" /* empty */;
    EmptyLayoutComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-layout-empty',
                    template: "\n    <router-outlet></router-outlet>\n    <abp-confirmation></abp-confirmation>\n    <abp-toast></abp-toast>\n  "
                }] }
    ];
    return EmptyLayoutComponent;
}());
if (false) {
    /** @type {?} */
    EmptyLayoutComponent.type;
}

/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/validation-error/validation-error.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var ValidationErrorComponent = /** @class */ (function (_super) {
    __extends(ValidationErrorComponent, _super);
    function ValidationErrorComponent() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    Object.defineProperty(ValidationErrorComponent.prototype, "abpErrors", {
        get: /**
         * @return {?}
         */
        function () {
            if (!this.errors || !this.errors.length)
                return [];
            return this.errors.map((/**
             * @param {?} error
             * @return {?}
             */
            function (error) {
                if (!error.message)
                    return error;
                /** @type {?} */
                var index = error.message.indexOf('[');
                if (index > -1) {
                    return __assign({}, error, { message: error.message.slice(0, index), interpoliteParams: error.message.slice(index + 1, error.message.length - 1).split(',') });
                }
                return error;
            }));
        },
        enumerable: true,
        configurable: true
    });
    ValidationErrorComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-validation-error',
                    template: "\n    <div class=\"invalid-feedback\" *ngFor=\"let error of abpErrors; trackBy: trackByFn\">\n      {{ error.message | abpLocalization: error.interpoliteParams }}\n    </div>\n  ",
                    changeDetection: ChangeDetectionStrategy.OnPush,
                    encapsulation: ViewEncapsulation.None
                }] }
    ];
    return ValidationErrorComponent;
}(ValidationErrorComponent$1));

/**
 * @fileoverview added by tsickle
 * Generated from: lib/constants/styles.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var styles = "\n.content-header-title {\n    font-size: 24px;\n}\n\n.entry-row {\n    margin-bottom: 15px;\n}\n\n#main-navbar-tools a.dropdown-toggle {\n    text-decoration: none;\n    color: #fff;\n}\n\n.navbar .dropdown-submenu {\n    position: relative;\n}\n.navbar .dropdown-menu {\n    margin: 0;\n    padding: 0;\n}\n    .navbar .dropdown-menu a {\n        font-size: .9em;\n        padding: 10px 15px;\n        display: block;\n        min-width: 210px;\n        text-align: left;\n        border-radius: 0.25rem;\n        min-height: 44px;\n    }\n.navbar .dropdown-submenu a::after {\n    transform: rotate(-90deg);\n    position: absolute;\n    right: 16px;\n    top: 18px;\n}\n.navbar .dropdown-submenu .dropdown-menu {\n    top: 0;\n    left: 100%;\n}\n\n.card-header .btn {\n    padding: 2px 6px;\n}\n.card-header h5 {\n    margin: 0;\n}\n.container > .card {\n    box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075) !important;\n}\n\n@media screen and (min-width: 768px) {\n    .navbar .dropdown:hover > .dropdown-menu {\n        display: block;\n    }\n\n    .navbar .dropdown-submenu:hover > .dropdown-menu {\n        display: block;\n    }\n}\n.input-validation-error {\n    border-color: #dc3545;\n}\n.field-validation-error {\n    font-size: 0.8em;\n}\n";

/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/initial.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var InitialService = /** @class */ (function () {
    function InitialService(lazyLoadService) {
        this.lazyLoadService = lazyLoadService;
        this.appendStyle().subscribe();
    }
    /**
     * @return {?}
     */
    InitialService.prototype.appendStyle = /**
     * @return {?}
     */
    function () {
        return this.lazyLoadService.load(null, 'style', styles, 'head', 'afterbegin');
    };
    InitialService.decorators = [
        { type: Injectable, args: [{ providedIn: 'root' },] }
    ];
    /** @nocollapse */
    InitialService.ctorParameters = function () { return [
        { type: LazyLoadService }
    ]; };
    /** @nocollapse */ InitialService.ngInjectableDef = ɵɵdefineInjectable({ factory: function InitialService_Factory() { return new InitialService(ɵɵinject(LazyLoadService)); }, token: InitialService, providedIn: "root" });
    return InitialService;
}());
if (false) {
    /**
     * @type {?}
     * @private
     */
    InitialService.prototype.lazyLoadService;
}

/**
 * @fileoverview added by tsickle
 * Generated from: lib/theme-basic.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/** @type {?} */
var LAYOUTS = [ApplicationLayoutComponent, AccountLayoutComponent, EmptyLayoutComponent];
var ThemeBasicModule = /** @class */ (function () {
    function ThemeBasicModule(initialService) {
        this.initialService = initialService;
    }
    ThemeBasicModule.decorators = [
        { type: NgModule, args: [{
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
                                maxlength: 'AbpAccount::ThisFieldMustBeAStringOrArrayTypeWithAMaximumLengthoOf{0}[{{ requiredLength }}]',
                                min: 'AbpAccount::ThisFieldMustBeBetween{0}And{1}[{{ min }},{{ max }}]',
                                minlength: 'AbpAccount::ThisFieldMustBeAStringOrArrayTypeWithAMinimumLengthOf{0}[{{ requiredLength }}]',
                                required: 'AbpAccount::ThisFieldIsRequired.',
                                passwordMismatch: 'AbpIdentity::Identity.PasswordConfirmationFailed',
                            },
                            errorTemplate: ValidationErrorComponent,
                        }),
                    ],
                    exports: __spread(LAYOUTS),
                    entryComponents: __spread(LAYOUTS, [ValidationErrorComponent]),
                },] }
    ];
    /** @nocollapse */
    ThemeBasicModule.ctorParameters = function () { return [
        { type: InitialService }
    ]; };
    return ThemeBasicModule;
}());
if (false) {
    /**
     * @type {?}
     * @private
     */
    ThemeBasicModule.prototype.initialService;
}

/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/index.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * Generated from: lib/models/layout.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var Layout;
(function (Layout) {
    /**
     * @record
     */
    function State() { }
    Layout.State = State;
    if (false) {
        /** @type {?} */
        State.prototype.navigationElements;
    }
    /**
     * @record
     */
    function NavigationElement() { }
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
 * Generated from: lib/models/index.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * Generated from: public-api.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * Generated from: abp-ng.theme.basic.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

export { AccountLayoutComponent, AddNavigationElement, ApplicationLayoutComponent, EmptyLayoutComponent, LAYOUTS, LayoutState, RemoveNavigationElementByName, ThemeBasicModule, ValidationErrorComponent, ApplicationLayoutComponent as ɵa, LayoutState as ɵb, AccountLayoutComponent as ɵc, EmptyLayoutComponent as ɵd, ValidationErrorComponent as ɵe, LayoutState as ɵf, AddNavigationElement as ɵg, RemoveNavigationElementByName as ɵh, InitialService as ɵj };
//# sourceMappingURL=abp-ng.theme.basic.js.map
