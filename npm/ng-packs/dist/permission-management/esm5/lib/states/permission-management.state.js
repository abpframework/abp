/**
 * @fileoverview added by tsickle
 * Generated from: lib/states/permission-management.state.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { State, Action, Selector } from '@ngxs/store';
import { GetPermissions, UpdatePermissions } from '../actions/permission-management.actions';
import { PermissionManagementService } from '../services/permission-management.service';
import { tap } from 'rxjs/operators';
var PermissionManagementState = /** @class */ (function () {
    function PermissionManagementState(permissionManagementService) {
        this.permissionManagementService = permissionManagementService;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    PermissionManagementState.getPermissionGroups = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var permissionRes = _a.permissionRes;
        return permissionRes.groups || [];
    };
    /**
     * @param {?} __0
     * @return {?}
     */
    PermissionManagementState.getEntityDisplayName = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var permissionRes = _a.permissionRes;
        return permissionRes.entityDisplayName;
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    PermissionManagementState.prototype.permissionManagementGet = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var patchState = _a.patchState;
        var payload = _b.payload;
        return this.permissionManagementService.getPermissions(payload).pipe(tap((/**
         * @param {?} permissionResponse
         * @return {?}
         */
        function (permissionResponse) {
            return patchState({
                permissionRes: permissionResponse,
            });
        })));
    };
    /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    PermissionManagementState.prototype.permissionManagementUpdate = /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    function (_, _a) {
        var payload = _a.payload;
        return this.permissionManagementService.updatePermissions(payload);
    };
    PermissionManagementState.ctorParameters = function () { return [
        { type: PermissionManagementService }
    ]; };
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
    ], PermissionManagementState, "getEntityDisplayName", null);
    PermissionManagementState = tslib_1.__decorate([
        State({
            name: 'PermissionManagementState',
            defaults: (/** @type {?} */ ({ permissionRes: {} })),
        }),
        tslib_1.__metadata("design:paramtypes", [PermissionManagementService])
    ], PermissionManagementState);
    return PermissionManagementState;
}());
export { PermissionManagementState };
if (false) {
    /**
     * @type {?}
     * @private
     */
    PermissionManagementState.prototype.permissionManagementService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5wZXJtaXNzaW9uLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL3Blcm1pc3Npb24tbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sRUFBZ0IsUUFBUSxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxjQUFjLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSwwQ0FBMEMsQ0FBQztBQUU3RixPQUFPLEVBQUUsMkJBQTJCLEVBQUUsTUFBTSwyQ0FBMkMsQ0FBQztBQUN4RixPQUFPLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7O0lBaUJuQyxtQ0FBb0IsMkJBQXdEO1FBQXhELGdDQUEyQixHQUEzQiwyQkFBMkIsQ0FBNkI7SUFBRyxDQUFDOzs7OztJQVR6RSw2Q0FBbUI7Ozs7SUFBMUIsVUFBMkIsRUFBNkM7WUFBM0MsZ0NBQWE7UUFDeEMsT0FBTyxhQUFhLENBQUMsTUFBTSxJQUFJLEVBQUUsQ0FBQztJQUNwQyxDQUFDOzs7OztJQUdNLDhDQUFvQjs7OztJQUEzQixVQUE0QixFQUE2QztZQUEzQyxnQ0FBYTtRQUN6QyxPQUFPLGFBQWEsQ0FBQyxpQkFBaUIsQ0FBQztJQUN6QyxDQUFDOzs7Ozs7SUFLRCwyREFBdUI7Ozs7O0lBQXZCLFVBQXdCLEVBQXdELEVBQUUsRUFBMkI7WUFBbkYsMEJBQVU7WUFBZ0Qsb0JBQU87UUFDekYsT0FBTyxJQUFJLENBQUMsMkJBQTJCLENBQUMsY0FBYyxDQUFDLE9BQU8sQ0FBQyxDQUFDLElBQUksQ0FDbEUsR0FBRzs7OztRQUFDLFVBQUEsa0JBQWtCO1lBQ3BCLE9BQUEsVUFBVSxDQUFDO2dCQUNULGFBQWEsRUFBRSxrQkFBa0I7YUFDbEMsQ0FBQztRQUZGLENBRUUsRUFDSCxDQUNGLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCw4REFBMEI7Ozs7O0lBQTFCLFVBQTJCLENBQUMsRUFBRSxFQUE4QjtZQUE1QixvQkFBTztRQUNyQyxPQUFPLElBQUksQ0FBQywyQkFBMkIsQ0FBQyxpQkFBaUIsQ0FBQyxPQUFPLENBQUMsQ0FBQztJQUNyRSxDQUFDOztnQkFoQmdELDJCQUEyQjs7SUFHNUU7UUFEQyxNQUFNLENBQUMsY0FBYyxDQUFDOzt5REFDd0UsY0FBYzs7NEVBUTVHO0lBR0Q7UUFEQyxNQUFNLENBQUMsaUJBQWlCLENBQUM7O3lEQUNpQixpQkFBaUI7OytFQUUzRDtJQXpCRDtRQURDLFFBQVEsRUFBRTs7Ozs4REFHVjtJQUdEO1FBREMsUUFBUSxFQUFFOzs7OytEQUdWO0lBVFUseUJBQXlCO1FBSnJDLEtBQUssQ0FBNkI7WUFDakMsSUFBSSxFQUFFLDJCQUEyQjtZQUNqQyxRQUFRLEVBQUUsbUJBQUEsRUFBRSxhQUFhLEVBQUUsRUFBRSxFQUFFLEVBQThCO1NBQzlELENBQUM7aURBWWlELDJCQUEyQjtPQVhqRSx5QkFBeUIsQ0E0QnJDO0lBQUQsZ0NBQUM7Q0FBQSxJQUFBO1NBNUJZLHlCQUF5Qjs7Ozs7O0lBV3hCLGdFQUFnRSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFN0YXRlLCBBY3Rpb24sIFN0YXRlQ29udGV4dCwgU2VsZWN0b3IgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBHZXRQZXJtaXNzaW9ucywgVXBkYXRlUGVybWlzc2lvbnMgfSBmcm9tICcuLi9hY3Rpb25zL3Blcm1pc3Npb24tbWFuYWdlbWVudC5hY3Rpb25zJztcbmltcG9ydCB7IFBlcm1pc3Npb25NYW5hZ2VtZW50IH0gZnJvbSAnLi4vbW9kZWxzL3Blcm1pc3Npb24tbWFuYWdlbWVudCc7XG5pbXBvcnQgeyBQZXJtaXNzaW9uTWFuYWdlbWVudFNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQuc2VydmljZSc7XG5pbXBvcnQgeyB0YXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5cbkBTdGF0ZTxQZXJtaXNzaW9uTWFuYWdlbWVudC5TdGF0ZT4oe1xuICBuYW1lOiAnUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZScsXG4gIGRlZmF1bHRzOiB7IHBlcm1pc3Npb25SZXM6IHt9IH0gYXMgUGVybWlzc2lvbk1hbmFnZW1lbnQuU3RhdGUsXG59KVxuZXhwb3J0IGNsYXNzIFBlcm1pc3Npb25NYW5hZ2VtZW50U3RhdGUge1xuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0UGVybWlzc2lvbkdyb3Vwcyh7IHBlcm1pc3Npb25SZXMgfTogUGVybWlzc2lvbk1hbmFnZW1lbnQuU3RhdGUpIHtcbiAgICByZXR1cm4gcGVybWlzc2lvblJlcy5ncm91cHMgfHwgW107XG4gIH1cblxuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0RW50aXR5RGlzcGxheU5hbWUoeyBwZXJtaXNzaW9uUmVzIH06IFBlcm1pc3Npb25NYW5hZ2VtZW50LlN0YXRlKTogc3RyaW5nIHtcbiAgICByZXR1cm4gcGVybWlzc2lvblJlcy5lbnRpdHlEaXNwbGF5TmFtZTtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcGVybWlzc2lvbk1hbmFnZW1lbnRTZXJ2aWNlOiBQZXJtaXNzaW9uTWFuYWdlbWVudFNlcnZpY2UpIHt9XG5cbiAgQEFjdGlvbihHZXRQZXJtaXNzaW9ucylcbiAgcGVybWlzc2lvbk1hbmFnZW1lbnRHZXQoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxQZXJtaXNzaW9uTWFuYWdlbWVudC5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRQZXJtaXNzaW9ucykge1xuICAgIHJldHVybiB0aGlzLnBlcm1pc3Npb25NYW5hZ2VtZW50U2VydmljZS5nZXRQZXJtaXNzaW9ucyhwYXlsb2FkKS5waXBlKFxuICAgICAgdGFwKHBlcm1pc3Npb25SZXNwb25zZSA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICBwZXJtaXNzaW9uUmVzOiBwZXJtaXNzaW9uUmVzcG9uc2UsXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihVcGRhdGVQZXJtaXNzaW9ucylcbiAgcGVybWlzc2lvbk1hbmFnZW1lbnRVcGRhdGUoXywgeyBwYXlsb2FkIH06IFVwZGF0ZVBlcm1pc3Npb25zKSB7XG4gICAgcmV0dXJuIHRoaXMucGVybWlzc2lvbk1hbmFnZW1lbnRTZXJ2aWNlLnVwZGF0ZVBlcm1pc3Npb25zKHBheWxvYWQpO1xuICB9XG59XG4iXX0=