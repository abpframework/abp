/**
 * @fileoverview added by tsickle
 * Generated from: lib/states/tenant-management.state.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Action, Selector, State } from '@ngxs/store';
import { switchMap, tap } from 'rxjs/operators';
import { CreateTenant, DeleteTenant, GetTenants, GetTenantById, UpdateTenant, } from '../actions/tenant-management.actions';
import { TenantManagementService } from '../services/tenant-management.service';
var TenantManagementState = /** @class */ (function () {
    function TenantManagementState(tenantManagementService) {
        this.tenantManagementService = tenantManagementService;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    TenantManagementState.get = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var result = _a.result;
        return result.items || [];
    };
    /**
     * @param {?} __0
     * @return {?}
     */
    TenantManagementState.getTenantsTotalCount = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var result = _a.result;
        return result.totalCount;
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    TenantManagementState.prototype.get = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var patchState = _a.patchState;
        var payload = _b.payload;
        return this.tenantManagementService.getTenant(payload).pipe(tap((/**
         * @param {?} result
         * @return {?}
         */
        function (result) {
            return patchState({
                result: result,
            });
        })));
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    TenantManagementState.prototype.getById = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var patchState = _a.patchState;
        var payload = _b.payload;
        return this.tenantManagementService.getTenantById(payload).pipe(tap((/**
         * @param {?} selectedItem
         * @return {?}
         */
        function (selectedItem) {
            return patchState({
                selectedItem: selectedItem,
            });
        })));
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    TenantManagementState.prototype.delete = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var dispatch = _a.dispatch;
        var payload = _b.payload;
        return this.tenantManagementService.deleteTenant(payload).pipe(switchMap((/**
         * @return {?}
         */
        function () { return dispatch(new GetTenants()); })));
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    TenantManagementState.prototype.add = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var dispatch = _a.dispatch;
        var payload = _b.payload;
        return this.tenantManagementService.createTenant(payload).pipe(switchMap((/**
         * @return {?}
         */
        function () { return dispatch(new GetTenants()); })));
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    TenantManagementState.prototype.update = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var dispatch = _a.dispatch, getState = _a.getState;
        var payload = _b.payload;
        return this.tenantManagementService
            .updateTenant(tslib_1.__assign({}, getState().selectedItem, payload))
            .pipe(switchMap((/**
         * @return {?}
         */
        function () { return dispatch(new GetTenants()); })));
    };
    TenantManagementState.ctorParameters = function () { return [
        { type: TenantManagementService }
    ]; };
    tslib_1.__decorate([
        Action(GetTenants),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object, GetTenants]),
        tslib_1.__metadata("design:returntype", void 0)
    ], TenantManagementState.prototype, "get", null);
    tslib_1.__decorate([
        Action(GetTenantById),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object, GetTenantById]),
        tslib_1.__metadata("design:returntype", void 0)
    ], TenantManagementState.prototype, "getById", null);
    tslib_1.__decorate([
        Action(DeleteTenant),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object, DeleteTenant]),
        tslib_1.__metadata("design:returntype", void 0)
    ], TenantManagementState.prototype, "delete", null);
    tslib_1.__decorate([
        Action(CreateTenant),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object, CreateTenant]),
        tslib_1.__metadata("design:returntype", void 0)
    ], TenantManagementState.prototype, "add", null);
    tslib_1.__decorate([
        Action(UpdateTenant),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object, UpdateTenant]),
        tslib_1.__metadata("design:returntype", void 0)
    ], TenantManagementState.prototype, "update", null);
    tslib_1.__decorate([
        Selector(),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object]),
        tslib_1.__metadata("design:returntype", Array)
    ], TenantManagementState, "get", null);
    tslib_1.__decorate([
        Selector(),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object]),
        tslib_1.__metadata("design:returntype", Number)
    ], TenantManagementState, "getTenantsTotalCount", null);
    TenantManagementState = tslib_1.__decorate([
        State({
            name: 'TenantManagementState',
            defaults: (/** @type {?} */ ({ result: {}, selectedItem: {} })),
        }),
        tslib_1.__metadata("design:paramtypes", [TenantManagementService])
    ], TenantManagementState);
    return TenantManagementState;
}());
export { TenantManagementState };
if (false) {
    /**
     * @type {?}
     * @private
     */
    TenantManagementState.prototype.tenantManagementService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQuc3RhdGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy90ZW5hbnQtbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLFFBQVEsRUFBRSxLQUFLLEVBQWdCLE1BQU0sYUFBYSxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxTQUFTLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDaEQsT0FBTyxFQUNMLFlBQVksRUFDWixZQUFZLEVBQ1osVUFBVSxFQUNWLGFBQWEsRUFDYixZQUFZLEdBQ2IsTUFBTSxzQ0FBc0MsQ0FBQztBQUU5QyxPQUFPLEVBQUUsdUJBQXVCLEVBQUUsTUFBTSx1Q0FBdUMsQ0FBQzs7SUFrQjlFLCtCQUFvQix1QkFBZ0Q7UUFBaEQsNEJBQXVCLEdBQXZCLHVCQUF1QixDQUF5QjtJQUFHLENBQUM7Ozs7O0lBVGpFLHlCQUFHOzs7O0lBQVYsVUFBVyxFQUFrQztZQUFoQyxrQkFBTTtRQUNqQixPQUFPLE1BQU0sQ0FBQyxLQUFLLElBQUksRUFBRSxDQUFDO0lBQzVCLENBQUM7Ozs7O0lBR00sMENBQW9COzs7O0lBQTNCLFVBQTRCLEVBQWtDO1lBQWhDLGtCQUFNO1FBQ2xDLE9BQU8sTUFBTSxDQUFDLFVBQVUsQ0FBQztJQUMzQixDQUFDOzs7Ozs7SUFLRCxtQ0FBRzs7Ozs7SUFBSCxVQUFJLEVBQW9ELEVBQUUsRUFBdUI7WUFBM0UsMEJBQVU7WUFBNEMsb0JBQU87UUFDakUsT0FBTyxJQUFJLENBQUMsdUJBQXVCLENBQUMsU0FBUyxDQUFDLE9BQU8sQ0FBQyxDQUFDLElBQUksQ0FDekQsR0FBRzs7OztRQUFDLFVBQUEsTUFBTTtZQUNSLE9BQUEsVUFBVSxDQUFDO2dCQUNULE1BQU0sUUFBQTthQUNQLENBQUM7UUFGRixDQUVFLEVBQ0gsQ0FDRixDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0QsdUNBQU87Ozs7O0lBQVAsVUFBUSxFQUFvRCxFQUFFLEVBQTBCO1lBQTlFLDBCQUFVO1lBQTRDLG9CQUFPO1FBQ3JFLE9BQU8sSUFBSSxDQUFDLHVCQUF1QixDQUFDLGFBQWEsQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQzdELEdBQUc7Ozs7UUFBQyxVQUFBLFlBQVk7WUFDZCxPQUFBLFVBQVUsQ0FBQztnQkFDVCxZQUFZLGNBQUE7YUFDYixDQUFDO1FBRkYsQ0FFRSxFQUNILENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELHNDQUFNOzs7OztJQUFOLFVBQU8sRUFBa0QsRUFBRSxFQUF5QjtZQUEzRSxzQkFBUTtZQUE0QyxvQkFBTztRQUNsRSxPQUFPLElBQUksQ0FBQyx1QkFBdUIsQ0FBQyxZQUFZLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUFDLFNBQVM7OztRQUFDLGNBQU0sT0FBQSxRQUFRLENBQUMsSUFBSSxVQUFVLEVBQUUsQ0FBQyxFQUExQixDQUEwQixFQUFDLENBQUMsQ0FBQztJQUM5RyxDQUFDOzs7Ozs7SUFHRCxtQ0FBRzs7Ozs7SUFBSCxVQUFJLEVBQWtELEVBQUUsRUFBeUI7WUFBM0Usc0JBQVE7WUFBNEMsb0JBQU87UUFDL0QsT0FBTyxJQUFJLENBQUMsdUJBQXVCLENBQUMsWUFBWSxDQUFDLE9BQU8sQ0FBQyxDQUFDLElBQUksQ0FBQyxTQUFTOzs7UUFBQyxjQUFNLE9BQUEsUUFBUSxDQUFDLElBQUksVUFBVSxFQUFFLENBQUMsRUFBMUIsQ0FBMEIsRUFBQyxDQUFDLENBQUM7SUFDOUcsQ0FBQzs7Ozs7O0lBR0Qsc0NBQU07Ozs7O0lBQU4sVUFBTyxFQUE0RCxFQUFFLEVBQXlCO1lBQXJGLHNCQUFRLEVBQUUsc0JBQVE7WUFBNEMsb0JBQU87UUFDNUUsT0FBTyxJQUFJLENBQUMsdUJBQXVCO2FBQ2hDLFlBQVksc0JBQU0sUUFBUSxFQUFFLENBQUMsWUFBWSxFQUFLLE9BQU8sRUFBRzthQUN4RCxJQUFJLENBQUMsU0FBUzs7O1FBQUMsY0FBTSxPQUFBLFFBQVEsQ0FBQyxJQUFJLFVBQVUsRUFBRSxDQUFDLEVBQTFCLENBQTBCLEVBQUMsQ0FBQyxDQUFDO0lBQ3ZELENBQUM7O2dCQXZDNEMsdUJBQXVCOztJQUdwRTtRQURDLE1BQU0sQ0FBQyxVQUFVLENBQUM7O3lEQUNvRCxVQUFVOztvREFRaEY7SUFHRDtRQURDLE1BQU0sQ0FBQyxhQUFhLENBQUM7O3lEQUNxRCxhQUFhOzt3REFRdkY7SUFHRDtRQURDLE1BQU0sQ0FBQyxZQUFZLENBQUM7O3lEQUNtRCxZQUFZOzt1REFFbkY7SUFHRDtRQURDLE1BQU0sQ0FBQyxZQUFZLENBQUM7O3lEQUNnRCxZQUFZOztvREFFaEY7SUFHRDtRQURDLE1BQU0sQ0FBQyxZQUFZLENBQUM7O3lEQUM2RCxZQUFZOzt1REFJN0Y7SUFoREQ7UUFEQyxRQUFRLEVBQUU7Ozs7MENBR1Y7SUFHRDtRQURDLFFBQVEsRUFBRTs7OzsyREFHVjtJQVRVLHFCQUFxQjtRQUpqQyxLQUFLLENBQXlCO1lBQzdCLElBQUksRUFBRSx1QkFBdUI7WUFDN0IsUUFBUSxFQUFFLG1CQUFBLEVBQUUsTUFBTSxFQUFFLEVBQUUsRUFBRSxZQUFZLEVBQUUsRUFBRSxFQUFFLEVBQTBCO1NBQ3JFLENBQUM7aURBWTZDLHVCQUF1QjtPQVh6RCxxQkFBcUIsQ0FtRGpDO0lBQUQsNEJBQUM7Q0FBQSxJQUFBO1NBbkRZLHFCQUFxQjs7Ozs7O0lBV3BCLHdEQUF3RCIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFjdGlvbiwgU2VsZWN0b3IsIFN0YXRlLCBTdGF0ZUNvbnRleHQgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IHN3aXRjaE1hcCwgdGFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5pbXBvcnQge1xyXG4gIENyZWF0ZVRlbmFudCxcclxuICBEZWxldGVUZW5hbnQsXHJcbiAgR2V0VGVuYW50cyxcclxuICBHZXRUZW5hbnRCeUlkLFxyXG4gIFVwZGF0ZVRlbmFudCxcclxufSBmcm9tICcuLi9hY3Rpb25zL3RlbmFudC1tYW5hZ2VtZW50LmFjdGlvbnMnO1xyXG5pbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50IH0gZnJvbSAnLi4vbW9kZWxzL3RlbmFudC1tYW5hZ2VtZW50JztcclxuaW1wb3J0IHsgVGVuYW50TWFuYWdlbWVudFNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy90ZW5hbnQtbWFuYWdlbWVudC5zZXJ2aWNlJztcclxuaW1wb3J0IHsgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuXHJcbkBTdGF0ZTxUZW5hbnRNYW5hZ2VtZW50LlN0YXRlPih7XHJcbiAgbmFtZTogJ1RlbmFudE1hbmFnZW1lbnRTdGF0ZScsXHJcbiAgZGVmYXVsdHM6IHsgcmVzdWx0OiB7fSwgc2VsZWN0ZWRJdGVtOiB7fSB9IGFzIFRlbmFudE1hbmFnZW1lbnQuU3RhdGUsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBUZW5hbnRNYW5hZ2VtZW50U3RhdGUge1xyXG4gIEBTZWxlY3RvcigpXHJcbiAgc3RhdGljIGdldCh7IHJlc3VsdCB9OiBUZW5hbnRNYW5hZ2VtZW50LlN0YXRlKTogQUJQLkJhc2ljSXRlbVtdIHtcclxuICAgIHJldHVybiByZXN1bHQuaXRlbXMgfHwgW107XHJcbiAgfVxyXG5cclxuICBAU2VsZWN0b3IoKVxyXG4gIHN0YXRpYyBnZXRUZW5hbnRzVG90YWxDb3VudCh7IHJlc3VsdCB9OiBUZW5hbnRNYW5hZ2VtZW50LlN0YXRlKTogbnVtYmVyIHtcclxuICAgIHJldHVybiByZXN1bHQudG90YWxDb3VudDtcclxuICB9XHJcblxyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgdGVuYW50TWFuYWdlbWVudFNlcnZpY2U6IFRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlKSB7fVxyXG5cclxuICBAQWN0aW9uKEdldFRlbmFudHMpXHJcbiAgZ2V0KHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8VGVuYW50TWFuYWdlbWVudC5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRUZW5hbnRzKSB7XHJcbiAgICByZXR1cm4gdGhpcy50ZW5hbnRNYW5hZ2VtZW50U2VydmljZS5nZXRUZW5hbnQocGF5bG9hZCkucGlwZShcclxuICAgICAgdGFwKHJlc3VsdCA9PlxyXG4gICAgICAgIHBhdGNoU3RhdGUoe1xyXG4gICAgICAgICAgcmVzdWx0LFxyXG4gICAgICAgIH0pLFxyXG4gICAgICApLFxyXG4gICAgKTtcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oR2V0VGVuYW50QnlJZClcclxuICBnZXRCeUlkKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8VGVuYW50TWFuYWdlbWVudC5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRUZW5hbnRCeUlkKSB7XHJcbiAgICByZXR1cm4gdGhpcy50ZW5hbnRNYW5hZ2VtZW50U2VydmljZS5nZXRUZW5hbnRCeUlkKHBheWxvYWQpLnBpcGUoXHJcbiAgICAgIHRhcChzZWxlY3RlZEl0ZW0gPT5cclxuICAgICAgICBwYXRjaFN0YXRlKHtcclxuICAgICAgICAgIHNlbGVjdGVkSXRlbSxcclxuICAgICAgICB9KSxcclxuICAgICAgKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKERlbGV0ZVRlbmFudClcclxuICBkZWxldGUoeyBkaXNwYXRjaCB9OiBTdGF0ZUNvbnRleHQ8VGVuYW50TWFuYWdlbWVudC5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBEZWxldGVUZW5hbnQpIHtcclxuICAgIHJldHVybiB0aGlzLnRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlLmRlbGV0ZVRlbmFudChwYXlsb2FkKS5waXBlKHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgR2V0VGVuYW50cygpKSkpO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihDcmVhdGVUZW5hbnQpXHJcbiAgYWRkKHsgZGlzcGF0Y2ggfTogU3RhdGVDb250ZXh0PFRlbmFudE1hbmFnZW1lbnQuU3RhdGU+LCB7IHBheWxvYWQgfTogQ3JlYXRlVGVuYW50KSB7XHJcbiAgICByZXR1cm4gdGhpcy50ZW5hbnRNYW5hZ2VtZW50U2VydmljZS5jcmVhdGVUZW5hbnQocGF5bG9hZCkucGlwZShzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IEdldFRlbmFudHMoKSkpKTtcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oVXBkYXRlVGVuYW50KVxyXG4gIHVwZGF0ZSh7IGRpc3BhdGNoLCBnZXRTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8VGVuYW50TWFuYWdlbWVudC5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBVcGRhdGVUZW5hbnQpIHtcclxuICAgIHJldHVybiB0aGlzLnRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlXHJcbiAgICAgIC51cGRhdGVUZW5hbnQoeyAuLi5nZXRTdGF0ZSgpLnNlbGVjdGVkSXRlbSwgLi4ucGF5bG9hZCB9KVxyXG4gICAgICAucGlwZShzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IEdldFRlbmFudHMoKSkpKTtcclxuICB9XHJcbn1cclxuIl19