/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LWNvbmZpZy5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5zZXR0aW5nLW1hbmFnZW1lbnQuY29uZmlnLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL3NldHRpbmctbWFuYWdlbWVudC1jb25maWcuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLFlBQVksRUFBZSxnQkFBZ0IsRUFBTyxNQUFNLGNBQWMsQ0FBQztBQUNoRixPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDdEQsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQzs7O0FBRXBDO0lBSUUsd0NBQW9CLEtBQVk7UUFBaEMsaUJBbUJDO1FBbkJtQixVQUFLLEdBQUwsS0FBSyxDQUFPOztZQUN4QixLQUFLLEdBQUcsbUJBQUE7WUFDWixJQUFJLEVBQUUsZ0NBQWdDO1lBQ3RDLElBQUksRUFBRSxvQkFBb0I7WUFDMUIsVUFBVSxFQUFFLHNDQUFzQztZQUNsRCxjQUFjLEVBQUUsOEJBQThCO1lBQzlDLE1BQU0saUNBQXlCO1lBQy9CLEtBQUssRUFBRSxDQUFDO1lBQ1IsU0FBUyxFQUFFLFdBQVc7U0FDdkIsRUFBaUI7UUFFbEIsWUFBWSxDQUFDLEtBQUssQ0FBQyxDQUFDO1FBRXBCLFVBQVU7OztRQUFDOztnQkFDSCxJQUFJLEdBQUcsY0FBYyxFQUFFO1lBQzdCLElBQUksQ0FBQyxJQUFJLElBQUksQ0FBQyxJQUFJLENBQUMsTUFBTSxFQUFFO2dCQUN6QixLQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLGdCQUFnQixDQUFDLGdDQUFnQyx1QkFBTyxLQUFLLElBQUUsU0FBUyxFQUFFLElBQUksSUFBRyxDQUFDLENBQUM7YUFDNUc7UUFDSCxDQUFDLEVBQUMsQ0FBQztJQUNMLENBQUM7O2dCQXZCRixVQUFVLFNBQUM7b0JBQ1YsVUFBVSxFQUFFLE1BQU07aUJBQ25COzs7O2dCQUpRLEtBQUs7Ozt5Q0FIZDtDQTZCQyxBQXhCRCxJQXdCQztTQXJCWSw4QkFBOEI7Ozs7OztJQUM3QiwrQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBhZGRBYnBSb3V0ZXMsIGVMYXlvdXRUeXBlLCBQYXRjaFJvdXRlQnlOYW1lLCBBQlAgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgZ2V0U2V0dGluZ1RhYnMgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcblxuQEluamVjdGFibGUoe1xuICBwcm92aWRlZEluOiAncm9vdCcsXG59KVxuZXhwb3J0IGNsYXNzIFNldHRpbmdNYW5hZ2VtZW50Q29uZmlnU2VydmljZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7XG4gICAgY29uc3Qgcm91dGUgPSB7XG4gICAgICBuYW1lOiAnQWJwU2V0dGluZ01hbmFnZW1lbnQ6OlNldHRpbmdzJyxcbiAgICAgIHBhdGg6ICdzZXR0aW5nLW1hbmFnZW1lbnQnLFxuICAgICAgcGFyZW50TmFtZTogJ0FicFVpTmF2aWdhdGlvbjo6TWVudTpBZG1pbmlzdHJhdGlvbicsXG4gICAgICByZXF1aXJlZFBvbGljeTogJ0FicEFjY291bnQuU2V0dGluZ01hbmFnZW1lbnQnLFxuICAgICAgbGF5b3V0OiBlTGF5b3V0VHlwZS5hcHBsaWNhdGlvbixcbiAgICAgIG9yZGVyOiA2LFxuICAgICAgaWNvbkNsYXNzOiAnZmEgZmEtY29nJyxcbiAgICB9IGFzIEFCUC5GdWxsUm91dGU7XG5cbiAgICBhZGRBYnBSb3V0ZXMocm91dGUpO1xuXG4gICAgc2V0VGltZW91dCgoKSA9PiB7XG4gICAgICBjb25zdCB0YWJzID0gZ2V0U2V0dGluZ1RhYnMoKTtcbiAgICAgIGlmICghdGFicyB8fCAhdGFicy5sZW5ndGgpIHtcbiAgICAgICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgUGF0Y2hSb3V0ZUJ5TmFtZSgnQWJwU2V0dGluZ01hbmFnZW1lbnQ6OlNldHRpbmdzJywgeyAuLi5yb3V0ZSwgaW52aXNpYmxlOiB0cnVlIH0pKTtcbiAgICAgIH1cbiAgICB9KTtcbiAgfVxufVxuIl19