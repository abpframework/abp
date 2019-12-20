import { ABP } from '@abp/ng.core';
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
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
import { IdentityState } from '../states/identity.state';

@Injectable({
  providedIn: 'root',
})
export class IdentityStateService {
  constructor(private store: Store) {}

  getRoles() {
    return this.store.selectSnapshot(IdentityState.getRoles);
  }
  getRolesTotalCount() {
    return this.store.selectSnapshot(IdentityState.getRolesTotalCount);
  }
  getUsers() {
    return this.store.selectSnapshot(IdentityState.getUsers);
  }
  getUsersTotalCount() {
    return this.store.selectSnapshot(IdentityState.getUsersTotalCount);
  }

  fetchRoles(payload?: ABP.PageQueryParams) {
    return this.store.dispatch(new GetRoles(payload));
  }

  fetchRole(payload: string) {
    return this.store.dispatch(new GetRoleById(payload));
  }

  deleteRole(payload: string) {
    return this.store.dispatch(new DeleteRole(payload));
  }

  createRole(payload: Identity.RoleSaveRequest) {
    return this.store.dispatch(new CreateRole(payload));
  }

  updateRole(payload: Identity.RoleItem) {
    return this.store.dispatch(new UpdateRole(payload));
  }

  fetchUsers(payload?: ABP.PageQueryParams) {
    return this.store.dispatch(new GetUsers(payload));
  }

  fetchUser(payload: string) {
    return this.store.dispatch(new GetUserById(payload));
  }

  deleteUser(payload: string) {
    return this.store.dispatch(new DeleteUser(payload));
  }

  createUser(payload: Identity.UserSaveRequest) {
    return this.store.dispatch(new CreateUser(payload));
  }

  updateUser(payload: Identity.UserItem) {
    return this.store.dispatch(new UpdateUser(payload));
  }

  getUserRoles(payload: string) {
    return this.store.dispatch(new GetUserRoles(payload));
  }
}
