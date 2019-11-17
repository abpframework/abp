/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/permission-management-state.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { PermissionManagementState } from '../states/permission-management.state';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
var PermissionManagementStateService = /** @class */ (function () {
    function PermissionManagementStateService(store) {
        this.store = store;
    }
    /**
     * @return {?}
     */
    PermissionManagementStateService.prototype.getPermissionGroups = /**
     * @return {?}
     */
    function () {
        return this.store.selectSnapshot(PermissionManagementState.getPermissionGroups);
    };
    /**
     * @return {?}
     */
    PermissionManagementStateService.prototype.getEntityDisplayName = /**
     * @return {?}
     */
    function () {
        return this.store.selectSnapshot(PermissionManagementState.getEntityDisplayName);
    };
    PermissionManagementStateService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    PermissionManagementStateService.ctorParameters = function () { return [
        { type: Store }
    ]; };
    /** @nocollapse */ PermissionManagementStateService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function PermissionManagementStateService_Factory() { return new PermissionManagementStateService(i0.ɵɵinject(i1.Store)); }, token: PermissionManagementStateService, providedIn: "root" });
    return PermissionManagementStateService;
}());
export { PermissionManagementStateService };
if (false) {
    /**
     * @type {?}
     * @private
     */
    PermissionManagementStateService.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LXN0YXRlLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnBlcm1pc3Npb24tbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQtc3RhdGUuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUseUJBQXlCLEVBQUUsTUFBTSx1Q0FBdUMsQ0FBQzs7O0FBRWxGO0lBSUUsMENBQW9CLEtBQVk7UUFBWixVQUFLLEdBQUwsS0FBSyxDQUFPO0lBQUcsQ0FBQzs7OztJQUVwQyw4REFBbUI7OztJQUFuQjtRQUNFLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMseUJBQXlCLENBQUMsbUJBQW1CLENBQUMsQ0FBQztJQUNsRixDQUFDOzs7O0lBQ0QsK0RBQW9COzs7SUFBcEI7UUFDRSxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLHlCQUF5QixDQUFDLG9CQUFvQixDQUFDLENBQUM7SUFDbkYsQ0FBQzs7Z0JBWEYsVUFBVSxTQUFDO29CQUNWLFVBQVUsRUFBRSxNQUFNO2lCQUNuQjs7OztnQkFMUSxLQUFLOzs7MkNBRGQ7Q0FnQkMsQUFaRCxJQVlDO1NBVFksZ0NBQWdDOzs7Ozs7SUFDL0IsaURBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQuc3RhdGUnO1xyXG5cclxuQEluamVjdGFibGUoe1xyXG4gIHByb3ZpZGVkSW46ICdyb290JyxcclxufSlcclxuZXhwb3J0IGNsYXNzIFBlcm1pc3Npb25NYW5hZ2VtZW50U3RhdGVTZXJ2aWNlIHtcclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cclxuXHJcbiAgZ2V0UGVybWlzc2lvbkdyb3VwcygpIHtcclxuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFBlcm1pc3Npb25NYW5hZ2VtZW50U3RhdGUuZ2V0UGVybWlzc2lvbkdyb3Vwcyk7XHJcbiAgfVxyXG4gIGdldEVudGl0eURpc3BsYXlOYW1lKCkge1xyXG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZS5nZXRFbnRpdHlEaXNwbGF5TmFtZSk7XHJcbiAgfVxyXG59XHJcbiJdfQ==