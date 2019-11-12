/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/tenant-management-state.service.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { TenantManagementState } from '../states/tenant-management.state';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
var TenantManagementStateService = /** @class */ (function () {
    function TenantManagementStateService(store) {
        this.store = store;
    }
    /**
     * @return {?}
     */
    TenantManagementStateService.prototype.get = /**
     * @return {?}
     */
    function () {
        return this.store.selectSnapshot(TenantManagementState.get);
    };
    /**
     * @return {?}
     */
    TenantManagementStateService.prototype.getTenantsTotalCount = /**
     * @return {?}
     */
    function () {
        return this.store.selectSnapshot(TenantManagementState.getTenantsTotalCount);
    };
    TenantManagementStateService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    TenantManagementStateService.ctorParameters = function () { return [
        { type: Store }
    ]; };
    /** @nocollapse */ TenantManagementStateService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function TenantManagementStateService_Factory() { return new TenantManagementStateService(i0.ɵɵinject(i1.Store)); }, token: TenantManagementStateService, providedIn: "root" });
    return TenantManagementStateService;
}());
export { TenantManagementStateService };
if (false) {
    /**
     * @type {?}
     * @private
     */
    TenantManagementStateService.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQtc3RhdGUuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGVuYW50LW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvdGVuYW50LW1hbmFnZW1lbnQtc3RhdGUuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSxtQ0FBbUMsQ0FBQzs7O0FBRTFFO0lBSUUsc0NBQW9CLEtBQVk7UUFBWixVQUFLLEdBQUwsS0FBSyxDQUFPO0lBQUcsQ0FBQzs7OztJQUVwQywwQ0FBRzs7O0lBQUg7UUFDRSxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLHFCQUFxQixDQUFDLEdBQUcsQ0FBQyxDQUFDO0lBQzlELENBQUM7Ozs7SUFFRCwyREFBb0I7OztJQUFwQjtRQUNFLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMscUJBQXFCLENBQUMsb0JBQW9CLENBQUMsQ0FBQztJQUMvRSxDQUFDOztnQkFaRixVQUFVLFNBQUM7b0JBQ1YsVUFBVSxFQUFFLE1BQU07aUJBQ25COzs7O2dCQUxRLEtBQUs7Ozt1Q0FEZDtDQWlCQyxBQWJELElBYUM7U0FWWSw0QkFBNEI7Ozs7OztJQUMzQiw2Q0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50U3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMvdGVuYW50LW1hbmFnZW1lbnQuc3RhdGUnO1xyXG5cclxuQEluamVjdGFibGUoe1xyXG4gIHByb3ZpZGVkSW46ICdyb290JyxcclxufSlcclxuZXhwb3J0IGNsYXNzIFRlbmFudE1hbmFnZW1lbnRTdGF0ZVNlcnZpY2Uge1xyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxyXG5cclxuICBnZXQoKSB7XHJcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChUZW5hbnRNYW5hZ2VtZW50U3RhdGUuZ2V0KTtcclxuICB9XHJcblxyXG4gIGdldFRlbmFudHNUb3RhbENvdW50KCkge1xyXG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoVGVuYW50TWFuYWdlbWVudFN0YXRlLmdldFRlbmFudHNUb3RhbENvdW50KTtcclxuICB9XHJcbn1cclxuIl19