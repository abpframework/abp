/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/setting-management-config.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Injectable } from '@angular/core';
import { addAbpRoutes, PatchRouteByName } from '@abp/ng.core';
import { getSettingTabs } from '@abp/ng.theme.shared';
import { Store } from '@ngxs/store';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
var SettingManagementConfigService = /** @class */ (function () {
    function SettingManagementConfigService(store) {
        var _this = this;
        this.store = store;
        /** @type {?} */
        var route = (/** @type {?} */ ({
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
        function () {
            /** @type {?} */
            var tabs = getSettingTabs();
            if (!tabs || !tabs.length) {
                _this.store.dispatch(new PatchRouteByName('AbpSettingManagement::Settings', tslib_1.__assign({}, route, { invisible: true })));
            }
        }));
    }
    SettingManagementConfigService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    SettingManagementConfigService.ctorParameters = function () { return [
        { type: Store }
    ]; };
    /** @nocollapse */ SettingManagementConfigService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function SettingManagementConfigService_Factory() { return new SettingManagementConfigService(i0.ɵɵinject(i1.Store)); }, token: SettingManagementConfigService, providedIn: "root" });
    return SettingManagementConfigService;
}());
export { SettingManagementConfigService };
if (false) {
    /**
     * @type {?}
     * @private
     */
    SettingManagementConfigService.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LWNvbmZpZy5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5zZXR0aW5nLW1hbmFnZW1lbnQuY29uZmlnLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL3NldHRpbmctbWFuYWdlbWVudC1jb25maWcuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxZQUFZLEVBQWUsZ0JBQWdCLEVBQU8sTUFBTSxjQUFjLENBQUM7QUFDaEYsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLHNCQUFzQixDQUFDO0FBQ3RELE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7OztBQUVwQztJQUlFLHdDQUFvQixLQUFZO1FBQWhDLGlCQW1CQztRQW5CbUIsVUFBSyxHQUFMLEtBQUssQ0FBTzs7WUFDeEIsS0FBSyxHQUFHLG1CQUFBO1lBQ1osSUFBSSxFQUFFLGdDQUFnQztZQUN0QyxJQUFJLEVBQUUsb0JBQW9CO1lBQzFCLFVBQVUsRUFBRSxzQ0FBc0M7WUFDbEQsY0FBYyxFQUFFLDhCQUE4QjtZQUM5QyxNQUFNLGlDQUF5QjtZQUMvQixLQUFLLEVBQUUsQ0FBQztZQUNSLFNBQVMsRUFBRSxXQUFXO1NBQ3ZCLEVBQWlCO1FBRWxCLFlBQVksQ0FBQyxLQUFLLENBQUMsQ0FBQztRQUVwQixVQUFVOzs7UUFBQzs7Z0JBQ0gsSUFBSSxHQUFHLGNBQWMsRUFBRTtZQUM3QixJQUFJLENBQUMsSUFBSSxJQUFJLENBQUMsSUFBSSxDQUFDLE1BQU0sRUFBRTtnQkFDekIsS0FBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxnQkFBZ0IsQ0FBQyxnQ0FBZ0MsdUJBQU8sS0FBSyxJQUFFLFNBQVMsRUFBRSxJQUFJLElBQUcsQ0FBQyxDQUFDO2FBQzVHO1FBQ0gsQ0FBQyxFQUFDLENBQUM7SUFDTCxDQUFDOztnQkF2QkYsVUFBVSxTQUFDO29CQUNWLFVBQVUsRUFBRSxNQUFNO2lCQUNuQjs7OztnQkFKUSxLQUFLOzs7eUNBSGQ7Q0E2QkMsQUF4QkQsSUF3QkM7U0FyQlksOEJBQThCOzs7Ozs7SUFDN0IsK0NBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgYWRkQWJwUm91dGVzLCBlTGF5b3V0VHlwZSwgUGF0Y2hSb3V0ZUJ5TmFtZSwgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IGdldFNldHRpbmdUYWJzIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBTZXR0aW5nTWFuYWdlbWVudENvbmZpZ1NlcnZpY2Uge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSkge1xuICAgIGNvbnN0IHJvdXRlID0ge1xuICAgICAgbmFtZTogJ0FicFNldHRpbmdNYW5hZ2VtZW50OjpTZXR0aW5ncycsXG4gICAgICBwYXRoOiAnc2V0dGluZy1tYW5hZ2VtZW50JyxcbiAgICAgIHBhcmVudE5hbWU6ICdBYnBVaU5hdmlnYXRpb246Ok1lbnU6QWRtaW5pc3RyYXRpb24nLFxuICAgICAgcmVxdWlyZWRQb2xpY3k6ICdBYnBBY2NvdW50LlNldHRpbmdNYW5hZ2VtZW50JyxcbiAgICAgIGxheW91dDogZUxheW91dFR5cGUuYXBwbGljYXRpb24sXG4gICAgICBvcmRlcjogNixcbiAgICAgIGljb25DbGFzczogJ2ZhIGZhLWNvZycsXG4gICAgfSBhcyBBQlAuRnVsbFJvdXRlO1xuXG4gICAgYWRkQWJwUm91dGVzKHJvdXRlKTtcblxuICAgIHNldFRpbWVvdXQoKCkgPT4ge1xuICAgICAgY29uc3QgdGFicyA9IGdldFNldHRpbmdUYWJzKCk7XG4gICAgICBpZiAoIXRhYnMgfHwgIXRhYnMubGVuZ3RoKSB7XG4gICAgICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IFBhdGNoUm91dGVCeU5hbWUoJ0FicFNldHRpbmdNYW5hZ2VtZW50OjpTZXR0aW5ncycsIHsgLi4ucm91dGUsIGludmlzaWJsZTogdHJ1ZSB9KSk7XG4gICAgICB9XG4gICAgfSk7XG4gIH1cbn1cbiJdfQ==