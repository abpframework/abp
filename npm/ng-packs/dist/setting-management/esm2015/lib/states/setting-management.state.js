/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import { Action, Selector, State } from '@ngxs/store';
import { SetSelectedSettingTab } from '../actions/setting-management.actions';
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
export { SettingManagementState };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5zZXR0aW5nLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL3NldHRpbmctbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sdUNBQXVDLENBQUM7SUFPakUsc0JBQXNCLFNBQXRCLHNCQUFzQjs7Ozs7SUFFakMsTUFBTSxDQUFDLGNBQWMsQ0FBQyxFQUFFLFdBQVcsRUFBMkI7UUFDNUQsT0FBTyxXQUFXLENBQUM7SUFDckIsQ0FBQzs7Ozs7O0lBR0QsdUJBQXVCLENBQUMsRUFBRSxVQUFVLEVBQXlDLEVBQUUsRUFBRSxPQUFPLEVBQXlCO1FBQy9HLFVBQVUsQ0FBQztZQUNULFdBQVcsRUFBRSxPQUFPO1NBQ3JCLENBQUMsQ0FBQztJQUNMLENBQUM7Q0FDRixDQUFBO0FBTEM7SUFEQyxNQUFNLENBQUMscUJBQXFCLENBQUM7O3FEQUM4RCxxQkFBcUI7O3FFQUloSDtBQVREO0lBREMsUUFBUSxFQUFFOzs7O2tEQUdWO0FBSlUsc0JBQXNCO0lBSmxDLEtBQUssQ0FBMEI7UUFDOUIsSUFBSSxFQUFFLHdCQUF3QjtRQUM5QixRQUFRLEVBQUUsbUJBQUEsRUFBRSxXQUFXLEVBQUUsRUFBRSxFQUFFLEVBQTJCO0tBQ3pELENBQUM7R0FDVyxzQkFBc0IsQ0FZbEM7U0FaWSxzQkFBc0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBY3Rpb24sIFNlbGVjdG9yLCBTdGF0ZSwgU3RhdGVDb250ZXh0IH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgU2V0U2VsZWN0ZWRTZXR0aW5nVGFiIH0gZnJvbSAnLi4vYWN0aW9ucy9zZXR0aW5nLW1hbmFnZW1lbnQuYWN0aW9ucyc7XG5pbXBvcnQgeyBTZXR0aW5nTWFuYWdlbWVudCB9IGZyb20gJy4uL21vZGVscy9zZXR0aW5nLW1hbmFnZW1lbnQnO1xuXG5AU3RhdGU8U2V0dGluZ01hbmFnZW1lbnQuU3RhdGU+KHtcbiAgbmFtZTogJ1NldHRpbmdNYW5hZ2VtZW50U3RhdGUnLFxuICBkZWZhdWx0czogeyBzZWxlY3RlZFRhYjoge30gfSBhcyBTZXR0aW5nTWFuYWdlbWVudC5TdGF0ZSxcbn0pXG5leHBvcnQgY2xhc3MgU2V0dGluZ01hbmFnZW1lbnRTdGF0ZSB7XG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRTZWxlY3RlZFRhYih7IHNlbGVjdGVkVGFiIH06IFNldHRpbmdNYW5hZ2VtZW50LlN0YXRlKSB7XG4gICAgcmV0dXJuIHNlbGVjdGVkVGFiO1xuICB9XG5cbiAgQEFjdGlvbihTZXRTZWxlY3RlZFNldHRpbmdUYWIpXG4gIHNldHRpbmdNYW5hZ2VtZW50QWN0aW9uKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8U2V0dGluZ01hbmFnZW1lbnQuU3RhdGU+LCB7IHBheWxvYWQgfTogU2V0U2VsZWN0ZWRTZXR0aW5nVGFiKSB7XG4gICAgcGF0Y2hTdGF0ZSh7XG4gICAgICBzZWxlY3RlZFRhYjogcGF5bG9hZCxcbiAgICB9KTtcbiAgfVxufVxuIl19
