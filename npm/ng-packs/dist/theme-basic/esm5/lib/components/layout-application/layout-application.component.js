/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { ConfigGetAppConfiguration, ConfigState, SessionSetLanguage, SessionState, takeUntilDestroy, } from '@abp/ng.core';
import { Component, ViewChild, TemplateRef } from '@angular/core';
import { Navigate, RouterState } from '@ngxs/router-plugin';
import { Select, Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';
import { map, filter } from 'rxjs/operators';
import snq from 'snq';
import { LayoutAddNavigationElement } from '../../actions';
import { LayoutState } from '../../states';
import compare from 'just-compare';
var LayoutApplicationComponent = /** @class */ (function () {
    function LayoutApplicationComponent(store, oauthService) {
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
        function (_, item) { return item.name; });
        this.trackElementByFn = (/**
         * @param {?} _
         * @param {?} element
         * @return {?}
         */
        function (_, element) { return element; });
    }
    Object.defineProperty(LayoutApplicationComponent.prototype, "visibleRoutes$", {
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
    Object.defineProperty(LayoutApplicationComponent.prototype, "defaultLanguage$", {
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
    Object.defineProperty(LayoutApplicationComponent.prototype, "dropdownLanguages$", {
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
    Object.defineProperty(LayoutApplicationComponent.prototype, "selectedLangCulture", {
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
     * @return {?}
     */
    LayoutApplicationComponent.prototype.ngAfterViewInit = /**
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
    };
    /**
     * @return {?}
     */
    LayoutApplicationComponent.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () { };
    /**
     * @param {?} cultureName
     * @return {?}
     */
    LayoutApplicationComponent.prototype.onChangeLang = /**
     * @param {?} cultureName
     * @return {?}
     */
    function (cultureName) {
        this.store.dispatch(new SessionSetLanguage(cultureName));
        this.store.dispatch(new ConfigGetAppConfiguration());
    };
    /**
     * @return {?}
     */
    LayoutApplicationComponent.prototype.logout = /**
     * @return {?}
     */
    function () {
        this.oauthService.logOut();
        this.store.dispatch(new Navigate(['/account/login'], null, {
            state: { redirectUrl: this.store.selectSnapshot(RouterState).state.url },
        }));
        this.store.dispatch(new ConfigGetAppConfiguration());
    };
    // required for dynamic component
    LayoutApplicationComponent.type = "application" /* application */;
    LayoutApplicationComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-layout-application',
                    template: "<abp-layout>\n  <ul class=\"navbar-nav mr-auto\">\n    <ng-container\n      *ngFor=\"let route of visibleRoutes$ | async; trackBy: trackByFn\"\n      [ngTemplateOutlet]=\"route?.children?.length ? dropdownLink : defaultLink\"\n      [ngTemplateOutletContext]=\"{ $implicit: route }\"\n    >\n    </ng-container>\n\n    <ng-template #defaultLink let-route>\n      <li class=\"nav-item\" [abpPermission]=\"route.requiredPolicy\">\n        <a class=\"nav-link\" [routerLink]=\"[route.url]\">{{ route.name | abpLocalization }}</a>\n      </li>\n    </ng-template>\n\n    <ng-template #dropdownLink let-route>\n      <li\n        class=\"nav-item dropdown\"\n        ngbDropdown\n        display=\"static\"\n        [abpPermission]=\"route.requiredPolicy\"\n        [abpVisibility]=\"routeContainer\"\n      >\n        <a ngbDropdownToggle class=\"nav-link dropdown-toggle\" data-toggle=\"dropdown\">\n          {{ route.name | abpLocalization }}\n        </a>\n        <div #routeContainer ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n          <ng-template\n            #forTemplate\n            ngFor\n            [ngForOf]=\"route.children\"\n            [ngForTrackBy]=\"trackByFn\"\n            [ngForTemplate]=\"childWrapper\"\n          ></ng-template>\n        </div>\n      </li>\n    </ng-template>\n\n    <ng-template #childWrapper let-child>\n      <ng-template\n        [ngTemplateOutlet]=\"child?.children?.length ? dropdownChild : defaultChild\"\n        [ngTemplateOutletContext]=\"{ $implicit: child }\"\n      ></ng-template>\n    </ng-template>\n\n    <ng-template #defaultChild let-child>\n      <div class=\"dropdown-submenu\" [abpPermission]=\"child.requiredPolicy\">\n        <a class=\"dropdown-item py-2 px-2\" [routerLink]=\"[child.url]\">{{ child.name | abpLocalization }}</a>\n      </div>\n    </ng-template>\n\n    <ng-template #dropdownChild let-child>\n      <div\n        [abpVisibility]=\"childrenContainer\"\n        class=\"dropdown-submenu\"\n        ngbDropdown\n        display=\"static\"\n        placement=\"right-top\"\n        [abpPermission]=\"child.requiredPolicy\"\n      >\n        <a role=\"button\" class=\"btn d-block text-left py-2 px-2\" ngbDropdownToggle>\n          {{ child.name | abpLocalization }}\n        </a>\n        <div #childrenContainer ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n          <ng-template\n            ngFor\n            [ngForOf]=\"child.children\"\n            [ngForTrackBy]=\"trackByFn\"\n            [ngForTemplate]=\"childWrapper\"\n          ></ng-template>\n        </div>\n      </div>\n    </ng-template>\n  </ul>\n\n  <ul class=\"navbar-nav ml-auto\">\n    <ng-container\n      *ngFor=\"let element of rightPartElements; trackBy: trackElementByFn\"\n      [ngTemplateOutlet]=\"element\"\n    ></ng-container>\n  </ul>\n</abp-layout>\n\n<abp-change-password [(visible)]=\"isOpenChangePassword\"></abp-change-password>\n\n<abp-profile [(visible)]=\"isOpenProfile\"></abp-profile>\n\n<ng-template #language>\n  <li class=\"nav-item dropdown\" ngbDropdown>\n    <a ngbDropdownToggle class=\"nav-link dropdown-toggle\" data-toggle=\"dropdown\">\n      {{ defaultLanguage$ | async }}\n    </a>\n    <div ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n      <a\n        *ngFor=\"let lang of dropdownLanguages$ | async\"\n        class=\"dropdown-item\"\n        (click)=\"onChangeLang(lang.cultureName)\"\n        >{{ lang?.displayName }}</a\n      >\n    </div>\n  </li>\n</ng-template>\n\n<ng-template #currentUser>\n  <li *ngIf=\"(currentUser$ | async)?.isAuthenticated\" class=\"nav-item dropdown\" ngbDropdown>\n    <a ngbDropdownToggle class=\"nav-link dropdown-toggle\" data-toggle=\"dropdown\">\n      {{ (currentUser$ | async)?.userName }}\n    </a>\n    <div ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n      <a class=\"dropdown-item pointer\" (click)=\"isOpenChangePassword = true\">Change Password</a>\n      <a class=\"dropdown-item pointer\" (click)=\"isOpenProfile = true\">My Profile</a>\n      <a class=\"dropdown-item pointer\" (click)=\"logout()\">Logout</a>\n    </div>\n  </li>\n</ng-template>\n"
                }] }
    ];
    /** @nocollapse */
    LayoutApplicationComponent.ctorParameters = function () { return [
        { type: Store },
        { type: OAuthService }
    ]; };
    LayoutApplicationComponent.propDecorators = {
        currentUserRef: [{ type: ViewChild, args: ['currentUser', { static: false, read: TemplateRef },] }],
        languageRef: [{ type: ViewChild, args: ['language', { static: false, read: TemplateRef },] }]
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
    return LayoutApplicationComponent;
}());
export { LayoutApplicationComponent };
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
    LayoutApplicationComponent.prototype.isOpenChangePassword;
    /** @type {?} */
    LayoutApplicationComponent.prototype.isOpenProfile;
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
    function (acc, val) {
        if (val.invisible)
            return acc;
        if (val.children && val.children.length) {
            val.children = getVisibleRoutes(val.children);
        }
        return tslib_1.__spread(acc, [val]);
    }), []);
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF5b3V0LWFwcGxpY2F0aW9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9sYXlvdXQtYXBwbGljYXRpb24vbGF5b3V0LWFwcGxpY2F0aW9uLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFHTCx5QkFBeUIsRUFDekIsV0FBVyxFQUVYLGtCQUFrQixFQUNsQixZQUFZLEVBQ1osZ0JBQWdCLEdBQ2pCLE1BQU0sY0FBYyxDQUFDO0FBQ3RCLE9BQU8sRUFBRSxTQUFTLEVBQW1CLFNBQVMsRUFBRSxXQUFXLEVBQTRCLE1BQU0sZUFBZSxDQUFDO0FBQzdHLE9BQU8sRUFBRSxRQUFRLEVBQUUsV0FBVyxFQUFFLE1BQU0scUJBQXFCLENBQUM7QUFDNUQsT0FBTyxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDNUMsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQ25ELE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDbEMsT0FBTyxFQUFFLEdBQUcsRUFBK0IsTUFBTSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDMUUsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBQ3RCLE9BQU8sRUFBRSwwQkFBMEIsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzRCxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBRTNDLE9BQU8sT0FBTyxNQUFNLGNBQWMsQ0FBQztBQUVuQztJQTJERSxvQ0FBb0IsS0FBWSxFQUFVLFlBQTBCO1FBQWhELFVBQUssR0FBTCxLQUFLLENBQU87UUFBVSxpQkFBWSxHQUFaLFlBQVksQ0FBYztRQWpDcEUseUJBQW9CLEdBQVksS0FBSyxDQUFDO1FBRXRDLGtCQUFhLEdBQVksS0FBSyxDQUFDO1FBeUIvQixzQkFBaUIs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U0FDL0M7UUFFRCx3QkFBVyxHQUFHLEdBQUUsR0FBRyxHQUFFO0lBQ3ZCLENBQUMsR0FBRSxFQUFFLENBQUMsQ0FBQztBQUNULENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQge1xuICBBQlAsXG4gIEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbixcbiAgQ29uZmlnR2V0QXBwQ29uZmlndXJhdGlvbixcbiAgQ29uZmlnU3RhdGUsXG4gIGVMYXlvdXRUeXBlLFxuICBTZXNzaW9uU2V0TGFuZ3VhZ2UsXG4gIFNlc3Npb25TdGF0ZSxcbiAgdGFrZVVudGlsRGVzdHJveSxcbn0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IENvbXBvbmVudCwgVHJhY2tCeUZ1bmN0aW9uLCBWaWV3Q2hpbGQsIFRlbXBsYXRlUmVmLCBBZnRlclZpZXdJbml0LCBPbkRlc3Ryb3kgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE5hdmlnYXRlLCBSb3V0ZXJTdGF0ZSB9IGZyb20gJ0BuZ3hzL3JvdXRlci1wbHVnaW4nO1xuaW1wb3J0IHsgU2VsZWN0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9BdXRoU2VydmljZSB9IGZyb20gJ2FuZ3VsYXItb2F1dGgyLW9pZGMnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgbWFwLCBkaXN0aW5jdFVudGlsQ2hhbmdlZCwgZGVsYXksIGZpbHRlciB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCBzbnEgZnJvbSAnc25xJztcbmltcG9ydCB7IExheW91dEFkZE5hdmlnYXRpb25FbGVtZW50IH0gZnJvbSAnLi4vLi4vYWN0aW9ucyc7XG5pbXBvcnQgeyBMYXlvdXRTdGF0ZSB9IGZyb20gJy4uLy4uL3N0YXRlcyc7XG5pbXBvcnQgeyBMYXlvdXQgfSBmcm9tICcuLi8uLi9tb2RlbHMvbGF5b3V0JztcbmltcG9ydCBjb21wYXJlIGZyb20gJ2p1c3QtY29tcGFyZSc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1sYXlvdXQtYXBwbGljYXRpb24nLFxuICB0ZW1wbGF0ZVVybDogJy4vbGF5b3V0LWFwcGxpY2F0aW9uLmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgTGF5b3V0QXBwbGljYXRpb25Db21wb25lbnQgaW1wbGVtZW50cyBBZnRlclZpZXdJbml0LCBPbkRlc3Ryb3kge1xuICAvLyByZXF1aXJlZCBmb3IgZHluYW1pYyBjb21wb25lbnRcbiAgc3RhdGljIHR5cGUgPSBlTGF5b3V0VHlwZS5hcHBsaWNhdGlvbjtcblxuICBAU2VsZWN0KENvbmZpZ1N0YXRlLmdldE9uZSgncm91dGVzJykpXG4gIHJvdXRlcyQ6IE9ic2VydmFibGU8QUJQLkZ1bGxSb3V0ZVtdPjtcblxuICBAU2VsZWN0KENvbmZpZ1N0YXRlLmdldE9uZSgnY3VycmVudFVzZXInKSlcbiAgY3VycmVudFVzZXIkOiBPYnNlcnZhYmxlPEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbi5DdXJyZW50VXNlcj47XG5cbiAgQFNlbGVjdChDb25maWdTdGF0ZS5nZXREZWVwKCdsb2NhbGl6YXRpb24ubGFuZ3VhZ2VzJykpXG4gIGxhbmd1YWdlcyQ6IE9ic2VydmFibGU8QXBwbGljYXRpb25Db25maWd1cmF0aW9uLkxhbmd1YWdlW10+O1xuXG4gIEBTZWxlY3QoTGF5b3V0U3RhdGUuZ2V0TmF2aWdhdGlvbkVsZW1lbnRzKVxuICBuYXZFbGVtZW50cyQ6IE9ic2VydmFibGU8TGF5b3V0Lk5hdmlnYXRpb25FbGVtZW50W10+O1xuXG4gIEBWaWV3Q2hpbGQoJ2N1cnJlbnRVc2VyJywgeyBzdGF0aWM6IGZhbHNlLCByZWFkOiBUZW1wbGF0ZVJlZiB9KVxuICBjdXJyZW50VXNlclJlZjogVGVtcGxhdGVSZWY8YW55PjtcblxuICBAVmlld0NoaWxkKCdsYW5ndWFnZScsIHsgc3RhdGljOiBmYWxzZSwgcmVhZDogVGVtcGxhdGVSZWYgfSlcbiAgbGFuZ3VhZ2VSZWY6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgaXNPcGVuQ2hhbmdlUGFzc3dvcmQ6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBpc09wZW5Qcm9maWxlOiBib29sZWFuID0gZmFsc2U7XG5cbiAgZ2V0IHZpc2libGVSb3V0ZXMkKCk6IE9ic2VydmFibGU8QUJQLkZ1bGxSb3V0ZVtdPiB7XG4gICAgcmV0dXJuIHRoaXMucm91dGVzJC5waXBlKG1hcChyb3V0ZXMgPT4gZ2V0VmlzaWJsZVJvdXRlcyhyb3V0ZXMpKSk7XG4gIH1cblxuICBnZXQgZGVmYXVsdExhbmd1YWdlJCgpOiBPYnNlcnZhYmxlPHN0cmluZz4ge1xuICAgIHJldHVybiB0aGlzLmxhbmd1YWdlcyQucGlwZShcbiAgICAgIG1hcChcbiAgICAgICAgbGFuZ3VhZ2VzID0+IHNucSgoKSA9PiBsYW5ndWFnZXMuZmluZChsYW5nID0+IGxhbmcuY3VsdHVyZU5hbWUgPT09IHRoaXMuc2VsZWN0ZWRMYW5nQ3VsdHVyZSkuZGlzcGxheU5hbWUpLFxuICAgICAgICAnJyxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIGdldCBkcm9wZG93bkxhbmd1YWdlcyQoKTogT2JzZXJ2YWJsZTxBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24uTGFuZ3VhZ2VbXT4ge1xuICAgIHJldHVybiB0aGlzLmxhbmd1YWdlcyQucGlwZShcbiAgICAgIG1hcChsYW5ndWFnZXMgPT4gc25xKCgpID0+IGxhbmd1YWdlcy5maWx0ZXIobGFuZyA9PiBsYW5nLmN1bHR1cmVOYW1lICE9PSB0aGlzLnNlbGVjdGVkTGFuZ0N1bHR1cmUpKSwgW10pLFxuICAgICk7XG4gIH1cblxuICBnZXQgc2VsZWN0ZWRMYW5nQ3VsdHVyZSgpOiBzdHJpbmcge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFNlc3Npb25TdGF0ZS5nZXRMYW5ndWFnZSk7XG4gIH1cblxuICByaWdodFBhcnRFbGVtZW50czogVGVtcGxhdGVSZWY8YW55PltdID0gW107XG5cbiAgdHJhY2tCeUZuOiBUcmFja0J5RnVuY3Rpb248QUJQLkZ1bGxSb3V0ZT4gPSAoXywgaXRlbSkgPT4gaXRlbS5uYW1lO1xuXG4gIHRyYWNrRWxlbWVudEJ5Rm46IFRyYWNrQnlGdW5jdGlvbjxBQlAuRnVsbFJvdXRlPiA9IChfLCBlbGVtZW50KSA9PiBlbGVtZW50O1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlLCBwcml2YXRlIG9hdXRoU2VydmljZTogT0F1dGhTZXJ2aWNlKSB7fVxuXG4gIG5nQWZ0ZXJWaWV3SW5pdCgpIHtcbiAgICBjb25zdCBuYXZpZ2F0aW9ucyA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoTGF5b3V0U3RhdGUuZ2V0TmF2aWdhdGlvbkVsZW1lbnRzKS5tYXAoKHsgbmFtZSB9KSA9PiBuYW1lKTtcblxuICAgIGlmIChuYXZpZ2F0aW9ucy5pbmRleE9mKCdMYW5ndWFnZVJlZicpIDwgMCkge1xuICAgICAgdGhpcy5zdG9yZS5kaXNwYXRjaChcbiAgICAgICAgbmV3IExheW91dEFkZE5hdmlnYXRpb25FbGVtZW50KFtcbiAgICAgICAgICB7IGVsZW1lbnQ6IHRoaXMubGFuZ3VhZ2VSZWYsIG9yZGVyOiA0LCBuYW1lOiAnTGFuZ3VhZ2VSZWYnIH0sXG4gICAgICAgICAgeyBlbGVtZW50OiB0aGlzLmN1cnJlbnRVc2VyUmVmLCBvcmRlcjogNSwgbmFtZTogJ0N1cnJlbnRVc2VyUmVmJyB9LFxuICAgICAgICBdKSxcbiAgICAgICk7XG4gICAgfVxuXG4gICAgdGhpcy5uYXZFbGVtZW50cyRcbiAgICAgIC5waXBlKFxuICAgICAgICBtYXAoZWxlbWVudHMgPT4gZWxlbWVudHMubWFwKCh7IGVsZW1lbnQgfSkgPT4gZWxlbWVudCkpLFxuICAgICAgICBmaWx0ZXIoZWxlbWVudHMgPT4gIWNvbXBhcmUoZWxlbWVudHMsIHRoaXMucmlnaHRQYXJ0RWxlbWVudHMpKSxcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoZWxlbWVudHMgPT4ge1xuICAgICAgICBzZXRUaW1lb3V0KCgpID0+ICh0aGlzLnJpZ2h0UGFydEVsZW1lbnRzID0gZWxlbWVudHMpLCAwKTtcbiAgICAgIH0pO1xuICB9XG5cbiAgbmdPbkRlc3Ryb3koKSB7fVxuXG4gIG9uQ2hhbmdlTGFuZyhjdWx0dXJlTmFtZTogc3RyaW5nKSB7XG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgU2Vzc2lvblNldExhbmd1YWdlKGN1bHR1cmVOYW1lKSk7XG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgQ29uZmlnR2V0QXBwQ29uZmlndXJhdGlvbigpKTtcbiAgfVxuXG4gIGxvZ291dCgpIHtcbiAgICB0aGlzLm9hdXRoU2VydmljZS5sb2dPdXQoKTtcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKFxuICAgICAgbmV3IE5hdmlnYXRlKFsnL2FjY291bnQvbG9naW4nXSwgbnVsbCwge1xuICAgICAgICBzdGF0ZTogeyByZWRpcmVjdFVybDogdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChSb3V0ZXJTdGF0ZSkuc3RhdGUudXJsIH0sXG4gICAgICB9KSxcbiAgICApO1xuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IENvbmZpZ0dldEFwcENvbmZpZ3VyYXRpb24oKSk7XG4gIH1cbn1cblxuZnVuY3Rpb24gZ2V0VmlzaWJsZVJvdXRlcyhyb3V0ZXM6IEFCUC5GdWxsUm91dGVbXSkge1xuICByZXR1cm4gcm91dGVzLnJlZHVjZSgoYWNjLCB2YWwpID0+IHtcbiAgICBpZiAodmFsLmludmlzaWJsZSkgcmV0dXJuIGFjYztcblxuICAgIGlmICh2YWwuY2hpbGRyZW4gJiYgdmFsLmNoaWxkcmVuLmxlbmd0aCkge1xuICAgICAgdmFsLmNoaWxkcmVuID0gZ2V0VmlzaWJsZVJvdXRlcyh2YWwuY2hpbGRyZW4pO1xuICAgIH1cblxuICAgIHJldHVybiBbLi4uYWNjLCB2YWxdO1xuICB9LCBbXSk7XG59XG4iXX0=