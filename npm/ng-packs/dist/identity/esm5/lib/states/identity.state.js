/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkuc3RhdGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy9pZGVudGl0eS5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFhLEdBQUcsRUFBRSxLQUFLLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUN2RCxPQUFPLEVBQ0wsVUFBVSxFQUNWLFVBQVUsRUFDVixVQUFVLEVBQ1YsVUFBVSxFQUNWLFdBQVcsRUFDWCxRQUFRLEVBQ1IsV0FBVyxFQUNYLFFBQVEsRUFDUixVQUFVLEVBQ1YsVUFBVSxFQUNWLFlBQVksR0FDYixNQUFNLDZCQUE2QixDQUFDO0FBRXJDLE9BQU8sRUFBRSxlQUFlLEVBQUUsTUFBTSw4QkFBOEIsQ0FBQzs7SUEyQjdELHVCQUFvQixlQUFnQztRQUFoQyxvQkFBZSxHQUFmLGVBQWUsQ0FBaUI7SUFBRyxDQUFDOzs7OztJQW5CakQsc0JBQVE7Ozs7SUFBZixVQUFnQixFQUF5QjtZQUF2QixnQkFBSztRQUNyQixPQUFPLEtBQUssQ0FBQyxLQUFLLElBQUksRUFBRSxDQUFDO0lBQzNCLENBQUM7Ozs7O0lBR00sZ0NBQWtCOzs7O0lBQXpCLFVBQTBCLEVBQXlCO1lBQXZCLGdCQUFLO1FBQy9CLE9BQU8sS0FBSyxDQUFDLFVBQVUsSUFBSSxDQUFDLENBQUM7SUFDL0IsQ0FBQzs7Ozs7SUFHTSxzQkFBUTs7OztJQUFmLFVBQWdCLEVBQXlCO1lBQXZCLGdCQUFLO1FBQ3JCLE9BQU8sS0FBSyxDQUFDLEtBQUssSUFBSSxFQUFFLENBQUM7SUFDM0IsQ0FBQzs7Ozs7SUFHTSxnQ0FBa0I7Ozs7SUFBekIsVUFBMEIsRUFBeUI7WUFBdkIsZ0JBQUs7UUFDL0IsT0FBTyxLQUFLLENBQUMsVUFBVSxJQUFJLENBQUMsQ0FBQztJ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QUU7Ozs7aURBR1Y7SUFHRDtRQURDLFFBQVEsRUFBRTs7Ozt1Q0FHVjtJQUdEO1FBREMsUUFBUSxFQUFFOzs7O2lEQUdWO0lBbkJVLGFBQWE7UUFKekIsS0FBSyxDQUFpQjtZQUNyQixJQUFJLEVBQUUsZUFBZTtZQUNyQixRQUFRLEVBQUUsbUJBQUEsRUFBRSxLQUFLLEVBQUUsRUFBRSxFQUFFLFlBQVksRUFBRSxFQUFFLEVBQUUsS0FBSyxFQUFFLEVBQUUsRUFBRSxZQUFZLEVBQUUsRUFBRSxFQUFFLEVBQWtCO1NBQ3pGLENBQUM7aURBc0JxQyxlQUFlO09BckJ6QyxhQUFhLENBNEd6QjtJQUFELG9CQUFDO0NBQUEsSUFBQTtTQTVHWSxhQUFhOzs7Ozs7SUFxQlosd0NBQXdDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQWN0aW9uLCBTZWxlY3RvciwgU3RhdGUsIFN0YXRlQ29udGV4dCB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgc3dpdGNoTWFwLCB0YXAsIHBsdWNrIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xyXG5pbXBvcnQge1xyXG4gIENyZWF0ZVJvbGUsXHJcbiAgQ3JlYXRlVXNlcixcclxuICBEZWxldGVSb2xlLFxyXG4gIERlbGV0ZVVzZXIsXHJcbiAgR2V0Um9sZUJ5SWQsXHJcbiAgR2V0Um9sZXMsXHJcbiAgR2V0VXNlckJ5SWQsXHJcbiAgR2V0VXNlcnMsXHJcbiAgVXBkYXRlUm9sZSxcclxuICBVcGRhdGVVc2VyLFxyXG4gIEdldFVzZXJSb2xlcyxcclxufSBmcm9tICcuLi9hY3Rpb25zL2lkZW50aXR5LmFjdGlvbnMnO1xyXG5pbXBvcnQgeyBJZGVudGl0eSB9IGZyb20gJy4uL21vZGVscy9pZGVudGl0eSc7XHJcbmltcG9ydCB7IElkZW50aXR5U2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL2lkZW50aXR5LnNlcnZpY2UnO1xyXG5cclxuQFN0YXRlPElkZW50aXR5LlN0YXRlPih7XHJcbiAgbmFtZTogJ0lkZW50aXR5U3RhdGUnLFxyXG4gIGRlZmF1bHRzOiB7IHJvbGVzOiB7fSwgc2VsZWN0ZWRSb2xlOiB7fSwgdXNlcnM6IHt9LCBzZWxlY3RlZFVzZXI6IHt9IH0gYXMgSWRlbnRpdHkuU3RhdGUsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBJZGVudGl0eVN0YXRlIHtcclxuICBAU2VsZWN0b3IoKVxyXG4gIHN0YXRpYyBnZXRSb2xlcyh7IHJvbGVzIH06IElkZW50aXR5LlN0YXRlKTogSWRlbnRpdHkuUm9sZUl0ZW1bXSB7XHJcbiAgICByZXR1cm4gcm9sZXMuaXRlbXMgfHwgW107XHJcbiAgfVxyXG5cclxuICBAU2VsZWN0b3IoKVxyXG4gIHN0YXRpYyBnZXRSb2xlc1RvdGFsQ291bnQoeyByb2xlcyB9OiBJZGVudGl0eS5TdGF0ZSk6IG51bWJlciB7XHJcbiAgICByZXR1cm4gcm9sZXMudG90YWxDb3VudCB8fCAwO1xyXG4gIH1cclxuXHJcbiAgQFNlbGVjdG9yKClcclxuICBzdGF0aWMgZ2V0VXNlcnMoeyB1c2VycyB9OiBJZGVudGl0eS5TdGF0ZSk6IElkZW50aXR5LlVzZXJJdGVtW10ge1xyXG4gICAgcmV0dXJuIHVzZXJzLml0ZW1zIHx8IFtdO1xyXG4gIH1cclxuXHJcbiAgQFNlbGVjdG9yKClcclxuICBzdGF0aWMgZ2V0VXNlcnNUb3RhbENvdW50KHsgdXNlcnMgfTogSWRlbnRpdHkuU3RhdGUpOiBudW1iZXIge1xyXG4gICAgcmV0dXJuIHVzZXJzLnRvdGFsQ291bnQgfHwgMDtcclxuICB9XHJcblxyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgaWRlbnRpdHlTZXJ2aWNlOiBJZGVudGl0eVNlcnZpY2UpIHt9XHJcblxyXG4gIEBBY3Rpb24oR2V0Um9sZXMpXHJcbiAgZ2V0Um9sZXMoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRSb2xlcykge1xyXG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmdldFJvbGVzKHBheWxvYWQpLnBpcGUoXHJcbiAgICAgIHRhcChyb2xlcyA9PlxyXG4gICAgICAgIHBhdGNoU3RhdGUoe1xyXG4gICAgICAgICAgcm9sZXMsXHJcbiAgICAgICAgfSksXHJcbiAgICAgICksXHJcbiAgICApO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihHZXRSb2xlQnlJZClcclxuICBnZXRSb2xlKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0Um9sZUJ5SWQpIHtcclxuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5nZXRSb2xlQnlJZChwYXlsb2FkKS5waXBlKFxyXG4gICAgICB0YXAoc2VsZWN0ZWRSb2xlID0+XHJcbiAgICAgICAgcGF0Y2hTdGF0ZSh7XHJcbiAgICAgICAgICBzZWxlY3RlZFJvbGUsXHJcbiAgICAgICAgfSksXHJcbiAgICAgICksXHJcbiAgICApO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihEZWxldGVSb2xlKVxyXG4gIGRlbGV0ZVJvbGUoXywgeyBwYXlsb2FkIH06IEdldFJvbGVCeUlkKSB7XHJcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZGVsZXRlUm9sZShwYXlsb2FkKTtcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oQ3JlYXRlUm9sZSlcclxuICBhZGRSb2xlKF8sIHsgcGF5bG9hZCB9OiBDcmVhdGVSb2xlKSB7XHJcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuY3JlYXRlUm9sZShwYXlsb2FkKTtcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oVXBkYXRlUm9sZSlcclxuICB1cGRhdGVSb2xlKHsgZ2V0U3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IFVwZGF0ZVJvbGUpIHtcclxuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS51cGRhdGVSb2xlKHsgLi4uZ2V0U3RhdGUoKS5zZWxlY3RlZFJvbGUsIC4uLnBheWxvYWQgfSk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKEdldFVzZXJzKVxyXG4gIGdldFVzZXJzKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0VXNlcnMpIHtcclxuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5nZXRVc2VycyhwYXlsb2FkKS5waXBlKFxyXG4gICAgICB0YXAodXNlcnMgPT5cclxuICAgICAgICBwYXRjaFN0YXRlKHtcclxuICAgICAgICAgIHVzZXJzLFxyXG4gICAgICAgIH0pLFxyXG4gICAgICApLFxyXG4gICAgKTtcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oR2V0VXNlckJ5SWQpXHJcbiAgZ2V0VXNlcih7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFVzZXJCeUlkKSB7XHJcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZ2V0VXNlckJ5SWQocGF5bG9hZCkucGlwZShcclxuICAgICAgdGFwKHNlbGVjdGVkVXNlciA9PlxyXG4gICAgICAgIHBhdGNoU3RhdGUoe1xyXG4gICAgICAgICAgc2VsZWN0ZWRVc2VyLFxyXG4gICAgICAgIH0pLFxyXG4gICAgICApLFxyXG4gICAgKTtcclxuICB9XHJcblxyXG4gIEBBY3Rpb24oRGVsZXRlVXNlcilcclxuICBkZWxldGVVc2VyKF8sIHsgcGF5bG9hZCB9OiBHZXRVc2VyQnlJZCkge1xyXG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmRlbGV0ZVVzZXIocGF5bG9hZCk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKENyZWF0ZVVzZXIpXHJcbiAgYWRkVXNlcihfLCB7IHBheWxvYWQgfTogQ3JlYXRlVXNlcikge1xyXG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmNyZWF0ZVVzZXIocGF5bG9hZCk7XHJcbiAgfVxyXG5cclxuICBAQWN0aW9uKFVwZGF0ZVVzZXIpXHJcbiAgdXBkYXRlVXNlcih7IGdldFN0YXRlIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBVcGRhdGVVc2VyKSB7XHJcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UudXBkYXRlVXNlcih7IC4uLmdldFN0YXRlKCkuc2VsZWN0ZWRVc2VyLCAuLi5wYXlsb2FkIH0pO1xyXG4gIH1cclxuXHJcbiAgQEFjdGlvbihHZXRVc2VyUm9sZXMpXHJcbiAgZ2V0VXNlclJvbGVzKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0VXNlclJvbGVzKSB7XHJcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZ2V0VXNlclJvbGVzKHBheWxvYWQpLnBpcGUoXHJcbiAgICAgIHBsdWNrKCdpdGVtcycpLFxyXG4gICAgICB0YXAoc2VsZWN0ZWRVc2VyUm9sZXMgPT5cclxuICAgICAgICBwYXRjaFN0YXRlKHtcclxuICAgICAgICAgIHNlbGVjdGVkVXNlclJvbGVzLFxyXG4gICAgICAgIH0pLFxyXG4gICAgICApLFxyXG4gICAgKTtcclxuICB9XHJcbn1cclxuIl19