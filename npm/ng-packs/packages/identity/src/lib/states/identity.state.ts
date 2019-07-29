import { Action, Selector, State, StateContext } from '@ngxs/store';
import { switchMap, tap, pluck } from 'rxjs/operators';
import {
  IdentityAddRole,
  IdentityAddUser,
  IdentityDeleteRole,
  IdentityDeleteUser,
  IdentityGetRoleById,
  IdentityGetRoles,
  IdentityGetUserById,
  IdentityGetUsers,
  IdentityUpdateRole,
  IdentityUpdateUser,
  IdentityGetUserRoles,
} from '../actions/identity.actions';
import { Identity } from '../models/identity';
import { IdentityService } from '../services/identity.service';

@State<Identity.State>({
  name: 'IdentityState',
  defaults: { roles: {}, selectedRole: {}, users: {}, selectedUser: {} } as Identity.State,
})
export class IdentityState {
  @Selector()
  static getRoles({ roles }: Identity.State): Identity.RoleItem[] {
    return roles.items;
  }

  @Selector()
  static getRolesTotalCount({ roles }: Identity.State): number {
    return roles.totalCount;
  }

  @Selector()
  static getUsers({ users }: Identity.State): Identity.UserItem[] {
    return users.items;
  }

  @Selector()
  static getUsersTotalCount({ users }: Identity.State): number {
    return users.totalCount;
  }

  constructor(private identityService: IdentityService) {}

  @Action(IdentityGetRoles)
  getRoles({ patchState }: StateContext<Identity.State>) {
    return this.identityService.getRoles().pipe(
      tap(roles =>
        patchState({
          roles,
        }),
      ),
    );
  }

  @Action(IdentityGetRoleById)
  getRole({ patchState }: StateContext<Identity.State>, { payload }: IdentityGetRoleById) {
    return this.identityService.getRoleById(payload).pipe(
      tap(selectedRole =>
        patchState({
          selectedRole,
        }),
      ),
    );
  }

  @Action(IdentityDeleteRole)
  deleteRole({ dispatch }: StateContext<Identity.State>, { payload }: IdentityGetRoleById) {
    return this.identityService.deleteRole(payload).pipe(switchMap(() => dispatch(new IdentityGetRoles())));
  }

  @Action(IdentityAddRole)
  addRole({ dispatch }: StateContext<Identity.State>, { payload }: IdentityAddRole) {
    return this.identityService.addRole(payload).pipe(switchMap(() => dispatch(new IdentityGetRoles())));
  }

  @Action(IdentityUpdateRole)
  updateRole({ getState, dispatch }: StateContext<Identity.State>, { payload }: IdentityUpdateRole) {
    return dispatch(new IdentityGetRoleById(payload.id)).pipe(
      switchMap(() => this.identityService.updateRole({ ...getState().selectedRole, ...payload })),
      switchMap(() => dispatch(new IdentityGetRoles())),
    );
  }

  @Action(IdentityGetUsers)
  getUsers({ patchState }: StateContext<Identity.State>, { payload }: IdentityGetUsers) {
    return this.identityService.getUsers(payload).pipe(
      tap(users =>
        patchState({
          users,
        }),
      ),
    );
  }

  @Action(IdentityGetUserById)
  getUser({ patchState }: StateContext<Identity.State>, { payload }: IdentityGetUserById) {
    return this.identityService.getUserById(payload).pipe(
      tap(selectedUser =>
        patchState({
          selectedUser,
        }),
      ),
    );
  }

  @Action(IdentityDeleteUser)
  deleteUser({ dispatch }: StateContext<Identity.State>, { payload }: IdentityGetUserById) {
    return this.identityService.deleteUser(payload).pipe(switchMap(() => dispatch(new IdentityGetUsers())));
  }

  @Action(IdentityAddUser)
  addUser({ dispatch }: StateContext<Identity.State>, { payload }: IdentityAddUser) {
    return this.identityService.addUser(payload).pipe(switchMap(() => dispatch(new IdentityGetUsers())));
  }

  @Action(IdentityUpdateUser)
  updateUser({ getState, dispatch }: StateContext<Identity.State>, { payload }: IdentityUpdateUser) {
    return dispatch(new IdentityGetUserById(payload.id)).pipe(
      switchMap(() => this.identityService.updateUser({ ...getState().selectedUser, ...payload })),
      switchMap(() => dispatch(new IdentityGetUsers())),
    );
  }

  @Action(IdentityGetUserRoles)
  getUserRoles({ patchState }: StateContext<Identity.State>, { payload }: IdentityGetUserRoles) {
    return this.identityService.getUserRoles(payload).pipe(
      pluck('items'),
      tap(selectedUserRoles =>
        patchState({
          selectedUserRoles,
        }),
      ),
    );
  }
}
