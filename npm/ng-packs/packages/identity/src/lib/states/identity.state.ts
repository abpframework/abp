import { Action, Selector, State, StateContext } from '@ngxs/store';
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
import { Identity } from '../models/identity';
import { IdentityService } from '../services/identity.service';
import { Injectable } from '@angular/core';

@State<Identity.State>({
  name: 'IdentityState',
  defaults: { roles: {}, selectedRole: {}, users: {}, selectedUser: {} } as Identity.State,
})
@Injectable()
export class IdentityState {
  @Selector()
  static getRoles({ roles }: Identity.State): Identity.RoleItem[] {
    return roles.items || [];
  }

  @Selector()
  static getRolesTotalCount({ roles }: Identity.State): number {
    return roles.totalCount || 0;
  }

  @Selector()
  static getUsers({ users }: Identity.State): Identity.UserItem[] {
    return users.items || [];
  }

  @Selector()
  static getUsersTotalCount({ users }: Identity.State): number {
    return users.totalCount || 0;
  }

  constructor(private identityService: IdentityService) {}

  @Action(GetRoles)
  getRoles({ patchState }: StateContext<Identity.State>, { payload }: GetRoles) {
    return this.identityService.getRoles(payload).pipe(
      tap(roles =>
        patchState({
          roles,
        }),
      ),
    );
  }

  @Action(GetRoleById)
  getRole({ patchState }: StateContext<Identity.State>, { payload }: GetRoleById) {
    return this.identityService.getRoleById(payload).pipe(
      tap(selectedRole =>
        patchState({
          selectedRole,
        }),
      ),
    );
  }

  @Action(DeleteRole)
  deleteRole(_, { payload }: GetRoleById) {
    return this.identityService.deleteRole(payload);
  }

  @Action(CreateRole)
  addRole(_, { payload }: CreateRole) {
    return this.identityService.createRole(payload);
  }

  @Action(UpdateRole)
  updateRole({ getState }: StateContext<Identity.State>, { payload }: UpdateRole) {
    return this.identityService.updateRole({ ...getState().selectedRole, ...payload });
  }

  @Action(GetUsers)
  getUsers({ patchState }: StateContext<Identity.State>, { payload }: GetUsers) {
    return this.identityService.getUsers(payload).pipe(
      tap(users =>
        patchState({
          users,
        }),
      ),
    );
  }

  @Action(GetUserById)
  getUser({ patchState }: StateContext<Identity.State>, { payload }: GetUserById) {
    return this.identityService.getUserById(payload).pipe(
      tap(selectedUser =>
        patchState({
          selectedUser,
        }),
      ),
    );
  }

  @Action(DeleteUser)
  deleteUser(_, { payload }: GetUserById) {
    return this.identityService.deleteUser(payload);
  }

  @Action(CreateUser)
  addUser(_, { payload }: CreateUser) {
    return this.identityService.createUser(payload);
  }

  @Action(UpdateUser)
  updateUser({ getState }: StateContext<Identity.State>, { payload }: UpdateUser) {
    return this.identityService.updateUser({ ...getState().selectedUser, ...payload });
  }

  @Action(GetUserRoles)
  getUserRoles({ patchState }: StateContext<Identity.State>, { payload }: GetUserRoles) {
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
