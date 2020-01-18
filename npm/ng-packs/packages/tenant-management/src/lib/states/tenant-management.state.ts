import { ABP } from '@abp/ng.core';
import { Action, Selector, State, StateContext } from '@ngxs/store';
import { tap } from 'rxjs/operators';
import {
  CreateTenant,
  DeleteTenant,
  GetTenantById,
  GetTenants,
  UpdateTenant,
} from '../actions/tenant-management.actions';
import { TenantManagement } from '../models/tenant-management';
import { TenantManagementService } from '../services/tenant-management.service';

@State<TenantManagement.State>({
  name: 'TenantManagementState',
  defaults: { result: {}, selectedItem: {} } as TenantManagement.State,
})
export class TenantManagementState {
  @Selector()
  static get({ result }: TenantManagement.State): ABP.BasicItem[] {
    return result.items || [];
  }

  @Selector()
  static getTenantsTotalCount({ result }: TenantManagement.State): number {
    return result.totalCount;
  }

  constructor(private tenantManagementService: TenantManagementService) {}

  @Action(GetTenants)
  get({ patchState }: StateContext<TenantManagement.State>, { payload }: GetTenants) {
    return this.tenantManagementService.getTenant(payload).pipe(
      tap(result =>
        patchState({
          result,
        }),
      ),
    );
  }

  @Action(GetTenantById)
  getById({ patchState }: StateContext<TenantManagement.State>, { payload }: GetTenantById) {
    return this.tenantManagementService.getTenantById(payload).pipe(
      tap(selectedItem =>
        patchState({
          selectedItem,
        }),
      ),
    );
  }

  @Action(DeleteTenant)
  delete(_, { payload }: DeleteTenant) {
    return this.tenantManagementService.deleteTenant(payload);
  }

  @Action(CreateTenant)
  add(_, { payload }: CreateTenant) {
    return this.tenantManagementService.createTenant(payload);
  }

  @Action(UpdateTenant)
  update({ getState }: StateContext<TenantManagement.State>, { payload }: UpdateTenant) {
    return this.tenantManagementService.updateTenant({ ...getState().selectedItem, ...payload });
  }
}
