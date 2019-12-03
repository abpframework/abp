/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkuc3RhdGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy9pZGVudGl0eS5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFhLEdBQUcsRUFBRSxLQUFLLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUN2RCxPQUFPLEVBQ0wsVUFBVSxFQUNWLFVBQVUsRUFDVixVQUFVLEVBQ1YsVUFBVSxFQUNWLFdBQVcsRUFDWCxRQUFRLEVBQ1IsV0FBVyxFQUNYLFFBQVEsRUFDUixVQUFVLEVBQ1YsVUFBVSxFQUNWLFlBQVksR0FDYixNQUFNLDZCQUE2QixDQUFDO0FBRXJDLE9BQU8sRUFBRSxlQUFlLEVBQUUsTUFBTSw4QkFBOEIsQ0FBQztJQU1sRCxhQUFhLFNBQWIsYUFBYTs7OztJQXFCeEIsWUFBb0IsZUFBZ0M7UUFBaEMsb0JBQWUsR0FBZixlQUFlLENBQWlCO0lBQUcsQ0FBQzs7Ozs7SUFuQnhELE1BQU0sQ0FBQyxRQUFRLENBQUMsRUFBRSxLQUFLLEVBQWtCO1FBQ3ZDLE9BQU8sS0FBSyxDQUFDLEtBQUssSUFBSSxFQUFFLENBQUM7SUFDM0IsQ0FBQzs7Ozs7SUFHRCxNQUFNLENBQUMsa0JBQWtCLENBQUMsRUFBRSxLQUFLLEVBQWtCO1FBQ2pELE9BQU8sS0FBSyxDQUFDLFVBQVUsSUFBSSxDQUFDLENBQUM7SUFDL0IsQ0FBQzs7Ozs7SUFHRCxNQUFNLENBQUMsUUFBUSxDQUFDLEVBQUUsS0FBSyxFQUFrQjtRQUN2QyxPQUFPLEtBQUssQ0FBQyxLQUFLLElBQUksRUFBRSxDQUFDO0lBQzNCLENBQUM7Ozs7O0lBR0QsTUFBTSxDQUFDLGtCQUFrQixDQUFDLEVBQUUsS0FBSyxFQUFrQjtRQUNqRCxPQUFPLEtBQUssQ0FBQyxVQUFVLElB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QUFFLEVBQUUsRUFBRSxFQUFrQjtLQUN6RixDQUFDOzZDQXNCcUMsZUFBZTtHQXJCekMsYUFBYSxDQTRHekI7U0E1R1ksYUFBYTs7Ozs7O0lBcUJaLHdDQUF3QyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFjdGlvbiwgU2VsZWN0b3IsIFN0YXRlLCBTdGF0ZUNvbnRleHQgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IHN3aXRjaE1hcCwgdGFwLCBwbHVjayB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcclxuaW1wb3J0IHtcclxuICBDcmVhdGVSb2xlLFxyXG4gIENyZWF0ZVVzZXIsXHJcbiAgRGVsZXRlUm9sZSxcclxuICBEZWxldGVVc2VyLFxyXG4gIEdldFJvbGVCeUlkLFxyXG4gIEdldFJvbGVzLFxyXG4gIEdldFVzZXJCeUlkLFxyXG4gIEdldFVzZXJzLFxyXG4gIFVwZGF0ZVJvbGUsXHJcbiAgVXBkYXRlVXNlcixcclxuICBHZXRVc2VyUm9sZXMsXHJcbn0gZnJvbSAnLi4vYWN0aW9ucy9pZGVudGl0eS5hY3Rpb25zJztcclxuaW1wb3J0IHsgSWRlbnRpdHkgfSBmcm9tICcuLi9tb2RlbHMvaWRlbnRpdHknO1xyXG5pbXBvcnQgeyBJZGVudGl0eVNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy9pZGVudGl0eS5zZXJ2aWNlJztcclxuXHJcbkBTdGF0ZTxJZGVudGl0eS5TdGF0ZT4oe1xyXG4gIG5hbWU6ICdJZGVudGl0eVN0YXRlJyxcclxuICBkZWZhdWx0czogeyByb2xlczoge30sIHNlbGVjdGVkUm9sZToge30sIHVzZXJzOiB7fSwgc2VsZWN0ZWRVc2VyOiB7fSB9IGFzIElkZW50aXR5LlN0YXRlLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgSWRlbnRpdHlTdGF0ZSB7XHJcbiAgQFNlbGVjdG9yKClcclxuICBzdGF0aWMgZ2V0Um9sZXMoeyByb2xlcyB9OiBJZGVudGl0eS5TdGF0ZSk6IElkZW50aXR5LlJvbGVJdGVtW10ge1xyXG4gICAgcmV0dXJuIHJvbGVzLml0ZW1zIHx8IFtdO1xyXG4gIH1cclxuXHJcbiAgQFNlbGVjdG9yKClcclxuICBzdGF0aWMgZ2V0Um9sZXNUb3RhbENvdW50KHsgcm9sZXMgfTogSWRlbnRpdHkuU3RhdGUpOiBudW1iZXIge1xyXG4gICAgcmV0dXJuIHJvbGVzLnRvdGFsQ291bnQgfHwgMDtcclxuICB9XHJcblxyXG4gIEBTZWxlY3RvcigpXHJcbiAgc3RhdGljIGdldFVzZXJzKHsgdXNlcnMgfTogSWRlbnRpdHkuU3RhdGUpOiBJZGVudGl0eS5Vc2VySXRlbVtdIHtcclxuICAgIHJldHVybiB1c2Vycy5pdGVtcyB8fCBbXTtcclxuICB9XHJcblxyXG4gIEBTZWxlY3RvcigpXHJcbiAgc3RhdGljIGdldFVzZXJzVG90YWxDb3VudCh7IHVzZXJzIH06IElkZW50aXR5LlN0YXRlKTogbnVtYmVyIHtcclxuICAgIHJldHVybiB1c2Vycy50b3RhbENvdW50IHx8IDA7XHJcbiAgfVxyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGlkZW50aXR5U2VydmljZTogSWRlbnRpdHlTZXJ2aWNlKSB7fVxyXG5cclxuICBAQWN0aW9uKEdldFJvbGVzKVxyXG4gIGdldFJvbGVzKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0Um9sZXMpIHtcclxuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5nZXRSb2xlcyhwYXlsb2FkKS5waXBlKFxyXG4gICAgICB0YXAocm9sZXMgPT5cclxuICAgICAgICBwYXRjaFN0YXRlKHtcclxuICAgICAgICAgIHJvbGVzLFxyXG4gICAgICAgIH0pLFxyXG4gICAgICApLFxyXG4gICAgKTtcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oR2V0Um9sZUJ5SWQpXHJcbiAgZ2V0Um9sZSh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFJvbGVCeUlkKSB7XHJcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZ2V0Um9sZUJ5SWQocGF5bG9hZCkucGlwZShcclxuICAgICAgdGFwKHNlbGVjdGVkUm9sZSA9PlxyXG4gICAgICAgIHBhdGNoU3RhdGUoe1xyXG4gICAgICAgICAgc2VsZWN0ZWRSb2xlLFxyXG4gICAgICAgIH0pLFxyXG4gICAgICApLFxyXG4gICAgKTtcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oRGVsZXRlUm9sZSlcclxuICBkZWxldGVSb2xlKF8sIHsgcGF5bG9hZCB9OiBHZXRSb2xlQnlJZCkge1xyXG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmRlbGV0ZVJvbGUocGF5bG9hZCk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKENyZWF0ZVJvbGUpXHJcbiAgYWRkUm9sZShfLCB7IHBheWxvYWQgfTogQ3JlYXRlUm9sZSkge1xyXG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmNyZWF0ZVJvbGUocGF5bG9hZCk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKFVwZGF0ZVJvbGUpXHJcbiAgdXBkYXRlUm9sZSh7IGdldFN0YXRlIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBVcGRhdGVSb2xlKSB7XHJcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UudXBkYXRlUm9sZSh7IC4uLmdldFN0YXRlKCkuc2VsZWN0ZWRSb2xlLCAuLi5wYXlsb2FkIH0pO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihHZXRVc2VycylcclxuICBnZXRVc2Vycyh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFVzZXJzKSB7XHJcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZ2V0VXNlcnMocGF5bG9hZCkucGlwZShcclxuICAgICAgdGFwKHVzZXJzID0+XHJcbiAgICAgICAgcGF0Y2hTdGF0ZSh7XHJcbiAgICAgICAgICB1c2VycyxcclxuICAgICAgICB9KSxcclxuICAgICAgKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKEdldFVzZXJCeUlkKVxyXG4gIGdldFVzZXIoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRVc2VyQnlJZCkge1xyXG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmdldFVzZXJCeUlkKHBheWxvYWQpLnBpcGUoXHJcbiAgICAgIHRhcChzZWxlY3RlZFVzZXIgPT5cclxuICAgICAgICBwYXRjaFN0YXRlKHtcclxuICAgICAgICAgIHNlbGVjdGVkVXNlcixcclxuICAgICAgICB9KSxcclxuICAgICAgKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKERlbGV0ZVVzZXIpXHJcbiAgZGVsZXRlVXNlcihfLCB7IHBheWxvYWQgfTogR2V0VXNlckJ5SWQpIHtcclxuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5kZWxldGVVc2VyKHBheWxvYWQpO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihDcmVhdGVVc2VyKVxyXG4gIGFkZFVzZXIoXywgeyBwYXlsb2FkIH06IENyZWF0ZVVzZXIpIHtcclxuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5jcmVhdGVVc2VyKHBheWxvYWQpO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihVcGRhdGVVc2VyKVxyXG4gIHVwZGF0ZVVzZXIoeyBnZXRTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogVXBkYXRlVXNlcikge1xyXG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLnVwZGF0ZVVzZXIoeyAuLi5nZXRTdGF0ZSgpLnNlbGVjdGVkVXNlciwgLi4ucGF5bG9hZCB9KTtcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oR2V0VXNlclJvbGVzKVxyXG4gIGdldFVzZXJSb2xlcyh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFVzZXJSb2xlcykge1xyXG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmdldFVzZXJSb2xlcyhwYXlsb2FkKS5waXBlKFxyXG4gICAgICBwbHVjaygnaXRlbXMnKSxcclxuICAgICAgdGFwKHNlbGVjdGVkVXNlclJvbGVzID0+XHJcbiAgICAgICAgcGF0Y2hTdGF0ZSh7XHJcbiAgICAgICAgICBzZWxlY3RlZFVzZXJSb2xlcyxcclxuICAgICAgICB9KSxcclxuICAgICAgKSxcclxuICAgICk7XHJcbiAgfVxyXG59XHJcbiJdfQ==