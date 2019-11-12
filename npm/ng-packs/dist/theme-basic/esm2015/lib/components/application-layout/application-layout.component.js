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
        this.trackByFn = (/**
         * @param {?} _
         * @param {?} item
         * @return {?}
         */
        (_, item) => item.name);
        this.trackElementByFn = (/**
         * @param {?} _
         * @param {?} element
         * @return {?}
         */
        (_, element) => element);
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
        return this.routes$.pipe(map((/**
         * @param {?} routes
         * @return {?}
         */
        routes => getVisibleRoutes(routes))));
    }
    /**
     * @return {?}
     */
    get defaultLanguage$() {
        return this.languages$.pipe(map((/**
         * @param {?} languages
         * @return {?}
         */
        languages => snq((/**
         * @return {?}
         */
        () => languages.find((/**
         * @param {?} lang
         * @return {?}
         */
        lang => lang.cultureName === this.selectedLangCulture)).displayName))), ''));
    }
    /**
     * @return {?}
     */
    get dropdownLanguages$() {
        return this.languages$.pipe(map((/**
         * @param {?} languages
         * @return {?}
         */
        languages => snq((/**
         * @return {?}
         */
        () => languages.filter((/**
         * @param {?} lang
         * @return {?}
         */
        lang => lang.cultureName !== this.selectedLangCulture))))), []));
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
        setTimeout((/**
         * @return {?}
         */
        () => {
            if (window.innerWidth < 768) {
                this.isDropdownChildDynamic = false;
                if (this.smallScreen === false) {
                    this.isCollapsed = false;
                    setTimeout((/**
                     * @return {?}
                     */
                    () => {
                        this.isCollapsed = true;
                    }), 100);
                }
                this.smallScreen = true;
            }
            else {
                this.isDropdownChildDynamic = true;
                this.smallScreen = false;
            }
        }), 0);
    }
    /**
     * @return {?}
     */
    ngAfterViewInit() {
        /** @type {?} */
        const navigations = this.store.selectSnapshot(LayoutState.getNavigationElements).map((/**
         * @param {?} __0
         * @return {?}
         */
        ({ name }) => name));
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
        elements => elements.map((/**
         * @param {?} __0
         * @return {?}
         */
        ({ element }) => element)))), filter((/**
         * @param {?} elements
         * @return {?}
         */
        elements => !compare(elements, this.rightPartElements))), takeUntilDestroy(this))
            .subscribe((/**
         * @param {?} elements
         * @return {?}
         */
        elements => {
            setTimeout((/**
             * @return {?}
             */
            () => (this.rightPartElements = elements)), 0);
        }));
        this.checkWindowWidth();
        fromEvent(window, 'resize')
            .pipe(takeUntilDestroy(this), debounceTime(150))
            .subscribe((/**
         * @return {?}
         */
        () => {
            this.checkWindowWidth();
        }));
    }
    /**
     * @return {?}
     */
    ngOnDestroy() { }
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
        this.store.dispatch(new Navigate(['/'], null, {
            state: { redirectUrl: this.store.selectSnapshot(RouterState).state.url },
        }));
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
                .filter((/**
             * @param {?} key
             * @return {?}
             */
            key => Number.isInteger(+key)))
                .forEach((/**
             * @param {?} key
             * @return {?}
             */
            key => {
                this.renderer.removeStyle(childrenContainer, childrenContainer.style[key]);
            }));
            this.renderer.removeStyle(childrenContainer, 'left');
        }
    }
}
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
ApplicationLayoutComponent.ctorParameters = () => [
    { type: Store },
    { type: OAuthService },
    { type: Renderer2 }
];
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
    (acc, val) => {
        if (val.invisible)
            return acc;
        if (val.children && val.children.length) {
            val.children = getVisibleRoutes(val.children);
        }
        return [...acc, val];
    }), []);
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9hcHBsaWNhdGlvbi1sYXlvdXQvYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBSUwsV0FBVyxFQUVYLG1CQUFtQixFQUNuQixZQUFZLEVBQ1osV0FBVyxFQUNYLGdCQUFnQixHQUNqQixNQUFNLGNBQWMsQ0FBQztBQUN0QixPQUFPLEVBQUUsa0JBQWtCLEVBQUUsZUFBZSxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDM0UsT0FBTyxFQUVMLFNBQVMsRUFHVCxTQUFTLEVBQ1QsV0FBVyxFQUVYLFNBQVMsR0FHVixNQUFNLGVBQWUsQ0FBQztBQUV2QixPQUFPLEVBQUUsUUFBUSxFQUFFLFdBQVcsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQzVELE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUNuRCxPQUFPLE9BQU8sTUFBTSxjQUFjLENBQUM7QUFDbkMsT0FBTyxFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDN0MsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDM0QsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBQ3RCLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUVyRCxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBTzNDLE1BQU0sT0FBTywwQkFBMEI7Ozs7OztJQTZEckMsWUFBb0IsS0FBWSxFQUFVLFlBQTBCLEVBQVUsUUFBbUI7UUFBN0UsVUFB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QUMsU0FBUztZQUFFLE9BQU8sR0FBRyxDQUFDO1FBRTlCLElBQUksR0FBRyxDQUFDLFFBQVEsSUFBSSxHQUFHLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUN2QyxHQUFHLENBQUMsUUFBUSxHQUFHLGdCQUFnQixDQUFDLEdBQUcsQ0FBQyxRQUFRLENBQUMsQ0FBQztTQUMvQztRQUVELE9BQU8sQ0FBQyxHQUFHLEdBQUcsRUFBRSxHQUFHLENBQUMsQ0FBQztJQUN2QixDQUFDLEdBQUUsRUFBRSxDQUFDLENBQUM7QUFDVCxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcclxuICBBQlAsXHJcbiAgQXBwbGljYXRpb25Db25maWd1cmF0aW9uLFxyXG4gIENvbmZpZyxcclxuICBDb25maWdTdGF0ZSxcclxuICBlTGF5b3V0VHlwZSxcclxuICBHZXRBcHBDb25maWd1cmF0aW9uLFxyXG4gIFNlc3Npb25TdGF0ZSxcclxuICBTZXRMYW5ndWFnZSxcclxuICB0YWtlVW50aWxEZXN0cm95LFxyXG59IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcbmltcG9ydCB7IGNvbGxhcHNlV2l0aE1hcmdpbiwgc2xpZGVGcm9tQm90dG9tIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xyXG5pbXBvcnQge1xyXG4gIEFmdGVyVmlld0luaXQsXHJcbiAgQ29tcG9uZW50LFxyXG4gIE9uRGVzdHJveSxcclxuICBRdWVyeUxpc3QsXHJcbiAgUmVuZGVyZXIyLFxyXG4gIFRlbXBsYXRlUmVmLFxyXG4gIFRyYWNrQnlGdW5jdGlvbixcclxuICBWaWV3Q2hpbGQsXHJcbiAgVmlld0NoaWxkcmVuLFxyXG4gIEVsZW1lbnRSZWYsXHJcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IE5nYkRyb3Bkb3duIH0gZnJvbSAnQG5nLWJvb3RzdHJhcC9uZy1ib290c3RyYXAnO1xyXG5pbXBvcnQgeyBOYXZpZ2F0ZSwgUm91dGVyU3RhdGUgfSBmcm9tICdAbmd4cy9yb3V0ZXItcGx1Z2luJztcclxuaW1wb3J0IHsgU2VsZWN0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgT0F1dGhTZXJ2aWNlIH0gZnJvbSAnYW5ndWxhci1vYXV0aDItb2lkYyc7XHJcbmltcG9ydCBjb21wYXJlIGZyb20gJ2p1c3QtY29tcGFyZSc7XHJcbmltcG9ydCB7IGZyb21FdmVudCwgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xyXG5pbXBvcnQgeyBkZWJvdW5jZVRpbWUsIGZpbHRlciwgbWFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XHJcbmltcG9ydCB7IEFkZE5hdmlnYXRpb25FbGVtZW50IH0gZnJvbSAnLi4vLi4vYWN0aW9ucyc7XHJcbmltcG9ydCB7IExheW91dCB9IGZyb20gJy4uLy4uL21vZGVscy9sYXlvdXQnO1xyXG5pbXBvcnQgeyBMYXlvdXRTdGF0ZSB9IGZyb20gJy4uLy4uL3N0YXRlcyc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ2FicC1sYXlvdXQtYXBwbGljYXRpb24nLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi9hcHBsaWNhdGlvbi1sYXlvdXQuY29tcG9uZW50Lmh0bWwnLFxyXG4gIGFuaW1hdGlvbnM6IFtzbGlkZUZyb21Cb3R0b20sIGNvbGxhcHNlV2l0aE1hcmdpbl0sXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBBcHBsaWNhdGlvbkxheW91dENvbXBvbmVudCBpbXBsZW1lbnRzIEFmdGVyVmlld0luaXQsIE9uRGVzdHJveSB7XHJcbiAgLy8gcmVxdWlyZWQgZm9yIGR5bmFtaWMgY29tcG9uZW50XHJcbiAgc3RhdGljIHR5cGUgPSBlTGF5b3V0VHlwZS5hcHBsaWNhdGlvbjtcclxuXHJcbiAgQFNlbGVjdChDb25maWdTdGF0ZS5nZXRPbmUoJ3JvdXRlcycpKVxyXG4gIHJvdXRlcyQ6IE9ic2VydmFibGU8QUJQLkZ1bGxSb3V0ZVtdPjtcclxuXHJcbiAgQFNlbGVjdChDb25maWdTdGF0ZS5nZXRPbmUoJ2N1cnJlbnRVc2VyJykpXHJcbiAgY3VycmVudFVzZXIkOiBPYnNlcnZhYmxlPEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbi5DdXJyZW50VXNlcj47XHJcblxyXG4gIEBTZWxlY3QoQ29uZmlnU3RhdGUuZ2V0RGVlcCgnbG9jYWxpemF0aW9uLmxhbmd1YWdlcycpKVxyXG4gIGxhbmd1YWdlcyQ6IE9ic2VydmFibGU8QXBwbGljYXRpb25Db25maWd1cmF0aW9uLkxhbmd1YWdlW10+O1xyXG5cclxuICBAU2VsZWN0KExheW91dFN0YXRlLmdldE5hdmlnYXRpb25FbGVtZW50cylcclxuICBuYXZFbGVtZW50cyQ6IE9ic2VydmFibGU8TGF5b3V0Lk5hdmlnYXRpb25FbGVtZW50W10+O1xyXG5cclxuICBAVmlld0NoaWxkKCdjdXJyZW50VXNlcicsIHsgc3RhdGljOiBmYWxzZSwgcmVhZDogVGVtcGxhdGVSZWYgfSlcclxuICBjdXJyZW50VXNlclJlZjogVGVtcGxhdGVSZWY8YW55PjtcclxuXHJcbiAgQFZpZXdDaGlsZCgnbGFuZ3VhZ2UnLCB7IHN0YXRpYzogZmFsc2UsIHJlYWQ6IFRlbXBsYXRlUmVmIH0pXHJcbiAgbGFuZ3VhZ2VSZWY6IFRlbXBsYXRlUmVmPGFueT47XHJcblxyXG4gIGlzRHJvcGRvd25DaGlsZER5bmFtaWM6IGJvb2xlYW47XHJcblxyXG4gIGlzQ29sbGFwc2VkID0gdHJ1ZTtcclxuXHJcbiAgc21hbGxTY3JlZW46IGJvb2xlYW47IC8vIGRvIG5vdCBzZXQgdHJ1ZSBvciBmYWxzZVxyXG5cclxuICBnZXQgYXBwSW5mbygpOiBDb25maWcuQXBwbGljYXRpb24ge1xyXG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0QXBwbGljYXRpb25JbmZvKTtcclxuICB9XHJcblxyXG4gIGdldCB2aXNpYmxlUm91dGVzJCgpOiBPYnNlcnZhYmxlPEFCUC5GdWxsUm91dGVbXT4ge1xyXG4gICAgcmV0dXJuIHRoaXMucm91dGVzJC5waXBlKG1hcChyb3V0ZXMgPT4gZ2V0VmlzaWJsZVJvdXRlcyhyb3V0ZXMpKSk7XHJcbiAgfVxyXG5cclxuICBnZXQgZGVmYXVsdExhbmd1YWdlJCgpOiBPYnNlcnZhYmxlPHN0cmluZz4ge1xyXG4gICAgcmV0dXJuIHRoaXMubGFuZ3VhZ2VzJC5waXBlKFxyXG4gICAgICBtYXAoXHJcbiAgICAgICAgbGFuZ3VhZ2VzID0+IHNucSgoKSA9PiBsYW5ndWFnZXMuZmluZChsYW5nID0+IGxhbmcuY3VsdHVyZU5hbWUgPT09IHRoaXMuc2VsZWN0ZWRMYW5nQ3VsdHVyZSkuZGlzcGxheU5hbWUpLFxyXG4gICAgICAgICcnLFxyXG4gICAgICApLFxyXG4gICAgKTtcclxuICB9XHJcblxyXG4gIGdldCBkcm9wZG93bkxhbmd1YWdlcyQoKTogT2JzZXJ2YWJsZTxBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24uTGFuZ3VhZ2VbXT4ge1xyXG4gICAgcmV0dXJuIHRoaXMubGFuZ3VhZ2VzJC5waXBlKFxyXG4gICAgICBtYXAobGFuZ3VhZ2VzID0+IHNucSgoKSA9PiBsYW5ndWFnZXMuZmlsdGVyKGxhbmcgPT4gbGFuZy5jdWx0dXJlTmFtZSAhPT0gdGhpcy5zZWxlY3RlZExhbmdDdWx0dXJlKSksIFtdKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBnZXQgc2VsZWN0ZWRMYW5nQ3VsdHVyZSgpOiBzdHJpbmcge1xyXG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoU2Vzc2lvblN0YXRlLmdldExhbmd1YWdlKTtcclxuICB9XHJcblxyXG4gIHJpZ2h0UGFydEVsZW1lbnRzOiBUZW1wbGF0ZVJlZjxhbnk+W10gPSBbXTtcclxuXHJcbiAgdHJhY2tCeUZuOiBUcmFja0J5RnVuY3Rpb248QUJQLkZ1bGxSb3V0ZT4gPSAoXywgaXRlbSkgPT4gaXRlbS5uYW1lO1xyXG5cclxuICB0cmFja0VsZW1lbnRCeUZuOiBUcmFja0J5RnVuY3Rpb248QUJQLkZ1bGxSb3V0ZT4gPSAoXywgZWxlbWVudCkgPT4gZWxlbWVudDtcclxuXHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUsIHByaXZhdGUgb2F1dGhTZXJ2aWNlOiBPQXV0aFNlcnZpY2UsIHByaXZhdGUgcmVuZGVyZXI6IFJlbmRlcmVyMikge31cclxuXHJcbiAgcHJpdmF0ZSBjaGVja1dpbmRvd1dpZHRoKCkge1xyXG4gICAgc2V0VGltZW91dCgoKSA9PiB7XHJcbiAgICAgIGlmICh3aW5kb3cuaW5uZXJXaWR0aCA8IDc2OCkge1xyXG4gICAgICAgIHRoaXMuaXNEcm9wZG93bkNoaWxkRHluYW1pYyA9IGZhbHNlO1xyXG4gICAgICAgIGlmICh0aGlzLnNtYWxsU2NyZWVuID09PSBmYWxzZSkge1xyXG4gICAgICAgICAgdGhpcy5pc0NvbGxhcHNlZCA9IGZhbHNlO1xyXG4gICAgICAgICAgc2V0VGltZW91dCgoKSA9PiB7XHJcbiAgICAgICAgICAgIHRoaXMuaXNDb2xsYXBzZWQgPSB0cnVlO1xyXG4gICAgICAgICAgfSwgMTAwKTtcclxuICAgICAgICB9XHJcbiAgICAgICAgdGhpcy5zbWFsbFNjcmVlbiA9IHRydWU7XHJcbiAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgdGhpcy5pc0Ryb3Bkb3duQ2hpbGREeW5hbWljID0gdHJ1ZTtcclxuICAgICAgICB0aGlzLnNtYWxsU2NyZWVuID0gZmFsc2U7XHJcbiAgICAgIH1cclxuICAgIH0sIDApO1xyXG4gIH1cclxuXHJcbiAgbmdBZnRlclZpZXdJbml0KCkge1xyXG4gICAgY29uc3QgbmF2aWdhdGlvbnMgPSB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KExheW91dFN0YXRlLmdldE5hdmlnYXRpb25FbGVtZW50cykubWFwKCh7IG5hbWUgfSkgPT4gbmFtZSk7XHJcblxyXG4gICAgaWYgKG5hdmlnYXRpb25zLmluZGV4T2YoJ0xhbmd1YWdlUmVmJykgPCAwKSB7XHJcbiAgICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2goXHJcbiAgICAgICAgbmV3IEFkZE5hdmlnYXRpb25FbGVtZW50KFtcclxuICAgICAgICAgIHsgZWxlbWVudDogdGhpcy5sYW5ndWFnZVJlZiwgb3JkZXI6IDQsIG5hbWU6ICdMYW5ndWFnZVJlZicgfSxcclxuICAgICAgICAgIHsgZWxlbWVudDogdGhpcy5jdXJyZW50VXNlclJlZiwgb3JkZXI6IDUsIG5hbWU6ICdDdXJyZW50VXNlclJlZicgfSxcclxuICAgICAgICBdKSxcclxuICAgICAgKTtcclxuICAgIH1cclxuXHJcbiAgICB0aGlzLm5hdkVsZW1lbnRzJFxyXG4gICAgICAucGlwZShcclxuICAgICAgICBtYXAoZWxlbWVudHMgPT4gZWxlbWVudHMubWFwKCh7IGVsZW1lbnQgfSkgPT4gZWxlbWVudCkpLFxyXG4gICAgICAgIGZpbHRlcihlbGVtZW50cyA9PiAhY29tcGFyZShlbGVtZW50cywgdGhpcy5yaWdodFBhcnRFbGVtZW50cykpLFxyXG4gICAgICAgIHRha2VVbnRpbERlc3Ryb3kodGhpcyksXHJcbiAgICAgIClcclxuICAgICAgLnN1YnNjcmliZShlbGVtZW50cyA9PiB7XHJcbiAgICAgICAgc2V0VGltZW91dCgoKSA9PiAodGhpcy5yaWdodFBhcnRFbGVtZW50cyA9IGVsZW1lbnRzKSwgMCk7XHJcbiAgICAgIH0pO1xyXG5cclxuICAgIHRoaXMuY2hlY2tXaW5kb3dXaWR0aCgpO1xyXG5cclxuICAgIGZyb21FdmVudCh3aW5kb3csICdyZXNpemUnKVxyXG4gICAgICAucGlwZShcclxuICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpLFxyXG4gICAgICAgIGRlYm91bmNlVGltZSgxNTApLFxyXG4gICAgICApXHJcbiAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xyXG4gICAgICAgIHRoaXMuY2hlY2tXaW5kb3dXaWR0aCgpO1xyXG4gICAgICB9KTtcclxuICB9XHJcblxyXG4gIG5nT25EZXN0cm95KCkge31cclxuXHJcbiAgb25DaGFuZ2VMYW5nKGN1bHR1cmVOYW1lOiBzdHJpbmcpIHtcclxuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IFNldExhbmd1YWdlKGN1bHR1cmVOYW1lKSk7XHJcbiAgfVxyXG5cclxuICBsb2dvdXQoKSB7XHJcbiAgICB0aGlzLm9hdXRoU2VydmljZS5sb2dPdXQoKTtcclxuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2goXHJcbiAgICAgIG5ldyBOYXZpZ2F0ZShbJy8nXSwgbnVsbCwge1xyXG4gICAgICAgIHN0YXRlOiB7IHJlZGlyZWN0VXJsOiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFJvdXRlclN0YXRlKS5zdGF0ZS51cmwgfSxcclxuICAgICAgfSksXHJcbiAgICApO1xyXG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgR2V0QXBwQ29uZmlndXJhdGlvbigpKTtcclxuICB9XHJcblxyXG4gIG9wZW5DaGFuZ2UoZXZlbnQ6IGJvb2xlYW4sIGNoaWxkcmVuQ29udGFpbmVyOiBIVE1MRGl2RWxlbWVudCkge1xyXG4gICAgaWYgKCFldmVudCkge1xyXG4gICAgICBPYmplY3Qua2V5cyhjaGlsZHJlbkNvbnRhaW5lci5zdHlsZSlcclxuICAgICAgICAuZmlsdGVyKGtleSA9PiBOdW1iZXIuaXNJbnRlZ2VyKCtrZXkpKVxyXG4gICAgICAgIC5mb3JFYWNoKGtleSA9PiB7XHJcbiAgICAgICAgICB0aGlzLnJlbmRlcmVyLnJlbW92ZVN0eWxlKGNoaWxkcmVuQ29udGFpbmVyLCBjaGlsZHJlbkNvbnRhaW5lci5zdHlsZVtrZXldKTtcclxuICAgICAgICB9KTtcclxuICAgICAgdGhpcy5yZW5kZXJlci5yZW1vdmVTdHlsZShjaGlsZHJlbkNvbnRhaW5lciwgJ2xlZnQnKTtcclxuICAgIH1cclxuICB9XHJcbn1cclxuXHJcbmZ1bmN0aW9uIGdldFZpc2libGVSb3V0ZXMocm91dGVzOiBBQlAuRnVsbFJvdXRlW10pIHtcclxuICByZXR1cm4gcm91dGVzLnJlZHVjZSgoYWNjLCB2YWwpID0+IHtcclxuICAgIGlmICh2YWwuaW52aXNpYmxlKSByZXR1cm4gYWNjO1xyXG5cclxuICAgIGlmICh2YWwuY2hpbGRyZW4gJiYgdmFsLmNoaWxkcmVuLmxlbmd0aCkge1xyXG4gICAgICB2YWwuY2hpbGRyZW4gPSBnZXRWaXNpYmxlUm91dGVzKHZhbC5jaGlsZHJlbik7XHJcbiAgICB9XHJcblxyXG4gICAgcmV0dXJuIFsuLi5hY2MsIHZhbF07XHJcbiAgfSwgW10pO1xyXG59XHJcbiJdfQ==