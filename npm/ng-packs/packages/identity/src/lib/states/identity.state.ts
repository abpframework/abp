import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext } from '@ngxs/store';
import { pluck, tap } from 'rxjs/operators';
import {
  CreateRole,
  CreateUser,
  DeleteRole,
  DeleteUser,
  GetRoleById,
  GetRoles,
  GetUserById,
  GetUserRoles,
  GetUsers,
  UpdateRole,
  UpdateUser,
} from '../actions/identity.actions';
import { Identity } from '../models/identity';
import { IdentityRoleService } from '../proxy/identity/identity-role.service';
import { IdentityUserService } from '../proxy/identity/identity-user.service';
import { IdentityRoleDto, IdentityUserDto } from '../proxy/identity/models';

@State<Identity.State>({
  name: 'IdentityState',
  defaults: { roles: {}, selectedRole: {}, users: {}, selectedUser: {} } as Identity.State,
})
@Injectable()
export class IdentityState {
  @Selector()
  static getRoles({ roles }: Identity.State): IdentityRoleDto[] {
    return roles.items || [];
  }

  @Selector()
  static getRolesTotalCount({ roles }: Identity.State): number {
    return roles.totalCount || 0;
  }

  @Selector()
  static getUsers({ users }: Identity.State): IdentityUserDto[] {
    return users.items || [];
  }

  @Selector()
  static getUsersTotalCount({ users }: Identity.State): number {
    return users.totalCount || 0;
  }

  constructor(
    private identityUserService: IdentityUserService,
    private identityRoleService: IdentityRoleService,
  ) {}

  @Action(GetRoles)
  getRoles({ patchState }: StateContext<Identity.State>, { payload }: GetRoles) {
    return this.identityRoleService.getList(payload).pipe(
      tap(roles =>
        patchState({
          roles,
        }),
      ),
    );
  }

  @Action(GetRoleById)
  getRole({ patchState }: StateContext<Identity.State>, { payload }: GetRoleById) {
    return this.identityRoleService.get(payload).pipe(
      tap(selectedRole =>
        patchState({
          selectedRole,
        }),
      ),
    );
  }

  @Action(DeleteRole)
  deleteRole(_, { payload }: GetRoleById) {
    return this.identityRoleService.delete(payload);
  }

  @Action(CreateRole)
  addRole(_, { payload }: CreateRole) {
    return this.identityRoleService.create(payload);
  }

  @Action(UpdateRole)
  updateRole({ getState }: StateContext<Identity.State>, { payload }: UpdateRole) {
    return this.identityRoleService.update(payload.id, { ...getState().selectedRole, ...payload });
  }

  @Action(GetUsers)
  getUsers({ patchState }: StateContext<Identity.State>, { payload }: GetUsers) {
    return this.identityUserService.getList(payload).pipe(
      tap(users =>
        patchState({
          users,
        }),
      ),
    );
  }

  @Action(GetUserById)
  getUser({ patchState }: StateContext<Identity.State>, { payload }: GetUserById) {
    return this.identityUserService.get(payload).pipe(
      tap(selectedUser =>
        patchState({
          selectedUser,
        }),
      ),
    );
  }

  @Action(DeleteUser)
  deleteUser(_, { payload }: GetUserById) {
    return this.identityUserService.delete(payload);
  }

  @Action(CreateUser)
  addUser(_, { payload }: CreateUser) {
    return this.identityUserService.create(payload);
  }

  @Action(UpdateUser)
  updateUser({ getState }: StateContext<Identity.State>, { payload }: UpdateUser) {
    return this.identityUserService.update(payload.id, { ...getState().selectedUser, ...payload });
  }

  @Action(GetUserRoles)
  getUserRoles({ patchState }: StateContext<Identity.State>, { payload }: GetUserRoles) {
    return this.identityUserService.getRoles(payload).pipe(
      pluck('items'),
      tap(selectedUserRoles =>
        patchState({
          selectedUserRoles,
        }),
      ),
    );
  }
}
