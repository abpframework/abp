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
let TenantManagementState = class TenantManagementState {
    /**
     * @param {?} tenantManagementService
     */
    constructor(tenantManagementService) {
        this.tenantManagementService = tenantManagementService;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static get({ result }) {
        return result.items || [];
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getTenantsTotalCount({ result }) {
        return result.totalCount;
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    get({ patchState }, { payload }) {
        return this.tenantManagementService.getTenant(payload).pipe(tap((/**
         * @param {?} result
         * @return {?}
         */
        result => patchState({
            result,
        }))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    getById({ patchState }, { payload }) {
        return this.tenantManagementService.getTenantById(payload).pipe(tap((/**
         * @param {?} selectedItem
         * @return {?}
         */
        selectedItem => patchState({
            selectedItem,
        }))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    delete({ dispatch }, { payload }) {
        return this.tenantManagementService.deleteTenant(payload).pipe(switchMap((/**
         * @return {?}
         */
        () => dispatch(new GetTenants()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    add({ dispatch }, { payload }) {
        return this.tenantManagementService.createTenant(payload).pipe(switchMap((/**
         * @return {?}
         */
        () => dispatch(new GetTenants()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    update({ dispatch, getState }, { payload }) {
        return this.tenantManagementService
            .updateTenant(Object.assign({}, getState().selectedItem, payload))
            .pipe(switchMap((/**
         * @return {?}
         */
        () => dispatch(new GetTenants()))));
    }
};
TenantManagementState.ctorParameters = () => [
    { type: TenantManagementService }
];
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
export { TenantManagementState };
if (false) {
    /**
     * @type {?}
     * @private
     */
    TenantManagementState.prototype.tenantManagementService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQuc3RhdGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy90ZW5hbnQtbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLFFBQVEsRUFBRSxLQUFLLEVBQWdCLE1BQU0sYUFBYSxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxTQUFTLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDaEQsT0FBTyxFQUNMLFlBQVksRUFDWixZQUFZLEVBQ1osVUFBVSxFQUNWLGFBQWEsRUFDYixZQUFZLEdBQ2IsTUFBTSxzQ0FBc0MsQ0FBQztBQUU5QyxPQUFPLEVBQUUsdUJBQXVCLEVBQUUsTUFBTSx1Q0FBdUMsQ0FBQztJQU9uRSxxQkFBcUIsU0FBckIscUJBQXFCOzs7O0lBV2hDLFlBQW9CLHVCQUFnRDtRQUFoRCw0QkFBdUIsR0FBdkIsdUJBQXVCLENBQXlCO0lBQUcsQ0FBQzs7Ozs7SUFUeEUsTUFBTSxDQUFDLEdBQUcsQ0FBQyxFQUFFLE1BQU0sRUFBMEI7UUFDM0MsT0FBTyxNQUFNLENBQUMsS0FBSyxJQUFJLEVBQUUsQ0FBQztJQUM1QixDQUFDOzs7OztJQUdELE1BQU0sQ0FBQyxvQkFBb0IsQ0FBQyxFQUFFLE1BQU0sRUFBMEI7UUFDNUQsT0FBTyxNQUFNLENBQUMsVUFBVSxDQUFDO0lBQzNCLENBQUM7Ozs7OztJQUtELEdBQUcsQ0FBQyxFQUFFLFVBQVUsRUFBd0MsRUFBRSxFQUFFLE9BQU8sRUFBYztRQUMvRSxPQUFPLElBQUksQ0FBQyx1QkFBdUIsQ0FBQyxTQUFTLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUN6RCxHQUFHOzs7O1FBQUMsTUFBTSxDQUFDLEVBQUUsQ0FDWCxVQUFVLENBQUM7WUFDVCxNQUFNO1NBQ1AsQ0FBQyxFQUNILENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELE9BQU8sQ0FBQyxFQUFFLFVBQVUsRUFBd0MsRUFBRSxFQUFFLE9BQU8sRUFBaUI7UUFDdEYsT0FBTyxJQUFJLENBQUMsdUJBQXVCLENBQUMsYUFBYSxDQUFDLE9BQU8sQ0FBQyxDQUFDLElBQUksQ0FDN0QsR0FBRzs7OztRQUFDLFlBQVksQ0FBQyxFQUFFLENBQ2pCLFVBQVUsQ0FBQztZQUNULFlBQVk7U0FDYixDQUFDLEVBQ0gsQ0FDRixDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0QsTUFBTSxDQUFDLEVBQUUsUUFBUSxFQUF3QyxFQUFFLEVBQUUsT0FBTyxFQUFnQjtRQUNsRixPQUFPLElBQUksQ0FBQyx1QkFBdUIsQ0FBQyxZQUFZLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUFDLFNBQVM7OztRQUFDLEdBQUcsRUFBRSxDQUFDLFFBQVEsQ0FBQyxJQUFJLFVBQVUsRUFBRSxDQUFDLEVBQUMsQ0FBQyxDQUFDO0lBQzlHLENBQUM7Ozs7OztJQUdELEdBQUcsQ0FBQyxFQUFFLFFBQVEsRUFBd0MsRUFBRSxFQUFFLE9BQU8sRUFBZ0I7UUFDL0UsT0FBTyxJQUFJLENBQUMsdUJBQXVCLENBQUMsWUFBWSxDQUFDLE9BQU8sQ0FBQyxDQUFDLElBQUksQ0FBQyxTQUFTOzs7UUFBQyxHQUFHLEVBQUUsQ0FBQyxRQUFRLENBQUMsSUFBSSxVQUFVLEVBQUUsQ0FBQyxFQUFDLENBQUMsQ0FBQztJQUM5RyxDQUFDOzs7Ozs7SUFHRCxNQUFNLENBQUMsRUFBRSxRQUFRLEVBQUUsUUFBUSxFQUF3QyxFQUFFLEVBQUUsT0FBTyxFQUFnQjtRQUM1RixPQUFPLElBQUksQ0FBQyx1QkFBdUI7YUFDaEMsWUFBWSxtQkFBTSxRQUFRLEVBQUUsQ0FBQyxZQUFZLEVBQUssT0FBTyxFQUFHO2FBQ3hELElBQUksQ0FBQyxTQUFTOzs7UUFBQyxHQUFHLEVBQUUsQ0FBQyxRQUFRLENBQUMsSUFBSSxVQUFVLEVBQUUsQ0FBQyxFQUFDLENBQUMsQ0FBQztJQUN2RCxDQUFDO0NBQ0YsQ0FBQTs7WUF4QzhDLHVCQUF1Qjs7QUFHcEU7SUFEQyxNQUFNLENBQUMsVUFBVSxDQUFDOztxREFDb0QsVUFBVTs7Z0RBUWhGO0FBR0Q7SUFEQyxNQUFNLENBQUMsYUFBYSxDQUFDOztxREFDcUQsYUFBYTs7b0RBUXZGO0FBR0Q7SUFEQyxNQUFNLENBQUMsWUFBWSxDQUFDOztxREFDbUQsWUFBWTs7bURBRW5GO0FBR0Q7SUFEQyxNQUFNLENBQUMsWUFBWSxDQUFDOztxREFDZ0QsWUFBWTs7Z0RBRWhGO0FBR0Q7SUFEQyxNQUFNLENBQUMsWUFBWSxDQUFDOztxREFDNkQsWUFBWTs7bURBSTdGO0FBaEREO0lBREMsUUFBUSxFQUFFOzs7O3NDQUdWO0FBR0Q7SUFEQyxRQUFRLEVBQUU7Ozs7dURBR1Y7QUFUVSxxQkFBcUI7SUFKakMsS0FBSyxDQUF5QjtRQUM3QixJQUFJLEVBQUUsdUJBQXVCO1FBQzdCLFFBQVEsRUFBRSxtQkFBQSxFQUFFLE1BQU0sRUFBRSxFQUFFLEVBQUUsWUFBWSxFQUFFLEVBQUUsRUFBRSxFQUEwQjtLQUNyRSxDQUFDOzZDQVk2Qyx1QkFBdUI7R0FYekQscUJBQXFCLENBbURqQztTQW5EWSxxQkFBcUI7Ozs7OztJQVdwQix3REFBd0QiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBY3Rpb24sIFNlbGVjdG9yLCBTdGF0ZSwgU3RhdGVDb250ZXh0IH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBzd2l0Y2hNYXAsIHRhcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcclxuaW1wb3J0IHtcclxuICBDcmVhdGVUZW5hbnQsXHJcbiAgRGVsZXRlVGVuYW50LFxyXG4gIEdldFRlbmFudHMsXHJcbiAgR2V0VGVuYW50QnlJZCxcclxuICBVcGRhdGVUZW5hbnQsXHJcbn0gZnJvbSAnLi4vYWN0aW9ucy90ZW5hbnQtbWFuYWdlbWVudC5hY3Rpb25zJztcclxuaW1wb3J0IHsgVGVuYW50TWFuYWdlbWVudCB9IGZyb20gJy4uL21vZGVscy90ZW5hbnQtbWFuYWdlbWVudCc7XHJcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvdGVuYW50LW1hbmFnZW1lbnQuc2VydmljZSc7XHJcbmltcG9ydCB7IEFCUCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcblxyXG5AU3RhdGU8VGVuYW50TWFuYWdlbWVudC5TdGF0ZT4oe1xyXG4gIG5hbWU6ICdUZW5hbnRNYW5hZ2VtZW50U3RhdGUnLFxyXG4gIGRlZmF1bHRzOiB7IHJlc3VsdDoge30sIHNlbGVjdGVkSXRlbToge30gfSBhcyBUZW5hbnRNYW5hZ2VtZW50LlN0YXRlLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgVGVuYW50TWFuYWdlbWVudFN0YXRlIHtcclxuICBAU2VsZWN0b3IoKVxyXG4gIHN0YXRpYyBnZXQoeyByZXN1bHQgfTogVGVuYW50TWFuYWdlbWVudC5TdGF0ZSk6IEFCUC5CYXNpY0l0ZW1bXSB7XHJcbiAgICByZXR1cm4gcmVzdWx0Lml0ZW1zIHx8IFtdO1xyXG4gIH1cclxuXHJcbiAgQFNlbGVjdG9yKClcclxuICBzdGF0aWMgZ2V0VGVuYW50c1RvdGFsQ291bnQoeyByZXN1bHQgfTogVGVuYW50TWFuYWdlbWVudC5TdGF0ZSk6IG51bWJlciB7XHJcbiAgICByZXR1cm4gcmVzdWx0LnRvdGFsQ291bnQ7XHJcbiAgfVxyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlOiBUZW5hbnRNYW5hZ2VtZW50U2VydmljZSkge31cclxuXHJcbiAgQEFjdGlvbihHZXRUZW5hbnRzKVxyXG4gIGdldCh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFRlbmFudE1hbmFnZW1lbnQuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0VGVuYW50cykge1xyXG4gICAgcmV0dXJuIHRoaXMudGVuYW50TWFuYWdlbWVudFNlcnZpY2UuZ2V0VGVuYW50KHBheWxvYWQpLnBpcGUoXHJcbiAgICAgIHRhcChyZXN1bHQgPT5cclxuICAgICAgICBwYXRjaFN0YXRlKHtcclxuICAgICAgICAgIHJlc3VsdCxcclxuICAgICAgICB9KSxcclxuICAgICAgKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKEdldFRlbmFudEJ5SWQpXHJcbiAgZ2V0QnlJZCh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFRlbmFudE1hbmFnZW1lbnQuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0VGVuYW50QnlJZCkge1xyXG4gICAgcmV0dXJuIHRoaXMudGVuYW50TWFuYWdlbWVudFNlcnZpY2UuZ2V0VGVuYW50QnlJZChwYXlsb2FkKS5waXBlKFxyXG4gICAgICB0YXAoc2VsZWN0ZWRJdGVtID0+XHJcbiAgICAgICAgcGF0Y2hTdGF0ZSh7XHJcbiAgICAgICAgICBzZWxlY3RlZEl0ZW0sXHJcbiAgICAgICAgfSksXHJcbiAgICAgICksXHJcbiAgICApO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihEZWxldGVUZW5hbnQpXHJcbiAgZGVsZXRlKHsgZGlzcGF0Y2ggfTogU3RhdGVDb250ZXh0PFRlbmFudE1hbmFnZW1lbnQuU3RhdGU+LCB7IHBheWxvYWQgfTogRGVsZXRlVGVuYW50KSB7XHJcbiAgICByZXR1cm4gdGhpcy50ZW5hbnRNYW5hZ2VtZW50U2VydmljZS5kZWxldGVUZW5hbnQocGF5bG9hZCkucGlwZShzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IEdldFRlbmFudHMoKSkpKTtcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oQ3JlYXRlVGVuYW50KVxyXG4gIGFkZCh7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxUZW5hbnRNYW5hZ2VtZW50LlN0YXRlPiwgeyBwYXlsb2FkIH06IENyZWF0ZVRlbmFudCkge1xyXG4gICAgcmV0dXJuIHRoaXMudGVuYW50TWFuYWdlbWVudFNlcnZpY2UuY3JlYXRlVGVuYW50KHBheWxvYWQpLnBpcGUoc3dpdGNoTWFwKCgpID0+IGRpc3BhdGNoKG5ldyBHZXRUZW5hbnRzKCkpKSk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKFVwZGF0ZVRlbmFudClcclxuICB1cGRhdGUoeyBkaXNwYXRjaCwgZ2V0U3RhdGUgfTogU3RhdGVDb250ZXh0PFRlbmFudE1hbmFnZW1lbnQuU3RhdGU+LCB7IHBheWxvYWQgfTogVXBkYXRlVGVuYW50KSB7XHJcbiAgICByZXR1cm4gdGhpcy50ZW5hbnRNYW5hZ2VtZW50U2VydmljZVxyXG4gICAgICAudXBkYXRlVGVuYW50KHsgLi4uZ2V0U3RhdGUoKS5zZWxlY3RlZEl0ZW0sIC4uLnBheWxvYWQgfSlcclxuICAgICAgLnBpcGUoc3dpdGNoTWFwKCgpID0+IGRpc3BhdGNoKG5ldyBHZXRUZW5hbnRzKCkpKSk7XHJcbiAgfVxyXG59XHJcbiJdfQ==