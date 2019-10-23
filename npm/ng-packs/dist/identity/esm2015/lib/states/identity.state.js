/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Action, Selector, State } from '@ngxs/store';
import { switchMap, tap, pluck } from 'rxjs/operators';
import { CreateRole, CreateUser, DeleteRole, DeleteUser, GetRoleById, GetRoles, GetUserById, GetUsers, UpdateRole, UpdateUser, GetUserRoles, } from '../actions/identity.actions';
import { IdentityService } from '../services/identity.service';
let IdentityState = class IdentityState {
    /**
     * @param {?} identityService
     */
    constructor(identityService) {
        this.identityService = identityService;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getRoles({ roles }) {
        return roles.items || [];
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getRolesTotalCount({ roles }) {
        return roles.totalCount || 0;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getUsers({ users }) {
        return users.items || [];
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getUsersTotalCount({ users }) {
        return users.totalCount || 0;
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    getRoles({ patchState }, { payload }) {
        return this.identityService.getRoles(payload).pipe(tap((/**
         * @param {?} roles
         * @return {?}
         */
        roles => patchState({
            roles,
        }))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    getRole({ patchState }, { payload }) {
        return this.identityService.getRoleById(payload).pipe(tap((/**
         * @param {?} selectedRole
         * @return {?}
         */
        selectedRole => patchState({
            selectedRole,
        }))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    deleteRole({ dispatch }, { payload }) {
        return this.identityService.deleteRole(payload).pipe(switchMap((/**
         * @return {?}
         */
        () => dispatch(new GetRoles()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    addRole({ dispatch }, { payload }) {
        return this.identityService.createRole(payload).pipe(switchMap((/**
         * @return {?}
         */
        () => dispatch(new GetRoles()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    updateRole({ getState, dispatch }, { payload }) {
        return dispatch(new GetRoleById(payload.id)).pipe(switchMap((/**
         * @return {?}
         */
        () => this.identityService.updateRole(Object.assign({}, getState().selectedRole, payload)))), switchMap((/**
         * @return {?}
         */
        () => dispatch(new GetRoles()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    getUsers({ patchState }, { payload }) {
        return this.identityService.getUsers(payload).pipe(tap((/**
         * @param {?} users
         * @return {?}
         */
        users => patchState({
            users,
        }))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    getUser({ patchState }, { payload }) {
        return this.identityService.getUserById(payload).pipe(tap((/**
         * @param {?} selectedUser
         * @return {?}
         */
        selectedUser => patchState({
            selectedUser,
        }))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    deleteUser({ dispatch }, { payload }) {
        return this.identityService.deleteUser(payload).pipe(switchMap((/**
         * @return {?}
         */
        () => dispatch(new GetUsers()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    addUser({ dispatch }, { payload }) {
        return this.identityService.createUser(payload).pipe(switchMap((/**
         * @return {?}
         */
        () => dispatch(new GetUsers()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    updateUser({ getState, dispatch }, { payload }) {
        return dispatch(new GetUserById(payload.id)).pipe(switchMap((/**
         * @return {?}
         */
        () => this.identityService.updateUser(Object.assign({}, getState().selectedUser, payload)))), switchMap((/**
         * @return {?}
         */
        () => dispatch(new GetUsers()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    getUserRoles({ patchState }, { payload }) {
        return this.identityService.getUserRoles(payload).pipe(pluck('items'), tap((/**
         * @param {?} selectedUserRoles
         * @return {?}
         */
        selectedUserRoles => patchState({
            selectedUserRoles,
        }))));
    }
};
IdentityState.ctorParameters = () => [
    { type: IdentityService }
];
tslib_1.__decorate([
    Action(GetRoles),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, GetRoles]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "getRoles", null);
tslib_1.__decorate([
    Action(GetRoleById),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, GetRoleById]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "getRole", null);
tslib_1.__decorate([
    Action(DeleteRole),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, GetRoleById]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "deleteRole", null);
tslib_1.__decorate([
    Action(CreateRole),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, CreateRole]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "addRole", null);
tslib_1.__decorate([
    Action(UpdateRole),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, UpdateRole]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "updateRole", null);
tslib_1.__decorate([
    Action(GetUsers),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, GetUsers]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "getUsers", null);
tslib_1.__decorate([
    Action(GetUserById),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, GetUserById]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "getUser", null);
tslib_1.__decorate([
    Action(DeleteUser),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, GetUserById]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "deleteUser", null);
tslib_1.__decorate([
    Action(CreateUser),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, CreateUser]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "addUser", null);
tslib_1.__decorate([
    Action(UpdateUser),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, UpdateUser]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "updateUser", null);
tslib_1.__decorate([
    Action(GetUserRoles),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, GetUserRoles]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "getUserRoles", null);
tslib_1.__decorate([
    Selector(),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object]),
    tslib_1.__metadata("design:returntype", Array)
], IdentityState, "getRoles", null);
tslib_1.__decorate([
    Selector(),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object]),
    tslib_1.__metadata("design:returntype", Number)
], IdentityState, "getRolesTotalCount", null);
tslib_1.__decorate([
    Selector(),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object]),
    tslib_1.__metadata("design:returntype", Array)
], IdentityState, "getUsers", null);
tslib_1.__decorate([
    Selector(),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object]),
    tslib_1.__metadata("design:returntype", Number)
], IdentityState, "getUsersTotalCount", null);
IdentityState = tslib_1.__decorate([
    State({
        name: 'IdentityState',
        defaults: (/** @type {?} */ ({ roles: {}, selectedRole: {}, users: {}, selectedUser: {} })),
    }),
    tslib_1.__metadata("design:paramtypes", [IdentityService])
], IdentityState);
export { IdentityState };
if (false) {
    /**
     * @type {?}
     * @private
     */
    IdentityState.prototype.identityService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkuc3RhdGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy9pZGVudGl0eS5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLFNBQVMsRUFBRSxHQUFHLEVBQUUsS0FBSyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDdkQsT0FBTyxFQUNMLFVBQVUsRUFDVixVQUFVLEVBQ1YsVUFBVSxFQUNWLFVBQVUsRUFDVixXQUFXLEVBQ1gsUUFBUSxFQUNSLFdBQVcsRUFDWCxRQUFRLEVBQ1IsVUFBVSxFQUNWLFVBQVUsRUFDVixZQUFZLEdBQ2IsTUFBTSw2QkFBNkIsQ0FBQztBQUVyQyxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sOEJBQThCLENBQUM7SUFNbEQsYUFBYSxTQUFiLGFBQWE7Ozs7SUFxQnhCLFlBQW9CLGVBQWdDO1FBQWhDLG9CQUFlLEdBQWYsZUFBZSxDQUFpQjtJQUFHLENBQUM7Ozs7O0lBbkJ4RCxNQUFNLENBQUMsUUFBUSxDQUFDLEVBQUUsS0FBSyxFQUFrQjtRQUN2QyxPQUFPLEtBQUssQ0FBQyxLQUFLLElBQUksRUFBRSxDQUFDO0lBQzNCLENBQUM7Ozs7O0lBR0QsTUFBTSxDQUFDLGtCQUFrQixDQUFDLEVBQUUsS0FBSyxFQUFrQjtRQUNqRCxPQUFPLEtBQUssQ0FBQyxVQUFVLElBQUksQ0FBQyxDQUFDO0lBQy9CLENBQUM7Ozs7O0lBR0QsTUFBTSxDQUFDLFFBQVEsQ0FBQyxFQUFFLEtBQUssRUFBa0I7UUFDdkMsT0FBTyxLQUFLLENBQUMsS0FBSyxJQUFJLEVBQUUsQ0FBQztJQUMzQixDQUFDOzs7OztJQUdELE1BQU0sQ0FBQyxrQkFBa0IsQ0FBQyxFQUFFLEtBQUssRUFBa0I7UUFDakQsT0FBTyxLQUFLLENBQUMsVUFBVSxJQUFJLENBQUMsQ0FBQztJQUMvQixDQUFDOzs7Ozs7SUFLRCxRQUFRLENBQUMsRUFBRSxVQUFVLEVBQWdDLEVBQUUsRUFBRSxPQUFPLEVBQVk7UUFDMUUsT0FBTyxJQUFJLENBQUMsZUFBZSxDQUFDLFFBQVEsQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQ2hELEdBQUc7Ozs7UUFBQyxLQUFLLENBQUMsRUFBRSxDQUNWLFVBQVUsQ0FBQztZQUNULEtBQUs7U0FDTixDQUFDLEVBQ0gsQ0FDRixDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0QsT0FBTyxDQUFDLEVBQUUsVUFBVSxFQUFnQyxFQUFFLEVBQUUsT0FBTyxFQUFlO1FBQzVFLE9BQU8sSUFBSSxDQUFDLGVBQWUsQ0FBQyxXQUFXLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUNuRCxHQUFHOzs7O1FBQUMsWUFBWSxDQUFDLEVBQUUsQ0FDakIsVUFBVSxDQUFDO1lBQ1QsWUFBWTtTQUNiLENBQUMsRUFDSCxDQUNGLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCxVQUFVLENBQUMsRUFBRSxRQUFRLEVBQWdDLEVBQUUsRUFBRSxPQUFPLEVBQWU7UUFDN0UsT0FBTyxJQUFJLENBQUMsZUFBZSxDQUFDLFVBQVUsQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQUMsU0FBUzs7O1FBQUMsR0FBRyxFQUFFLENBQUMsUUFBUSxDQUFDLElBQUksUUFBUSxFQUFFLENBQUMsRUFBQyxDQUFDLENBQUM7SUFDbEcsQ0FBQzs7Ozs7O0lBR0QsT0FBTyxDQUFDLEVBQUUsUUFBUSxFQUFnQyxFQUFFLEVBQUUsT0FBTyxFQUFjO1FBQ3pFLE9BQU8sSUFBSSxDQUFDLGVBQWUsQ0FBQyxVQUFVLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUFDLFNBQVM7OztRQUFDLEdBQUcsRUFBRSxDQUFDLFFBQVEsQ0FBQyxJQUFJLFFBQVEsRUFBRSxDQUFDLEVBQUMsQ0FBQyxDQUFDO0lBQ2xHLENBQUM7Ozs7OztJQUdELFVBQVUsQ0FBQyxFQUFFLFFBQVEsRUFBRSxRQUFRLEVBQWdDLEVBQUUsRUFBRSxPQUFPLEVBQWM7UUFDdEYsT0FBTyxRQUFRLENBQUMsSUFBSSxXQUFXLENBQUMsT0FBTyxDQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUMvQyxTQUFTOzs7UUFBQyxHQUFHLEVBQUUsQ0FBQyxJQUFJLENBQUMsZUFBZSxDQUFDLFVBQVUsbUJBQU0sUUFBUSxFQUFFLENBQUMsWUFBWSxFQUFLLE9BQU8sRUFBRyxFQUFDLEVBQzVGLFNBQVM7OztRQUFDLEdBQUcsRUFBRSxDQUFDLFFBQVEsQ0FBQyxJQUFJLFFBQVEsRUFBRSxDQUFDLEVBQUMsQ0FDMUMsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELFFBQVEsQ0FBQyxFQUFFLFVBQVUsRUFBZ0MsRUFBRSxFQUFFLE9BQU8sRUFBWTtRQUMxRSxPQUFPLElBQUksQ0FBQyxlQUFlLENBQUMsUUFBUSxDQUFDLE9BQU8sQ0FBQyxDQUFDLElBQUksQ0FDaEQsR0FBRzs7OztRQUFDLEtBQUssQ0FBQyxFQUFFLENBQ1YsVUFBVSxDQUFDO1lBQ1QsS0FBSztTQUNOLENBQUMsRUFDSCxDQUNGLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCxPQUFPLENBQUMsRUFBRSxVQUFVLEVBQWdDLEVBQUUsRUFBRSxPQUFPLEVBQWU7UUFDNUUsT0FBTyxJQUFJLENBQUMsZUFBZSxDQUFDLFdBQVcsQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQ25ELEdBQUc7Ozs7UUFBQyxZQUFZLENBQUMsRUFBRSxDQUNqQixVQUFVLENBQUM7WUFDVCxZQUFZO1NBQ2IsQ0FBQyxFQUNILENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELFVBQVUsQ0FBQyxFQUFFLFFBQVEsRUFBZ0MsRUFBRSxFQUFFLE9BQU8sRUFBZTtRQUM3RSxPQUFPLElBQUksQ0FBQyxlQUFlLENBQUMsVUFBVSxDQUFDLE9BQU8sQ0FBQyxDQUFDLElBQUksQ0FBQyxTQUFTOzs7UUFBQyxHQUFHLEVBQUUsQ0FBQyxRQUFRLENBQUMsSUFBSSxRQUFRLEVBQUUsQ0FBQyxFQUFDLENBQUMsQ0FBQztJQUNsRyxDQUFDOzs7Ozs7SUFHRCxPQUFPLENBQUMsRUFBRSxRQUFRLEVBQWdDLEVBQUUsRUFBRSxPQUFPLEVBQWM7UUFDekUsT0FBTyxJQUFJLENBQUMsZUFBZSxDQUFDLFVBQVUsQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQUMsU0FBUzs7O1FBQUMsR0FBRyxFQUFFLENBQUMsUUFBUSxDQUFDLElBQUksUUFBUSxFQUFFLENBQUMsRUFBQyxDQUFDLENBQUM7SUFDbEcsQ0FBQzs7Ozs7O0lBR0QsVUFBVSxDQUFDLEVBQUUsUUFBUSxFQUFFLFFBQVEsRUFBZ0MsRUFBRSxFQUFFLE9BQU8sRUFBYztRQUN0RixPQUFPLFFBQVEsQ0FBQyxJQUFJLFdBQVcsQ0FBQyxPQUFPLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQy9DLFNBQVM7OztRQUFDLEdBQUcsRUFBRSxDQUFDLElBQUksQ0FBQyxlQUFlLENBQUMsVUFBVSxtQkFBTSxRQUFRLEVBQUUsQ0FBQyxZQUFZLEVBQUssT0FBTyxFQUFHLEVBQUMsRUFDNUYsU0FBUzs7O1FBQUMsR0FBRyxFQUFFLENBQUMsUUFBUSxDQUFDLElBQUksUUFBUSxFQUFFLENBQUMsRUFBQyxDQUMxQyxDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0QsWUFBWSxDQUFDLEVBQUUsVUFBVSxFQUFnQyxFQUFFLEVBQUUsT0FBTyxFQUFnQjtRQUNsRixPQUFPLElBQUksQ0FBQyxlQUFlLENBQUMsWUFBWSxDQUFDLE9BQU8sQ0FBQyxDQUFDLElBQUksQ0FDcEQsS0FBSyxDQUFDLE9BQU8sQ0FBQyxFQUNkLEdBQUc7Ozs7UUFBQyxpQkFBaUIsQ0FBQyxFQUFFLENBQ3RCLFVBQVUsQ0FBQztZQUNULGlCQUFpQjtTQUNsQixDQUFDLEVBQ0gsQ0FDRixDQUFDO0lBQ0osQ0FBQztDQUNGLENBQUE7O1lBN0ZzQyxlQUFlOztBQUdwRDtJQURDLE1BQU0sQ0FBQyxRQUFRLENBQUM7O3FEQUNtRCxRQUFROzs2Q0FRM0U7QUFHRDtJQURDLE1BQU0sQ0FBQyxXQUFXLENBQUM7O3FEQUMrQyxXQUFXOzs0Q0FRN0U7QUFHRDtJQURDLE1BQU0sQ0FBQyxVQUFVLENBQUM7O3FEQUNpRCxXQUFXOzsrQ0FFOUU7QUFHRDtJQURDLE1BQU0sQ0FBQyxVQUFVLENBQUM7O3FEQUM4QyxVQUFVOzs0Q0FFMUU7QUFHRDtJQURDLE1BQU0sQ0FBQyxVQUFVLENBQUM7O3FEQUMyRCxVQUFVOzsrQ0FLdkY7QUFHRDtJQURDLE1BQU0sQ0FBQyxRQUFRLENBQUM7O3FEQUNtRCxRQUFROzs2Q0FRM0U7QUFHRDtJQURDLE1BQU0sQ0FBQyxXQUFXLENBQUM7O3FEQUMrQyxXQUFXOzs0Q0FRN0U7QUFHRDtJQURDLE1BQU0sQ0FBQyxVQUFVLENBQUM7O3FEQUNpRCxXQUFXOzsrQ0FFOUU7QUFHRDtJQURDLE1BQU0sQ0FBQyxVQUFVLENBQUM7O3FEQUM4QyxVQUFVOzs0Q0FFMUU7QUFHRDtJQURDLE1BQU0sQ0FBQyxVQUFVLENBQUM7O3FEQUMyRCxVQUFVOzsrQ0FLdkY7QUFHRDtJQURDLE1BQU0sQ0FBQyxZQUFZLENBQUM7O3FEQUNtRCxZQUFZOztpREFTbkY7QUEvR0Q7SUFEQyxRQUFRLEVBQUU7Ozs7bUNBR1Y7QUFHRDtJQURDLFFBQVEsRUFBRTs7Ozs2Q0FHVjtBQUdEO0lBREMsUUFBUSxFQUFFOzs7O21DQUdWO0FBR0Q7SUFEQyxRQUFRLEVBQUU7Ozs7NkNBR1Y7QUFuQlUsYUFBYTtJQUp6QixLQUFLLENBQWlCO1FBQ3JCLElBQUksRUFBRSxlQUFlO1FBQ3JCLFFBQVEsRUFBRSxtQkFBQSxFQUFFLEtBQUssRUFBRSxFQUFFLEVBQUUsWUFBWSxFQUFFLEVBQUUsRUFBRSxLQUFLLEVBQUUsRUFBRSxFQUFFLFlBQVksRUFBRSxFQUFFLEVBQUUsRUFBa0I7S0FDekYsQ0FBQzs2Q0FzQnFDLGVBQWU7R0FyQnpDLGFBQWEsQ0FrSHpCO1NBbEhZLGFBQWE7Ozs7OztJQXFCWix3Q0FBd0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBY3Rpb24sIFNlbGVjdG9yLCBTdGF0ZSwgU3RhdGVDb250ZXh0IH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBzd2l0Y2hNYXAsIHRhcCwgcGx1Y2sgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XHJcbmltcG9ydCB7XHJcbiAgQ3JlYXRlUm9sZSxcclxuICBDcmVhdGVVc2VyLFxyXG4gIERlbGV0ZVJvbGUsXHJcbiAgRGVsZXRlVXNlcixcclxuICBHZXRSb2xlQnlJZCxcclxuICBHZXRSb2xlcyxcclxuICBHZXRVc2VyQnlJZCxcclxuICBHZXRVc2VycyxcclxuICBVcGRhdGVSb2xlLFxyXG4gIFVwZGF0ZVVzZXIsXHJcbiAgR2V0VXNlclJvbGVzLFxyXG59IGZyb20gJy4uL2FjdGlvbnMvaWRlbnRpdHkuYWN0aW9ucyc7XHJcbmltcG9ydCB7IElkZW50aXR5IH0gZnJvbSAnLi4vbW9kZWxzL2lkZW50aXR5JztcclxuaW1wb3J0IHsgSWRlbnRpdHlTZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvaWRlbnRpdHkuc2VydmljZSc7XHJcblxyXG5AU3RhdGU8SWRlbnRpdHkuU3RhdGU+KHtcclxuICBuYW1lOiAnSWRlbnRpdHlTdGF0ZScsXHJcbiAgZGVmYXVsdHM6IHsgcm9sZXM6IHt9LCBzZWxlY3RlZFJvbGU6IHt9LCB1c2Vyczoge30sIHNlbGVjdGVkVXNlcjoge30gfSBhcyBJZGVudGl0eS5TdGF0ZSxcclxufSlcclxuZXhwb3J0IGNsYXNzIElkZW50aXR5U3RhdGUge1xyXG4gIEBTZWxlY3RvcigpXHJcbiAgc3RhdGljIGdldFJvbGVzKHsgcm9sZXMgfTogSWRlbnRpdHkuU3RhdGUpOiBJZGVudGl0eS5Sb2xlSXRlbVtdIHtcclxuICAgIHJldHVybiByb2xlcy5pdGVtcyB8fCBbXTtcclxuICB9XHJcblxyXG4gIEBTZWxlY3RvcigpXHJcbiAgc3RhdGljIGdldFJvbGVzVG90YWxDb3VudCh7IHJvbGVzIH06IElkZW50aXR5LlN0YXRlKTogbnVtYmVyIHtcclxuICAgIHJldHVybiByb2xlcy50b3RhbENvdW50IHx8IDA7XHJcbiAgfVxyXG5cclxuICBAU2VsZWN0b3IoKVxyXG4gIHN0YXRpYyBnZXRVc2Vycyh7IHVzZXJzIH06IElkZW50aXR5LlN0YXRlKTogSWRlbnRpdHkuVXNlckl0ZW1bXSB7XHJcbiAgICByZXR1cm4gdXNlcnMuaXRlbXMgfHwgW107XHJcbiAgfVxyXG5cclxuICBAU2VsZWN0b3IoKVxyXG4gIHN0YXRpYyBnZXRVc2Vyc1RvdGFsQ291bnQoeyB1c2VycyB9OiBJZGVudGl0eS5TdGF0ZSk6IG51bWJlciB7XHJcbiAgICByZXR1cm4gdXNlcnMudG90YWxDb3VudCB8fCAwO1xyXG4gIH1cclxuXHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBpZGVudGl0eVNlcnZpY2U6IElkZW50aXR5U2VydmljZSkge31cclxuXHJcbiAgQEFjdGlvbihHZXRSb2xlcylcclxuICBnZXRSb2xlcyh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFJvbGVzKSB7XHJcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZ2V0Um9sZXMocGF5bG9hZCkucGlwZShcclxuICAgICAgdGFwKHJvbGVzID0+XHJcbiAgICAgICAgcGF0Y2hTdGF0ZSh7XHJcbiAgICAgICAgICByb2xlcyxcclxuICAgICAgICB9KSxcclxuICAgICAgKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKEdldFJvbGVCeUlkKVxyXG4gIGdldFJvbGUoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRSb2xlQnlJZCkge1xyXG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmdldFJvbGVCeUlkKHBheWxvYWQpLnBpcGUoXHJcbiAgICAgIHRhcChzZWxlY3RlZFJvbGUgPT5cclxuICAgICAgICBwYXRjaFN0YXRlKHtcclxuICAgICAgICAgIHNlbGVjdGVkUm9sZSxcclxuICAgICAgICB9KSxcclxuICAgICAgKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKERlbGV0ZVJvbGUpXHJcbiAgZGVsZXRlUm9sZSh7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRSb2xlQnlJZCkge1xyXG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmRlbGV0ZVJvbGUocGF5bG9hZCkucGlwZShzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IEdldFJvbGVzKCkpKSk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKENyZWF0ZVJvbGUpXHJcbiAgYWRkUm9sZSh7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBDcmVhdGVSb2xlKSB7XHJcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuY3JlYXRlUm9sZShwYXlsb2FkKS5waXBlKHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgR2V0Um9sZXMoKSkpKTtcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oVXBkYXRlUm9sZSlcclxuICB1cGRhdGVSb2xlKHsgZ2V0U3RhdGUsIGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBVcGRhdGVSb2xlKSB7XHJcbiAgICByZXR1cm4gZGlzcGF0Y2gobmV3IEdldFJvbGVCeUlkKHBheWxvYWQuaWQpKS5waXBlKFxyXG4gICAgICBzd2l0Y2hNYXAoKCkgPT4gdGhpcy5pZGVudGl0eVNlcnZpY2UudXBkYXRlUm9sZSh7IC4uLmdldFN0YXRlKCkuc2VsZWN0ZWRSb2xlLCAuLi5wYXlsb2FkIH0pKSxcclxuICAgICAgc3dpdGNoTWFwKCgpID0+IGRpc3BhdGNoKG5ldyBHZXRSb2xlcygpKSksXHJcbiAgICApO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihHZXRVc2VycylcclxuICBnZXRVc2Vycyh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFVzZXJzKSB7XHJcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZ2V0VXNlcnMocGF5bG9hZCkucGlwZShcclxuICAgICAgdGFwKHVzZXJzID0+XHJcbiAgICAgICAgcGF0Y2hTdGF0ZSh7XHJcbiAgICAgICAgICB1c2VycyxcclxuICAgICAgICB9KSxcclxuICAgICAgKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKEdldFVzZXJCeUlkKVxyXG4gIGdldFVzZXIoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRVc2VyQnlJZCkge1xyXG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmdldFVzZXJCeUlkKHBheWxvYWQpLnBpcGUoXHJcbiAgICAgIHRhcChzZWxlY3RlZFVzZXIgPT5cclxuICAgICAgICBwYXRjaFN0YXRlKHtcclxuICAgICAgICAgIHNlbGVjdGVkVXNlcixcclxuICAgICAgICB9KSxcclxuICAgICAgKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKERlbGV0ZVVzZXIpXHJcbiAgZGVsZXRlVXNlcih7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRVc2VyQnlJZCkge1xyXG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmRlbGV0ZVVzZXIocGF5bG9hZCkucGlwZShzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IEdldFVzZXJzKCkpKSk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKENyZWF0ZVVzZXIpXHJcbiAgYWRkVXNlcih7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBDcmVhdGVVc2VyKSB7XHJcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuY3JlYXRlVXNlcihwYXlsb2FkKS5waXBlKHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgR2V0VXNlcnMoKSkpKTtcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oVXBkYXRlVXNlcilcclxuICB1cGRhdGVVc2VyKHsgZ2V0U3RhdGUsIGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBVcGRhdGVVc2VyKSB7XHJcbiAgICByZXR1cm4gZGlzcGF0Y2gobmV3IEdldFVzZXJCeUlkKHBheWxvYWQuaWQpKS5waXBlKFxyXG4gICAgICBzd2l0Y2hNYXAoKCkgPT4gdGhpcy5pZGVudGl0eVNlcnZpY2UudXBkYXRlVXNlcih7IC4uLmdldFN0YXRlKCkuc2VsZWN0ZWRVc2VyLCAuLi5wYXlsb2FkIH0pKSxcclxuICAgICAgc3dpdGNoTWFwKCgpID0+IGRpc3BhdGNoKG5ldyBHZXRVc2VycygpKSksXHJcbiAgICApO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihHZXRVc2VyUm9sZXMpXHJcbiAgZ2V0VXNlclJvbGVzKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0VXNlclJvbGVzKSB7XHJcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZ2V0VXNlclJvbGVzKHBheWxvYWQpLnBpcGUoXHJcbiAgICAgIHBsdWNrKCdpdGVtcycpLFxyXG4gICAgICB0YXAoc2VsZWN0ZWRVc2VyUm9sZXMgPT5cclxuICAgICAgICBwYXRjaFN0YXRlKHtcclxuICAgICAgICAgIHNlbGVjdGVkVXNlclJvbGVzLFxyXG4gICAgICAgIH0pLFxyXG4gICAgICApLFxyXG4gICAgKTtcclxuICB9XHJcbn1cclxuIl19