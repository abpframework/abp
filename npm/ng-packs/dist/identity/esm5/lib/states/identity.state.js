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
        return roles.items || [];
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
        return roles.totalCount || 0;
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
        return users.items || [];
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
        return users.totalCount || 0;
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
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    IdentityState.prototype.deleteRole = /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    function (_, _a) {
        var payload = _a.payload;
        return this.identityService.deleteRole(payload);
    };
    /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    IdentityState.prototype.addRole = /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    function (_, _a) {
        var payload = _a.payload;
        return this.identityService.createRole(payload);
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
        var getState = _a.getState;
        var payload = _b.payload;
        return this.identityService.updateRole(tslib_1.__assign({}, getState().selectedRole, payload));
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
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    IdentityState.prototype.deleteUser = /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    function (_, _a) {
        var payload = _a.payload;
        return this.identityService.deleteUser(payload);
    };
    /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    IdentityState.prototype.addUser = /**
     * @param {?} _
     * @param {?} __1
     * @return {?}
     */
    function (_, _a) {
        var payload = _a.payload;
        return this.identityService.createUser(payload);
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
        var getState = _a.getState;
        var payload = _b.payload;
        return this.identityService.updateUser(tslib_1.__assign({}, getState().selectedUser, payload));
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
    IdentityState.ctorParameters = function () { return [
        { type: IdentityService }
    ]; };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkuc3RhdGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy9pZGVudGl0eS5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLFFBQVEsRUFBRSxLQUFLLEVBQWdCLE1BQU0sYUFBYSxDQUFDO0FBQ3BFLE9BQU8sRUFBYSxHQUFHLEVBQUUsS0FBSyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDdkQsT0FBTyxFQUNMLFVBQVUsRUFDVixVQUFVLEVBQ1YsVUFBVSxFQUNWLFVBQVUsRUFDVixXQUFXLEVBQ1gsUUFBUSxFQUNSLFdBQVcsRUFDWCxRQUFRLEVBQ1IsVUFBVSxFQUNWLFVBQVUsRUFDVixZQUFZLEdBQ2IsTUFBTSw2QkFBNkIsQ0FBQztBQUVyQyxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sOEJBQThCLENBQUM7O0lBMkI3RCx1QkFBb0IsZUFBZ0M7UUFBaEMsb0JBQWUsR0FBZixlQUFlLENBQWlCO0lBQUcsQ0FBQzs7Ozs7SUFuQmpELHNCQUFROzs7O0lBQWYsVUFBZ0IsRUFBeUI7WUFBdkIsZ0JBQUs7UUFDckIsT0FBTyxLQUFLLENBQUMsS0FBSyxJQUFJLEVBQUUsQ0FBQztJQUMzQixDQUFDOzs7OztJQUdNLGdDQUFrQjs7OztJQUF6QixVQUEwQixFQUF5QjtZQUF2QixnQkFBSztRQUMvQixPQUFPLEtBQUssQ0FBQyxVQUFVLElBQUksQ0FBQyxDQUFDO0lBQy9CLENBQUM7Ozs7O0lBR00sc0JBQVE7Ozs7SUFBZixVQUFnQixFQUF5QjtZQUF2QixnQkFBSztRQUNyQixPQUFPLEtBQUssQ0FBQyxLQUFLLElBQUksRUFBRSxDQUFDO0lBQzNCLENBQUM7Ozs7O0lBR00sZ0NBQWtCOzs7O0lBQXpCLFVBQTBCLEVBQXlCO1lBQXZCLGdCQUFLO1FBQy9CLE9BQU8sS0FBSyxDQUFDLFVBQVUsSUFBSSxDQUFDLENBQUM7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QUFFOzs7O2lEQUdWO0lBR0Q7UUFEQyxRQUFRLEVBQUU7Ozs7dUNBR1Y7SUFHRDtRQURDLFFBQVEsRUFBRTs7OztpREFHVjtJQW5CVSxhQUFhO1FBSnpCLEtBQUssQ0FBaUI7WUFDckIsSUFBSSxFQUFFLGVBQWU7WUFDckIsUUFBUSxFQUFFLG1CQUFBLEVBQUUsS0FBSyxFQUFFLEVBQUUsRUFBRSxZQUFZLEVBQUUsRUFBRSxFQUFFLEtBQUssRUFBRSxFQUFFLEVBQUUsWUFBWSxFQUFFLEVBQUUsRUFBRSxFQUFrQjtTQUN6RixDQUFDO2lEQXNCcUMsZUFBZTtPQXJCekMsYUFBYSxDQTRHekI7SUFBRCxvQkFBQztDQUFBLElBQUE7U0E1R1ksYUFBYTs7Ozs7O0lBcUJaLHdDQUF3QyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFjdGlvbiwgU2VsZWN0b3IsIFN0YXRlLCBTdGF0ZUNvbnRleHQgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBzd2l0Y2hNYXAsIHRhcCwgcGx1Y2sgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQge1xuICBDcmVhdGVSb2xlLFxuICBDcmVhdGVVc2VyLFxuICBEZWxldGVSb2xlLFxuICBEZWxldGVVc2VyLFxuICBHZXRSb2xlQnlJZCxcbiAgR2V0Um9sZXMsXG4gIEdldFVzZXJCeUlkLFxuICBHZXRVc2VycyxcbiAgVXBkYXRlUm9sZSxcbiAgVXBkYXRlVXNlcixcbiAgR2V0VXNlclJvbGVzLFxufSBmcm9tICcuLi9hY3Rpb25zL2lkZW50aXR5LmFjdGlvbnMnO1xuaW1wb3J0IHsgSWRlbnRpdHkgfSBmcm9tICcuLi9tb2RlbHMvaWRlbnRpdHknO1xuaW1wb3J0IHsgSWRlbnRpdHlTZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvaWRlbnRpdHkuc2VydmljZSc7XG5cbkBTdGF0ZTxJZGVudGl0eS5TdGF0ZT4oe1xuICBuYW1lOiAnSWRlbnRpdHlTdGF0ZScsXG4gIGRlZmF1bHRzOiB7IHJvbGVzOiB7fSwgc2VsZWN0ZWRSb2xlOiB7fSwgdXNlcnM6IHt9LCBzZWxlY3RlZFVzZXI6IHt9IH0gYXMgSWRlbnRpdHkuU3RhdGUsXG59KVxuZXhwb3J0IGNsYXNzIElkZW50aXR5U3RhdGUge1xuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0Um9sZXMoeyByb2xlcyB9OiBJZGVudGl0eS5TdGF0ZSk6IElkZW50aXR5LlJvbGVJdGVtW10ge1xuICAgIHJldHVybiByb2xlcy5pdGVtcyB8fCBbXTtcbiAgfVxuXG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRSb2xlc1RvdGFsQ291bnQoeyByb2xlcyB9OiBJZGVudGl0eS5TdGF0ZSk6IG51bWJlciB7XG4gICAgcmV0dXJuIHJvbGVzLnRvdGFsQ291bnQgfHwgMDtcbiAgfVxuXG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRVc2Vycyh7IHVzZXJzIH06IElkZW50aXR5LlN0YXRlKTogSWRlbnRpdHkuVXNlckl0ZW1bXSB7XG4gICAgcmV0dXJuIHVzZXJzLml0ZW1zIHx8IFtdO1xuICB9XG5cbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldFVzZXJzVG90YWxDb3VudCh7IHVzZXJzIH06IElkZW50aXR5LlN0YXRlKTogbnVtYmVyIHtcbiAgICByZXR1cm4gdXNlcnMudG90YWxDb3VudCB8fCAwO1xuICB9XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBpZGVudGl0eVNlcnZpY2U6IElkZW50aXR5U2VydmljZSkge31cblxuICBAQWN0aW9uKEdldFJvbGVzKVxuICBnZXRSb2xlcyh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFJvbGVzKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmdldFJvbGVzKHBheWxvYWQpLnBpcGUoXG4gICAgICB0YXAocm9sZXMgPT5cbiAgICAgICAgcGF0Y2hTdGF0ZSh7XG4gICAgICAgICAgcm9sZXMsXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihHZXRSb2xlQnlJZClcbiAgZ2V0Um9sZSh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFJvbGVCeUlkKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmdldFJvbGVCeUlkKHBheWxvYWQpLnBpcGUoXG4gICAgICB0YXAoc2VsZWN0ZWRSb2xlID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHNlbGVjdGVkUm9sZSxcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKERlbGV0ZVJvbGUpXG4gIGRlbGV0ZVJvbGUoXywgeyBwYXlsb2FkIH06IEdldFJvbGVCeUlkKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmRlbGV0ZVJvbGUocGF5bG9hZCk7XG4gIH1cblxuICBAQWN0aW9uKENyZWF0ZVJvbGUpXG4gIGFkZFJvbGUoXywgeyBwYXlsb2FkIH06IENyZWF0ZVJvbGUpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuY3JlYXRlUm9sZShwYXlsb2FkKTtcbiAgfVxuXG4gIEBBY3Rpb24oVXBkYXRlUm9sZSlcbiAgdXBkYXRlUm9sZSh7IGdldFN0YXRlIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBVcGRhdGVSb2xlKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLnVwZGF0ZVJvbGUoeyAuLi5nZXRTdGF0ZSgpLnNlbGVjdGVkUm9sZSwgLi4ucGF5bG9hZCB9KTtcbiAgfVxuXG4gIEBBY3Rpb24oR2V0VXNlcnMpXG4gIGdldFVzZXJzKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0VXNlcnMpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZ2V0VXNlcnMocGF5bG9hZCkucGlwZShcbiAgICAgIHRhcCh1c2VycyA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICB1c2VycyxcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKEdldFVzZXJCeUlkKVxuICBnZXRVc2VyKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0VXNlckJ5SWQpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZ2V0VXNlckJ5SWQocGF5bG9hZCkucGlwZShcbiAgICAgIHRhcChzZWxlY3RlZFVzZXIgPT5cbiAgICAgICAgcGF0Y2hTdGF0ZSh7XG4gICAgICAgICAgc2VsZWN0ZWRVc2VyLFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIEBBY3Rpb24oRGVsZXRlVXNlcilcbiAgZGVsZXRlVXNlcihfLCB7IHBheWxvYWQgfTogR2V0VXNlckJ5SWQpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZGVsZXRlVXNlcihwYXlsb2FkKTtcbiAgfVxuXG4gIEBBY3Rpb24oQ3JlYXRlVXNlcilcbiAgYWRkVXNlcihfLCB7IHBheWxvYWQgfTogQ3JlYXRlVXNlcikge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5jcmVhdGVVc2VyKHBheWxvYWQpO1xuICB9XG5cbiAgQEFjdGlvbihVcGRhdGVVc2VyKVxuICB1cGRhdGVVc2VyKHsgZ2V0U3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IFVwZGF0ZVVzZXIpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UudXBkYXRlVXNlcih7IC4uLmdldFN0YXRlKCkuc2VsZWN0ZWRVc2VyLCAuLi5wYXlsb2FkIH0pO1xuICB9XG5cbiAgQEFjdGlvbihHZXRVc2VyUm9sZXMpXG4gIGdldFVzZXJSb2xlcyh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFVzZXJSb2xlcykge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5nZXRVc2VyUm9sZXMocGF5bG9hZCkucGlwZShcbiAgICAgIHBsdWNrKCdpdGVtcycpLFxuICAgICAgdGFwKHNlbGVjdGVkVXNlclJvbGVzID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHNlbGVjdGVkVXNlclJvbGVzLFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxufVxuIl19