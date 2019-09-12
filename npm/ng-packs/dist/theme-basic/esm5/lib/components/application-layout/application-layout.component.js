/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
var ApplicationLayoutComponent = /** @class */ (function () {
    function ApplicationLayoutComponent(store, oauthService) {
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
    // required for dynamic component
    ApplicationLayoutComponent.type = "application" /* application */;
    ApplicationLayoutComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-layout-application',
                    template: "<abp-layout>\n  <ul class=\"navbar-nav mr-auto\">\n    <ng-container\n      *ngFor=\"let route of visibleRoutes$ | async; trackBy: trackByFn\"\n      [ngTemplateOutlet]=\"route?.children?.length ? dropdownLink : defaultLink\"\n      [ngTemplateOutletContext]=\"{ $implicit: route }\"\n    >\n    </ng-container>\n\n    <ng-template #defaultLink let-route>\n      <li class=\"nav-item\" [abpPermission]=\"route.requiredPolicy\">\n        <a class=\"nav-link\" [routerLink]=\"[route.url]\">{{ route.name | abpLocalization }}</a>\n      </li>\n    </ng-template>\n\n    <ng-template #dropdownLink let-route>\n      <li\n        #navbarRootDropdown\n        ngbDropdown\n        [abpPermission]=\"route.requiredPolicy\"\n        [abpVisibility]=\"routeContainer\"\n        class=\"nav-item dropdown pointer\"\n        display=\"static\"\n      >\n        <a ngbDropdownToggle class=\"nav-link dropdown-toggle pointer\" data-toggle=\"dropdown\">\n          {{ route.name | abpLocalization }}\n        </a>\n        <div #routeContainer ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n          <ng-template\n            #forTemplate\n            ngFor\n            [ngForOf]=\"route.children\"\n            [ngForTrackBy]=\"trackByFn\"\n            [ngForTemplate]=\"childWrapper\"\n          ></ng-template>\n        </div>\n      </li>\n    </ng-template>\n\n    <ng-template #childWrapper let-child>\n      <ng-template\n        [ngTemplateOutlet]=\"child?.children?.length ? dropdownChild : defaultChild\"\n        [ngTemplateOutletContext]=\"{ $implicit: child }\"\n      ></ng-template>\n    </ng-template>\n\n    <ng-template #defaultChild let-child>\n      <div class=\"dropdown-submenu\" [abpPermission]=\"child.requiredPolicy\">\n        <a class=\"dropdown-item py-2 px-2\" [routerLink]=\"[child.url]\">\n          <i *ngIf=\"child.iconClass\" [ngClass]=\"child.iconClass\"></i>\n          {{ child.name | abpLocalization }}</a\n        >\n      </div>\n    </ng-template>\n\n    <ng-template #dropdownChild let-child>\n      <div\n        [abpVisibility]=\"childrenContainer\"\n        class=\"dropdown-submenu pointer\"\n        ngbDropdown\n        [display]=\"isDropdownChildDynamic ? 'dynamic' : 'static'\"\n        placement=\"right-top\"\n        [abpPermission]=\"child.requiredPolicy\"\n      >\n        <div ngbDropdownToggle [class.dropdown-toggle]=\"false\" class=\"pointer\">\n          <a\n            abpEllipsis=\"210px\"\n            [abpEllipsisEnabled]=\"isDropdownChildDynamic\"\n            role=\"button\"\n            class=\"btn d-block text-left py-2 px-2 dropdown-toggle\"\n          >\n            <i *ngIf=\"child.iconClass\" [ngClass]=\"child.iconClass\"></i>\n            {{ child.name | abpLocalization }}\n          </a>\n        </div>\n        <div #childrenContainer ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n          <ng-template\n            ngFor\n            [ngForOf]=\"child.children\"\n            [ngForTrackBy]=\"trackByFn\"\n            [ngForTemplate]=\"childWrapper\"\n          ></ng-template>\n        </div>\n      </div>\n    </ng-template>\n  </ul>\n\n  <ul class=\"navbar-nav ml-auto\">\n    <ng-container\n      *ngFor=\"let element of rightPartElements; trackBy: trackElementByFn\"\n      [ngTemplateOutlet]=\"element\"\n    ></ng-container>\n  </ul>\n</abp-layout>\n\n<ng-template #language>\n  <li class=\"nav-item dropdown pointer\" ngbDropdown>\n    <a ngbDropdownToggle class=\"nav-link dropdown-toggle text-white pointer\" data-toggle=\"dropdown\">\n      {{ defaultLanguage$ | async }}\n    </a>\n    <div ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n      <a\n        *ngFor=\"let lang of dropdownLanguages$ | async\"\n        class=\"dropdown-item\"\n        (click)=\"onChangeLang(lang.cultureName)\"\n        >{{ lang?.displayName }}</a\n      >\n    </div>\n  </li>\n</ng-template>\n\n<ng-template #currentUser>\n  <li *ngIf=\"(currentUser$ | async)?.isAuthenticated\" class=\"nav-item dropdown pointer\" ngbDropdown>\n    <a ngbDropdownToggle class=\"nav-link dropdown-toggle text-white pointer\" data-toggle=\"dropdown\">\n      {{ (currentUser$ | async)?.userName }}\n    </a>\n    <div ngbDropdownMenu class=\"dropdown-menu dropdown-menu-right\">\n      <a class=\"dropdown-item pointer\" (click)=\"isOpenChangePassword = true\">{{\n        'AbpUi::ChangePassword' | abpLocalization\n      }}</a>\n      <a class=\"dropdown-item pointer\" (click)=\"isOpenProfile = true\">{{ 'AbpUi::PersonalInfo' | abpLocalization }}</a>\n      <a class=\"dropdown-item pointer\" (click)=\"logout()\">{{ 'AbpUi::Logout' | abpLocalization }}</a>\n    </div>\n  </li>\n\n  <abp-change-password [(visible)]=\"isOpenChangePassword\"></abp-change-password>\n\n  <abp-profile [(visible)]=\"isOpenProfile\"></abp-profile>\n</ng-template>\n"
                }] }
    ];
    /** @nocollapse */
    ApplicationLayoutComponent.ctorParameters = function () { return [
        { type: Store },
        { type: OAuthService }
    ]; };
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
    ApplicationLayoutComponent.prototype.navbarRootDropdowns;
    /** @type {?} */
    ApplicationLayoutComponent.prototype.isOpenChangePassword;
    /** @type {?} */
    ApplicationLayoutComponent.prototype.isOpenProfile;
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
    function (acc, val) {
        if (val.invisible)
            return acc;
        if (val.children && val.children.length) {
            val.children = getVisibleRoutes(val.children);
        }
        return tslib_1.__spread(acc, [val]);
    }), []);
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9hcHBsaWNhdGlvbi1sYXlvdXQvYXBwbGljYXRpb24tbGF5b3V0LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFHTCxtQkFBbUIsRUFDbkIsV0FBVyxFQUVYLFdBQVcsRUFDWCxZQUFZLEVBQ1osZ0JBQWdCLEdBQ2pCLE1BQU0sY0FBYyxDQUFDO0FBQ3RCLE9BQU8sRUFFTCxTQUFTLEVBRVQsU0FBUyxFQUNULFdBQVcsRUFFWCxTQUFTLEVBQ1QsWUFBWSxHQUNiLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUN6RCxPQUFPLEVBQUUsUUFBUSxFQUFFLFdBQVcsRUFBRSxNQUFNLHFCQUFxQixDQUFDO0FBQzVELE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxxQkFBcUIsQ0FBQztBQUNuRCxPQUFPLE9BQU8sTUFBTSxjQUFjLENBQUM7QUFDbkMsT0FBTyxFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDN0MsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDM0QsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBQ3RCLE9BQU8sRUFBRSxvQkFBb0IsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUVyRCxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBRTNDO0lBZ0VFLG9DQUFvQixLQUFZLEVBQVUsWUFBMEI7UUFBaEQsVUFBSyxHQUFMLEtBQUssQ0FBTztRQUFVLGlCQUFZLEdBQVosWUFB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QkFBd0IsQ0FBQyxDQUFDOzBDQUMxQyxVQUFVO2tFQUFzQztJQUc1RDtRQURDLE1BQU0sQ0FBQyxXQUFXLENBQUMscUJBQXFCLENBQUM7MENBQzVCLFVBQVU7b0VBQTZCO0lBOEd2RCxpQ0FBQztDQUFBLEFBaElELElBZ0lDO1NBNUhZLDBCQUEwQjs7O0lBRXJDLGdDQUFzQzs7SUFFdEMsNkNBQ3FDOztJQUVyQyxrREFDK0Q7O0lBRS9ELGdEQUM0RDs7SUFFNUQsa0RBQ3FEOztJQUVyRCxvREFDaUM7O0lBRWpDLGlEQUM4Qjs7SUFFOUIseURBQzRDOztJQUU1QywwREFBc0M7O0lBRXRDLG1EQUErQjs7SUFFL0IsNERBQWdDOztJQXlCaEMsdURBQTJDOztJQUUzQywrQ0FBbUU7O0lBRW5FLHNEQUEyRTs7Ozs7SUFFL0QsMkNBQW9COzs7OztJQUFFLGtEQUFrQzs7Ozs7O0FBa0V0RSxTQUFTLGdCQUFnQixDQUFDLE1BQXVCO0lBQy9DLE9BQU8sTUFBTSxDQUFDLE1BQU07Ozs7O0lBQUMsVUFBQyxHQUFHLEVBQUUsR0FBRztRQUM1QixJQUFJLEdBQUcsQ0FBQyxTQUFTO1lBQUUsT0FBTyxHQUFHLENBQUM7UUFFOUIsSUFBSSxHQUFHLENBQUMsUUFBUSxJQUFJLEdBQUcsQ0FBQyxRQUFRLENBQUMsTUFBTSxFQUFFO1lBQ3ZDLEdBQUcsQ0FBQyxRQUFRLEdBQUcsZ0JBQWdCLENBQUMsR0FBRyxDQUFDLFFBQVEsQ0FBQyxDQUFDO1NBQy9DO1FBRUQsd0JBQVcsR0FBRyxHQUFFLEdBQUcsR0FBRTtJQUN2QixDQUFDLEdBQUUsRUFBRSxDQUFDLENBQUM7QUFDVCxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcbiAgQUJQLFxuICBBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24sXG4gIEdldEFwcENvbmZpZ3VyYXRpb24sXG4gIENvbmZpZ1N0YXRlLFxuICBlTGF5b3V0VHlwZSxcbiAgU2V0TGFuZ3VhZ2UsXG4gIFNlc3Npb25TdGF0ZSxcbiAgdGFrZVVudGlsRGVzdHJveSxcbn0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7XG4gIEFmdGVyVmlld0luaXQsXG4gIENvbXBvbmVudCxcbiAgT25EZXN0cm95LFxuICBRdWVyeUxpc3QsXG4gIFRlbXBsYXRlUmVmLFxuICBUcmFja0J5RnVuY3Rpb24sXG4gIFZpZXdDaGlsZCxcbiAgVmlld0NoaWxkcmVuLFxufSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IE5nYkRyb3Bkb3duIH0gZnJvbSAnQG5nLWJvb3RzdHJhcC9uZy1ib290c3RyYXAnO1xuaW1wb3J0IHsgTmF2aWdhdGUsIFJvdXRlclN0YXRlIH0gZnJvbSAnQG5neHMvcm91dGVyLXBsdWdpbic7XG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgT0F1dGhTZXJ2aWNlIH0gZnJvbSAnYW5ndWxhci1vYXV0aDItb2lkYyc7XG5pbXBvcnQgY29tcGFyZSBmcm9tICdqdXN0LWNvbXBhcmUnO1xuaW1wb3J0IHsgZnJvbUV2ZW50LCBPYnNlcnZhYmxlIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBkZWJvdW5jZVRpbWUsIGZpbHRlciwgbWFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xuaW1wb3J0IHsgQWRkTmF2aWdhdGlvbkVsZW1lbnQgfSBmcm9tICcuLi8uLi9hY3Rpb25zJztcbmltcG9ydCB7IExheW91dCB9IGZyb20gJy4uLy4uL21vZGVscy9sYXlvdXQnO1xuaW1wb3J0IHsgTGF5b3V0U3RhdGUgfSBmcm9tICcuLi8uLi9zdGF0ZXMnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtbGF5b3V0LWFwcGxpY2F0aW9uJyxcbiAgdGVtcGxhdGVVcmw6ICcuL2FwcGxpY2F0aW9uLWxheW91dC5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIEFwcGxpY2F0aW9uTGF5b3V0Q29tcG9uZW50IGltcGxlbWVudHMgQWZ0ZXJWaWV3SW5pdCwgT25EZXN0cm95IHtcbiAgLy8gcmVxdWlyZWQgZm9yIGR5bmFtaWMgY29tcG9uZW50XG4gIHN0YXRpYyB0eXBlID0gZUxheW91dFR5cGUuYXBwbGljYXRpb247XG5cbiAgQFNlbGVjdChDb25maWdTdGF0ZS5nZXRPbmUoJ3JvdXRlcycpKVxuICByb3V0ZXMkOiBPYnNlcnZhYmxlPEFCUC5GdWxsUm91dGVbXT47XG5cbiAgQFNlbGVjdChDb25maWdTdGF0ZS5nZXRPbmUoJ2N1cnJlbnRVc2VyJykpXG4gIGN1cnJlbnRVc2VyJDogT2JzZXJ2YWJsZTxBcHBsaWNhdGlvbkNvbmZpZ3VyYXRpb24uQ3VycmVudFVzZXI+O1xuXG4gIEBTZWxlY3QoQ29uZmlnU3RhdGUuZ2V0RGVlcCgnbG9jYWxpemF0aW9uLmxhbmd1YWdlcycpKVxuICBsYW5ndWFnZXMkOiBPYnNlcnZhYmxlPEFwcGxpY2F0aW9uQ29uZmlndXJhdGlvbi5MYW5ndWFnZVtdPjtcblxuICBAU2VsZWN0KExheW91dFN0YXRlLmdldE5hdmlnYXRpb25FbGVtZW50cylcbiAgbmF2RWxlbWVudHMkOiBPYnNlcnZhYmxlPExheW91dC5OYXZpZ2F0aW9uRWxlbWVudFtdPjtcblxuICBAVmlld0NoaWxkKCdjdXJyZW50VXNlcicsIHsgc3RhdGljOiBmYWxzZSwgcmVhZDogVGVtcGxhdGVSZWYgfSlcbiAgY3VycmVudFVzZXJSZWY6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgQFZpZXdDaGlsZCgnbGFuZ3VhZ2UnLCB7IHN0YXRpYzogZmFsc2UsIHJlYWQ6IFRlbXBsYXRlUmVmIH0pXG4gIGxhbmd1YWdlUmVmOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBWaWV3Q2hpbGRyZW4oJ25hdmJhclJvb3REcm9wZG93bicsIHsgcmVhZDogTmdiRHJvcGRvd24gfSlcbiAgbmF2YmFyUm9vdERyb3Bkb3duczogUXVlcnlMaXN0PE5nYkRyb3Bkb3duPjtcblxuICBpc09wZW5DaGFuZ2VQYXNzd29yZDogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIGlzT3BlblByb2ZpbGU6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBpc0Ryb3Bkb3duQ2hpbGREeW5hbWljOiBib29sZWFuO1xuXG4gIGdldCB2aXNpYmxlUm91dGVzJCgpOiBPYnNlcnZhYmxlPEFCUC5GdWxsUm91dGVbXT4ge1xuICAgIHJldHVybiB0aGlzLnJvdXRlcyQucGlwZShtYXAocm91dGVzID0+IGdldFZpc2libGVSb3V0ZXMocm91dGVzKSkpO1xuICB9XG5cbiAgZ2V0IGRlZmF1bHRMYW5ndWFnZSQoKTogT2JzZXJ2YWJsZTxzdHJpbmc+IHtcbiAgICByZXR1cm4gdGhpcy5sYW5ndWFnZXMkLnBpcGUoXG4gICAgICBtYXAoXG4gICAgICAgIGxhbmd1YWdlcyA9PiBzbnEoKCkgPT4gbGFuZ3VhZ2VzLmZpbmQobGFuZyA9PiBsYW5nLmN1bHR1cmVOYW1lID09PSB0aGlzLnNlbGVjdGVkTGFuZ0N1bHR1cmUpLmRpc3BsYXlOYW1lKSxcbiAgICAgICAgJycsXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBnZXQgZHJvcGRvd25MYW5ndWFnZXMkKCk6IE9ic2VydmFibGU8QXBwbGljYXRpb25Db25maWd1cmF0aW9uLkxhbmd1YWdlW10+IHtcbiAgICByZXR1cm4gdGhpcy5sYW5ndWFnZXMkLnBpcGUoXG4gICAgICBtYXAobGFuZ3VhZ2VzID0+IHNucSgoKSA9PiBsYW5ndWFnZXMuZmlsdGVyKGxhbmcgPT4gbGFuZy5jdWx0dXJlTmFtZSAhPT0gdGhpcy5zZWxlY3RlZExhbmdDdWx0dXJlKSksIFtdKSxcbiAgICApO1xuICB9XG5cbiAgZ2V0IHNlbGVjdGVkTGFuZ0N1bHR1cmUoKTogc3RyaW5nIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChTZXNzaW9uU3RhdGUuZ2V0TGFuZ3VhZ2UpO1xuICB9XG5cbiAgcmlnaHRQYXJ0RWxlbWVudHM6IFRlbXBsYXRlUmVmPGFueT5bXSA9IFtdO1xuXG4gIHRyYWNrQnlGbjogVHJhY2tCeUZ1bmN0aW9uPEFCUC5GdWxsUm91dGU+ID0gKF8sIGl0ZW0pID0+IGl0ZW0ubmFtZTtcblxuICB0cmFja0VsZW1lbnRCeUZuOiBUcmFja0J5RnVuY3Rpb248QUJQLkZ1bGxSb3V0ZT4gPSAoXywgZWxlbWVudCkgPT4gZWxlbWVudDtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSwgcHJpdmF0ZSBvYXV0aFNlcnZpY2U6IE9BdXRoU2VydmljZSkge31cblxuICBwcml2YXRlIGNoZWNrV2luZG93V2lkdGgoKSB7XG4gICAgc2V0VGltZW91dCgoKSA9PiB7XG4gICAgICB0aGlzLm5hdmJhclJvb3REcm9wZG93bnMuZm9yRWFjaChpdGVtID0+IHtcbiAgICAgICAgaXRlbS5jbG9zZSgpO1xuICAgICAgfSk7XG4gICAgICBpZiAod2luZG93LmlubmVyV2lkdGggPCA3NjgpIHtcbiAgICAgICAgdGhpcy5pc0Ryb3Bkb3duQ2hpbGREeW5hbWljID0gZmFsc2U7XG4gICAgICB9IGVsc2Uge1xuICAgICAgICB0aGlzLmlzRHJvcGRvd25DaGlsZER5bmFtaWMgPSB0cnVlO1xuICAgICAgfVxuICAgIH0sIDApO1xuICB9XG5cbiAgbmdBZnRlclZpZXdJbml0KCkge1xuICAgIGNvbnN0IG5hdmlnYXRpb25zID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChMYXlvdXRTdGF0ZS5nZXROYXZpZ2F0aW9uRWxlbWVudHMpLm1hcCgoeyBuYW1lIH0pID0+IG5hbWUpO1xuXG4gICAgaWYgKG5hdmlnYXRpb25zLmluZGV4T2YoJ0xhbmd1YWdlUmVmJykgPCAwKSB7XG4gICAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKFxuICAgICAgICBuZXcgQWRkTmF2aWdhdGlvbkVsZW1lbnQoW1xuICAgICAgICAgIHsgZWxlbWVudDogdGhpcy5sYW5ndWFnZVJlZiwgb3JkZXI6IDQsIG5hbWU6ICdMYW5ndWFnZVJlZicgfSxcbiAgICAgICAgICB7IGVsZW1lbnQ6IHRoaXMuY3VycmVudFVzZXJSZWYsIG9yZGVyOiA1LCBuYW1lOiAnQ3VycmVudFVzZXJSZWYnIH0sXG4gICAgICAgIF0pLFxuICAgICAgKTtcbiAgICB9XG5cbiAgICB0aGlzLm5hdkVsZW1lbnRzJFxuICAgICAgLnBpcGUoXG4gICAgICAgIG1hcChlbGVtZW50cyA9PiBlbGVtZW50cy5tYXAoKHsgZWxlbWVudCB9KSA9PiBlbGVtZW50KSksXG4gICAgICAgIGZpbHRlcihlbGVtZW50cyA9PiAhY29tcGFyZShlbGVtZW50cywgdGhpcy5yaWdodFBhcnRFbGVtZW50cykpLFxuICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZShlbGVtZW50cyA9PiB7XG4gICAgICAgIHNldFRpbWVvdXQoKCkgPT4gKHRoaXMucmlnaHRQYXJ0RWxlbWVudHMgPSBlbGVtZW50cyksIDApO1xuICAgICAgfSk7XG5cbiAgICB0aGlzLmNoZWNrV2luZG93V2lkdGgoKTtcblxuICAgIGZyb21FdmVudCh3aW5kb3csICdyZXNpemUnKVxuICAgICAgLnBpcGUoXG4gICAgICAgIHRha2VVbnRpbERlc3Ryb3kodGhpcyksXG4gICAgICAgIGRlYm91bmNlVGltZSgyNTApLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgIHRoaXMuY2hlY2tXaW5kb3dXaWR0aCgpO1xuICAgICAgfSk7XG4gIH1cblxuICBuZ09uRGVzdHJveSgpIHt9XG5cbiAgb25DaGFuZ2VMYW5nKGN1bHR1cmVOYW1lOiBzdHJpbmcpIHtcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBTZXRMYW5ndWFnZShjdWx0dXJlTmFtZSkpO1xuICB9XG5cbiAgbG9nb3V0KCkge1xuICAgIHRoaXMub2F1dGhTZXJ2aWNlLmxvZ091dCgpO1xuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2goXG4gICAgICBuZXcgTmF2aWdhdGUoWycvJ10sIG51bGwsIHtcbiAgICAgICAgc3RhdGU6IHsgcmVkaXJlY3RVcmw6IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoUm91dGVyU3RhdGUpLnN0YXRlLnVybCB9LFxuICAgICAgfSksXG4gICAgKTtcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBHZXRBcHBDb25maWd1cmF0aW9uKCkpO1xuICB9XG59XG5cbmZ1bmN0aW9uIGdldFZpc2libGVSb3V0ZXMocm91dGVzOiBBQlAuRnVsbFJvdXRlW10pIHtcbiAgcmV0dXJuIHJvdXRlcy5yZWR1Y2UoKGFjYywgdmFsKSA9PiB7XG4gICAgaWYgKHZhbC5pbnZpc2libGUpIHJldHVybiBhY2M7XG5cbiAgICBpZiAodmFsLmNoaWxkcmVuICYmIHZhbC5jaGlsZHJlbi5sZW5ndGgpIHtcbiAgICAgIHZhbC5jaGlsZHJlbiA9IGdldFZpc2libGVSb3V0ZXModmFsLmNoaWxkcmVuKTtcbiAgICB9XG5cbiAgICByZXR1cm4gWy4uLmFjYywgdmFsXTtcbiAgfSwgW10pO1xufVxuIl19