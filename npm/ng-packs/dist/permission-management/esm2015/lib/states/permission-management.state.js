/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { State, Action, Selector } from '@ngxs/store';
import { GetPermissions, UpdatePermissions } from '../actions/permission-management.actions';
import { PermissionManagementService } from '../services/permission-management.service';
import { tap } from 'rxjs/operators';
let PermissionManagementState = class PermissionManagementState {
    /**
     * @param {?} permissionManagementService
     */
    constructor(permissionManagementService) {
        this.permissionManagementService = permissionManagementService;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getPermissionGroups({ permissionRes }) {
        return permissionRes.groups || [];
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getEntitiyDisplayName({ permissionRes }) {
        return permissionRes.entityDisplayName;
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    permissionManagementGet({ patchState }, { payload }) {
        return this.permissionManagementService.getPermissions(payload).pipe(tap((/**
         * @param {?} permissionResponse
         * @return {?}
         */
        permissionResponse => patchState({
            permissionRes: permissionResponse,
        }))));
    }
    /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    permissionManagementUpdate(_, { payload }) {
        return this.permissionManagementService.updatePermissions(payload);
    }
};
PermissionManagementState.ctorParameters = () => [
    { type: PermissionManagementService }
];
tslib_1.__decorate([
    Action(GetPermissions),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, GetPermissions]),
    tslib_1.__metadata("design:returntype", void 0)
], PermissionManagementState.prototype, "permissionManagementGet", null);
tslib_1.__decorate([
    Action(UpdatePermissions),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, UpdatePermissions]),
    tslib_1.__metadata("design:returntype", void 0)
], PermissionManagementState.prototype, "permissionManagementUpdate", null);
tslib_1.__decorate([
    Selector(),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object]),
    tslib_1.__metadata("design:returntype", void 0)
], PermissionManagementState, "getPermissionGroups", null);
tslib_1.__decorate([
    Selector(),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object]),
    tslib_1.__metadata("design:returntype", String)
], PermissionManagementState, "getEntitiyDisplayName", null);
PermissionManagementState = tslib_1.__decorate([
    State({
        name: 'PermissionManagementState',
        defaults: (/** @type {?} */ ({ permissionRes: {} })),
    }),
    tslib_1.__metadata("design:paramtypes", [PermissionManagementService])
], PermissionManagementState);
export { PermissionManagementState };
if (false) {
    /**
     * @type {?}
     * @private
     */
    PermissionManagementState.prototype.permissionManagementService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5wZXJtaXNzaW9uLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL3Blcm1pc3Npb24tbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFnQixRQUFRLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLGNBQWMsRUFBRSxpQkFBaUIsRUFBRSxNQUFNLDBDQUEwQyxDQUFDO0FBRTdGLE9BQU8sRUFBRSwyQkFBMkIsRUFBRSxNQUFNLDJDQUEyQyxDQUFDO0FBQ3hGLE9BQU8sRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztJQU14Qix5QkFBeUIsU0FBekIseUJBQXlCOzs7O0lBV3BDLFlBQW9CLDJCQUF3RDtRQUF4RCxnQ0FBMkIsR0FBM0IsMkJBQTJCLENBQTZCO0lBQUcsQ0FBQzs7Ozs7SUFUaEYsTUFBTSxDQUFDLG1CQUFtQixDQUFDLEVBQUUsYUFBYSxFQUE4QjtRQUN0RSxPQUFPLGFBQWEsQ0FBQyxNQUFNLElBQUksRUFBRSxDQUFDO0lBQ3BDLENBQUM7Ozs7O0lBR0QsTUFBTSxDQUFDLHFCQUFxQixDQUFDLEVBQUUsYUFBYSxFQUE4QjtRQUN4RSxPQUFPLGFBQWEsQ0FBQyxpQkFBaUIsQ0FBQztJQUN6QyxDQUFDOzs7Ozs7SUFLRCx1QkFBdUIsQ0FBQyxFQUFFLFVBQVUsRUFBNEMsRUFBRSxFQUFFLE9BQU8sRUFBa0I7UUFDM0csT0FBTyxJQUFJLENBQUMsMkJBQTJCLENBQUMsY0FBYyxDQUFDLE9BQU8sQ0FBQyxDQUFDLElBQUksQ0FDbEUsR0FBRzs7OztRQUFDLGtCQUFrQixDQUFDLEVBQUUsQ0FDdkIsVUFBVSxDQUFDO1lBQ1QsYUFBYSxFQUFFLGtCQUFrQjtTQUNsQyxDQUFDLEVBQ0gsQ0FDRixDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0QsMEJBQTBCLENBQUMsQ0FBQyxFQUFFLEVBQUUsT0FBTyxFQUFxQjtRQUMxRCxPQUFPLElBQUksQ0FBQywyQkFBMkIsQ0FBQyxpQkFBaUIsQ0FBQyxPQUFPLENBQUMsQ0FBQztJQUNyRSxDQUFDO0NBQ0YsQ0FBQTs7WUFqQmtELDJCQUEyQjs7QUFHNUU7SUFEQyxNQUFNLENBQUMsY0FBYyxDQUFDOztxREFDd0UsY0FBYzs7d0VBUTVHO0FBR0Q7SUFEQyxNQUFNLENBQUMsaUJBQWlCLENBQUM7O3FEQUNpQixpQkFBaUI7OzJFQUUzRDtBQXpCRDtJQURDLFFBQVEsRUFBRTs7OzswREFHVjtBQUdEO0lBREMsUUFBUSxFQUFFOzs7OzREQUdWO0FBVFUseUJBQXlCO0lBSnJDLEtBQUssQ0FBNkI7UUFDakMsSUFBSSxFQUFFLDJCQUEyQjtRQUNqQyxRQUFRLEVBQUUsbUJBQUEsRUFBRSxhQUFhLEVBQUUsRUFBRSxFQUFFLEVBQThCO0tBQzlELENBQUM7NkNBWWlELDJCQUEyQjtHQVhqRSx5QkFBeUIsQ0E0QnJDO1NBNUJZLHlCQUF5Qjs7Ozs7O0lBV3hCLGdFQUFnRSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFN0YXRlLCBBY3Rpb24sIFN0YXRlQ29udGV4dCwgU2VsZWN0b3IgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBHZXRQZXJtaXNzaW9ucywgVXBkYXRlUGVybWlzc2lvbnMgfSBmcm9tICcuLi9hY3Rpb25zL3Blcm1pc3Npb24tbWFuYWdlbWVudC5hY3Rpb25zJztcbmltcG9ydCB7IFBlcm1pc3Npb25NYW5hZ2VtZW50IH0gZnJvbSAnLi4vbW9kZWxzL3Blcm1pc3Npb24tbWFuYWdlbWVudCc7XG5pbXBvcnQgeyBQZXJtaXNzaW9uTWFuYWdlbWVudFNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQuc2VydmljZSc7XG5pbXBvcnQgeyB0YXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5cbkBTdGF0ZTxQZXJtaXNzaW9uTWFuYWdlbWVudC5TdGF0ZT4oe1xuICBuYW1lOiAnUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZScsXG4gIGRlZmF1bHRzOiB7IHBlcm1pc3Npb25SZXM6IHt9IH0gYXMgUGVybWlzc2lvbk1hbmFnZW1lbnQuU3RhdGUsXG59KVxuZXhwb3J0IGNsYXNzIFBlcm1pc3Npb25NYW5hZ2VtZW50U3RhdGUge1xuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0UGVybWlzc2lvbkdyb3Vwcyh7IHBlcm1pc3Npb25SZXMgfTogUGVybWlzc2lvbk1hbmFnZW1lbnQuU3RhdGUpIHtcbiAgICByZXR1cm4gcGVybWlzc2lvblJlcy5ncm91cHMgfHwgW107XG4gIH1cblxuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0RW50aXRpeURpc3BsYXlOYW1lKHsgcGVybWlzc2lvblJlcyB9OiBQZXJtaXNzaW9uTWFuYWdlbWVudC5TdGF0ZSk6IHN0cmluZyB7XG4gICAgcmV0dXJuIHBlcm1pc3Npb25SZXMuZW50aXR5RGlzcGxheU5hbWU7XG4gIH1cblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHBlcm1pc3Npb25NYW5hZ2VtZW50U2VydmljZTogUGVybWlzc2lvbk1hbmFnZW1lbnRTZXJ2aWNlKSB7fVxuXG4gIEBBY3Rpb24oR2V0UGVybWlzc2lvbnMpXG4gIHBlcm1pc3Npb25NYW5hZ2VtZW50R2V0KHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8UGVybWlzc2lvbk1hbmFnZW1lbnQuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0UGVybWlzc2lvbnMpIHtcbiAgICByZXR1cm4gdGhpcy5wZXJtaXNzaW9uTWFuYWdlbWVudFNlcnZpY2UuZ2V0UGVybWlzc2lvbnMocGF5bG9hZCkucGlwZShcbiAgICAgIHRhcChwZXJtaXNzaW9uUmVzcG9uc2UgPT5cbiAgICAgICAgcGF0Y2hTdGF0ZSh7XG4gICAgICAgICAgcGVybWlzc2lvblJlczogcGVybWlzc2lvblJlc3BvbnNlLFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIEBBY3Rpb24oVXBkYXRlUGVybWlzc2lvbnMpXG4gIHBlcm1pc3Npb25NYW5hZ2VtZW50VXBkYXRlKF8sIHsgcGF5bG9hZCB9OiBVcGRhdGVQZXJtaXNzaW9ucykge1xuICAgIHJldHVybiB0aGlzLnBlcm1pc3Npb25NYW5hZ2VtZW50U2VydmljZS51cGRhdGVQZXJtaXNzaW9ucyhwYXlsb2FkKTtcbiAgfVxufVxuIl19