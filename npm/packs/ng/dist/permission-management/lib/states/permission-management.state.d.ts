import { StateContext } from '@ngxs/store';
import { PermissionManagementGetPermissions, PermissionManagementUpdatePermissions } from '../actions/permission-management.actions';
import { PermissionManagement } from '../models/permission-management';
import { PermissionManagementService } from '../services/permission-management.service';
export declare class PermissionManagementState {
    private permissionManagementService;
    static getPermissionGroups({ permissionRes }: PermissionManagement.State): PermissionManagement.Group[];
    static getEntitiyDisplayName({ permissionRes }: PermissionManagement.State): string;
    constructor(permissionManagementService: PermissionManagementService);
    permissionManagementGet({ patchState }: StateContext<PermissionManagement.State>, { payload }: PermissionManagementGetPermissions): import("rxjs").Observable<PermissionManagement.Response>;
    permissionManagementUpdate(_: any, { payload }: PermissionManagementUpdatePermissions): import("rxjs").Observable<null>;
}
