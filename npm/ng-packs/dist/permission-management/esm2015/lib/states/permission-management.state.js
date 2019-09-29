/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LnN0YXRlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5wZXJtaXNzaW9uLW1hbmFnZW1lbnQvIiwic291cmNlcyI6WyJsaWIvc3RhdGVzL3Blcm1pc3Npb24tbWFuYWdlbWVudC5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFnQixRQUFRLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLGNBQWMsRUFBRSxpQkFBaUIsRUFBRSxNQUFNLDBDQUEwQyxDQUFDO0FBRTdGLE9BQU8sRUFBRSwyQkFBMkIsRUFBRSxNQUFNLDJDQUEyQyxDQUFDO0FBQ3hGLE9BQU8sRUFBRSxHQUFHLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztJQU14Qix5QkFBeUIsU0FBekIseUJBQXlCOzs7O0lBV3BDLFlBQW9CLDJCQUF3RDtRQUF4RCxnQ0FBMkIsR0FBM0IsMkJBQTJCLENBQTZCO0lBQUcsQ0FBQzs7Ozs7SUFUaEYsTUFBTSxDQUFDLG1CQUFtQixDQUFDLEVBQUUsYUFBYSxFQUE4QjtRQUN0RSxPQUFPLGFBQWEsQ0FBQyxNQUFNLElBQUksRUFBRSxDQUFDO0lBQ3BDLENBQUM7Ozs7O0lBR0QsTUFBTSxDQUFDLHFCQUFxQixDQUFDLEVBQUUsYUFBYSxFQUE4QjtRQUN4RSxPQUFPLGFBQWEsQ0FBQyxpQkFBaUIsQ0FBQztJQUN6QyxDQUFDOzs7Ozs7SUFLRCx1QkFBdUIsQ0FBQyxFQUFFLFVBQVUsRUFBNEMsRUFBRSxFQUFFLE9BQU8sRUFBa0I7UUFDM0csT0FBTyxJQUFJLENBQUMsMkJBQTJCLENBQUMsY0FBYyxDQUFDLE9BQU8sQ0FBQyxDQUFDLElBQUksQ0FDbEUsR0FBRzs7OztRQUFDLGtCQUFrQixDQUFDLEVBQUUsQ0FDdkIsVUFBVSxDQUFDO1lBQ1QsYUFBYSxFQUFFLGtCQUFrQjtTQUNsQyxDQUFDLEVBQ0gsQ0FDRixDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0QsMEJBQTBCLENBQUMsQ0FBQyxFQUFFLEVBQUUsT0FBTyxFQUFxQjtRQUMxRCxPQUFPLElBQUksQ0FBQywyQkFBMkIsQ0FBQyxpQkFBaUIsQ0FBQyxPQUFPLENBQUMsQ0FBQztJQUNyRSxDQUFDO0NBQ0YsQ0FBQTtBQWRDO0lBREMsTUFBTSxDQUFDLGNBQWMsQ0FBQzs7cURBQ3dFLGNBQWM7O3dFQVE1RztBQUdEO0lBREMsTUFBTSxDQUFDLGlCQUFpQixDQUFDOztxREFDaUIsaUJBQWlCOzsyRUFFM0Q7QUF6QkQ7SUFEQyxRQUFRLEVBQUU7Ozs7MERBR1Y7QUFHRDtJQURDLFFBQVEsRUFBRTs7Ozs0REFHVjtBQVRVLHlCQUF5QjtJQUpyQyxLQUFLLENBQTZCO1FBQ2pDLElBQUksRUFBRSwyQkFBMkI7UUFDakMsUUFBUSxFQUFFLG1CQUFBLEVBQUUsYUFBYSxFQUFFLEVBQUUsRUFBRSxFQUE4QjtLQUM5RCxDQUFDOzZDQVlpRCwyQkFBMkI7R0FYakUseUJBQXlCLENBNEJyQztTQTVCWSx5QkFBeUI7Ozs7OztJQVd4QixnRUFBZ0UiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBTdGF0ZSwgQWN0aW9uLCBTdGF0ZUNvbnRleHQsIFNlbGVjdG9yIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgR2V0UGVybWlzc2lvbnMsIFVwZGF0ZVBlcm1pc3Npb25zIH0gZnJvbSAnLi4vYWN0aW9ucy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQuYWN0aW9ucyc7XG5pbXBvcnQgeyBQZXJtaXNzaW9uTWFuYWdlbWVudCB9IGZyb20gJy4uL21vZGVscy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQnO1xuaW1wb3J0IHsgUGVybWlzc2lvbk1hbmFnZW1lbnRTZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvcGVybWlzc2lvbi1tYW5hZ2VtZW50LnNlcnZpY2UnO1xuaW1wb3J0IHsgdGFwIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuXG5AU3RhdGU8UGVybWlzc2lvbk1hbmFnZW1lbnQuU3RhdGU+KHtcbiAgbmFtZTogJ1Blcm1pc3Npb25NYW5hZ2VtZW50U3RhdGUnLFxuICBkZWZhdWx0czogeyBwZXJtaXNzaW9uUmVzOiB7fSB9IGFzIFBlcm1pc3Npb25NYW5hZ2VtZW50LlN0YXRlLFxufSlcbmV4cG9ydCBjbGFzcyBQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlIHtcbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldFBlcm1pc3Npb25Hcm91cHMoeyBwZXJtaXNzaW9uUmVzIH06IFBlcm1pc3Npb25NYW5hZ2VtZW50LlN0YXRlKSB7XG4gICAgcmV0dXJuIHBlcm1pc3Npb25SZXMuZ3JvdXBzIHx8IFtdO1xuICB9XG5cbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldEVudGl0aXlEaXNwbGF5TmFtZSh7IHBlcm1pc3Npb25SZXMgfTogUGVybWlzc2lvbk1hbmFnZW1lbnQuU3RhdGUpOiBzdHJpbmcge1xuICAgIHJldHVybiBwZXJtaXNzaW9uUmVzLmVudGl0eURpc3BsYXlOYW1lO1xuICB9XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBwZXJtaXNzaW9uTWFuYWdlbWVudFNlcnZpY2U6IFBlcm1pc3Npb25NYW5hZ2VtZW50U2VydmljZSkge31cblxuICBAQWN0aW9uKEdldFBlcm1pc3Npb25zKVxuICBwZXJtaXNzaW9uTWFuYWdlbWVudEdldCh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PFBlcm1pc3Npb25NYW5hZ2VtZW50LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFBlcm1pc3Npb25zKSB7XG4gICAgcmV0dXJuIHRoaXMucGVybWlzc2lvbk1hbmFnZW1lbnRTZXJ2aWNlLmdldFBlcm1pc3Npb25zKHBheWxvYWQpLnBpcGUoXG4gICAgICB0YXAocGVybWlzc2lvblJlc3BvbnNlID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHBlcm1pc3Npb25SZXM6IHBlcm1pc3Npb25SZXNwb25zZSxcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKFVwZGF0ZVBlcm1pc3Npb25zKVxuICBwZXJtaXNzaW9uTWFuYWdlbWVudFVwZGF0ZShfLCB7IHBheWxvYWQgfTogVXBkYXRlUGVybWlzc2lvbnMpIHtcbiAgICByZXR1cm4gdGhpcy5wZXJtaXNzaW9uTWFuYWdlbWVudFNlcnZpY2UudXBkYXRlUGVybWlzc2lvbnMocGF5bG9hZCk7XG4gIH1cbn1cbiJdfQ==