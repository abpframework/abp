/**
 * @fileoverview added by tsickle
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
    PermissionManagementState.getEntitiyDisplayName = /**
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
    ], PermissionManagementState, "getEntitiyDisplayName", null);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5wZXJtaXNzaW9uLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL3Blcm1pc3Npb24tbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFnQixRQUFRLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLGNBQWMsRUFBRSxpQkFBaUIsRUFBRSxNQUFNLDBDQUEwQyxDQUFDO0FBRTdGLE9BQU8sRUFBRSwyQkFBMkIsRUFBRSxNQUFNLDJDQUEyQyxDQUFDO0FBQ3hGLE9BQU8sRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQzs7SUFpQm5DLG1DQUFvQiwyQkFBd0Q7UUFBeEQsZ0NBQTJCLEdBQTNCLDJCQUEyQixDQUE2QjtJQUFHLENBQUM7Ozs7O0lBVHpFLDZDQUFtQjs7OztJQUExQixVQUEyQixFQUE2QztZQUEzQyxnQ0FBYTtRQUN4QyxPQUFPLGFBQWEsQ0FBQyxNQUFNLElBQUksRUFBRSxDQUFDO0lBQ3BDLENBQUM7Ozs7O0lBR00sK0NBQXFCOzs7O0lBQTVCLFVBQTZCLEVBQTZDO1lBQTNDLGdDQUFhO1FBQzFDLE9BQU8sYUFBYSxDQUFDLGlCQUFpQixDQUFDO0lBQ3pDLENBQUM7Ozs7OztJQUtELDJEQUF1Qjs7Ozs7SUFBdkIsVUFBd0IsRUFBd0QsRUFBRSxFQUEyQjtZQUFuRiwwQkFBVTtZQUFnRCxvQkFBTztRQUN6RixPQUFPLElBQUksQ0FBQywyQkFBMkIsQ0FBQyxjQUFjLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUNsRSxHQUFHOzs7O1FBQUMsVUFBQSxrQkFBa0I7WUFDcEIsT0FBQSxVQUFVLENBQUM7Z0JBQ1QsYUFBYSxFQUFFLGtCQUFrQjthQUNsQyxDQUFDO1FBRkYsQ0FFRSxFQUNILENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELDhEQUEwQjs7Ozs7SUFBMUIsVUFBMkIsQ0FBQyxFQUFFLEVBQThCO1lBQTVCLG9CQUFPO1FBQ3JDLE9BQU8sSUFBSSxDQUFDLDJCQUEyQixDQUFDLGlCQUFpQixDQUFDLE9BQU8sQ0FBQyxDQUFDO0lBQ3JFLENBQUM7O2dCQWhCZ0QsMkJBQTJCOztJQUc1RTtRQURDLE1BQU0sQ0FBQyxjQUFjLENBQUM7O3lEQUN3RSxjQUFjOzs0RUFRNUc7SUFHRDtRQURDLE1BQU0sQ0FBQyxpQkFBaUIsQ0FBQzs7eURBQ2lCLGlCQUFpQjs7K0VBRTNEO0lBekJEO1FBREMsUUFBUSxFQUFFOzs7OzhEQUdWO0lBR0Q7UUFEQyxRQUFRLEVBQUU7Ozs7Z0VBR1Y7SUFUVSx5QkFBeUI7UUFKckMsS0FBSyxDQUE2QjtZQUNqQyxJQUFJLEVBQUUsMkJBQTJCO1lBQ2pDLFFBQVEsRUFBRSxtQkFBQSxFQUFFLGFBQWEsRUFBRSxFQUFFLEVBQUUsRUFBOEI7U0FDOUQsQ0FBQztpREFZaUQsMkJBQTJCO09BWGpFLHlCQUF5QixDQTRCckM7SUFBRCxnQ0FBQztDQUFBLElBQUE7U0E1QlkseUJBQXlCOzs7Ozs7SUFXeEIsZ0VBQWdFIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgU3RhdGUsIEFjdGlvbiwgU3RhdGVDb250ZXh0LCBTZWxlY3RvciB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgR2V0UGVybWlzc2lvbnMsIFVwZGF0ZVBlcm1pc3Npb25zIH0gZnJvbSAnLi4vYWN0aW9ucy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQuYWN0aW9ucyc7XHJcbmltcG9ydCB7IFBlcm1pc3Npb25NYW5hZ2VtZW50IH0gZnJvbSAnLi4vbW9kZWxzL3Blcm1pc3Npb24tbWFuYWdlbWVudCc7XHJcbmltcG9ydCB7IFBlcm1pc3Npb25NYW5hZ2VtZW50U2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL3Blcm1pc3Npb24tbWFuYWdlbWVudC5zZXJ2aWNlJztcclxuaW1wb3J0IHsgdGFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5cclxuQFN0YXRlPFBlcm1pc3Npb25NYW5hZ2VtZW50LlN0YXRlPih7XHJcbiAgbmFtZTogJ1Blcm1pc3Npb25NYW5hZ2VtZW50U3RhdGUnLFxyXG4gIGRlZmF1bHRzOiB7IHBlcm1pc3Npb25SZXM6IHt9IH0gYXMgUGVybWlzc2lvbk1hbmFnZW1lbnQuU3RhdGUsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlIHtcclxuICBAU2VsZWN0b3IoKVxyXG4gIHN0YXRpYyBnZXRQZXJtaXNzaW9uR3JvdXBzKHsgcGVybWlzc2lvblJlcyB9OiBQZXJtaXNzaW9uTWFuYWdlbWVudC5TdGF0ZSkge1xyXG4gICAgcmV0dXJuIHBlcm1pc3Npb25SZXMuZ3JvdXBzIHx8IFtdO1xyXG4gIH1cclxuXHJcbiAgQFNlbGVjdG9yKClcclxuICBzdGF0aWMgZ2V0RW50aXRpeURpc3BsYXlOYW1lKHsgcGVybWlzc2lvblJlcyB9OiBQZXJtaXNzaW9uTWFuYWdlbWVudC5TdGF0ZSk6IHN0cmluZyB7XHJcbiAgICByZXR1cm4gcGVybWlzc2lvblJlcy5lbnRpdHlEaXNwbGF5TmFtZTtcclxuICB9XHJcblxyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcGVybWlzc2lvbk1hbmFnZW1lbnRTZXJ2aWNlOiBQZXJtaXNzaW9uTWFuYWdlbWVudFNlcnZpY2UpIHt9XHJcblxyXG4gIEBBY3Rpb24oR2V0UGVybWlzc2lvbnMpXHJcbiAgcGVybWlzc2lvbk1hbmFnZW1lbnRHZXQoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxQZXJtaXNzaW9uTWFuYWdlbWVudC5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRQZXJtaXNzaW9ucykge1xyXG4gICAgcmV0dXJuIHRoaXMucGVybWlzc2lvbk1hbmFnZW1lbnRTZXJ2aWNlLmdldFBlcm1pc3Npb25zKHBheWxvYWQpLnBpcGUoXHJcbiAgICAgIHRhcChwZXJtaXNzaW9uUmVzcG9uc2UgPT5cclxuICAgICAgICBwYXRjaFN0YXRlKHtcclxuICAgICAgICAgIHBlcm1pc3Npb25SZXM6IHBlcm1pc3Npb25SZXNwb25zZSxcclxuICAgICAgICB9KSxcclxuICAgICAgKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKFVwZGF0ZVBlcm1pc3Npb25zKVxyXG4gIHBlcm1pc3Npb25NYW5hZ2VtZW50VXBkYXRlKF8sIHsgcGF5bG9hZCB9OiBVcGRhdGVQZXJtaXNzaW9ucykge1xyXG4gICAgcmV0dXJuIHRoaXMucGVybWlzc2lvbk1hbmFnZW1lbnRTZXJ2aWNlLnVwZGF0ZVBlcm1pc3Npb25zKHBheWxvYWQpO1xyXG4gIH1cclxufVxyXG4iXX0=