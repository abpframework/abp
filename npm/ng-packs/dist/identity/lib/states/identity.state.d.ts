import { StateContext } from '@ngxs/store';
import { IdentityAddRole, IdentityAddUser, IdentityGetRoleById, IdentityGetUserById, IdentityGetUsers, IdentityUpdateRole, IdentityUpdateUser, IdentityGetUserRoles } from '../actions/identity.actions';
import { Identity } from '../models/identity';
import { IdentityService } from '../services/identity.service';
export declare class IdentityState {
    private identityService;
    static getRoles({ roles }: Identity.State): Identity.RoleItem[];
    static getRolesTotalCount({ roles }: Identity.State): number;
    static getUsers({ users }: Identity.State): Identity.UserItem[];
    static getUsersTotalCount({ users }: Identity.State): number;
    constructor(identityService: IdentityService);
    getRoles({ patchState }: StateContext<Identity.State>): import("rxjs").Observable<import("@abp/ng.core").ABP.PagedResponse<Identity.RoleItem>>;
    getRole({ patchState }: StateContext<Identity.State>, { payload }: IdentityGetRoleById): import("rxjs").Observable<Identity.RoleItem>;
    deleteRole({ dispatch }: StateContext<Identity.State>, { payload }: IdentityGetRoleById): import("rxjs").Observable<void>;
    addRole({ dispatch }: StateContext<Identity.State>, { payload }: IdentityAddRole): import("rxjs").Observable<void>;
    updateRole({ getState, dispatch }: StateContext<Identity.State>, { payload }: IdentityUpdateRole): import("rxjs").Observable<void>;
    getUsers({ patchState }: StateContext<Identity.State>, { payload }: IdentityGetUsers): import("rxjs").Observable<import("@abp/ng.core").ABP.PagedResponse<Identity.UserItem>>;
    getUser({ patchState }: StateContext<Identity.State>, { payload }: IdentityGetUserById): import("rxjs").Observable<Identity.UserItem>;
    deleteUser({ dispatch }: StateContext<Identity.State>, { payload }: IdentityGetUserById): import("rxjs").Observable<void>;
    addUser({ dispatch }: StateContext<Identity.State>, { payload }: IdentityAddUser): import("rxjs").Observable<void>;
    updateUser({ getState, dispatch }: StateContext<Identity.State>, { payload }: IdentityUpdateUser): import("rxjs").Observable<void>;
    getUserRoles({ patchState }: StateContext<Identity.State>, { payload }: IdentityGetUserRoles): import("rxjs").Observable<Identity.RoleItem[]>;
}
