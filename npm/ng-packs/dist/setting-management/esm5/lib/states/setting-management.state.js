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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5zZXR0aW5nLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL3NldHRpbmctbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLFFBQVEsRUFBRSxLQUFLLEVBQWdCLE1BQU0sYUFBYSxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLHVDQUF1QyxDQUFDOzs7SUFtQjlFLENBQUM7Ozs7O0lBVlEscUNBQWM7Ozs7SUFBckIsVUFBc0IsRUFBd0M7WUFBdEMsNEJBQVc7UUFDakMsT0FBTyxXQUFXLENBQUM7SUFDckIsQ0FBQzs7Ozs7O0lBR0Qsd0RBQXVCOzs7OztJQUF2QixVQUF3QixFQUFxRCxFQUFFLEVBQWtDO1lBQXZGLDBCQUFVO1lBQTZDLG9CQUFPO1FBQ3RGLFVBQVUsQ0FBQztZQUNULFdBQVcsRUFBRSxPQUFPO1NBQ3JCLENBQUMsQ0FBQztJQUNMLENBQUM7SUFKRDtRQURDLE1BQU0sQ0FBQyxxQkFBcUIsQ0FBQzs7eURBQzhELHFCQUFxQjs7eUVBSWhIO0lBVEQ7UUFEQyxRQUFRLEVBQUU7Ozs7c0RBR1Y7SUFKVSxzQkFBc0I7UUFKbEMsS0FBSyxDQUEwQjtZQUM5QixJQUFJLEVBQUUsd0JBQXdCO1lBQzlCLFFBQVEsRUFBRSxtQkFBQSxFQUFFLFdBQVcsRUFBRSxFQUFFLEVBQUUsRUFBMkI7U0FDekQsQ0FBQztPQUNXLHNCQUFzQixDQVlsQztJQUFELDZCQUFDO0NBQUEsSUFBQTtTQVpZLHNCQUFzQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFjdGlvbiwgU2VsZWN0b3IsIFN0YXRlLCBTdGF0ZUNvbnRleHQgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IFNldFNlbGVjdGVkU2V0dGluZ1RhYiB9IGZyb20gJy4uL2FjdGlvbnMvc2V0dGluZy1tYW5hZ2VtZW50LmFjdGlvbnMnO1xyXG5pbXBvcnQgeyBTZXR0aW5nTWFuYWdlbWVudCB9IGZyb20gJy4uL21vZGVscy9zZXR0aW5nLW1hbmFnZW1lbnQnO1xyXG5cclxuQFN0YXRlPFNldHRpbmdNYW5hZ2VtZW50LlN0YXRlPih7XHJcbiAgbmFtZTogJ1NldHRpbmdNYW5hZ2VtZW50U3RhdGUnLFxyXG4gIGRlZmF1bHRzOiB7IHNlbGVjdGVkVGFiOiB7fSB9IGFzIFNldHRpbmdNYW5hZ2VtZW50LlN0YXRlLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgU2V0dGluZ01hbmFnZW1lbnRTdGF0ZSB7XHJcbiAgQFNlbGVjdG9yKClcclxuICBzdGF0aWMgZ2V0U2VsZWN0ZWRUYWIoeyBzZWxlY3RlZFRhYiB9OiBTZXR0aW5nTWFuYWdlbWVudC5TdGF0ZSkge1xyXG4gICAgcmV0dXJuIHNlbGVjdGVkVGFiO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihTZXRTZWxlY3RlZFNldHRpbmdUYWIpXHJcbiAgc2V0dGluZ01hbmFnZW1lbnRBY3Rpb24oeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxTZXR0aW5nTWFuYWdlbWVudC5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBTZXRTZWxlY3RlZFNldHRpbmdUYWIpIHtcclxuICAgIHBhdGNoU3RhdGUoe1xyXG4gICAgICBzZWxlY3RlZFRhYjogcGF5bG9hZCxcclxuICAgIH0pO1xyXG4gIH1cclxufVxyXG4iXX0=