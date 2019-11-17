/**
 * @fileoverview added by tsickle
 * Generated from: lib/states/setting-management.state.ts
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5zZXR0aW5nLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL3NldHRpbmctbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLFFBQVEsRUFBRSxLQUFLLEVBQWdCLE1BQU0sYUFBYSxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLHVDQUF1QyxDQUFDO0lBT2pFLHNCQUFzQixTQUF0QixzQkFBc0I7Ozs7O0lBRWpDLE1BQU0sQ0FBQyxjQUFjLENBQUMsRUFBRSxXQUFXLEVBQTJCO1FBQzVELE9BQU8sV0FBVyxDQUFDO0lBQ3JCLENBQUM7Ozs7OztJQUdELHVCQUF1QixDQUFDLEVBQUUsVUFBVSxFQUF5QyxFQUFFLEVBQUUsT0FBTyxFQUF5QjtRQUMvRyxVQUFVLENBQUM7WUFDVCxXQUFXLEVBQUUsT0FBTztTQUNyQixDQUFDLENBQUM7SUFDTCxDQUFDO0NBQ0YsQ0FBQTtBQUxDO0lBREMsTUFBTSxDQUFDLHFCQUFxQixDQUFDOztxREFDOEQscUJBQXFCOztxRUFJaEg7QUFURDtJQURDLFFBQVEsRUFBRTs7OztrREFHVjtBQUpVLHNCQUFzQjtJQUpsQyxLQUFLLENBQTBCO1FBQzlCLElBQUksRUFBRSx3QkFBd0I7UUFDOUIsUUFBUSxFQUFFLG1CQUFBLEVBQUUsV0FBVyxFQUFFLEVBQUUsRUFBRSxFQUEyQjtLQUN6RCxDQUFDO0dBQ1csc0JBQXNCLENBWWxDO1NBWlksc0JBQXNCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQWN0aW9uLCBTZWxlY3RvciwgU3RhdGUsIFN0YXRlQ29udGV4dCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgU2V0U2VsZWN0ZWRTZXR0aW5nVGFiIH0gZnJvbSAnLi4vYWN0aW9ucy9zZXR0aW5nLW1hbmFnZW1lbnQuYWN0aW9ucyc7XHJcbmltcG9ydCB7IFNldHRpbmdNYW5hZ2VtZW50IH0gZnJvbSAnLi4vbW9kZWxzL3NldHRpbmctbWFuYWdlbWVudCc7XHJcblxyXG5AU3RhdGU8U2V0dGluZ01hbmFnZW1lbnQuU3RhdGU+KHtcclxuICBuYW1lOiAnU2V0dGluZ01hbmFnZW1lbnRTdGF0ZScsXHJcbiAgZGVmYXVsdHM6IHsgc2VsZWN0ZWRUYWI6IHt9IH0gYXMgU2V0dGluZ01hbmFnZW1lbnQuU3RhdGUsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBTZXR0aW5nTWFuYWdlbWVudFN0YXRlIHtcclxuICBAU2VsZWN0b3IoKVxyXG4gIHN0YXRpYyBnZXRTZWxlY3RlZFRhYih7IHNlbGVjdGVkVGFiIH06IFNldHRpbmdNYW5hZ2VtZW50LlN0YXRlKSB7XHJcbiAgICByZXR1cm4gc2VsZWN0ZWRUYWI7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKFNldFNlbGVjdGVkU2V0dGluZ1RhYilcclxuICBzZXR0aW5nTWFuYWdlbWVudEFjdGlvbih7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFNldHRpbmdNYW5hZ2VtZW50LlN0YXRlPiwgeyBwYXlsb2FkIH06IFNldFNlbGVjdGVkU2V0dGluZ1RhYikge1xyXG4gICAgcGF0Y2hTdGF0ZSh7XHJcbiAgICAgIHNlbGVjdGVkVGFiOiBwYXlsb2FkLFxyXG4gICAgfSk7XHJcbiAgfVxyXG59XHJcbiJdfQ==