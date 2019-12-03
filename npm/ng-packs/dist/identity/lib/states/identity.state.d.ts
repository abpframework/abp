import { StateContext } from '@ngxs/store';
import { CreateRole, CreateUser, GetRoleById, GetRoles, GetUserById, GetUsers, UpdateRole, UpdateUser, GetUserRoles } from '../actions/identity.actions';
import { Identity } from '../models/identity';
import { IdentityService } from '../services/identity.service';
export declare class IdentityState {
    private identityService;
    static getRoles({ roles }: Identity.State): Identity.RoleItem[];
    static getRolesTotalCount({ roles }: Identity.State): number;
    static getUsers({ users }: Identity.State): Identity.UserItem[];
    static getUsersTotalCount({ users }: Identity.State): number;
    constructor(identityService: IdentityService);
    getRoles({ patchState }: StateContext<Identity.State>, { payload }: GetRoles): import("rxjs").Observable<import("@abp/ng.core").ABP.PagedResponse<Identity.RoleItem>>;
    getRole({ patchState }: StateContext<Identity.State>, { payload }: GetRoleById): import("rxjs").Observable<Identity.RoleItem>;
    deleteRole(_: any, { payload }: GetRoleById): import("rxjs").Observable<Identity.RoleItem>;
    addRole(_: any, { payload }: CreateRole): import("rxjs").Observable<Identity.RoleItem>;
    updateRole({ getState }: StateContext<Identity.State>, { payload }: UpdateRole): import("rxjs").Observable<Identity.RoleItem>;
    getUsers({ patchState }: StateContext<Identity.State>, { payload }: GetUsers): import("rxjs").Observable<import("@abp/ng.core").ABP.PagedResponse<Identity.UserItem>>;
    getUser({ patchState }: StateContext<Identity.State>, { payload }: GetUserById): import("rxjs").Observable<Identity.UserItem>;
    deleteUser(_: any, { payload }: GetUserById): import("rxjs").Observable<null>;
    addUser(_: any, { payload }: CreateUser): import("rxjs").Observable<Identity.UserItem>;
    updateUser({ getState }: StateContext<Identity.State>, { payload }: UpdateUser): import("rxjs").Observable<Identity.UserItem>;
    getUserRoles({ patchState }: StateContext<Identity.State>, { payload }: GetUserRoles): import("rxjs").Observable<Identity.RoleItem[]>;
}
