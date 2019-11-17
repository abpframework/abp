import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { PermissionManagementState } from '../states/permission-management.state';

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
}
