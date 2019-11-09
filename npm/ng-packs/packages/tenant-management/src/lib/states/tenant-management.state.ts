import { Action, Selector, State, StateContext } from '@ngxs/store';
import { switchMap, tap } from 'rxjs/operators';
import {
  CreateTenant,
  DeleteTenant,
  GetTenants,
  GetTenantById,
  UpdateTenant,
} from '../actions/tenant-management.actions';
import { TenantManagement } from '../models/tenant-management';
import { TenantManagementService } from '../services/tenant-management.service';
import { ABP } from '@abp/ng.core';

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
  delete({ dispatch }: StateContext<TenantManagement.State>, { payload }: DeleteTenant) {
    return this.tenantManagementService.deleteTenant(payload).pipe(switchMap(() => dispatch(new GetTenants())));
  }

  @Action(CreateTenant)
  add({ dispatch }: StateContext<TenantManagement.State>, { payload }: CreateTenant) {
    return this.tenantManagementService.createTenant(payload).pipe(switchMap(() => dispatch(new GetTenants())));
  }

  @Action(UpdateTenant)
  update({ dispatch, getState }: StateContext<TenantManagement.State>, { payload }: UpdateTenant) {
    return this.tenantManagementService
      .updateTenant({ ...getState().selectedItem, ...payload })
      .pipe(switchMap(() => dispatch(new GetTenants())));
  }
}
