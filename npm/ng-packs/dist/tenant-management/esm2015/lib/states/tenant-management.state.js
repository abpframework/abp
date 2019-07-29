/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Action, Selector, State } from '@ngxs/store';
import { switchMap, tap } from 'rxjs/operators';
import { TenantManagementAdd, TenantManagementDelete, TenantManagementGet, TenantManagementGetById, TenantManagementUpdate, } from '../actions/tenant-management.actions';
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
    get({ patchState }) {
        return this.tenantManagementService.get().pipe(tap((/**
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
        return this.tenantManagementService.getById(payload).pipe(tap((/**
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
        return this.tenantManagementService.delete(payload).pipe(switchMap((/**
         * @return {?}
         */
        () => dispatch(new TenantManagementGet()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    add({ dispatch }, { payload }) {
        return this.tenantManagementService.add(payload).pipe(switchMap((/**
         * @return {?}
         */
        () => dispatch(new TenantManagementGet()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    update({ dispatch, getState }, { payload }) {
        return dispatch(new TenantManagementGetById(payload.id)).pipe(switchMap((/**
         * @return {?}
         */
        () => this.tenantManagementService.update(Object.assign({}, getState().selectedItem, payload)))), switchMap((/**
         * @return {?}
         */
        () => dispatch(new TenantManagementGet()))));
    }
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
export { TenantManagementState };
if (false) {
    /**
     * @type {?}
     * @private
     */
    TenantManagementState.prototype.tenantManagementService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LW1hbmFnZW1lbnQuc3RhdGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRlbmFudC1tYW5hZ2VtZW50LyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy90ZW5hbnQtbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLFNBQVMsRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNoRCxPQUFPLEVBQ0wsbUJBQW1CLEVBQ25CLHNCQUFzQixFQUN0QixtQkFBbUIsRUFDbkIsdUJBQXVCLEVBQ3ZCLHNCQUFzQixHQUN2QixNQUFNLHNDQUFzQyxDQUFDO0FBRTlDLE9BQU8sRUFBRSx1QkFBdUIsRUFBRSxNQUFNLHVDQUF1QyxDQUFDO0lBT25FLHFCQUFxQixTQUFyQixxQkFBcUI7Ozs7SUFNaEMsWUFBb0IsdUJBQWdEO1FBQWhELDRCQUF1QixHQUF2Qix1QkFBdUIsQ0FBeUI7SUFBRyxDQUFDOzs7OztJQUp4RSxNQUFNLENBQUMsR0FBRyxDQUFDLEVBQUUsTUFBTSxFQUEwQjtRQUMzQyxPQUFPLE1BQU0sQ0FBQyxLQUFLLElBQUksRUFBRSxDQUFDO0lBQzVCLENBQUM7Ozs7O0lBS0QsR0FBRyxDQUFDLEVBQUUsVUFBVSxFQUF3QztRQUN0RCxPQUFPLElBQUksQ0FBQyx1QkFBdUIsQ0FBQyxHQUFHLEVBQUUsQ0FBQyxJQUFJLENBQzVDLEdBQUc7Ozs7UUFBQyxNQUFNLENBQUMsRUFBRSxDQUNYLFVBQVUsQ0FBQztZQUNULE1BQU07U0FDUCxDQUFDLEVBQ0gsQ0FDRixDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0QsT0FBTyxDQUFDLEVBQUUsVUFBVSxFQUF3QyxFQUFFLEVBQUUsT0FBTyxFQUEyQjtRQUNoRyxPQUFPLElBQUksQ0FBQyx1QkFBdUIsQ0FBQyxPQUFPLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUN2RCxHQUFHOzs7O1FBQUMsWUFBWSxDQUFDLEVBQUUsQ0FDakIsVUFBVSxDQUFDO1lBQ1QsWUFBWTtTQUNiLENBQUMsRUFDSCxDQUNGLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCxNQUFNLENBQUMsRUFBRSxRQUFRLEVBQXdDLEVBQUUsRUFBRSxPQUFPLEVBQTBCO1FBQzVGLE9BQU8sSUFBSSxDQUFDLHVCQUF1QixDQUFDLE1BQU0sQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQUMsU0FBUzs7O1FBQUMsR0FBRyxFQUFFLENBQUMsUUFBUSxDQUFDLElBQUksbUJBQW1CLEVBQUUsQ0FBQyxFQUFDLENBQUMsQ0FBQztJQUNqSCxDQUFDOzs7Ozs7SUFHRCxHQUFHLENBQUMsRUFBRSxRQUFRLEVBQXdDLEVBQUUsRUFBRSxPQUFPLEVBQXVCO1FBQ3RGLE9BQU8sSUFBSSxDQUFDLHVCQUF1QixDQUFDLEdBQUcsQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQUMsU0FBUzs7O1FBQUMsR0FBRyxFQUFFLENBQUMsUUFBUSxDQUFDLElBQUksbUJBQW1CLEVBQUUsQ0FBQyxFQUFDLENBQUMsQ0FBQztJQUM5RyxDQUFDOzs7Ozs7SUFHRCxNQUFNLENBQUMsRUFBRSxRQUFRLEVBQUUsUUFBUSxFQUF3QyxFQUFFLEVBQUUsT0FBTyxFQUEwQjtRQUN0RyxPQUFPLFFBQVEsQ0FBQyxJQUFJLHVCQUF1QixDQUFDLE9BQU8sQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FDM0QsU0FBUzs7O1FBQUMsR0FBRyxFQUFFLENBQUMsSUFBSSxDQUFDLHVCQUF1QixDQUFDLE1BQU0sbUJBQU0sUUFBUSxFQUFFLENBQUMsWUFBWSxFQUFLLE9BQU8sRUFBRyxFQUFDLEVBQ2hHLFNBQVM7OztRQUFDLEdBQUcsRUFBRSxDQUFDLFFBQVEsQ0FBQyxJQUFJLG1CQUFtQixFQUFFLENBQUMsRUFBQyxDQUNyRCxDQUFDO0lBQ0osQ0FBQztDQUNGLENBQUE7QUF0Q0M7SUFEQyxNQUFNLENBQUMsbUJBQW1CLENBQUM7Ozs7Z0RBUzNCO0FBR0Q7SUFEQyxNQUFNLENBQUMsdUJBQXVCLENBQUM7O3FEQUMyQyx1QkFBdUI7O29EQVFqRztBQUdEO0lBREMsTUFBTSxDQUFDLHNCQUFzQixDQUFDOztxREFDeUMsc0JBQXNCOzttREFFN0Y7QUFHRDtJQURDLE1BQU0sQ0FBQyxtQkFBbUIsQ0FBQzs7cURBQ3lDLG1CQUFtQjs7Z0RBRXZGO0FBR0Q7SUFEQyxNQUFNLENBQUMsc0JBQXNCLENBQUM7O3FEQUNtRCxzQkFBc0I7O21EQUt2RztBQTVDRDtJQURDLFFBQVEsRUFBRTs7OztzQ0FHVjtBQUpVLHFCQUFxQjtJQUpqQyxLQUFLLENBQXlCO1FBQzdCLElBQUksRUFBRSx1QkFBdUI7UUFDN0IsUUFBUSxFQUFFLG1CQUFBLEVBQUUsTUFBTSxFQUFFLEVBQUUsRUFBRSxZQUFZLEVBQUUsRUFBRSxFQUFFLEVBQTBCO0tBQ3JFLENBQUM7NkNBTzZDLHVCQUF1QjtHQU56RCxxQkFBcUIsQ0ErQ2pDO1NBL0NZLHFCQUFxQjs7Ozs7O0lBTXBCLHdEQUF3RCIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFjdGlvbiwgU2VsZWN0b3IsIFN0YXRlLCBTdGF0ZUNvbnRleHQgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBzd2l0Y2hNYXAsIHRhcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7XG4gIFRlbmFudE1hbmFnZW1lbnRBZGQsXG4gIFRlbmFudE1hbmFnZW1lbnREZWxldGUsXG4gIFRlbmFudE1hbmFnZW1lbnRHZXQsXG4gIFRlbmFudE1hbmFnZW1lbnRHZXRCeUlkLFxuICBUZW5hbnRNYW5hZ2VtZW50VXBkYXRlLFxufSBmcm9tICcuLi9hY3Rpb25zL3RlbmFudC1tYW5hZ2VtZW50LmFjdGlvbnMnO1xuaW1wb3J0IHsgVGVuYW50TWFuYWdlbWVudCB9IGZyb20gJy4uL21vZGVscy90ZW5hbnQtbWFuYWdlbWVudCc7XG5pbXBvcnQgeyBUZW5hbnRNYW5hZ2VtZW50U2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL3RlbmFudC1tYW5hZ2VtZW50LnNlcnZpY2UnO1xuaW1wb3J0IHsgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcblxuQFN0YXRlPFRlbmFudE1hbmFnZW1lbnQuU3RhdGU+KHtcbiAgbmFtZTogJ1RlbmFudE1hbmFnZW1lbnRTdGF0ZScsXG4gIGRlZmF1bHRzOiB7IHJlc3VsdDoge30sIHNlbGVjdGVkSXRlbToge30gfSBhcyBUZW5hbnRNYW5hZ2VtZW50LlN0YXRlLFxufSlcbmV4cG9ydCBjbGFzcyBUZW5hbnRNYW5hZ2VtZW50U3RhdGUge1xuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0KHsgcmVzdWx0IH06IFRlbmFudE1hbmFnZW1lbnQuU3RhdGUpOiBBQlAuQmFzaWNJdGVtW10ge1xuICAgIHJldHVybiByZXN1bHQuaXRlbXMgfHwgW107XG4gIH1cblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHRlbmFudE1hbmFnZW1lbnRTZXJ2aWNlOiBUZW5hbnRNYW5hZ2VtZW50U2VydmljZSkge31cblxuICBAQWN0aW9uKFRlbmFudE1hbmFnZW1lbnRHZXQpXG4gIGdldCh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFRlbmFudE1hbmFnZW1lbnQuU3RhdGU+KSB7XG4gICAgcmV0dXJuIHRoaXMudGVuYW50TWFuYWdlbWVudFNlcnZpY2UuZ2V0KCkucGlwZShcbiAgICAgIHRhcChyZXN1bHQgPT5cbiAgICAgICAgcGF0Y2hTdGF0ZSh7XG4gICAgICAgICAgcmVzdWx0LFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIEBBY3Rpb24oVGVuYW50TWFuYWdlbWVudEdldEJ5SWQpXG4gIGdldEJ5SWQoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxUZW5hbnRNYW5hZ2VtZW50LlN0YXRlPiwgeyBwYXlsb2FkIH06IFRlbmFudE1hbmFnZW1lbnRHZXRCeUlkKSB7XG4gICAgcmV0dXJuIHRoaXMudGVuYW50TWFuYWdlbWVudFNlcnZpY2UuZ2V0QnlJZChwYXlsb2FkKS5waXBlKFxuICAgICAgdGFwKHNlbGVjdGVkSXRlbSA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICBzZWxlY3RlZEl0ZW0sXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihUZW5hbnRNYW5hZ2VtZW50RGVsZXRlKVxuICBkZWxldGUoeyBkaXNwYXRjaCB9OiBTdGF0ZUNvbnRleHQ8VGVuYW50TWFuYWdlbWVudC5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBUZW5hbnRNYW5hZ2VtZW50RGVsZXRlKSB7XG4gICAgcmV0dXJuIHRoaXMudGVuYW50TWFuYWdlbWVudFNlcnZpY2UuZGVsZXRlKHBheWxvYWQpLnBpcGUoc3dpdGNoTWFwKCgpID0+IGRpc3BhdGNoKG5ldyBUZW5hbnRNYW5hZ2VtZW50R2V0KCkpKSk7XG4gIH1cblxuICBAQWN0aW9uKFRlbmFudE1hbmFnZW1lbnRBZGQpXG4gIGFkZCh7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxUZW5hbnRNYW5hZ2VtZW50LlN0YXRlPiwgeyBwYXlsb2FkIH06IFRlbmFudE1hbmFnZW1lbnRBZGQpIHtcbiAgICByZXR1cm4gdGhpcy50ZW5hbnRNYW5hZ2VtZW50U2VydmljZS5hZGQocGF5bG9hZCkucGlwZShzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IFRlbmFudE1hbmFnZW1lbnRHZXQoKSkpKTtcbiAgfVxuXG4gIEBBY3Rpb24oVGVuYW50TWFuYWdlbWVudFVwZGF0ZSlcbiAgdXBkYXRlKHsgZGlzcGF0Y2gsIGdldFN0YXRlIH06IFN0YXRlQ29udGV4dDxUZW5hbnRNYW5hZ2VtZW50LlN0YXRlPiwgeyBwYXlsb2FkIH06IFRlbmFudE1hbmFnZW1lbnRVcGRhdGUpIHtcbiAgICByZXR1cm4gZGlzcGF0Y2gobmV3IFRlbmFudE1hbmFnZW1lbnRHZXRCeUlkKHBheWxvYWQuaWQpKS5waXBlKFxuICAgICAgc3dpdGNoTWFwKCgpID0+IHRoaXMudGVuYW50TWFuYWdlbWVudFNlcnZpY2UudXBkYXRlKHsgLi4uZ2V0U3RhdGUoKS5zZWxlY3RlZEl0ZW0sIC4uLnBheWxvYWQgfSkpLFxuICAgICAgc3dpdGNoTWFwKCgpID0+IGRpc3BhdGNoKG5ldyBUZW5hbnRNYW5hZ2VtZW50R2V0KCkpKSxcbiAgICApO1xuICB9XG59XG4iXX0=