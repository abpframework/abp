/**
 * @fileoverview added by tsickle
 * Generated from: lib/states/identity.state.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Action, Selector, State } from '@ngxs/store';
import { tap, pluck } from 'rxjs/operators';
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
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    deleteRole(_, { payload }) {
        return this.identityService.deleteRole(payload);
    }
    /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    addRole(_, { payload }) {
        return this.identityService.createRole(payload);
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    updateRole({ getState }, { payload }) {
        return this.identityService.updateRole(Object.assign({}, getState().selectedRole, payload));
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
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    deleteUser(_, { payload }) {
        return this.identityService.deleteUser(payload);
    }
    /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    addUser(_, { payload }) {
        return this.identityService.createUser(payload);
    }
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    updateUser({ getState }, { payload }) {
        return this.identityService.updateUser(Object.assign({}, getState().selectedUser, payload));
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkuc3RhdGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy9pZGVudGl0eS5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLFFBQVEsRUFBRSxLQUFLLEVBQWdCLE1BQU0sYUFBYSxDQUFDO0FBQ3BFLE9BQU8sRUFBYSxHQUFHLEVBQUUsS0FBSyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDdkQsT0FBTyxFQUNMLFVBQVUsRUFDVixVQUFVLEVBQ1YsVUFBVSxFQUNWLFVBQVUsRUFDVixXQUFXLEVBQ1gsUUFBUSxFQUNSLFdBQVcsRUFDWCxRQUFRLEVBQ1IsVUFBVSxFQUNWLFVBQVUsRUFDVixZQUFZLEdBQ2IsTUFBTSw2QkFBNkIsQ0FBQztBQUVyQyxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sOEJBQThCLENBQUM7SUFNbEQsYUFBYSxTQUFiLGFBQWE7Ozs7SUFxQnhCLFlBQW9CLGVBQWdDO1FBQWhDLG9CQUFlLEdBQWYsZUFBZSxDQUFpQjtJQUFHLENBQUM7Ozs7O0lBbkJ4RCxNQUFNLENBQUMsUUFBUSxDQUFDLEVBQUUsS0FBSyxFQUFrQjtRQUN2QyxPQUFPLEtBQUssQ0FBQyxLQUFLLElBQUksRUFBRSxDQUFDO0lBQzNCLENBQUM7Ozs7O0lBR0QsTUFBTSxDQUFDLGtCQUFrQixDQUFDLEVBQUUsS0FBSyxFQUFrQjtRQUNqRCxPQUFPLEtBQUssQ0FBQyxVQUFVLElBQUksQ0FBQyxDQUFDO0lBQy9CLENBQUM7Ozs7O0lBR0QsTUFBTSxDQUFDLFFBQVEsQ0FBQyxFQUFFLEtBQUssRUFBa0I7UUFDdkMsT0FBTyxLQUFLLENBQUMsS0FBSyxJQUFJLEVBQUUsQ0FBQztJQUMzQixDQUFDOzs7OztJQUdELE1BQU0sQ0FBQyxrQkFBa0IsQ0FBQyxFQUFFLEtBQUssRUFBa0I7UUFDakQsT0FBTyxLQUFLLENBQUMsVUFBVSxJ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RUFBRSxFQUFFLEVBQUUsRUFBa0I7S0FDekYsQ0FBQzs2Q0FzQnFDLGVBQWU7R0FyQnpDLGFBQWEsQ0E0R3pCO1NBNUdZLGFBQWE7Ozs7OztJQXFCWix3Q0FBd0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBY3Rpb24sIFNlbGVjdG9yLCBTdGF0ZSwgU3RhdGVDb250ZXh0IH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgc3dpdGNoTWFwLCB0YXAsIHBsdWNrIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHtcbiAgQ3JlYXRlUm9sZSxcbiAgQ3JlYXRlVXNlcixcbiAgRGVsZXRlUm9sZSxcbiAgRGVsZXRlVXNlcixcbiAgR2V0Um9sZUJ5SWQsXG4gIEdldFJvbGVzLFxuICBHZXRVc2VyQnlJZCxcbiAgR2V0VXNlcnMsXG4gIFVwZGF0ZVJvbGUsXG4gIFVwZGF0ZVVzZXIsXG4gIEdldFVzZXJSb2xlcyxcbn0gZnJvbSAnLi4vYWN0aW9ucy9pZGVudGl0eS5hY3Rpb25zJztcbmltcG9ydCB7IElkZW50aXR5IH0gZnJvbSAnLi4vbW9kZWxzL2lkZW50aXR5JztcbmltcG9ydCB7IElkZW50aXR5U2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL2lkZW50aXR5LnNlcnZpY2UnO1xuXG5AU3RhdGU8SWRlbnRpdHkuU3RhdGU+KHtcbiAgbmFtZTogJ0lkZW50aXR5U3RhdGUnLFxuICBkZWZhdWx0czogeyByb2xlczoge30sIHNlbGVjdGVkUm9sZToge30sIHVzZXJzOiB7fSwgc2VsZWN0ZWRVc2VyOiB7fSB9IGFzIElkZW50aXR5LlN0YXRlLFxufSlcbmV4cG9ydCBjbGFzcyBJZGVudGl0eVN0YXRlIHtcbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldFJvbGVzKHsgcm9sZXMgfTogSWRlbnRpdHkuU3RhdGUpOiBJZGVudGl0eS5Sb2xlSXRlbVtdIHtcbiAgICByZXR1cm4gcm9sZXMuaXRlbXMgfHwgW107XG4gIH1cblxuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0Um9sZXNUb3RhbENvdW50KHsgcm9sZXMgfTogSWRlbnRpdHkuU3RhdGUpOiBudW1iZXIge1xuICAgIHJldHVybiByb2xlcy50b3RhbENvdW50IHx8IDA7XG4gIH1cblxuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0VXNlcnMoeyB1c2VycyB9OiBJZGVudGl0eS5TdGF0ZSk6IElkZW50aXR5LlVzZXJJdGVtW10ge1xuICAgIHJldHVybiB1c2Vycy5pdGVtcyB8fCBbXTtcbiAgfVxuXG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRVc2Vyc1RvdGFsQ291bnQoeyB1c2VycyB9OiBJZGVudGl0eS5TdGF0ZSk6IG51bWJlciB7XG4gICAgcmV0dXJuIHVzZXJzLnRvdGFsQ291bnQgfHwgMDtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgaWRlbnRpdHlTZXJ2aWNlOiBJZGVudGl0eVNlcnZpY2UpIHt9XG5cbiAgQEFjdGlvbihHZXRSb2xlcylcbiAgZ2V0Um9sZXMoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRSb2xlcykge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5nZXRSb2xlcyhwYXlsb2FkKS5waXBlKFxuICAgICAgdGFwKHJvbGVzID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHJvbGVzLFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIEBBY3Rpb24oR2V0Um9sZUJ5SWQpXG4gIGdldFJvbGUoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRSb2xlQnlJZCkge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5nZXRSb2xlQnlJZChwYXlsb2FkKS5waXBlKFxuICAgICAgdGFwKHNlbGVjdGVkUm9sZSA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICBzZWxlY3RlZFJvbGUsXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihEZWxldGVSb2xlKVxuICBkZWxldGVSb2xlKF8sIHsgcGF5bG9hZCB9OiBHZXRSb2xlQnlJZCkge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5kZWxldGVSb2xlKHBheWxvYWQpO1xuICB9XG5cbiAgQEFjdGlvbihDcmVhdGVSb2xlKVxuICBhZGRSb2xlKF8sIHsgcGF5bG9hZCB9OiBDcmVhdGVSb2xlKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmNyZWF0ZVJvbGUocGF5bG9hZCk7XG4gIH1cblxuICBAQWN0aW9uKFVwZGF0ZVJvbGUpXG4gIHVwZGF0ZVJvbGUoeyBnZXRTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogVXBkYXRlUm9sZSkge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS51cGRhdGVSb2xlKHsgLi4uZ2V0U3RhdGUoKS5zZWxlY3RlZFJvbGUsIC4uLnBheWxvYWQgfSk7XG4gIH1cblxuICBAQWN0aW9uKEdldFVzZXJzKVxuICBnZXRVc2Vycyh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFVzZXJzKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmdldFVzZXJzKHBheWxvYWQpLnBpcGUoXG4gICAgICB0YXAodXNlcnMgPT5cbiAgICAgICAgcGF0Y2hTdGF0ZSh7XG4gICAgICAgICAgdXNlcnMsXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihHZXRVc2VyQnlJZClcbiAgZ2V0VXNlcih7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFVzZXJCeUlkKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmdldFVzZXJCeUlkKHBheWxvYWQpLnBpcGUoXG4gICAgICB0YXAoc2VsZWN0ZWRVc2VyID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHNlbGVjdGVkVXNlcixcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKERlbGV0ZVVzZXIpXG4gIGRlbGV0ZVVzZXIoXywgeyBwYXlsb2FkIH06IEdldFVzZXJCeUlkKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmRlbGV0ZVVzZXIocGF5bG9hZCk7XG4gIH1cblxuICBAQWN0aW9uKENyZWF0ZVVzZXIpXG4gIGFkZFVzZXIoXywgeyBwYXlsb2FkIH06IENyZWF0ZVVzZXIpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuY3JlYXRlVXNlcihwYXlsb2FkKTtcbiAgfVxuXG4gIEBBY3Rpb24oVXBkYXRlVXNlcilcbiAgdXBkYXRlVXNlcih7IGdldFN0YXRlIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBVcGRhdGVVc2VyKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLnVwZGF0ZVVzZXIoeyAuLi5nZXRTdGF0ZSgpLnNlbGVjdGVkVXNlciwgLi4ucGF5bG9hZCB9KTtcbiAgfVxuXG4gIEBBY3Rpb24oR2V0VXNlclJvbGVzKVxuICBnZXRVc2VyUm9sZXMoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRVc2VyUm9sZXMpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZ2V0VXNlclJvbGVzKHBheWxvYWQpLnBpcGUoXG4gICAgICBwbHVjaygnaXRlbXMnKSxcbiAgICAgIHRhcChzZWxlY3RlZFVzZXJSb2xlcyA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICBzZWxlY3RlZFVzZXJSb2xlcyxcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cbn1cbiJdfQ==