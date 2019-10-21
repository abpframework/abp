(function(global, factory) {
  typeof exports === 'object' && typeof module !== 'undefined'
    ? factory(
        exports,
        require('@abp/ng.core'),
        require('@abp/ng.theme.shared'),
        require('@angular/core'),
        require('@angular/router'),
        require('@ngxs/store'),
      )
    : typeof define === 'function' && define.amd
    ? define('@abp/ng.setting-management', [
        'exports',
        '@abp/ng.core',
        '@abp/ng.theme.shared',
        '@angular/core',
        '@angular/router',
        '@ngxs/store',
      ], factory)
    : ((global = global || self),
      factory(
        ((global.abp = global.abp || {}),
        (global.abp.ng = global.abp.ng || {}),
        (global.abp.ng['setting-management'] = {})),
        global.ng_core,
        global.ng_theme_shared,
        global.ng.core,
        global.ng.router,
        global.store,
      ));
})(this, function(exports, ng_core, ng_theme_shared, core, router, store) {
  'use strict';

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  var SettingManagementComponent = /** @class */ (function() {
    function SettingManagementComponent(router, store) {
      this.router = router;
      this.store = store;
      this.settings = [];
      this.selected = /** @type {?} */ ({});
      this.trackByFn
      /**
       * @param {?} _
       * @param {?} item
       * @return {?}
       */ = function(_, item) {
        return item.name;
      };
    }
    /**
     * @return {?}
     */
    SettingManagementComponent.prototype.ngOnInit
    /**
     * @return {?}
     */ = function() {
      var _this = this;
      this.settings = ng_theme_shared
        .getSettingTabs()
        .filter(
          /**
           * @param {?} setting
           * @return {?}
           */
          function(setting) {
            return _this.store.selectSnapshot(ng_core.ConfigState.getGrantedPolicy(setting.requiredPolicy));
          },
        )
        .sort(
          /**
           * @param {?} a
           * @param {?} b
           * @return {?}
           */
          function(a, b) {
            return a.order - b.order;
          },
        );
      if (this.settings.length) {
        this.selected = this.settings[0];
      }
    };
    SettingManagementComponent.decorators = [
      {
        type: core.Component,
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
    SettingManagementComponent.ctorParameters = function() {
      return [{ type: router.Router }, { type: store.Store }];
    };
    return SettingManagementComponent;
  })();
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
  var routes = [
    {
      path: '',
      component: ng_core.DynamicLayoutComponent,
      children: [{ path: '', component: SettingManagementComponent }],
    },
  ];
  var SettingManagementRoutingModule = /** @class */ (function() {
    function SettingManagementRoutingModule() {}
    SettingManagementRoutingModule.decorators = [
      {
        type: core.NgModule,
        args: [
          {
            imports: [router.RouterModule.forChild(routes)],
            exports: [router.RouterModule],
          },
        ],
      },
    ];
    return SettingManagementRoutingModule;
  })();

  /**
   * @fileoverview added by tsickle
   * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
   */
  var SettingManagementModule = /** @class */ (function() {
    function SettingManagementModule() {}
    SettingManagementModule.decorators = [
      {
        type: core.NgModule,
        args: [
          {
            declarations: [SettingManagementComponent],
            imports: [SettingManagementRoutingModule, ng_core.CoreModule, ng_theme_shared.ThemeSharedModule],
          },
        ],
      },
    ];
    return SettingManagementModule;
  })();

  exports.SettingManagementComponent = SettingManagementComponent;
  exports.SettingManagementModule = SettingManagementModule;
  exports.ɵa = SettingManagementRoutingModule;

  Object.defineProperty(exports, '__esModule', { value: true });
});
//# sourceMappingURL=abp-ng.setting-management.umd.js.map
