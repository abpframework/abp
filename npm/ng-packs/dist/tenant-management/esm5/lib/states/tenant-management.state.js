/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
        var _this = this;
        var dispatch = _a.dispatch, getState = _a.getState;
        var payload = _b.payload;
        return dispatch(new GetTenantById(payload.id)).pipe(switchMap((/**
         * @return {?}
         */
        function () { return _this.tenantManagementService.updateTenant(tslib_1.__assign({}, getState().selectedItem, payload)); })), switchMap((/**
         * @return {?}
         */
        function () { return dispatch(new GetTenants()); })));
    };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQuc3RhdGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy90ZW5hbnQtbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLFNBQVMsRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNoRCxPQUFPLEVBQ0wsWUFBWSxFQUNaLFlBQVksRUFDWixVQUFVLEVBQ1YsYUFBYSxFQUNiLFlBQVksR0FDYixNQUFNLHNDQUFzQyxDQUFDO0FBRTlDLE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxNQUFNLHVDQUF1QyxDQUFDOztJQWtCOUUsK0JBQW9CLHVCQUFnRDtRQUFoRCw0QkFBdUIsR0FBdkIsdUJBQXVCLENBQXlCO0lBQUcsQ0FBQzs7Ozs7SUFUakUseUJBQUc7Ozs7SUFBVixVQUFXLEVBQWtDO1lBQWhDLGtCQUFNO1FBQ2pCLE9BQU8sTUFBTSxDQUFDLEtBQUssSUFBSSxFQUFFLENBQUM7SUFDNUIsQ0FBQzs7Ozs7SUFHTSwwQ0FBb0I7Ozs7SUFBM0IsVUFBNEIsRUFBa0M7WUFBaEMsa0JBQU07UUFDbEMsT0FBTyxNQUFNLENBQUMsVUFBVSxDQUFDO0lBQzNCLENBQUM7Ozs7OztJQUtELG1DQUFHOzs7OztJQUFILFVBQUksRUFBb0QsRUFBRSxFQUF1QjtZQUEzRSwwQkFBVTtZQUE0QyxvQkFBTztRQUNqRSxPQUFPLElBQUksQ0FBQyx1QkFBdUIsQ0FBQyxTQUFTLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUN6RCxHQUFHOzs7O1FBQUMsVUFBQSxNQUFNO1lBQ1IsT0FBQSxVQUFVLENBQUM7Z0JBQ1QsTUFBTSxRQUFBO2FBQ1AsQ0FBQztRQUZGLENBRUUsRUFDSCxDQUNGLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCx1Q0FBTzs7Ozs7SUFBUCxVQUFRLEVBQW9ELEVBQUUsRUFBMEI7WUFBOUUsMEJBQVU7WUFBNEMsb0JBQU87UUFDckUsT0FBTyxJQUFJLENBQUMsdUJBQXVCLENBQUMsYUFBYSxDQUFDLE9BQU8sQ0FBQyxDQUFDLElBQUksQ0FDN0QsR0FBRzs7OztRQUFDLFVBQUEsWUFBWTtZQUNkLE9BQUEsVUFBVSxDQUFDO2dCQUNULFlBQVksY0FBQTthQUNiLENBQUM7UUFGRixDQUVFLEVBQ0gsQ0FDRixDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0Qsc0NBQU07Ozs7O0lBQU4sVUFBTyxFQUFrRCxFQUFFLEVBQXlCO1lBQTNFLHNCQUFRO1lBQTRDLG9CQUFPO1FBQ2xFLE9BQU8sSUFBSSxDQUFDLHVCQUF1QixDQUFDLFlBQVksQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQUMsU0FBUzs7O1FBQUMsY0FBTSxPQUFBLFFBQVEsQ0FBQyxJQUFJLFVBQVUsRUFBRSxDQUFDLEVBQTFCLENBQTBCLEVBQUMsQ0FBQyxDQUFDO0lBQzlHLENBQUM7Ozs7OztJQUdELG1DQUFHOzs7OztJQUFILFVBQUksRUFBa0QsRUFBRSxFQUF5QjtZQUEzRSxzQkFBUTtZQUE0QyxvQkFBTztRQUMvRCxPQUFPLElBQUksQ0FBQyx1QkFBdUIsQ0FBQyxZQUFZLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUFDLFNBQVM7OztRQUFDLGNBQU0sT0FBQSxRQUFRLENBQUMsSUFBSSxVQUFVLEVBQUUsQ0FBQyxFQUExQixDQUEwQixFQUFDLENBQUMsQ0FBQztJQUM5RyxDQUFDOzs7Ozs7SUFHRCxzQ0FBTTs7Ozs7SUFBTixVQUFPLEVBQTRELEVBQUUsRUFBeUI7UUFEOUYsaUJBTUM7WUFMUSxzQkFBUSxFQUFFLHNCQUFRO1lBQTRDLG9CQUFPO1FBQzVFLE9BQU8sUUFBUSxDQUFDLElBQUksYUFBYSxDQUFDLE9BQU8sQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FDakQsU0FBUzs7O1FBQUMsY0FBTSxPQUFBLEtBQUksQ0FBQyx1QkFBdUIsQ0FBQyxZQUFZLHNCQUFNLFFBQVEsRUFBRSxDQUFDLFlBQVksRUFBSyxPQUFPLEVBQUcsRUFBckYsQ0FBcUYsRUFBQyxFQUN0RyxTQUFTOzs7UUFBQyxjQUFNLE9BQUEsUUFBUSxDQUFDLElBQUksVUFBVSxFQUFFLENBQUMsRUFBMUIsQ0FBMEIsRUFBQyxDQUM1QyxDQUFDO0lBQ0osQ0FBQztJQXJDRDtRQURDLE1BQU0sQ0FBQyxVQUFVLENBQUM7O3lEQUNvRCxVQUFVOztvREFRaEY7SUFHRDtRQURDLE1BQU0sQ0FBQyxhQUFhLENBQUM7O3lEQUNxRCxhQUFhOzt3REFRdkY7SUFHRDtRQURDLE1BQU0sQ0FBQyxZQUFZLENBQUM7O3lEQUNtRCxZQUFZOzt1REFFbkY7SUFHRDtRQURDLE1BQU0sQ0FBQyxZQUFZLENBQUM7O3lEQUNnRCxZQUFZOztvREFFaEY7SUFHRDtRQURDLE1BQU0sQ0FBQyxZQUFZLENBQUM7O3lEQUM2RCxZQUFZOzt1REFLN0Y7SUFqREQ7UUFEQyxRQUFRLEVBQUU7Ozs7MENBR1Y7SUFHRDtRQURDLFFBQVEsRUFBRTs7OzsyREFHVjtJQVRVLHFCQUFxQjtRQUpqQyxLQUFLLENBQXlCO1lBQzdCLElBQUksRUFBRSx1QkFBdUI7WUFDN0IsUUFBUSxFQUFFLG1CQUFBLEVBQUUsTUFBTSxFQUFFLEVBQUUsRUFBRSxZQUFZLEVBQUUsRUFBRSxFQUFFLEVBQTBCO1NBQ3JFLENBQUM7aURBWTZDLHVCQUF1QjtPQVh6RCxxQkFBcUIsQ0FvRGpDO0lBQUQsNEJBQUM7Q0FBQSxJQUFBO1NBcERZLHFCQUFxQjs7Ozs7O0lBV3BCLHdEQUF3RCIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFjdGlvbiwgU2VsZWN0b3IsIFN0YXRlLCBTdGF0ZUNvbnRleHQgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBzd2l0Y2hNYXAsIHRhcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7XG4gIENyZWF0ZVRlbmFudCxcbiAgRGVsZXRlVGVuYW50LFxuICBHZXRUZW5hbnRzLFxuICBHZXRUZW5hbnRCeUlkLFxuICBVcGRhdGVUZW5hbnQsXG59IGZyb20gJy4uL2FjdGlvbnMvdGVuYW50LW1hbmFnZW1lbnQuYWN0aW9ucyc7XG5pbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50IH0gZnJvbSAnLi4vbW9kZWxzL3RlbmFudC1tYW5hZ2VtZW50JztcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvdGVuYW50LW1hbmFnZW1lbnQuc2VydmljZSc7XG5pbXBvcnQgeyBBQlAgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuXG5AU3RhdGU8VGVuYW50TWFuYWdlbWVudC5TdGF0ZT4oe1xuICBuYW1lOiAnVGVuYW50TWFuYWdlbWVudFN0YXRlJyxcbiAgZGVmYXVsdHM6IHsgcmVzdWx0OiB7fSwgc2VsZWN0ZWRJdGVtOiB7fSB9IGFzIFRlbmFudE1hbmFnZW1lbnQuU3RhdGUsXG59KVxuZXhwb3J0IGNsYXNzIFRlbmFudE1hbmFnZW1lbnRTdGF0ZSB7XG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXQoeyByZXN1bHQgfTogVGVuYW50TWFuYWdlbWVudC5TdGF0ZSk6IEFCUC5CYXNpY0l0ZW1bXSB7XG4gICAgcmV0dXJuIHJlc3VsdC5pdGVtcyB8fCBbXTtcbiAgfVxuXG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRUZW5hbnRzVG90YWxDb3VudCh7IHJlc3VsdCB9OiBUZW5hbnRNYW5hZ2VtZW50LlN0YXRlKTogbnVtYmVyIHtcbiAgICByZXR1cm4gcmVzdWx0LnRvdGFsQ291bnQ7XG4gIH1cblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlOiBUZW5hbnRNYW5hZ2VtZW50U2VydmljZSkge31cblxuICBAQWN0aW9uKEdldFRlbmFudHMpXG4gIGdldCh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFRlbmFudE1hbmFnZW1lbnQuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0VGVuYW50cykge1xuICAgIHJldHVybiB0aGlzLnRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlLmdldFRlbmFudChwYXlsb2FkKS5waXBlKFxuICAgICAgdGFwKHJlc3VsdCA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICByZXN1bHQsXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihHZXRUZW5hbnRCeUlkKVxuICBnZXRCeUlkKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8VGVuYW50TWFuYWdlbWVudC5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRUZW5hbnRCeUlkKSB7XG4gICAgcmV0dXJuIHRoaXMudGVuYW50TWFuYWdlbWVudFNlcnZpY2UuZ2V0VGVuYW50QnlJZChwYXlsb2FkKS5waXBlKFxuICAgICAgdGFwKHNlbGVjdGVkSXRlbSA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICBzZWxlY3RlZEl0ZW0sXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihEZWxldGVUZW5hbnQpXG4gIGRlbGV0ZSh7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxUZW5hbnRNYW5hZ2VtZW50LlN0YXRlPiwgeyBwYXlsb2FkIH06IERlbGV0ZVRlbmFudCkge1xuICAgIHJldHVybiB0aGlzLnRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlLmRlbGV0ZVRlbmFudChwYXlsb2FkKS5waXBlKHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgR2V0VGVuYW50cygpKSkpO1xuICB9XG5cbiAgQEFjdGlvbihDcmVhdGVUZW5hbnQpXG4gIGFkZCh7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxUZW5hbnRNYW5hZ2VtZW50LlN0YXRlPiwgeyBwYXlsb2FkIH06IENyZWF0ZVRlbmFudCkge1xuICAgIHJldHVybiB0aGlzLnRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlLmNyZWF0ZVRlbmFudChwYXlsb2FkKS5waXBlKHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgR2V0VGVuYW50cygpKSkpO1xuICB9XG5cbiAgQEFjdGlvbihVcGRhdGVUZW5hbnQpXG4gIHVwZGF0ZSh7IGRpc3BhdGNoLCBnZXRTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8VGVuYW50TWFuYWdlbWVudC5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBVcGRhdGVUZW5hbnQpIHtcbiAgICByZXR1cm4gZGlzcGF0Y2gobmV3IEdldFRlbmFudEJ5SWQocGF5bG9hZC5pZCkpLnBpcGUoXG4gICAgICBzd2l0Y2hNYXAoKCkgPT4gdGhpcy50ZW5hbnRNYW5hZ2VtZW50U2VydmljZS51cGRhdGVUZW5hbnQoeyAuLi5nZXRTdGF0ZSgpLnNlbGVjdGVkSXRlbSwgLi4ucGF5bG9hZCB9KSksXG4gICAgICBzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IEdldFRlbmFudHMoKSkpLFxuICAgICk7XG4gIH1cbn1cbiJdfQ==