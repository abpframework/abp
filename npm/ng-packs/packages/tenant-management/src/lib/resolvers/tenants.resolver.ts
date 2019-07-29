import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Store } from '@ngxs/store';
import { TenantManagementGet } from '../actions/tenant-management.actions';
import { TenantManagement } from '../models/tenant-management';
import { TenantManagementState } from '../states/tenant-management.state';

@Injectable()
export class TenantsResolver implements Resolve<TenantManagement.State> {
  constructor(private store: Store) {}

  resolve() {
    const data = this.store.selectSnapshot(TenantManagementState.get);
    return data && data.length
     ? null 
     : this.store.dispatch(new TenantManagementGet());
  }
}
