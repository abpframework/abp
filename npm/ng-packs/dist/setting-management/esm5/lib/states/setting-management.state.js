/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5zZXR0aW5nLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL3NldHRpbmctbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sdUNBQXVDLENBQUM7OztJQW1COUUsQ0FBQzs7Ozs7SUFWUSxxQ0FBYzs7OztJQUFyQixVQUFzQixFQUF3QztZQUF0Qyw0QkFBVztRQUNqQyxPQUFPLFdBQVcsQ0FBQztJQUNyQixDQUFDOzs7Ozs7SUFHRCx3REFBdUI7Ozs7O0lBQXZCLFVBQXdCLEVBQXFELEVBQUUsRUFBa0M7WUFBdkYsMEJBQVU7WUFBNkMsb0JBQU87UUFDdEYsVUFBVSxDQUFDO1lBQ1QsV0FBVyxFQUFFLE9BQU87U0FDckIsQ0FBQyxDQUFDO0lBQ0wsQ0FBQztJQUpEO1FBREMsTUFBTSxDQUFDLHFCQUFxQixDQUFDOzt5REFDOEQscUJBQXFCOzt5RUFJaEg7SUFURDtRQURDLFFBQVEsRUFBRTs7OztzREFHVjtJQUpVLHNCQUFzQjtRQUpsQyxLQUFLLENBQTBCO1lBQzlCLElBQUksRUFBRSx3QkFBd0I7WUFDOUIsUUFBUSxFQUFFLG1CQUFBLEVBQUUsV0FBVyxFQUFFLEVBQUUsRUFBRSxFQUEyQjtTQUN6RCxDQUFDO09BQ1csc0JBQXNCLENBWWxDO0lBQUQsNkJBQUM7Q0FBQSxJQUFBO1NBWlksc0JBQXNCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQWN0aW9uLCBTZWxlY3RvciwgU3RhdGUsIFN0YXRlQ29udGV4dCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgU2V0U2VsZWN0ZWRTZXR0aW5nVGFiIH0gZnJvbSAnLi4vYWN0aW9ucy9zZXR0aW5nLW1hbmFnZW1lbnQuYWN0aW9ucyc7XHJcbmltcG9ydCB7IFNldHRpbmdNYW5hZ2VtZW50IH0gZnJvbSAnLi4vbW9kZWxzL3NldHRpbmctbWFuYWdlbWVudCc7XHJcblxyXG5AU3RhdGU8U2V0dGluZ01hbmFnZW1lbnQuU3RhdGU+KHtcclxuICBuYW1lOiAnU2V0dGluZ01hbmFnZW1lbnRTdGF0ZScsXHJcbiAgZGVmYXVsdHM6IHsgc2VsZWN0ZWRUYWI6IHt9IH0gYXMgU2V0dGluZ01hbmFnZW1lbnQuU3RhdGUsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBTZXR0aW5nTWFuYWdlbWVudFN0YXRlIHtcclxuICBAU2VsZWN0b3IoKVxyXG4gIHN0YXRpYyBnZXRTZWxlY3RlZFRhYih7IHNlbGVjdGVkVGFiIH06IFNldHRpbmdNYW5hZ2VtZW50LlN0YXRlKSB7XHJcbiAgICByZXR1cm4gc2VsZWN0ZWRUYWI7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKFNldFNlbGVjdGVkU2V0dGluZ1RhYilcclxuICBzZXR0aW5nTWFuYWdlbWVudEFjdGlvbih7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFNldHRpbmdNYW5hZ2VtZW50LlN0YXRlPiwgeyBwYXlsb2FkIH06IFNldFNlbGVjdGVkU2V0dGluZ1RhYikge1xyXG4gICAgcGF0Y2hTdGF0ZSh7XHJcbiAgICAgIHNlbGVjdGVkVGFiOiBwYXlsb2FkLFxyXG4gICAgfSk7XHJcbiAgfVxyXG59XHJcbiJdfQ==