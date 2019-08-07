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
     * @private
     * @return {?}
     */
    LayoutApplicationComponent.prototype.checkWindowWidth = /**
     * @private
     * @return {?}
     */
    function () {
        var _this = this;
        setTimeout((/**
         * @return {?}
         */
        function () {
            _this.navbarRootDropdowns.forEach((/**
             * @param {?} item
             * @return {?}
             */
            function (item) {
                item.close();
            }));
            if (window.innerWidth < 768) {
                _this.isDropdownChildDynamic = false;
            }
            else {
                _this.isDropdownChildDynamic = true;
            }
        }), 0);
    };
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
        this.checkWindowWidth();
        fromEvent(window, 'resize')
            .pipe(takeUntilDestroy(this), debounceTime(250))
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
                    template: "<abp-layout>\n  <ul class=\"navbar-nav mr-auto\">\n    <ng-container\n      *ngFor=\"let route of visibleRoutes$ | async; trackBy: trackByFn\"\n      [ngTemplateOutlet]=\"route?.children?.length ? dropdownLink : defaultLink\"\n      [ngTemplateOutletContext]=\"{ $implicit: route }\"\n    >\n    </ng-container>\n\n    <ng-template #defaultLink let-route>\n      <li class=\"nav-item\" [abpPermission]=\"route.requiredPolicy\">\n        <a class=\"nav-link\" [routerLink]=\"[route.url]\">{{ route.name | abpLocalization }}</a>\n      </li>\n    </ng-template>\n\n    <ng-template #dropdownLink let-route>\n      <li\n        #navbarRootDropdown\n        class=\"nav-item dropdown\"\n        ngbDropdown\n        display=\"static\"\n        [abpPermission]=\"route.requiredPolicy\"\n        [abpVisibility]=\"routeContainer\"\n      >\n        <a ngbDropdownToggle class=\"nav-link dropdown-toggle\" data-toggle=\"dropdown\">\n          {{ route.name | abpLocalization }}\n        </a>\n        <div #routeContainer ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n          <ng-template\n            #forTemplate\n            ngFor\n            [ngForOf]=\"route.children\"\n            [ngForTrackBy]=\"trackByFn\"\n            [ngForTemplate]=\"childWrapper\"\n          ></ng-template>\n        </div>\n      </li>\n    </ng-template>\n\n    <ng-template #childWrapper let-child>\n      <ng-template\n        [ngTemplateOutlet]=\"child?.children?.length ? dropdownChild : defaultChild\"\n        [ngTemplateOutletContext]=\"{ $implicit: child }\"\n      ></ng-template>\n    </ng-template>\n\n    <ng-template #defaultChild let-child>\n      <div class=\"dropdown-submenu\" [abpPermission]=\"child.requiredPolicy\">\n        <a class=\"dropdown-item py-2 px-2\" [routerLink]=\"[child.url]\">{{ child.name | abpLocalization }}</a>\n      </div>\n    </ng-template>\n\n    <ng-template #dropdownChild let-child>\n      <div\n        [abpVisibility]=\"childrenContainer\"\n        class=\"dropdown-submenu\"\n        ngbDropdown\n        [display]=\"isDropdownChildDynamic ? 'dynamic' : 'static'\"\n        placement=\"right-top\"\n        [abpPermission]=\"child.requiredPolicy\"\n      >\n        <div ngbDropdownToggle [class.dropdown-toggle]=\"false\">\n          <a\n            abpEllipsis=\"140px\"\n            [abpEllipsisEnabled]=\"isDropdownChildDynamic\"\n            role=\"button\"\n            class=\"btn d-block text-left py-2 px-2 dropdown-toggle\"\n          >\n            {{ child.name | abpLocalization }}\n          </a>\n        </div>\n        <div #childrenContainer ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n          <ng-template\n            ngFor\n            [ngForOf]=\"child.children\"\n            [ngForTrackBy]=\"trackByFn\"\n            [ngForTemplate]=\"childWrapper\"\n          ></ng-template>\n        </div>\n      </div>\n    </ng-template>\n  </ul>\n\n  <ul class=\"navbar-nav ml-auto\">\n    <ng-container\n      *ngFor=\"let element of rightPartElements; trackBy: trackElementByFn\"\n      [ngTemplateOutlet]=\"element\"\n    ></ng-container>\n  </ul>\n</abp-layout>\n\n<ng-template #language>\n  <li class=\"nav-item dropdown\" ngbDropdown>\n    <a ngbDropdownToggle class=\"nav-link dropdown-toggle\" data-toggle=\"dropdown\">\n      {{ defaultLanguage$ | async }}\n    </a>\n    <div ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n      <a\n        *ngFor=\"let lang of dropdownLanguages$ | async\"\n        class=\"dropdown-item\"\n        (click)=\"onChangeLang(lang.cultureName)\"\n        >{{ lang?.displayName }}</a\n      >\n    </div>\n  </li>\n</ng-template>\n\n<ng-template #currentUser>\n  <li *ngIf=\"(currentUser$ | async)?.isAuthenticated\" class=\"nav-item dropdown\" ngbDropdown>\n    <a ngbDropdownToggle class=\"nav-link dropdown-toggle\" data-toggle=\"dropdown\">\n      {{ (currentUser$ | async)?.userName }}\n    </a>\n    <div ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n      <a class=\"dropdown-item pointer\" (click)=\"isOpenChangePassword = true\">Change Password</a>\n      <a class=\"dropdown-item pointer\" (click)=\"isOpenProfile = true\">My Profile</a>\n      <a class=\"dropdown-item pointer\" (click)=\"logout()\">Logout</a>\n    </div>\n  </li>\n\n  <abp-change-password [(visible)]=\"isOpenChangePassword\"></abp-change-password>\n\n  <abp-profile [(visible)]=\"isOpenProfile\"></abp-profile>\n</ng-template>\n"
                }] }
    ];
    /** @nocollapse */
    LayoutApplicationComponent.ctorParameters = function () { return [
        { type: Store },
        { type: OAuthService }
    ]; };
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
    function (acc, val) {
        if (val.invisible)
            return acc;
        if (val.children && val.children.length) {
            val.children = getVisibleRoutes(val.children);
        }
        return tslib_1.__spread(acc, [val]);
    }), []);
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF5b3V0LWFwcGxpY2F0aW9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9sYXlvdXQtYXBwbGljYXRpb24vbGF5b3V0LWFwcGxpY2F0aW9uLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFHTCx5QkFBeUIsRUFDekIsV0FBVyxFQUVYLGtCQUFrQixFQUNsQixZQUFZLEVBQ1osZ0JBQWdCLEdBQ2pCLE1BQU0sY0FBYyxDQUFDO0FBQ3RCLE9BQU8sRUFFTCxTQUFTLEVBRVQsU0FBUyxFQUNULFdBQVcsRUFFWCxTQUFTLEVBQ1QsWUFBWSxHQUNiLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUN6RCxPQUFPLEVBQUUsUUFBUSxFQUFFLFdBQVcsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQzVELE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUNuRCxPQUFPLE9BQU8sTUFBTSxjQUFjLENBQUM7QUFDbkMsT0FBTyxFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDN0MsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDM0QsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBQ3RCLE9BQU8sRUFBRSwwQkFBMEIsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUzRCxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBRTNDO0lBZ0VFLG9DQUFvQixLQUFZLEVBQVUsWUFBMEI7UUFBaEQsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUFVLGlCQUFZLEdBQVos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QUFDLENBQUM7MENBQzVCLFVBQVU7b0VBQXVDO0lBRy9EO1FBREMsTUFBTSxDQUFDLFdBQVcsQ0FBQyxPQUFPLENBQUMsd0JBQXdCLENBQUMsQ0FBQzswQ0FDMUMsVUFBVTtrRUFBc0M7SUFHNUQ7UUFEQyxNQUFNLENBQUMsV0FBVyxDQUFDLHFCQUFxQixDQUFDOzBDQUM1QixVQUFVO29FQUE2QjtJQStHdkQsaUNBQUM7Q0FBQSxBQWpJRCxJQWlJQztTQTdIWSwwQkFBMEI7OztJQUVyQyxnQ0FBc0M7O0lBRXRDLDZDQUNxQzs7SUFFckMsa0RBQytEOztJQUUvRCxnREFDNEQ7O0lBRTVELGtEQUNxRDs7SUFFckQsb0RBQ2lDOztJQUVqQyxpREFDOEI7O0lBRTlCLHlEQUM0Qzs7SUFFNUMsMERBQXNDOztJQUV0QyxtREFBK0I7O0lBRS9CLDREQUFnQzs7SUF5QmhDLHVEQUEyQzs7SUFFM0MsK0NBQW1FOztJQUVuRSxzREFBMkU7Ozs7O0lBRS9ELDJDQUFvQjs7Ozs7SUFBRSxrREFBa0M7Ozs7OztBQW1FdEUsU0FBUyxnQkFBZ0IsQ0FBQyxNQUF1QjtJQUMvQyxPQUFPLE1BQU0sQ0FBQyxNQUFNOzs7OztJQUFDLFVBQUMsR0FBRyxFQUFFLEdBQUc7UUFDNUIsSUFBSSxHQUFHLENBQUMsU0FBUztZQUFFLE9BQU8sR0FBRyxDQUFDO1FBRTlCLElBQUksR0FBRyxDQUFDLFFBQVEsSUFBSSxHQUFHLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUN2QyxHQUFHLENBQUMsUUFBUSxHQUFHLGdCQUFnQixDQUFDLEdBQUcsQ0FBQyxRQUFRLENBQUMsQ0FBQztTQUMvQztRQUVELHdCQUFXLEdBQUcsR0FBRSxHQUFHLEdBQUU7SUFDdkIsQ0FBQyxHQUFFLEVBQUUsQ0FBQyxDQUFDO0FBQ1QsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7XG4gIEFCUCxcbiAgQXBwbGljYXRpb25Db25maWd1cmF0aW9uLFxuICBDb25maWdHZXRBcHBDb25maWd1cmF0aW9uLFxuICBDb25maWdTdGF0ZSxcbiAgZUxheW91dFR5cGUsXG4gIFNlc3Npb25TZXRMYW5ndWFnZSxcbiAgU2Vzc2lvblN0YXRlLFxuICB0YWtlVW50aWxEZXN0cm95LFxufSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHtcbiAgQWZ0ZXJWaWV3SW5pdCxcbiAgQ29tcG9uZW50LFxuICBPbkRlc3Ryb3ksXG4gIFF1ZXJ5TGlzdCxcbiAgVGVtcGxhdGVSZWYsXG4gIFRyYWNrQnlGdW5jdGlvbixcbiAgVmlld0NoaWxkLFxuICBWaWV3Q2hpbGRyZW4sXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgTmdiRHJvcGRvd24gfSBmcm9tICdAbmctYm9vdHN0cmFwL25nLWJvb3RzdHJhcCc7XG5pbXBvcnQgeyBOYXZpZ2F0ZSwgUm91dGVyU3RhdGUgfSBmcm9tICdAbmd4cy9yb3V0ZXItcGx1Z2luJztcbmltcG9ydCB7IFNlbGVjdCwgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBPQXV0aFNlcnZpY2UgfSBmcm9tICdhbmd1bGFyLW9hdXRoMi1vaWRjJztcbmltcG9ydCBjb21wYXJlIGZyb20gJ2p1c3QtY29tcGFyZSc7XG5pbXBvcnQgeyBmcm9tRXZlbnQsIE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGRlYm91bmNlVGltZSwgZmlsdGVyLCBtYXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5pbXBvcnQgeyBMYXlvdXRBZGROYXZpZ2F0aW9uRWxlbWVudCB9IGZyb20gJy4uLy4uL2FjdGlvbnMnO1xuaW1wb3J0IHsgTGF5b3V0IH0gZnJvbSAnLi4vLi4vbW9kZWxzL2xheW91dCc7XG5pbXBvcnQgeyBMYXlvdXRTdGF0ZSB9IGZyb20gJy4uLy4uL3N0YXRlcyc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1sYXlvdXQtYXBwbGljYXRpb24nLFxuICB0ZW1wbGF0ZVVybDogJy4vbGF5b3V0LWFwcGxpY2F0aW9uLmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgTGF5b3V0QXBwbGljYXRpb25Db21wb25lbnQgaW1wbGVtZW50cyBBZnRlclZpZXdJbml0LCBPbkRlc3Ryb3kge1xuICAvLyByZXF1aXJlZCBmb3IgZHluYW1pYyBjb21wb25lbnRcbiAgc3RhdGljIHR5cGUgPSBlTGF5b3V0VHlwZS5hcHBsaWNhdGlvbjtcblxuICBAU2VsZWN0KENvbmZpZ1N0YXRlLmdldE9uZSgncm91dGVzJykpXG4gIHJvdXRlcyQ6IE9ic2VydmFibGU8QUJQLkZ1bGxSb3V0ZVtdPjtcblxuICBAU2VsZWN0KENvbmZpZ1N0YXRlLmdldE9uZSgnY3VycmVudFVzZXInKSlcbiAgY3VycmVudFVzZXIkOiBPYnNlcnZhYmxlPEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbi5DdXJyZW50VXNlcj47XG5cbiAgQFNlbGVjdChDb25maWdTdGF0ZS5nZXREZWVwKCdsb2NhbGl6YXRpb24ubGFuZ3VhZ2VzJykpXG4gIGxhbmd1YWdlcyQ6IE9ic2VydmFibGU8QXBwbGljYXRpb25Db25maWd1cmF0aW9uLkxhbmd1YWdlW10+O1xuXG4gIEBTZWxlY3QoTGF5b3V0U3RhdGUuZ2V0TmF2aWdhdGlvbkVsZW1lbnRzKVxuICBuYXZFbGVtZW50cyQ6IE9ic2VydmFibGU8TGF5b3V0Lk5hdmlnYXRpb25FbGVtZW50W10+O1xuXG4gIEBWaWV3Q2hpbGQoJ2N1cnJlbnRVc2VyJywgeyBzdGF0aWM6IGZhbHNlLCByZWFkOiBUZW1wbGF0ZVJlZiB9KVxuICBjdXJyZW50VXNlclJlZjogVGVtcGxhdGVSZWY8YW55PjtcblxuICBAVmlld0NoaWxkKCdsYW5ndWFnZScsIHsgc3RhdGljOiBmYWxzZSwgcmVhZDogVGVtcGxhdGVSZWYgfSlcbiAgbGFuZ3VhZ2VSZWY6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgQFZpZXdDaGlsZHJlbignbmF2YmFyUm9vdERyb3Bkb3duJywgeyByZWFkOiBOZ2JEcm9wZG93biB9KVxuICBuYXZiYXJSb290RHJvcGRvd25zOiBRdWVyeUxpc3Q8TmdiRHJvcGRvd24+O1xuXG4gIGlzT3BlbkNoYW5nZVBhc3N3b3JkOiBib29sZWFuID0gZmFsc2U7XG5cbiAgaXNPcGVuUHJvZmlsZTogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIGlzRHJvcGRvd25DaGlsZER5bmFtaWM6IGJvb2xlYW47XG5cbiAgZ2V0IHZpc2libGVSb3V0ZXMkKCk6IE9ic2VydmFibGU8QUJQLkZ1bGxSb3V0ZVtdPiB7XG4gICAgcmV0dXJuIHRoaXMucm91dGVzJC5waXBlKG1hcChyb3V0ZXMgPT4gZ2V0VmlzaWJsZVJvdXRlcyhyb3V0ZXMpKSk7XG4gIH1cblxuICBnZXQgZGVmYXVsdExhbmd1YWdlJCgpOiBPYnNlcnZhYmxlPHN0cmluZz4ge1xuICAgIHJldHVybiB0aGlzLmxhbmd1YWdlcyQucGlwZShcbiAgICAgIG1hcChcbiAgICAgICAgbGFuZ3VhZ2VzID0+IHNucSgoKSA9PiBsYW5ndWFnZXMuZmluZChsYW5nID0+IGxhbmcuY3VsdHVyZU5hbWUgPT09IHRoaXMuc2VsZWN0ZWRMYW5nQ3VsdHVyZSkuZGlzcGxheU5hbWUpLFxuICAgICAgICAnJyxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIGdldCBkcm9wZG93bkxhbmd1YWdlcyQoKTogT2JzZXJ2YWJsZTxBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24uTGFuZ3VhZ2VbXT4ge1xuICAgIHJldHVybiB0aGlzLmxhbmd1YWdlcyQucGlwZShcbiAgICAgIG1hcChsYW5ndWFnZXMgPT4gc25xKCgpID0+IGxhbmd1YWdlcy5maWx0ZXIobGFuZyA9PiBsYW5nLmN1bHR1cmVOYW1lICE9PSB0aGlzLnNlbGVjdGVkTGFuZ0N1bHR1cmUpKSwgW10pLFxuICAgICk7XG4gIH1cblxuICBnZXQgc2VsZWN0ZWRMYW5nQ3VsdHVyZSgpOiBzdHJpbmcge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFNlc3Npb25TdGF0ZS5nZXRMYW5ndWFnZSk7XG4gIH1cblxuICByaWdodFBhcnRFbGVtZW50czogVGVtcGxhdGVSZWY8YW55PltdID0gW107XG5cbiAgdHJhY2tCeUZuOiBUcmFja0J5RnVuY3Rpb248QUJQLkZ1bGxSb3V0ZT4gPSAoXywgaXRlbSkgPT4gaXRlbS5uYW1lO1xuXG4gIHRyYWNrRWxlbWVudEJ5Rm46IFRyYWNrQnlGdW5jdGlvbjxBQlAuRnVsbFJvdXRlPiA9IChfLCBlbGVtZW50KSA9PiBlbGVtZW50O1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlLCBwcml2YXRlIG9hdXRoU2VydmljZTogT0F1dGhTZXJ2aWNlKSB7fVxuXG4gIHByaXZhdGUgY2hlY2tXaW5kb3dXaWR0aCgpIHtcbiAgICBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgIHRoaXMubmF2YmFyUm9vdERyb3Bkb3ducy5mb3JFYWNoKGl0ZW0gPT4ge1xuICAgICAgICBpdGVtLmNsb3NlKCk7XG4gICAgICB9KTtcbiAgICAgIGlmICh3aW5kb3cuaW5uZXJXaWR0aCA8IDc2OCkge1xuICAgICAgICB0aGlzLmlzRHJvcGRvd25DaGlsZER5bmFtaWMgPSBmYWxzZTtcbiAgICAgIH0gZWxzZSB7XG4gICAgICAgIHRoaXMuaXNEcm9wZG93bkNoaWxkRHluYW1pYyA9IHRydWU7XG4gICAgICB9XG4gICAgfSwgMCk7XG4gIH1cblxuICBuZ0FmdGVyVmlld0luaXQoKSB7XG4gICAgY29uc3QgbmF2aWdhdGlvbnMgPSB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KExheW91dFN0YXRlLmdldE5hdmlnYXRpb25FbGVtZW50cykubWFwKCh7IG5hbWUgfSkgPT4gbmFtZSk7XG5cbiAgICBpZiAobmF2aWdhdGlvbnMuaW5kZXhPZignTGFuZ3VhZ2VSZWYnKSA8IDApIHtcbiAgICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2goXG4gICAgICAgIG5ldyBMYXlvdXRBZGROYXZpZ2F0aW9uRWxlbWVudChbXG4gICAgICAgICAgeyBlbGVtZW50OiB0aGlzLmxhbmd1YWdlUmVmLCBvcmRlcjogNCwgbmFtZTogJ0xhbmd1YWdlUmVmJyB9LFxuICAgICAgICAgIHsgZWxlbWVudDogdGhpcy5jdXJyZW50VXNlclJlZiwgb3JkZXI6IDUsIG5hbWU6ICdDdXJyZW50VXNlclJlZicgfSxcbiAgICAgICAgXSksXG4gICAgICApO1xuICAgIH1cblxuICAgIHRoaXMubmF2RWxlbWVudHMkXG4gICAgICAucGlwZShcbiAgICAgICAgbWFwKGVsZW1lbnRzID0+IGVsZW1lbnRzLm1hcCgoeyBlbGVtZW50IH0pID0+IGVsZW1lbnQpKSxcbiAgICAgICAgZmlsdGVyKGVsZW1lbnRzID0+ICFjb21wYXJlKGVsZW1lbnRzLCB0aGlzLnJpZ2h0UGFydEVsZW1lbnRzKSksXG4gICAgICAgIHRha2VVbnRpbERlc3Ryb3kodGhpcyksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKGVsZW1lbnRzID0+IHtcbiAgICAgICAgc2V0VGltZW91dCgoKSA9PiAodGhpcy5yaWdodFBhcnRFbGVtZW50cyA9IGVsZW1lbnRzKSwgMCk7XG4gICAgICB9KTtcblxuICAgIHRoaXMuY2hlY2tXaW5kb3dXaWR0aCgpO1xuXG4gICAgZnJvbUV2ZW50KHdpbmRvdywgJ3Jlc2l6ZScpXG4gICAgICAucGlwZShcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKSxcbiAgICAgICAgZGVib3VuY2VUaW1lKDI1MCksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgdGhpcy5jaGVja1dpbmRvd1dpZHRoKCk7XG4gICAgICB9KTtcbiAgfVxuXG4gIG5nT25EZXN0cm95KCkge31cblxuICBvbkNoYW5nZUxhbmcoY3VsdHVyZU5hbWU6IHN0cmluZykge1xuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IFNlc3Npb25TZXRMYW5ndWFnZShjdWx0dXJlTmFtZSkpO1xuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IENvbmZpZ0dldEFwcENvbmZpZ3VyYXRpb24oKSk7XG4gIH1cblxuICBsb2dvdXQoKSB7XG4gICAgdGhpcy5vYXV0aFNlcnZpY2UubG9nT3V0KCk7XG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChcbiAgICAgIG5ldyBOYXZpZ2F0ZShbJy9hY2NvdW50L2xvZ2luJ10sIG51bGwsIHtcbiAgICAgICAgc3RhdGU6IHsgcmVkaXJlY3RVcmw6IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoUm91dGVyU3RhdGUpLnN0YXRlLnVybCB9LFxuICAgICAgfSksXG4gICAgKTtcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBDb25maWdHZXRBcHBDb25maWd1cmF0aW9uKCkpO1xuICB9XG59XG5cbmZ1bmN0aW9uIGdldFZpc2libGVSb3V0ZXMocm91dGVzOiBBQlAuRnVsbFJvdXRlW10pIHtcbiAgcmV0dXJuIHJvdXRlcy5yZWR1Y2UoKGFjYywgdmFsKSA9PiB7XG4gICAgaWYgKHZhbC5pbnZpc2libGUpIHJldHVybiBhY2M7XG5cbiAgICBpZiAodmFsLmNoaWxkcmVuICYmIHZhbC5jaGlsZHJlbi5sZW5ndGgpIHtcbiAgICAgIHZhbC5jaGlsZHJlbiA9IGdldFZpc2libGVSb3V0ZXModmFsLmNoaWxkcmVuKTtcbiAgICB9XG5cbiAgICByZXR1cm4gWy4uLmFjYywgdmFsXTtcbiAgfSwgW10pO1xufVxuIl19