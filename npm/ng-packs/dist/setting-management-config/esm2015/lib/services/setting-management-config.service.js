/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/setting-management-config.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { addAbpRoutes, PatchRouteByName } from '@abp/ng.core';
import { getSettingTabs } from '@abp/ng.theme.shared';
import { Store } from '@ngxs/store';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
export class SettingManagementConfigService {
    /**
     * @param {?} store
     */
    constructor(store) {
        this.store = store;
        /** @type {?} */
        const route = (/** @type {?} */ ({
            name: 'AbpSettingManagement::Settings',
            path: 'setting-management',
            parentName: 'AbpUiNavigation::Menu:Administration',
            requiredPolicy: 'AbpAccount.SettingManagement',
            layout: "application" /* application */,
            order: 6,
            iconClass: 'fa fa-cog',
        }));
        addAbpRoutes(route);
        setTimeout((/**
         * @return {?}
         */
        () => {
            /** @type {?} */
            const tabs = getSettingTabs();
            if (!tabs || !tabs.length) {
                this.store.dispatch(new PatchRouteByName('AbpSettingManagement::Settings', Object.assign({}, route, { invisible: true })));
            }
        }));
    }
}
SettingManagementConfigService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */
SettingManagementConfigService.ctorParameters = () => [
    { type: Store }
];
/** @nocollapse */ SettingManagementConfigService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function SettingManagementConfigService_Factory() { return new SettingManagementConfigService(i0.ɵɵinject(i1.Store)); }, token: SettingManagementConfigService, providedIn: "root" });
if (false) {
    /**
     * @type {?}
     * @private
     */
    SettingManagementConfigService.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LWNvbmZpZy5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5zZXR0aW5nLW1hbmFnZW1lbnQuY29uZmlnLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL3NldHRpbmctbWFuYWdlbWVudC1jb25maWcuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLFlBQVksRUFBZSxnQkFBZ0IsRUFBTyxNQUFNLGNBQWMsQ0FBQztBQUNoRixPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDdEQsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQzs7O0FBS3BDLE1BQU0sT0FBTyw4QkFBOEI7Ozs7SUFDekMsWUFBb0IsS0FBWTtRQUFaLFVBQUssR0FBTCxLQUFLLENBQU87O2NBQ3hCLEtBQUssR0FBRyxtQkFBQTtZQUNaLElBQUksRUFBRSxnQ0FBZ0M7WUFDdEMsSUFBSSxFQUFFLG9CQUFvQjtZQUMxQixVQUFVLEVBQUUsc0NBQXNDO1lBQ2xELGNBQWMsRUFBRSw4QkFBOEI7WUFDOUMsTUFBTSxpQ0FBeUI7WUFDL0IsS0FBSyxFQUFFLENBQUM7WUFDUixTQUFTLEVBQUUsV0FBVztTQUN2QixFQUFpQjtRQUVsQixZQUFZLENBQUMsS0FBSyxDQUFDLENBQUM7UUFFcEIsVUFBVTs7O1FBQUMsR0FBRyxFQUFFOztrQkFDUixJQUFJLEdBQUcsY0FBYyxFQUFFO1lBQzdCLElBQUksQ0FBQyxJQUFJLElBQUksQ0FBQyxJQUFJLENBQUMsTUFBTSxFQUFFO2dCQUN6QixJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLGdCQUFnQixDQUFDLGdDQUFnQyxvQkFBTyxLQUFLLElBQUUsU0FBUyxFQUFFLElBQUksSUFBRyxDQUFDLENBQUM7YUFDNUc7UUFDSCxDQUFDLEVBQUMsQ0FBQztJQUNMLENBQUM7OztZQXZCRixVQUFVLFNBQUM7Z0JBQ1YsVUFBVSxFQUFFLE1BQU07YUFDbkI7Ozs7WUFKUSxLQUFLOzs7Ozs7OztJQU1BLCtDQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IGFkZEFicFJvdXRlcywgZUxheW91dFR5cGUsIFBhdGNoUm91dGVCeU5hbWUsIEFCUCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBnZXRTZXR0aW5nVGFicyB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuXG5ASW5qZWN0YWJsZSh7XG4gIHByb3ZpZGVkSW46ICdyb290Jyxcbn0pXG5leHBvcnQgY2xhc3MgU2V0dGluZ01hbmFnZW1lbnRDb25maWdTZXJ2aWNlIHtcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUpIHtcbiAgICBjb25zdCByb3V0ZSA9IHtcbiAgICAgIG5hbWU6ICdBYnBTZXR0aW5nTWFuYWdlbWVudDo6U2V0dGluZ3MnLFxuICAgICAgcGF0aDogJ3NldHRpbmctbWFuYWdlbWVudCcsXG4gICAgICBwYXJlbnROYW1lOiAnQWJwVWlOYXZpZ2F0aW9uOjpNZW51OkFkbWluaXN0cmF0aW9uJyxcbiAgICAgIHJlcXVpcmVkUG9saWN5OiAnQWJwQWNjb3VudC5TZXR0aW5nTWFuYWdlbWVudCcsXG4gICAgICBsYXlvdXQ6IGVMYXlvdXRUeXBlLmFwcGxpY2F0aW9uLFxuICAgICAgb3JkZXI6IDYsXG4gICAgICBpY29uQ2xhc3M6ICdmYSBmYS1jb2cnLFxuICAgIH0gYXMgQUJQLkZ1bGxSb3V0ZTtcblxuICAgIGFkZEFicFJvdXRlcyhyb3V0ZSk7XG5cbiAgICBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgIGNvbnN0IHRhYnMgPSBnZXRTZXR0aW5nVGFicygpO1xuICAgICAgaWYgKCF0YWJzIHx8ICF0YWJzLmxlbmd0aCkge1xuICAgICAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBQYXRjaFJvdXRlQnlOYW1lKCdBYnBTZXR0aW5nTWFuYWdlbWVudDo6U2V0dGluZ3MnLCB7IC4uLnJvdXRlLCBpbnZpc2libGU6IHRydWUgfSkpO1xuICAgICAgfVxuICAgIH0pO1xuICB9XG59XG4iXX0=