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
   * @param {?} __1
   * @return {?}
   */
  getRoles({ patchState }, { payload }) {
    return this.identityService.getRoles(payload).pipe(
      tap(
        /**
         * @param {?} roles
         * @return {?}
         */
        roles =>
          patchState({
            roles,
          }),
      ),
    );
  }
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  getRole({ patchState }, { payload }) {
    return this.identityService.getRoleById(payload).pipe(
      tap(
        /**
         * @param {?} selectedRole
         * @return {?}
         */
        selectedRole =>
          patchState({
            selectedRole,
          }),
      ),
    );
  }
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  deleteRole({ dispatch }, { payload }) {
    return this.identityService.deleteRole(payload).pipe(
      switchMap(
        /**
         * @return {?}
         */
        () => dispatch(new GetRoles()),
      ),
    );
  }
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  addRole({ dispatch }, { payload }) {
    return this.identityService.createRole(payload).pipe(
      switchMap(
        /**
         * @return {?}
         */
        () => dispatch(new GetRoles()),
      ),
    );
  }
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  updateRole({ getState, dispatch }, { payload }) {
    return dispatch(new GetRoleById(payload.id)).pipe(
      switchMap(
        /**
         * @return {?}
         */
        () => this.identityService.updateRole(Object.assign({}, getState().selectedRole, payload)),
      ),
      switchMap(
        /**
         * @return {?}
         */
        () => dispatch(new GetRoles()),
      ),
    );
  }
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  getUsers({ patchState }, { payload }) {
    return this.identityService.getUsers(payload).pipe(
      tap(
        /**
         * @param {?} users
         * @return {?}
         */
        users =>
          patchState({
            users,
          }),
      ),
    );
  }
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  getUser({ patchState }, { payload }) {
    return this.identityService.getUserById(payload).pipe(
      tap(
        /**
         * @param {?} selectedUser
         * @return {?}
         */
        selectedUser =>
          patchState({
            selectedUser,
          }),
      ),
    );
  }
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  deleteUser({ dispatch }, { payload }) {
    return this.identityService.deleteUser(payload).pipe(
      switchMap(
        /**
         * @return {?}
         */
        () => dispatch(new GetUsers()),
      ),
    );
  }
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  addUser({ dispatch }, { payload }) {
    return this.identityService.createUser(payload).pipe(
      switchMap(
        /**
         * @return {?}
         */
        () => dispatch(new GetUsers()),
      ),
    );
  }
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  updateUser({ getState, dispatch }, { payload }) {
    return dispatch(new GetUserById(payload.id)).pipe(
      switchMap(
        /**
         * @return {?}
         */
        () => this.identityService.updateUser(Object.assign({}, getState().selectedUser, payload)),
      ),
      switchMap(
        /**
         * @return {?}
         */
        () => dispatch(new GetUsers()),
      ),
    );
  }
  /**
   * @param {?} __0
   * @param {?} __1
   * @return {?}
   */
  getUserRoles({ patchState }, { payload }) {
    return this.identityService.getUserRoles(payload).pipe(
      pluck('items'),
      tap(
        /**
         * @param {?} selectedUserRoles
         * @return {?}
         */
        selectedUserRoles =>
          patchState({
            selectedUserRoles,
          }),
      ),
    );
  }
};
IdentityState.ctorParameters = () => [{ type: IdentityService }];
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
export { IdentityState };
if (false) {
  /**
   * @type {?}
   * @private
   */
  IdentityState.prototype.identityService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWRlbnRpdHkuc3RhdGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmlkZW50aXR5LyIsInNvdXJjZXMiOlsibGliL3N0YXRlcy9pZGVudGl0eS5zdGF0ZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxNQUFNLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBZ0IsTUFBTSxhQUFhLENBQUM7QUFDcEUsT0FBTyxFQUFFLFNBQVMsRUFBRSxHQUFHLEVBQUUsS0FBSyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDdkQsT0FBTyxFQUNMLFVBQVUsRUFDVixVQUFVLEVBQ1YsVUFBVSxFQUNWLFVBQVUsRUFDVixXQUFXLEVBQ1gsUUFBUSxFQUNSLFdBQVcsRUFDWCxRQUFRLEVBQ1IsVUFBVSxFQUNWLFVBQVUsRUFDVixZQUFZLEdBQ2IsTUFBTSw2QkFBNkIsQ0FBQztBQUVyQyxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sOEJBQThCLENBQUM7SUFNbEQsYUFBYSxTQUFiLGFBQWE7Ozs7SUFxQnhCLFlBQW9CLGVBQWdDO1FBQWhDLG9CQUFlLEdBQWYsZUFBZSxDQUFpQjtJQUFHLENBQUM7Ozs7O0lBbkJ4RCxNQUFNLENBQUMsUUFBUSxDQUFDLEVBQUUsS0FBSyxFQUFrQjtRQUN2QyxPQUFPLEtBQUssQ0FBQyxLQUFLLENBQUM7SUFDckIsQ0FBQzs7Ozs7SUFHRCxNQUFNLENBQUMsa0JBQWtCLENBQUMsRUFBRSxLQUFLLEVBQWtCO1FBQ2pELE9BQU8sS0FBSyxDQUFDLFVBQVUsQ0FBQztJQUMxQixDQUFDOzs7OztJQUdELE1BQU0sQ0FBQyxRQUFRLENBQUMsRUFBRSxLQUFLLEVBQWtCO1FBQ3ZDLE9BQU8sS0FBSyxDQUFDLEtBQUssQ0FBQztJQUNyQixDQUFDOzs7OztJQUdELE1BQU0sQ0FBQyxrQkFBa0IsQ0FBQyxFQUFFLEtBQUssRUFBa0I7UUFDakQsT0FBTyxLQUFLLENBQUMsVUFBVSxDQUFDO0lBQzFCLENBQUM7Ozs7OztJ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TUFBTSxDQUFDLFVBQVUsQ0FBQzs7cURBQ2lELFdBQVc7OytDQUU5RTtBQUdEO0lBREMsTUFBTSxDQUFDLFVBQVUsQ0FBQzs7cURBQzhDLFVBQVU7OzRDQUUxRTtBQUdEO0lBREMsTUFBTSxDQUFDLFVBQVUsQ0FBQzs7cURBQzJELFVBQVU7OytDQUt2RjtBQUdEO0lBREMsTUFBTSxDQUFDLFFBQVEsQ0FBQzs7cURBQ21ELFFBQVE7OzZDQVEzRTtBQUdEO0lBREMsTUFBTSxDQUFDLFdBQVcsQ0FBQzs7cURBQytDLFdBQVc7OzRDQVE3RTtBQUdEO0lBREMsTUFBTSxDQUFDLFVBQVUsQ0FBQzs7cURBQ2lELFdBQVc7OytDQUU5RTtBQUdEO0lBREMsTUFBTSxDQUFDLFVBQVUsQ0FBQzs7cURBQzhDLFVBQVU7OzRDQUUxRTtBQUdEO0lBREMsTUFBTSxDQUFDLFVBQVUsQ0FBQzs7cURBQzJELFVBQVU7OytDQUt2RjtBQUdEO0lBREMsTUFBTSxDQUFDLFlBQVksQ0FBQzs7cURBQ21ELFlBQVk7O2lEQVNuRjtBQS9HRDtJQURDLFFBQVEsRUFBRTs7OzttQ0FHVjtBQUdEO0lBREMsUUFBUSxFQUFFOzs7OzZDQUdWO0FBR0Q7SUFEQyxRQUFRLEVBQUU7Ozs7bUNBR1Y7QUFHRDtJQURDLFFBQVEsRUFBRTs7Ozs2Q0FHVjtBQW5CVSxhQUFhO0lBSnpCLEtBQUssQ0FBaUI7UUFDckIsSUFBSSxFQUFFLGVBQWU7UUFDckIsUUFBUSxFQUFFLG1CQUFBLEVBQUUsS0FBSyxFQUFFLEVBQUUsRUFBRSxZQUFZLEVBQUUsRUFBRSxFQUFFLEtBQUssRUFBRSxFQUFFLEVBQUUsWUFBWSxFQUFFLEVBQUUsRUFBRSxFQUFrQjtLQUN6RixDQUFDOzZDQXNCcUMsZUFBZTtHQXJCekMsYUFBYSxDQWtIekI7U0FsSFksYUFBYTs7Ozs7O0lBcUJaLHdDQUF3QyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFjdGlvbiwgU2VsZWN0b3IsIFN0YXRlLCBTdGF0ZUNvbnRleHQgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBzd2l0Y2hNYXAsIHRhcCwgcGx1Y2sgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQge1xuICBDcmVhdGVSb2xlLFxuICBDcmVhdGVVc2VyLFxuICBEZWxldGVSb2xlLFxuICBEZWxldGVVc2VyLFxuICBHZXRSb2xlQnlJZCxcbiAgR2V0Um9sZXMsXG4gIEdldFVzZXJCeUlkLFxuICBHZXRVc2VycyxcbiAgVXBkYXRlUm9sZSxcbiAgVXBkYXRlVXNlcixcbiAgR2V0VXNlclJvbGVzLFxufSBmcm9tICcuLi9hY3Rpb25zL2lkZW50aXR5LmFjdGlvbnMnO1xuaW1wb3J0IHsgSWRlbnRpdHkgfSBmcm9tICcuLi9tb2RlbHMvaWRlbnRpdHknO1xuaW1wb3J0IHsgSWRlbnRpdHlTZXJ2aWNlIH0gZnJvbSAnLi4vc2VydmljZXMvaWRlbnRpdHkuc2VydmljZSc7XG5cbkBTdGF0ZTxJZGVudGl0eS5TdGF0ZT4oe1xuICBuYW1lOiAnSWRlbnRpdHlTdGF0ZScsXG4gIGRlZmF1bHRzOiB7IHJvbGVzOiB7fSwgc2VsZWN0ZWRSb2xlOiB7fSwgdXNlcnM6IHt9LCBzZWxlY3RlZFVzZXI6IHt9IH0gYXMgSWRlbnRpdHkuU3RhdGUsXG59KVxuZXhwb3J0IGNsYXNzIElkZW50aXR5U3RhdGUge1xuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0Um9sZXMoeyByb2xlcyB9OiBJZGVudGl0eS5TdGF0ZSk6IElkZW50aXR5LlJvbGVJdGVtW10ge1xuICAgIHJldHVybiByb2xlcy5pdGVtcztcbiAgfVxuXG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRSb2xlc1RvdGFsQ291bnQoeyByb2xlcyB9OiBJZGVudGl0eS5TdGF0ZSk6IG51bWJlciB7XG4gICAgcmV0dXJuIHJvbGVzLnRvdGFsQ291bnQ7XG4gIH1cblxuICBAU2VsZWN0b3IoKVxuICBzdGF0aWMgZ2V0VXNlcnMoeyB1c2VycyB9OiBJZGVudGl0eS5TdGF0ZSk6IElkZW50aXR5LlVzZXJJdGVtW10ge1xuICAgIHJldHVybiB1c2Vycy5pdGVtcztcbiAgfVxuXG4gIEBTZWxlY3RvcigpXG4gIHN0YXRpYyBnZXRVc2Vyc1RvdGFsQ291bnQoeyB1c2VycyB9OiBJZGVudGl0eS5TdGF0ZSk6IG51bWJlciB7XG4gICAgcmV0dXJuIHVzZXJzLnRvdGFsQ291bnQ7XG4gIH1cblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGlkZW50aXR5U2VydmljZTogSWRlbnRpdHlTZXJ2aWNlKSB7fVxuXG4gIEBBY3Rpb24oR2V0Um9sZXMpXG4gIGdldFJvbGVzKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0Um9sZXMpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZ2V0Um9sZXMocGF5bG9hZCkucGlwZShcbiAgICAgIHRhcChyb2xlcyA9PlxuICAgICAgICBwYXRjaFN0YXRlKHtcbiAgICAgICAgICByb2xlcyxcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKEdldFJvbGVCeUlkKVxuICBnZXRSb2xlKHsgcGF0Y2hTdGF0ZSB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0Um9sZUJ5SWQpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZ2V0Um9sZUJ5SWQocGF5bG9hZCkucGlwZShcbiAgICAgIHRhcChzZWxlY3RlZFJvbGUgPT5cbiAgICAgICAgcGF0Y2hTdGF0ZSh7XG4gICAgICAgICAgc2VsZWN0ZWRSb2xlLFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxuXG4gIEBBY3Rpb24oRGVsZXRlUm9sZSlcbiAgZGVsZXRlUm9sZSh7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBHZXRSb2xlQnlJZCkge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5kZWxldGVSb2xlKHBheWxvYWQpLnBpcGUoc3dpdGNoTWFwKCgpID0+IGRpc3BhdGNoKG5ldyBHZXRSb2xlcygpKSkpO1xuICB9XG5cbiAgQEFjdGlvbihDcmVhdGVSb2xlKVxuICBhZGRSb2xlKHsgZGlzcGF0Y2ggfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IENyZWF0ZVJvbGUpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuY3JlYXRlUm9sZShwYXlsb2FkKS5waXBlKHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgR2V0Um9sZXMoKSkpKTtcbiAgfVxuXG4gIEBBY3Rpb24oVXBkYXRlUm9sZSlcbiAgdXBkYXRlUm9sZSh7IGdldFN0YXRlLCBkaXNwYXRjaCB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogVXBkYXRlUm9sZSkge1xuICAgIHJldHVybiBkaXNwYXRjaChuZXcgR2V0Um9sZUJ5SWQocGF5bG9hZC5pZCkpLnBpcGUoXG4gICAgICBzd2l0Y2hNYXAoKCkgPT4gdGhpcy5pZGVudGl0eVNlcnZpY2UudXBkYXRlUm9sZSh7IC4uLmdldFN0YXRlKCkuc2VsZWN0ZWRSb2xlLCAuLi5wYXlsb2FkIH0pKSxcbiAgICAgIHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgR2V0Um9sZXMoKSkpLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKEdldFVzZXJzKVxuICBnZXRVc2Vycyh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFVzZXJzKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmdldFVzZXJzKHBheWxvYWQpLnBpcGUoXG4gICAgICB0YXAodXNlcnMgPT5cbiAgICAgICAgcGF0Y2hTdGF0ZSh7XG4gICAgICAgICAgdXNlcnMsXG4gICAgICAgIH0pLFxuICAgICAgKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihHZXRVc2VyQnlJZClcbiAgZ2V0VXNlcih7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFVzZXJCeUlkKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmdldFVzZXJCeUlkKHBheWxvYWQpLnBpcGUoXG4gICAgICB0YXAoc2VsZWN0ZWRVc2VyID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHNlbGVjdGVkVXNlcixcbiAgICAgICAgfSksXG4gICAgICApLFxuICAgICk7XG4gIH1cblxuICBAQWN0aW9uKERlbGV0ZVVzZXIpXG4gIGRlbGV0ZVVzZXIoeyBkaXNwYXRjaCB9OiBTdGF0ZUNvbnRleHQ8SWRlbnRpdHkuU3RhdGU+LCB7IHBheWxvYWQgfTogR2V0VXNlckJ5SWQpIHtcbiAgICByZXR1cm4gdGhpcy5pZGVudGl0eVNlcnZpY2UuZGVsZXRlVXNlcihwYXlsb2FkKS5waXBlKHN3aXRjaE1hcCgoKSA9PiBkaXNwYXRjaChuZXcgR2V0VXNlcnMoKSkpKTtcbiAgfVxuXG4gIEBBY3Rpb24oQ3JlYXRlVXNlcilcbiAgYWRkVXNlcih7IGRpc3BhdGNoIH06IFN0YXRlQ29udGV4dDxJZGVudGl0eS5TdGF0ZT4sIHsgcGF5bG9hZCB9OiBDcmVhdGVVc2VyKSB7XG4gICAgcmV0dXJuIHRoaXMuaWRlbnRpdHlTZXJ2aWNlLmNyZWF0ZVVzZXIocGF5bG9hZCkucGlwZShzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IEdldFVzZXJzKCkpKSk7XG4gIH1cblxuICBAQWN0aW9uKFVwZGF0ZVVzZXIpXG4gIHVwZGF0ZVVzZXIoeyBnZXRTdGF0ZSwgZGlzcGF0Y2ggfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IFVwZGF0ZVVzZXIpIHtcbiAgICByZXR1cm4gZGlzcGF0Y2gobmV3IEdldFVzZXJCeUlkKHBheWxvYWQuaWQpKS5waXBlKFxuICAgICAgc3dpdGNoTWFwKCgpID0+IHRoaXMuaWRlbnRpdHlTZXJ2aWNlLnVwZGF0ZVVzZXIoeyAuLi5nZXRTdGF0ZSgpLnNlbGVjdGVkVXNlciwgLi4ucGF5bG9hZCB9KSksXG4gICAgICBzd2l0Y2hNYXAoKCkgPT4gZGlzcGF0Y2gobmV3IEdldFVzZXJzKCkpKSxcbiAgICApO1xuICB9XG5cbiAgQEFjdGlvbihHZXRVc2VyUm9sZXMpXG4gIGdldFVzZXJSb2xlcyh7IHBhdGNoU3RhdGUgfTogU3RhdGVDb250ZXh0PElkZW50aXR5LlN0YXRlPiwgeyBwYXlsb2FkIH06IEdldFVzZXJSb2xlcykge1xuICAgIHJldHVybiB0aGlzLmlkZW50aXR5U2VydmljZS5nZXRVc2VyUm9sZXMocGF5bG9hZCkucGlwZShcbiAgICAgIHBsdWNrKCdpdGVtcycpLFxuICAgICAgdGFwKHNlbGVjdGVkVXNlclJvbGVzID0+XG4gICAgICAgIHBhdGNoU3RhdGUoe1xuICAgICAgICAgIHNlbGVjdGVkVXNlclJvbGVzLFxuICAgICAgICB9KSxcbiAgICAgICksXG4gICAgKTtcbiAgfVxufVxuIl19
