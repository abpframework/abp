/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Action, Selector, State } from '@ngxs/store';
import { switchMap, tap, pluck } from 'rxjs/operators';
import { IdentityAddRole, IdentityAddUser, IdentityDeleteRole, IdentityDeleteUser, IdentityGetRoleById, IdentityGetRoles, IdentityGetUserById, IdentityGetUsers, IdentityUpdateRole, IdentityUpdateUser, IdentityGetUserRoles, } from '../actions/identity.actions';
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
        return roles.items;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getRolesTotalCount({ roles }) {
        return roles.totalCount;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getUsers({ users }) {
        return users.items;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    static getUsersTotalCount({ users }) {
        return users.totalCount;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    getRoles({ patchState }) {
        return this.identityService.getRoles().pipe(tap((/**
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
        () => dispatch(new IdentityGetRoles()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    addRole({ dispatch }, { payload }) {
        return this.identityService.addRole(payload).pipe(switchMap((/**
         * @return {?}
         */
        () => dispatch(new IdentityGetRoles()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    updateRole({ getState, dispatch }, { payload }) {
        return dispatch(new IdentityGetRoleById(payload.id)).pipe(switchMap((/**
         * @return {?}
         */
        () => this.identityService.updateRole(Object.assign({}, getState().selectedRole, payload)))), switchMap((/**
         * @return {?}
         */
        () => dispatch(new IdentityGetRoles()))));
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
        () => dispatch(new IdentityGetUsers()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    addUser({ dispatch }, { payload }) {
        return this.identityService.addUser(payload).pipe(switchMap((/**
         * @return {?}
         */
        () => dispatch(new IdentityGetUsers()))));
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    updateUser({ getState, dispatch }, { payload }) {
        return dispatch(new IdentityGetUserById(payload.id)).pipe(switchMap((/**
         * @return {?}
         */
        () => this.identityService.updateUser(Object.assign({}, getState().selectedUser, payload)))), switchMap((/**
         * @return {?}
         */
        () => dispatch(new IdentityGetUsers()))));
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
tslib_1.__decorate([
    Action(IdentityGetRoles),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "getRoles", null);
tslib_1.__decorate([
    Action(IdentityGetRoleById),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, IdentityGetRoleById]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "getRole", null);
tslib_1.__decorate([
    Action(IdentityDeleteRole),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, IdentityGetRoleById]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "deleteRole", null);
tslib_1.__decorate([
    Action(IdentityAddRole),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, IdentityAddRole]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "addRole", null);
tslib_1.__decorate([
    Action(IdentityUpdateRole),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, IdentityUpdateRole]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "updateRole", null);
tslib_1.__decorate([
    Action(IdentityGetUsers),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, IdentityGetUsers]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "getUsers", null);
tslib_1.__decorate([
    Action(IdentityGetUserById),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, IdentityGetUserById]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "getUser", null);
tslib_1.__decorate([
    Action(IdentityDeleteUser),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, IdentityGetUserById]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "deleteUser", null);
tslib_1.__decorate([
    Action(IdentityAddUser),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, IdentityAddUser]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "addUser", null);
tslib_1.__decorate([
    Action(IdentityUpdateUser),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, IdentityUpdateUser]),
    tslib_1.__metadata("design:returntype", void 0)
], IdentityState.prototype, "updateUser", null);
tslib_1.__decorate([
    Action(IdentityGetUserRoles),
    tslib_1.__metadata("design:type", Function),
    tslib_1.__metadata("design:paramtypes", [Object, IdentityGetUserRoles]),
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkuc3RhdGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy9pZGVudGl0eS5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLFNBQVMsRUFBRSxHQUFHLEVBQUUsS0FBSyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDdkQsT0FBTyxFQUNMLGVBQWUsRUFDZixlQUFlLEVBQ2Ysa0JBQWtCLEVBQ2xCLGtCQUFrQixFQUNsQixtQkFBbUIsRUFDbkIsZ0JBQWdCLEVBQ2hCLG1CQUFtQixFQUNuQixnQkFBZ0IsRUFDaEIsa0JBQWtCLEVBQ2xCLGtCQUFrQixFQUNsQixvQkFBb0IsR0FDckIsTUFBTSw2QkFBNkIsQ0FBQztBQUVyQyxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sOEJBQThCLENBQUM7SUFNbEQsYUFBYSxTQUFiLGFBQWE7Ozs7SUFxQnhCLFlBQW9CLGVBQWdDO1FBQWhDLG9CQUFlLEdBQWYsZUFBZSxDQUFpQjtJQUFHLENBQUM7Ozs7O0lBbkJ4RCxNQUFNLENBQUMsUUFBUSxDQUFDLEVBQUUsS0FBSyxFQUFrQjtRQUN2QyxPQUFPLEtBQUssQ0FBQyxLQUFLLENBQUM7SUFDckIsQ0FBQzs7Ozs7SUFHRCxNQUFNLENBQUMsa0JBQWtCLENBQUMsRUFBRSxLQUFLLEVBQWtCO1FBQ2pELE9BQU8sS0FBSyxDQUFDLFVBQVUsQ0FBQztJQUMxQixDQUFDOzs7OztJQUdELE1BQU0sQ0FBQyxRQUFRLENBQUMsRUFBRSxLQUFLLEVBQWtCO1FBQ3ZDLE9BQU8sS0FBSyxDQUFDLEtBQUssQ0FBQztJQUNyQixDQUFDOzs7OztJQUdELE1BQU0sQ0FBQyxrQkFBa0IsQ0FBQyxFQUFFLEtBQUssRUFBa0I7UUFDakQsT0FBTyxLQUFLLENBQUMs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QyxNQUFNLENBQUMsa0JBQWtCLENBQUM7O3FEQUN5QyxtQkFBbUI7OytDQUV0RjtBQUdEO0lBREMsTUFBTSxDQUFDLGVBQWUsQ0FBQzs7cURBQ3lDLGVBQWU7OzRDQUUvRTtBQUdEO0lBREMsTUFBTSxDQUFDLGtCQUFrQixDQUFDOztxREFDbUQsa0JBQWtCOzsrQ0FLL0Y7QUFHRDtJQURDLE1BQU0sQ0FBQyxnQkFBZ0IsQ0FBQzs7cURBQzJDLGdCQUFnQjs7NkNBUW5GO0FBR0Q7SUFEQyxNQUFNLENBQUMsbUJBQW1CLENBQUM7O3FEQUN1QyxtQkFBbUI7OzRDQVFyRjtBQUdEO0lBREMsTUFBTSxDQUFDLGtCQUFrQixDQUFDOztxREFDeUMsbUJBQW1COzsrQ0FFdEY7QUFHRDtJQURDLE1BQU0sQ0FBQyxlQUFlLENBQUM7O3FEQUN5QyxlQUFlOzs0Q0FFL0U7QUFHRDtJQURDLE1BQU0sQ0FBQyxrQkFBa0IsQ0FBQzs7cURBQ21ELGtCQUFrQjs7K0NBSy9GO0FBR0Q7SUFEQyxNQUFNLENBQUMsb0JBQW9CLENBQUM7O3FEQUMyQyxvQkFBb0I7O2lEQVMzRjtBQS9HRDtJQURDLFFBQVEsRUFBRTs7OzttQ0FHVjtBQUdEO0lBREMsUUFBUSxFQUFFOzs7OzZDQUdWO0FBR0Q7SUFEQyxRQUFRLEVBQUU7Ozs7bUNBR1Y7QUFHRDtJQURDLFFBQVEsRUFBRTs7Ozs2Q0FHVjtBQW5CVSxhQUFhO0lBSnpCLEtBQUssQ0FBaUI7UUFDckIsSUFBSSxFQUFFLGVBQWU7UUFDckIsUUFBUSxFQUFFLG1CQUFBLEVBQUUsS0FBSyxFQUFFLEVBQUUsRUFBRSxZQUFZLEVBQUUsRUFBRSxFQUFFLEtBQUssRUFBRSxFQUFFLEVBQUUsWUFBWSxFQUFFLEVBQUUsRUFBRSxFQUFrQjtLQUN6RixDQUFDOzZDQXNCcUMsZUFBZTtHQXJCekMsYUFBYSxDQWtIekI7U0FsSFksYUFBYTs7Ozs7O0lBcUJaLHdDQUF3QyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFjdGlvbiwgU2VsZWN0b3IsIFN0YXRlLCBTdGF0ZUNvbnRleHQgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBzd2l0Y2hNYXAsIHRhcCwgcGx1Y2sgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQge1xuICBJZGVudGl0eUFkZFJvbGUsXG4gIElkZW50aXR5QWRkVXNlcixcbiAgSWRlbnRpdHlEZWxldGVSb2xlLFxuICBJZGVudGl0eURlbGV0ZVVzZXIsXG4gIElkZW50aXR5R2V0Um9sZUJ5SWQsXG4gIElkZW50aXR5R2V0Um9sZXMsXG4gIElkZW50aXR5R2V0VXNlckJ5SWQsXG4gIElkZW50aXR5R2V0VXNlcnMsXG4gIElkZW50aXR5VXBkYXRlUm9sZSxcbiAgSWRlbnRpdHlVcGRhdGVVc2VyLFxuICBJZGVudGl0eUdldFVzZXJSb2xlcyxcbn0gZnJvbSAnLi4vYWN0aW9ucy9pZGVudGl0eS5hY3Rpb25zJztcbmltcG9ydCB7IElkZW50aXR5IH0gZnJvbSAnLi4vbW9kZWxzL2lkZW50aXR5JztcbmltcG9ydCB7IElkZW50aXR5U2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL2lkZW50aXR5LnNlcnZpY2UnO1xuXG5AU3RhdGU8SWRlbnRpdHkuU3RhdGU+KHtcbiAgbmFtZTogJ0lkZW50aXR5U3RhdGUnLFxuICBkZWZhdWx0czogeyByb2xlczoge30sIHNlbGVjdGVkUm9sZToge30sIHVzZXJzOiB7fSwgc2VsZWN0ZWRVc2VyOiB7fSB9IGFzIElkZW50aXR5LlN0YXRlLFxufSlcbmV4cG9ydCBjbGFzcyBJZGVudGl0eVN0YXRlIHtcbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldFJvbGVzKHsgcm9sZXMgfTogSWRlbnRpdHkuU3RhdGUpOiBJZGVudGl0eS5Sb2xlSXRlbVtdIHtcbiAgICByZXR1cm4gcm9sZXMuaXRlbXM7XG4gIH1cblxuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0Um9sZXNUb3RhbENvdW50KHsgcm9sZXMgfTogSWRlbnRpdHkuU3RhdGUpOiBudW1iZXIge1xuICAgIHJldHVybiByb2xlcy50b3RhbENvdW50O1xuICB9XG5cbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldFVzZXJzKHsgdXNlcnMgfTogSWRlbnRpdHkuU3RhdGUpOiBJZGVudGl0eS5Vc2VySXRlbVtdIHtcbiAgICByZXR1cm4gdXNlcnMuaXRlbXM7XG4gIH1cblxuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0VXNlcnNUb3RhbENvdW50KHsgdXNlcnMgfTogSWRlbnRpdHkuU3RhdGUpOiBudW1iZXIge1xuICAgIHJldHVybiB1c2Vycy50b3RhbENvdW50O1xuICB9XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBpZGVudGl0eVNlcnZpY2U6IElkZW50aXR5U2VydmljZSkge31cblxuICBAQWN0aW9uKElkZW50aXR5R2V0Um9sZXMpXG4gIGdldFJvbGVzKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+KSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmdldFJvbGVzKCkucGlwZShcbiAgICAgIHRhcChyb2xlcyA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICByb2xlcyxcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKElkZW50aXR5R2V0Um9sZUJ5SWQpXG4gIGdldFJvbGUoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBJZGVudGl0eUdldFJvbGVCeUlkKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmdldFJvbGVCeUlkKHBheWxvYWQpLnBpcGUoXG4gICAgICB0YXAoc2VsZWN0ZWRSb2xlID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHNlbGVjdGVkUm9sZSxcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKElkZW50aXR5RGVsZXRlUm9sZSlcbiAgZGVsZXRlUm9sZSh7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBJZGVudGl0eUdldFJvbGVCeUlkKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmRlbGV0ZVJvbGUocGF5bG9hZCkucGlwZShzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IElkZW50aXR5R2V0Um9sZXMoKSkpKTtcbiAgfVxuXG4gIEBBY3Rpb24oSWRlbnRpdHlBZGRSb2xlKVxuICBhZGRSb2xlKHsgZGlzcGF0Y2ggfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IElkZW50aXR5QWRkUm9sZSkge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5hZGRSb2xlKHBheWxvYWQpLnBpcGUoc3dpdGNoTWFwKCgpID0+IGRpc3BhdGNoKG5ldyBJZGVudGl0eUdldFJvbGVzKCkpKSk7XG4gIH1cblxuICBAQWN0aW9uKElkZW50aXR5VXBkYXRlUm9sZSlcbiAgdXBkYXRlUm9sZSh7IGdldFN0YXRlLCBkaXNwYXRjaCB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogSWRlbnRpdHlVcGRhdGVSb2xlKSB7XG4gICAgcmV0dXJuIGRpc3BhdGNoKG5ldyBJZGVudGl0eUdldFJvbGVCeUlkKHBheWxvYWQuaWQpKS5waXBlKFxuICAgICAgc3dpdGNoTWFwKCgpID0+IHRoaXMuaWRlbnRpdHlTZXJ2aWNlLnVwZGF0ZVJvbGUoeyAuLi5nZXRTdGF0ZSgpLnNlbGVjdGVkUm9sZSwgLi4ucGF5bG9hZCB9KSksXG4gICAgICBzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IElkZW50aXR5R2V0Um9sZXMoKSkpLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKElkZW50aXR5R2V0VXNlcnMpXG4gIGdldFVzZXJzKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogSWRlbnRpdHlHZXRVc2Vycykge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5nZXRVc2VycyhwYXlsb2FkKS5waXBlKFxuICAgICAgdGFwKHVzZXJzID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHVzZXJzLFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIEBBY3Rpb24oSWRlbnRpdHlHZXRVc2VyQnlJZClcbiAgZ2V0VXNlcih7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IElkZW50aXR5R2V0VXNlckJ5SWQpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZ2V0VXNlckJ5SWQocGF5bG9hZCkucGlwZShcbiAgICAgIHRhcChzZWxlY3RlZFVzZXIgPT5cbiAgICAgICAgcGF0Y2hTdGF0ZSh7XG4gICAgICAgICAgc2VsZWN0ZWRVc2VyLFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIEBBY3Rpb24oSWRlbnRpdHlEZWxldGVVc2VyKVxuICBkZWxldGVVc2VyKHsgZGlzcGF0Y2ggfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IElkZW50aXR5R2V0VXNlckJ5SWQpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZGVsZXRlVXNlcihwYXlsb2FkKS5waXBlKHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgSWRlbnRpdHlHZXRVc2VycygpKSkpO1xuICB9XG5cbiAgQEFjdGlvbihJZGVudGl0eUFkZFVzZXIpXG4gIGFkZFVzZXIoeyBkaXNwYXRjaCB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogSWRlbnRpdHlBZGRVc2VyKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmFkZFVzZXIocGF5bG9hZCkucGlwZShzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IElkZW50aXR5R2V0VXNlcnMoKSkpKTtcbiAgfVxuXG4gIEBBY3Rpb24oSWRlbnRpdHlVcGRhdGVVc2VyKVxuICB1cGRhdGVVc2VyKHsgZ2V0U3RhdGUsIGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBJZGVudGl0eVVwZGF0ZVVzZXIpIHtcbiAgICByZXR1cm4gZGlzcGF0Y2gobmV3IElkZW50aXR5R2V0VXNlckJ5SWQocGF5bG9hZC5pZCkpLnBpcGUoXG4gICAgICBzd2l0Y2hNYXAoKCkgPT4gdGhpcy5pZGVudGl0eVNlcnZpY2UudXBkYXRlVXNlcih7IC4uLmdldFN0YXRlKCkuc2VsZWN0ZWRVc2VyLCAuLi5wYXlsb2FkIH0pKSxcbiAgICAgIHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgSWRlbnRpdHlHZXRVc2VycygpKSksXG4gICAgKTtcbiAgfVxuXG4gIEBBY3Rpb24oSWRlbnRpdHlHZXRVc2VyUm9sZXMpXG4gIGdldFVzZXJSb2xlcyh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IElkZW50aXR5R2V0VXNlclJvbGVzKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmdldFVzZXJSb2xlcyhwYXlsb2FkKS5waXBlKFxuICAgICAgcGx1Y2soJ2l0ZW1zJyksXG4gICAgICB0YXAoc2VsZWN0ZWRVc2VyUm9sZXMgPT5cbiAgICAgICAgcGF0Y2hTdGF0ZSh7XG4gICAgICAgICAgc2VsZWN0ZWRVc2VyUm9sZXMsXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG59XG4iXX0=