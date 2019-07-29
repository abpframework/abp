import { Action, Selector, State, StateContext } from '@ngxs/store';
import { switchMap, tap } from 'rxjs/operators';
import {
  TenantManagementAdd,
  TenantManagementDelete,
  TenantManagementGet,
  TenantManagementGetById,
  TenantManagementUpdate,
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

  constructor(private tenantManagementService: TenantManagementService) {}

  @Action(TenantManagementGet)
  get({ patchState }: StateContext<TenantManagement.State>) {
    return this.tenantManagementService.get().pipe(
      tap(result =>
        patchState({
          result,
        }),
      ),
    );
  }

  @Action(TenantManagementGetById)
  getById({ patchState }: StateContext<TenantManagement.State>, { payload }: TenantManagementGetById) {
    return this.tenantManagementService.getById(payload).pipe(
      tap(selectedItem =>
        patchState({
          selectedItem,
        }),
      ),
    );
  }

  @Action(TenantManagementDelete)
  delete({ dispatch }: StateContext<TenantManagement.State>, { payload }: TenantManagementDelete) {
    return this.tenantManagementService.delete(payload).pipe(switchMap(() => dispatch(new TenantManagementGet())));
  }

  @Action(TenantManagementAdd)
  add({ dispatch }: StateContext<TenantManagement.State>, { payload }: TenantManagementAdd) {
    return this.tenantManagementService.add(payload).pipe(switchMap(() => dispatch(new TenantManagementGet())));
  }

  @Action(TenantManagementUpdate)
  update({ dispatch, getState }: StateContext<TenantManagement.State>, { payload }: TenantManagementUpdate) {
    return dispatch(new TenantManagementGetById(payload.id)).pipe(
      switchMap(() => this.tenantManagementService.update({ ...getState().selectedItem, ...payload })),
      switchMap(() => dispatch(new TenantManagementGet())),
    );
  }
}
