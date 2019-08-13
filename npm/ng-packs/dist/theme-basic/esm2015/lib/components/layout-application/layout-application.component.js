/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { ConfigGetAppConfiguration, ConfigState, SessionSetLanguage, SessionState, takeUntilDestroy, } from '@abp/ng.core';
import { Component, QueryList, TemplateRef, ViewChild, ViewChildren, } from '@angular/core';
import { NgbDropdown } from '@ng-bootstrap/ng-bootstrap';
import { Navigate, RouterState } from '@ngxs/router-plugin';
import { Select, Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import compare from 'just-compare';
import { fromEvent, Observable } from 'rxjs';
import { debounceTime, filter, map } from 'rxjs/operators';
import snq from 'snq';
import { LayoutAddNavigationElement } from '../../actions';
import { LayoutState } from '../../states';
export class LayoutApplicationComponent {
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
            this.navbarRootDropdowns.forEach((/**
             * @param {?} item
             * @return {?}
             */
            item => {
                item.close();
            }));
            if (window.innerWidth < 768) {
                this.isDropdownChildDynamic = false;
            }
            else {
                this.isDropdownChildDynamic = true;
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
            this.store.dispatch(new LayoutAddNavigationElement([
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
            .pipe(takeUntilDestroy(this), debounceTime(250))
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
        this.store.dispatch(new SessionSetLanguage(cultureName));
        this.store.dispatch(new ConfigGetAppConfiguration());
    }
    /**
     * @return {?}
     */
    logout() {
        this.oauthService.logOut();
        this.store.dispatch(new Navigate(['/account/login'], null, {
            state: { redirectUrl: this.store.selectSnapshot(RouterState).state.url },
        }));
        this.store.dispatch(new ConfigGetAppConfiguration());
    }
}
// required for dynamic component
LayoutApplicationComponent.type = "application" /* application */;
LayoutApplicationComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-layout-application',
                template: "<abp-layout>\n  <ul class=\"navbar-nav mr-auto\">\n    <ng-container\n      *ngFor=\"let route of visibleRoutes$ | async; trackBy: trackByFn\"\n      [ngTemplateOutlet]=\"route?.children?.length ? dropdownLink : defaultLink\"\n      [ngTemplateOutletContext]=\"{ $implicit: route }\"\n    >\n    </ng-container>\n\n    <ng-template #defaultLink let-route>\n      <li class=\"nav-item\" [abpPermission]=\"route.requiredPolicy\">\n        <a class=\"nav-link\" [routerLink]=\"[route.url]\">{{ route.name | abpLocalization }}</a>\n      </li>\n    </ng-template>\n\n    <ng-template #dropdownLink let-route>\n      <li\n        #navbarRootDropdown\n        class=\"nav-item dropdown\"\n        ngbDropdown\n        display=\"static\"\n        [abpPermission]=\"route.requiredPolicy\"\n        [abpVisibility]=\"routeContainer\"\n      >\n        <a ngbDropdownToggle class=\"nav-link dropdown-toggle\" data-toggle=\"dropdown\">\n          {{ route.name | abpLocalization }}\n        </a>\n        <div #routeContainer ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n          <ng-template\n            #forTemplate\n            ngFor\n            [ngForOf]=\"route.children\"\n            [ngForTrackBy]=\"trackByFn\"\n            [ngForTemplate]=\"childWrapper\"\n          ></ng-template>\n        </div>\n      </li>\n    </ng-template>\n\n    <ng-template #childWrapper let-child>\n      <ng-template\n        [ngTemplateOutlet]=\"child?.children?.length ? dropdownChild : defaultChild\"\n        [ngTemplateOutletContext]=\"{ $implicit: child }\"\n      ></ng-template>\n    </ng-template>\n\n    <ng-template #defaultChild let-child>\n      <div class=\"dropdown-submenu\" [abpPermission]=\"child.requiredPolicy\">\n        <a class=\"dropdown-item py-2 px-2\" [routerLink]=\"[child.url]\">{{ child.name | abpLocalization }}</a>\n      </div>\n    </ng-template>\n\n    <ng-template #dropdownChild let-child>\n      <div\n        [abpVisibility]=\"childrenContainer\"\n        class=\"dropdown-submenu\"\n        ngbDropdown\n        [display]=\"isDropdownChildDynamic ? 'dynamic' : 'static'\"\n        placement=\"right-top\"\n        [abpPermission]=\"child.requiredPolicy\"\n      >\n        <div ngbDropdownToggle [class.dropdown-toggle]=\"false\">\n          <a\n            abpEllipsis=\"140px\"\n            [abpEllipsisEnabled]=\"isDropdownChildDynamic\"\n            role=\"button\"\n            class=\"btn d-block text-left py-2 px-2 dropdown-toggle\"\n          >\n            {{ child.name | abpLocalization }}\n          </a>\n        </div>\n        <div #childrenContainer ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n          <ng-template\n            ngFor\n            [ngForOf]=\"child.children\"\n            [ngForTrackBy]=\"trackByFn\"\n            [ngForTemplate]=\"childWrapper\"\n          ></ng-template>\n        </div>\n      </div>\n    </ng-template>\n  </ul>\n\n  <ul class=\"navbar-nav ml-auto\">\n    <ng-container\n      *ngFor=\"let element of rightPartElements; trackBy: trackElementByFn\"\n      [ngTemplateOutlet]=\"element\"\n    ></ng-container>\n  </ul>\n</abp-layout>\n\n<ng-template #language>\n  <li class=\"nav-item dropdown\" ngbDropdown>\n    <a ngbDropdownToggle class=\"nav-link dropdown-toggle\" data-toggle=\"dropdown\">\n      {{ defaultLanguage$ | async }}\n    </a>\n    <div ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n      <a\n        *ngFor=\"let lang of dropdownLanguages$ | async\"\n        class=\"dropdown-item\"\n        (click)=\"onChangeLang(lang.cultureName)\"\n        >{{ lang?.displayName }}</a\n      >\n    </div>\n  </li>\n</ng-template>\n\n<ng-template #currentUser>\n  <li *ngIf=\"(currentUser$ | async)?.isAuthenticated\" class=\"nav-item dropdown\" ngbDropdown>\n    <a ngbDropdownToggle class=\"nav-link dropdown-toggle\" data-toggle=\"dropdown\">\n      {{ (currentUser$ | async)?.userName }}\n    </a>\n    <div ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n      <a class=\"dropdown-item pointer\" (click)=\"isOpenChangePassword = true\">Change Password</a>\n      <a class=\"dropdown-item pointer\" (click)=\"isOpenProfile = true\">My Profile</a>\n      <a class=\"dropdown-item pointer\" (click)=\"logout()\">Logout</a>\n    </div>\n  </li>\n\n  <abp-change-password [(visible)]=\"isOpenChangePassword\"></abp-change-password>\n\n  <abp-profile [(visible)]=\"isOpenProfile\"></abp-profile>\n</ng-template>\n"
            }] }
];
/** @nocollapse */
LayoutApplicationComponent.ctorParameters = () => [
    { type: Store },
    { type: OAuthService }
];
LayoutApplicationComponent.propDecorators = {
    currentUserRef: [{ type: ViewChild, args: ['currentUser', { static: false, read: TemplateRef },] }],
    languageRef: [{ type: ViewChild, args: ['language', { static: false, read: TemplateRef },] }],
    navbarRootDropdowns: [{ type: ViewChildren, args: ['navbarRootDropdown', { read: NgbDropdown },] }]
};
tslib_1.__decorate([
    Select(ConfigState.getOne('routes')),
    tslib_1.__metadata("design:type", Observable)
], LayoutApplicationComponent.prototype, "routes$", void 0);
tslib_1.__decorate([
    Select(ConfigState.getOne('currentUser')),
    tslib_1.__metadata("design:type", Observable)
], LayoutApplicationComponent.prototype, "currentUser$", void 0);
tslib_1.__decorate([
    Select(ConfigState.getDeep('localization.languages')),
    tslib_1.__metadata("design:type", Observable)
], LayoutApplicationComponent.prototype, "languages$", void 0);
tslib_1.__decorate([
    Select(LayoutState.getNavigationElements),
    tslib_1.__metadata("design:type", Observable)
], LayoutApplicationComponent.prototype, "navElements$", void 0);
if (false) {
    /** @type {?} */
    LayoutApplicationComponent.type;
    /** @type {?} */
    LayoutApplicationComponent.prototype.routes$;
    /** @type {?} */
    LayoutApplicationComponent.prototype.currentUser$;
    /** @type {?} */
    LayoutApplicationComponent.prototype.languages$;
    /** @type {?} */
    LayoutApplicationComponent.prototype.navElements$;
    /** @type {?} */
    LayoutApplicationComponent.prototype.currentUserRef;
    /** @type {?} */
    LayoutApplicationComponent.prototype.languageRef;
    /** @type {?} */
    LayoutApplicationComponent.prototype.navbarRootDropdowns;
    /** @type {?} */
    LayoutApplicationComponent.prototype.isOpenChangePassword;
    /** @type {?} */
    LayoutApplicationComponent.prototype.isOpenProfile;
    /** @type {?} */
    LayoutApplicationComponent.prototype.isDropdownChildDynamic;
    /** @type {?} */
    LayoutApplicationComponent.prototype.rightPartElements;
    /** @type {?} */
    LayoutApplicationComponent.prototype.trackByFn;
    /** @type {?} */
    LayoutApplicationComponent.prototype.trackElementByFn;
    /**
     * @type {?}
     * @private
     */
    LayoutApplicationComponent.prototype.store;
    /**
     * @type {?}
     * @private
     */
    LayoutApplicationComponent.prototype.oauthService;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF5b3V0LWFwcGxpY2F0aW9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9sYXlvdXQtYXBwbGljYXRpb24vbGF5b3V0LWFwcGxpY2F0aW9uLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFHTCx5QkFBeUIsRUFDekIsV0FBVyxFQUVYLGtCQUFrQixFQUNsQixZQUFZLEVBQ1osZ0JBQWdCLEdBQ2pCLE1BQU0sY0FBYyxDQUFDO0FBQ3RCLE9BQU8sRUFFTCxTQUFTLEVBRVQsU0FBUyxFQUNULFdBQVcsRUFFWCxTQUFTLEVBQ1QsWUFBWSxHQUNiLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUN6RCxPQUFPLEVBQUUsUUFBUSxFQUFFLFdBQVcsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQzVELE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUNuRCxPQUFPLE9BQU8sTUFBTSxjQUFjLENBQUM7QUFDbkMsT0FBTyxFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDN0MsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDM0QsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBQ3RCLE9BQU8sRUFBRSwwQkFBMEIsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUzRCxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBTTNDLE1BQU0sT0FBTywwQkFBMEI7Ozs7O0lBNERyQyxZQUFvQixLQUFZLEVBQVUsWUFBMEI7UUFBaEQsVUFBSyxHQUFMLEtBQUss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QzREOztJQUU1RCxrREFDcUQ7O0lBRXJELG9EQUNpQzs7SUFFakMsaURBQzhCOztJQUU5Qix5REFDNEM7O0lBRTVDLDBEQUFzQzs7SUFFdEMsbURBQStCOztJQUUvQiw0REFBZ0M7O0lBeUJoQyx1REFBMkM7O0lBRTNDLCtDQUFtRTs7SUFFbkUsc0RBQTJFOzs7OztJQUUvRCwyQ0FBb0I7Ozs7O0lBQUUsa0RBQWtDOzs7Ozs7QUFtRXRFLFNBQVMsZ0JBQWdCLENBQUMsTUFBdUI7SUFDL0MsT0FBTyxNQUFNLENBQUMsTUFBTTs7Ozs7SUFBQyxDQUFDLEdBQUcsRUFBRSxHQUFHLEVBQUUsRUFBRTtRQUNoQyxJQUFJLEdBQUcsQ0FBQyxTQUFTO1lBQUUsT0FBTyxHQUFHLENBQUM7UUFFOUIsSUFBSSxHQUFHLENBQUMsUUFBUSxJQUFJLEdBQUcsQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO1lBQ3ZDLEdBQUcsQ0FBQyxRQUFRLEdBQUcsZ0JBQWdCLENBQUMsR0FBRyxDQUFDLFFBQVEsQ0FBQyxDQUFDO1NBQy9DO1FBRUQsT0FBTyxDQUFDLEdBQUcsR0FBRyxFQUFFLEdBQUcsQ0FBQyxDQUFDO0lBQ3ZCLENBQUMsR0FBRSxFQUFFLENBQUMsQ0FBQztBQUNULENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQge1xuICBBQlAsXG4gIEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbixcbiAgQ29uZmlnR2V0QXBwQ29uZmlndXJhdGlvbixcbiAgQ29uZmlnU3RhdGUsXG4gIGVMYXlvdXRUeXBlLFxuICBTZXNzaW9uU2V0TGFuZ3VhZ2UsXG4gIFNlc3Npb25TdGF0ZSxcbiAgdGFrZVVudGlsRGVzdHJveSxcbn0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7XG4gIEFmdGVyVmlld0luaXQsXG4gIENvbXBvbmVudCxcbiAgT25EZXN0cm95LFxuICBRdWVyeUxpc3QsXG4gIFRlbXBsYXRlUmVmLFxuICBUcmFja0J5RnVuY3Rpb24sXG4gIFZpZXdDaGlsZCxcbiAgVmlld0NoaWxkcmVuLFxufSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE5nYkRyb3Bkb3duIH0gZnJvbSAnQG5nLWJvb3RzdHJhcC9uZy1ib290c3RyYXAnO1xuaW1wb3J0IHsgTmF2aWdhdGUsIFJvdXRlclN0YXRlIH0gZnJvbSAnQG5neHMvcm91dGVyLXBsdWdpbic7XG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgT0F1dGhTZXJ2aWNlIH0gZnJvbSAnYW5ndWxhci1vYXV0aDItb2lkYyc7XG5pbXBvcnQgY29tcGFyZSBmcm9tICdqdXN0LWNvbXBhcmUnO1xuaW1wb3J0IHsgZnJvbUV2ZW50LCBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBkZWJvdW5jZVRpbWUsIGZpbHRlciwgbWFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xuaW1wb3J0IHsgTGF5b3V0QWRkTmF2aWdhdGlvbkVsZW1lbnQgfSBmcm9tICcuLi8uLi9hY3Rpb25zJztcbmltcG9ydCB7IExheW91dCB9IGZyb20gJy4uLy4uL21vZGVscy9sYXlvdXQnO1xuaW1wb3J0IHsgTGF5b3V0U3RhdGUgfSBmcm9tICcuLi8uLi9zdGF0ZXMnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtbGF5b3V0LWFwcGxpY2F0aW9uJyxcbiAgdGVtcGxhdGVVcmw6ICcuL2xheW91dC1hcHBsaWNhdGlvbi5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIExheW91dEFwcGxpY2F0aW9uQ29tcG9uZW50IGltcGxlbWVudHMgQWZ0ZXJWaWV3SW5pdCwgT25EZXN0cm95IHtcbiAgLy8gcmVxdWlyZWQgZm9yIGR5bmFtaWMgY29tcG9uZW50XG4gIHN0YXRpYyB0eXBlID0gZUxheW91dFR5cGUuYXBwbGljYXRpb247XG5cbiAgQFNlbGVjdChDb25maWdTdGF0ZS5nZXRPbmUoJ3JvdXRlcycpKVxuICByb3V0ZXMkOiBPYnNlcnZhYmxlPEFCUC5GdWxsUm91dGVbXT47XG5cbiAgQFNlbGVjdChDb25maWdTdGF0ZS5nZXRPbmUoJ2N1cnJlbnRVc2VyJykpXG4gIGN1cnJlbnRVc2VyJDogT2JzZXJ2YWJsZTxBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24uQ3VycmVudFVzZXI+O1xuXG4gIEBTZWxlY3QoQ29uZmlnU3RhdGUuZ2V0RGVlcCgnbG9jYWxpemF0aW9uLmxhbmd1YWdlcycpKVxuICBsYW5ndWFnZXMkOiBPYnNlcnZhYmxlPEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbi5MYW5ndWFnZVtdPjtcblxuICBAU2VsZWN0KExheW91dFN0YXRlLmdldE5hdmlnYXRpb25FbGVtZW50cylcbiAgbmF2RWxlbWVudHMkOiBPYnNlcnZhYmxlPExheW91dC5OYXZpZ2F0aW9uRWxlbWVudFtdPjtcblxuICBAVmlld0NoaWxkKCdjdXJyZW50VXNlcicsIHsgc3RhdGljOiBmYWxzZSwgcmVhZDogVGVtcGxhdGVSZWYgfSlcbiAgY3VycmVudFVzZXJSZWY6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgQFZpZXdDaGlsZCgnbGFuZ3VhZ2UnLCB7IHN0YXRpYzogZmFsc2UsIHJlYWQ6IFRlbXBsYXRlUmVmIH0pXG4gIGxhbmd1YWdlUmVmOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBWaWV3Q2hpbGRyZW4oJ25hdmJhclJvb3REcm9wZG93bicsIHsgcmVhZDogTmdiRHJvcGRvd24gfSlcbiAgbmF2YmFyUm9vdERyb3Bkb3duczogUXVlcnlMaXN0PE5nYkRyb3Bkb3duPjtcblxuICBpc09wZW5DaGFuZ2VQYXNzd29yZDogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIGlzT3BlblByb2ZpbGU6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBpc0Ryb3Bkb3duQ2hpbGREeW5hbWljOiBib29sZWFuO1xuXG4gIGdldCB2aXNpYmxlUm91dGVzJCgpOiBPYnNlcnZhYmxlPEFCUC5GdWxsUm91dGVbXT4ge1xuICAgIHJldHVybiB0aGlzLnJvdXRlcyQucGlwZShtYXAocm91dGVzID0+IGdldFZpc2libGVSb3V0ZXMocm91dGVzKSkpO1xuICB9XG5cbiAgZ2V0IGRlZmF1bHRMYW5ndWFnZSQoKTogT2JzZXJ2YWJsZTxzdHJpbmc+IHtcbiAgICByZXR1cm4gdGhpcy5sYW5ndWFnZXMkLnBpcGUoXG4gICAgICBtYXAoXG4gICAgICAgIGxhbmd1YWdlcyA9PiBzbnEoKCkgPT4gbGFuZ3VhZ2VzLmZpbmQobGFuZyA9PiBsYW5nLmN1bHR1cmVOYW1lID09PSB0aGlzLnNlbGVjdGVkTGFuZ0N1bHR1cmUpLmRpc3BsYXlOYW1lKSxcbiAgICAgICAgJycsXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBnZXQgZHJvcGRvd25MYW5ndWFnZXMkKCk6IE9ic2VydmFibGU8QXBwbGljYXRpb25Db25maWd1cmF0aW9uLkxhbmd1YWdlW10+IHtcbiAgICByZXR1cm4gdGhpcy5sYW5ndWFnZXMkLnBpcGUoXG4gICAgICBtYXAobGFuZ3VhZ2VzID0+IHNucSgoKSA9PiBsYW5ndWFnZXMuZmlsdGVyKGxhbmcgPT4gbGFuZy5jdWx0dXJlTmFtZSAhPT0gdGhpcy5zZWxlY3RlZExhbmdDdWx0dXJlKSksIFtdKSxcbiAgICApO1xuICB9XG5cbiAgZ2V0IHNlbGVjdGVkTGFuZ0N1bHR1cmUoKTogc3RyaW5nIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChTZXNzaW9uU3RhdGUuZ2V0TGFuZ3VhZ2UpO1xuICB9XG5cbiAgcmlnaHRQYXJ0RWxlbWVudHM6IFRlbXBsYXRlUmVmPGFueT5bXSA9IFtdO1xuXG4gIHRyYWNrQnlGbjogVHJhY2tCeUZ1bmN0aW9uPEFCUC5GdWxsUm91dGU+ID0gKF8sIGl0ZW0pID0+IGl0ZW0ubmFtZTtcblxuICB0cmFja0VsZW1lbnRCeUZuOiBUcmFja0J5RnVuY3Rpb248QUJQLkZ1bGxSb3V0ZT4gPSAoXywgZWxlbWVudCkgPT4gZWxlbWVudDtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSwgcHJpdmF0ZSBvYXV0aFNlcnZpY2U6IE9BdXRoU2VydmljZSkge31cblxuICBwcml2YXRlIGNoZWNrV2luZG93V2lkdGgoKSB7XG4gICAgc2V0VGltZW91dCgoKSA9PiB7XG4gICAgICB0aGlzLm5hdmJhclJvb3REcm9wZG93bnMuZm9yRWFjaChpdGVtID0+IHtcbiAgICAgICAgaXRlbS5jbG9zZSgpO1xuICAgICAgfSk7XG4gICAgICBpZiAod2luZG93LmlubmVyV2lkdGggPCA3NjgpIHtcbiAgICAgICAgdGhpcy5pc0Ryb3Bkb3duQ2hpbGREeW5hbWljID0gZmFsc2U7XG4gICAgICB9IGVsc2Uge1xuICAgICAgICB0aGlzLmlzRHJvcGRvd25DaGlsZER5bmFtaWMgPSB0cnVlO1xuICAgICAgfVxuICAgIH0sIDApO1xuICB9XG5cbiAgbmdBZnRlclZpZXdJbml0KCkge1xuICAgIGNvbnN0IG5hdmlnYXRpb25zID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChMYXlvdXRTdGF0ZS5nZXROYXZpZ2F0aW9uRWxlbWVudHMpLm1hcCgoeyBuYW1lIH0pID0+IG5hbWUpO1xuXG4gICAgaWYgKG5hdmlnYXRpb25zLmluZGV4T2YoJ0xhbmd1YWdlUmVmJykgPCAwKSB7XG4gICAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKFxuICAgICAgICBuZXcgTGF5b3V0QWRkTmF2aWdhdGlvbkVsZW1lbnQoW1xuICAgICAgICAgIHsgZWxlbWVudDogdGhpcy5sYW5ndWFnZVJlZiwgb3JkZXI6IDQsIG5hbWU6ICdMYW5ndWFnZVJlZicgfSxcbiAgICAgICAgICB7IGVsZW1lbnQ6IHRoaXMuY3VycmVudFVzZXJSZWYsIG9yZGVyOiA1LCBuYW1lOiAnQ3VycmVudFVzZXJSZWYnIH0sXG4gICAgICAgIF0pLFxuICAgICAgKTtcbiAgICB9XG5cbiAgICB0aGlzLm5hdkVsZW1lbnRzJFxuICAgICAgLnBpcGUoXG4gICAgICAgIG1hcChlbGVtZW50cyA9PiBlbGVtZW50cy5tYXAoKHsgZWxlbWVudCB9KSA9PiBlbGVtZW50KSksXG4gICAgICAgIGZpbHRlcihlbGVtZW50cyA9PiAhY29tcGFyZShlbGVtZW50cywgdGhpcy5yaWdodFBhcnRFbGVtZW50cykpLFxuICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZShlbGVtZW50cyA9PiB7XG4gICAgICAgIHNldFRpbWVvdXQoKCkgPT4gKHRoaXMucmlnaHRQYXJ0RWxlbWVudHMgPSBlbGVtZW50cyksIDApO1xuICAgICAgfSk7XG5cbiAgICB0aGlzLmNoZWNrV2luZG93V2lkdGgoKTtcblxuICAgIGZyb21FdmVudCh3aW5kb3csICdyZXNpemUnKVxuICAgICAgLnBpcGUoXG4gICAgICAgIHRha2VVbnRpbERlc3Ryb3kodGhpcyksXG4gICAgICAgIGRlYm91bmNlVGltZSgyNTApLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgIHRoaXMuY2hlY2tXaW5kb3dXaWR0aCgpO1xuICAgICAgfSk7XG4gIH1cblxuICBuZ09uRGVzdHJveSgpIHt9XG5cbiAgb25DaGFuZ2VMYW5nKGN1bHR1cmVOYW1lOiBzdHJpbmcpIHtcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBTZXNzaW9uU2V0TGFuZ3VhZ2UoY3VsdHVyZU5hbWUpKTtcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBDb25maWdHZXRBcHBDb25maWd1cmF0aW9uKCkpO1xuICB9XG5cbiAgbG9nb3V0KCkge1xuICAgIHRoaXMub2F1dGhTZXJ2aWNlLmxvZ091dCgpO1xuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2goXG4gICAgICBuZXcgTmF2aWdhdGUoWycvYWNjb3VudC9sb2dpbiddLCBudWxsLCB7XG4gICAgICAgIHN0YXRlOiB7IHJlZGlyZWN0VXJsOiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFJvdXRlclN0YXRlKS5zdGF0ZS51cmwgfSxcbiAgICAgIH0pLFxuICAgICk7XG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgQ29uZmlnR2V0QXBwQ29uZmlndXJhdGlvbigpKTtcbiAgfVxufVxuXG5mdW5jdGlvbiBnZXRWaXNpYmxlUm91dGVzKHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdKSB7XG4gIHJldHVybiByb3V0ZXMucmVkdWNlKChhY2MsIHZhbCkgPT4ge1xuICAgIGlmICh2YWwuaW52aXNpYmxlKSByZXR1cm4gYWNjO1xuXG4gICAgaWYgKHZhbC5jaGlsZHJlbiAmJiB2YWwuY2hpbGRyZW4ubGVuZ3RoKSB7XG4gICAgICB2YWwuY2hpbGRyZW4gPSBnZXRWaXNpYmxlUm91dGVzKHZhbC5jaGlsZHJlbik7XG4gICAgfVxuXG4gICAgcmV0dXJuIFsuLi5hY2MsIHZhbF07XG4gIH0sIFtdKTtcbn1cbiJdfQ==