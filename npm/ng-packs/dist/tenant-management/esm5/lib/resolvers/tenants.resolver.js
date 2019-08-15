/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { GetTenants } from '../actions/tenant-management.actions';
import { TenantManagementState } from '../states/tenant-management.state';
var TenantsResolver = /** @class */ (function () {
    function TenantsResolver(store) {
        this.store = store;
    }
    /**
     * @return {?}
     */
    TenantsResolver.prototype.resolve = /**
     * @return {?}
     */
    function () {
        /** @type {?} */
        var data = this.store.selectSnapshot(TenantManagementState.get);
        return data && data.length ? null : this.store.dispatch(new GetTenants());
    };
    TenantsResolver.decorators = [
        { type: Injectable }
    ];
    /** @nocollapse */
    TenantsResolver.ctorParameters = function () { return [
        { type: Store }
    ]; };
    return TenantsResolver;
}());
export { TenantsResolver };
if (false) {
    /**
     * @type {?}
     * @private
     */
    TenantsResolver.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50cy5yZXNvbHZlci5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGVuYW50LW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvcmVzb2x2ZXJzL3RlbmFudHMucmVzb2x2ZXIudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFM0MsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sc0NBQXNDLENBQUM7QUFFbEUsT0FBTyxFQUFFLHFCQUFxQixFQUFFLE1BQU0sbUNBQW1DLENBQUM7QUFFMUU7SUFFRSx5QkFBb0IsS0FBWTtRQUFaLFVBQUssR0FBTCxLQUFLLENBQU87SUFBRyxDQUFDOzs7O0lBRXBDLGlDQUFPOzs7SUFBUDs7WUFDUSxJQUFJLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMscUJBQXFCLENBQUMsR0FBRyxDQUFDO1FBQ2pFLE9BQU8sSUFBSSxJQUFJLElBQUksQ0FBQyxNQUFNLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxVQUFVLEVBQUUsQ0FBQyxDQUFDO0lBQzVFLENBQUM7O2dCQVBGLFVBQVU7Ozs7Z0JBTEYsS0FBSzs7SUFhZCxzQkFBQztDQUFBLEFBUkQsSUFRQztTQVBZLGVBQWU7Ozs7OztJQUNkLGdDQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFJlc29sdmUgfSBmcm9tICdAYW5ndWxhci9yb3V0ZXInO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBHZXRUZW5hbnRzIH0gZnJvbSAnLi4vYWN0aW9ucy90ZW5hbnQtbWFuYWdlbWVudC5hY3Rpb25zJztcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMvdGVuYW50LW1hbmFnZW1lbnQnO1xuaW1wb3J0IHsgVGVuYW50TWFuYWdlbWVudFN0YXRlIH0gZnJvbSAnLi4vc3RhdGVzL3RlbmFudC1tYW5hZ2VtZW50LnN0YXRlJztcblxuQEluamVjdGFibGUoKVxuZXhwb3J0IGNsYXNzIFRlbmFudHNSZXNvbHZlciBpbXBsZW1lbnRzIFJlc29sdmU8VGVuYW50TWFuYWdlbWVudC5TdGF0ZT4ge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cblxuICByZXNvbHZlKCkge1xuICAgIGNvbnN0IGRhdGEgPSB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFRlbmFudE1hbmFnZW1lbnRTdGF0ZS5nZXQpO1xuICAgIHJldHVybiBkYXRhICYmIGRhdGEubGVuZ3RoID8gbnVsbCA6IHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IEdldFRlbmFudHMoKSk7XG4gIH1cbn1cbiJdfQ==