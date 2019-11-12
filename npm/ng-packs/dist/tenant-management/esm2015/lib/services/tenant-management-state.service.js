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
    get() {
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQtc3RhdGUuc2VydmljZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGVuYW50LW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvdGVuYW50LW1hbmFnZW1lbnQtc3RhdGUuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSxtQ0FBbUMsQ0FBQzs7O0FBSzFFLE1BQU0sT0FBTyw0QkFBNEI7Ozs7SUFDdkMsWUFBb0IsS0FBWTtRQUFaLFVBQUssR0FBTCxLQUFLLENBQU87SUFBRyxDQUFDOzs7O0lBRXBDLEdBQUc7UUFDRCxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLHFCQUFxQixDQUFDLEdBQUcsQ0FBQyxDQUFDO0lBQzlELENBQUM7Ozs7SUFFRCxvQkFBb0I7UUFDbEIsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxxQkFBcUIsQ0FBQyxvQkFBb0IsQ0FBQyxDQUFDO0lBQy9FLENBQUM7OztZQVpGLFVBQVUsU0FBQztnQkFDVixVQUFVLEVBQUUsTUFBTTthQUNuQjs7OztZQUxRLEtBQUs7Ozs7Ozs7O0lBT0EsNkNBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgVGVuYW50TWFuYWdlbWVudFN0YXRlIH0gZnJvbSAnLi4vc3RhdGVzL3RlbmFudC1tYW5hZ2VtZW50LnN0YXRlJztcclxuXHJcbkBJbmplY3RhYmxlKHtcclxuICBwcm92aWRlZEluOiAncm9vdCcsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBUZW5hbnRNYW5hZ2VtZW50U3RhdGVTZXJ2aWNlIHtcclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cclxuXHJcbiAgZ2V0KCkge1xyXG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoVGVuYW50TWFuYWdlbWVudFN0YXRlLmdldCk7XHJcbiAgfVxyXG5cclxuICBnZXRUZW5hbnRzVG90YWxDb3VudCgpIHtcclxuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFRlbmFudE1hbmFnZW1lbnRTdGF0ZS5nZXRUZW5hbnRzVG90YWxDb3VudCk7XHJcbiAgfVxyXG59XHJcbiJdfQ==