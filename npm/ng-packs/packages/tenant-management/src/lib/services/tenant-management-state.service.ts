import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { TenantManagementState } from '../states/tenant-management.state';

@Injectable({
  providedIn: 'root',
})
export class TenantManagementStateService {
  constructor(private store: Store) {}

  get() {
    return this.store.selectSnapshot(TenantManagementState.get);
  }

  getTenantsTotalCount() {
    return this.store.selectSnapshot(TenantManagementState.getTenantsTotalCount);
  }
}
