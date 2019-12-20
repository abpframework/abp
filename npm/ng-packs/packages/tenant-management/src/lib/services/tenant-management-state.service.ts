import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { TenantManagementState } from '../states/tenant-management.state';
import { ABP } from '@abp/ng.core';
import { GetTenants, GetTenantById, CreateTenant, UpdateTenant, DeleteTenant } from '../actions';
import { TenantManagement } from '../models';

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

  getTenants(payload?: ABP.PageQueryParams) {
    return this.store.dispatch(new GetTenants(payload));
  }

  getTenantById(payload: string) {
    return this.store.dispatch(new GetTenantById(payload));
  }

  createTenant(payload: TenantManagement.AddRequest) {
    return this.store.dispatch(new CreateTenant(payload));
  }

  updateTenant(payload: TenantManagement.UpdateRequest) {
    return this.store.dispatch(new UpdateTenant(payload));
  }

  deleteTenant(payload: string) {
    return this.store.dispatch(new DeleteTenant(payload));
  }
}
