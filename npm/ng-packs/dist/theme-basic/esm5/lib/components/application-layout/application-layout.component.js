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
                    template: "<nav\n  class=\"navbar navbar-expand-md navbar-dark bg-dark shadow-sm flex-column flex-md-row mb-4\"\n  id=\"main-navbar\"\n  style=\"min-height: 4rem;\"\n>\n  <div class=\"container \">\n    <a class=\"navbar-brand\" routerLink=\"/\">\n      <img *ngIf=\"appInfo.logoUrl; else appName\" [src]=\"appInfo.logoUrl\" [alt]=\"appInfo.name\" />\n    </a>\n    <button\n      class=\"navbar-toggler\"\n      type=\"button\"\n      [attr.aria-expanded]=\"!isCollapsed\"\n      (click)=\"isCollapsed = !isCollapsed\"\n    >\n      <span class=\"navbar-toggler-icon\"></span>\n    </button>\n    <div class=\"navbar-collapse\" [class.overflow-hidden]=\"smallScreen\" id=\"main-navbar-collapse\">\n      <ng-container *ngTemplateOutlet=\"!smallScreen ? navigations : null\"></ng-container>\n\n      <div *ngIf=\"smallScreen\" [@collapseWithMargin]=\"isCollapsed ? 'collapsed' : 'expanded'\">\n        <ng-container *ngTemplateOutlet=\"navigations\"></ng-container>\n      </div>\n\n      <ng-template #navigations>\n        <ul class=\"navbar-nav mx-auto\">\n          <ng-container\n            *ngFor=\"let route of visibleRoutes$ | async; trackBy: trackByFn\"\n            [ngTemplateOutlet]=\"route?.children?.length ? dropdownLink : defaultLink\"\n            [ngTemplateOutletContext]=\"{ $implicit: route }\"\n          >\n          </ng-container>\n\n          <ng-template #defaultLink let-route>\n            <li class=\"nav-item\" *abpPermission=\"route.requiredPolicy\">\n              <a class=\"nav-link\" [routerLink]=\"[route.url]\"\n                ><i *ngIf=\"route.iconClass\" [ngClass]=\"route.iconClass\"></i> {{ route.name | abpLocalization }}</a\n              >\n            </li>\n          </ng-template>\n\n          <ng-template #dropdownLink let-route>\n            <li\n              #navbarRootDropdown\n              *abpPermission=\"route.requiredPolicy\"\n              [abpVisibility]=\"routeContainer\"\n              class=\"nav-item dropdown\"\n              display=\"static\"\n              (click)=\"\n                navbarRootDropdown.expand ? (navbarRootDropdown.expand = false) : (navbarRootDropdown.expand = true)\n              \"\n            >\n              <a\n                class=\"nav-link dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                aria-expanded=\"false\"\n                href=\"javascript:void(0)\"\n              >\n                <i *ngIf=\"route.iconClass\" [ngClass]=\"route.iconClass\"></i> {{ route.name | abpLocalization }}\n              </a>\n              <div\n                #routeContainer\n                class=\"dropdown-menu border-0 shadow-sm\"\n                (click)=\"$event.preventDefault(); $event.stopPropagation()\"\n                [class.abp-collapsed-height]=\"smallScreen\"\n                [class.d-block]=\"smallScreen\"\n                [class.abp-mh-25]=\"smallScreen && navbarRootDropdown.expand\"\n              >\n                <ng-template\n                  #forTemplate\n                  ngFor\n                  [ngForOf]=\"route.children\"\n                  [ngForTrackBy]=\"trackByFn\"\n                  [ngForTemplate]=\"childWrapper\"\n                ></ng-template>\n              </div>\n            </li>\n          </ng-template>\n\n          <ng-template #childWrapper let-child>\n            <ng-template\n              [ngTemplateOutlet]=\"child?.children?.length ? dropdownChild : defaultChild\"\n              [ngTemplateOutletContext]=\"{ $implicit: child }\"\n            ></ng-template>\n          </ng-template>\n\n          <ng-template #defaultChild let-child>\n            <div class=\"dropdown-submenu\" *abpPermission=\"child.requiredPolicy\">\n              <a class=\"dropdown-item\" [routerLink]=\"[child.url]\">\n                <i *ngIf=\"child.iconClass\" [ngClass]=\"child.iconClass\"></i>\n                {{ child.name | abpLocalization }}</a\n              >\n            </div>\n          </ng-template>\n\n          <ng-template #dropdownChild let-child>\n            <div\n              [abpVisibility]=\"childrenContainer\"\n              class=\"dropdown-submenu\"\n              ngbDropdown\n              #dropdownSubmenu=\"ngbDropdown\"\n              [display]=\"isDropdownChildDynamic ? 'dynamic' : 'static'\"\n              placement=\"right-top\"\n              [autoClose]=\"true\"\n              *abpPermission=\"child.requiredPolicy\"\n              (openChange)=\"openChange($event, childrenContainer)\"\n            >\n              <div ngbDropdownToggle [class.dropdown-toggle]=\"false\">\n                <a\n                  abpEllipsis=\"210px\"\n                  [abpEllipsisEnabled]=\"isDropdownChildDynamic\"\n                  role=\"button\"\n                  class=\"btn d-block text-left dropdown-toggle\"\n                >\n                  <i *ngIf=\"child.iconClass\" [ngClass]=\"child.iconClass\"></i>\n                  {{ child.name | abpLocalization }}\n                </a>\n              </div>\n              <div\n                #childrenContainer\n                class=\"dropdown-menu border-0 shadow-sm\"\n                [class.abp-collapsed-height]=\"smallScreen\"\n                [class.d-block]=\"smallScreen\"\n                [class.abp-mh-25]=\"smallScreen && dropdownSubmenu.isOpen()\"\n              >\n                <ng-template\n                  ngFor\n                  [ngForOf]=\"child.children\"\n                  [ngForTrackBy]=\"trackByFn\"\n                  [ngForTemplate]=\"childWrapper\"\n                ></ng-template>\n              </div>\n            </div>\n          </ng-template>\n        </ul>\n\n        <ul class=\"navbar-nav\">\n          <ng-container\n            *ngFor=\"let element of rightPartElements; trackBy: trackElementByFn\"\n            [ngTemplateOutlet]=\"element\"\n          ></ng-container>\n        </ul>\n      </ng-template>\n    </div>\n  </div>\n</nav>\n\n<div [@slideFromBottom]=\"outlet && outlet.activatedRoute && outlet.activatedRoute.routeConfig.path\" class=\"container\">\n  <router-outlet #outlet=\"outlet\"></router-outlet>\n</div>\n\n<abp-confirmation></abp-confirmation>\n<abp-toast></abp-toast>\n\n<ng-template #appName>\n  {{ appInfo.name }}\n</ng-template>\n\n<ng-template #language>\n  <li *ngIf=\"(dropdownLanguages$ | async)?.length > 0\" class=\"nav-item\">\n    <div class=\"dropdown\" ngbDropdown #languageDropdown=\"ngbDropdown\" display=\"static\">\n      <a\n        ngbDropdownToggle\n        class=\"nav-link\"\n        href=\"javascript:void(0)\"\n        role=\"button\"\n        id=\"dropdownMenuLink\"\n        data-toggle=\"dropdown\"\n        aria-haspopup=\"true\"\n        aria-expanded=\"false\"\n      >\n        {{ defaultLanguage$ | async }}\n      </a>\n      <div\n        class=\"dropdown-menu dropdown-menu-right border-0 shadow-sm\"\n        aria-labelledby=\"dropdownMenuLink\"\n        [class.abp-collapsed-height]=\"smallScreen\"\n        [class.d-block]=\"smallScreen\"\n        [class.abp-mh-25]=\"smallScreen && languageDropdown.isOpen()\"\n      >\n        <a\n          *ngFor=\"let lang of dropdownLanguages$ | async\"\n          href=\"javascript:void(0)\"\n          class=\"dropdown-item\"\n          (click)=\"onChangeLang(lang.cultureName)\"\n          >{{ lang?.displayName }}</a\n        >\n      </div>\n    </div>\n  </li>\n</ng-template>\n\n<ng-template #currentUser>\n  <li *ngIf=\"(currentUser$ | async)?.isAuthenticated\" class=\"nav-item\">\n    <div ngbDropdown class=\"dropdown\" #currentUserDropdown=\"ngbDropdown\" display=\"static\">\n      <a\n        ngbDropdownToggle\n        class=\"nav-link\"\n        href=\"javascript:void(0)\"\n        role=\"button\"\n        id=\"dropdownMenuLink\"\n        data-toggle=\"dropdown\"\n        aria-haspopup=\"true\"\n        aria-expanded=\"false\"\n      >\n        {{ (currentUser$ | async)?.userName }}\n      </a>\n      <div\n        class=\"dropdown-menu dropdown-menu-right border-0 shadow-sm\"\n        aria-labelledby=\"dropdownMenuLink\"\n        [class.abp-collapsed-height]=\"smallScreen\"\n        [class.d-block]=\"smallScreen\"\n        [class.abp-mh-25]=\"smallScreen && currentUserDropdown.isOpen()\"\n      >\n        <a class=\"dropdown-item\" routerLink=\"/account/manage-profile\"><i class=\"fa fa-cog mr-1\"></i>{{\n          'AbpAccount::ManageYourProfile' | abpLocalization\n        }}</a>\n        <a class=\"dropdown-item\" href=\"javascript:void(0)\" (click)=\"logout()\"><i class=\"fa fa-power-off mr-1\"></i>{{\n          'AbpUi::Logout' | abpLocalization\n        }}</a>\n      </div>\n    </div>\n  </li>\n</ng-template>\n",
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9hcHBsaWNhdGlvbi1sYXlvdXQvYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBSUwsV0FBVyxFQUVYLG1CQUFtQixFQUNuQixZQUFZLEVBQ1osV0FBVyxFQUNYLGdCQUFnQixHQUNqQixNQUFNLGNBQWMsQ0FBQztBQUN0QixPQUFPLEVBQUUsa0JBQWtCLEVBQUUsZUFBZSxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDM0UsT0FBTyxFQUVMLFNBQVMsRUFHVCxTQUFTLEVBQ1QsV0FBVyxFQUVYLFNBQVMsR0FHVixNQUFNLGVBQWUsQ0FBQztBQUV2QixPQUFPLEVBQUUsUUFBUSxFQUFFLFdBQVcsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQzVELE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUNuRCxPQUFPLE9BQU8sTUFBTSxjQUFjLENBQUM7QUFDbkMsT0FBTyxFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDN0MsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDM0QsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBQ3RCLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUVyRCxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBRTNDO0lBa0VFLG9DQUFvQixLQUFZLEVBQVUsWUFBMEIsRUFBVSxRQUFtQjtRQUE3RSxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBQVUs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LGdEQUM0RDs7SUFFNUQsa0RBQ3FEOztJQUVyRCxvREFDaUM7O0lBRWpDLGlEQUM4Qjs7SUFFOUIsNERBQWdDOztJQUVoQyxpREFBbUI7O0lBRW5CLGlEQUFxQjs7SUE2QnJCLHVEQUEyQzs7SUFFM0MsK0NBQW1FOztJQUVuRSxzREFBMkU7Ozs7O0lBRS9ELDJDQUFvQjs7Ozs7SUFBRSxrREFBa0M7Ozs7O0lBQUUsOENBQTJCOzs7Ozs7QUFrRm5HLFNBQVMsZ0JBQWdCLENBQUMsTUFBdUI7SUFDL0MsT0FBTyxNQUFNLENBQUMsTUFBTTs7Ozs7SUFBQyxVQUFDLEdBQUcsRUFBRSxHQUFHO1FBQzVCLElBQUksR0FBRyxDQUFDLFNBQVM7WUFBRSxPQUFPLEdBQUcsQ0FBQztRQUU5QixJQUFJLEdBQUcsQ0FBQyxRQUFRLElBQUksR0FBRyxDQUFDLFFBQVEsQ0FBQyxNQUFNLEVBQUU7WUFDdkMsR0FBRyxDQUFDLFFBQVEsR0FBRyxnQkFBZ0IsQ0FBQyxHQUFHLENBQUMsUUFBUSxDQUFDLENBQUM7U0FDL0M7UUFFRCx3QkFBVyxHQUFHLEdBQUUsR0FBRyxHQUFFO0lBQ3ZCLENBQUMsR0FBRSxFQUFFLENBQUMsQ0FBQztBQUNULENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQge1xuICBBQlAsXG4gIEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbixcbiAgQ29uZmlnLFxuICBDb25maWdTdGF0ZSxcbiAgZUxheW91dFR5cGUsXG4gIEdldEFwcENvbmZpZ3VyYXRpb24sXG4gIFNlc3Npb25TdGF0ZSxcbiAgU2V0TGFuZ3VhZ2UsXG4gIHRha2VVbnRpbERlc3Ryb3ksXG59IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBjb2xsYXBzZVdpdGhNYXJnaW4sIHNsaWRlRnJvbUJvdHRvbSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7XG4gIEFmdGVyVmlld0luaXQsXG4gIENvbXBvbmVudCxcbiAgT25EZXN0cm95LFxuICBRdWVyeUxpc3QsXG4gIFJlbmRlcmVyMixcbiAgVGVtcGxhdGVSZWYsXG4gIFRyYWNrQnlGdW5jdGlvbixcbiAgVmlld0NoaWxkLFxuICBWaWV3Q2hpbGRyZW4sXG4gIEVsZW1lbnRSZWYsXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgTmdiRHJvcGRvd24gfSBmcm9tICdAbmctYm9vdHN0cmFwL25nLWJvb3RzdHJhcCc7XG5pbXBvcnQgeyBOYXZpZ2F0ZSwgUm91dGVyU3RhdGUgfSBmcm9tICdAbmd4cy9yb3V0ZXItcGx1Z2luJztcbmltcG9ydCB7IFNlbGVjdCwgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBPQXV0aFNlcnZpY2UgfSBmcm9tICdhbmd1bGFyLW9hdXRoMi1vaWRjJztcbmltcG9ydCBjb21wYXJlIGZyb20gJ2p1c3QtY29tcGFyZSc7XG5pbXBvcnQgeyBmcm9tRXZlbnQsIE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGRlYm91bmNlVGltZSwgZmlsdGVyLCBtYXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5pbXBvcnQgeyBBZGROYXZpZ2F0aW9uRWxlbWVudCB9IGZyb20gJy4uLy4uL2FjdGlvbnMnO1xuaW1wb3J0IHsgTGF5b3V0IH0gZnJvbSAnLi4vLi4vbW9kZWxzL2xheW91dCc7XG5pbXBvcnQgeyBMYXlvdXRTdGF0ZSB9IGZyb20gJy4uLy4uL3N0YXRlcyc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1sYXlvdXQtYXBwbGljYXRpb24nLFxuICB0ZW1wbGF0ZVVybDogJy4vYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC5odG1sJyxcbiAgYW5pbWF0aW9uczogW3NsaWRlRnJvbUJvdHRvbSwgY29sbGFwc2VXaXRoTWFyZ2luXSxcbn0pXG5leHBvcnQgY2xhc3MgQXBwbGljYXRpb25MYXlvdXRDb21wb25lbnQgaW1wbGVtZW50cyBBZnRlclZpZXdJbml0LCBPbkRlc3Ryb3kge1xuICAvLyByZXF1aXJlZCBmb3IgZHluYW1pYyBjb21wb25lbnRcbiAgc3RhdGljIHR5cGUgPSBlTGF5b3V0VHlwZS5hcHBsaWNhdGlvbjtcblxuICBAU2VsZWN0KENvbmZpZ1N0YXRlLmdldE9uZSgncm91dGVzJykpXG4gIHJvdXRlcyQ6IE9ic2VydmFibGU8QUJQLkZ1bGxSb3V0ZVtdPjtcblxuICBAU2VsZWN0KENvbmZpZ1N0YXRlLmdldE9uZSgnY3VycmVudFVzZXInKSlcbiAgY3VycmVudFVzZXIkOiBPYnNlcnZhYmxlPEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbi5DdXJyZW50VXNlcj47XG5cbiAgQFNlbGVjdChDb25maWdTdGF0ZS5nZXREZWVwKCdsb2NhbGl6YXRpb24ubGFuZ3VhZ2VzJykpXG4gIGxhbmd1YWdlcyQ6IE9ic2VydmFibGU8QXBwbGljYXRpb25Db25maWd1cmF0aW9uLkxhbmd1YWdlW10+O1xuXG4gIEBTZWxlY3QoTGF5b3V0U3RhdGUuZ2V0TmF2aWdhdGlvbkVsZW1lbnRzKVxuICBuYXZFbGVtZW50cyQ6IE9ic2VydmFibGU8TGF5b3V0Lk5hdmlnYXRpb25FbGVtZW50W10+O1xuXG4gIEBWaWV3Q2hpbGQoJ2N1cnJlbnRVc2VyJywgeyBzdGF0aWM6IGZhbHNlLCByZWFkOiBUZW1wbGF0ZVJlZiB9KVxuICBjdXJyZW50VXNlclJlZjogVGVtcGxhdGVSZWY8YW55PjtcblxuICBAVmlld0NoaWxkKCdsYW5ndWFnZScsIHsgc3RhdGljOiBmYWxzZSwgcmVhZDogVGVtcGxhdGVSZWYgfSlcbiAgbGFuZ3VhZ2VSZWY6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgaXNEcm9wZG93bkNoaWxkRHluYW1pYzogYm9vbGVhbjtcblxuICBpc0NvbGxhcHNlZCA9IHRydWU7XG5cbiAgc21hbGxTY3JlZW46IGJvb2xlYW47IC8vIGRvIG5vdCBzZXQgdHJ1ZSBvciBmYWxzZVxuXG4gIGdldCBhcHBJbmZvKCk6IENvbmZpZy5BcHBsaWNhdGlvbiB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0QXBwbGljYXRpb25JbmZvKTtcbiAgfVxuXG4gIGdldCB2aXNpYmxlUm91dGVzJCgpOiBPYnNlcnZhYmxlPEFCUC5GdWxsUm91dGVbXT4ge1xuICAgIHJldHVybiB0aGlzLnJvdXRlcyQucGlwZShtYXAocm91dGVzID0+IGdldFZpc2libGVSb3V0ZXMocm91dGVzKSkpO1xuICB9XG5cbiAgZ2V0IGRlZmF1bHRMYW5ndWFnZSQoKTogT2JzZXJ2YWJsZTxzdHJpbmc+IHtcbiAgICByZXR1cm4gdGhpcy5sYW5ndWFnZXMkLnBpcGUoXG4gICAgICBtYXAoXG4gICAgICAgIGxhbmd1YWdlcyA9PiBzbnEoKCkgPT4gbGFuZ3VhZ2VzLmZpbmQobGFuZyA9PiBsYW5nLmN1bHR1cmVOYW1lID09PSB0aGlzLnNlbGVjdGVkTGFuZ0N1bHR1cmUpLmRpc3BsYXlOYW1lKSxcbiAgICAgICAgJycsXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBnZXQgZHJvcGRvd25MYW5ndWFnZXMkKCk6IE9ic2VydmFibGU8QXBwbGljYXRpb25Db25maWd1cmF0aW9uLkxhbmd1YWdlW10+IHtcbiAgICByZXR1cm4gdGhpcy5sYW5ndWFnZXMkLnBpcGUoXG4gICAgICBtYXAobGFuZ3VhZ2VzID0+IHNucSgoKSA9PiBsYW5ndWFnZXMuZmlsdGVyKGxhbmcgPT4gbGFuZy5jdWx0dXJlTmFtZSAhPT0gdGhpcy5zZWxlY3RlZExhbmdDdWx0dXJlKSksIFtdKSxcbiAgICApO1xuICB9XG5cbiAgZ2V0IHNlbGVjdGVkTGFuZ0N1bHR1cmUoKTogc3RyaW5nIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChTZXNzaW9uU3RhdGUuZ2V0TGFuZ3VhZ2UpO1xuICB9XG5cbiAgcmlnaHRQYXJ0RWxlbWVudHM6IFRlbXBsYXRlUmVmPGFueT5bXSA9IFtdO1xuXG4gIHRyYWNrQnlGbjogVHJhY2tCeUZ1bmN0aW9uPEFCUC5GdWxsUm91dGU+ID0gKF8sIGl0ZW0pID0+IGl0ZW0ubmFtZTtcblxuICB0cmFja0VsZW1lbnRCeUZuOiBUcmFja0J5RnVuY3Rpb248QUJQLkZ1bGxSb3V0ZT4gPSAoXywgZWxlbWVudCkgPT4gZWxlbWVudDtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSwgcHJpdmF0ZSBvYXV0aFNlcnZpY2U6IE9BdXRoU2VydmljZSwgcHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyKSB7fVxuXG4gIHByaXZhdGUgY2hlY2tXaW5kb3dXaWR0aCgpIHtcbiAgICBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgIGlmICh3aW5kb3cuaW5uZXJXaWR0aCA8IDc2OCkge1xuICAgICAgICB0aGlzLmlzRHJvcGRvd25DaGlsZER5bmFtaWMgPSBmYWxzZTtcbiAgICAgICAgaWYgKHRoaXMuc21hbGxTY3JlZW4gPT09IGZhbHNlKSB7XG4gICAgICAgICAgdGhpcy5pc0NvbGxhcHNlZCA9IGZhbHNlO1xuICAgICAgICAgIHNldFRpbWVvdXQoKCkgPT4ge1xuICAgICAgICAgICAgdGhpcy5pc0NvbGxhcHNlZCA9IHRydWU7XG4gICAgICAgICAgfSwgMTAwKTtcbiAgICAgICAgfVxuICAgICAgICB0aGlzLnNtYWxsU2NyZWVuID0gdHJ1ZTtcbiAgICAgIH0gZWxzZSB7XG4gICAgICAgIHRoaXMuaXNEcm9wZG93bkNoaWxkRHluYW1pYyA9IHRydWU7XG4gICAgICAgIHRoaXMuc21hbGxTY3JlZW4gPSBmYWxzZTtcbiAgICAgIH1cbiAgICB9LCAwKTtcbiAgfVxuXG4gIG5nQWZ0ZXJWaWV3SW5pdCgpIHtcbiAgICBjb25zdCBuYXZpZ2F0aW9ucyA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoTGF5b3V0U3RhdGUuZ2V0TmF2aWdhdGlvbkVsZW1lbnRzKS5tYXAoKHsgbmFtZSB9KSA9PiBuYW1lKTtcblxuICAgIGlmIChuYXZpZ2F0aW9ucy5pbmRleE9mKCdMYW5ndWFnZVJlZicpIDwgMCkge1xuICAgICAgdGhpcy5zdG9yZS5kaXNwYXRjaChcbiAgICAgICAgbmV3IEFkZE5hdmlnYXRpb25FbGVtZW50KFtcbiAgICAgICAgICB7IGVsZW1lbnQ6IHRoaXMubGFuZ3VhZ2VSZWYsIG9yZGVyOiA0LCBuYW1lOiAnTGFuZ3VhZ2VSZWYnIH0sXG4gICAgICAgICAgeyBlbGVtZW50OiB0aGlzLmN1cnJlbnRVc2VyUmVmLCBvcmRlcjogNSwgbmFtZTogJ0N1cnJlbnRVc2VyUmVmJyB9LFxuICAgICAgICBdKSxcbiAgICAgICk7XG4gICAgfVxuXG4gICAgdGhpcy5uYXZFbGVtZW50cyRcbiAgICAgIC5waXBlKFxuICAgICAgICBtYXAoZWxlbWVudHMgPT4gZWxlbWVudHMubWFwKCh7IGVsZW1lbnQgfSkgPT4gZWxlbWVudCkpLFxuICAgICAgICBmaWx0ZXIoZWxlbWVudHMgPT4gIWNvbXBhcmUoZWxlbWVudHMsIHRoaXMucmlnaHRQYXJ0RWxlbWVudHMpKSxcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoZWxlbWVudHMgPT4ge1xuICAgICAgICBzZXRUaW1lb3V0KCgpID0+ICh0aGlzLnJpZ2h0UGFydEVsZW1lbnRzID0gZWxlbWVudHMpLCAwKTtcbiAgICAgIH0pO1xuXG4gICAgdGhpcy5jaGVja1dpbmRvd1dpZHRoKCk7XG5cbiAgICBmcm9tRXZlbnQod2luZG93LCAncmVzaXplJylcbiAgICAgIC5waXBlKFxuICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpLFxuICAgICAgICBkZWJvdW5jZVRpbWUoMTUwKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICB0aGlzLmNoZWNrV2luZG93V2lkdGgoKTtcbiAgICAgIH0pO1xuICB9XG5cbiAgbmdPbkRlc3Ryb3koKSB7fVxuXG4gIG9uQ2hhbmdlTGFuZyhjdWx0dXJlTmFtZTogc3RyaW5nKSB7XG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgU2V0TGFuZ3VhZ2UoY3VsdHVyZU5hbWUpKTtcbiAgfVxuXG4gIGxvZ291dCgpIHtcbiAgICB0aGlzLm9hdXRoU2VydmljZS5sb2dPdXQoKTtcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKFxuICAgICAgbmV3IE5hdmlnYXRlKFsnLyddLCBudWxsLCB7XG4gICAgICAgIHN0YXRlOiB7IHJlZGlyZWN0VXJsOiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFJvdXRlclN0YXRlKS5zdGF0ZS51cmwgfSxcbiAgICAgIH0pLFxuICAgICk7XG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgR2V0QXBwQ29uZmlndXJhdGlvbigpKTtcbiAgfVxuXG4gIG9wZW5DaGFuZ2UoZXZlbnQ6IGJvb2xlYW4sIGNoaWxkcmVuQ29udGFpbmVyOiBIVE1MRGl2RWxlbWVudCkge1xuICAgIGlmICghZXZlbnQpIHtcbiAgICAgIE9iamVjdC5rZXlzKGNoaWxkcmVuQ29udGFpbmVyLnN0eWxlKVxuICAgICAgICAuZmlsdGVyKGtleSA9PiBOdW1iZXIuaXNJbnRlZ2VyKCtrZXkpKVxuICAgICAgICAuZm9yRWFjaChrZXkgPT4ge1xuICAgICAgICAgIHRoaXMucmVuZGVyZXIucmVtb3ZlU3R5bGUoY2hpbGRyZW5Db250YWluZXIsIGNoaWxkcmVuQ29udGFpbmVyLnN0eWxlW2tleV0pO1xuICAgICAgICB9KTtcbiAgICAgIHRoaXMucmVuZGVyZXIucmVtb3ZlU3R5bGUoY2hpbGRyZW5Db250YWluZXIsICdsZWZ0Jyk7XG4gICAgfVxuICB9XG59XG5cbmZ1bmN0aW9uIGdldFZpc2libGVSb3V0ZXMocm91dGVzOiBBQlAuRnVsbFJvdXRlW10pIHtcbiAgcmV0dXJuIHJvdXRlcy5yZWR1Y2UoKGFjYywgdmFsKSA9PiB7XG4gICAgaWYgKHZhbC5pbnZpc2libGUpIHJldHVybiBhY2M7XG5cbiAgICBpZiAodmFsLmNoaWxkcmVuICYmIHZhbC5jaGlsZHJlbi5sZW5ndGgpIHtcbiAgICAgIHZhbC5jaGlsZHJlbiA9IGdldFZpc2libGVSb3V0ZXModmFsLmNoaWxkcmVuKTtcbiAgICB9XG5cbiAgICByZXR1cm4gWy4uLmFjYywgdmFsXTtcbiAgfSwgW10pO1xufVxuIl19