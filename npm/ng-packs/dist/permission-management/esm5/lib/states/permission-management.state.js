/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5wZXJtaXNzaW9uLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL3Blcm1pc3Npb24tbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFnQixRQUFRLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLGNBQWMsRUFBRSxpQkFBaUIsRUFBRSxNQUFNLDBDQUEwQyxDQUFDO0FBRTdGLE9BQU8sRUFBRSwyQkFBMkIsRUFBRSxNQUFNLDJDQUEyQyxDQUFDO0FBQ3hGLE9BQU8sRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQzs7SUFpQm5DLG1DQUFvQiwyQkFBd0Q7UUFBeEQsZ0NBQTJCLEdBQTNCLDJCQUEyQixDQUE2QjtJQUFHLENBQUM7Ozs7O0lBVHpFLDZDQUFtQjs7OztJQUExQixVQUEyQixFQUE2QztZQUEzQyxnQ0FBYTtRQUN4QyxPQUFPLGFBQWEsQ0FBQyxNQUFNLElBQUksRUFBRSxDQUFDO0lBQ3BDLENBQUM7Ozs7O0lBR00sK0NBQXFCOzs7O0lBQTVCLFVBQTZCLEVBQTZDO1lBQTNDLGdDQUFhO1FBQzFDLE9BQU8sYUFBYSxDQUFDLGlCQUFpQixDQUFDO0lBQ3pDLENBQUM7Ozs7OztJQUtELDJEQUF1Qjs7Ozs7SUFBdkIsVUFBd0IsRUFBd0QsRUFBRSxFQUEyQjtZQUFuRiwwQkFBVTtZQUFnRCxvQkFBTztRQUN6RixPQUFPLElBQUksQ0FBQywyQkFBMkIsQ0FBQyxjQUFjLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUNsRSxHQUFHOzs7O1FBQUMsVUFBQSxrQkFBa0I7WUFDcEIsT0FBQSxVQUFVLENBQUM7Z0JBQ1QsYUFBYSxFQUFFLGtCQUFrQjthQUNsQyxDQUFDO1FBRkYsQ0FFRSxFQUNILENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELDhEQUEwQjs7Ozs7SUFBMUIsVUFBMkIsQ0FBQyxFQUFFLEVBQThCO1lBQTVCLG9CQUFPO1FBQ3JDLE9BQU8sSUFBSSxDQUFDLDJCQUEyQixDQUFDLGlCQUFpQixDQUFDLE9BQU8sQ0FBQyxDQUFDO0lBQ3JFLENBQUM7SUFiRDtRQURDLE1BQU0sQ0FBQyxjQUFjLENBQUM7O3lEQUN3RSxjQUFjOzs0RUFRNUc7SUFHRDtRQURDLE1BQU0sQ0FBQyxpQkFBaUIsQ0FBQzs7eURBQ2lCLGlCQUFpQjs7K0VBRTNEO0lBekJEO1FBREMsUUFBUSxFQUFFOzs7OzhEQUdWO0lBR0Q7UUFEQyxRQUFRLEVBQUU7Ozs7Z0VBR1Y7SUFUVSx5QkFBeUI7UUFKckMsS0FBSyxDQUE2QjtZQUNqQyxJQUFJLEVBQUUsMkJBQTJCO1lBQ2pDLFFBQVEsRUFBRSxtQkFBQSxFQUFFLGFBQWEsRUFBRSxFQUFFLEVBQUUsRUFBOEI7U0FDOUQsQ0FBQztpREFZaUQsMkJBQTJCO09BWGpFLHlCQUF5QixDQTRCckM7SUFBRCxnQ0FBQztDQUFBLElBQUE7U0E1QlkseUJBQXlCOzs7Ozs7SUFXeEIsZ0VBQWdFIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgU3RhdGUsIEFjdGlvbiwgU3RhdGVDb250ZXh0LCBTZWxlY3RvciB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IEdldFBlcm1pc3Npb25zLCBVcGRhdGVQZXJtaXNzaW9ucyB9IGZyb20gJy4uL2FjdGlvbnMvcGVybWlzc2lvbi1tYW5hZ2VtZW50LmFjdGlvbnMnO1xuaW1wb3J0IHsgUGVybWlzc2lvbk1hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMvcGVybWlzc2lvbi1tYW5hZ2VtZW50JztcbmltcG9ydCB7IFBlcm1pc3Npb25NYW5hZ2VtZW50U2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL3Blcm1pc3Npb24tbWFuYWdlbWVudC5zZXJ2aWNlJztcbmltcG9ydCB7IHRhcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcblxuQFN0YXRlPFBlcm1pc3Npb25NYW5hZ2VtZW50LlN0YXRlPih7XG4gIG5hbWU6ICdQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlJyxcbiAgZGVmYXVsdHM6IHsgcGVybWlzc2lvblJlczoge30gfSBhcyBQZXJtaXNzaW9uTWFuYWdlbWVudC5TdGF0ZSxcbn0pXG5leHBvcnQgY2xhc3MgUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZSB7XG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRQZXJtaXNzaW9uR3JvdXBzKHsgcGVybWlzc2lvblJlcyB9OiBQZXJtaXNzaW9uTWFuYWdlbWVudC5TdGF0ZSkge1xuICAgIHJldHVybiBwZXJtaXNzaW9uUmVzLmdyb3VwcyB8fCBbXTtcbiAgfVxuXG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRFbnRpdGl5RGlzcGxheU5hbWUoeyBwZXJtaXNzaW9uUmVzIH06IFBlcm1pc3Npb25NYW5hZ2VtZW50LlN0YXRlKTogc3RyaW5nIHtcbiAgICByZXR1cm4gcGVybWlzc2lvblJlcy5lbnRpdHlEaXNwbGF5TmFtZTtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcGVybWlzc2lvbk1hbmFnZW1lbnRTZXJ2aWNlOiBQZXJtaXNzaW9uTWFuYWdlbWVudFNlcnZpY2UpIHt9XG5cbiAgQEFjdGlvbihHZXRQZXJtaXNzaW9ucylcbiAgcGVybWlzc2lvbk1hbmFnZW1lbnRHZXQoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxQZXJtaXNzaW9uTWFuYWdlbWVudC5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRQZXJtaXNzaW9ucykge1xuICAgIHJldHVybiB0aGlzLnBlcm1pc3Npb25NYW5hZ2VtZW50U2VydmljZS5nZXRQZXJtaXNzaW9ucyhwYXlsb2FkKS5waXBlKFxuICAgICAgdGFwKHBlcm1pc3Npb25SZXNwb25zZSA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICBwZXJtaXNzaW9uUmVzOiBwZXJtaXNzaW9uUmVzcG9uc2UsXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihVcGRhdGVQZXJtaXNzaW9ucylcbiAgcGVybWlzc2lvbk1hbmFnZW1lbnRVcGRhdGUoXywgeyBwYXlsb2FkIH06IFVwZGF0ZVBlcm1pc3Npb25zKSB7XG4gICAgcmV0dXJuIHRoaXMucGVybWlzc2lvbk1hbmFnZW1lbnRTZXJ2aWNlLnVwZGF0ZVBlcm1pc3Npb25zKHBheWxvYWQpO1xuICB9XG59XG4iXX0=