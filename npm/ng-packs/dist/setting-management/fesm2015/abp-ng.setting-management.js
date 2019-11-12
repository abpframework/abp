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
class SetSelectedSettingTab {
    /**
     * @param {?} payload
     */
    constructor(payload) {
        this.payload = payload;
    }
}
SetSelectedSettingTab.type = '[SettingManagement] Set Selected Tab';
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
let SettingManagementState = class SettingManagementState {
    /**
     * @param {?} __0
     * @return {?}
     */
    static getSelectedTab({ selectedTab }) {
        return selectedTab;
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    settingManagementAction({ patchState }, { payload }) {
        patchState({
            selectedTab: payload,
        });
    }
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

/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/setting-management.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class SettingManagementComponent {
    /**
     * @param {?} router
     * @param {?} store
     */
    constructor(router, store) {
        this.router = router;
        this.store = store;
        this.settings = [];
        this.trackByFn = (/**
         * @param {?} _
         * @param {?} item
         * @return {?}
         */
        (_, item) => item.name);
    }
    /**
     * @param {?} value
     * @return {?}
     */
    set selected(value) {
        this.store.dispatch(new SetSelectedSettingTab(value));
    }
    /**
     * @return {?}
     */
    get selected() {
        /** @type {?} */
        const value = this.store.selectSnapshot(SettingManagementState.getSelectedTab);
        if ((!value || !value.component) && this.settings.length) {
            return this.settings[0];
        }
        return value;
    }
    /**
     * @return {?}
     */
    ngOnInit() {
        this.settings = getSettingTabs()
            .filter((/**
         * @param {?} setting
         * @return {?}
         */
        setting => this.store.selectSnapshot(ConfigState.getGrantedPolicy(setting.requiredPolicy))))
            .sort((/**
         * @param {?} a
         * @param {?} b
         * @return {?}
         */
        (a, b) => a.order - b.order));
        if (!this.selected && this.settings.length) {
            this.selected = this.settings[0];
        }
    }
}
SettingManagementComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-setting-management',
                template: "<div class=\"row entry-row\">\r\n  <div class=\"col-auto\">\r\n    <h1 class=\"content-header-title\">{{ 'AbpSettingManagement::Settings' | abpLocalization }}</h1>\r\n  </div>\r\n  <div id=\"breadcrumb\" class=\"col-md-auto pl-md-0\">\r\n    <abp-breadcrumb></abp-breadcrumb>\r\n  </div>\r\n  <div class=\"col\">\r\n    <div class=\"text-lg-right pt-2\" id=\"AbpContentToolbar\"></div>\r\n  </div>\r\n</div>\r\n\r\n<div id=\"SettingManagementWrapper\">\r\n  <div class=\"card\">\r\n    <div class=\"card-body\">\r\n      <div class=\"row\">\r\n        <div class=\"col-12 col-md-3\">\r\n          <ul class=\"nav flex-column nav-pills\" id=\"nav-tab\" role=\"tablist\">\r\n            <li\r\n              *abpFor=\"let setting of settings; trackBy: trackByFn\"\r\n              (click)=\"selected = setting\"\r\n              class=\"nav-item\"\r\n              [abpPermission]=\"setting.requiredPolicy\"\r\n            >\r\n              <a\r\n                class=\"nav-link\"\r\n                [id]=\"setting.name + '-tab'\"\r\n                role=\"tab\"\r\n                [class.active]=\"setting.name === selected.name\"\r\n                >{{ setting.name | abpLocalization }}</a\r\n              >\r\n            </li>\r\n          </ul>\r\n        </div>\r\n        <div class=\"col-12 col-md-9\">\r\n          <div *ngIf=\"settings.length\" class=\"tab-content\">\r\n            <div class=\"tab-pane fade show active\" [id]=\"selected.name + '-tab'\" role=\"tabpanel\">\r\n              <ng-container *ngComponentOutlet=\"selected.component\"></ng-container>\r\n            </div>\r\n          </div>\r\n        </div>\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n"
            }] }
];
/** @nocollapse */
SettingManagementComponent.ctorParameters = () => [
    { type: Router },
    { type: Store }
];
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
const ɵ0 = { requiredPolicy: 'AbpAccount.SettingManagement' };
/** @type {?} */
const routes = [
    {
        path: '',
        component: DynamicLayoutComponent,
        children: [
            { path: '', component: SettingManagementComponent, data: ɵ0 },
        ],
    },
];
class SettingManagementRoutingModule {
}
SettingManagementRoutingModule.decorators = [
    { type: NgModule, args: [{
                imports: [RouterModule.forChild(routes)],
                exports: [RouterModule],
            },] }
];

/**
 * @fileoverview added by tsickle
 * Generated from: lib/setting-management.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class SettingManagementModule {
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
