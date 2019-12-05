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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LXN0YXRlLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnBlcm1pc3Npb24tbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQtc3RhdGUuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUseUJBQXlCLEVBQUUsTUFBTSx1Q0FBdUMsQ0FBQzs7O0FBRWxGO0lBSUUsMENBQW9CLEtBQVk7UUFBWixVQUFLLEdBQUwsS0FBSyxDQUFPO0lBQUcsQ0FBQzs7OztJQUVwQyw4REFBbUI7OztJQUFuQjtRQUNFLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMseUJBQXlCLENBQUMsbUJBQW1CLENBQUMsQ0FBQztJQUNsRixDQUFDOzs7O0lBQ0QsK0RBQW9COzs7SUFBcEI7UUFDRSxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLHlCQUF5QixDQUFDLG9CQUFvQixDQUFDLENBQUM7SUFDbkYsQ0FBQzs7Z0JBWEYsVUFBVSxTQUFDO29CQUNWLFVBQVUsRUFBRSxNQUFNO2lCQUNuQjs7OztnQkFMUSxLQUFLOzs7MkNBRGQ7Q0FnQkMsQUFaRCxJQVlDO1NBVFksZ0NBQWdDOzs7Ozs7SUFDL0IsaURBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlIH0gZnJvbSAnLi4vc3RhdGVzL3Blcm1pc3Npb24tbWFuYWdlbWVudC5zdGF0ZSc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlU2VydmljZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIGdldFBlcm1pc3Npb25Hcm91cHMoKSB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZS5nZXRQZXJtaXNzaW9uR3JvdXBzKTtcbiAgfVxuICBnZXRFbnRpdHlEaXNwbGF5TmFtZSgpIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlLmdldEVudGl0eURpc3BsYXlOYW1lKTtcbiAgfVxufVxuIl19