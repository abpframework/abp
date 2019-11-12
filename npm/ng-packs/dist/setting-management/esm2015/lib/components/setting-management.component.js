/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
import { getSettingTabs } from '@abp/ng.theme.shared';
import { Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { ConfigState } from '@abp/ng.core';
import { SettingManagementState } from '../states/setting-management.state';
import { SetSelectedSettingTab } from '../actions/setting-management.actions';
export class SettingManagementComponent {
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuc2V0dGluZy1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvc2V0dGluZy1tYW5hZ2VtZW50LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFNBQVMsRUFBMkIsTUFBTSxlQUFlLENBQUM7QUFDbkUsT0FBTyxFQUFjLGNBQWMsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ2xFLE9BQU8sRUFBRSxNQUFNLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUN6QyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxjQUFjLENBQUM7QUFDM0MsT0FBTyxFQUFFLHNCQUFzQixFQUFFLE1BQU0sb0NBQW9DLENBQUM7QUFDNUUsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sdUNBQXVDLENBQUM7QUFPOUUsTUFBTSxPQUFPLDBCQUEwQjs7Ozs7SUFrQnJDLFlBQW9CLE1BQWMsRUFBVSxLQUFZO1FBQXBDLFdBQU0sR0FBTixNQUFNLENBQVE7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBakJ4RCxhQUFRLEdBQWlCLEVBQUUsQ0FBQztRQWU1QixjQUFTOzs7OztRQUFnQyxDQUFDLENBQUMsRUFBRSxJQUFJLEVBQUUsRUFBRSxDQUFDLElBQUksQ0FBQyxJQUFJLEVBQUM7SUFFTCxDQUFDOzs7OztJQWY1RCxJQUFJLFFBQVEsQ0FBQyxLQUFpQjtRQUM1QixJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLHFCQUFxQixDQUFDLEtBQUssQ0FBQyxDQUFDLENBQUM7SUFDeEQsQ0FBQzs7OztJQUNELElBQUksUUFBUTs7Y0FDSixLQUFLLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsc0JBQXNCLENBQUMsY0FBYyxDQUFDO1FBRTlFLElBQUksQ0FBQyxDQUFDLEtBQUssSUFBSSxDQUFDLEtBQUssQ0FBQyxTQUFTLENBQUMsSUFBSSxJQUFJLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUN4RCxPQUFPLElBQUksQ0FBQyxRQUFRLENBQUMsQ0FBQyxDQUFDLENBQUM7U0FDekI7UUFFRCxPQUFPLEtBQUssQ0FBQztJQUNmLENBQUM7Ozs7SUFNRCxRQUFRO1FBQ04sSUFBSSxDQUFDLFFBQVEsR0FBRyxjQUFjLEVBQUU7YUFDN0IsTUFBTTs7OztRQUFDLE9BQU8sQ0FBQyxFQUFFLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLGdCQUFnQixDQUFDLE9BQU8sQ0FBQyxjQUFjLENBQUMsQ0FBQyxFQUFDO2FBQ2xHLElBQUk7Ozs7O1FBQUMsQ0FBQyxDQUFDLEVBQUUsQ0FBQyxFQUFFLEVBQUUsQ0FBQyxDQUFDLENBQUMsS0FBSyxHQUFHLENBQUMsQ0FBQyxLQUFLLEVBQUMsQ0FBQztRQUVyQyxJQUFJLENBQUMsSUFBSSxDQUFDLFFBQVEsSUFBSSxJQUFJLENBQUMsUUFBUSxDQUFDLE1BQU0sRUFBRTtZQUMxQyxJQUFJLENBQUMsUUFBUSxHQUFHLElBQUksQ0FBQyxRQUFRLENBQUMsQ0FBQyxDQUFDLENBQUM7U0FDbEM7SUFDSCxDQUFDOzs7WUFoQ0YsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSx3QkFBd0I7Z0JBQ2xDLHdqREFBa0Q7YUFDbkQ7Ozs7WUFWUSxNQUFNO1lBQ04sS0FBSzs7OztJQVdaLDhDQUE0Qjs7SUFlNUIsK0NBQWdFOzs7OztJQUVwRCw0Q0FBc0I7Ozs7O0lBQUUsMkNBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBUcmFja0J5RnVuY3Rpb24sIE9uSW5pdCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU2V0dGluZ1RhYiwgZ2V0U2V0dGluZ1RhYnMgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyBSb3V0ZXIgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBDb25maWdTdGF0ZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBTZXR0aW5nTWFuYWdlbWVudFN0YXRlIH0gZnJvbSAnLi4vc3RhdGVzL3NldHRpbmctbWFuYWdlbWVudC5zdGF0ZSc7XG5pbXBvcnQgeyBTZXRTZWxlY3RlZFNldHRpbmdUYWIgfSBmcm9tICcuLi9hY3Rpb25zL3NldHRpbmctbWFuYWdlbWVudC5hY3Rpb25zJztcbmltcG9ydCB7IFJvdXRlclN0YXRlIH0gZnJvbSAnQG5neHMvcm91dGVyLXBsdWdpbic7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1zZXR0aW5nLW1hbmFnZW1lbnQnLFxuICB0ZW1wbGF0ZVVybDogJy4vc2V0dGluZy1tYW5hZ2VtZW50LmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgU2V0dGluZ01hbmFnZW1lbnRDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xuICBzZXR0aW5nczogU2V0dGluZ1RhYltdID0gW107XG5cbiAgc2V0IHNlbGVjdGVkKHZhbHVlOiBTZXR0aW5nVGFiKSB7XG4gICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgU2V0U2VsZWN0ZWRTZXR0aW5nVGFiKHZhbHVlKSk7XG4gIH1cbiAgZ2V0IHNlbGVjdGVkKCk6IFNldHRpbmdUYWIge1xuICAgIGNvbnN0IHZhbHVlID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChTZXR0aW5nTWFuYWdlbWVudFN0YXRlLmdldFNlbGVjdGVkVGFiKTtcblxuICAgIGlmICgoIXZhbHVlIHx8ICF2YWx1ZS5jb21wb25lbnQpICYmIHRoaXMuc2V0dGluZ3MubGVuZ3RoKSB7XG4gICAgICByZXR1cm4gdGhpcy5zZXR0aW5nc1swXTtcbiAgICB9XG5cbiAgICByZXR1cm4gdmFsdWU7XG4gIH1cblxuICB0cmFja0J5Rm46IFRyYWNrQnlGdW5jdGlvbjxTZXR0aW5nVGFiPiA9IChfLCBpdGVtKSA9PiBpdGVtLm5hbWU7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSByb3V0ZXI6IFJvdXRlciwgcHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgbmdPbkluaXQoKSB7XG4gICAgdGhpcy5zZXR0aW5ncyA9IGdldFNldHRpbmdUYWJzKClcbiAgICAgIC5maWx0ZXIoc2V0dGluZyA9PiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldEdyYW50ZWRQb2xpY3koc2V0dGluZy5yZXF1aXJlZFBvbGljeSkpKVxuICAgICAgLnNvcnQoKGEsIGIpID0+IGEub3JkZXIgLSBiLm9yZGVyKTtcblxuICAgIGlmICghdGhpcy5zZWxlY3RlZCAmJiB0aGlzLnNldHRpbmdzLmxlbmd0aCkge1xuICAgICAgdGhpcy5zZWxlY3RlZCA9IHRoaXMuc2V0dGluZ3NbMF07XG4gICAgfVxuICB9XG59XG4iXX0=
