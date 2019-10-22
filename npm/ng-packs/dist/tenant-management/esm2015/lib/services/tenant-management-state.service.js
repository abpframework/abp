/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { TenantManagementState } from '../states/tenant-management.state';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
export class TenantManagementStateService {
    /**
     * @param {?} store
     */
    constructor(store) {
        this.store = store;
    }
    /**
     * @return {?}
     */
    getTenants() {
        return this.store.selectSnapshot(TenantManagementState.get);
    }
    /**
     * @return {?}
     */
    getTenantsTotalCount() {
        return this.store.selectSnapshot(TenantManagementState.getTenantsTotalCount);
    }
}
TenantManagementStateService.decorators = [
    { type: Injectable, args: [{
                providedIn: 'root',
            },] }
];
/** @nocollapse */
TenantManagementStateService.ctorParameters = () => [
    { type: Store }
];
/** @nocollapse */ TenantManagementStateService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function TenantManagementStateService_Factory() { return new TenantManagementStateService(i0.ɵɵinject(i1.Store)); }, token: TenantManagementStateService, providedIn: "root" });
if (false) {
    /**
     * @type {?}
     * @private
     */
    TenantManagementStateService.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQtc3RhdGUuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGVuYW50LW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvdGVuYW50LW1hbmFnZW1lbnQtc3RhdGUuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxxQkFBcUIsRUFBRSxNQUFNLG1DQUFtQyxDQUFDOzs7QUFLMUUsTUFBTSxPQUFPLDRCQUE0Qjs7OztJQUN2QyxZQUFvQixLQUFZO1FBQVosVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7SUFFcEMsVUFBVTtRQUNSLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMscUJBQXFCLENBQUMsR0FBRyxDQUFDLENBQUM7SUFDOUQsQ0FBQzs7OztJQUVELG9CQUFvQjtRQUNsQixPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLHFCQUFxQixDQUFDLG9CQUFvQixDQUFDLENBQUM7SUFDL0UsQ0FBQzs7O1lBWkYsVUFBVSxTQUFDO2dCQUNWLFVBQVUsRUFBRSxNQUFNO2FBQ25COzs7O1lBTFEsS0FBSzs7Ozs7Ozs7SUFPQSw2Q0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50U3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMvdGVuYW50LW1hbmFnZW1lbnQuc3RhdGUnO1xyXG5cclxuQEluamVjdGFibGUoe1xyXG4gIHByb3ZpZGVkSW46ICdyb290JyxcclxufSlcclxuZXhwb3J0IGNsYXNzIFRlbmFudE1hbmFnZW1lbnRTdGF0ZVNlcnZpY2Uge1xyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxyXG5cclxuICBnZXRUZW5hbnRzKCkge1xyXG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoVGVuYW50TWFuYWdlbWVudFN0YXRlLmdldCk7XHJcbiAgfVxyXG5cclxuICBnZXRUZW5hbnRzVG90YWxDb3VudCgpIHtcclxuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFRlbmFudE1hbmFnZW1lbnRTdGF0ZS5nZXRUZW5hbnRzVG90YWxDb3VudCk7XHJcbiAgfVxyXG59XHJcbiJdfQ==