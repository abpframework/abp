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

  dispatchGetTenants(...args: ConstructorParameters<typeof GetTenants>) {
    return this.store.dispatch(new GetTenants(...args));
  }

  dispatchGetTenantById(...args: ConstructorParameters<typeof GetTenantById>) {
    return this.store.dispatch(new GetTenantById(...args));
  }

  dispatchCreateTenant(...args: ConstructorParameters<typeof CreateTenant>) {
    return this.store.dispatch(new CreateTenant(...args));
  }

  dispatchUpdateTenant(...args: ConstructorParameters<typeof UpdateTenant>) {
    return this.store.dispatch(new UpdateTenant(...args));
  }

  dispatchDeleteTenant(...args: ConstructorParameters<typeof DeleteTenant>) {
    return this.store.dispatch(new DeleteTenant(...args));
  }
}
