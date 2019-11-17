/**
 * @fileoverview added by tsickle
 * Generated from: lib/states/identity.state.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
        var getState = _a.getState, dispatch = _a.dispatch;
        var payload = _b.payload;
        return this.identityService
            .updateRole(tslib_1.__assign({}, getState().selectedRole, payload))
            .pipe(switchMap((/**
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
        var getState = _a.getState, dispatch = _a.dispatch;
        var payload = _b.payload;
        return this.identityService
            .updateUser(tslib_1.__assign({}, getState().selectedUser, payload))
            .pipe(switchMap((/**
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkuc3RhdGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy9pZGVudGl0eS5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQUUsTUFBTSxFQUFFLFFBQVEsRUFBRSxLQUFLLEVBQWdCLE1BQU0sYUFBYSxDQUFDO0FBQ3BFLE9BQU8sRUFBRSxTQUFTLEVBQUUsR0FBRyxFQUFFLEtBQUssRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3ZELE9BQU8sRUFDTCxVQUFVLEVBQ1YsVUFBVSxFQUNWLFVBQVUsRUFDVixVQUFVLEVBQ1YsV0FBVyxFQUNYLFFBQVEsRUFDUixXQUFXLEVBQ1gsUUFBUSxFQUNSLFVBQVUsRUFDVixVQUFVLEVBQ1YsWUFBWSxHQUNiLE1BQU0sNkJBQTZCLENBQUM7QUFFckMsT0FBTyxFQUFFLGVBQWUsRUFBRSxNQUFNLDhCQUE4QixDQUFDOztJQTJCN0QsdUJBQW9CLGVBQWdDO1FBQWhDLG9CQUFlLEdBQWYsZUFBZSxDQUFpQjtJQUFHLENBQUM7Ozs7O0lBbkJqRCxzQkFBUTs7OztJQUFmLFVBQWdCLEVBQXlCO1lBQXZCLGdCQUFLO1FBQ3JCLE9BQU8sS0FBSyxDQUFDLEtBQUssSUFBSSxFQUFFLENBQUM7SUFDM0IsQ0FBQzs7Ozs7SUFHTSxnQ0FBa0I7Ozs7SUFBekIsVUFBMEIsRUFBeUI7WUFBdkIsZ0JBQUs7UUFDL0IsT0FBTyxLQUFLLENBQUMsVUFBVSxJQUFJLENBQUMsQ0FBQztJQUMvQixDQUFDOzs7OztJQUdNLHNCQUFROzs7O0lBQWYsVUFBZ0IsRUFBeUI7WUFBdkIsZ0JBQUs7UUFDckIsT0FBTyxLQUFLLENBQUMsS0FBSyxJQUFJLEVBQUUsQ0FBQztJQUMzQixDQUFDOzs7OztJQUdNLGdDQUFrQjs7OztJQUF6QixVQUEwQixFQUF5QjtZQUF2QixnQkFBSztRQUMvQixPQUFPLEtBQUssQ0FBQyxVQUFVLElBQUks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QjtJQUFELG9CQUFDO0NBQUEsSUFBQTtTQWhIWSxhQUFhOzs7Ozs7SUFxQlosd0NBQXdDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQWN0aW9uLCBTZWxlY3RvciwgU3RhdGUsIFN0YXRlQ29udGV4dCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgc3dpdGNoTWFwLCB0YXAsIHBsdWNrIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5pbXBvcnQge1xyXG4gIENyZWF0ZVJvbGUsXHJcbiAgQ3JlYXRlVXNlcixcclxuICBEZWxldGVSb2xlLFxyXG4gIERlbGV0ZVVzZXIsXHJcbiAgR2V0Um9sZUJ5SWQsXHJcbiAgR2V0Um9sZXMsXHJcbiAgR2V0VXNlckJ5SWQsXHJcbiAgR2V0VXNlcnMsXHJcbiAgVXBkYXRlUm9sZSxcclxuICBVcGRhdGVVc2VyLFxyXG4gIEdldFVzZXJSb2xlcyxcclxufSBmcm9tICcuLi9hY3Rpb25zL2lkZW50aXR5LmFjdGlvbnMnO1xyXG5pbXBvcnQgeyBJZGVudGl0eSB9IGZyb20gJy4uL21vZGVscy9pZGVudGl0eSc7XHJcbmltcG9ydCB7IElkZW50aXR5U2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL2lkZW50aXR5LnNlcnZpY2UnO1xyXG5cclxuQFN0YXRlPElkZW50aXR5LlN0YXRlPih7XHJcbiAgbmFtZTogJ0lkZW50aXR5U3RhdGUnLFxyXG4gIGRlZmF1bHRzOiB7IHJvbGVzOiB7fSwgc2VsZWN0ZWRSb2xlOiB7fSwgdXNlcnM6IHt9LCBzZWxlY3RlZFVzZXI6IHt9IH0gYXMgSWRlbnRpdHkuU3RhdGUsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBJZGVudGl0eVN0YXRlIHtcclxuICBAU2VsZWN0b3IoKVxyXG4gIHN0YXRpYyBnZXRSb2xlcyh7IHJvbGVzIH06IElkZW50aXR5LlN0YXRlKTogSWRlbnRpdHkuUm9sZUl0ZW1bXSB7XHJcbiAgICByZXR1cm4gcm9sZXMuaXRlbXMgfHwgW107XHJcbiAgfVxyXG5cclxuICBAU2VsZWN0b3IoKVxyXG4gIHN0YXRpYyBnZXRSb2xlc1RvdGFsQ291bnQoeyByb2xlcyB9OiBJZGVudGl0eS5TdGF0ZSk6IG51bWJlciB7XHJcbiAgICByZXR1cm4gcm9sZXMudG90YWxDb3VudCB8fCAwO1xyXG4gIH1cclxuXHJcbiAgQFNlbGVjdG9yKClcclxuICBzdGF0aWMgZ2V0VXNlcnMoeyB1c2VycyB9OiBJZGVudGl0eS5TdGF0ZSk6IElkZW50aXR5LlVzZXJJdGVtW10ge1xyXG4gICAgcmV0dXJuIHVzZXJzLml0ZW1zIHx8IFtdO1xyXG4gIH1cclxuXHJcbiAgQFNlbGVjdG9yKClcclxuICBzdGF0aWMgZ2V0VXNlcnNUb3RhbENvdW50KHsgdXNlcnMgfTogSWRlbnRpdHkuU3RhdGUpOiBudW1iZXIge1xyXG4gICAgcmV0dXJuIHVzZXJzLnRvdGFsQ291bnQgfHwgMDtcclxuICB9XHJcblxyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgaWRlbnRpdHlTZXJ2aWNlOiBJZGVudGl0eVNlcnZpY2UpIHt9XHJcblxyXG4gIEBBY3Rpb24oR2V0Um9sZXMpXHJcbiAgZ2V0Um9sZXMoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRSb2xlcykge1xyXG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmdldFJvbGVzKHBheWxvYWQpLnBpcGUoXHJcbiAgICAgIHRhcChyb2xlcyA9PlxyXG4gICAgICAgIHBhdGNoU3RhdGUoe1xyXG4gICAgICAgICAgcm9sZXMsXHJcbiAgICAgICAgfSksXHJcbiAgICAgICksXHJcbiAgICApO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihHZXRSb2xlQnlJZClcclxuICBnZXRSb2xlKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0Um9sZUJ5SWQpIHtcclxuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5nZXRSb2xlQnlJZChwYXlsb2FkKS5waXBlKFxyXG4gICAgICB0YXAoc2VsZWN0ZWRSb2xlID0+XHJcbiAgICAgICAgcGF0Y2hTdGF0ZSh7XHJcbiAgICAgICAgICBzZWxlY3RlZFJvbGUsXHJcbiAgICAgICAgfSksXHJcbiAgICAgICksXHJcbiAgICApO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihEZWxldGVSb2xlKVxyXG4gIGRlbGV0ZVJvbGUoeyBkaXNwYXRjaCB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0Um9sZUJ5SWQpIHtcclxuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5kZWxldGVSb2xlKHBheWxvYWQpLnBpcGUoc3dpdGNoTWFwKCgpID0+IGRpc3BhdGNoKG5ldyBHZXRSb2xlcygpKSkpO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihDcmVhdGVSb2xlKVxyXG4gIGFkZFJvbGUoeyBkaXNwYXRjaCB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogQ3JlYXRlUm9sZSkge1xyXG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmNyZWF0ZVJvbGUocGF5bG9hZCkucGlwZShzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IEdldFJvbGVzKCkpKSk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKFVwZGF0ZVJvbGUpXHJcbiAgdXBkYXRlUm9sZSh7IGdldFN0YXRlLCBkaXNwYXRjaCB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogVXBkYXRlUm9sZSkge1xyXG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlXHJcbiAgICAgIC51cGRhdGVSb2xlKHsgLi4uZ2V0U3RhdGUoKS5zZWxlY3RlZFJvbGUsIC4uLnBheWxvYWQgfSlcclxuICAgICAgLnBpcGUoc3dpdGNoTWFwKCgpID0+IGRpc3BhdGNoKG5ldyBHZXRSb2xlcygpKSkpO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihHZXRVc2VycylcclxuICBnZXRVc2Vycyh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFVzZXJzKSB7XHJcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZ2V0VXNlcnMocGF5bG9hZCkucGlwZShcclxuICAgICAgdGFwKHVzZXJzID0+XHJcbiAgICAgICAgcGF0Y2hTdGF0ZSh7XHJcbiAgICAgICAgICB1c2VycyxcclxuICAgICAgICB9KSxcclxuICAgICAgKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKEdldFVzZXJCeUlkKVxyXG4gIGdldFVzZXIoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRVc2VyQnlJZCkge1xyXG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmdldFVzZXJCeUlkKHBheWxvYWQpLnBpcGUoXHJcbiAgICAgIHRhcChzZWxlY3RlZFVzZXIgPT5cclxuICAgICAgICBwYXRjaFN0YXRlKHtcclxuICAgICAgICAgIHNlbGVjdGVkVXNlcixcclxuICAgICAgICB9KSxcclxuICAgICAgKSxcclxuICAgICk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKERlbGV0ZVVzZXIpXHJcbiAgZGVsZXRlVXNlcih7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRVc2VyQnlJZCkge1xyXG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmRlbGV0ZVVzZXIocGF5bG9hZCkucGlwZShzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IEdldFVzZXJzKCkpKSk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKENyZWF0ZVVzZXIpXHJcbiAgYWRkVXNlcih7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBDcmVhdGVVc2VyKSB7XHJcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuY3JlYXRlVXNlcihwYXlsb2FkKS5waXBlKHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgR2V0VXNlcnMoKSkpKTtcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oVXBkYXRlVXNlcilcclxuICB1cGRhdGVVc2VyKHsgZ2V0U3RhdGUsIGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBVcGRhdGVVc2VyKSB7XHJcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2VcclxuICAgICAgLnVwZGF0ZVVzZXIoeyAuLi5nZXRTdGF0ZSgpLnNlbGVjdGVkVXNlciwgLi4ucGF5bG9hZCB9KVxyXG4gICAgICAucGlwZShzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IEdldFVzZXJzKCkpKSk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKEdldFVzZXJSb2xlcylcclxuICBnZXRVc2VyUm9sZXMoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRVc2VyUm9sZXMpIHtcclxuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5nZXRVc2VyUm9sZXMocGF5bG9hZCkucGlwZShcclxuICAgICAgcGx1Y2soJ2l0ZW1zJyksXHJcbiAgICAgIHRhcChzZWxlY3RlZFVzZXJSb2xlcyA9PlxyXG4gICAgICAgIHBhdGNoU3RhdGUoe1xyXG4gICAgICAgICAgc2VsZWN0ZWRVc2VyUm9sZXMsXHJcbiAgICAgICAgfSksXHJcbiAgICAgICksXHJcbiAgICApO1xyXG4gIH1cclxufVxyXG4iXX0=