/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Action, Selector, State } from '@ngxs/store';
import { switchMap, tap } from 'rxjs/operators';
import { TenantManagementAdd, TenantManagementDelete, TenantManagementGet, TenantManagementGetById, TenantManagementUpdate, } from '../actions/tenant-management.actions';
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
    TenantManagementState.prototype.get = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var patchState = _a.patchState;
        return this.tenantManagementService.get().pipe(tap((/**
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
        return this.tenantManagementService.getById(payload).pipe(tap((/**
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
        return this.tenantManagementService.delete(payload).pipe(switchMap((/**
         * @return {?}
         */
        function () { return dispatch(new TenantManagementGet()); })));
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
        return this.tenantManagementService.add(payload).pipe(switchMap((/**
         * @return {?}
         */
        function () { return dispatch(new TenantManagementGet()); })));
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
        var _this = this;
        var dispatch = _a.dispatch, getState = _a.getState;
        var payload = _b.payload;
        return dispatch(new TenantManagementGetById(payload.id)).pipe(switchMap((/**
         * @return {?}
         */
        function () { return _this.tenantManagementService.update(tslib_1.__assign({}, getState().selectedItem, payload)); })), switchMap((/**
         * @return {?}
         */
        function () { return dispatch(new TenantManagementGet()); })));
    };
    tslib_1.__decorate([
        Action(TenantManagementGet),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object]),
        tslib_1.__metadata("design:returntype", void 0)
    ], TenantManagementState.prototype, "get", null);
    tslib_1.__decorate([
        Action(TenantManagementGetById),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object, TenantManagementGetById]),
        tslib_1.__metadata("design:returntype", void 0)
    ], TenantManagementState.prototype, "getById", null);
    tslib_1.__decorate([
        Action(TenantManagementDelete),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object, TenantManagementDelete]),
        tslib_1.__metadata("design:returntype", void 0)
    ], TenantManagementState.prototype, "delete", null);
    tslib_1.__decorate([
        Action(TenantManagementAdd),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object, TenantManagementAdd]),
        tslib_1.__metadata("design:returntype", void 0)
    ], TenantManagementState.prototype, "add", null);
    tslib_1.__decorate([
        Action(TenantManagementUpdate),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object, TenantManagementUpdate]),
        tslib_1.__metadata("design:returntype", void 0)
    ], TenantManagementState.prototype, "update", null);
    tslib_1.__decorate([
        Selector(),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object]),
        tslib_1.__metadata("design:returntype", Array)
    ], TenantManagementState, "get", null);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQuc3RhdGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy90ZW5hbnQtbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLFNBQVMsRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNoRCxPQUFPLEVBQ0wsbUJBQW1CLEVBQ25CLHNCQUFzQixFQUN0QixtQkFBbUIsRUFDbkIsdUJBQXVCLEVBQ3ZCLHNCQUFzQixHQUN2QixNQUFNLHNDQUFzQyxDQUFDO0FBRTlDLE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxNQUFNLHVDQUF1QyxDQUFDOztJQWE5RSwrQkFBb0IsdUJBQWdEO1FBQWhELDRCQUF1QixHQUF2Qix1QkFBdUIsQ0FBeUI7SUFBRyxDQUFDOzs7OztJQUpqRSx5QkFBRzs7OztJQUFWLFVBQVcsRUFBa0M7WUFBaEMsa0JBQU07UUFDakIsT0FBTyxNQUFNLENBQUMsS0FBSyxJQUFJLEVBQUUsQ0FBQztJQUM1QixDQUFDOzs7OztJQUtELG1DQUFHOzs7O0lBQUgsVUFBSSxFQUFvRDtZQUFsRCwwQkFBVTtRQUNkLE9BQU8sSUFBSSxDQUFDLHVCQUF1QixDQUFDLEdBQUcsRUFBRSxDQUFDLElBQUksQ0FDNUMsR0FBRzs7OztRQUFDLFVBQUEsTUFBTTtZQUNSLE9BQUEsVUFBVSxDQUFDO2dCQUNULE1BQU0sUUFBQTthQUNQLENBQUM7UUFGRixDQUVFLEVBQ0gsQ0FDRixDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0QsdUNBQU87Ozs7O0lBQVAsVUFBUSxFQUFvRCxFQUFFLEVBQW9DO1lBQXhGLDBCQUFVO1lBQTRDLG9CQUFPO1FBQ3JFLE9BQU8sSUFBSSxDQUFDLHVCQUF1QixDQUFDLE9BQU8sQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQ3ZELEdBQUc7Ozs7UUFBQyxVQUFBLFlBQVk7WUFDZCxPQUFBLFVBQVUsQ0FBQztnQkFDVCxZQUFZLGNBQUE7YUFDYixDQUFDO1FBRkYsQ0FFRSxFQUNILENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELHNDQUFNOzs7OztJQUFOLFVBQU8sRUFBa0QsRUFBRSxFQUFtQztZQUFyRixzQkFBUTtZQUE0QyxvQkFBTztRQUNsRSxPQUFPLElBQUksQ0FBQyx1QkFBdUIsQ0FBQyxNQUFNLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUFDLFNBQVM7OztRQUFDLGNBQU0sT0FBQSxRQUFRLENBQUMsSUFBSSxtQkFBbUIsRUFBRSxDQUFDLEVBQW5DLENBQW1DLEVBQUMsQ0FBQyxDQUFDO0lBQ2pILENBQUM7Ozs7OztJQUdELG1DQUFHOzs7OztJQUFILFVBQUksRUFBa0QsRUFBRSxFQUFnQztZQUFsRixzQkFBUTtZQUE0QyxvQkFBTztRQUMvRCxPQUFPLElBQUksQ0FBQyx1QkFBdUIsQ0FBQyxHQUFHLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUFDLFNBQVM7OztRQUFDLGNBQU0sT0FBQSxRQUFRLENBQUMsSUFBSSxtQkFBbUIsRUFBRSxDQUFDLEVBQW5DLENBQW1DLEVBQUMsQ0FBQyxDQUFDO0lBQzlHLENBQUM7Ozs7OztJQUdELHNDQUFNOzs7OztJQUFOLFVBQU8sRUFBNEQsRUFBRSxFQUFtQztRQUR4RyxpQkFNQztZQUxRLHNCQUFRLEVBQUUsc0JBQVE7WUFBNEMsb0JBQU87UUFDNUUsT0FBTyxRQUFRLENBQUMsSUFBSSx1QkFBdUIsQ0FBQyxPQUFPLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQzNELFNBQVM7OztRQUFDLGNBQU0sT0FBQSxLQUFJLENBQUMsdUJBQXVCLENBQUMsTUFBTSxzQkFBTSxRQUFRLEVBQUUsQ0FBQyxZQUFZLEVBQUssT0FBTyxFQUFHLEVBQS9FLENBQStFLEVBQUMsRUFDaEcsU0FBUzs7O1FBQUMsY0FBTSxPQUFBLFFBQVEsQ0FBQyxJQUFJLG1CQUFtQixFQUFFLENBQUMsRUFBbkMsQ0FBbUMsRUFBQyxDQUNyRCxDQUFDO0lBQ0osQ0FBQztJQXJDRDtRQURDLE1BQU0sQ0FBQyxtQkFBbUIsQ0FBQzs7OztvREFTM0I7SUFHRDtRQURDLE1BQU0sQ0FBQyx1QkFBdUIsQ0FBQzs7eURBQzJDLHVCQUF1Qjs7d0RBUWpHO0lBR0Q7UUFEQyxNQUFNLENBQUMsc0JBQXNCLENBQUM7O3lEQUN5QyxzQkFBc0I7O3VEQUU3RjtJQUdEO1FBREMsTUFBTSxDQUFDLG1CQUFtQixDQUFDOzt5REFDeUMsbUJBQW1COztvREFFdkY7SUFHRDtRQURDLE1BQU0sQ0FBQyxzQkFBc0IsQ0FBQzs7eURBQ21ELHNCQUFzQjs7dURBS3ZHO0lBNUNEO1FBREMsUUFBUSxFQUFFOzs7OzBDQUdWO0lBSlUscUJBQXFCO1FBSmpDLEtBQUssQ0FBeUI7WUFDN0IsSUFBSSxFQUFFLHVCQUF1QjtZQUM3QixRQUFRLEVBQUUsbUJBQUEsRUFBRSxNQUFNLEVBQUUsRUFBRSxFQUFFLFlBQVksRUFBRSxFQUFFLEVBQUUsRUFBMEI7U0FDckUsQ0FBQztpREFPNkMsdUJBQXVCO09BTnpELHFCQUFxQixDQStDakM7SUFBRCw0QkFBQztDQUFBLElBQUE7U0EvQ1kscUJBQXFCOzs7Ozs7SUFNcEIsd0RBQXdEIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQWN0aW9uLCBTZWxlY3RvciwgU3RhdGUsIFN0YXRlQ29udGV4dCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IHN3aXRjaE1hcCwgdGFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHtcbiAgVGVuYW50TWFuYWdlbWVudEFkZCxcbiAgVGVuYW50TWFuYWdlbWVudERlbGV0ZSxcbiAgVGVuYW50TWFuYWdlbWVudEdldCxcbiAgVGVuYW50TWFuYWdlbWVudEdldEJ5SWQsXG4gIFRlbmFudE1hbmFnZW1lbnRVcGRhdGUsXG59IGZyb20gJy4uL2FjdGlvbnMvdGVuYW50LW1hbmFnZW1lbnQuYWN0aW9ucyc7XG5pbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50IH0gZnJvbSAnLi4vbW9kZWxzL3RlbmFudC1tYW5hZ2VtZW50JztcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvdGVuYW50LW1hbmFnZW1lbnQuc2VydmljZSc7XG5pbXBvcnQgeyBBQlAgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuXG5AU3RhdGU8VGVuYW50TWFuYWdlbWVudC5TdGF0ZT4oe1xuICBuYW1lOiAnVGVuYW50TWFuYWdlbWVudFN0YXRlJyxcbiAgZGVmYXVsdHM6IHsgcmVzdWx0OiB7fSwgc2VsZWN0ZWRJdGVtOiB7fSB9IGFzIFRlbmFudE1hbmFnZW1lbnQuU3RhdGUsXG59KVxuZXhwb3J0IGNsYXNzIFRlbmFudE1hbmFnZW1lbnRTdGF0ZSB7XG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXQoeyByZXN1bHQgfTogVGVuYW50TWFuYWdlbWVudC5TdGF0ZSk6IEFCUC5CYXNpY0l0ZW1bXSB7XG4gICAgcmV0dXJuIHJlc3VsdC5pdGVtcyB8fCBbXTtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgdGVuYW50TWFuYWdlbWVudFNlcnZpY2U6IFRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlKSB7fVxuXG4gIEBBY3Rpb24oVGVuYW50TWFuYWdlbWVudEdldClcbiAgZ2V0KHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8VGVuYW50TWFuYWdlbWVudC5TdGF0ZT4pIHtcbiAgICByZXR1cm4gdGhpcy50ZW5hbnRNYW5hZ2VtZW50U2VydmljZS5nZXQoKS5waXBlKFxuICAgICAgdGFwKHJlc3VsdCA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICByZXN1bHQsXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihUZW5hbnRNYW5hZ2VtZW50R2V0QnlJZClcbiAgZ2V0QnlJZCh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFRlbmFudE1hbmFnZW1lbnQuU3RhdGU+LCB7IHBheWxvYWQgfTogVGVuYW50TWFuYWdlbWVudEdldEJ5SWQpIHtcbiAgICByZXR1cm4gdGhpcy50ZW5hbnRNYW5hZ2VtZW50U2VydmljZS5nZXRCeUlkKHBheWxvYWQpLnBpcGUoXG4gICAgICB0YXAoc2VsZWN0ZWRJdGVtID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHNlbGVjdGVkSXRlbSxcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKFRlbmFudE1hbmFnZW1lbnREZWxldGUpXG4gIGRlbGV0ZSh7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxUZW5hbnRNYW5hZ2VtZW50LlN0YXRlPiwgeyBwYXlsb2FkIH06IFRlbmFudE1hbmFnZW1lbnREZWxldGUpIHtcbiAgICByZXR1cm4gdGhpcy50ZW5hbnRNYW5hZ2VtZW50U2VydmljZS5kZWxldGUocGF5bG9hZCkucGlwZShzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IFRlbmFudE1hbmFnZW1lbnRHZXQoKSkpKTtcbiAgfVxuXG4gIEBBY3Rpb24oVGVuYW50TWFuYWdlbWVudEFkZClcbiAgYWRkKHsgZGlzcGF0Y2ggfTogU3RhdGVDb250ZXh0PFRlbmFudE1hbmFnZW1lbnQuU3RhdGU+LCB7IHBheWxvYWQgfTogVGVuYW50TWFuYWdlbWVudEFkZCkge1xuICAgIHJldHVybiB0aGlzLnRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlLmFkZChwYXlsb2FkKS5waXBlKHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgVGVuYW50TWFuYWdlbWVudEdldCgpKSkpO1xuICB9XG5cbiAgQEFjdGlvbihUZW5hbnRNYW5hZ2VtZW50VXBkYXRlKVxuICB1cGRhdGUoeyBkaXNwYXRjaCwgZ2V0U3RhdGUgfTogU3RhdGVDb250ZXh0PFRlbmFudE1hbmFnZW1lbnQuU3RhdGU+LCB7IHBheWxvYWQgfTogVGVuYW50TWFuYWdlbWVudFVwZGF0ZSkge1xuICAgIHJldHVybiBkaXNwYXRjaChuZXcgVGVuYW50TWFuYWdlbWVudEdldEJ5SWQocGF5bG9hZC5pZCkpLnBpcGUoXG4gICAgICBzd2l0Y2hNYXAoKCkgPT4gdGhpcy50ZW5hbnRNYW5hZ2VtZW50U2VydmljZS51cGRhdGUoeyAuLi5nZXRTdGF0ZSgpLnNlbGVjdGVkSXRlbSwgLi4ucGF5bG9hZCB9KSksXG4gICAgICBzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IFRlbmFudE1hbmFnZW1lbnRHZXQoKSkpLFxuICAgICk7XG4gIH1cbn1cbiJdfQ==