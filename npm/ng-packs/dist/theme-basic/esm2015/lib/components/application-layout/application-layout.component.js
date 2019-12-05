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
                template: "<nav\n  class=\"navbar navbar-expand-md navbar-dark bg-dark shadow-sm flex-column flex-md-row mb-4\"\n  id=\"main-navbar\"\n  style=\"min-height: 4rem;\"\n>\n  <div class=\"container \">\n    <a class=\"navbar-brand\" routerLink=\"/\">\n      <img *ngIf=\"appInfo.logoUrl; else appName\" [src]=\"appInfo.logoUrl\" [alt]=\"appInfo.name\" />\n    </a>\n    <button\n      class=\"navbar-toggler\"\n      type=\"button\"\n      [attr.aria-expanded]=\"!isCollapsed\"\n      (click)=\"isCollapsed = !isCollapsed\"\n    >\n      <span class=\"navbar-toggler-icon\"></span>\n    </button>\n    <div class=\"navbar-collapse\" [class.overflow-hidden]=\"smallScreen\" id=\"main-navbar-collapse\">\n      <ng-container *ngTemplateOutlet=\"!smallScreen ? navigations : null\"></ng-container>\n\n      <div *ngIf=\"smallScreen\" [@collapseWithMargin]=\"isCollapsed ? 'collapsed' : 'expanded'\">\n        <ng-container *ngTemplateOutlet=\"navigations\"></ng-container>\n      </div>\n\n      <ng-template #navigations>\n        <ul class=\"navbar-nav mx-auto\">\n          <ng-container\n            *ngFor=\"let route of visibleRoutes$ | async; trackBy: trackByFn\"\n            [ngTemplateOutlet]=\"route?.children?.length ? dropdownLink : defaultLink\"\n            [ngTemplateOutletContext]=\"{ $implicit: route }\"\n          >\n          </ng-container>\n\n          <ng-template #defaultLink let-route>\n            <li class=\"nav-item\" *abpPermission=\"route.requiredPolicy\">\n              <a class=\"nav-link\" [routerLink]=\"[route.url]\"\n                ><i *ngIf=\"route.iconClass\" [ngClass]=\"route.iconClass\"></i> {{ route.name | abpLocalization }}</a\n              >\n            </li>\n          </ng-template>\n\n          <ng-template #dropdownLink let-route>\n            <li\n              #navbarRootDropdown\n              *abpPermission=\"route.requiredPolicy\"\n              [abpVisibility]=\"routeContainer\"\n              class=\"nav-item dropdown\"\n              display=\"static\"\n              (click)=\"\n                navbarRootDropdown.expand ? (navbarRootDropdown.expand = false) : (navbarRootDropdown.expand = true)\n              \"\n            >\n              <a\n                class=\"nav-link dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                aria-expanded=\"false\"\n                href=\"javascript:void(0)\"\n              >\n                <i *ngIf=\"route.iconClass\" [ngClass]=\"route.iconClass\"></i> {{ route.name | abpLocalization }}\n              </a>\n              <div\n                #routeContainer\n                class=\"dropdown-menu border-0 shadow-sm\"\n                (click)=\"$event.preventDefault(); $event.stopPropagation()\"\n                [class.abp-collapsed-height]=\"smallScreen\"\n                [class.d-block]=\"smallScreen\"\n                [class.abp-mh-25]=\"smallScreen && navbarRootDropdown.expand\"\n              >\n                <ng-template\n                  #forTemplate\n                  ngFor\n                  [ngForOf]=\"route.children\"\n                  [ngForTrackBy]=\"trackByFn\"\n                  [ngForTemplate]=\"childWrapper\"\n                ></ng-template>\n              </div>\n            </li>\n          </ng-template>\n\n          <ng-template #childWrapper let-child>\n            <ng-template\n              [ngTemplateOutlet]=\"child?.children?.length ? dropdownChild : defaultChild\"\n              [ngTemplateOutletContext]=\"{ $implicit: child }\"\n            ></ng-template>\n          </ng-template>\n\n          <ng-template #defaultChild let-child>\n            <div class=\"dropdown-submenu\" *abpPermission=\"child.requiredPolicy\">\n              <a class=\"dropdown-item\" [routerLink]=\"[child.url]\">\n                <i *ngIf=\"child.iconClass\" [ngClass]=\"child.iconClass\"></i>\n                {{ child.name | abpLocalization }}</a\n              >\n            </div>\n          </ng-template>\n\n          <ng-template #dropdownChild let-child>\n            <div\n              [abpVisibility]=\"childrenContainer\"\n              class=\"dropdown-submenu\"\n              ngbDropdown\n              #dropdownSubmenu=\"ngbDropdown\"\n              [display]=\"isDropdownChildDynamic ? 'dynamic' : 'static'\"\n              placement=\"right-top\"\n              [autoClose]=\"true\"\n              *abpPermission=\"child.requiredPolicy\"\n              (openChange)=\"openChange($event, childrenContainer)\"\n            >\n              <div ngbDropdownToggle [class.dropdown-toggle]=\"false\">\n                <a\n                  abpEllipsis=\"210px\"\n                  [abpEllipsisEnabled]=\"isDropdownChildDynamic\"\n                  role=\"button\"\n                  class=\"btn d-block text-left dropdown-toggle\"\n                >\n                  <i *ngIf=\"child.iconClass\" [ngClass]=\"child.iconClass\"></i>\n                  {{ child.name | abpLocalization }}\n                </a>\n              </div>\n              <div\n                #childrenContainer\n                class=\"dropdown-menu border-0 shadow-sm\"\n                [class.abp-collapsed-height]=\"smallScreen\"\n                [class.d-block]=\"smallScreen\"\n                [class.abp-mh-25]=\"smallScreen && dropdownSubmenu.isOpen()\"\n              >\n                <ng-template\n                  ngFor\n                  [ngForOf]=\"child.children\"\n                  [ngForTrackBy]=\"trackByFn\"\n                  [ngForTemplate]=\"childWrapper\"\n                ></ng-template>\n              </div>\n            </div>\n          </ng-template>\n        </ul>\n\n        <ul class=\"navbar-nav\">\n          <ng-container\n            *ngFor=\"let element of rightPartElements; trackBy: trackElementByFn\"\n            [ngTemplateOutlet]=\"element\"\n          ></ng-container>\n        </ul>\n      </ng-template>\n    </div>\n  </div>\n</nav>\n\n<div [@slideFromBottom]=\"outlet && outlet.activatedRoute && outlet.activatedRoute.routeConfig.path\" class=\"container\">\n  <router-outlet #outlet=\"outlet\"></router-outlet>\n</div>\n\n<abp-confirmation></abp-confirmation>\n<abp-toast></abp-toast>\n\n<ng-template #appName>\n  {{ appInfo.name }}\n</ng-template>\n\n<ng-template #language>\n  <li *ngIf=\"(dropdownLanguages$ | async)?.length > 0\" class=\"nav-item\">\n    <div class=\"dropdown\" ngbDropdown #languageDropdown=\"ngbDropdown\" display=\"static\">\n      <a\n        ngbDropdownToggle\n        class=\"nav-link\"\n        href=\"javascript:void(0)\"\n        role=\"button\"\n        id=\"dropdownMenuLink\"\n        data-toggle=\"dropdown\"\n        aria-haspopup=\"true\"\n        aria-expanded=\"false\"\n      >\n        {{ defaultLanguage$ | async }}\n      </a>\n      <div\n        class=\"dropdown-menu dropdown-menu-right border-0 shadow-sm\"\n        aria-labelledby=\"dropdownMenuLink\"\n        [class.abp-collapsed-height]=\"smallScreen\"\n        [class.d-block]=\"smallScreen\"\n        [class.abp-mh-25]=\"smallScreen && languageDropdown.isOpen()\"\n      >\n        <a\n          *ngFor=\"let lang of dropdownLanguages$ | async\"\n          href=\"javascript:void(0)\"\n          class=\"dropdown-item\"\n          (click)=\"onChangeLang(lang.cultureName)\"\n          >{{ lang?.displayName }}</a\n        >\n      </div>\n    </div>\n  </li>\n</ng-template>\n\n<ng-template #currentUser>\n  <li *ngIf=\"(currentUser$ | async)?.isAuthenticated\" class=\"nav-item\">\n    <div ngbDropdown class=\"dropdown\" #currentUserDropdown=\"ngbDropdown\" display=\"static\">\n      <a\n        ngbDropdownToggle\n        class=\"nav-link\"\n        href=\"javascript:void(0)\"\n        role=\"button\"\n        id=\"dropdownMenuLink\"\n        data-toggle=\"dropdown\"\n        aria-haspopup=\"true\"\n        aria-expanded=\"false\"\n      >\n        {{ (currentUser$ | async)?.userName }}\n      </a>\n      <div\n        class=\"dropdown-menu dropdown-menu-right border-0 shadow-sm\"\n        aria-labelledby=\"dropdownMenuLink\"\n        [class.abp-collapsed-height]=\"smallScreen\"\n        [class.d-block]=\"smallScreen\"\n        [class.abp-mh-25]=\"smallScreen && currentUserDropdown.isOpen()\"\n      >\n        <a class=\"dropdown-item\" routerLink=\"/account/manage-profile\"><i class=\"fa fa-cog mr-1\"></i>{{\n          'AbpAccount::ManageYourProfile' | abpLocalization\n        }}</a>\n        <a class=\"dropdown-item\" href=\"javascript:void(0)\" (click)=\"logout()\"><i class=\"fa fa-power-off mr-1\"></i>{{\n          'AbpUi::Logout' | abpLocalization\n        }}</a>\n      </div>\n    </div>\n  </li>\n</ng-template>\n",
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9hcHBsaWNhdGlvbi1sYXlvdXQvYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBSUwsV0FBVyxFQUVYLG1CQUFtQixFQUNuQixZQUFZLEVBQ1osV0FBVyxFQUNYLGdCQUFnQixHQUNqQixNQUFNLGNBQWMsQ0FBQztBQUN0QixPQUFPLEVBQUUsa0JBQWtCLEVBQUUsZUFBZSxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDM0UsT0FBTyxFQUVMLFNBQVMsRUFHVCxTQUFTLEVBQ1QsV0FBVyxFQUVYLFNBQVMsR0FHVixNQUFNLGVBQWUsQ0FBQztBQUV2QixPQUFPLEVBQUUsUUFBUSxFQUFFLFdBQVcsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQzVELE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUNuRCxPQUFPLE9BQU8sTUFBTSxjQUFjLENBQUM7QUFDbkMsT0FBTyxFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDN0MsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDM0QsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBQ3RCLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUVyRCxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBTzNDLE1BQU0sT0FBTywwQkFBMEI7Ozs7OztJQTZEckMsWUFBb0IsS0FBWSxFQUFVLFlBQTBCLEVBQVUsUUFBbUI7UUFBN0UsVUFB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QUMsU0FBUztZQUFFLE9BQU8sR0FBRyxDQUFDO1FBRTlCLElBQUksR0FBRyxDQUFDLFFBQVEsSUFBSSxHQUFHLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUN2QyxHQUFHLENBQUMsUUFBUSxHQUFHLGdCQUFnQixDQUFDLEdBQUcsQ0FBQyxRQUFRLENBQUMsQ0FBQztTQUMvQztRQUVELE9BQU8sQ0FBQyxHQUFHLEdBQUcsRUFBRSxHQUFHLENBQUMsQ0FBQztJQUN2QixDQUFDLEdBQUUsRUFBRSxDQUFDLENBQUM7QUFDVCxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcbiAgQUJQLFxuICBBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24sXG4gIENvbmZpZyxcbiAgQ29uZmlnU3RhdGUsXG4gIGVMYXlvdXRUeXBlLFxuICBHZXRBcHBDb25maWd1cmF0aW9uLFxuICBTZXNzaW9uU3RhdGUsXG4gIFNldExhbmd1YWdlLFxuICB0YWtlVW50aWxEZXN0cm95LFxufSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgY29sbGFwc2VXaXRoTWFyZ2luLCBzbGlkZUZyb21Cb3R0b20gfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQge1xuICBBZnRlclZpZXdJbml0LFxuICBDb21wb25lbnQsXG4gIE9uRGVzdHJveSxcbiAgUXVlcnlMaXN0LFxuICBSZW5kZXJlcjIsXG4gIFRlbXBsYXRlUmVmLFxuICBUcmFja0J5RnVuY3Rpb24sXG4gIFZpZXdDaGlsZCxcbiAgVmlld0NoaWxkcmVuLFxuICBFbGVtZW50UmVmLFxufSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE5nYkRyb3Bkb3duIH0gZnJvbSAnQG5nLWJvb3RzdHJhcC9uZy1ib290c3RyYXAnO1xuaW1wb3J0IHsgTmF2aWdhdGUsIFJvdXRlclN0YXRlIH0gZnJvbSAnQG5neHMvcm91dGVyLXBsdWdpbic7XG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgT0F1dGhTZXJ2aWNlIH0gZnJvbSAnYW5ndWxhci1vYXV0aDItb2lkYyc7XG5pbXBvcnQgY29tcGFyZSBmcm9tICdqdXN0LWNvbXBhcmUnO1xuaW1wb3J0IHsgZnJvbUV2ZW50LCBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBkZWJvdW5jZVRpbWUsIGZpbHRlciwgbWFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xuaW1wb3J0IHsgQWRkTmF2aWdhdGlvbkVsZW1lbnQgfSBmcm9tICcuLi8uLi9hY3Rpb25zJztcbmltcG9ydCB7IExheW91dCB9IGZyb20gJy4uLy4uL21vZGVscy9sYXlvdXQnO1xuaW1wb3J0IHsgTGF5b3V0U3RhdGUgfSBmcm9tICcuLi8uLi9zdGF0ZXMnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtbGF5b3V0LWFwcGxpY2F0aW9uJyxcbiAgdGVtcGxhdGVVcmw6ICcuL2FwcGxpY2F0aW9uLWxheW91dC5jb21wb25lbnQuaHRtbCcsXG4gIGFuaW1hdGlvbnM6IFtzbGlkZUZyb21Cb3R0b20sIGNvbGxhcHNlV2l0aE1hcmdpbl0sXG59KVxuZXhwb3J0IGNsYXNzIEFwcGxpY2F0aW9uTGF5b3V0Q29tcG9uZW50IGltcGxlbWVudHMgQWZ0ZXJWaWV3SW5pdCwgT25EZXN0cm95IHtcbiAgLy8gcmVxdWlyZWQgZm9yIGR5bmFtaWMgY29tcG9uZW50XG4gIHN0YXRpYyB0eXBlID0gZUxheW91dFR5cGUuYXBwbGljYXRpb247XG5cbiAgQFNlbGVjdChDb25maWdTdGF0ZS5nZXRPbmUoJ3JvdXRlcycpKVxuICByb3V0ZXMkOiBPYnNlcnZhYmxlPEFCUC5GdWxsUm91dGVbXT47XG5cbiAgQFNlbGVjdChDb25maWdTdGF0ZS5nZXRPbmUoJ2N1cnJlbnRVc2VyJykpXG4gIGN1cnJlbnRVc2VyJDogT2JzZXJ2YWJsZTxBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24uQ3VycmVudFVzZXI+O1xuXG4gIEBTZWxlY3QoQ29uZmlnU3RhdGUuZ2V0RGVlcCgnbG9jYWxpemF0aW9uLmxhbmd1YWdlcycpKVxuICBsYW5ndWFnZXMkOiBPYnNlcnZhYmxlPEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbi5MYW5ndWFnZVtdPjtcblxuICBAU2VsZWN0KExheW91dFN0YXRlLmdldE5hdmlnYXRpb25FbGVtZW50cylcbiAgbmF2RWxlbWVudHMkOiBPYnNlcnZhYmxlPExheW91dC5OYXZpZ2F0aW9uRWxlbWVudFtdPjtcblxuICBAVmlld0NoaWxkKCdjdXJyZW50VXNlcicsIHsgc3RhdGljOiBmYWxzZSwgcmVhZDogVGVtcGxhdGVSZWYgfSlcbiAgY3VycmVudFVzZXJSZWY6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgQFZpZXdDaGlsZCgnbGFuZ3VhZ2UnLCB7IHN0YXRpYzogZmFsc2UsIHJlYWQ6IFRlbXBsYXRlUmVmIH0pXG4gIGxhbmd1YWdlUmVmOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIGlzRHJvcGRvd25DaGlsZER5bmFtaWM6IGJvb2xlYW47XG5cbiAgaXNDb2xsYXBzZWQgPSB0cnVlO1xuXG4gIHNtYWxsU2NyZWVuOiBib29sZWFuOyAvLyBkbyBub3Qgc2V0IHRydWUgb3IgZmFsc2VcblxuICBnZXQgYXBwSW5mbygpOiBDb25maWcuQXBwbGljYXRpb24ge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldEFwcGxpY2F0aW9uSW5mbyk7XG4gIH1cblxuICBnZXQgdmlzaWJsZVJvdXRlcyQoKTogT2JzZXJ2YWJsZTxBQlAuRnVsbFJvdXRlW10+IHtcbiAgICByZXR1cm4gdGhpcy5yb3V0ZXMkLnBpcGUobWFwKHJvdXRlcyA9PiBnZXRWaXNpYmxlUm91dGVzKHJvdXRlcykpKTtcbiAgfVxuXG4gIGdldCBkZWZhdWx0TGFuZ3VhZ2UkKCk6IE9ic2VydmFibGU8c3RyaW5nPiB7XG4gICAgcmV0dXJuIHRoaXMubGFuZ3VhZ2VzJC5waXBlKFxuICAgICAgbWFwKFxuICAgICAgICBsYW5ndWFnZXMgPT4gc25xKCgpID0+IGxhbmd1YWdlcy5maW5kKGxhbmcgPT4gbGFuZy5jdWx0dXJlTmFtZSA9PT0gdGhpcy5zZWxlY3RlZExhbmdDdWx0dXJlKS5kaXNwbGF5TmFtZSksXG4gICAgICAgICcnLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgZ2V0IGRyb3Bkb3duTGFuZ3VhZ2VzJCgpOiBPYnNlcnZhYmxlPEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbi5MYW5ndWFnZVtdPiB7XG4gICAgcmV0dXJuIHRoaXMubGFuZ3VhZ2VzJC5waXBlKFxuICAgICAgbWFwKGxhbmd1YWdlcyA9PiBzbnEoKCkgPT4gbGFuZ3VhZ2VzLmZpbHRlcihsYW5nID0+IGxhbmcuY3VsdHVyZU5hbWUgIT09IHRoaXMuc2VsZWN0ZWRMYW5nQ3VsdHVyZSkpLCBbXSksXG4gICAgKTtcbiAgfVxuXG4gIGdldCBzZWxlY3RlZExhbmdDdWx0dXJlKCk6IHN0cmluZyB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoU2Vzc2lvblN0YXRlLmdldExhbmd1YWdlKTtcbiAgfVxuXG4gIHJpZ2h0UGFydEVsZW1lbnRzOiBUZW1wbGF0ZVJlZjxhbnk+W10gPSBbXTtcblxuICB0cmFja0J5Rm46IFRyYWNrQnlGdW5jdGlvbjxBQlAuRnVsbFJvdXRlPiA9IChfLCBpdGVtKSA9PiBpdGVtLm5hbWU7XG5cbiAgdHJhY2tFbGVtZW50QnlGbjogVHJhY2tCeUZ1bmN0aW9uPEFCUC5GdWxsUm91dGU+ID0gKF8sIGVsZW1lbnQpID0+IGVsZW1lbnQ7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUsIHByaXZhdGUgb2F1dGhTZXJ2aWNlOiBPQXV0aFNlcnZpY2UsIHByaXZhdGUgcmVuZGVyZXI6IFJlbmRlcmVyMikge31cblxuICBwcml2YXRlIGNoZWNrV2luZG93V2lkdGgoKSB7XG4gICAgc2V0VGltZW91dCgoKSA9PiB7XG4gICAgICBpZiAod2luZG93LmlubmVyV2lkdGggPCA3NjgpIHtcbiAgICAgICAgdGhpcy5pc0Ryb3Bkb3duQ2hpbGREeW5hbWljID0gZmFsc2U7XG4gICAgICAgIGlmICh0aGlzLnNtYWxsU2NyZWVuID09PSBmYWxzZSkge1xuICAgICAgICAgIHRoaXMuaXNDb2xsYXBzZWQgPSBmYWxzZTtcbiAgICAgICAgICBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgICAgICAgIHRoaXMuaXNDb2xsYXBzZWQgPSB0cnVlO1xuICAgICAgICAgIH0sIDEwMCk7XG4gICAgICAgIH1cbiAgICAgICAgdGhpcy5zbWFsbFNjcmVlbiA9IHRydWU7XG4gICAgICB9IGVsc2Uge1xuICAgICAgICB0aGlzLmlzRHJvcGRvd25DaGlsZER5bmFtaWMgPSB0cnVlO1xuICAgICAgICB0aGlzLnNtYWxsU2NyZWVuID0gZmFsc2U7XG4gICAgICB9XG4gICAgfSwgMCk7XG4gIH1cblxuICBuZ0FmdGVyVmlld0luaXQoKSB7XG4gICAgY29uc3QgbmF2aWdhdGlvbnMgPSB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KExheW91dFN0YXRlLmdldE5hdmlnYXRpb25FbGVtZW50cykubWFwKCh7IG5hbWUgfSkgPT4gbmFtZSk7XG5cbiAgICBpZiAobmF2aWdhdGlvbnMuaW5kZXhPZignTGFuZ3VhZ2VSZWYnKSA8IDApIHtcbiAgICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2goXG4gICAgICAgIG5ldyBBZGROYXZpZ2F0aW9uRWxlbWVudChbXG4gICAgICAgICAgeyBlbGVtZW50OiB0aGlzLmxhbmd1YWdlUmVmLCBvcmRlcjogNCwgbmFtZTogJ0xhbmd1YWdlUmVmJyB9LFxuICAgICAgICAgIHsgZWxlbWVudDogdGhpcy5jdXJyZW50VXNlclJlZiwgb3JkZXI6IDUsIG5hbWU6ICdDdXJyZW50VXNlclJlZicgfSxcbiAgICAgICAgXSksXG4gICAgICApO1xuICAgIH1cblxuICAgIHRoaXMubmF2RWxlbWVudHMkXG4gICAgICAucGlwZShcbiAgICAgICAgbWFwKGVsZW1lbnRzID0+IGVsZW1lbnRzLm1hcCgoeyBlbGVtZW50IH0pID0+IGVsZW1lbnQpKSxcbiAgICAgICAgZmlsdGVyKGVsZW1lbnRzID0+ICFjb21wYXJlKGVsZW1lbnRzLCB0aGlzLnJpZ2h0UGFydEVsZW1lbnRzKSksXG4gICAgICAgIHRha2VVbnRpbERlc3Ryb3kodGhpcyksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKGVsZW1lbnRzID0+IHtcbiAgICAgICAgc2V0VGltZW91dCgoKSA9PiAodGhpcy5yaWdodFBhcnRFbGVtZW50cyA9IGVsZW1lbnRzKSwgMCk7XG4gICAgICB9KTtcblxuICAgIHRoaXMuY2hlY2tXaW5kb3dXaWR0aCgpO1xuXG4gICAgZnJvbUV2ZW50KHdpbmRvdywgJ3Jlc2l6ZScpXG4gICAgICAucGlwZShcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKSxcbiAgICAgICAgZGVib3VuY2VUaW1lKDE1MCksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgdGhpcy5jaGVja1dpbmRvd1dpZHRoKCk7XG4gICAgICB9KTtcbiAgfVxuXG4gIG5nT25EZXN0cm95KCkge31cblxuICBvbkNoYW5nZUxhbmcoY3VsdHVyZU5hbWU6IHN0cmluZykge1xuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IFNldExhbmd1YWdlKGN1bHR1cmVOYW1lKSk7XG4gIH1cblxuICBsb2dvdXQoKSB7XG4gICAgdGhpcy5vYXV0aFNlcnZpY2UubG9nT3V0KCk7XG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChcbiAgICAgIG5ldyBOYXZpZ2F0ZShbJy8nXSwgbnVsbCwge1xuICAgICAgICBzdGF0ZTogeyByZWRpcmVjdFVybDogdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChSb3V0ZXJTdGF0ZSkuc3RhdGUudXJsIH0sXG4gICAgICB9KSxcbiAgICApO1xuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IEdldEFwcENvbmZpZ3VyYXRpb24oKSk7XG4gIH1cblxuICBvcGVuQ2hhbmdlKGV2ZW50OiBib29sZWFuLCBjaGlsZHJlbkNvbnRhaW5lcjogSFRNTERpdkVsZW1lbnQpIHtcbiAgICBpZiAoIWV2ZW50KSB7XG4gICAgICBPYmplY3Qua2V5cyhjaGlsZHJlbkNvbnRhaW5lci5zdHlsZSlcbiAgICAgICAgLmZpbHRlcihrZXkgPT4gTnVtYmVyLmlzSW50ZWdlcigra2V5KSlcbiAgICAgICAgLmZvckVhY2goa2V5ID0+IHtcbiAgICAgICAgICB0aGlzLnJlbmRlcmVyLnJlbW92ZVN0eWxlKGNoaWxkcmVuQ29udGFpbmVyLCBjaGlsZHJlbkNvbnRhaW5lci5zdHlsZVtrZXldKTtcbiAgICAgICAgfSk7XG4gICAgICB0aGlzLnJlbmRlcmVyLnJlbW92ZVN0eWxlKGNoaWxkcmVuQ29udGFpbmVyLCAnbGVmdCcpO1xuICAgIH1cbiAgfVxufVxuXG5mdW5jdGlvbiBnZXRWaXNpYmxlUm91dGVzKHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdKSB7XG4gIHJldHVybiByb3V0ZXMucmVkdWNlKChhY2MsIHZhbCkgPT4ge1xuICAgIGlmICh2YWwuaW52aXNpYmxlKSByZXR1cm4gYWNjO1xuXG4gICAgaWYgKHZhbC5jaGlsZHJlbiAmJiB2YWwuY2hpbGRyZW4ubGVuZ3RoKSB7XG4gICAgICB2YWwuY2hpbGRyZW4gPSBnZXRWaXNpYmxlUm91dGVzKHZhbC5jaGlsZHJlbik7XG4gICAgfVxuXG4gICAgcmV0dXJuIFsuLi5hY2MsIHZhbF07XG4gIH0sIFtdKTtcbn1cbiJdfQ==