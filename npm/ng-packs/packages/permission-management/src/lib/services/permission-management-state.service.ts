import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { PermissionManagementState } from '../states/permission-management.state';
import { PermissionManagement } from '../models';
import { GetPermissions, UpdatePermissions } from '../actions';

@Injectable({
  providedIn: 'root',
})
export class PermissionManagementStateService {
  constructor(private store: Store) {}

  getPermissionGroups() {
    return this.store.selectSnapshot(PermissionManagementState.getPermissionGroups);
  }
  getEntityDisplayName() {
    return this.store.selectSnapshot(PermissionManagementState.getEntityDisplayName);
  }

  getPermissions(payload: PermissionManagement.GrantedProvider) {
    return this.store.dispatch(new GetPermissions(payload));
  }

  updatePermissions(
    payload: PermissionManagement.GrantedProvider & PermissionManagement.UpdateRequest,
  ) {
    return this.store.dispatch(new UpdatePermissions(payload));
  }
}
