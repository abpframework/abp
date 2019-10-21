/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQuc3RhdGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy90ZW5hbnQtbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLFNBQVMsRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNoRCxPQUFPLEVBQ0wsWUFBWSxFQUNaLFlBQVksRUFDWixVQUFVLEVBQ1YsYUFBYSxFQUNiLFlBQVksR0FDYixNQUFNLHNDQUFzQyxDQUFDO0FBRTlDLE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxNQUFNLHVDQUF1QyxDQUFDOztJQWtCOUUsK0JBQW9CLHVCQUFnRDtRQUFoRCw0QkFBdUIsR0FBdkIsdUJBQXVCLENBQXlCO0lBQUcsQ0FBQzs7Ozs7SUFUakUseUJBQUc7Ozs7SUFBVixVQUFXLEVBQWtDO1lBQWhDLGtCQUFNO1FBQ2pCLE9BQU8sTUFBTSxDQUFDLEtBQUssSUFBSSxFQUFFLENBQUM7SUFDNUIsQ0FBQzs7Ozs7SUFHTSwwQ0FBb0I7Ozs7SUFBM0IsVUFBNEIsRUFBa0M7WUFBaEMsa0JBQU07UUFDbEMsT0FBTyxNQUFNLENBQUMsVUFBVSxDQUFDO0lBQzNCLENBQUM7Ozs7OztJQUtELG1DQUFHOzs7OztJQUFILFVBQUksRUFBb0QsRUFBRSxFQUF1QjtZQUEzRSwwQkFBVTtZQUE0QyxvQkFBTztRQUNqRSxPQUFPLElBQUksQ0FBQyx1QkFBdUIsQ0FBQyxTQUFTLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUN6RCxHQUFHOzs7O1FBQUMsVUFBQSxNQUFNO1lBQ1IsT0FBQSxVQUFVLENBQUM7Z0JBQ1QsTUFBTSxRQUFBO2FBQ1AsQ0FBQztRQUZGLENBRUUsRUFDSCxDQUNGLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCx1Q0FBTzs7Ozs7SUFBUCxVQUFRLEVBQW9ELEVBQUUsRUFBMEI7WUFBOUUsMEJBQVU7WUFBNEMsb0JBQU87UUFDckUsT0FBTyxJQUFJLENBQUMsdUJBQXVCLENBQUMsYUFBYSxDQUFDLE9BQU8sQ0FBQyxDQUFDLElBQUksQ0FDN0QsR0FBRzs7OztRQUFDLFVBQUEsWUFBWTtZQUNkLE9BQUEsVUFBVSxDQUFDO2dCQUNULFlBQVksY0FBQTthQUNiLENBQUM7UUFGRixDQUVFLEVBQ0gsQ0FDRixDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0Qsc0NBQU07Ozs7O0lBQU4sVUFBTyxFQUFrRCxFQUFFLEVBQXlCO1lBQTNFLHNCQUFRO1lBQTRDLG9CQUFPO1FBQ2xFLE9BQU8sSUFBSSxDQUFDLHVCQUF1QixDQUFDLFlBQVksQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQUMsU0FBUzs7O1FBQUMsY0FBTSxPQUFBLFFBQVEsQ0FBQyxJQUFJLFVBQVUsRUFBRSxDQUFDLEVBQTFCLENBQTBCLEVBQUMsQ0FBQyxDQUFDO0lBQzlHLENBQUM7Ozs7OztJQUdELG1DQUFHOzs7OztJQUFILFVBQUksRUFBa0QsRUFBRSxFQUF5QjtZQUEzRSxzQkFBUTtZQUE0QyxvQkFBTztRQUMvRCxPQUFPLElBQUksQ0FBQyx1QkFBdUIsQ0FBQyxZQUFZLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUFDLFNBQVM7OztRQUFDLGNBQU0sT0FBQSxRQUFRLENBQUMsSUFBSSxVQUFVLEVBQUUsQ0FBQyxFQUExQixDQUEwQixFQUFDLENBQUMsQ0FBQztJQUM5RyxDQUFDOzs7Ozs7SUFHRCxzQ0FBTTs7Ozs7SUFBTixVQUFPLEVBQTRELEVBQUUsRUFBeUI7UUFEOUYsaUJBTUM7WUFMUSxzQkFBUSxFQUFFLHNCQUFRO1lBQTRDLG9CQUFPO1FBQzVFLE9BQU8sUUFBUSxDQUFDLElBQUksYUFBYSxDQUFDLE9BQU8sQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FDakQsU0FBUzs7O1FBQUMsY0FBTSxPQUFBLEtBQUksQ0FBQyx1QkFBdUIsQ0FBQyxZQUFZLHNCQUFNLFFBQVEsRUFBRSxDQUFDLFlBQVksRUFBSyxPQUFPLEVBQUcsRUFBckYsQ0FBcUYsRUFBQyxFQUN0RyxTQUFTOzs7UUFBQyxjQUFNLE9BQUEsUUFBUSxDQUFDLElBQUksVUFBVSxFQUFFLENBQUMsRUFBMUIsQ0FBMEIsRUFBQyxDQUM1QyxDQUFDO0lBQ0osQ0FBQzs7Z0JBeEM0Qyx1QkFBdUI7O0lBR3BFO1FBREMsTUFBTSxDQUFDLFVBQVUsQ0FBQzs7eURBQ29ELFVBQVU7O29EQVFoRjtJQUdEO1FBREMsTUFBTSxDQUFDLGFBQWEsQ0FBQzs7eURBQ3FELGFBQWE7O3dEQVF2RjtJQUdEO1FBREMsTUFBTSxDQUFDLFlBQVksQ0FBQzs7eURBQ21ELFlBQVk7O3VEQUVuRjtJQUdEO1FBREMsTUFBTSxDQUFDLFlBQVksQ0FBQzs7eURBQ2dELFlBQVk7O29EQUVoRjtJQUdEO1FBREMsTUFBTSxDQUFDLFlBQVksQ0FBQzs7eURBQzZELFlBQVk7O3VEQUs3RjtJQWpERDtRQURDLFFBQVEsRUFBRTs7OzswQ0FHVjtJQUdEO1FBREMsUUFBUSxFQUFFOzs7OzJEQUdWO0lBVFUscUJBQXFCO1FBSmpDLEtBQUssQ0FBeUI7WUFDN0IsSUFBSSxFQUFFLHVCQUF1QjtZQUM3QixRQUFRLEVBQUUsbUJBQUEsRUFBRSxNQUFNLEVBQUUsRUFBRSxFQUFFLFlBQVksRUFBRSxFQUFFLEVBQUUsRUFBMEI7U0FDckUsQ0FBQztpREFZNkMsdUJBQXVCO09BWHpELHFCQUFxQixDQW9EakM7SUFBRCw0QkFBQztDQUFBLElBQUE7U0FwRFkscUJBQXFCOzs7Ozs7SUFXcEIsd0RBQXdEIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQWN0aW9uLCBTZWxlY3RvciwgU3RhdGUsIFN0YXRlQ29udGV4dCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IHN3aXRjaE1hcCwgdGFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHtcbiAgQ3JlYXRlVGVuYW50LFxuICBEZWxldGVUZW5hbnQsXG4gIEdldFRlbmFudHMsXG4gIEdldFRlbmFudEJ5SWQsXG4gIFVwZGF0ZVRlbmFudCxcbn0gZnJvbSAnLi4vYWN0aW9ucy90ZW5hbnQtbWFuYWdlbWVudC5hY3Rpb25zJztcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMvdGVuYW50LW1hbmFnZW1lbnQnO1xuaW1wb3J0IHsgVGVuYW50TWFuYWdlbWVudFNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy90ZW5hbnQtbWFuYWdlbWVudC5zZXJ2aWNlJztcbmltcG9ydCB7IEFCUCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5cbkBTdGF0ZTxUZW5hbnRNYW5hZ2VtZW50LlN0YXRlPih7XG4gIG5hbWU6ICdUZW5hbnRNYW5hZ2VtZW50U3RhdGUnLFxuICBkZWZhdWx0czogeyByZXN1bHQ6IHt9LCBzZWxlY3RlZEl0ZW06IHt9IH0gYXMgVGVuYW50TWFuYWdlbWVudC5TdGF0ZSxcbn0pXG5leHBvcnQgY2xhc3MgVGVuYW50TWFuYWdlbWVudFN0YXRlIHtcbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldCh7IHJlc3VsdCB9OiBUZW5hbnRNYW5hZ2VtZW50LlN0YXRlKTogQUJQLkJhc2ljSXRlbVtdIHtcbiAgICByZXR1cm4gcmVzdWx0Lml0ZW1zIHx8IFtdO1xuICB9XG5cbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldFRlbmFudHNUb3RhbENvdW50KHsgcmVzdWx0IH06IFRlbmFudE1hbmFnZW1lbnQuU3RhdGUpOiBudW1iZXIge1xuICAgIHJldHVybiByZXN1bHQudG90YWxDb3VudDtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgdGVuYW50TWFuYWdlbWVudFNlcnZpY2U6IFRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlKSB7fVxuXG4gIEBBY3Rpb24oR2V0VGVuYW50cylcbiAgZ2V0KHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8VGVuYW50TWFuYWdlbWVudC5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRUZW5hbnRzKSB7XG4gICAgcmV0dXJuIHRoaXMudGVuYW50TWFuYWdlbWVudFNlcnZpY2UuZ2V0VGVuYW50KHBheWxvYWQpLnBpcGUoXG4gICAgICB0YXAocmVzdWx0ID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHJlc3VsdCxcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKEdldFRlbmFudEJ5SWQpXG4gIGdldEJ5SWQoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxUZW5hbnRNYW5hZ2VtZW50LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFRlbmFudEJ5SWQpIHtcbiAgICByZXR1cm4gdGhpcy50ZW5hbnRNYW5hZ2VtZW50U2VydmljZS5nZXRUZW5hbnRCeUlkKHBheWxvYWQpLnBpcGUoXG4gICAgICB0YXAoc2VsZWN0ZWRJdGVtID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHNlbGVjdGVkSXRlbSxcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKERlbGV0ZVRlbmFudClcbiAgZGVsZXRlKHsgZGlzcGF0Y2ggfTogU3RhdGVDb250ZXh0PFRlbmFudE1hbmFnZW1lbnQuU3RhdGU+LCB7IHBheWxvYWQgfTogRGVsZXRlVGVuYW50KSB7XG4gICAgcmV0dXJuIHRoaXMudGVuYW50TWFuYWdlbWVudFNlcnZpY2UuZGVsZXRlVGVuYW50KHBheWxvYWQpLnBpcGUoc3dpdGNoTWFwKCgpID0+IGRpc3BhdGNoKG5ldyBHZXRUZW5hbnRzKCkpKSk7XG4gIH1cblxuICBAQWN0aW9uKENyZWF0ZVRlbmFudClcbiAgYWRkKHsgZGlzcGF0Y2ggfTogU3RhdGVDb250ZXh0PFRlbmFudE1hbmFnZW1lbnQuU3RhdGU+LCB7IHBheWxvYWQgfTogQ3JlYXRlVGVuYW50KSB7XG4gICAgcmV0dXJuIHRoaXMudGVuYW50TWFuYWdlbWVudFNlcnZpY2UuY3JlYXRlVGVuYW50KHBheWxvYWQpLnBpcGUoc3dpdGNoTWFwKCgpID0+IGRpc3BhdGNoKG5ldyBHZXRUZW5hbnRzKCkpKSk7XG4gIH1cblxuICBAQWN0aW9uKFVwZGF0ZVRlbmFudClcbiAgdXBkYXRlKHsgZGlzcGF0Y2gsIGdldFN0YXRlIH06IFN0YXRlQ29udGV4dDxUZW5hbnRNYW5hZ2VtZW50LlN0YXRlPiwgeyBwYXlsb2FkIH06IFVwZGF0ZVRlbmFudCkge1xuICAgIHJldHVybiBkaXNwYXRjaChuZXcgR2V0VGVuYW50QnlJZChwYXlsb2FkLmlkKSkucGlwZShcbiAgICAgIHN3aXRjaE1hcCgoKSA9PiB0aGlzLnRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlLnVwZGF0ZVRlbmFudCh7IC4uLmdldFN0YXRlKCkuc2VsZWN0ZWRJdGVtLCAuLi5wYXlsb2FkIH0pKSxcbiAgICAgIHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgR2V0VGVuYW50cygpKSksXG4gICAgKTtcbiAgfVxufVxuIl19