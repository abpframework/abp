import { StateContext } from '@ngxs/store';
import { CreateTenant, DeleteTenant, GetTenants, GetTenantById, UpdateTenant } from '../actions/tenant-management.actions';
import { TenantManagement } from '../models/tenant-management';
import { TenantManagementService } from '../services/tenant-management.service';
import { ABP } from '@abp/ng.core';
export declare class TenantManagementState {
    private tenantManagementService;
    static get({ result }: TenantManagement.State): ABP.BasicItem[];
    static getTenantsTotalCount({ result }: TenantManagement.State): number;
    constructor(tenantManagementService: TenantManagementService);
    get({ patchState }: StateContext<TenantManagement.State>, { payload }: GetTenants): import("rxjs").Observable<ABP.PagedResponse<TenantManagement.Item>>;
    getById({ patchState }: StateContext<TenantManagement.State>, { payload }: GetTenantById): import("rxjs").Observable<ABP.BasicItem>;
    delete(_: any, { payload }: DeleteTenant): import("rxjs").Observable<null>;
    add(_: any, { payload }: CreateTenant): import("rxjs").Observable<ABP.BasicItem>;
    update({ getState }: StateContext<TenantManagement.State>, { payload }: UpdateTenant): import("rxjs").Observable<ABP.BasicItem>;
}
