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

  dispatchGetRoles(...args: ConstructorParameters<typeof GetRoles>) {
    return this.store.dispatch(new GetRoles(...args));
  }

  dispatchGetRoleById(...args: ConstructorParameters<typeof GetRoleById>) {
    return this.store.dispatch(new GetRoleById(...args));
  }

  dispatchDeleteRole(...args: ConstructorParameters<typeof DeleteRole>) {
    return this.store.dispatch(new DeleteRole(...args));
  }

  dispatchCreateRole(...args: ConstructorParameters<typeof CreateRole>) {
    return this.store.dispatch(new CreateRole(...args));
  }

  dispatchUpdateRole(...args: ConstructorParameters<typeof UpdateRole>) {
    return this.store.dispatch(new UpdateRole(...args));
  }

  dispatchGetUsers(...args: ConstructorParameters<typeof GetUsers>) {
    return this.store.dispatch(new GetUsers(...args));
  }

  dispatchGetUserById(...args: ConstructorParameters<typeof GetUserById>) {
    return this.store.dispatch(new GetUserById(...args));
  }

  dispatchDeleteUser(...args: ConstructorParameters<typeof DeleteUser>) {
    return this.store.dispatch(new DeleteUser(...args));
  }

  dispatchCreateUser(...args: ConstructorParameters<typeof CreateUser>) {
    return this.store.dispatch(new CreateUser(...args));
  }

  dispatchUpdateUser(...args: ConstructorParameters<typeof UpdateUser>) {
    return this.store.dispatch(new UpdateUser(...args));
  }

  dispatchGetUserRoles(...args: ConstructorParameters<typeof GetUserRoles>) {
    return this.store.dispatch(new GetUserRoles(...args));
  }
}
