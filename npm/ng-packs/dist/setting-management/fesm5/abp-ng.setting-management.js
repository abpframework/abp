import { ConfigState, DynamicLayoutComponent, CoreModule } from '@abp/ng.core';
import { getSettingTabs, ThemeSharedModule } from '@abp/ng.theme.shared';
import { Component, NgModule } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { Action, Selector, State, Store, NgxsModule } from '@ngxs/store';
import { __decorate, __metadata } from 'tslib';

/**
 * @fileoverview added by tsickle
 * Generated from: lib/actions/setting-management.actions.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var SetSelectedSettingTab = /** @class */ (function () {
    function SetSelectedSettingTab(payload) {
        this.payload = payload;
    }
    SetSelectedSettingTab.type = '[SettingManagement] Set Selected Tab';
    return SetSelectedSettingTab;
}());
if (false) {
    /** @type {?} */
    SetSelectedSettingTab.type;
    /** @type {?} */
    SetSelectedSettingTab.prototype.payload;
}

/**
 * @fileoverview added by tsickle
 * Generated from: lib/states/setting-management.state.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var SettingManagementState = /** @class */ (function () {
    function SettingManagementState() {
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    SettingManagementState.getSelectedTab = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var selectedTab = _a.selectedTab;
        return selectedTab;
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    SettingManagementState.prototype.settingManagementAction = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var patchState = _a.patchState;
        var payload = _b.payload;
        patchState({
            selectedTab: payload,
        });
    };
    __decorate([
        Action(SetSelectedSettingTab),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object, SetSelectedSettingTab]),
        __metadata("design:returntype", void 0)
    ], SettingManagementState.prototype, "settingManagementAction", null);
    __decorate([
        Selector(),
        __metadata("design:type", Function),
        __metadata("design:paramtypes", [Object]),
        __metadata("design:returntype", void 0)
    ], SettingManagementState, "getSelectedTab", null);
    SettingManagementState = __decorate([
        State({
            name: 'SettingManagementState',
            defaults: (/** @type {?} */ ({ selectedTab: {} })),
        })
    ], SettingManagementState);
    return SettingManagementState;
}());

/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/setting-management.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var SettingManagementComponent = /** @class */ (function () {
    function SettingManagementComponent(router, store) {
        this.router = router;
        this.store = store;
        this.settings = [];
        this.trackByFn = (/**
         * @param {?} _
         * @param {?} item
         * @return {?}
         */
        function (_, item) { return item.name; });
    }
    Object.defineProperty(SettingManagementComponent.prototype, "selected", {
        get: /**
         * @return {?}
         */
        function () {
            /** @type {?} */
            var value = this.store.selectSnapshot(SettingManagementState.getSelectedTab);
            if ((!value || !value.component) && this.settings.length) {
                return this.settings[0];
            }
            return value;
        },
        set: /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            this.store.dispatch(new SetSelectedSettingTab(value));
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @return {?}
     */
    SettingManagementComponent.prototype.ngOnInit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        this.settings = getSettingTabs()
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
        if (!this.selected && this.settings.length) {
            this.selected = this.settings[0];
        }
    };
    SettingManagementComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-setting-management',
                    template: "<div class=\"row entry-row\">\n  <div class=\"col-auto\">\n    <h1 class=\"content-header-title\">{{ 'AbpSettingManagement::Settings' | abpLocalization }}</h1>\n  </div>\n  <div id=\"breadcrumb\" class=\"col-md-auto pl-md-0\">\n    <abp-breadcrumb></abp-breadcrumb>\n  </div>\n  <div class=\"col\">\n    <div class=\"text-lg-right pt-2\" id=\"AbpContentToolbar\"></div>\n  </div>\n</div>\n\n<div id=\"SettingManagementWrapper\">\n  <div class=\"card\">\n    <div class=\"card-body\">\n      <div class=\"row\">\n        <div class=\"col-12 col-md-3\">\n          <ul class=\"nav flex-column nav-pills\" id=\"nav-tab\" role=\"tablist\">\n            <li\n              *abpFor=\"let setting of settings; trackBy: trackByFn\"\n              (click)=\"selected = setting\"\n              class=\"nav-item pointer\"\n              [abpPermission]=\"setting.requiredPolicy\"\n            >\n              <a\n                class=\"nav-link\"\n                [id]=\"setting.name + '-tab'\"\n                role=\"tab\"\n                [class.active]=\"setting.name === selected.name\"\n                >{{ setting.name | abpLocalization }}</a\n              >\n            </li>\n          </ul>\n        </div>\n        <div class=\"col-12 col-md-9\">\n          <div *ngIf=\"settings.length\" class=\"tab-content\">\n            <div class=\"tab-pane fade show active\" [id]=\"selected.name + '-tab'\" role=\"tabpanel\">\n              <ng-container *ngComponentOutlet=\"selected.component\"></ng-container>\n            </div>\n          </div>\n        </div>\n      </div>\n    </div>\n  </div>\n</div>\n"
                }] }
    ];
    /** @nocollapse */
    SettingManagementComponent.ctorParameters = function () { return [
        { type: Router },
        { type: Store }
    ]; };
    return SettingManagementComponent;
}());
if (false) {
    /** @type {?} */
    SettingManagementComponent.prototype.settings;
    /** @type {?} */
    SettingManagementComponent.prototype.trackByFn;
    /**
     * @type {?}
     * @private
     */
    SettingManagementComponent.prototype.router;
    /**
     * @type {?}
     * @private
     */
    SettingManagementComponent.prototype.store;
}

/**
 * @fileoverview added by tsickle
 * Generated from: lib/setting-management-routing.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var ɵ0 = { requiredPolicy: 'AbpAccount.SettingManagement' };
/** @type {?} */
var routes = [
    {
        path: '',
        component: DynamicLayoutComponent,
        children: [
            { path: '', component: SettingManagementComponent, data: ɵ0 },
        ],
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
 * Generated from: lib/setting-management.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
var SettingManagementModule = /** @class */ (function () {
    function SettingManagementModule() {
    }
    SettingManagementModule.decorators = [
        { type: NgModule, args: [{
                    declarations: [SettingManagementComponent],
                    imports: [
                        SettingManagementRoutingModule,
                        CoreModule,
                        ThemeSharedModule,
                        NgxsModule.forFeature([SettingManagementState]),
                    ],
                },] }
    ];
    return SettingManagementModule;
}());

/**
 * @fileoverview added by tsickle
 * Generated from: public-api.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * Generated from: abp-ng.setting-management.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

export { SettingManagementComponent, SettingManagementModule, SettingManagementRoutingModule as ɵa, SettingManagementState as ɵb, SetSelectedSettingTab as ɵc };
//# sourceMappingURL=abp-ng.setting-management.js.map
