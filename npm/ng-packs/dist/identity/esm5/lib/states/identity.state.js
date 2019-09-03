/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Action, Selector, State } from '@ngxs/store';
import { switchMap, tap, pluck } from 'rxjs/operators';
import { CreateRole, CreateUser, DeleteRole, DeleteUser, GetRoleById, GetRoles, GetUserById, GetUsers, UpdateRole, UpdateUser, GetUserRoles, } from '../actions/identity.actions';
import { IdentityService } from '../services/identity.service';
var IdentityState = /** @class */ (function () {
    function IdentityState(identityService) {
        this.identityService = identityService;
    }
    /**
     * @param {?} __0
     * @return {?}
     */
    IdentityState.getRoles = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var roles = _a.roles;
        return roles.items;
    };
    /**
     * @param {?} __0
     * @return {?}
     */
    IdentityState.getRolesTotalCount = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var roles = _a.roles;
        return roles.totalCount;
    };
    /**
     * @param {?} __0
     * @return {?}
     */
    IdentityState.getUsers = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var users = _a.users;
        return users.items;
    };
    /**
     * @param {?} __0
     * @return {?}
     */
    IdentityState.getUsersTotalCount = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var users = _a.users;
        return users.totalCount;
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    IdentityState.prototype.getRoles = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var patchState = _a.patchState;
        var payload = _b.payload;
        return this.identityService.getRoles(payload).pipe(tap((/**
         * @param {?} roles
         * @return {?}
         */
        function (roles) {
            return patchState({
                roles: roles,
            });
        })));
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    IdentityState.prototype.getRole = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var patchState = _a.patchState;
        var payload = _b.payload;
        return this.identityService.getRoleById(payload).pipe(tap((/**
         * @param {?} selectedRole
         * @return {?}
         */
        function (selectedRole) {
            return patchState({
                selectedRole: selectedRole,
            });
        })));
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    IdentityState.prototype.deleteRole = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var dispatch = _a.dispatch;
        var payload = _b.payload;
        return this.identityService.deleteRole(payload).pipe(switchMap((/**
         * @return {?}
         */
        function () { return dispatch(new GetRoles()); })));
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    IdentityState.prototype.addRole = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var dispatch = _a.dispatch;
        var payload = _b.payload;
        return this.identityService.createRole(payload).pipe(switchMap((/**
         * @return {?}
         */
        function () { return dispatch(new GetRoles()); })));
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    IdentityState.prototype.updateRole = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var _this = this;
        var getState = _a.getState, dispatch = _a.dispatch;
        var payload = _b.payload;
        return dispatch(new GetRoleById(payload.id)).pipe(switchMap((/**
         * @return {?}
         */
        function () { return _this.identityService.updateRole(tslib_1.__assign({}, getState().selectedRole, payload)); })), switchMap((/**
         * @return {?}
         */
        function () { return dispatch(new GetRoles()); })));
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    IdentityState.prototype.getUsers = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var patchState = _a.patchState;
        var payload = _b.payload;
        return this.identityService.getUsers(payload).pipe(tap((/**
         * @param {?} users
         * @return {?}
         */
        function (users) {
            return patchState({
                users: users,
            });
        })));
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    IdentityState.prototype.getUser = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var patchState = _a.patchState;
        var payload = _b.payload;
        return this.identityService.getUserById(payload).pipe(tap((/**
         * @param {?} selectedUser
         * @return {?}
         */
        function (selectedUser) {
            return patchState({
                selectedUser: selectedUser,
            });
        })));
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    IdentityState.prototype.deleteUser = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var dispatch = _a.dispatch;
        var payload = _b.payload;
        return this.identityService.deleteUser(payload).pipe(switchMap((/**
         * @return {?}
         */
        function () { return dispatch(new GetUsers()); })));
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    IdentityState.prototype.addUser = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var dispatch = _a.dispatch;
        var payload = _b.payload;
        return this.identityService.createUser(payload).pipe(switchMap((/**
         * @return {?}
         */
        function () { return dispatch(new GetUsers()); })));
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    IdentityState.prototype.updateUser = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var _this = this;
        var getState = _a.getState, dispatch = _a.dispatch;
        var payload = _b.payload;
        return dispatch(new GetUserById(payload.id)).pipe(switchMap((/**
         * @return {?}
         */
        function () { return _this.identityService.updateUser(tslib_1.__assign({}, getState().selectedUser, payload)); })), switchMap((/**
         * @return {?}
         */
        function () { return dispatch(new GetUsers()); })));
    };
    /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    IdentityState.prototype.getUserRoles = /**
     * @param {?} __0
     * @param {?} __1
     * @return {?}
     */
    function (_a, _b) {
        var patchState = _a.patchState;
        var payload = _b.payload;
        return this.identityService.getUserRoles(payload).pipe(pluck('items'), tap((/**
         * @param {?} selectedUserRoles
         * @return {?}
         */
        function (selectedUserRoles) {
            return patchState({
                selectedUserRoles: selectedUserRoles,
            });
        })));
    };
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
    return IdentityState;
}());
export { IdentityState };
if (false) {
    /**
     * @type {?}
     * @private
     */
    IdentityState.prototype.identityService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkuc3RhdGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy9pZGVudGl0eS5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLFNBQVMsRUFBRSxHQUFHLEVBQUUsS0FBSyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDdkQsT0FBTyxFQUNMLFVBQVUsRUFDVixVQUFVLEVBQ1YsVUFBVSxFQUNWLFVBQVUsRUFDVixXQUFXLEVBQ1gsUUFBUSxFQUNSLFdBQVcsRUFDWCxRQUFRLEVBQ1IsVUFBVSxFQUNWLFVBQVUsRUFDVixZQUFZLEdBQ2IsTUFBTSw2QkFBNkIsQ0FBQztBQUVyQyxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sOEJBQThCLENBQUM7O0lBMkI3RCx1QkFBb0IsZUFBZ0M7UUFBaEMsb0JBQWUsR0FBZixlQUFlLENBQWlCO0lBQUcsQ0FBQzs7Ozs7SUFuQmpELHNCQUFROzs7O0lBQWYsVUFBZ0IsRUFBeUI7WUFBdkIsZ0JBQUs7UUFDckIsT0FBTyxLQUFLLENBQUMsS0FBSyxDQUFDO0lBQ3JCLENBQUM7Ozs7O0lBR00sZ0NBQWtCOzs7O0lBQXpCLFVBQTBCLEVBQXlCO1lBQXZCLGdCQUFLO1FBQy9CLE9BQU8sS0FBSyxDQUFDLFVBQVUsQ0FBQztJQUMxQixDQUFDOzs7OztJQUdNLHNCQUFROzs7O0lBQWYsVUFBZ0IsRUFBeUI7WUFBdkIsZ0JBQUs7UUFDckIsT0FBTyxLQUFLLENBQUMsS0FBSyxDQUFDO0lBQ3JCLENBQUM7Ozs7O0lBR00sZ0NBQWtCOzs7O0lBQXpCLFVBQTBCLEVBQXlCO1lBQXZCLGdCQUFLO1FBQy9CLE9BQU8sS0FBSyxDQUFDLFVBQVUsQ0FBQztJQUMxQixDQUFDOzs7Ozs7SUFLRCxnQ0FBUTs7Ozs7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WUFDckIsUUFBUSxFQUFFLG1CQUFBLEVBQUUsS0FBSyxFQUFFLEVBQUUsRUFBRSxZQUFZLEVBQUUsRUFBRSxFQUFFLEtBQUssRUFBRSxFQUFFLEVBQUUsWUFBWSxFQUFFLEVBQUUsRUFBRSxFQUFrQjtTQUN6RixDQUFDO2lEQXNCcUMsZUFBZTtPQXJCekMsYUFBYSxDQWtIekI7SUFBRCxvQkFBQztDQUFBLElBQUE7U0FsSFksYUFBYTs7Ozs7O0lBcUJaLHdDQUF3QyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFjdGlvbiwgU2VsZWN0b3IsIFN0YXRlLCBTdGF0ZUNvbnRleHQgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBzd2l0Y2hNYXAsIHRhcCwgcGx1Y2sgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQge1xuICBDcmVhdGVSb2xlLFxuICBDcmVhdGVVc2VyLFxuICBEZWxldGVSb2xlLFxuICBEZWxldGVVc2VyLFxuICBHZXRSb2xlQnlJZCxcbiAgR2V0Um9sZXMsXG4gIEdldFVzZXJCeUlkLFxuICBHZXRVc2VycyxcbiAgVXBkYXRlUm9sZSxcbiAgVXBkYXRlVXNlcixcbiAgR2V0VXNlclJvbGVzLFxufSBmcm9tICcuLi9hY3Rpb25zL2lkZW50aXR5LmFjdGlvbnMnO1xuaW1wb3J0IHsgSWRlbnRpdHkgfSBmcm9tICcuLi9tb2RlbHMvaWRlbnRpdHknO1xuaW1wb3J0IHsgSWRlbnRpdHlTZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvaWRlbnRpdHkuc2VydmljZSc7XG5cbkBTdGF0ZTxJZGVudGl0eS5TdGF0ZT4oe1xuICBuYW1lOiAnSWRlbnRpdHlTdGF0ZScsXG4gIGRlZmF1bHRzOiB7IHJvbGVzOiB7fSwgc2VsZWN0ZWRSb2xlOiB7fSwgdXNlcnM6IHt9LCBzZWxlY3RlZFVzZXI6IHt9IH0gYXMgSWRlbnRpdHkuU3RhdGUsXG59KVxuZXhwb3J0IGNsYXNzIElkZW50aXR5U3RhdGUge1xuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0Um9sZXMoeyByb2xlcyB9OiBJZGVudGl0eS5TdGF0ZSk6IElkZW50aXR5LlJvbGVJdGVtW10ge1xuICAgIHJldHVybiByb2xlcy5pdGVtcztcbiAgfVxuXG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRSb2xlc1RvdGFsQ291bnQoeyByb2xlcyB9OiBJZGVudGl0eS5TdGF0ZSk6IG51bWJlciB7XG4gICAgcmV0dXJuIHJvbGVzLnRvdGFsQ291bnQ7XG4gIH1cblxuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0VXNlcnMoeyB1c2VycyB9OiBJZGVudGl0eS5TdGF0ZSk6IElkZW50aXR5LlVzZXJJdGVtW10ge1xuICAgIHJldHVybiB1c2Vycy5pdGVtcztcbiAgfVxuXG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRVc2Vyc1RvdGFsQ291bnQoeyB1c2VycyB9OiBJZGVudGl0eS5TdGF0ZSk6IG51bWJlciB7XG4gICAgcmV0dXJuIHVzZXJzLnRvdGFsQ291bnQ7XG4gIH1cblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGlkZW50aXR5U2VydmljZTogSWRlbnRpdHlTZXJ2aWNlKSB7fVxuXG4gIEBBY3Rpb24oR2V0Um9sZXMpXG4gIGdldFJvbGVzKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0Um9sZXMpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZ2V0Um9sZXMocGF5bG9hZCkucGlwZShcbiAgICAgIHRhcChyb2xlcyA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICByb2xlcyxcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKEdldFJvbGVCeUlkKVxuICBnZXRSb2xlKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0Um9sZUJ5SWQpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZ2V0Um9sZUJ5SWQocGF5bG9hZCkucGlwZShcbiAgICAgIHRhcChzZWxlY3RlZFJvbGUgPT5cbiAgICAgICAgcGF0Y2hTdGF0ZSh7XG4gICAgICAgICAgc2VsZWN0ZWRSb2xlLFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIEBBY3Rpb24oRGVsZXRlUm9sZSlcbiAgZGVsZXRlUm9sZSh7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRSb2xlQnlJZCkge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5kZWxldGVSb2xlKHBheWxvYWQpLnBpcGUoc3dpdGNoTWFwKCgpID0+IGRpc3BhdGNoKG5ldyBHZXRSb2xlcygpKSkpO1xuICB9XG5cbiAgQEFjdGlvbihDcmVhdGVSb2xlKVxuICBhZGRSb2xlKHsgZGlzcGF0Y2ggfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IENyZWF0ZVJvbGUpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuY3JlYXRlUm9sZShwYXlsb2FkKS5waXBlKHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgR2V0Um9sZXMoKSkpKTtcbiAgfVxuXG4gIEBBY3Rpb24oVXBkYXRlUm9sZSlcbiAgdXBkYXRlUm9sZSh7IGdldFN0YXRlLCBkaXNwYXRjaCB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogVXBkYXRlUm9sZSkge1xuICAgIHJldHVybiBkaXNwYXRjaChuZXcgR2V0Um9sZUJ5SWQocGF5bG9hZC5pZCkpLnBpcGUoXG4gICAgICBzd2l0Y2hNYXAoKCkgPT4gdGhpcy5pZGVudGl0eVNlcnZpY2UudXBkYXRlUm9sZSh7IC4uLmdldFN0YXRlKCkuc2VsZWN0ZWRSb2xlLCAuLi5wYXlsb2FkIH0pKSxcbiAgICAgIHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgR2V0Um9sZXMoKSkpLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKEdldFVzZXJzKVxuICBnZXRVc2Vycyh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFVzZXJzKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmdldFVzZXJzKHBheWxvYWQpLnBpcGUoXG4gICAgICB0YXAodXNlcnMgPT5cbiAgICAgICAgcGF0Y2hTdGF0ZSh7XG4gICAgICAgICAgdXNlcnMsXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihHZXRVc2VyQnlJZClcbiAgZ2V0VXNlcih7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFVzZXJCeUlkKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmdldFVzZXJCeUlkKHBheWxvYWQpLnBpcGUoXG4gICAgICB0YXAoc2VsZWN0ZWRVc2VyID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHNlbGVjdGVkVXNlcixcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKERlbGV0ZVVzZXIpXG4gIGRlbGV0ZVVzZXIoeyBkaXNwYXRjaCB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0VXNlckJ5SWQpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZGVsZXRlVXNlcihwYXlsb2FkKS5waXBlKHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgR2V0VXNlcnMoKSkpKTtcbiAgfVxuXG4gIEBBY3Rpb24oQ3JlYXRlVXNlcilcbiAgYWRkVXNlcih7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBDcmVhdGVVc2VyKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmNyZWF0ZVVzZXIocGF5bG9hZCkucGlwZShzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IEdldFVzZXJzKCkpKSk7XG4gIH1cblxuICBAQWN0aW9uKFVwZGF0ZVVzZXIpXG4gIHVwZGF0ZVVzZXIoeyBnZXRTdGF0ZSwgZGlzcGF0Y2ggfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IFVwZGF0ZVVzZXIpIHtcbiAgICByZXR1cm4gZGlzcGF0Y2gobmV3IEdldFVzZXJCeUlkKHBheWxvYWQuaWQpKS5waXBlKFxuICAgICAgc3dpdGNoTWFwKCgpID0+IHRoaXMuaWRlbnRpdHlTZXJ2aWNlLnVwZGF0ZVVzZXIoeyAuLi5nZXRTdGF0ZSgpLnNlbGVjdGVkVXNlciwgLi4ucGF5bG9hZCB9KSksXG4gICAgICBzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IEdldFVzZXJzKCkpKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihHZXRVc2VyUm9sZXMpXG4gIGdldFVzZXJSb2xlcyh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFVzZXJSb2xlcykge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5nZXRVc2VyUm9sZXMocGF5bG9hZCkucGlwZShcbiAgICAgIHBsdWNrKCdpdGVtcycpLFxuICAgICAgdGFwKHNlbGVjdGVkVXNlclJvbGVzID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHNlbGVjdGVkVXNlclJvbGVzLFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxufVxuIl19