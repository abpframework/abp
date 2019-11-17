import { StateContext } from '@ngxs/store';
import { GetPermissions, UpdatePermissions } from '../actions/permission-management.actions';
import { PermissionManagement } from '../models/permission-management';
import { PermissionManagementService } from '../services/permission-management.service';
export declare class PermissionManagementState {
    private permissionManagementService;
    static getPermissionGroups({ permissionRes }: PermissionManagement.State): PermissionManagement.Group[];
    static getEntityDisplayName({ permissionRes }: PermissionManagement.State): string;
    constructor(permissionManagementService: PermissionManagementService);
    permissionManagementGet({ patchState }: StateContext<PermissionManagement.State>, { payload }: GetPermissions): import("rxjs").Observable<PermissionManagement.Response>;
    permissionManagementUpdate(_: any, { payload }: UpdatePermissions): import("rxjs").Observable<null>;
}
