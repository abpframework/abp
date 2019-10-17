import { ConfigState, DynamicLayoutComponent, CoreModule } from '@abp/ng.core';
import { getSettingTabs, ThemeSharedModule } from '@abp/ng.theme.shared';
import { Component, NgModule } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { Store } from '@ngxs/store';

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
    this.selected = /** @type {?} */ ({});
    this.trackByFn
    /**
     * @param {?} _
     * @param {?} item
     * @return {?}
     */ = (_, item) => item.name;
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
    if (this.settings.length) {
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
  SettingManagementComponent.prototype.selected;
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
        imports: [SettingManagementRoutingModule, CoreModule, ThemeSharedModule],
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

export { SettingManagementComponent, SettingManagementModule, SettingManagementRoutingModule as ɵa };
//# sourceMappingURL=abp-ng.setting-management.js.map
