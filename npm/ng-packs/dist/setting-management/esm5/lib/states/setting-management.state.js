/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import { Action, Selector, State } from '@ngxs/store';
import { SetSelectedSettingTab } from '../actions/setting-management.actions';
var SettingManagementState = /** @class */ (function() {
  function SettingManagementState() {}
  /**
   * @param {?} __0
   * @return {?}
   */
  SettingManagementState.getSelectedTab
  /**
   * @param {?} __0
   * @return {?}
   */ = function(_a) {
    var selectedTab = _a.selectedTab;
    return selectedTab;
  };
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  SettingManagementState.prototype.settingManagementAction
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */ = function(_a, _b) {
    var patchState = _a.patchState;
    var payload = _b.payload;
    patchState({
      selectedTab: payload,
    });
  };
  tslib_1.__decorate(
    [
      Action(SetSelectedSettingTab),
      tslib_1.__metadata('design:type', Function),
      tslib_1.__metadata('design:paramtypes', [Object, SetSelectedSettingTab]),
      tslib_1.__metadata('design:returntype', void 0),
    ],
    SettingManagementState.prototype,
    'settingManagementAction',
    null,
  );
  tslib_1.__decorate(
    [
      Selector(),
      tslib_1.__metadata('design:type', Function),
      tslib_1.__metadata('design:paramtypes', [Object]),
      tslib_1.__metadata('design:returntype', void 0),
    ],
    SettingManagementState,
    'getSelectedTab',
    null,
  );
  SettingManagementState = tslib_1.__decorate(
    [
      State({
        name: 'SettingManagementState',
        defaults: /** @type {?} */ ({ selectedTab: {} }),
      }),
    ],
    SettingManagementState,
  );
  return SettingManagementState;
})();
export { SettingManagementState };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5zZXR0aW5nLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL3NldHRpbmctbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sdUNBQXVDLENBQUM7OztJQW1COUUsQ0FBQzs7Ozs7SUFWUSxxQ0FBYzs7OztJQUFyQixVQUFzQixFQUF3QztZQUF0Qyw0QkFBVztRQUNqQyxPQUFPLFdBQVcsQ0FBQztJQUNyQixDQUFDOzs7Ozs7SUFHRCx3REFBdUI7Ozs7O0lBQXZCLFVBQXdCLEVBQXFELEVBQUUsRUFBa0M7WUFBdkYsMEJBQVU7WUFBNkMsb0JBQU87UUFDdEYsVUFBVSxDQUFDO1lBQ1QsV0FBVyxFQUFFLE9BQU87U0FDckIsQ0FBQyxDQUFDO0lBQ0wsQ0FBQztJQUpEO1FBREMsTUFBTSxDQUFDLHFCQUFxQixDQUFDOzt5REFDOEQscUJBQXFCOzt5RUFJaEg7SUFURDtRQURDLFFBQVEsRUFBRTs7OztzREFHVjtJQUpVLHNCQUFzQjtRQUpsQyxLQUFLLENBQTBCO1lBQzlCLElBQUksRUFBRSx3QkFBd0I7WUFDOUIsUUFBUSxFQUFFLG1CQUFBLEVBQUUsV0FBVyxFQUFFLEVBQUUsRUFBRSxFQUEyQjtTQUN6RCxDQUFDO09BQ1csc0JBQXNCLENBWWxDO0lBQUQsNkJBQUM7Q0FBQSxJQUFBO1NBWlksc0JBQXNCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQWN0aW9uLCBTZWxlY3RvciwgU3RhdGUsIFN0YXRlQ29udGV4dCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IFNldFNlbGVjdGVkU2V0dGluZ1RhYiB9IGZyb20gJy4uL2FjdGlvbnMvc2V0dGluZy1tYW5hZ2VtZW50LmFjdGlvbnMnO1xuaW1wb3J0IHsgU2V0dGluZ01hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMvc2V0dGluZy1tYW5hZ2VtZW50JztcblxuQFN0YXRlPFNldHRpbmdNYW5hZ2VtZW50LlN0YXRlPih7XG4gIG5hbWU6ICdTZXR0aW5nTWFuYWdlbWVudFN0YXRlJyxcbiAgZGVmYXVsdHM6IHsgc2VsZWN0ZWRUYWI6IHt9IH0gYXMgU2V0dGluZ01hbmFnZW1lbnQuU3RhdGUsXG59KVxuZXhwb3J0IGNsYXNzIFNldHRpbmdNYW5hZ2VtZW50U3RhdGUge1xuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0U2VsZWN0ZWRUYWIoeyBzZWxlY3RlZFRhYiB9OiBTZXR0aW5nTWFuYWdlbWVudC5TdGF0ZSkge1xuICAgIHJldHVybiBzZWxlY3RlZFRhYjtcbiAgfVxuXG4gIEBBY3Rpb24oU2V0U2VsZWN0ZWRTZXR0aW5nVGFiKVxuICBzZXR0aW5nTWFuYWdlbWVudEFjdGlvbih7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFNldHRpbmdNYW5hZ2VtZW50LlN0YXRlPiwgeyBwYXlsb2FkIH06IFNldFNlbGVjdGVkU2V0dGluZ1RhYikge1xuICAgIHBhdGNoU3RhdGUoe1xuICAgICAgc2VsZWN0ZWRUYWI6IHBheWxvYWQsXG4gICAgfSk7XG4gIH1cbn1cbiJdfQ==
