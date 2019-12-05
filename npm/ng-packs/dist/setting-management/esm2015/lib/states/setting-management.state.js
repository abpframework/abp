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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5zZXR0aW5nLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL3NldHRpbmctbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLFFBQVEsRUFBRSxLQUFLLEVBQWdCLE1BQU0sYUFBYSxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLHVDQUF1QyxDQUFDO0lBT2pFLHNCQUFzQixTQUF0QixzQkFBc0I7Ozs7O0lBRWpDLE1BQU0sQ0FBQyxjQUFjLENBQUMsRUFBRSxXQUFXLEVBQTJCO1FBQzVELE9BQU8sV0FBVyxDQUFDO0lBQ3JCLENBQUM7Ozs7OztJQUdELHVCQUF1QixDQUFDLEVBQUUsVUFBVSxFQUF5QyxFQUFFLEVBQUUsT0FBTyxFQUF5QjtRQUMvRyxVQUFVLENBQUM7WUFDVCxXQUFXLEVBQUUsT0FBTztTQUNyQixDQUFDLENBQUM7SUFDTCxDQUFDO0NBQ0YsQ0FBQTtBQUxDO0lBREMsTUFBTSxDQUFDLHFCQUFxQixDQUFDOztxREFDOEQscUJBQXFCOztxRUFJaEg7QUFURDtJQURDLFFBQVEsRUFBRTs7OztrREFHVjtBQUpVLHNCQUFzQjtJQUpsQyxLQUFLLENBQTBCO1FBQzlCLElBQUksRUFBRSx3QkFBd0I7UUFDOUIsUUFBUSxFQUFFLG1CQUFBLEVBQUUsV0FBVyxFQUFFLEVBQUUsRUFBRSxFQUEyQjtLQUN6RCxDQUFDO0dBQ1csc0JBQXNCLENBWWxDO1NBWlksc0JBQXNCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQWN0aW9uLCBTZWxlY3RvciwgU3RhdGUsIFN0YXRlQ29udGV4dCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IFNldFNlbGVjdGVkU2V0dGluZ1RhYiB9IGZyb20gJy4uL2FjdGlvbnMvc2V0dGluZy1tYW5hZ2VtZW50LmFjdGlvbnMnO1xuaW1wb3J0IHsgU2V0dGluZ01hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMvc2V0dGluZy1tYW5hZ2VtZW50JztcblxuQFN0YXRlPFNldHRpbmdNYW5hZ2VtZW50LlN0YXRlPih7XG4gIG5hbWU6ICdTZXR0aW5nTWFuYWdlbWVudFN0YXRlJyxcbiAgZGVmYXVsdHM6IHsgc2VsZWN0ZWRUYWI6IHt9IH0gYXMgU2V0dGluZ01hbmFnZW1lbnQuU3RhdGUsXG59KVxuZXhwb3J0IGNsYXNzIFNldHRpbmdNYW5hZ2VtZW50U3RhdGUge1xuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0U2VsZWN0ZWRUYWIoeyBzZWxlY3RlZFRhYiB9OiBTZXR0aW5nTWFuYWdlbWVudC5TdGF0ZSkge1xuICAgIHJldHVybiBzZWxlY3RlZFRhYjtcbiAgfVxuXG4gIEBBY3Rpb24oU2V0U2VsZWN0ZWRTZXR0aW5nVGFiKVxuICBzZXR0aW5nTWFuYWdlbWVudEFjdGlvbih7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFNldHRpbmdNYW5hZ2VtZW50LlN0YXRlPiwgeyBwYXlsb2FkIH06IFNldFNlbGVjdGVkU2V0dGluZ1RhYikge1xuICAgIHBhdGNoU3RhdGUoe1xuICAgICAgc2VsZWN0ZWRUYWI6IHBheWxvYWQsXG4gICAgfSk7XG4gIH1cbn1cbiJdfQ==