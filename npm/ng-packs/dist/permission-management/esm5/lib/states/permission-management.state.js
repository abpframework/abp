/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { State, Action, Selector } from '@ngxs/store';
import { PermissionManagementGetPermissions, PermissionManagementUpdatePermissions, } from '../actions/permission-management.actions';
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
        Action(PermissionManagementGetPermissions),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object, PermissionManagementGetPermissions]),
        tslib_1.__metadata("design:returntype", void 0)
    ], PermissionManagementState.prototype, "permissionManagementGet", null);
    tslib_1.__decorate([
        Action(PermissionManagementUpdatePermissions),
        tslib_1.__metadata("design:type", Function),
        tslib_1.__metadata("design:paramtypes", [Object, PermissionManagementUpdatePermissions]),
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5wZXJtaXNzaW9uLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL3Blcm1pc3Npb24tbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFnQixRQUFRLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUNMLGtDQUFrQyxFQUNsQyxxQ0FBcUMsR0FDdEMsTUFBTSwwQ0FBMEMsQ0FBQztBQUVsRCxPQUFPLEVBQUUsMkJBQTJCLEVBQUUsTUFBTSwyQ0FBMkMsQ0FBQztBQUN4RixPQUFPLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7O0lBaUJuQyxtQ0FBb0IsMkJBQXdEO1FBQXhELGdDQUEyQixHQUEzQiwyQkFBMkIsQ0FBNkI7SUFBRyxDQUFDOzs7OztJQVR6RSw2Q0FBbUI7Ozs7SUFBMUIsVUFBMkIsRUFBNkM7WUFBM0MsZ0NBQWE7UUFDeEMsT0FBTyxhQUFhLENBQUMsTUFBTSxJQUFJLEVBQUUsQ0FBQztJQUNwQyxDQUFDOzs7OztJQUdNLCtDQUFxQjs7OztJQUE1QixVQUE2QixFQUE2QztZQUEzQyxnQ0FBYTtRQUMxQyxPQUFPLGFBQWEsQ0FBQyxpQkFBaUIsQ0FBQztJQUN6QyxDQUFDOzs7Ozs7SUFLRCwyREFBdUI7Ozs7O0lBQXZCLFVBQ0UsRUFBd0QsRUFDeEQsRUFBK0M7WUFEN0MsMEJBQVU7WUFDVixvQkFBTztRQUVULE9BQU8sSUFBSSxDQUFDLDJCQUEyQixDQUFDLGNBQWMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQ2xFLEdBQUc7Ozs7UUFBQyxVQUFBLGtCQUFrQjtZQUNwQixPQUFBLFVBQVUsQ0FBQztnQkFDVCxhQUFhLEVBQUUsa0JBQWtCO2FBQ2xDLENBQUM7UUFGRixDQUVFLEVBQ0gsQ0FDRixDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0QsOERBQTBCOzs7OztJQUExQixVQUEyQixDQUFDLEVBQUUsRUFBa0Q7WUFBaEQsb0JBQU87UUFDckMsT0FBTyxJQUFJLENBQUMsMkJBQTJCLENBQUMsaUJBQWlCLENBQUMsT0FBTyxDQUFDLENBQUM7SUFDckUsQ0FBQztJQWhCRDtRQURDLE1BQU0sQ0FBQyxrQ0FBa0MsQ0FBQzs7eURBRzVCLGtDQUFrQzs7NEVBU2hEO0lBR0Q7UUFEQyxNQUFNLENBQUMscUNBQXFDLENBQUM7O3lEQUNILHFDQUFxQzs7K0VBRS9FO0lBNUJEO1FBREMsUUFBUSxFQUFFOzs7OzhEQUdWO0lBR0Q7UUFEQyxRQUFRLEVBQUU7Ozs7Z0VBR1Y7SUFUVSx5QkFBeUI7UUFKckMsS0FBSyxDQUE2QjtZQUNqQyxJQUFJLEVBQUUsMkJBQTJCO1lBQ2pDLFFBQVEsRUFBRSxtQkFBQSxFQUFFLGFBQWEsRUFBRSxFQUFFLEVBQUUsRUFBOEI7U0FDOUQsQ0FBQztpREFZaUQsMkJBQTJCO09BWGpFLHlCQUF5QixDQStCckM7SUFBRCxnQ0FBQztDQUFBLElBQUE7U0EvQlkseUJBQXlCOzs7Ozs7SUFXeEIsZ0VBQWdFIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgU3RhdGUsIEFjdGlvbiwgU3RhdGVDb250ZXh0LCBTZWxlY3RvciB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7XG4gIFBlcm1pc3Npb25NYW5hZ2VtZW50R2V0UGVybWlzc2lvbnMsXG4gIFBlcm1pc3Npb25NYW5hZ2VtZW50VXBkYXRlUGVybWlzc2lvbnMsXG59IGZyb20gJy4uL2FjdGlvbnMvcGVybWlzc2lvbi1tYW5hZ2VtZW50LmFjdGlvbnMnO1xuaW1wb3J0IHsgUGVybWlzc2lvbk1hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMvcGVybWlzc2lvbi1tYW5hZ2VtZW50JztcbmltcG9ydCB7IFBlcm1pc3Npb25NYW5hZ2VtZW50U2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL3Blcm1pc3Npb24tbWFuYWdlbWVudC5zZXJ2aWNlJztcbmltcG9ydCB7IHRhcCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcblxuQFN0YXRlPFBlcm1pc3Npb25NYW5hZ2VtZW50LlN0YXRlPih7XG4gIG5hbWU6ICdQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlJyxcbiAgZGVmYXVsdHM6IHsgcGVybWlzc2lvblJlczoge30gfSBhcyBQZXJtaXNzaW9uTWFuYWdlbWVudC5TdGF0ZSxcbn0pXG5leHBvcnQgY2xhc3MgUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZSB7XG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRQZXJtaXNzaW9uR3JvdXBzKHsgcGVybWlzc2lvblJlcyB9OiBQZXJtaXNzaW9uTWFuYWdlbWVudC5TdGF0ZSkge1xuICAgIHJldHVybiBwZXJtaXNzaW9uUmVzLmdyb3VwcyB8fCBbXTtcbiAgfVxuXG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRFbnRpdGl5RGlzcGxheU5hbWUoeyBwZXJtaXNzaW9uUmVzIH06IFBlcm1pc3Npb25NYW5hZ2VtZW50LlN0YXRlKTogc3RyaW5nIHtcbiAgICByZXR1cm4gcGVybWlzc2lvblJlcy5lbnRpdHlEaXNwbGF5TmFtZTtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcGVybWlzc2lvbk1hbmFnZW1lbnRTZXJ2aWNlOiBQZXJtaXNzaW9uTWFuYWdlbWVudFNlcnZpY2UpIHt9XG5cbiAgQEFjdGlvbihQZXJtaXNzaW9uTWFuYWdlbWVudEdldFBlcm1pc3Npb25zKVxuICBwZXJtaXNzaW9uTWFuYWdlbWVudEdldChcbiAgICB7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFBlcm1pc3Npb25NYW5hZ2VtZW50LlN0YXRlPixcbiAgICB7IHBheWxvYWQgfTogUGVybWlzc2lvbk1hbmFnZW1lbnRHZXRQZXJtaXNzaW9ucyxcbiAgKSB7XG4gICAgcmV0dXJuIHRoaXMucGVybWlzc2lvbk1hbmFnZW1lbnRTZXJ2aWNlLmdldFBlcm1pc3Npb25zKHBheWxvYWQpLnBpcGUoXG4gICAgICB0YXAocGVybWlzc2lvblJlc3BvbnNlID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHBlcm1pc3Npb25SZXM6IHBlcm1pc3Npb25SZXNwb25zZSxcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKFBlcm1pc3Npb25NYW5hZ2VtZW50VXBkYXRlUGVybWlzc2lvbnMpXG4gIHBlcm1pc3Npb25NYW5hZ2VtZW50VXBkYXRlKF8sIHsgcGF5bG9hZCB9OiBQZXJtaXNzaW9uTWFuYWdlbWVudFVwZGF0ZVBlcm1pc3Npb25zKSB7XG4gICAgcmV0dXJuIHRoaXMucGVybWlzc2lvbk1hbmFnZW1lbnRTZXJ2aWNlLnVwZGF0ZVBlcm1pc3Npb25zKHBheWxvYWQpO1xuICB9XG59XG4iXX0=