/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Action, Selector, State } from '@ngxs/store';
import { switchMap, tap, pluck } from 'rxjs/operators';
import { IdentityAddRole, IdentityAddUser, IdentityDeleteRole, IdentityDeleteUser, IdentityGetRoleById, IdentityGetRoles, IdentityGetUserById, IdentityGetUsers, IdentityUpdateRole, IdentityUpdateUser, IdentityGetUserRoles, } from '../actions/identity.actions';
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
     * @return {?}
     */
    IdentityState.prototype.getRoles = /**
     * @param {?} __0
     * @return {?}
     */
    function (_a) {
        var patchState = _a.patchState;
        return this.identityService.getRoles().pipe(tap((/**
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
        function () { return dispatch(new IdentityGetRoles()); })));
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
        return this.identityService.addRole(payload).pipe(switchMap((/**
         * @return {?}
         */
        function () { return dispatch(new IdentityGetRoles()); })));
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
        return dispatch(new IdentityGetRoleById(payload.id)).pipe(switchMap((/**
         * @return {?}
         */
        function () { return _this.identityService.updateRole(tslib_1.__assign({}, getState().selectedRole, payload)); })), switchMap((/**
         * @return {?}
         */
        function () { return dispatch(new IdentityGetRoles()); })));
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
        function () { return dispatch(new IdentityGetUsers()); })));
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
        return this.identityService.addUser(payload).pipe(switchMap((/**
         * @return {?}
         */
        function () { return dispatch(new IdentityGetUsers()); })));
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
        return dispatch(new IdentityGetUserById(payload.id)).pipe(switchMap((/**
         * @return {?}
         */
        function () { return _this.identityService.updateUser(tslib_1.__assign({}, getState().selectedUser, payload)); })), switchMap((/**
         * @return {?}
         */
        function () { return dispatch(new IdentityGetUsers()); })));
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkuc3RhdGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy9pZGVudGl0eS5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLFNBQVMsRUFBRSxHQUFHLEVBQUUsS0FBSyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDdkQsT0FBTyxFQUNMLGVBQWUsRUFDZixlQUFlLEVBQ2Ysa0JBQWtCLEVBQ2xCLGtCQUFrQixFQUNsQixtQkFBbUIsRUFDbkIsZ0JBQWdCLEVBQ2hCLG1CQUFtQixFQUNuQixnQkFBZ0IsRUFDaEIsa0JBQWtCLEVBQ2xCLGtCQUFrQixFQUNsQixvQkFBb0IsR0FDckIsTUFBTSw2QkFBNkIsQ0FBQztBQUVyQyxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sOEJBQThCLENBQUM7O0lBMkI3RCx1QkFBb0IsZUFBZ0M7UUFBaEMsb0JBQWUsR0FBZixlQUFlLENBQWlCO0lBQUcsQ0FBQzs7Ozs7SUFuQmpELHNCQUFROzs7O0lBQWYsVUFBZ0IsRUFBeUI7WUFBdkIsZ0JBQUs7UUFDckIsT0FBTyxLQUFLLENBQUMsS0FBSyxDQUFDO0lBQ3JCLENBQUM7Ozs7O0lBR00sZ0NBQWtCOzs7O0lBQXpCLFVBQTBCLEVBQXlCO1lBQXZCLGdCQUFLO1FBQy9CLE9BQU8sS0FBSyxDQUFDLFVBQVUsQ0FBQztJQUMxQixDQUFDOzs7OztJQUdNLHNCQUFROzs7O0lBQWYsVUFBZ0IsRUFBeUI7WUFBdkIsZ0JBQUs7UUFDckIsT0FBTyxLQUFLLENBQUMsS0FBSyxDQUFDO0lBQ3JCLENBQUM7Ozs7O0lBR00sZ0NBQWtCOzs7O0lBQXpCLFVBQTBCLEVBQXlCO1lBQXZCLGdCQUFLO1FBQy9CLE9BQU8sS0FBSyxDQUFDLFVBQVUsQ0FBQztJQUMxQixDQUFDOzs7OztJQUtELGdDQUFROzs7O0lBQVIsVUFBUyxFQUE0QztZQUExQywwQkFBVTtRQUNuQixPQUFPLElBQUksQ0FBQyxlQUFlLENBQUMsUUFBUSxFQUFFLENBQUMsSUFBSSxDQUN6QyxHQUFHOzs7O1FBQUMsVUFBQSxLQUFLO1lBQ1AsT0FBQSxVQUFVLENBQUM7Z0JBQ1QsS0FBSyxPQUFBO2FBQ04sQ0FBQztRQUZGLENBRUUsRUFDSCxDQUNGLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCwrQkFBTzs7Ozs7SUFBUCxVQUFRLEVBQTRDLEVBQUUsRUFBZ0M7WUFBNUUsMEJBQVU7WUFBb0Msb0JBQU87UUFDN0QsT0FBTyxJQUFJLENBQUMsZUFBZSxDQUFDLFdBQVcsQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQ25ELEdBQUc7Ozs7UUFBQyxVQUFBLFlBQVk7WUFDZCxPQUFBLFVBQVUsQ0FBQztnQkFDVCxZQUFZLGNBQUE7YUFDYixDQUFDO1FBRkYsQ0FFRSxFQUNILENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELGtDQUFVOzs7OztJQUFWLFVBQVcsRUFBMEMsRUFBRSxFQUFnQztZQUExRSxzQkFBUTtZQUFvQyxvQkFBTztRQUM5RCxPQUFPLElBQUksQ0FBQyxlQUFlLENBQUMsVUFBVSxDQUFDLE9BQU8sQ0FBQyxDQUFDLElBQUksQ0FBQyxTQUFTOzs7UUFBQyxjQUFNLE9BQUEsUUFBUSxDQUFDLElBQUksZ0JBQWdCLEVBQUUsQ0FBQyxFQUFoQyxDQUFnQyxFQUFDLENBQUMsQ0FBQztJQUMxRyxDQUFDOzs7Ozs7SUFHRCwrQkFBTzs7Ozs7SUFBUCxVQUFRLEVBQTBDLEVBQUUsRUFBNEI7WUFBdEUsc0JBQVE7WUFBb0Msb0JBQU87UUFDM0QsT0FBTyxJQUFJLENBQUMsZUFBZSxDQUFDLE9BQU8sQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQUMsU0FBUzs7O1FBQUMsY0FBTSxPQUFBLFFBQVEsQ0FBQyxJQUFJLGdCQUFnQixFQUFFLENBQUMsRUFBaEMsQ0FBZ0MsRUFBQyxDQUFDLENBQUM7SUFDdkcsQ0FBQzs7Ozs7O0lBR0Qsa0NBQVU7Ozs7O0lBQVYsVUFBVyxFQUFvRCxFQUFFLEVBQStCO1FBRGhHLGlCQU1DO1lBTFksc0JBQVEsRUFBRSxzQkFBUTtZQUFvQyxvQkFBTztRQUN4RSxPQUFPLFFBQVEsQ0FBQyxJQUFJLG1CQUFtQixDQUFDLE9BQU8sQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FDdkQsU0FBUzs7O1FBQUMsY0FBTSxPQUFBLEtBQUksQ0FBQyxlQUFlLENBQUMsVUFBVSxzQkFBTSxRQUFRLEVBQUUsQ0FBQyxZQUFZLEVBQUssT0FBTyxFQUFHLEVBQTNFLENBQTJFLEVBQUMsRUFDNUYsU0FBUzs7O1FBQUMsY0FBTSxPQUFBLFFBQVEsQ0FBQyxJQUFJLGdCQUFnQixFQUFFLENBQUMsRUFBaEMsQ0FBZ0MsRUFBQyxDQUNsRCxDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0QsZ0NBQVE7Ozs7O0lBQVIsVUFBUyxFQUE0QyxFQUFFLEVBQTZCO1lBQXpFLDBCQUFVO1lBQW9DLG9CQUFPO1FBQzlELE9BQU8sSUFBSSxDQUFDLGVBQWUsQ0FBQyxRQUFRLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUNoRCxHQUFHOzs7O1FBQUMsVUFBQSxLQUFLO1lBQ1AsT0FBQSxVQUFVLENBQUM7Z0JBQ1QsS0FBSyxPQUFBO2FBQ04sQ0FBQztRQUZGLENBRUUsRUFDSCxDQUNGLENBQUM7SUFDSixDQUFDOzs7Ozs7SUFHRCwrQkFBTzs7Ozs7SUFBUCxVQUFRLEVBQTRDLEVBQUUsRUFBZ0M7WUFBNUUsMEJBQVU7WUFBb0Msb0JBQU87UUFDN0QsT0FBTyxJQUFJLENBQUMsZUFBZSxDQUFDLFdBQVcsQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQ25ELEdBQUc7Ozs7UUFBQyxVQUFBLFlBQVk7WUFDZCxPQUFBLFVBQVUsQ0FBQztnQkFDVCxZQUFZLGNBQUE7YUFDYixDQUFDO1FBRkYsQ0FFRSxFQUNILENBQ0YsQ0FBQztJQUNKLENBQUM7Ozs7OztJQUdELGtDQUFVOzs7OztJQUFWLFVBQVcsRUFBMEMsRUFBRSxFQUFnQztZQUExRSxzQkFBUTtZQUFvQyxvQkFBTztRQUM5RCxPQUFPLElBQUksQ0FBQyxlQUFlLENBQUMsVUFBVSxDQUFDLE9BQU8sQ0FBQyxDQUFDLElBQUksQ0FBQyxTQUFTOzs7UUFBQyxjQUFNLE9BQUEsUUFBUSxDQUFDLElBQUksZ0JBQWdCLEVBQUUsQ0FBQyxFQUFoQyxDQUFnQyxFQUFDLENBQUMsQ0FBQztJQUMxRyxDQUFDOzs7Ozs7SUFHRCwrQkFBTzs7Ozs7SUFBUCxVQUFRLEVBQTBDLEVBQUUsRUFBNEI7WUFBdEUsc0JBQVE7WUFBb0Msb0JBQU87UUFDM0QsT0FBTyxJQUFJLENBQUMsZUFBZSxDQUFDLE9BQU8sQ0FBQyxPQUFPLENBQUMsQ0FBQyxJQUFJLENBQUMsU0FBUzs7O1FBQUMsY0FBTSxPQUFBLFFBQVEsQ0FBQyxJQUFJLGdCQUFnQixFQUFFLENBQUMsRUFBaEMsQ0FBZ0MsRUFBQyxDQUFDLENBQUM7SUFDdkcsQ0FBQzs7Ozs7O0lBR0Qsa0NBQVU7Ozs7O0lBQVYsVUFBVyxFQUFvRCxFQUFFLEVBQStCO1FBRGhHLGlCQU1DO1lBTFksc0JBQVEsRUFBRSxzQkFBUTtZQUFvQyxvQkFBTztRQUN4RSxPQUFPLFFBQVEsQ0FBQyxJQUFJLG1CQUFtQixDQUFDLE9BQU8sQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FDdkQsU0FBUzs7O1FBQUMsY0FBTSxPQUFBLEtBQUksQ0FBQyxlQUFlLENBQUMsVUFBVSxzQkFBTSxRQUFRLEVBQUUsQ0FBQyxZQUFZLEVBQUssT0FBTyxFQUFHLEVBQTNFLENBQTJFLEVBQUMsRUFDNUYsU0FBUzs7O1FBQUMsY0FBTSxPQUFBLFFBQVEsQ0FBQyxJQUFJLGdCQUFnQixFQUFFLENBQUMsRUFBaEMsQ0FBZ0MsRUFBQyxDQUNsRCxDQUFDO0lBQ0osQ0FBQzs7Ozs7O0lBR0Qsb0NBQVk7Ozs7O0lBQVosVUFBYSxFQUE0QyxFQUFFLEVBQWlDO1lBQTdFLDBCQUFVO1lBQW9DLG9CQUFPO1FBQ2xFLE9BQU8sSUFBSSxDQUFDLGVBQWUsQ0FBQyxZQUFZLENBQUMsT0FBTyxDQUFDLENBQUMsSUFBSSxDQUNwRCxLQUFLLENBQUMsT0FBTyxDQUFDLEVBQ2QsR0FBRzs7OztRQUFDLFVBQUEsaUJBQWlCO1lBQ25CLE9BQUEsVUFBVSxDQUFDO2dCQUNULGlCQUFpQixtQkFBQTthQUNsQixDQUFDO1FBRkYsQ0FFRSxFQUNILENBQ0YsQ0FBQztJQUNKLENBQUM7SUF6RkQ7UUFEQyxNQUFNLENBQUMsZ0JBQWdCLENBQUM7Ozs7aURBU3hCO0lBR0Q7UUFEQyxNQUFNLENBQUMsbUJBQW1CLENBQUM7O3lEQUN1QyxtQkFBbUI7O2dEQVFyRjtJQUdEO1FBREMsTUFBTSxDQUFDLGtCQUFrQixDQUFDOzt5REFDeUMsbUJBQW1COzttREFFdEY7SUFHRDtRQURDLE1BQU0sQ0FBQyxlQUFlLENBQUM7O3lEQUN5QyxlQUFlOztnREFFL0U7SUFHRDtRQURDLE1BQU0sQ0FBQyxrQkFBa0IsQ0FBQzs7eURBQ21ELGtCQUFrQjs7bURBSy9GO0lBR0Q7UUFEQyxNQUFNLENBQUMsZ0JBQWdCLENBQUM7O3lEQUMyQyxnQkFBZ0I7O2lEQVFuRjtJQUdEO1FBREMsTUFBTSxDQUFDLG1CQUFtQixDQUFDOzt5REFDdUMsbUJBQW1COztnREFRckY7SUFHRDtRQURDLE1BQU0sQ0FBQyxrQkFBa0IsQ0FBQzs7eURBQ3lDLG1CQUFtQjs7bURBRXRGO0lBR0Q7UUFEQyxNQUFNLENBQUMsZUFBZSxDQUFDOzt5REFDeUMsZUFBZTs7Z0RBRS9FO0lBR0Q7UUFEQyxNQUFNLENBQUMsa0JBQWtCLENBQUM7O3lEQUNtRCxrQkFBa0I7O21EQUsvRjtJQUdEO1FBREMsTUFBTSxDQUFDLG9CQUFvQixDQUFDOzt5REFDMkMsb0JBQW9COztxREFTM0Y7SUEvR0Q7UUFEQyxRQUFRLEVBQUU7Ozs7dUNBR1Y7SUFHRDtRQURDLFFBQVEsRUFBRTs7OztpREFHVjtJQUdEO1FBREMsUUFBUSxFQUFFOzs7O3VDQUdWO0lBR0Q7UUFEQyxRQUFRLEVBQUU7Ozs7aURBR1Y7SUFuQlUsYUFBYTtRQUp6QixLQUFLLENBQWlCO1lBQ3JCLElBQUksRUFBRSxlQUFlO1lBQ3JCLFFBQVEsRUFBRSxtQkFBQSxFQUFFLEtBQUssRUFBRSxFQUFFLEVBQUUsWUFBWSxFQUFFLEVBQUUsRUFBRSxLQUFLLEVBQUUsRUFBRSxFQUFFLFlBQVksRUFBRSxFQUFFLEVBQUUsRUFBa0I7U0FDekYsQ0FBQztpREFzQnFDLGVBQWU7T0FyQnpDLGFBQWEsQ0FrSHpCO0lBQUQsb0JBQUM7Q0FBQSxJQUFBO1NBbEhZLGFBQWE7Ozs7OztJQXFCWix3Q0FBd0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBY3Rpb24sIFNlbGVjdG9yLCBTdGF0ZSwgU3RhdGVDb250ZXh0IH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgc3dpdGNoTWFwLCB0YXAsIHBsdWNrIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHtcbiAgSWRlbnRpdHlBZGRSb2xlLFxuICBJZGVudGl0eUFkZFVzZXIsXG4gIElkZW50aXR5RGVsZXRlUm9sZSxcbiAgSWRlbnRpdHlEZWxldGVVc2VyLFxuICBJZGVudGl0eUdldFJvbGVCeUlkLFxuICBJZGVudGl0eUdldFJvbGVzLFxuICBJZGVudGl0eUdldFVzZXJCeUlkLFxuICBJZGVudGl0eUdldFVzZXJzLFxuICBJZGVudGl0eVVwZGF0ZVJvbGUsXG4gIElkZW50aXR5VXBkYXRlVXNlcixcbiAgSWRlbnRpdHlHZXRVc2VyUm9sZXMsXG59IGZyb20gJy4uL2FjdGlvbnMvaWRlbnRpdHkuYWN0aW9ucyc7XG5pbXBvcnQgeyBJZGVudGl0eSB9IGZyb20gJy4uL21vZGVscy9pZGVudGl0eSc7XG5pbXBvcnQgeyBJZGVudGl0eVNlcnZpY2UgfSBmcm9tICcuLi9zZXJ2aWNlcy9pZGVudGl0eS5zZXJ2aWNlJztcblxuQFN0YXRlPElkZW50aXR5LlN0YXRlPih7XG4gIG5hbWU6ICdJZGVudGl0eVN0YXRlJyxcbiAgZGVmYXVsdHM6IHsgcm9sZXM6IHt9LCBzZWxlY3RlZFJvbGU6IHt9LCB1c2Vyczoge30sIHNlbGVjdGVkVXNlcjoge30gfSBhcyBJZGVudGl0eS5TdGF0ZSxcbn0pXG5leHBvcnQgY2xhc3MgSWRlbnRpdHlTdGF0ZSB7XG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRSb2xlcyh7IHJvbGVzIH06IElkZW50aXR5LlN0YXRlKTogSWRlbnRpdHkuUm9sZUl0ZW1bXSB7XG4gICAgcmV0dXJuIHJvbGVzLml0ZW1zO1xuICB9XG5cbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldFJvbGVzVG90YWxDb3VudCh7IHJvbGVzIH06IElkZW50aXR5LlN0YXRlKTogbnVtYmVyIHtcbiAgICByZXR1cm4gcm9sZXMudG90YWxDb3VudDtcbiAgfVxuXG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRVc2Vycyh7IHVzZXJzIH06IElkZW50aXR5LlN0YXRlKTogSWRlbnRpdHkuVXNlckl0ZW1bXSB7XG4gICAgcmV0dXJuIHVzZXJzLml0ZW1zO1xuICB9XG5cbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldFVzZXJzVG90YWxDb3VudCh7IHVzZXJzIH06IElkZW50aXR5LlN0YXRlKTogbnVtYmVyIHtcbiAgICByZXR1cm4gdXNlcnMudG90YWxDb3VudDtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgaWRlbnRpdHlTZXJ2aWNlOiBJZGVudGl0eVNlcnZpY2UpIHt9XG5cbiAgQEFjdGlvbihJZGVudGl0eUdldFJvbGVzKVxuICBnZXRSb2xlcyh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPikge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5nZXRSb2xlcygpLnBpcGUoXG4gICAgICB0YXAocm9sZXMgPT5cbiAgICAgICAgcGF0Y2hTdGF0ZSh7XG4gICAgICAgICAgcm9sZXMsXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihJZGVudGl0eUdldFJvbGVCeUlkKVxuICBnZXRSb2xlKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogSWRlbnRpdHlHZXRSb2xlQnlJZCkge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5nZXRSb2xlQnlJZChwYXlsb2FkKS5waXBlKFxuICAgICAgdGFwKHNlbGVjdGVkUm9sZSA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICBzZWxlY3RlZFJvbGUsXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihJZGVudGl0eURlbGV0ZVJvbGUpXG4gIGRlbGV0ZVJvbGUoeyBkaXNwYXRjaCB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogSWRlbnRpdHlHZXRSb2xlQnlJZCkge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5kZWxldGVSb2xlKHBheWxvYWQpLnBpcGUoc3dpdGNoTWFwKCgpID0+IGRpc3BhdGNoKG5ldyBJZGVudGl0eUdldFJvbGVzKCkpKSk7XG4gIH1cblxuICBAQWN0aW9uKElkZW50aXR5QWRkUm9sZSlcbiAgYWRkUm9sZSh7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBJZGVudGl0eUFkZFJvbGUpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuYWRkUm9sZShwYXlsb2FkKS5waXBlKHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgSWRlbnRpdHlHZXRSb2xlcygpKSkpO1xuICB9XG5cbiAgQEFjdGlvbihJZGVudGl0eVVwZGF0ZVJvbGUpXG4gIHVwZGF0ZVJvbGUoeyBnZXRTdGF0ZSwgZGlzcGF0Y2ggfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IElkZW50aXR5VXBkYXRlUm9sZSkge1xuICAgIHJldHVybiBkaXNwYXRjaChuZXcgSWRlbnRpdHlHZXRSb2xlQnlJZChwYXlsb2FkLmlkKSkucGlwZShcbiAgICAgIHN3aXRjaE1hcCgoKSA9PiB0aGlzLmlkZW50aXR5U2VydmljZS51cGRhdGVSb2xlKHsgLi4uZ2V0U3RhdGUoKS5zZWxlY3RlZFJvbGUsIC4uLnBheWxvYWQgfSkpLFxuICAgICAgc3dpdGNoTWFwKCgpID0+IGRpc3BhdGNoKG5ldyBJZGVudGl0eUdldFJvbGVzKCkpKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihJZGVudGl0eUdldFVzZXJzKVxuICBnZXRVc2Vycyh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IElkZW50aXR5R2V0VXNlcnMpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZ2V0VXNlcnMocGF5bG9hZCkucGlwZShcbiAgICAgIHRhcCh1c2VycyA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICB1c2VycyxcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKElkZW50aXR5R2V0VXNlckJ5SWQpXG4gIGdldFVzZXIoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBJZGVudGl0eUdldFVzZXJCeUlkKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmdldFVzZXJCeUlkKHBheWxvYWQpLnBpcGUoXG4gICAgICB0YXAoc2VsZWN0ZWRVc2VyID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHNlbGVjdGVkVXNlcixcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKElkZW50aXR5RGVsZXRlVXNlcilcbiAgZGVsZXRlVXNlcih7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBJZGVudGl0eUdldFVzZXJCeUlkKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmRlbGV0ZVVzZXIocGF5bG9hZCkucGlwZShzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IElkZW50aXR5R2V0VXNlcnMoKSkpKTtcbiAgfVxuXG4gIEBBY3Rpb24oSWRlbnRpdHlBZGRVc2VyKVxuICBhZGRVc2VyKHsgZGlzcGF0Y2ggfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IElkZW50aXR5QWRkVXNlcikge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5hZGRVc2VyKHBheWxvYWQpLnBpcGUoc3dpdGNoTWFwKCgpID0+IGRpc3BhdGNoKG5ldyBJZGVudGl0eUdldFVzZXJzKCkpKSk7XG4gIH1cblxuICBAQWN0aW9uKElkZW50aXR5VXBkYXRlVXNlcilcbiAgdXBkYXRlVXNlcih7IGdldFN0YXRlLCBkaXNwYXRjaCB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogSWRlbnRpdHlVcGRhdGVVc2VyKSB7XG4gICAgcmV0dXJuIGRpc3BhdGNoKG5ldyBJZGVudGl0eUdldFVzZXJCeUlkKHBheWxvYWQuaWQpKS5waXBlKFxuICAgICAgc3dpdGNoTWFwKCgpID0+IHRoaXMuaWRlbnRpdHlTZXJ2aWNlLnVwZGF0ZVVzZXIoeyAuLi5nZXRTdGF0ZSgpLnNlbGVjdGVkVXNlciwgLi4ucGF5bG9hZCB9KSksXG4gICAgICBzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IElkZW50aXR5R2V0VXNlcnMoKSkpLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKElkZW50aXR5R2V0VXNlclJvbGVzKVxuICBnZXRVc2VyUm9sZXMoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBJZGVudGl0eUdldFVzZXJSb2xlcykge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5nZXRVc2VyUm9sZXMocGF5bG9hZCkucGlwZShcbiAgICAgIHBsdWNrKCdpdGVtcycpLFxuICAgICAgdGFwKHNlbGVjdGVkVXNlclJvbGVzID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHNlbGVjdGVkVXNlclJvbGVzLFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxufVxuIl19