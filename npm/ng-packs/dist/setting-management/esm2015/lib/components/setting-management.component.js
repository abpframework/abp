/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
import { getSettingTabs } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { ConfigState } from '@abp/ng.core';
export class SettingManagementComponent {
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuc2V0dGluZy1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvc2V0dGluZy1tYW5hZ2VtZW50LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBMkIsTUFBTSxlQUFlLENBQUM7QUFDbkUsT0FBTyxFQUFjLGNBQWMsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ2xFLE9BQU8sRUFBRSxNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUN6QyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFNM0MsTUFBTSxPQUFPLDBCQUEwQjs7Ozs7SUFPckMsWUFBb0IsTUFBYyxFQUFVLEtBQVk7UUFBcEMsV0FBTSxHQUFOLE1BQU0sQ0FBUTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87UUFOeEQsYUFBUSxHQUFpQixFQUFFLENBQUM7UUFFNUIsYUFBUSxHQUFHLG1CQUFBLEVBQUUsRUFBYyxDQUFDO1FBRTVCLGNBQVM7Ozs7O1FBQWdDLENBQUMsQ0FBQyxFQUFFLElBQUksRUFBRSxFQUFFLENBQUMsSUFBSSxDQUFDLElBQUksRUFBQztJQUVMLENBQUM7Ozs7SUFFNUQsUUFBUTtRQUNOLElBQUksQ0FBQyxRQUFRLEdBQUcsY0FBYyxFQUFFO2FBQzdCLE1BQU07Ozs7UUFBQyxPQUFPLENBQUMsRUFBRSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxnQkFBZ0IsQ0FBQyxPQUFPLENBQUMsY0FBYyxDQUFDLENBQUMsRUFBQzthQUNsRyxJQUFJOzs7OztRQUFDLENBQUMsQ0FBQyxFQUFFLENBQUMsRUFBRSxFQUFFLENBQUMsQ0FBQyxDQUFDLEtBQUssR0FBRyxDQUFDLENBQUMsS0FBSyxFQUFDLENBQUM7UUFDckMsSUFBSSxJQUFJLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUN4QixJQUFJLENBQUMsUUFBUSxHQUFHLElBQUksQ0FBQyxRQUFRLENBQUMsQ0FBQyxDQUFDLENBQUM7U0FDbEM7SUFDSCxDQUFDOzs7WUFwQkYsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSx3QkFBd0I7Z0JBQ2xDLHdqREFBa0Q7YUFDbkQ7Ozs7WUFQUSxNQUFNO1lBQ04sS0FBSzs7OztJQVFaLDhDQUE0Qjs7SUFFNUIsOENBQTRCOztJQUU1QiwrQ0FBZ0U7Ozs7O0lBRXBELDRDQUFzQjs7Ozs7SUFBRSwyQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIFRyYWNrQnlGdW5jdGlvbiwgT25Jbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTZXR0aW5nVGFiLCBnZXRTZXR0aW5nVGFicyB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IFJvdXRlciB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXNldHRpbmctbWFuYWdlbWVudCcsXG4gIHRlbXBsYXRlVXJsOiAnLi9zZXR0aW5nLW1hbmFnZW1lbnQuY29tcG9uZW50Lmh0bWwnLFxufSlcbmV4cG9ydCBjbGFzcyBTZXR0aW5nTWFuYWdlbWVudENvbXBvbmVudCBpbXBsZW1lbnRzIE9uSW5pdCB7XG4gIHNldHRpbmdzOiBTZXR0aW5nVGFiW10gPSBbXTtcblxuICBzZWxlY3RlZCA9IHt9IGFzIFNldHRpbmdUYWI7XG5cbiAgdHJhY2tCeUZuOiBUcmFja0J5RnVuY3Rpb248U2V0dGluZ1RhYj4gPSAoXywgaXRlbSkgPT4gaXRlbS5uYW1lO1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcm91dGVyOiBSb3V0ZXIsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIG5nT25Jbml0KCkge1xuICAgIHRoaXMuc2V0dGluZ3MgPSBnZXRTZXR0aW5nVGFicygpXG4gICAgICAuZmlsdGVyKHNldHRpbmcgPT4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRHcmFudGVkUG9saWN5KHNldHRpbmcucmVxdWlyZWRQb2xpY3kpKSlcbiAgICAgIC5zb3J0KChhLCBiKSA9PiBhLm9yZGVyIC0gYi5vcmRlcik7XG4gICAgaWYgKHRoaXMuc2V0dGluZ3MubGVuZ3RoKSB7XG4gICAgICB0aGlzLnNlbGVjdGVkID0gdGhpcy5zZXR0aW5nc1swXTtcbiAgICB9XG4gIH1cbn1cbiJdfQ==
