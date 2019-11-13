import { ConfigState, DynamicLayoutComponent, CoreModule } from '@abp/ng.core';
import { getSettingTabs, ThemeSharedModule } from '@abp/ng.theme.shared';
import { Component, NgModule } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { Action, Selector, State, Store, NgxsModule } from '@ngxs/store';
import { __decorate, __metadata } from 'tslib';

/**
 * @fileoverview added by tsickle
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
__decorate(
  [
    Action(SetSelectedSettingTab),
    __metadata('design:type', Function),
    __metadata('design:paramtypes', [Object, SetSelectedSettingTab]),
    __metadata('design:returntype', void 0),
  ],
  SettingManagementState.prototype,
  'settingManagementAction',
  null,
);
__decorate(
  [
    Selector(),
    __metadata('design:type', Function),
    __metadata('design:paramtypes', [Object]),
    __metadata('design:returntype', void 0),
  ],
  SettingManagementState,
  'getSelectedTab',
  null,
);
SettingManagementState = __decorate(
  [
    State({
      name: 'SettingManagementState',
      defaults: /** @type {?} */ ({ selectedTab: {} }),
    }),
  ],
  SettingManagementState,
);

/**
 * @fileoverview added by tsickle
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
    this.trackByFn
    /**
     * @param {?} _
     * @param {?} item
     * @return {?}
     */ = (_, item) => item.name;
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
      .filter(
        /**
         * @param {?} setting
         * @return {?}
         */
        setting => this.store.selectSnapshot(ConfigState.getGrantedPolicy(setting.requiredPolicy)),
      )
      .sort(
        /**
         * @param {?} a
         * @param {?} b
         * @return {?}
         */
        (a, b) => a.order - b.order,
      );
    if (!this.selected && this.settings.length) {
      this.selected = this.settings[0];
    }
  }
}
SettingManagementComponent.decorators = [
  {
    type: Component,
    args: [
      {
        selector: 'abp-setting-management',
        template:
          '<div class="row entry-row">\n  <div class="col-auto">\n    <h1 class="content-header-title">{{ \'AbpSettingManagement::Settings\' | abpLocalization }}</h1>\n  </div>\n  <div id="breadcrumb" class="col-md-auto pl-md-0">\n    <abp-breadcrumb></abp-breadcrumb>\n  </div>\n  <div class="col">\n    <div class="text-lg-right pt-2" id="AbpContentToolbar"></div>\n  </div>\n</div>\n\n<div id="SettingManagementWrapper">\n  <div class="card">\n    <div class="card-body">\n      <div class="row">\n        <div class="col-3">\n          <ul class="nav flex-column nav-pills" id="nav-tab" role="tablist">\n            <li\n              *abpFor="let setting of settings; trackBy: trackByFn"\n              (click)="selected = setting"\n              class="nav-item"\n              [abpPermission]="setting.requiredPolicy"\n            >\n              <a\n                class="nav-link"\n                [id]="setting.name + \'-tab\'"\n                role="tab"\n                [class.active]="setting.name === selected.name"\n                >{{ setting.name | abpLocalization }}</a\n              >\n            </li>\n          </ul>\n        </div>\n        <div class="col-9">\n          <div *ngIf="settings.length" class="tab-content">\n            <div class="tab-pane fade show active" [id]="selected.name + \'-tab\'" role="tabpanel">\n              <ng-container *ngComponentOutlet="selected.component"></ng-container>\n            </div>\n          </div>\n        </div>\n      </div>\n    </div>\n  </div>\n</div>\n',
      },
    ],
  },
];
/** @nocollapse */
SettingManagementComponent.ctorParameters = () => [{ type: Router }, { type: Store }];
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
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
/** @type {?} */
const routes = [
  {
    path: '',
    component: DynamicLayoutComponent,
    children: [{ path: '', component: SettingManagementComponent }],
  },
];
class SettingManagementRoutingModule {}
SettingManagementRoutingModule.decorators = [
  {
    type: NgModule,
    args: [
      {
        imports: [RouterModule.forChild(routes)],
        exports: [RouterModule],
      },
    ],
  },
];

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
class SettingManagementModule {}
SettingManagementModule.decorators = [
  {
    type: NgModule,
    args: [
      {
        declarations: [SettingManagementComponent],
        imports: [
          SettingManagementRoutingModule,
          CoreModule,
          ThemeSharedModule,
          NgxsModule.forFeature([SettingManagementState]),
        ],
      },
    ],
  },
];

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */

export {
  SettingManagementComponent,
  SettingManagementModule,
  SettingManagementRoutingModule as ɵa,
  SettingManagementState as ɵb,
  SetSelectedSettingTab as ɵc,
};
//# sourceMappingURL=abp-ng.setting-management.js.map
