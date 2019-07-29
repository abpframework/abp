import { State, Action, StateContext, Selector } from '@ngxs/store';
import {
  PermissionManagementGetPermissions,
  PermissionManagementUpdatePermissions,
} from '../actions/permission-management.actions';
import { PermissionManagement } from '../models/permission-management';
import { PermissionManagementService } from '../services/permission-management.service';
import { tap } from 'rxjs/operators';

@State<PermissionManagement.State>({
  name: 'PermissionManagementState',
  defaults: { permissionRes: {} } as PermissionManagement.State,
})
export class PermissionManagementState {
  @Selector()
  static getPermissionGroups({ permissionRes }: PermissionManagement.State) {
    return permissionRes.groups || [];
  }

  @Selector()
  static getEntitiyDisplayName({ permissionRes }: PermissionManagement.State): string {
    return permissionRes.entityDisplayName;
  }

  constructor(private permissionManagementService: PermissionManagementService) {}

  @Action(PermissionManagementGetPermissions)
  permissionManagementGet(
    { patchState }: StateContext<PermissionManagement.State>,
    { payload }: PermissionManagementGetPermissions,
  ) {
    return this.permissionManagementService.getPermissions(payload).pipe(
      tap(permissionResponse =>
        patchState({
          permissionRes: permissionResponse,
        }),
      ),
    );
  }

  @Action(PermissionManagementUpdatePermissions)
  permissionManagementUpdate(_, { payload }: PermissionManagementUpdatePermissions) {
    return this.permissionManagementService.updatePermissions(payload);
  }
}
