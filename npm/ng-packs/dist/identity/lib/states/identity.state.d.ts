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
    deleteRole({ dispatch }: StateContext<Identity.State>, { payload }: GetRoleById): import("rxjs").Observable<void>;
    addRole({ dispatch }: StateContext<Identity.State>, { payload }: CreateRole): import("rxjs").Observable<void>;
    updateRole({ getState, dispatch }: StateContext<Identity.State>, { payload }: UpdateRole): import("rxjs").Observable<void>;
    getUsers({ patchState }: StateContext<Identity.State>, { payload }: GetUsers): import("rxjs").Observable<import("@abp/ng.core").ABP.PagedResponse<Identity.UserItem>>;
    getUser({ patchState }: StateContext<Identity.State>, { payload }: GetUserById): import("rxjs").Observable<Identity.UserItem>;
    deleteUser({ dispatch }: StateContext<Identity.State>, { payload }: GetUserById): import("rxjs").Observable<void>;
    addUser({ dispatch }: StateContext<Identity.State>, { payload }: CreateUser): import("rxjs").Observable<void>;
    updateUser({ getState, dispatch }: StateContext<Identity.State>, { payload }: UpdateUser): import("rxjs").Observable<void>;
    getUserRoles({ patchState }: StateContext<Identity.State>, { payload }: GetUserRoles): import("rxjs").Observable<Identity.RoleItem[]>;
}
