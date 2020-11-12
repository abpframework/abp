import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext } from '@ngxs/store';
import { tap } from 'rxjs/operators';
import { GetPermissions, UpdatePermissions } from '../actions/permission-management.actions';
import { PermissionManagement } from '../models/permission-management';
import { ProviderInfoDto } from '../proxy/models';
import { PermissionsService } from '../proxy/permissions.service';

@State<PermissionManagement.State>({
  name: 'PermissionManagementState',
  defaults: { permissionRes: {} } as PermissionManagement.State,
})
@Injectable()
export class PermissionManagementState {
  @Selector()
  static getPermissionGroups({ permissionRes }: PermissionManagement.State) {
    return permissionRes.groups || [];
  }

  @Selector()
  static getEntityDisplayName({ permissionRes }: PermissionManagement.State): string {
    return permissionRes.entityDisplayName;
  }

  constructor(private service: PermissionsService) {}

  @Action(GetPermissions)
  permissionManagementGet(
    { patchState }: StateContext<PermissionManagement.State>,
    { payload: { providerKey, providerName } = {} as ProviderInfoDto }: GetPermissions,
  ) {
    return this.service.get(providerName, providerKey).pipe(
      tap(permissionResponse =>
        patchState({
          permissionRes: permissionResponse,
        }),
      ),
    );
  }

  @Action(UpdatePermissions)
  permissionManagementUpdate(_, { payload }: UpdatePermissions) {
    return this.service.update(payload.providerName, payload.providerKey, {
      permissions: payload.permissions,
    });
  }
}
