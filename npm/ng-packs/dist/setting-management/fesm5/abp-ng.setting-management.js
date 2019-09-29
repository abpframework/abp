import { GetAppConfiguration, ConfigState, DynamicLayoutComponent, CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { Injectable, ɵɵdefineInjectable, ɵɵinject, Component, NgModule } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { Navigate } from '@ngxs/router-plugin';
import { ofActionSuccessful, Actions, Store } from '@ngxs/store';
import { Subject } from 'rxjs';
import { OAuthService } from 'angular-oauth2-oidc';
import { takeUntil } from 'rxjs/operators';

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var SettingManagementService = /** @class */ (function () {
    function SettingManagementService(actions, router, store, oAuthService) {
        var _this = this;
        this.actions = actions;
        this.router = router;
        this.store = store;
        this.oAuthService = oAuthService;
        this.settings = [];
        this.selected = (/** @type {?} */ ({}));
        this.destroy$ = new Subject();
        setTimeout((/**
         * @return {?}
         */
        function () { return _this.setSettings(); }), 0);
        this.actions
            .pipe(ofActionSuccessful(GetAppConfiguration))
            .pipe(takeUntil(this.destroy$))
            .subscribe((/**
         * @return {?}
         */
        function () {
            if (_this.oAuthService.hasValidAccessToken()) {
                _this.setSettings();
            }
        }));
    }
    /**
     * @return {?}
     */
    SettingManagementService.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () {
        this.destroy$.next();
    };
    /**
     * @return {?}
     */
    SettingManagementService.prototype.setSettings = /**
     * @return {?}
     */
    function () {
        var _this = this;
        /** @type {?} */
        var route = this.router.config.find((/**
         * @param {?} r
         * @return {?}
         */
        function (r) { return r.path === 'setting-management'; }));
        this.settings = ((/** @type {?} */ (route.data.settings)))
            .filter((/**
         * @param {?} setting
         * @return {?}
         */
        function (setting) { return _this.store.selectSnapshot(ConfigState.getGrantedPolicy(setting.requiredPolicy)); }))
            .sort((/**
         * @param {?} a
         * @param {?} b
         * @return {?}
         */
        function (a, b) { return a.order - b.order; }));
        this.checkSelected();
    };
    /**
     * @return {?}
     */
    SettingManagementService.prototype.checkSelected = /**
     * @return {?}
     */
    function () {
        var _this = this;
        this.selected = this.settings.find((/**
         * @param {?} setting
         * @return {?}
         */
        function (setting) { return setting.url === _this.router.url; })) || ((/** @type {?} */ ({})));
        if (!this.selected.name && this.settings.length) {
            this.setSelected(this.settings[0]);
        }
    };
    /**
     * @param {?} selected
     * @return {?}
     */
    SettingManagementService.prototype.setSelected = /**
     * @param {?} selected
     * @return {?}
     */
    function (selected) {
        this.selected = selected;
        this.store.dispatch(new Navigate([selected.url]));
    };
    SettingManagementService.decorators = [
        { type: Injectable, args: [{ providedIn: 'root' },] }
    ];
    /** @nocollapse */
    SettingManagementService.ctorParameters = function () { return [
        { type: Actions },
        { type: Router },
        { type: Store },
        { type: OAuthService }
    ]; };
    /** @nocollapse */ SettingManagementService.ngInjectableDef = ɵɵdefineInjectable({ factory: function SettingManagementService_Factory() { return new SettingManagementService(ɵɵinject(Actions), ɵɵinject(Router), ɵɵinject(Store), ɵɵinject(OAuthService)); }, token: SettingManagementService, providedIn: "root" });
    return SettingManagementService;
}());
if (false) {
    /** @type {?} */
    SettingManagementService.prototype.settings;
    /** @type {?} */
    SettingManagementService.prototype.selected;
    /**
     * @type {?}
     * @private
     */
    SettingManagementService.prototype.destroy$;
    /**
     * @type {?}
     * @private
     */
    SettingManagementService.prototype.actions;
    /**
     * @type {?}
     * @private
     */
    SettingManagementService.prototype.router;
    /**
     * @type {?}
     * @private
     */
    SettingManagementService.prototype.store;
    /**
     * @type {?}
     * @private
     */
    SettingManagementService.prototype.oAuthService;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var SettingLayoutComponent = /** @class */ (function () {
    function SettingLayoutComponent(settingManagementService, router) {
        this.settingManagementService = settingManagementService;
        this.router = router;
        this.trackByFn = (/**
         * @param {?} _
         * @param {?} item
         * @return {?}
         */
        function (_, item) { return item.name; });
        if (settingManagementService.selected &&
            this.router.url !== settingManagementService.selected.url &&
            settingManagementService.settings.length) {
            settingManagementService.setSelected(settingManagementService.settings[0]);
        }
    }
    /**
     * @return {?}
     */
    SettingLayoutComponent.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () { };
    // required for dynamic component
    SettingLayoutComponent.type = "setting" /* setting */;
    SettingLayoutComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-setting-layout',
                    template: "<div class=\"row entry-row\">\n  <div class=\"col-auto\">\n    <h1 class=\"content-header-title\">{{ 'AbpSettingManagement::Settings' | abpLocalization }}</h1>\n  </div>\n  <!-- <div id=\"breadcrumb\" class=\"col-md-auto pl-md-0\">\n    <abp-breadcrumb></abp-breadcrumb>\n  </div> -->\n  <div class=\"col\">\n    <div class=\"text-lg-right pt-2\" id=\"AbpContentToolbar\"></div>\n  </div>\n</div>\n\n<div id=\"SettingManagementWrapper\">\n  <div class=\"card\">\n    <div class=\"card-body\">\n      <div *ngIf=\"!settingManagementService.settings.length\" class=\"text-center\">\n        <i class=\"fa fa-spinner fa-spin\"></i>\n      </div>\n      <div class=\"row\">\n        <div class=\"col-3\">\n          <ul class=\"nav flex-column nav-pills\" id=\"nav-tab\" role=\"tablist\">\n            <li\n              *abpFor=\"\n                let setting of settingManagementService.settings;\n                trackBy: trackByFn;\n                orderBy: 'order';\n                orderDir: 'ASC'\n              \"\n              (click)=\"settingManagementService.setSelected(setting)\"\n              class=\"nav-item\"\n              [abpPermission]=\"setting.requiredPolicy\"\n            >\n              <a\n                class=\"nav-link\"\n                [id]=\"setting.name + '-tab'\"\n                role=\"tab\"\n                [class.active]=\"setting.name === settingManagementService.selected.name\"\n                >{{ setting.name | abpLocalization }}</a\n              >\n            </li>\n          </ul>\n        </div>\n        <div class=\"col-9\">\n          <div *ngIf=\"settingManagementService.settings.length\" class=\"tab-content\">\n            <div\n              class=\"tab-pane fade show active\"\n              [id]=\"settingManagementService.selected.name + '-tab'\"\n              role=\"tabpanel\"\n            >\n              <router-outlet></router-outlet>\n            </div>\n          </div>\n        </div>\n      </div>\n    </div>\n  </div>\n</div>\n"
                }] }
    ];
    /** @nocollapse */
    SettingLayoutComponent.ctorParameters = function () { return [
        { type: SettingManagementService },
        { type: Router }
    ]; };
    return SettingLayoutComponent;
}());
if (false) {
    /** @type {?} */
    SettingLayoutComponent.type;
    /** @type {?} */
    SettingLayoutComponent.prototype.trackByFn;
    /** @type {?} */
    SettingLayoutComponent.prototype.settingManagementService;
    /**
     * @type {?}
     * @private
     */
    SettingLayoutComponent.prototype.router;
}

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/** @type {?} */
var SETTING_MANAGEMENT_ROUTES = {
    routes: (/** @type {?} */ ([
        {
            name: 'Settings',
            path: 'setting-management',
            parentName: 'AbpUiNavigation::Menu:Administration',
            layout: "application" /* application */,
            order: 6,
            iconClass: 'fa fa-cog',
        },
    ])),
};

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var ɵ0 = { routes: SETTING_MANAGEMENT_ROUTES, settings: [] };
/** @type {?} */
var routes = [
    {
        path: 'setting-management',
        component: DynamicLayoutComponent,
        children: [{ path: '', component: SettingLayoutComponent }],
        data: ɵ0,
    },
];
var SettingManagementRoutingModule = /** @class */ (function () {
    function SettingManagementRoutingModule() {
    }
    SettingManagementRoutingModule.decorators = [
        { type: NgModule, args: [{
                    imports: [RouterModule.forChild(routes)],
                    exports: [RouterModule],
                },] }
    ];
    return SettingManagementRoutingModule;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/** @type {?} */
var SETTING_LAYOUT = SettingLayoutComponent;
var SettingManagementModule = /** @class */ (function () {
    function SettingManagementModule() {
    }
    SettingManagementModule.decorators = [
        { type: NgModule, args: [{
                    declarations: [SETTING_LAYOUT],
                    imports: [SettingManagementRoutingModule, CoreModule, ThemeSharedModule],
                    entryComponents: [SETTING_LAYOUT],
                },] }
    ];
    return SettingManagementModule;
}());

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

export { SETTING_LAYOUT, SETTING_MANAGEMENT_ROUTES, SettingLayoutComponent, SettingManagementModule, SettingManagementService as ɵa, SettingManagementRoutingModule as ɵb, SETTING_MANAGEMENT_ROUTES as ɵc };
//# sourceMappingURL=abp-ng.setting-management.js.map
