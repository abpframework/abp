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

  dispatchGetRoles(payload?: ABP.PageQueryParams) {
    return this.store.dispatch(new GetRoles(payload));
  }

  dispatchGetRoleById(payload: string) {
    return this.store.dispatch(new GetRoleById(payload));
  }

  dispatchDeleteRole(payload: string) {
    return this.store.dispatch(new DeleteRole(payload));
  }

  dispatchCreateRole(payload: Identity.RoleSaveRequest) {
    return this.store.dispatch(new CreateRole(payload));
  }

  dispatchUpdateRole(payload: Identity.RoleItem) {
    return this.store.dispatch(new UpdateRole(payload));
  }

  dispatchGetUsers(payload?: ABP.PageQueryParams) {
    return this.store.dispatch(new GetUsers(payload));
  }

  dispatchGetUserById(payload: string) {
    return this.store.dispatch(new GetUserById(payload));
  }

  dispatchDeleteUser(payload: string) {
    return this.store.dispatch(new DeleteUser(payload));
  }

  dispatchCreateUser(payload: Identity.UserSaveRequest) {
    return this.store.dispatch(new CreateUser(payload));
  }

  dispatchUpdateUser(payload: Identity.UserSaveRequest & { id: string }) {
    return this.store.dispatch(new UpdateUser(payload));
  }

  dispatchGetUserRoles(payload: string) {
    return this.store.dispatch(new GetUserRoles(payload));
  }
}
