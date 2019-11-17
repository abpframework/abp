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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5wZXJtaXNzaW9uLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL3Blcm1pc3Npb24tbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sRUFBZ0IsUUFBUSxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxjQUFjLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSwwQ0FBMEMsQ0FBQztBQUU3RixPQUFPLEVBQUUsMkJBQTJCLEVBQUUsTUFBTSwyQ0FBMkMsQ0FBQztBQUN4RixPQUFPLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7O0lBaUJuQyxtQ0FBb0IsMkJBQXdEO1FBQXhELGdDQUEyQixHQUEzQiwyQkFBMkIsQ0FBNkI7SUFBRyxDQUFDOzs7OztJQVR6RSw2Q0FBbUI7Ozs7SUFBMUIsVUFBMkIsRUFBNkM7WUFBM0MsZ0NBQWE7UUFDeEMsT0FBTyxhQUFhLENBQUMsTUFBTSxJQUFJLEVBQUUsQ0FBQztJQUNwQyxDQUFDOzs7OztJQUdNLDhDQUFvQjs7OztJQUEzQixVQUE0QixFQUE2QztZQUEzQyxnQ0FBYTtRQUN6QyxPQUFPLGFBQWEsQ0FBQyxpQkFBaUIsQ0FBQztJQUN6QyxDQUFDOzs7Ozs7SUFLRCwyREFBdUI7Ozs7O0lBQXZCLFVBQXdCLEVBQXdELEVBQUUsRUFBMkI7WUFBbkYsMEJBQVU7WUFBZ0Qsb0JBQU87UUFDekYsT0FBTyxJQUFJLENBQUMsMkJBQTJCLENBQUMsY0FBYyxDQUFDLE9BQU8sQ0FBQyxDQUFDLElBQUksQ0FDbEUsR0FBRzs7OztRQUFDLFVBQUEsa0JBQWtCO1lBQ3BCLE9BQUEsVUFBVSxDQUFDO2dCQUNULGFBQWEsRUFBRSxrQkFBa0I7YUFDbEMsQ0FBQztRQUZGLENBRUUsRUFDSCxDQUNGLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCw4REFBMEI7Ozs7O0lBQTFCLFVBQTJCLENBQUMsRUFBRSxFQUE4QjtZQUE1QixvQkFBTztRQUNyQyxPQUFPLElBQUksQ0FBQywyQkFBMkIsQ0FBQyxpQkFBaUIsQ0FBQyxPQUFPLENBQUMsQ0FBQztJQUNyRSxDQUFDOztnQkFoQmdELDJCQUEyQjs7SUFHNUU7UUFEQyxNQUFNLENBQUMsY0FBYyxDQUFDOzt5REFDd0UsY0FBYzs7NEVBUTVHO0lBR0Q7UUFEQyxNQUFNLENBQUMsaUJBQWlCLENBQUM7O3lEQUNpQixpQkFBaUI7OytFQUUzRDtJQXpCRDtRQURDLFFBQVEsRUFBRTs7Ozs4REFHVjtJQUdEO1FBREMsUUFBUSxFQUFFOzs7OytEQUdWO0lBVFUseUJBQXlCO1FBSnJDLEtBQUssQ0FBNkI7WUFDakMsSUFBSSxFQUFFLDJCQUEyQjtZQUNqQyxRQUFRLEVBQUUsbUJBQUEsRUFBRSxhQUFhLEVBQUUsRUFBRSxFQUFFLEVBQThCO1NBQzlELENBQUM7aURBWWlELDJCQUEyQjtPQVhqRSx5QkFBeUIsQ0E0QnJDO0lBQUQsZ0NBQUM7Q0FBQSxJQUFBO1NBNUJZLHlCQUF5Qjs7Ozs7O0lBV3hCLGdFQUFnRSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IFN0YXRlLCBBY3Rpb24sIFN0YXRlQ29udGV4dCwgU2VsZWN0b3IgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IEdldFBlcm1pc3Npb25zLCBVcGRhdGVQZXJtaXNzaW9ucyB9IGZyb20gJy4uL2FjdGlvbnMvcGVybWlzc2lvbi1tYW5hZ2VtZW50LmFjdGlvbnMnO1xyXG5pbXBvcnQgeyBQZXJtaXNzaW9uTWFuYWdlbWVudCB9IGZyb20gJy4uL21vZGVscy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQnO1xyXG5pbXBvcnQgeyBQZXJtaXNzaW9uTWFuYWdlbWVudFNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQuc2VydmljZSc7XHJcbmltcG9ydCB7IHRhcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcclxuXHJcbkBTdGF0ZTxQZXJtaXNzaW9uTWFuYWdlbWVudC5TdGF0ZT4oe1xyXG4gIG5hbWU6ICdQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlJyxcclxuICBkZWZhdWx0czogeyBwZXJtaXNzaW9uUmVzOiB7fSB9IGFzIFBlcm1pc3Npb25NYW5hZ2VtZW50LlN0YXRlLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZSB7XHJcbiAgQFNlbGVjdG9yKClcclxuICBzdGF0aWMgZ2V0UGVybWlzc2lvbkdyb3Vwcyh7IHBlcm1pc3Npb25SZXMgfTogUGVybWlzc2lvbk1hbmFnZW1lbnQuU3RhdGUpIHtcclxuICAgIHJldHVybiBwZXJtaXNzaW9uUmVzLmdyb3VwcyB8fCBbXTtcclxuICB9XHJcblxyXG4gIEBTZWxlY3RvcigpXHJcbiAgc3RhdGljIGdldEVudGl0eURpc3BsYXlOYW1lKHsgcGVybWlzc2lvblJlcyB9OiBQZXJtaXNzaW9uTWFuYWdlbWVudC5TdGF0ZSk6IHN0cmluZyB7XHJcbiAgICByZXR1cm4gcGVybWlzc2lvblJlcy5lbnRpdHlEaXNwbGF5TmFtZTtcclxuICB9XHJcblxyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcGVybWlzc2lvbk1hbmFnZW1lbnRTZXJ2aWNlOiBQZXJtaXNzaW9uTWFuYWdlbWVudFNlcnZpY2UpIHt9XHJcblxyXG4gIEBBY3Rpb24oR2V0UGVybWlzc2lvbnMpXHJcbiAgcGVybWlzc2lvbk1hbmFnZW1lbnRHZXQoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxQZXJtaXNzaW9uTWFuYWdlbWVudC5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRQZXJtaXNzaW9ucykge1xyXG4gICAgcmV0dXJuIHRoaXMucGVybWlzc2lvbk1hbmFnZW1lbnRTZXJ2aWNlLmdldFBlcm1pc3Npb25zKHBheWxvYWQpLnBpcGUoXHJcbiAgICAgIHRhcChwZXJtaXNzaW9uUmVzcG9uc2UgPT5cclxuICAgICAgICBwYXRjaFN0YXRlKHtcclxuICAgICAgICAgIHBlcm1pc3Npb25SZXM6IHBlcm1pc3Npb25SZXNwb25zZSxcclxuICAgICAgICB9KSxcclxuICAgICAgKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKFVwZGF0ZVBlcm1pc3Npb25zKVxyXG4gIHBlcm1pc3Npb25NYW5hZ2VtZW50VXBkYXRlKF8sIHsgcGF5bG9hZCB9OiBVcGRhdGVQZXJtaXNzaW9ucykge1xyXG4gICAgcmV0dXJuIHRoaXMucGVybWlzc2lvbk1hbmFnZW1lbnRTZXJ2aWNlLnVwZGF0ZVBlcm1pc3Npb25zKHBheWxvYWQpO1xyXG4gIH1cclxufVxyXG4iXX0=