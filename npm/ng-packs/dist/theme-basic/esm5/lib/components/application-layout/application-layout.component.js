/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/application-layout/application-layout.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { ConfigState, GetAppConfiguration, SessionState, SetLanguage, takeUntilDestroy, } from '@abp/ng.core';
import { collapseWithMargin, slideFromBottom } from '@abp/ng.theme.shared';
import { Component, Renderer2, TemplateRef, ViewChild, } from '@angular/core';
import { Navigate, RouterState } from '@ngxs/router-plugin';
import { Select, Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import compare from 'just-compare';
import { fromEvent, Observable } from 'rxjs';
import { debounceTime, filter, map } from 'rxjs/operators';
import snq from 'snq';
import { AddNavigationElement } from '../../actions';
import { LayoutState } from '../../states';
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
    tslib_1.__decorate([
        Select(ConfigState.getOne('routes')),
        tslib_1.__metadata("design:type", Observable)
    ], ApplicationLayoutComponent.prototype, "routes$", void 0);
    tslib_1.__decorate([
        Select(ConfigState.getOne('currentUser')),
        tslib_1.__metadata("design:type", Observable)
    ], ApplicationLayoutComponent.prototype, "currentUser$", void 0);
    tslib_1.__decorate([
        Select(ConfigState.getDeep('localization.languages')),
        tslib_1.__metadata("design:type", Observable)
    ], ApplicationLayoutComponent.prototype, "languages$", void 0);
    tslib_1.__decorate([
        Select(LayoutState.getNavigationElements),
        tslib_1.__metadata("design:type", Observable)
    ], ApplicationLayoutComponent.prototype, "navElements$", void 0);
    return ApplicationLayoutComponent;
}());
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
        return tslib_1.__spread(acc, [val]);
    }), []);
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9hcHBsaWNhdGlvbi1sYXlvdXQvYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBSUwsV0FBVyxFQUVYLG1CQUFtQixFQUNuQixZQUFZLEVBQ1osV0FBVyxFQUNYLGdCQUFnQixHQUNqQixNQUFNLGNBQWMsQ0FBQztBQUN0QixPQUFPLEVBQUUsa0JBQWtCLEVBQUUsZUFBZSxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDM0UsT0FBTyxFQUVMLFNBQVMsRUFHVCxTQUFTLEVBQ1QsV0FBVyxFQUVYLFNBQVMsR0FHVixNQUFNLGVBQWUsQ0FBQztBQUV2QixPQUFPLEVBQUUsUUFBUSxFQUFFLFdBQVcsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQzVELE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUNuRCxPQUFPLE9BQU8sTUFBTSxjQUFjLENBQUM7QUFDbkMsT0FBTyxFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDN0MsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDM0QsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBQ3RCLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUVyRCxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBRTNDO0lBa0VFLG9DQUFvQixLQUFZLEVBQVUsWUFBMEIsRUFBVSxRQUFtQjtRQUE3RSxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQVUs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LGdEQUM0RDs7SUFFNUQsa0RBQ3FEOztJQUVyRCxvREFDaUM7O0lBRWpDLGlEQUM4Qjs7SUFFOUIsNERBQWdDOztJQUVoQyxpREFBbUI7O0lBRW5CLGlEQUFxQjs7SUE2QnJCLHVEQUEyQzs7SUFFM0MsK0NBQW1FOztJQUVuRSxzREFBMkU7Ozs7O0lBRS9ELDJDQUFvQjs7Ozs7SUFBRSxrREFBa0M7Ozs7O0lBQUUsOENBQTJCOzs7Ozs7QUFrRm5HLFNBQVMsZ0JBQWdCLENBQUMsTUFBdUI7SUFDL0MsT0FBTyxNQUFNLENBQUMsTUFBTTs7Ozs7SUFBQyxVQUFDLEdBQUcsRUFBRSxHQUFHO1FBQzVCLElBQUksR0FBRyxDQUFDLFNBQVM7WUFBRSxPQUFPLEdBQUcsQ0FBQztRQUU5QixJQUFJLEdBQUcsQ0FBQyxRQUFRLElBQUksR0FBRyxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDdkMsR0FBRyxDQUFDLFFBQVEsR0FBRyxnQkFBZ0IsQ0FBQyxHQUFHLENBQUMsUUFBUSxDQUFDLENBQUM7U0FDL0M7UUFFRCx3QkFBVyxHQUFHLEdBQUUsR0FBRyxHQUFFO0lBQ3ZCLENBQUMsR0FBRSxFQUFFLENBQUMsQ0FBQztBQUNULENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQge1xyXG4gIEFCUCxcclxuICBBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24sXHJcbiAgQ29uZmlnLFxyXG4gIENvbmZpZ1N0YXRlLFxyXG4gIGVMYXlvdXRUeXBlLFxyXG4gIEdldEFwcENvbmZpZ3VyYXRpb24sXHJcbiAgU2Vzc2lvblN0YXRlLFxyXG4gIFNldExhbmd1YWdlLFxyXG4gIHRha2VVbnRpbERlc3Ryb3ksXHJcbn0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHsgY29sbGFwc2VXaXRoTWFyZ2luLCBzbGlkZUZyb21Cb3R0b20gfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XHJcbmltcG9ydCB7XHJcbiAgQWZ0ZXJWaWV3SW5pdCxcclxuICBDb21wb25lbnQsXHJcbiAgT25EZXN0cm95LFxyXG4gIFF1ZXJ5TGlzdCxcclxuICBSZW5kZXJlcjIsXHJcbiAgVGVtcGxhdGVSZWYsXHJcbiAgVHJhY2tCeUZ1bmN0aW9uLFxyXG4gIFZpZXdDaGlsZCxcclxuICBWaWV3Q2hpbGRyZW4sXHJcbiAgRWxlbWVudFJlZixcclxufSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgTmdiRHJvcGRvd24gfSBmcm9tICdAbmctYm9vdHN0cmFwL25nLWJvb3RzdHJhcCc7XHJcbmltcG9ydCB7IE5hdmlnYXRlLCBSb3V0ZXJTdGF0ZSB9IGZyb20gJ0BuZ3hzL3JvdXRlci1wbHVnaW4nO1xyXG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBPQXV0aFNlcnZpY2UgfSBmcm9tICdhbmd1bGFyLW9hdXRoMi1vaWRjJztcclxuaW1wb3J0IGNvbXBhcmUgZnJvbSAnanVzdC1jb21wYXJlJztcclxuaW1wb3J0IHsgZnJvbUV2ZW50LCBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XHJcbmltcG9ydCB7IGRlYm91bmNlVGltZSwgZmlsdGVyLCBtYXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XHJcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcclxuaW1wb3J0IHsgQWRkTmF2aWdhdGlvbkVsZW1lbnQgfSBmcm9tICcuLi8uLi9hY3Rpb25zJztcclxuaW1wb3J0IHsgTGF5b3V0IH0gZnJvbSAnLi4vLi4vbW9kZWxzL2xheW91dCc7XHJcbmltcG9ydCB7IExheW91dFN0YXRlIH0gZnJvbSAnLi4vLi4vc3RhdGVzJztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnYWJwLWxheW91dC1hcHBsaWNhdGlvbicsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL2FwcGxpY2F0aW9uLWxheW91dC5jb21wb25lbnQuaHRtbCcsXHJcbiAgYW5pbWF0aW9uczogW3NsaWRlRnJvbUJvdHRvbSwgY29sbGFwc2VXaXRoTWFyZ2luXSxcclxufSlcclxuZXhwb3J0IGNsYXNzIEFwcGxpY2F0aW9uTGF5b3V0Q29tcG9uZW50IGltcGxlbWVudHMgQWZ0ZXJWaWV3SW5pdCwgT25EZXN0cm95IHtcclxuICAvLyByZXF1aXJlZCBmb3IgZHluYW1pYyBjb21wb25lbnRcclxuICBzdGF0aWMgdHlwZSA9IGVMYXlvdXRUeXBlLmFwcGxpY2F0aW9uO1xyXG5cclxuICBAU2VsZWN0KENvbmZpZ1N0YXRlLmdldE9uZSgncm91dGVzJykpXHJcbiAgcm91dGVzJDogT2JzZXJ2YWJsZTxBQlAuRnVsbFJvdXRlW10+O1xyXG5cclxuICBAU2VsZWN0KENvbmZpZ1N0YXRlLmdldE9uZSgnY3VycmVudFVzZXInKSlcclxuICBjdXJyZW50VXNlciQ6IE9ic2VydmFibGU8QXBwbGljYXRpb25Db25maWd1cmF0aW9uLkN1cnJlbnRVc2VyPjtcclxuXHJcbiAgQFNlbGVjdChDb25maWdTdGF0ZS5nZXREZWVwKCdsb2NhbGl6YXRpb24ubGFuZ3VhZ2VzJykpXHJcbiAgbGFuZ3VhZ2VzJDogT2JzZXJ2YWJsZTxBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24uTGFuZ3VhZ2VbXT47XHJcblxyXG4gIEBTZWxlY3QoTGF5b3V0U3RhdGUuZ2V0TmF2aWdhdGlvbkVsZW1lbnRzKVxyXG4gIG5hdkVsZW1lbnRzJDogT2JzZXJ2YWJsZTxMYXlvdXQuTmF2aWdhdGlvbkVsZW1lbnRbXT47XHJcblxyXG4gIEBWaWV3Q2hpbGQoJ2N1cnJlbnRVc2VyJywgeyBzdGF0aWM6IGZhbHNlLCByZWFkOiBUZW1wbGF0ZVJlZiB9KVxyXG4gIGN1cnJlbnRVc2VyUmVmOiBUZW1wbGF0ZVJlZjxhbnk+O1xyXG5cclxuICBAVmlld0NoaWxkKCdsYW5ndWFnZScsIHsgc3RhdGljOiBmYWxzZSwgcmVhZDogVGVtcGxhdGVSZWYgfSlcclxuICBsYW5ndWFnZVJlZjogVGVtcGxhdGVSZWY8YW55PjtcclxuXHJcbiAgaXNEcm9wZG93bkNoaWxkRHluYW1pYzogYm9vbGVhbjtcclxuXHJcbiAgaXNDb2xsYXBzZWQgPSB0cnVlO1xyXG5cclxuICBzbWFsbFNjcmVlbjogYm9vbGVhbjsgLy8gZG8gbm90IHNldCB0cnVlIG9yIGZhbHNlXHJcblxyXG4gIGdldCBhcHBJbmZvKCk6IENvbmZpZy5BcHBsaWNhdGlvbiB7XHJcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRBcHBsaWNhdGlvbkluZm8pO1xyXG4gIH1cclxuXHJcbiAgZ2V0IHZpc2libGVSb3V0ZXMkKCk6IE9ic2VydmFibGU8QUJQLkZ1bGxSb3V0ZVtdPiB7XHJcbiAgICByZXR1cm4gdGhpcy5yb3V0ZXMkLnBpcGUobWFwKHJvdXRlcyA9PiBnZXRWaXNpYmxlUm91dGVzKHJvdXRlcykpKTtcclxuICB9XHJcblxyXG4gIGdldCBkZWZhdWx0TGFuZ3VhZ2UkKCk6IE9ic2VydmFibGU8c3RyaW5nPiB7XHJcbiAgICByZXR1cm4gdGhpcy5sYW5ndWFnZXMkLnBpcGUoXHJcbiAgICAgIG1hcChcclxuICAgICAgICBsYW5ndWFnZXMgPT4gc25xKCgpID0+IGxhbmd1YWdlcy5maW5kKGxhbmcgPT4gbGFuZy5jdWx0dXJlTmFtZSA9PT0gdGhpcy5zZWxlY3RlZExhbmdDdWx0dXJlKS5kaXNwbGF5TmFtZSksXHJcbiAgICAgICAgJycsXHJcbiAgICAgICksXHJcbiAgICApO1xyXG4gIH1cclxuXHJcbiAgZ2V0IGRyb3Bkb3duTGFuZ3VhZ2VzJCgpOiBPYnNlcnZhYmxlPEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbi5MYW5ndWFnZVtdPiB7XHJcbiAgICByZXR1cm4gdGhpcy5sYW5ndWFnZXMkLnBpcGUoXHJcbiAgICAgIG1hcChsYW5ndWFnZXMgPT4gc25xKCgpID0+IGxhbmd1YWdlcy5maWx0ZXIobGFuZyA9PiBsYW5nLmN1bHR1cmVOYW1lICE9PSB0aGlzLnNlbGVjdGVkTGFuZ0N1bHR1cmUpKSwgW10pLFxyXG4gICAgKTtcclxuICB9XHJcblxyXG4gIGdldCBzZWxlY3RlZExhbmdDdWx0dXJlKCk6IHN0cmluZyB7XHJcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChTZXNzaW9uU3RhdGUuZ2V0TGFuZ3VhZ2UpO1xyXG4gIH1cclxuXHJcbiAgcmlnaHRQYXJ0RWxlbWVudHM6IFRlbXBsYXRlUmVmPGFueT5bXSA9IFtdO1xyXG5cclxuICB0cmFja0J5Rm46IFRyYWNrQnlGdW5jdGlvbjxBQlAuRnVsbFJvdXRlPiA9IChfLCBpdGVtKSA9PiBpdGVtLm5hbWU7XHJcblxyXG4gIHRyYWNrRWxlbWVudEJ5Rm46IFRyYWNrQnlGdW5jdGlvbjxBQlAuRnVsbFJvdXRlPiA9IChfLCBlbGVtZW50KSA9PiBlbGVtZW50O1xyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSwgcHJpdmF0ZSBvYXV0aFNlcnZpY2U6IE9BdXRoU2VydmljZSwgcHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyKSB7fVxyXG5cclxuICBwcml2YXRlIGNoZWNrV2luZG93V2lkdGgoKSB7XHJcbiAgICBzZXRUaW1lb3V0KCgpID0+IHtcclxuICAgICAgaWYgKHdpbmRvdy5pbm5lcldpZHRoIDwgNzY4KSB7XHJcbiAgICAgICAgdGhpcy5pc0Ryb3Bkb3duQ2hpbGREeW5hbWljID0gZmFsc2U7XHJcbiAgICAgICAgaWYgKHRoaXMuc21hbGxTY3JlZW4gPT09IGZhbHNlKSB7XHJcbiAgICAgICAgICB0aGlzLmlzQ29sbGFwc2VkID0gZmFsc2U7XHJcbiAgICAgICAgICBzZXRUaW1lb3V0KCgpID0+IHtcclxuICAgICAgICAgICAgdGhpcy5pc0NvbGxhcHNlZCA9IHRydWU7XHJcbiAgICAgICAgICB9LCAxMDApO1xyXG4gICAgICAgIH1cclxuICAgICAgICB0aGlzLnNtYWxsU2NyZWVuID0gdHJ1ZTtcclxuICAgICAgfSBlbHNlIHtcclxuICAgICAgICB0aGlzLmlzRHJvcGRvd25DaGlsZER5bmFtaWMgPSB0cnVlO1xyXG4gICAgICAgIHRoaXMuc21hbGxTY3JlZW4gPSBmYWxzZTtcclxuICAgICAgfVxyXG4gICAgfSwgMCk7XHJcbiAgfVxyXG5cclxuICBuZ0FmdGVyVmlld0luaXQoKSB7XHJcbiAgICBjb25zdCBuYXZpZ2F0aW9ucyA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoTGF5b3V0U3RhdGUuZ2V0TmF2aWdhdGlvbkVsZW1lbnRzKS5tYXAoKHsgbmFtZSB9KSA9PiBuYW1lKTtcclxuXHJcbiAgICBpZiAobmF2aWdhdGlvbnMuaW5kZXhPZignTGFuZ3VhZ2VSZWYnKSA8IDApIHtcclxuICAgICAgdGhpcy5zdG9yZS5kaXNwYXRjaChcclxuICAgICAgICBuZXcgQWRkTmF2aWdhdGlvbkVsZW1lbnQoW1xyXG4gICAgICAgICAgeyBlbGVtZW50OiB0aGlzLmxhbmd1YWdlUmVmLCBvcmRlcjogNCwgbmFtZTogJ0xhbmd1YWdlUmVmJyB9LFxyXG4gICAgICAgICAgeyBlbGVtZW50OiB0aGlzLmN1cnJlbnRVc2VyUmVmLCBvcmRlcjogNSwgbmFtZTogJ0N1cnJlbnRVc2VyUmVmJyB9LFxyXG4gICAgICAgIF0pLFxyXG4gICAgICApO1xyXG4gICAgfVxyXG5cclxuICAgIHRoaXMubmF2RWxlbWVudHMkXHJcbiAgICAgIC5waXBlKFxyXG4gICAgICAgIG1hcChlbGVtZW50cyA9PiBlbGVtZW50cy5tYXAoKHsgZWxlbWVudCB9KSA9PiBlbGVtZW50KSksXHJcbiAgICAgICAgZmlsdGVyKGVsZW1lbnRzID0+ICFjb21wYXJlKGVsZW1lbnRzLCB0aGlzLnJpZ2h0UGFydEVsZW1lbnRzKSksXHJcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKSxcclxuICAgICAgKVxyXG4gICAgICAuc3Vic2NyaWJlKGVsZW1lbnRzID0+IHtcclxuICAgICAgICBzZXRUaW1lb3V0KCgpID0+ICh0aGlzLnJpZ2h0UGFydEVsZW1lbnRzID0gZWxlbWVudHMpLCAwKTtcclxuICAgICAgfSk7XHJcblxyXG4gICAgdGhpcy5jaGVja1dpbmRvd1dpZHRoKCk7XHJcblxyXG4gICAgZnJvbUV2ZW50KHdpbmRvdywgJ3Jlc2l6ZScpXHJcbiAgICAgIC5waXBlKFxyXG4gICAgICAgIHRha2VVbnRpbERlc3Ryb3kodGhpcyksXHJcbiAgICAgICAgZGVib3VuY2VUaW1lKDE1MCksXHJcbiAgICAgIClcclxuICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XHJcbiAgICAgICAgdGhpcy5jaGVja1dpbmRvd1dpZHRoKCk7XHJcbiAgICAgIH0pO1xyXG4gIH1cclxuXHJcbiAgbmdPbkRlc3Ryb3koKSB7fVxyXG5cclxuICBvbkNoYW5nZUxhbmcoY3VsdHVyZU5hbWU6IHN0cmluZykge1xyXG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgU2V0TGFuZ3VhZ2UoY3VsdHVyZU5hbWUpKTtcclxuICB9XHJcblxyXG4gIGxvZ291dCgpIHtcclxuICAgIHRoaXMub2F1dGhTZXJ2aWNlLmxvZ091dCgpO1xyXG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChcclxuICAgICAgbmV3IE5hdmlnYXRlKFsnLyddLCBudWxsLCB7XHJcbiAgICAgICAgc3RhdGU6IHsgcmVkaXJlY3RVcmw6IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoUm91dGVyU3RhdGUpLnN0YXRlLnVybCB9LFxyXG4gICAgICB9KSxcclxuICAgICk7XHJcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBHZXRBcHBDb25maWd1cmF0aW9uKCkpO1xyXG4gIH1cclxuXHJcbiAgb3BlbkNoYW5nZShldmVudDogYm9vbGVhbiwgY2hpbGRyZW5Db250YWluZXI6IEhUTUxEaXZFbGVtZW50KSB7XHJcbiAgICBpZiAoIWV2ZW50KSB7XHJcbiAgICAgIE9iamVjdC5rZXlzKGNoaWxkcmVuQ29udGFpbmVyLnN0eWxlKVxyXG4gICAgICAgIC5maWx0ZXIoa2V5ID0+IE51bWJlci5pc0ludGVnZXIoK2tleSkpXHJcbiAgICAgICAgLmZvckVhY2goa2V5ID0+IHtcclxuICAgICAgICAgIHRoaXMucmVuZGVyZXIucmVtb3ZlU3R5bGUoY2hpbGRyZW5Db250YWluZXIsIGNoaWxkcmVuQ29udGFpbmVyLnN0eWxlW2tleV0pO1xyXG4gICAgICAgIH0pO1xyXG4gICAgICB0aGlzLnJlbmRlcmVyLnJlbW92ZVN0eWxlKGNoaWxkcmVuQ29udGFpbmVyLCAnbGVmdCcpO1xyXG4gICAgfVxyXG4gIH1cclxufVxyXG5cclxuZnVuY3Rpb24gZ2V0VmlzaWJsZVJvdXRlcyhyb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSkge1xyXG4gIHJldHVybiByb3V0ZXMucmVkdWNlKChhY2MsIHZhbCkgPT4ge1xyXG4gICAgaWYgKHZhbC5pbnZpc2libGUpIHJldHVybiBhY2M7XHJcblxyXG4gICAgaWYgKHZhbC5jaGlsZHJlbiAmJiB2YWwuY2hpbGRyZW4ubGVuZ3RoKSB7XHJcbiAgICAgIHZhbC5jaGlsZHJlbiA9IGdldFZpc2libGVSb3V0ZXModmFsLmNoaWxkcmVuKTtcclxuICAgIH1cclxuXHJcbiAgICByZXR1cm4gWy4uLmFjYywgdmFsXTtcclxuICB9LCBbXSk7XHJcbn1cclxuIl19