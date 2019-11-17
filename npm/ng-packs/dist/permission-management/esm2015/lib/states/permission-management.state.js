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
    static getEntityDisplayName({ permissionRes }) {
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
], PermissionManagementState, "getEntityDisplayName", null);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5wZXJtaXNzaW9uLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL3Blcm1pc3Npb24tbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sRUFBZ0IsUUFBUSxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxjQUFjLEVBQUUsaUJBQWlCLEVBQUUsTUFBTSwwQ0FBMEMsQ0FBQztBQUU3RixPQUFPLEVBQUUsMkJBQTJCLEVBQUUsTUFBTSwyQ0FBMkMsQ0FBQztBQUN4RixPQUFPLEVBQUUsR0FBRyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7SUFNeEIseUJBQXlCLFNBQXpCLHlCQUF5Qjs7OztJQVdwQyxZQUFvQiwyQkFBd0Q7UUFBeEQsZ0NBQTJCLEdBQTNCLDJCQUEyQixDQUE2QjtJQUFHLENBQUM7Ozs7O0lBVGhGLE1BQU0sQ0FBQyxtQkFBbUIsQ0FBQyxFQUFFLGFBQWEsRUFBOEI7UUFDdEUsT0FBTyxhQUFhLENBQUMsTUFBTSxJQUFJLEVBQUUsQ0FBQztJQUNwQyxDQUFDOzs7OztJQUdELE1BQU0sQ0FBQyxvQkFBb0IsQ0FBQyxFQUFFLGFBQWEsRUFBOEI7UUFDdkUsT0FBTyxhQUFhLENBQUMsaUJBQWlCLENBQUM7SUFDekMsQ0FBQzs7Ozs7O0lBS0QsdUJBQXVCLENBQUMsRUFBRSxVQUFVLEVBQTRDLEVBQUUsRUFBRSxPQUFPLEVBQWtCO1FBQzNHLE9BQU8sSUFBSSxDQUFDLDJCQUEyQixDQUFDLGNBQWMsQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQ2xFLEdBQUc7Ozs7UUFBQyxrQkFBa0IsQ0FBQyxFQUFFLENBQ3ZCLFVBQVUsQ0FBQztZQUNULGFBQWEsRUFBRSxrQkFBa0I7U0FDbEMsQ0FBQyxFQUNILENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELDBCQUEwQixDQUFDLENBQUMsRUFBRSxFQUFFLE9BQU8sRUFBcUI7UUFDMUQsT0FBTyxJQUFJLENBQUMsMkJBQTJCLENBQUMsaUJBQWlCLENBQUMsT0FBTyxDQUFDLENBQUM7SUFDckUsQ0FBQztDQUNGLENBQUE7O1lBakJrRCwyQkFBMkI7O0FBRzVFO0lBREMsTUFBTSxDQUFDLGNBQWMsQ0FBQzs7cURBQ3dFLGNBQWM7O3dFQVE1RztBQUdEO0lBREMsTUFBTSxDQUFDLGlCQUFpQixDQUFDOztxREFDaUIsaUJBQWlCOzsyRUFFM0Q7QUF6QkQ7SUFEQyxRQUFRLEVBQUU7Ozs7MERBR1Y7QUFHRDtJQURDLFFBQVEsRUFBRTs7OzsyREFHVjtBQVRVLHlCQUF5QjtJQUpyQyxLQUFLLENBQTZCO1FBQ2pDLElBQUksRUFBRSwyQkFBMkI7UUFDakMsUUFBUSxFQUFFLG1CQUFBLEVBQUUsYUFBYSxFQUFFLEVBQUUsRUFBRSxFQUE4QjtLQUM5RCxDQUFDOzZDQVlpRCwyQkFBMkI7R0FYakUseUJBQXlCLENBNEJyQztTQTVCWSx5QkFBeUI7Ozs7OztJQVd4QixnRUFBZ0UiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBTdGF0ZSwgQWN0aW9uLCBTdGF0ZUNvbnRleHQsIFNlbGVjdG9yIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBHZXRQZXJtaXNzaW9ucywgVXBkYXRlUGVybWlzc2lvbnMgfSBmcm9tICcuLi9hY3Rpb25zL3Blcm1pc3Npb24tbWFuYWdlbWVudC5hY3Rpb25zJztcclxuaW1wb3J0IHsgUGVybWlzc2lvbk1hbmFnZW1lbnQgfSBmcm9tICcuLi9tb2RlbHMvcGVybWlzc2lvbi1tYW5hZ2VtZW50JztcclxuaW1wb3J0IHsgUGVybWlzc2lvbk1hbmFnZW1lbnRTZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvcGVybWlzc2lvbi1tYW5hZ2VtZW50LnNlcnZpY2UnO1xyXG5pbXBvcnQgeyB0YXAgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XHJcblxyXG5AU3RhdGU8UGVybWlzc2lvbk1hbmFnZW1lbnQuU3RhdGU+KHtcclxuICBuYW1lOiAnUGVybWlzc2lvbk1hbmFnZW1lbnRTdGF0ZScsXHJcbiAgZGVmYXVsdHM6IHsgcGVybWlzc2lvblJlczoge30gfSBhcyBQZXJtaXNzaW9uTWFuYWdlbWVudC5TdGF0ZSxcclxufSlcclxuZXhwb3J0IGNsYXNzIFBlcm1pc3Npb25NYW5hZ2VtZW50U3RhdGUge1xyXG4gIEBTZWxlY3RvcigpXHJcbiAgc3RhdGljIGdldFBlcm1pc3Npb25Hcm91cHMoeyBwZXJtaXNzaW9uUmVzIH06IFBlcm1pc3Npb25NYW5hZ2VtZW50LlN0YXRlKSB7XHJcbiAgICByZXR1cm4gcGVybWlzc2lvblJlcy5ncm91cHMgfHwgW107XHJcbiAgfVxyXG5cclxuICBAU2VsZWN0b3IoKVxyXG4gIHN0YXRpYyBnZXRFbnRpdHlEaXNwbGF5TmFtZSh7IHBlcm1pc3Npb25SZXMgfTogUGVybWlzc2lvbk1hbmFnZW1lbnQuU3RhdGUpOiBzdHJpbmcge1xyXG4gICAgcmV0dXJuIHBlcm1pc3Npb25SZXMuZW50aXR5RGlzcGxheU5hbWU7XHJcbiAgfVxyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHBlcm1pc3Npb25NYW5hZ2VtZW50U2VydmljZTogUGVybWlzc2lvbk1hbmFnZW1lbnRTZXJ2aWNlKSB7fVxyXG5cclxuICBAQWN0aW9uKEdldFBlcm1pc3Npb25zKVxyXG4gIHBlcm1pc3Npb25NYW5hZ2VtZW50R2V0KHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8UGVybWlzc2lvbk1hbmFnZW1lbnQuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0UGVybWlzc2lvbnMpIHtcclxuICAgIHJldHVybiB0aGlzLnBlcm1pc3Npb25NYW5hZ2VtZW50U2VydmljZS5nZXRQZXJtaXNzaW9ucyhwYXlsb2FkKS5waXBlKFxyXG4gICAgICB0YXAocGVybWlzc2lvblJlc3BvbnNlID0+XHJcbiAgICAgICAgcGF0Y2hTdGF0ZSh7XHJcbiAgICAgICAgICBwZXJtaXNzaW9uUmVzOiBwZXJtaXNzaW9uUmVzcG9uc2UsXHJcbiAgICAgICAgfSksXHJcbiAgICAgICksXHJcbiAgICApO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihVcGRhdGVQZXJtaXNzaW9ucylcclxuICBwZXJtaXNzaW9uTWFuYWdlbWVudFVwZGF0ZShfLCB7IHBheWxvYWQgfTogVXBkYXRlUGVybWlzc2lvbnMpIHtcclxuICAgIHJldHVybiB0aGlzLnBlcm1pc3Npb25NYW5hZ2VtZW50U2VydmljZS51cGRhdGVQZXJtaXNzaW9ucyhwYXlsb2FkKTtcclxuICB9XHJcbn1cclxuIl19