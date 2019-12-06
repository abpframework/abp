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
export class PermissionManagementStateService {
    /**
     * @param {?} store
     */
    constructor(store) {
        this.store = store;
    }
    /**
     * @return {?}
     */
    getPermissionGroups() {
        return this.store.selectSnapshot(PermissionManagementState.getPermissionGroups);
    }
    /**
     * @return {?}
     */
    getEntityDisplayName() {
        return this.store.selectSnapshot(PermissionManagementState.getEntityDisplayName);
    }
}
PermissionManagementStateService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */
PermissionManagementStateService.ctorParameters = () => [
    { type: Store }
];
/** @nocollapse */ PermissionManagementStateService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function PermissionManagementStateService_Factory() { return new PermissionManagementStateService(i0.ɵɵinject(i1.Store)); }, token: PermissionManagementStateService, providedIn: "root" });
if (false) {
    /**
     * @type {?}
     * @private
     */
    PermissionManagementStateService.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LXN0YXRlLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnBlcm1pc3Npb24tbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQtc3RhdGUuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUseUJBQXlCLEVBQUUsTUFBTSx1Q0FBdUMsQ0FBQzs7O0FBS2xGLE1BQU0sT0FBTyxnQ0FBZ0M7Ozs7SUFDM0MsWUFBb0IsS0FBWTtRQUFaLFVBQUssR0FBTCxLQUFLLENBQU87SUFBRyxDQUFDOzs7O0lBRXBDLG1CQUFtQjtRQUNqQixPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLHlCQUF5QixDQUFDLG1CQUFtQixDQUFDLENBQUM7SUFDbEYsQ0FBQzs7OztJQUNELG9CQUFvQjtRQUNsQixPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLHlCQUF5QixDQUFDLG9CQUFvQixDQUFDLENBQUM7SUFDbkYsQ0FBQzs7O1lBWEYsVUFBVSxTQUFDO2dCQUNWLFVBQVUsRUFBRSxNQUFNO2FBQ25COzs7O1lBTFEsS0FBSzs7Ozs7Ozs7SUFPQSxpREFBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IFBlcm1pc3Npb25NYW5hZ2VtZW50U3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMvcGVybWlzc2lvbi1tYW5hZ2VtZW50LnN0YXRlJztcblxuQEluamVjdGFibGUoe1xuICBwcm92aWRlZEluOiAncm9vdCcsXG59KVxuZXhwb3J0IGNsYXNzIFBlcm1pc3Npb25NYW5hZ2VtZW50U3RhdGVTZXJ2aWNlIHtcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgZ2V0UGVybWlzc2lvbkdyb3VwcygpIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlLmdldFBlcm1pc3Npb25Hcm91cHMpO1xuICB9XG4gIGdldEVudGl0eURpc3BsYXlOYW1lKCkge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFBlcm1pc3Npb25NYW5hZ2VtZW50U3RhdGUuZ2V0RW50aXR5RGlzcGxheU5hbWUpO1xuICB9XG59XG4iXX0=