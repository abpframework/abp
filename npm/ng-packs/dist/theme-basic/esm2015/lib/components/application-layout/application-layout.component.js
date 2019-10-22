/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { GetAppConfiguration, ConfigState, SetLanguage, SessionState, takeUntilDestroy, } from '@abp/ng.core';
import { Component, QueryList, TemplateRef, ViewChild, ViewChildren, } from '@angular/core';
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
}
// required for dynamic component
ApplicationLayoutComponent.type = "application" /* application */;
ApplicationLayoutComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-layout-application',
                template: "<abp-layout>\r\n  <ul class=\"navbar-nav mr-auto\">\r\n    <ng-container\r\n      *ngFor=\"let route of visibleRoutes$ | async; trackBy: trackByFn\"\r\n      [ngTemplateOutlet]=\"route?.children?.length ? dropdownLink : defaultLink\"\r\n      [ngTemplateOutletContext]=\"{ $implicit: route }\"\r\n    >\r\n    </ng-container>\r\n\r\n    <ng-template #defaultLink let-route>\r\n      <li class=\"nav-item\" [abpPermission]=\"route.requiredPolicy\">\r\n        <a class=\"nav-link\" [routerLink]=\"[route.url]\">{{ route.name | abpLocalization }}</a>\r\n      </li>\r\n    </ng-template>\r\n\r\n    <ng-template #dropdownLink let-route>\r\n      <li\r\n        #navbarRootDropdown\r\n        ngbDropdown\r\n        [abpPermission]=\"route.requiredPolicy\"\r\n        [abpVisibility]=\"routeContainer\"\r\n        class=\"nav-item dropdown pointer\"\r\n        display=\"static\"\r\n      >\r\n        <a ngbDropdownToggle class=\"nav-link dropdown-toggle pointer\" data-toggle=\"dropdown\">\r\n          {{ route.name | abpLocalization }}\r\n        </a>\r\n        <div #routeContainer ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\r\n          <ng-template\r\n            #forTemplate\r\n            ngFor\r\n            [ngForOf]=\"route.children\"\r\n            [ngForTrackBy]=\"trackByFn\"\r\n            [ngForTemplate]=\"childWrapper\"\r\n          ></ng-template>\r\n        </div>\r\n      </li>\r\n    </ng-template>\r\n\r\n    <ng-template #childWrapper let-child>\r\n      <ng-template\r\n        [ngTemplateOutlet]=\"child?.children?.length ? dropdownChild : defaultChild\"\r\n        [ngTemplateOutletContext]=\"{ $implicit: child }\"\r\n      ></ng-template>\r\n    </ng-template>\r\n\r\n    <ng-template #defaultChild let-child>\r\n      <div class=\"dropdown-submenu\" [abpPermission]=\"child.requiredPolicy\">\r\n        <a class=\"dropdown-item py-2 px-2\" [routerLink]=\"[child.url]\">\r\n          <i *ngIf=\"child.iconClass\" [ngClass]=\"child.iconClass\"></i>\r\n          {{ child.name | abpLocalization }}</a\r\n        >\r\n      </div>\r\n    </ng-template>\r\n\r\n    <ng-template #dropdownChild let-child>\r\n      <div\r\n        [abpVisibility]=\"childrenContainer\"\r\n        class=\"dropdown-submenu pointer\"\r\n        ngbDropdown\r\n        [display]=\"isDropdownChildDynamic ? 'dynamic' : 'static'\"\r\n        placement=\"right-top\"\r\n        [abpPermission]=\"child.requiredPolicy\"\r\n      >\r\n        <div ngbDropdownToggle [class.dropdown-toggle]=\"false\" class=\"pointer\">\r\n          <a\r\n            abpEllipsis=\"210px\"\r\n            [abpEllipsisEnabled]=\"isDropdownChildDynamic\"\r\n            role=\"button\"\r\n            class=\"btn d-block text-left py-2 px-2 dropdown-toggle\"\r\n          >\r\n            <i *ngIf=\"child.iconClass\" [ngClass]=\"child.iconClass\"></i>\r\n            {{ child.name | abpLocalization }}\r\n          </a>\r\n        </div>\r\n        <div #childrenContainer ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\r\n          <ng-template\r\n            ngFor\r\n            [ngForOf]=\"child.children\"\r\n            [ngForTrackBy]=\"trackByFn\"\r\n            [ngForTemplate]=\"childWrapper\"\r\n          ></ng-template>\r\n        </div>\r\n      </div>\r\n    </ng-template>\r\n  </ul>\r\n\r\n  <ul class=\"navbar-nav ml-auto\">\r\n    <ng-container\r\n      *ngFor=\"let element of rightPartElements; trackBy: trackElementByFn\"\r\n      [ngTemplateOutlet]=\"element\"\r\n    ></ng-container>\r\n  </ul>\r\n</abp-layout>\r\n\r\n<ng-template #language>\r\n  <li class=\"nav-item dropdown pointer\" ngbDropdown>\r\n    <a ngbDropdownToggle class=\"nav-link dropdown-toggle text-white pointer\" data-toggle=\"dropdown\">\r\n      {{ defaultLanguage$ | async }}\r\n    </a>\r\n    <div ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\r\n      <a\r\n        *ngFor=\"let lang of dropdownLanguages$ | async\"\r\n        class=\"dropdown-item\"\r\n        (click)=\"onChangeLang(lang.cultureName)\"\r\n        >{{ lang?.displayName }}</a\r\n      >\r\n    </div>\r\n  </li>\r\n</ng-template>\r\n\r\n<ng-template #currentUser>\r\n  <li *ngIf=\"(currentUser$ | async)?.isAuthenticated\" class=\"nav-item dropdown pointer\" ngbDropdown>\r\n    <a ngbDropdownToggle class=\"nav-link dropdown-toggle text-white pointer\" data-toggle=\"dropdown\">\r\n      {{ (currentUser$ | async)?.userName }}\r\n    </a>\r\n    <div ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\r\n      <a class=\"dropdown-item pointer\" routerLink=\"/account/manage-profile\">{{\r\n        'AbpAccount::ManageYourProfile' | abpLocalization\r\n      }}</a>\r\n      <a class=\"dropdown-item pointer\" (click)=\"logout()\">{{ 'AbpUi::Logout' | abpLocalization }}</a>\r\n    </div>\r\n  </li>\r\n</ng-template>\r\n"
            }] }
];
/** @nocollapse */
ApplicationLayoutComponent.ctorParameters = () => [
    { type: Store },
    { type: OAuthService }
];
ApplicationLayoutComponent.propDecorators = {
    currentUserRef: [{ type: ViewChild, args: ['currentUser', { static: false, read: TemplateRef },] }],
    languageRef: [{ type: ViewChild, args: ['language', { static: false, read: TemplateRef },] }],
    navbarRootDropdowns: [{ type: ViewChildren, args: ['navbarRootDropdown', { read: NgbDropdown },] }]
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
    ApplicationLayoutComponent.prototype.navbarRootDropdowns;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9hcHBsaWNhdGlvbi1sYXlvdXQvYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFHTCxtQkFBbUIsRUFDbkIsV0FBVyxFQUVYLFdBQVcsRUFDWCxZQUFZLEVBQ1osZ0JBQWdCLEdBQ2pCLE1BQU0sY0FBYyxDQUFDO0FBQ3RCLE9BQU8sRUFFTCxTQUFTLEVBRVQsU0FBUyxFQUNULFdBQVcsRUFFWCxTQUFTLEVBQ1QsWUFBWSxHQUNiLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUN6RCxPQUFPLEVBQUUsUUFBUSxFQUFFLFdBQVcsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQzVELE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUNuRCxPQUFPLE9BQU8sTUFBTSxjQUFjLENBQUM7QUFDbkMsT0FBTyxFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDN0MsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDM0QsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBQ3RCLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUVyRCxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBTTNDLE1BQU0sT0FBTywwQkFBMEI7Ozs7O0lBd0RyQyxZQUFvQixLQUFZLEVBQVUsWUFBMEI7UUFBaEQsVUFBSyxHQUFMLEtBQUssQ0FB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MkNBQW9COzs7OztJQUFFLGtEQUFrQzs7Ozs7O0FBa0V0RSxTQUFTLGdCQUFnQixDQUFDLE1BQXVCO0lBQy9DLE9BQU8sTUFBTSxDQUFDLE1BQU07Ozs7O0lBQUMsQ0FBQyxHQUFHLEVBQUUsR0FBRyxFQUFFLEVBQUU7UUFDaEMsSUFBSSxHQUFHLENBQUMsU0FBUztZQUFFLE9BQU8sR0FBRyxDQUFDO1FBRTlCLElBQUksR0FBRyxDQUFDLFFBQVEsSUFBSSxHQUFHLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUN2QyxHQUFHLENBQUMsUUFBUSxHQUFHLGdCQUFnQixDQUFDLEdBQUcsQ0FBQyxRQUFRLENBQUMsQ0FBQztTQUMvQztRQUVELE9BQU8sQ0FBQyxHQUFHLEdBQUcsRUFBRSxHQUFHLENBQUMsQ0FBQztJQUN2QixDQUFDLEdBQUUsRUFBRSxDQUFDLENBQUM7QUFDVCxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcclxuICBBQlAsXHJcbiAgQXBwbGljYXRpb25Db25maWd1cmF0aW9uLFxyXG4gIEdldEFwcENvbmZpZ3VyYXRpb24sXHJcbiAgQ29uZmlnU3RhdGUsXHJcbiAgZUxheW91dFR5cGUsXHJcbiAgU2V0TGFuZ3VhZ2UsXHJcbiAgU2Vzc2lvblN0YXRlLFxyXG4gIHRha2VVbnRpbERlc3Ryb3ksXHJcbn0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHtcclxuICBBZnRlclZpZXdJbml0LFxyXG4gIENvbXBvbmVudCxcclxuICBPbkRlc3Ryb3ksXHJcbiAgUXVlcnlMaXN0LFxyXG4gIFRlbXBsYXRlUmVmLFxyXG4gIFRyYWNrQnlGdW5jdGlvbixcclxuICBWaWV3Q2hpbGQsXHJcbiAgVmlld0NoaWxkcmVuLFxyXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBOZ2JEcm9wZG93biB9IGZyb20gJ0BuZy1ib290c3RyYXAvbmctYm9vdHN0cmFwJztcclxuaW1wb3J0IHsgTmF2aWdhdGUsIFJvdXRlclN0YXRlIH0gZnJvbSAnQG5neHMvcm91dGVyLXBsdWdpbic7XHJcbmltcG9ydCB7IFNlbGVjdCwgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IE9BdXRoU2VydmljZSB9IGZyb20gJ2FuZ3VsYXItb2F1dGgyLW9pZGMnO1xyXG5pbXBvcnQgY29tcGFyZSBmcm9tICdqdXN0LWNvbXBhcmUnO1xyXG5pbXBvcnQgeyBmcm9tRXZlbnQsIE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHsgZGVib3VuY2VUaW1lLCBmaWx0ZXIsIG1hcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcclxuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xyXG5pbXBvcnQgeyBBZGROYXZpZ2F0aW9uRWxlbWVudCB9IGZyb20gJy4uLy4uL2FjdGlvbnMnO1xyXG5pbXBvcnQgeyBMYXlvdXQgfSBmcm9tICcuLi8uLi9tb2RlbHMvbGF5b3V0JztcclxuaW1wb3J0IHsgTGF5b3V0U3RhdGUgfSBmcm9tICcuLi8uLi9zdGF0ZXMnO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdhYnAtbGF5b3V0LWFwcGxpY2F0aW9uJyxcclxuICB0ZW1wbGF0ZVVybDogJy4vYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC5odG1sJyxcclxufSlcclxuZXhwb3J0IGNsYXNzIEFwcGxpY2F0aW9uTGF5b3V0Q29tcG9uZW50IGltcGxlbWVudHMgQWZ0ZXJWaWV3SW5pdCwgT25EZXN0cm95IHtcclxuICAvLyByZXF1aXJlZCBmb3IgZHluYW1pYyBjb21wb25lbnRcclxuICBzdGF0aWMgdHlwZSA9IGVMYXlvdXRUeXBlLmFwcGxpY2F0aW9uO1xyXG5cclxuICBAU2VsZWN0KENvbmZpZ1N0YXRlLmdldE9uZSgncm91dGVzJykpXHJcbiAgcm91dGVzJDogT2JzZXJ2YWJsZTxBQlAuRnVsbFJvdXRlW10+O1xyXG5cclxuICBAU2VsZWN0KENvbmZpZ1N0YXRlLmdldE9uZSgnY3VycmVudFVzZXInKSlcclxuICBjdXJyZW50VXNlciQ6IE9ic2VydmFibGU8QXBwbGljYXRpb25Db25maWd1cmF0aW9uLkN1cnJlbnRVc2VyPjtcclxuXHJcbiAgQFNlbGVjdChDb25maWdTdGF0ZS5nZXREZWVwKCdsb2NhbGl6YXRpb24ubGFuZ3VhZ2VzJykpXHJcbiAgbGFuZ3VhZ2VzJDogT2JzZXJ2YWJsZTxBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24uTGFuZ3VhZ2VbXT47XHJcblxyXG4gIEBTZWxlY3QoTGF5b3V0U3RhdGUuZ2V0TmF2aWdhdGlvbkVsZW1lbnRzKVxyXG4gIG5hdkVsZW1lbnRzJDogT2JzZXJ2YWJsZTxMYXlvdXQuTmF2aWdhdGlvbkVsZW1lbnRbXT47XHJcblxyXG4gIEBWaWV3Q2hpbGQoJ2N1cnJlbnRVc2VyJywgeyBzdGF0aWM6IGZhbHNlLCByZWFkOiBUZW1wbGF0ZVJlZiB9KVxyXG4gIGN1cnJlbnRVc2VyUmVmOiBUZW1wbGF0ZVJlZjxhbnk+O1xyXG5cclxuICBAVmlld0NoaWxkKCdsYW5ndWFnZScsIHsgc3RhdGljOiBmYWxzZSwgcmVhZDogVGVtcGxhdGVSZWYgfSlcclxuICBsYW5ndWFnZVJlZjogVGVtcGxhdGVSZWY8YW55PjtcclxuXHJcbiAgQFZpZXdDaGlsZHJlbignbmF2YmFyUm9vdERyb3Bkb3duJywgeyByZWFkOiBOZ2JEcm9wZG93biB9KVxyXG4gIG5hdmJhclJvb3REcm9wZG93bnM6IFF1ZXJ5TGlzdDxOZ2JEcm9wZG93bj47XHJcblxyXG4gIGlzRHJvcGRvd25DaGlsZER5bmFtaWM6IGJvb2xlYW47XHJcblxyXG4gIGdldCB2aXNpYmxlUm91dGVzJCgpOiBPYnNlcnZhYmxlPEFCUC5GdWxsUm91dGVbXT4ge1xyXG4gICAgcmV0dXJuIHRoaXMucm91dGVzJC5waXBlKG1hcChyb3V0ZXMgPT4gZ2V0VmlzaWJsZVJvdXRlcyhyb3V0ZXMpKSk7XHJcbiAgfVxyXG5cclxuICBnZXQgZGVmYXVsdExhbmd1YWdlJCgpOiBPYnNlcnZhYmxlPHN0cmluZz4ge1xyXG4gICAgcmV0dXJuIHRoaXMubGFuZ3VhZ2VzJC5waXBlKFxyXG4gICAgICBtYXAoXHJcbiAgICAgICAgbGFuZ3VhZ2VzID0+IHNucSgoKSA9PiBsYW5ndWFnZXMuZmluZChsYW5nID0+IGxhbmcuY3VsdHVyZU5hbWUgPT09IHRoaXMuc2VsZWN0ZWRMYW5nQ3VsdHVyZSkuZGlzcGxheU5hbWUpLFxyXG4gICAgICAgICcnLFxyXG4gICAgICApLFxyXG4gICAgKTtcclxuICB9XHJcblxyXG4gIGdldCBkcm9wZG93bkxhbmd1YWdlcyQoKTogT2JzZXJ2YWJsZTxBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24uTGFuZ3VhZ2VbXT4ge1xyXG4gICAgcmV0dXJuIHRoaXMubGFuZ3VhZ2VzJC5waXBlKFxyXG4gICAgICBtYXAobGFuZ3VhZ2VzID0+IHNucSgoKSA9PiBsYW5ndWFnZXMuZmlsdGVyKGxhbmcgPT4gbGFuZy5jdWx0dXJlTmFtZSAhPT0gdGhpcy5zZWxlY3RlZExhbmdDdWx0dXJlKSksIFtdKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBnZXQgc2VsZWN0ZWRMYW5nQ3VsdHVyZSgpOiBzdHJpbmcge1xyXG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoU2Vzc2lvblN0YXRlLmdldExhbmd1YWdlKTtcclxuICB9XHJcblxyXG4gIHJpZ2h0UGFydEVsZW1lbnRzOiBUZW1wbGF0ZVJlZjxhbnk+W10gPSBbXTtcclxuXHJcbiAgdHJhY2tCeUZuOiBUcmFja0J5RnVuY3Rpb248QUJQLkZ1bGxSb3V0ZT4gPSAoXywgaXRlbSkgPT4gaXRlbS5uYW1lO1xyXG5cclxuICB0cmFja0VsZW1lbnRCeUZuOiBUcmFja0J5RnVuY3Rpb248QUJQLkZ1bGxSb3V0ZT4gPSAoXywgZWxlbWVudCkgPT4gZWxlbWVudDtcclxuXHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUsIHByaXZhdGUgb2F1dGhTZXJ2aWNlOiBPQXV0aFNlcnZpY2UpIHt9XHJcblxyXG4gIHByaXZhdGUgY2hlY2tXaW5kb3dXaWR0aCgpIHtcclxuICAgIHNldFRpbWVvdXQoKCkgPT4ge1xyXG4gICAgICB0aGlzLm5hdmJhclJvb3REcm9wZG93bnMuZm9yRWFjaChpdGVtID0+IHtcclxuICAgICAgICBpdGVtLmNsb3NlKCk7XHJcbiAgICAgIH0pO1xyXG4gICAgICBpZiAod2luZG93LmlubmVyV2lkdGggPCA3NjgpIHtcclxuICAgICAgICB0aGlzLmlzRHJvcGRvd25DaGlsZER5bmFtaWMgPSBmYWxzZTtcclxuICAgICAgfSBlbHNlIHtcclxuICAgICAgICB0aGlzLmlzRHJvcGRvd25DaGlsZER5bmFtaWMgPSB0cnVlO1xyXG4gICAgICB9XHJcbiAgICB9LCAwKTtcclxuICB9XHJcblxyXG4gIG5nQWZ0ZXJWaWV3SW5pdCgpIHtcclxuICAgIGNvbnN0IG5hdmlnYXRpb25zID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChMYXlvdXRTdGF0ZS5nZXROYXZpZ2F0aW9uRWxlbWVudHMpLm1hcCgoeyBuYW1lIH0pID0+IG5hbWUpO1xyXG5cclxuICAgIGlmIChuYXZpZ2F0aW9ucy5pbmRleE9mKCdMYW5ndWFnZVJlZicpIDwgMCkge1xyXG4gICAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKFxyXG4gICAgICAgIG5ldyBBZGROYXZpZ2F0aW9uRWxlbWVudChbXHJcbiAgICAgICAgICB7IGVsZW1lbnQ6IHRoaXMubGFuZ3VhZ2VSZWYsIG9yZGVyOiA0LCBuYW1lOiAnTGFuZ3VhZ2VSZWYnIH0sXHJcbiAgICAgICAgICB7IGVsZW1lbnQ6IHRoaXMuY3VycmVudFVzZXJSZWYsIG9yZGVyOiA1LCBuYW1lOiAnQ3VycmVudFVzZXJSZWYnIH0sXHJcbiAgICAgICAgXSksXHJcbiAgICAgICk7XHJcbiAgICB9XHJcblxyXG4gICAgdGhpcy5uYXZFbGVtZW50cyRcclxuICAgICAgLnBpcGUoXHJcbiAgICAgICAgbWFwKGVsZW1lbnRzID0+IGVsZW1lbnRzLm1hcCgoeyBlbGVtZW50IH0pID0+IGVsZW1lbnQpKSxcclxuICAgICAgICBmaWx0ZXIoZWxlbWVudHMgPT4gIWNvbXBhcmUoZWxlbWVudHMsIHRoaXMucmlnaHRQYXJ0RWxlbWVudHMpKSxcclxuICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpLFxyXG4gICAgICApXHJcbiAgICAgIC5zdWJzY3JpYmUoZWxlbWVudHMgPT4ge1xyXG4gICAgICAgIHNldFRpbWVvdXQoKCkgPT4gKHRoaXMucmlnaHRQYXJ0RWxlbWVudHMgPSBlbGVtZW50cyksIDApO1xyXG4gICAgICB9KTtcclxuXHJcbiAgICB0aGlzLmNoZWNrV2luZG93V2lkdGgoKTtcclxuXHJcbiAgICBmcm9tRXZlbnQod2luZG93LCAncmVzaXplJylcclxuICAgICAgLnBpcGUoXHJcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKSxcclxuICAgICAgICBkZWJvdW5jZVRpbWUoMjUwKSxcclxuICAgICAgKVxyXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHtcclxuICAgICAgICB0aGlzLmNoZWNrV2luZG93V2lkdGgoKTtcclxuICAgICAgfSk7XHJcbiAgfVxyXG5cclxuICBuZ09uRGVzdHJveSgpIHt9XHJcblxyXG4gIG9uQ2hhbmdlTGFuZyhjdWx0dXJlTmFtZTogc3RyaW5nKSB7XHJcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBTZXRMYW5ndWFnZShjdWx0dXJlTmFtZSkpO1xyXG4gIH1cclxuXHJcbiAgbG9nb3V0KCkge1xyXG4gICAgdGhpcy5vYXV0aFNlcnZpY2UubG9nT3V0KCk7XHJcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKFxyXG4gICAgICBuZXcgTmF2aWdhdGUoWycvJ10sIG51bGwsIHtcclxuICAgICAgICBzdGF0ZTogeyByZWRpcmVjdFVybDogdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChSb3V0ZXJTdGF0ZSkuc3RhdGUudXJsIH0sXHJcbiAgICAgIH0pLFxyXG4gICAgKTtcclxuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IEdldEFwcENvbmZpZ3VyYXRpb24oKSk7XHJcbiAgfVxyXG59XHJcblxyXG5mdW5jdGlvbiBnZXRWaXNpYmxlUm91dGVzKHJvdXRlczogQUJQLkZ1bGxSb3V0ZVtdKSB7XHJcbiAgcmV0dXJuIHJvdXRlcy5yZWR1Y2UoKGFjYywgdmFsKSA9PiB7XHJcbiAgICBpZiAodmFsLmludmlzaWJsZSkgcmV0dXJuIGFjYztcclxuXHJcbiAgICBpZiAodmFsLmNoaWxkcmVuICYmIHZhbC5jaGlsZHJlbi5sZW5ndGgpIHtcclxuICAgICAgdmFsLmNoaWxkcmVuID0gZ2V0VmlzaWJsZVJvdXRlcyh2YWwuY2hpbGRyZW4pO1xyXG4gICAgfVxyXG5cclxuICAgIHJldHVybiBbLi4uYWNjLCB2YWxdO1xyXG4gIH0sIFtdKTtcclxufVxyXG4iXX0=