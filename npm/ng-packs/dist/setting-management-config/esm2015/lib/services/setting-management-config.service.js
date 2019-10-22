/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LWNvbmZpZy5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5zZXR0aW5nLW1hbmFnZW1lbnQuY29uZmlnLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL3NldHRpbmctbWFuYWdlbWVudC1jb25maWcuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsWUFBWSxFQUFlLGdCQUFnQixFQUFPLE1BQU0sY0FBYyxDQUFDO0FBQ2hGLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxzQkFBc0IsQ0FBQztBQUN0RCxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDOzs7QUFLcEMsTUFBTSxPQUFPLDhCQUE4Qjs7OztJQUN6QyxZQUFvQixLQUFZO1FBQVosVUFBSyxHQUFMLEtBQUssQ0FBTzs7Y0FDeEIsS0FBSyxHQUFHLG1CQUFBO1lBQ1osSUFBSSxFQUFFLGdDQUFnQztZQUN0QyxJQUFJLEVBQUUsb0JBQW9CO1lBQzFCLFVBQVUsRUFBRSxzQ0FBc0M7WUFDbEQsTUFBTSxpQ0FBeUI7WUFDL0IsS0FBSyxFQUFFLENBQUM7WUFDUixTQUFTLEVBQUUsV0FBVztTQUN2QixFQUFpQjtRQUVsQixZQUFZLENBQUMsS0FBSyxDQUFDLENBQUM7UUFFcEIsVUFBVTs7O1FBQUMsR0FBRyxFQUFFOztrQkFDUixJQUFJLEdBQUcsY0FBYyxFQUFFO1lBQzdCLElBQUksQ0FBQyxJQUFJLElBQUksQ0FBQyxJQUFJLENBQUMsTUFBTSxFQUFFO2dCQUN6QixJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLGdCQUFnQixDQUFDLGdDQUFnQyxvQkFBTyxLQUFLLElBQUUsU0FBUyxFQUFFLElBQUksSUFBRyxDQUFDLENBQUM7YUFDNUc7UUFDSCxDQUFDLEVBQUMsQ0FBQztJQUNMLENBQUM7OztZQXRCRixVQUFVLFNBQUM7Z0JBQ1YsVUFBVSxFQUFFLE1BQU07YUFDbkI7Ozs7WUFKUSxLQUFLOzs7Ozs7OztJQU1BLCtDQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgYWRkQWJwUm91dGVzLCBlTGF5b3V0VHlwZSwgUGF0Y2hSb3V0ZUJ5TmFtZSwgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHsgZ2V0U2V0dGluZ1RhYnMgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XHJcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5cclxuQEluamVjdGFibGUoe1xyXG4gIHByb3ZpZGVkSW46ICdyb290JyxcclxufSlcclxuZXhwb3J0IGNsYXNzIFNldHRpbmdNYW5hZ2VtZW50Q29uZmlnU2VydmljZSB7XHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUpIHtcclxuICAgIGNvbnN0IHJvdXRlID0ge1xyXG4gICAgICBuYW1lOiAnQWJwU2V0dGluZ01hbmFnZW1lbnQ6OlNldHRpbmdzJyxcclxuICAgICAgcGF0aDogJ3NldHRpbmctbWFuYWdlbWVudCcsXHJcbiAgICAgIHBhcmVudE5hbWU6ICdBYnBVaU5hdmlnYXRpb246Ok1lbnU6QWRtaW5pc3RyYXRpb24nLFxyXG4gICAgICBsYXlvdXQ6IGVMYXlvdXRUeXBlLmFwcGxpY2F0aW9uLFxyXG4gICAgICBvcmRlcjogNixcclxuICAgICAgaWNvbkNsYXNzOiAnZmEgZmEtY29nJyxcclxuICAgIH0gYXMgQUJQLkZ1bGxSb3V0ZTtcclxuXHJcbiAgICBhZGRBYnBSb3V0ZXMocm91dGUpO1xyXG5cclxuICAgIHNldFRpbWVvdXQoKCkgPT4ge1xyXG4gICAgICBjb25zdCB0YWJzID0gZ2V0U2V0dGluZ1RhYnMoKTtcclxuICAgICAgaWYgKCF0YWJzIHx8ICF0YWJzLmxlbmd0aCkge1xyXG4gICAgICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IFBhdGNoUm91dGVCeU5hbWUoJ0FicFNldHRpbmdNYW5hZ2VtZW50OjpTZXR0aW5ncycsIHsgLi4ucm91dGUsIGludmlzaWJsZTogdHJ1ZSB9KSk7XHJcbiAgICAgIH1cclxuICAgIH0pO1xyXG4gIH1cclxufVxyXG4iXX0=