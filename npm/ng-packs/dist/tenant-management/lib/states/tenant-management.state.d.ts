import { StateContext } from '@ngxs/store';
import { TenantManagementAdd, TenantManagementDelete, TenantManagementGetById, TenantManagementUpdate } from '../actions/tenant-management.actions';
import { TenantManagement } from '../models/tenant-management';
import { TenantManagementService } from '../services/tenant-management.service';
import { ABP } from '@abp/ng.core';
export declare class TenantManagementState {
    private tenantManagementService;
    static get({ result }: TenantManagement.State): ABP.BasicItem[];
    constructor(tenantManagementService: TenantManagementService);
    get({ patchState }: StateContext<TenantManagement.State>): import("rxjs").Observable<ABP.PagedResponse<TenantManagement.Item>>;
    getById({ patchState }: StateContext<TenantManagement.State>, { payload }: TenantManagementGetById): import("rxjs").Observable<ABP.BasicItem>;
    delete({ dispatch }: StateContext<TenantManagement.State>, { payload }: TenantManagementDelete): import("rxjs").Observable<void>;
    add({ dispatch }: StateContext<TenantManagement.State>, { payload }: TenantManagementAdd): import("rxjs").Observable<void>;
    update({ dispatch, getState }: StateContext<TenantManagement.State>, { payload }: TenantManagementUpdate): import("rxjs").Observable<void>;
}
