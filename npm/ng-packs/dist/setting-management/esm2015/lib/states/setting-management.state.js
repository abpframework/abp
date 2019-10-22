/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
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
tslib_1.__decorate([
    Action(SetSelectedSettingTab),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, SetSelectedSettingTab]),
    tslib_1.__metadata("design:returntype", void 0)
], SettingManagementState.prototype, "settingManagementAction", null);
tslib_1.__decorate([
    Selector(),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object]),
    tslib_1.__metadata("design:returntype", void 0)
], SettingManagementState, "getSelectedTab", null);
SettingManagementState = tslib_1.__decorate([
    State({
        name: 'SettingManagementState',
        defaults: (/** @type {?} */ ({ selectedTab: {} })),
    })
], SettingManagementState);
export { SettingManagementState };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5zZXR0aW5nLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL3NldHRpbmctbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sdUNBQXVDLENBQUM7SUFPakUsc0JBQXNCLFNBQXRCLHNCQUFzQjs7Ozs7SUFFakMsTUFBTSxDQUFDLGNBQWMsQ0FBQyxFQUFFLFdBQVcsRUFBMkI7UUFDNUQsT0FBTyxXQUFXLENBQUM7SUFDckIsQ0FBQzs7Ozs7O0lBR0QsdUJBQXVCLENBQUMsRUFBRSxVQUFVLEVBQXlDLEVBQUUsRUFBRSxPQUFPLEVBQXlCO1FBQy9HLFVBQVUsQ0FBQztZQUNULFdBQVcsRUFBRSxPQUFPO1NBQ3JCLENBQUMsQ0FBQztJQUNMLENBQUM7Q0FDRixDQUFBO0FBTEM7SUFEQyxNQUFNLENBQUMscUJBQXFCLENBQUM7O3FEQUM4RCxxQkFBcUI7O3FFQUloSDtBQVREO0lBREMsUUFBUSxFQUFFOzs7O2tEQUdWO0FBSlUsc0JBQXNCO0lBSmxDLEtBQUssQ0FBMEI7UUFDOUIsSUFBSSxFQUFFLHdCQUF3QjtRQUM5QixRQUFRLEVBQUUsbUJBQUEsRUFBRSxXQUFXLEVBQUUsRUFBRSxFQUFFLEVBQTJCO0tBQ3pELENBQUM7R0FDVyxzQkFBc0IsQ0FZbEM7U0FaWSxzQkFBc0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBY3Rpb24sIFNlbGVjdG9yLCBTdGF0ZSwgU3RhdGVDb250ZXh0IH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBTZXRTZWxlY3RlZFNldHRpbmdUYWIgfSBmcm9tICcuLi9hY3Rpb25zL3NldHRpbmctbWFuYWdlbWVudC5hY3Rpb25zJztcclxuaW1wb3J0IHsgU2V0dGluZ01hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMvc2V0dGluZy1tYW5hZ2VtZW50JztcclxuXHJcbkBTdGF0ZTxTZXR0aW5nTWFuYWdlbWVudC5TdGF0ZT4oe1xyXG4gIG5hbWU6ICdTZXR0aW5nTWFuYWdlbWVudFN0YXRlJyxcclxuICBkZWZhdWx0czogeyBzZWxlY3RlZFRhYjoge30gfSBhcyBTZXR0aW5nTWFuYWdlbWVudC5TdGF0ZSxcclxufSlcclxuZXhwb3J0IGNsYXNzIFNldHRpbmdNYW5hZ2VtZW50U3RhdGUge1xyXG4gIEBTZWxlY3RvcigpXHJcbiAgc3RhdGljIGdldFNlbGVjdGVkVGFiKHsgc2VsZWN0ZWRUYWIgfTogU2V0dGluZ01hbmFnZW1lbnQuU3RhdGUpIHtcclxuICAgIHJldHVybiBzZWxlY3RlZFRhYjtcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oU2V0U2VsZWN0ZWRTZXR0aW5nVGFiKVxyXG4gIHNldHRpbmdNYW5hZ2VtZW50QWN0aW9uKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8U2V0dGluZ01hbmFnZW1lbnQuU3RhdGU+LCB7IHBheWxvYWQgfTogU2V0U2VsZWN0ZWRTZXR0aW5nVGFiKSB7XHJcbiAgICBwYXRjaFN0YXRlKHtcclxuICAgICAgc2VsZWN0ZWRUYWI6IHBheWxvYWQsXHJcbiAgICB9KTtcclxuICB9XHJcbn1cclxuIl19