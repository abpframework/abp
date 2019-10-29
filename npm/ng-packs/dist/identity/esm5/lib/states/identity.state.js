/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import { Action, Selector, State } from '@ngxs/store';
import { switchMap, tap, pluck } from 'rxjs/operators';
import {
  CreateRole,
  CreateUser,
  DeleteRole,
  DeleteUser,
  GetRoleById,
  GetRoles,
  GetUserById,
  GetUsers,
  UpdateRole,
  UpdateUser,
  GetUserRoles,
} from '../actions/identity.actions';
import { IdentityService } from '../services/identity.service';
var IdentityState = /** @class */ (function() {
  function IdentityState(identityService) {
    this.identityService = identityService;
  }
  /**
   * @param {?} __0
   * @return {?}
   */
  IdentityState.getRoles
  /**
   * @param {?} __0
   * @return {?}
   */ = function(_a) {
    var roles = _a.roles;
    return roles.items || [];
  };
  /**
   * @param {?} __0
   * @return {?}
   */
  IdentityState.getRolesTotalCount
  /**
   * @param {?} __0
   * @return {?}
   */ = function(_a) {
    var roles = _a.roles;
    return roles.totalCount || 0;
  };
  /**
   * @param {?} __0
   * @return {?}
   */
  IdentityState.getUsers
  /**
   * @param {?} __0
   * @return {?}
   */ = function(_a) {
    var users = _a.users;
    return users.items || [];
  };
  /**
   * @param {?} __0
   * @return {?}
   */
  IdentityState.getUsersTotalCount
  /**
   * @param {?} __0
   * @return {?}
   */ = function(_a) {
    var users = _a.users;
    return users.totalCount || 0;
  };
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  IdentityState.prototype.getRoles
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */ = function(_a, _b) {
    var patchState = _a.patchState;
    var payload = _b.payload;
    return this.identityService.getRoles(payload).pipe(
      tap(
        /**
         * @param {?} roles
         * @return {?}
         */
        function(roles) {
          return patchState({
            roles: roles,
          });
        },
      ),
    );
  };
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  IdentityState.prototype.getRole
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */ = function(_a, _b) {
    var patchState = _a.patchState;
    var payload = _b.payload;
    return this.identityService.getRoleById(payload).pipe(
      tap(
        /**
         * @param {?} selectedRole
         * @return {?}
         */
        function(selectedRole) {
          return patchState({
            selectedRole: selectedRole,
          });
        },
      ),
    );
  };
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  IdentityState.prototype.deleteRole
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */ = function(_a, _b) {
    var dispatch = _a.dispatch;
    var payload = _b.payload;
    return this.identityService.deleteRole(payload).pipe(
      switchMap(
        /**
         * @return {?}
         */
        function() {
          return dispatch(new GetRoles());
        },
      ),
    );
  };
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  IdentityState.prototype.addRole
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */ = function(_a, _b) {
    var dispatch = _a.dispatch;
    var payload = _b.payload;
    return this.identityService.createRole(payload).pipe(
      switchMap(
        /**
         * @return {?}
         */
        function() {
          return dispatch(new GetRoles());
        },
      ),
    );
  };
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  IdentityState.prototype.updateRole
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */ = function(_a, _b) {
    var _this = this;
    var getState = _a.getState,
      dispatch = _a.dispatch;
    var payload = _b.payload;
    return dispatch(new GetRoleById(payload.id)).pipe(
      switchMap(
        /**
         * @return {?}
         */
        function() {
          return _this.identityService.updateRole(tslib_1.__assign({}, getState().selectedRole, payload));
        },
      ),
      switchMap(
        /**
         * @return {?}
         */
        function() {
          return dispatch(new GetRoles());
        },
      ),
    );
  };
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  IdentityState.prototype.getUsers
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */ = function(_a, _b) {
    var patchState = _a.patchState;
    var payload = _b.payload;
    return this.identityService.getUsers(payload).pipe(
      tap(
        /**
         * @param {?} users
         * @return {?}
         */
        function(users) {
          return patchState({
            users: users,
          });
        },
      ),
    );
  };
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  IdentityState.prototype.getUser
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */ = function(_a, _b) {
    var patchState = _a.patchState;
    var payload = _b.payload;
    return this.identityService.getUserById(payload).pipe(
      tap(
        /**
         * @param {?} selectedUser
         * @return {?}
         */
        function(selectedUser) {
          return patchState({
            selectedUser: selectedUser,
          });
        },
      ),
    );
  };
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  IdentityState.prototype.deleteUser
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */ = function(_a, _b) {
    var dispatch = _a.dispatch;
    var payload = _b.payload;
    return this.identityService.deleteUser(payload).pipe(
      switchMap(
        /**
         * @return {?}
         */
        function() {
          return dispatch(new GetUsers());
        },
      ),
    );
  };
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  IdentityState.prototype.addUser
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */ = function(_a, _b) {
    var dispatch = _a.dispatch;
    var payload = _b.payload;
    return this.identityService.createUser(payload).pipe(
      switchMap(
        /**
         * @return {?}
         */
        function() {
          return dispatch(new GetUsers());
        },
      ),
    );
  };
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  IdentityState.prototype.updateUser
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */ = function(_a, _b) {
    var _this = this;
    var getState = _a.getState,
      dispatch = _a.dispatch;
    var payload = _b.payload;
    return dispatch(new GetUserById(payload.id)).pipe(
      switchMap(
        /**
         * @return {?}
         */
        function() {
          return _this.identityService.updateUser(tslib_1.__assign({}, getState().selectedUser, payload));
        },
      ),
      switchMap(
        /**
         * @return {?}
         */
        function() {
          return dispatch(new GetUsers());
        },
      ),
    );
  };
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  IdentityState.prototype.getUserRoles
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */ = function(_a, _b) {
    var patchState = _a.patchState;
    var payload = _b.payload;
    return this.identityService.getUserRoles(payload).pipe(
      pluck('items'),
      tap(
        /**
         * @param {?} selectedUserRoles
         * @return {?}
         */
        function(selectedUserRoles) {
          return patchState({
            selectedUserRoles: selectedUserRoles,
          });
        },
      ),
    );
  };
  IdentityState.ctorParameters = function() {
    return [{ type: IdentityService }];
  };
  tslib_1.__decorate(
    [
      Action(GetRoles),
      tslib_1.__metadata('design:type', Function),
      tslib_1.__metadata('design:paramtypes', [Object, GetRoles]),
      tslib_1.__metadata('design:returntype', void 0),
    ],
    IdentityState.prototype,
    'getRoles',
    null,
  );
  tslib_1.__decorate(
    [
      Action(GetRoleById),
      tslib_1.__metadata('design:type', Function),
      tslib_1.__metadata('design:paramtypes', [Object, GetRoleById]),
      tslib_1.__metadata('design:returntype', void 0),
    ],
    IdentityState.prototype,
    'getRole',
    null,
  );
  tslib_1.__decorate(
    [
      Action(DeleteRole),
      tslib_1.__metadata('design:type', Function),
      tslib_1.__metadata('design:paramtypes', [Object, GetRoleById]),
      tslib_1.__metadata('design:returntype', void 0),
    ],
    IdentityState.prototype,
    'deleteRole',
    null,
  );
  tslib_1.__decorate(
    [
      Action(CreateRole),
      tslib_1.__metadata('design:type', Function),
      tslib_1.__metadata('design:paramtypes', [Object, CreateRole]),
      tslib_1.__metadata('design:returntype', void 0),
    ],
    IdentityState.prototype,
    'addRole',
    null,
  );
  tslib_1.__decorate(
    [
      Action(UpdateRole),
      tslib_1.__metadata('design:type', Function),
      tslib_1.__metadata('design:paramtypes', [Object, UpdateRole]),
      tslib_1.__metadata('design:returntype', void 0),
    ],
    IdentityState.prototype,
    'updateRole',
    null,
  );
  tslib_1.__decorate(
    [
      Action(GetUsers),
      tslib_1.__metadata('design:type', Function),
      tslib_1.__metadata('design:paramtypes', [Object, GetUsers]),
      tslib_1.__metadata('design:returntype', void 0),
    ],
    IdentityState.prototype,
    'getUsers',
    null,
  );
  tslib_1.__decorate(
    [
      Action(GetUserById),
      tslib_1.__metadata('design:type', Function),
      tslib_1.__metadata('design:paramtypes', [Object, GetUserById]),
      tslib_1.__metadata('design:returntype', void 0),
    ],
    IdentityState.prototype,
    'getUser',
    null,
  );
  tslib_1.__decorate(
    [
      Action(DeleteUser),
      tslib_1.__metadata('design:type', Function),
      tslib_1.__metadata('design:paramtypes', [Object, GetUserById]),
      tslib_1.__metadata('design:returntype', void 0),
    ],
    IdentityState.prototype,
    'deleteUser',
    null,
  );
  tslib_1.__decorate(
    [
      Action(CreateUser),
      tslib_1.__metadata('design:type', Function),
      tslib_1.__metadata('design:paramtypes', [Object, CreateUser]),
      tslib_1.__metadata('design:returntype', void 0),
    ],
    IdentityState.prototype,
    'addUser',
    null,
  );
  tslib_1.__decorate(
    [
      Action(UpdateUser),
      tslib_1.__metadata('design:type', Function),
      tslib_1.__metadata('design:paramtypes', [Object, UpdateUser]),
      tslib_1.__metadata('design:returntype', void 0),
    ],
    IdentityState.prototype,
    'updateUser',
    null,
  );
  tslib_1.__decorate(
    [
      Action(GetUserRoles),
      tslib_1.__metadata('design:type', Function),
      tslib_1.__metadata('design:paramtypes', [Object, GetUserRoles]),
      tslib_1.__metadata('design:returntype', void 0),
    ],
    IdentityState.prototype,
    'getUserRoles',
    null,
  );
  tslib_1.__decorate(
    [
      Selector(),
      tslib_1.__metadata('design:type', Function),
      tslib_1.__metadata('design:paramtypes', [Object]),
      tslib_1.__metadata('design:returntype', Array),
    ],
    IdentityState,
    'getRoles',
    null,
  );
  tslib_1.__decorate(
    [
      Selector(),
      tslib_1.__metadata('design:type', Function),
      tslib_1.__metadata('design:paramtypes', [Object]),
      tslib_1.__metadata('design:returntype', Number),
    ],
    IdentityState,
    'getRolesTotalCount',
    null,
  );
  tslib_1.__decorate(
    [
      Selector(),
      tslib_1.__metadata('design:type', Function),
      tslib_1.__metadata('design:paramtypes', [Object]),
      tslib_1.__metadata('design:returntype', Array),
    ],
    IdentityState,
    'getUsers',
    null,
  );
  tslib_1.__decorate(
    [
      Selector(),
      tslib_1.__metadata('design:type', Function),
      tslib_1.__metadata('design:paramtypes', [Object]),
      tslib_1.__metadata('design:returntype', Number),
    ],
    IdentityState,
    'getUsersTotalCount',
    null,
  );
  IdentityState = tslib_1.__decorate(
    [
      State({
        name: 'IdentityState',
        defaults: /** @type {?} */ ({ roles: {}, selectedRole: {}, users: {}, selectedUser: {} }),
      }),
      tslib_1.__metadata('design:paramtypes', [IdentityService]),
    ],
    IdentityState,
  );
  return IdentityState;
})();
export { IdentityState };
if (false) {
  /**
   * @type {?}
   * @private
   */
  IdentityState.prototype.identityService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkuc3RhdGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy9pZGVudGl0eS5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLFNBQVMsRUFBRSxHQUFHLEVBQUUsS0FBSyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDdkQsT0FBTyxFQUNMLFVBQVUsRUFDVixVQUFVLEVBQ1YsVUFBVSxFQUNWLFVBQVUsRUFDVixXQUFXLEVBQ1gsUUFBUSxFQUNSLFdBQVcsRUFDWCxRQUFRLEVBQ1IsVUFBVSxFQUNWLFVBQVUsRUFDVixZQUFZLEdBQ2IsTUFBTSw2QkFBNkIsQ0FBQztBQUVyQyxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sOEJBQThCLENBQUM7O0lBMkI3RCx1QkFBb0IsZUFBZ0M7UUFBaEMsb0JBQWUsR0FBZixlQUFlLENBQWlCO0lBQUcsQ0FBQzs7Ozs7SUFuQmpELHNCQUFROzs7O0lBQWYsVUFBZ0IsRUFBeUI7WUFBdkIsZ0JBQUs7UUFDckIsT0FBTyxLQUFLLENBQUMsS0FBSyxJQUFJLEVBQUUsQ0FBQztJQUMzQixDQUFDOzs7OztJQUdNLGdDQUFrQjs7OztJQUF6QixVQUEwQixFQUF5QjtZQUF2QixnQkFBSztRQUMvQixPQUFPLEtBQUssQ0FBQyxVQUFVLElBQUksQ0FBQyxDQUFDO0lBQy9CLENBQUM7Ozs7O0lBR00sc0JBQVE7Ozs7SUFBZixVQUFnQixFQUF5QjtZQUF2QixnQkFBSztRQUNyQixPQUFPLEtBQUssQ0FBQyxLQUFLLElBQUksRUFBRSxDQUFDO0lBQzNCLENBQUM7Ozs7O0lBR00sZ0NBQWtCOzs7O0lBQXpCLFVBQTBCLEVBQXlCO1lBQXZCLGdCQUFLO1FBQy9CLE9BQU8sS0FBSyxDQUFDLFVBQVUsSUFBSSxD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aURBR1Y7SUFuQlUsYUFBYTtRQUp6QixLQUFLLENBQWlCO1lBQ3JCLElBQUksRUFBRSxlQUFlO1lBQ3JCLFFBQVEsRUFBRSxtQkFBQSxFQUFFLEtBQUssRUFBRSxFQUFFLEVBQUUsWUFBWSxFQUFFLEVBQUUsRUFBRSxLQUFLLEVBQUUsRUFBRSxFQUFFLFlBQVksRUFBRSxFQUFFLEVBQUUsRUFBa0I7U0FDekYsQ0FBQztpREFzQnFDLGVBQWU7T0FyQnpDLGFBQWEsQ0FrSHpCO0lBQUQsb0JBQUM7Q0FBQSxJQUFBO1NBbEhZLGFBQWE7Ozs7OztJQXFCWix3Q0FBd0MiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBY3Rpb24sIFNlbGVjdG9yLCBTdGF0ZSwgU3RhdGVDb250ZXh0IH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgc3dpdGNoTWFwLCB0YXAsIHBsdWNrIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHtcbiAgQ3JlYXRlUm9sZSxcbiAgQ3JlYXRlVXNlcixcbiAgRGVsZXRlUm9sZSxcbiAgRGVsZXRlVXNlcixcbiAgR2V0Um9sZUJ5SWQsXG4gIEdldFJvbGVzLFxuICBHZXRVc2VyQnlJZCxcbiAgR2V0VXNlcnMsXG4gIFVwZGF0ZVJvbGUsXG4gIFVwZGF0ZVVzZXIsXG4gIEdldFVzZXJSb2xlcyxcbn0gZnJvbSAnLi4vYWN0aW9ucy9pZGVudGl0eS5hY3Rpb25zJztcbmltcG9ydCB7IElkZW50aXR5IH0gZnJvbSAnLi4vbW9kZWxzL2lkZW50aXR5JztcbmltcG9ydCB7IElkZW50aXR5U2VydmljZSB9IGZyb20gJy4uL3NlcnZpY2VzL2lkZW50aXR5LnNlcnZpY2UnO1xuXG5AU3RhdGU8SWRlbnRpdHkuU3RhdGU+KHtcbiAgbmFtZTogJ0lkZW50aXR5U3RhdGUnLFxuICBkZWZhdWx0czogeyByb2xlczoge30sIHNlbGVjdGVkUm9sZToge30sIHVzZXJzOiB7fSwgc2VsZWN0ZWRVc2VyOiB7fSB9IGFzIElkZW50aXR5LlN0YXRlLFxufSlcbmV4cG9ydCBjbGFzcyBJZGVudGl0eVN0YXRlIHtcbiAgQFNlbGVjdG9yKClcbiAgc3RhdGljIGdldFJvbGVzKHsgcm9sZXMgfTogSWRlbnRpdHkuU3RhdGUpOiBJZGVudGl0eS5Sb2xlSXRlbVtdIHtcbiAgICByZXR1cm4gcm9sZXMuaXRlbXMgfHwgW107XG4gIH1cblxuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0Um9sZXNUb3RhbENvdW50KHsgcm9sZXMgfTogSWRlbnRpdHkuU3RhdGUpOiBudW1iZXIge1xuICAgIHJldHVybiByb2xlcy50b3RhbENvdW50IHx8IDA7XG4gIH1cblxuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0VXNlcnMoeyB1c2VycyB9OiBJZGVudGl0eS5TdGF0ZSk6IElkZW50aXR5LlVzZXJJdGVtW10ge1xuICAgIHJldHVybiB1c2Vycy5pdGVtcyB8fCBbXTtcbiAgfVxuXG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRVc2Vyc1RvdGFsQ291bnQoeyB1c2VycyB9OiBJZGVudGl0eS5TdGF0ZSk6IG51bWJlciB7XG4gICAgcmV0dXJuIHVzZXJzLnRvdGFsQ291bnQgfHwgMDtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgaWRlbnRpdHlTZXJ2aWNlOiBJZGVudGl0eVNlcnZpY2UpIHt9XG5cbiAgQEFjdGlvbihHZXRSb2xlcylcbiAgZ2V0Um9sZXMoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRSb2xlcykge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5nZXRSb2xlcyhwYXlsb2FkKS5waXBlKFxuICAgICAgdGFwKHJvbGVzID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHJvbGVzLFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIEBBY3Rpb24oR2V0Um9sZUJ5SWQpXG4gIGdldFJvbGUoeyBwYXRjaFN0YXRlIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRSb2xlQnlJZCkge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5nZXRSb2xlQnlJZChwYXlsb2FkKS5waXBlKFxuICAgICAgdGFwKHNlbGVjdGVkUm9sZSA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICBzZWxlY3RlZFJvbGUsXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihEZWxldGVSb2xlKVxuICBkZWxldGVSb2xlKHsgZGlzcGF0Y2ggfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFJvbGVCeUlkKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmRlbGV0ZVJvbGUocGF5bG9hZCkucGlwZShzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IEdldFJvbGVzKCkpKSk7XG4gIH1cblxuICBAQWN0aW9uKENyZWF0ZVJvbGUpXG4gIGFkZFJvbGUoeyBkaXNwYXRjaCB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogQ3JlYXRlUm9sZSkge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5jcmVhdGVSb2xlKHBheWxvYWQpLnBpcGUoc3dpdGNoTWFwKCgpID0+IGRpc3BhdGNoKG5ldyBHZXRSb2xlcygpKSkpO1xuICB9XG5cbiAgQEFjdGlvbihVcGRhdGVSb2xlKVxuICB1cGRhdGVSb2xlKHsgZ2V0U3RhdGUsIGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBVcGRhdGVSb2xlKSB7XG4gICAgcmV0dXJuIGRpc3BhdGNoKG5ldyBHZXRSb2xlQnlJZChwYXlsb2FkLmlkKSkucGlwZShcbiAgICAgIHN3aXRjaE1hcCgoKSA9PiB0aGlzLmlkZW50aXR5U2VydmljZS51cGRhdGVSb2xlKHsgLi4uZ2V0U3RhdGUoKS5zZWxlY3RlZFJvbGUsIC4uLnBheWxvYWQgfSkpLFxuICAgICAgc3dpdGNoTWFwKCgpID0+IGRpc3BhdGNoKG5ldyBHZXRSb2xlcygpKSksXG4gICAgKTtcbiAgfVxuXG4gIEBBY3Rpb24oR2V0VXNlcnMpXG4gIGdldFVzZXJzKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0VXNlcnMpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZ2V0VXNlcnMocGF5bG9hZCkucGlwZShcbiAgICAgIHRhcCh1c2VycyA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICB1c2VycyxcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKEdldFVzZXJCeUlkKVxuICBnZXRVc2VyKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0VXNlckJ5SWQpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZ2V0VXNlckJ5SWQocGF5bG9hZCkucGlwZShcbiAgICAgIHRhcChzZWxlY3RlZFVzZXIgPT5cbiAgICAgICAgcGF0Y2hTdGF0ZSh7XG4gICAgICAgICAgc2VsZWN0ZWRVc2VyLFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIEBBY3Rpb24oRGVsZXRlVXNlcilcbiAgZGVsZXRlVXNlcih7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRVc2VyQnlJZCkge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5kZWxldGVVc2VyKHBheWxvYWQpLnBpcGUoc3dpdGNoTWFwKCgpID0+IGRpc3BhdGNoKG5ldyBHZXRVc2VycygpKSkpO1xuICB9XG5cbiAgQEFjdGlvbihDcmVhdGVVc2VyKVxuICBhZGRVc2VyKHsgZGlzcGF0Y2ggfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IENyZWF0ZVVzZXIpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuY3JlYXRlVXNlcihwYXlsb2FkKS5waXBlKHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgR2V0VXNlcnMoKSkpKTtcbiAgfVxuXG4gIEBBY3Rpb24oVXBkYXRlVXNlcilcbiAgdXBkYXRlVXNlcih7IGdldFN0YXRlLCBkaXNwYXRjaCB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogVXBkYXRlVXNlcikge1xuICAgIHJldHVybiBkaXNwYXRjaChuZXcgR2V0VXNlckJ5SWQocGF5bG9hZC5pZCkpLnBpcGUoXG4gICAgICBzd2l0Y2hNYXAoKCkgPT4gdGhpcy5pZGVudGl0eVNlcnZpY2UudXBkYXRlVXNlcih7IC4uLmdldFN0YXRlKCkuc2VsZWN0ZWRVc2VyLCAuLi5wYXlsb2FkIH0pKSxcbiAgICAgIHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgR2V0VXNlcnMoKSkpLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKEdldFVzZXJSb2xlcylcbiAgZ2V0VXNlclJvbGVzKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0VXNlclJvbGVzKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmdldFVzZXJSb2xlcyhwYXlsb2FkKS5waXBlKFxuICAgICAgcGx1Y2soJ2l0ZW1zJyksXG4gICAgICB0YXAoc2VsZWN0ZWRVc2VyUm9sZXMgPT5cbiAgICAgICAgcGF0Y2hTdGF0ZSh7XG4gICAgICAgICAgc2VsZWN0ZWRVc2VyUm9sZXMsXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG59XG4iXX0=
