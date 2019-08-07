/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { TenantManagementGet } from '../actions/tenant-management.actions';
import { TenantManagementState } from '../states/tenant-management.state';
export class TenantsResolver {
    /**
     * @param {?} store
     */
    constructor(store) {
        this.store = store;
    }
    /**
     * @return {?}
     */
    resolve() {
        /** @type {?} */
        const data = this.store.selectSnapshot(TenantManagementState.get);
        return data && data.length
            ? null
            : this.store.dispatch(new TenantManagementGet());
    }
}
TenantsResolver.decorators = [
    { type: Injectable }
];
/** @nocollapse */
TenantsResolver.ctorParameters = () => [
    { type: Store }
];
if (false) {
    /**
     * @type {?}
     * @private
     */
    TenantsResolver.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50cy5yZXNvbHZlci5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGVuYW50LW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvcmVzb2x2ZXJzL3RlbmFudHMucmVzb2x2ZXIudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFM0MsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSxzQ0FBc0MsQ0FBQztBQUUzRSxPQUFPLEVBQUUscUJBQXFCLEVBQUUsTUFBTSxtQ0FBbUMsQ0FBQztBQUcxRSxNQUFNLE9BQU8sZUFBZTs7OztJQUMxQixZQUFvQixLQUFZO1FBQVosVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7SUFFcEMsT0FBTzs7Y0FDQyxJQUFJLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMscUJBQXFCLENBQUMsR0FBRyxDQUFDO1FBQ2pFLE9BQU8sSUFBSSxJQUFJLElBQUksQ0FBQyxNQUFNO1lBQ3pCLENBQUMsQ0FBQyxJQUFJO1lBQ04sQ0FBQyxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksbUJBQW1CLEVBQUUsQ0FBQyxDQUFDO0lBQ3BELENBQUM7OztZQVRGLFVBQVU7Ozs7WUFMRixLQUFLOzs7Ozs7O0lBT0EsZ0NBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgUmVzb2x2ZSB9IGZyb20gJ0Bhbmd1bGFyL3JvdXRlcic7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnRHZXQgfSBmcm9tICcuLi9hY3Rpb25zL3RlbmFudC1tYW5hZ2VtZW50LmFjdGlvbnMnO1xuaW1wb3J0IHsgVGVuYW50TWFuYWdlbWVudCB9IGZyb20gJy4uL21vZGVscy90ZW5hbnQtbWFuYWdlbWVudCc7XG5pbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50U3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMvdGVuYW50LW1hbmFnZW1lbnQuc3RhdGUnO1xuXG5ASW5qZWN0YWJsZSgpXG5leHBvcnQgY2xhc3MgVGVuYW50c1Jlc29sdmVyIGltcGxlbWVudHMgUmVzb2x2ZTxUZW5hbnRNYW5hZ2VtZW50LlN0YXRlPiB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIHJlc29sdmUoKSB7XG4gICAgY29uc3QgZGF0YSA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoVGVuYW50TWFuYWdlbWVudFN0YXRlLmdldCk7XG4gICAgcmV0dXJuIGRhdGEgJiYgZGF0YS5sZW5ndGhcbiAgICAgPyBudWxsIFxuICAgICA6IHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IFRlbmFudE1hbmFnZW1lbnRHZXQoKSk7XG4gIH1cbn1cbiJdfQ==