/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Action, Selector, State } from '@ngxs/store';
import { tap } from 'rxjs/operators';
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
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    delete(_, { payload }) {
        return this.tenantManagementService.deleteTenant(payload);
    }
    /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    add(_, { payload }) {
        return this.tenantManagementService.createTenant(payload);
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    update({ getState }, { payload }) {
        return this.tenantManagementService.updateTenant(Object.assign({}, getState().selectedItem, payload));
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQuc3RhdGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy90ZW5hbnQtbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFhLEdBQUcsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ2hELE9BQU8sRUFDTCxZQUFZLEVBQ1osWUFBWSxFQUNaLFVBQVUsRUFDVixhQUFhLEVBQ2IsWUFBWSxHQUNiLE1BQU0sc0NBQXNDLENBQUM7QUFFOUMsT0FBTyxFQUFFLHVCQUF1QixFQUFFLE1BQU0sdUNBQXVDLENBQUM7SUFPbkUscUJBQXFCLFNBQXJCLHFCQUFxQjs7OztJQVdoQyxZQUFvQix1QkFBZ0Q7UUFBaEQsNEJBQXVCLEdBQXZCLHVCQUF1QixDQUF5QjtJQUFHLENBQUM7Ozs7O0lBVHhFLE1BQU0sQ0FBQyxHQUFHLENBQUMsRUFBRSxNQUFNLEVBQTBCO1FBQzNDLE9BQU8sTUFBTSxDQUFDLEtBQUssSUFBSSxFQUFFLENBQUM7SUFDNUIsQ0FBQzs7Ozs7SUFHRCxNQUFNLENBQUMsb0JBQW9CLENBQUMsRUFBRSxNQUFNLEVBQTBCO1FBQzVELE9BQU8sTUFBTSxDQUFDLFVBQVUsQ0FBQztJQUMzQixDQUFDOzs7Ozs7SUFLRCxHQUFHLENBQUMsRUFBRSxVQUFVLEVBQXdDLEVBQUUsRUFBRSxPQUFPLEVBQWM7UUFDL0UsT0FBTyxJQUFJLENBQUMsdUJBQXVCLENBQUMsU0FBUyxDQUFDLE9BQU8sQ0FBQyxDQUFDLElBQUksQ0FDekQsR0FBRzs7OztRQUFDLE1BQU0sQ0FBQyxFQUFFLENBQ1gsVUFBVSxDQUFDO1lBQ1QsTUFBTTtTQUNQLENBQUMsRUFDSCxDQUNGLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCxPQUFPLENBQUMsRUFBRSxVQUFVLEVBQXdDLEVBQUUsRUFBRSxPQUFPLEVBQWlCO1FBQ3RGLE9BQU8sSUFBSSxDQUFDLHVCQUF1QixDQUFDLGFBQWEsQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQzdELEdBQUc7Ozs7UUFBQyxZQUFZLENBQUMsRUFBRSxDQUNqQixVQUFVLENBQUM7WUFDVCxZQUFZO1NBQ2IsQ0FBQyxFQUNILENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELE1BQU0sQ0FBQyxDQUFDLEVBQUUsRUFBRSxPQUFPLEVBQWdCO1FBQ2pDLE9BQU8sSUFBSSxDQUFDLHVCQUF1QixDQUFDLFlBQVksQ0FBQyxPQUFPLENBQUMsQ0FBQztJQUM1RCxDQUFDOzs7Ozs7SUFHRCxHQUFHLENBQUMsQ0FBQyxFQUFFLEVBQUUsT0FBTyxFQUFnQjtRQUM5QixPQUFPLElBQUksQ0FBQyx1QkFBdUIsQ0FBQyxZQUFZLENBQUMsT0FBTyxDQUFDLENBQUM7SUFDNUQsQ0FBQzs7Ozs7O0lBR0QsTUFBTSxDQUFDLEVBQUUsUUFBUSxFQUF3QyxFQUFFLEVBQUUsT0FBTyxFQUFnQjtRQUNsRixPQUFPLElBQUksQ0FBQyx1QkFBdUIsQ0FBQyxZQUFZLG1CQUFNLFFBQVEsRUFBRSxDQUFDLFlBQVksRUFBSyxPQUFPLEVBQUcsQ0FBQztJQUMvRixDQUFDO0NBQ0YsQ0FBQTs7WUF0QzhDLHVCQUF1Qjs7QUFHcEU7SUFEQyxNQUFNLENBQUMsVUFBVSxDQUFDOztxREFDb0QsVUFBVTs7Z0RBUWhGO0FBR0Q7SUFEQyxNQUFNLENBQUMsYUFBYSxDQUFDOztxREFDcUQsYUFBYTs7b0RBUXZGO0FBR0Q7SUFEQyxNQUFNLENBQUMsWUFBWSxDQUFDOztxREFDRSxZQUFZOzttREFFbEM7QUFHRDtJQURDLE1BQU0sQ0FBQyxZQUFZLENBQUM7O3FEQUNELFlBQVk7O2dEQUUvQjtBQUdEO0lBREMsTUFBTSxDQUFDLFlBQVksQ0FBQzs7cURBQ21ELFlBQVk7O21EQUVuRjtBQTlDRDtJQURDLFFBQVEsRUFBRTs7OztzQ0FHVjtBQUdEO0lBREMsUUFBUSxFQUFFOzs7O3VEQUdWO0FBVFUscUJBQXFCO0lBSmpDLEtBQUssQ0FBeUI7UUFDN0IsSUFBSSxFQUFFLHVCQUF1QjtRQUM3QixRQUFRLEVBQUUsbUJBQUEsRUFBRSxNQUFNLEVBQUUsRUFBRSxFQUFFLFlBQVksRUFBRSxFQUFFLEVBQUUsRUFBMEI7S0FDckUsQ0FBQzs2Q0FZNkMsdUJBQXVCO0dBWHpELHFCQUFxQixDQWlEakM7U0FqRFkscUJBQXFCOzs7Ozs7SUFXcEIsd0RBQXdEIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQWN0aW9uLCBTZWxlY3RvciwgU3RhdGUsIFN0YXRlQ29udGV4dCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgc3dpdGNoTWFwLCB0YXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XHJcbmltcG9ydCB7XHJcbiAgQ3JlYXRlVGVuYW50LFxyXG4gIERlbGV0ZVRlbmFudCxcclxuICBHZXRUZW5hbnRzLFxyXG4gIEdldFRlbmFudEJ5SWQsXHJcbiAgVXBkYXRlVGVuYW50LFxyXG59IGZyb20gJy4uL2FjdGlvbnMvdGVuYW50LW1hbmFnZW1lbnQuYWN0aW9ucyc7XHJcbmltcG9ydCB7IFRlbmFudE1hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMvdGVuYW50LW1hbmFnZW1lbnQnO1xyXG5pbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50U2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL3RlbmFudC1tYW5hZ2VtZW50LnNlcnZpY2UnO1xyXG5pbXBvcnQgeyBBQlAgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xyXG5cclxuQFN0YXRlPFRlbmFudE1hbmFnZW1lbnQuU3RhdGU+KHtcclxuICBuYW1lOiAnVGVuYW50TWFuYWdlbWVudFN0YXRlJyxcclxuICBkZWZhdWx0czogeyByZXN1bHQ6IHt9LCBzZWxlY3RlZEl0ZW06IHt9IH0gYXMgVGVuYW50TWFuYWdlbWVudC5TdGF0ZSxcclxufSlcclxuZXhwb3J0IGNsYXNzIFRlbmFudE1hbmFnZW1lbnRTdGF0ZSB7XHJcbiAgQFNlbGVjdG9yKClcclxuICBzdGF0aWMgZ2V0KHsgcmVzdWx0IH06IFRlbmFudE1hbmFnZW1lbnQuU3RhdGUpOiBBQlAuQmFzaWNJdGVtW10ge1xyXG4gICAgcmV0dXJuIHJlc3VsdC5pdGVtcyB8fCBbXTtcclxuICB9XHJcblxyXG4gIEBTZWxlY3RvcigpXHJcbiAgc3RhdGljIGdldFRlbmFudHNUb3RhbENvdW50KHsgcmVzdWx0IH06IFRlbmFudE1hbmFnZW1lbnQuU3RhdGUpOiBudW1iZXIge1xyXG4gICAgcmV0dXJuIHJlc3VsdC50b3RhbENvdW50O1xyXG4gIH1cclxuXHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSB0ZW5hbnRNYW5hZ2VtZW50U2VydmljZTogVGVuYW50TWFuYWdlbWVudFNlcnZpY2UpIHt9XHJcblxyXG4gIEBBY3Rpb24oR2V0VGVuYW50cylcclxuICBnZXQoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxUZW5hbnRNYW5hZ2VtZW50LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFRlbmFudHMpIHtcclxuICAgIHJldHVybiB0aGlzLnRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlLmdldFRlbmFudChwYXlsb2FkKS5waXBlKFxyXG4gICAgICB0YXAocmVzdWx0ID0+XHJcbiAgICAgICAgcGF0Y2hTdGF0ZSh7XHJcbiAgICAgICAgICByZXN1bHQsXHJcbiAgICAgICAgfSksXHJcbiAgICAgICksXHJcbiAgICApO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihHZXRUZW5hbnRCeUlkKVxyXG4gIGdldEJ5SWQoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxUZW5hbnRNYW5hZ2VtZW50LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFRlbmFudEJ5SWQpIHtcclxuICAgIHJldHVybiB0aGlzLnRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlLmdldFRlbmFudEJ5SWQocGF5bG9hZCkucGlwZShcclxuICAgICAgdGFwKHNlbGVjdGVkSXRlbSA9PlxyXG4gICAgICAgIHBhdGNoU3RhdGUoe1xyXG4gICAgICAgICAgc2VsZWN0ZWRJdGVtLFxyXG4gICAgICAgIH0pLFxyXG4gICAgICApLFxyXG4gICAgKTtcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oRGVsZXRlVGVuYW50KVxyXG4gIGRlbGV0ZShfLCB7IHBheWxvYWQgfTogRGVsZXRlVGVuYW50KSB7XHJcbiAgICByZXR1cm4gdGhpcy50ZW5hbnRNYW5hZ2VtZW50U2VydmljZS5kZWxldGVUZW5hbnQocGF5bG9hZCk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKENyZWF0ZVRlbmFudClcclxuICBhZGQoXywgeyBwYXlsb2FkIH06IENyZWF0ZVRlbmFudCkge1xyXG4gICAgcmV0dXJuIHRoaXMudGVuYW50TWFuYWdlbWVudFNlcnZpY2UuY3JlYXRlVGVuYW50KHBheWxvYWQpO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihVcGRhdGVUZW5hbnQpXHJcbiAgdXBkYXRlKHsgZ2V0U3RhdGUgfTogU3RhdGVDb250ZXh0PFRlbmFudE1hbmFnZW1lbnQuU3RhdGU+LCB7IHBheWxvYWQgfTogVXBkYXRlVGVuYW50KSB7XHJcbiAgICByZXR1cm4gdGhpcy50ZW5hbnRNYW5hZ2VtZW50U2VydmljZS51cGRhdGVUZW5hbnQoeyAuLi5nZXRTdGF0ZSgpLnNlbGVjdGVkSXRlbSwgLi4ucGF5bG9hZCB9KTtcclxuICB9XHJcbn1cclxuIl19