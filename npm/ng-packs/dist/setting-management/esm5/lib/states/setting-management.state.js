/**
 * @fileoverview added by tsickle
 * Generated from: lib/states/setting-management.state.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Action, Selector, State } from '@ngxs/store';
import { SetSelectedSettingTab } from '../actions/setting-management.actions';
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
    return SettingManagementState;
}());
export { SettingManagementState };
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5zZXR0aW5nLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL3NldHRpbmctbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLFFBQVEsRUFBRSxLQUFLLEVBQWdCLE1BQU0sYUFBYSxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLHVDQUF1QyxDQUFDOzs7SUFtQjlFLENBQUM7Ozs7O0lBVlEscUNBQWM7Ozs7SUFBckIsVUFBc0IsRUFBd0M7WUFBdEMsNEJBQVc7UUFDakMsT0FBTyxXQUFXLENBQUM7SUFDckIsQ0FBQzs7Ozs7O0lBR0Qsd0RBQXVCOzs7OztJQUF2QixVQUF3QixFQUFxRCxFQUFFLEVBQWtDO1lBQXZGLDBCQUFVO1lBQTZDLG9CQUFPO1FBQ3RGLFVBQVUsQ0FBQztZQUNULFdBQVcsRUFBRSxPQUFPO1NBQ3JCLENBQUMsQ0FBQztJQUNMLENBQUM7SUFKRDtRQURDLE1BQU0sQ0FBQyxxQkFBcUIsQ0FBQzs7eURBQzhELHFCQUFxQjs7eUVBSWhIO0lBVEQ7UUFEQyxRQUFRLEVBQUU7Ozs7c0RBR1Y7SUFKVSxzQkFBc0I7UUFKbEMsS0FBSyxDQUEwQjtZQUM5QixJQUFJLEVBQUUsd0JBQXdCO1lBQzlCLFFBQVEsRUFBRSxtQkFBQSxFQUFFLFdBQVcsRUFBRSxFQUFFLEVBQUUsRUFBMkI7U0FDekQsQ0FBQztPQUNXLHNCQUFzQixDQVlsQztJQUFELDZCQUFDO0NBQUEsSUFBQTtTQVpZLHNCQUFzQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFjdGlvbiwgU2VsZWN0b3IsIFN0YXRlLCBTdGF0ZUNvbnRleHQgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBTZXRTZWxlY3RlZFNldHRpbmdUYWIgfSBmcm9tICcuLi9hY3Rpb25zL3NldHRpbmctbWFuYWdlbWVudC5hY3Rpb25zJztcbmltcG9ydCB7IFNldHRpbmdNYW5hZ2VtZW50IH0gZnJvbSAnLi4vbW9kZWxzL3NldHRpbmctbWFuYWdlbWVudCc7XG5cbkBTdGF0ZTxTZXR0aW5nTWFuYWdlbWVudC5TdGF0ZT4oe1xuICBuYW1lOiAnU2V0dGluZ01hbmFnZW1lbnRTdGF0ZScsXG4gIGRlZmF1bHRzOiB7IHNlbGVjdGVkVGFiOiB7fSB9IGFzIFNldHRpbmdNYW5hZ2VtZW50LlN0YXRlLFxufSlcbmV4cG9ydCBjbGFzcyBTZXR0aW5nTWFuYWdlbWVudFN0YXRlIHtcbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldFNlbGVjdGVkVGFiKHsgc2VsZWN0ZWRUYWIgfTogU2V0dGluZ01hbmFnZW1lbnQuU3RhdGUpIHtcbiAgICByZXR1cm4gc2VsZWN0ZWRUYWI7XG4gIH1cblxuICBAQWN0aW9uKFNldFNlbGVjdGVkU2V0dGluZ1RhYilcbiAgc2V0dGluZ01hbmFnZW1lbnRBY3Rpb24oeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxTZXR0aW5nTWFuYWdlbWVudC5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBTZXRTZWxlY3RlZFNldHRpbmdUYWIpIHtcbiAgICBwYXRjaFN0YXRlKHtcbiAgICAgIHNlbGVjdGVkVGFiOiBwYXlsb2FkLFxuICAgIH0pO1xuICB9XG59XG4iXX0=