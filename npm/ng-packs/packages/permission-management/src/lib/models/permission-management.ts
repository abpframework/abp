import { EventEmitter } from '@angular/core';
import { GetPermissionListResultDto } from '../proxy/models';

export namespace PermissionManagement {
  export interface State {
    permissionRes: GetPermissionListResultDto;
  }

  export interface PermissionManagementComponentInputs {
    visible: boolean;
    readonly providerName: string;
    readonly providerKey: string;
    readonly hideBadges: boolean;
  }

  export interface PermissionManagementComponentOutputs {
    readonly visibleChange: EventEmitter<boolean>;
  }
}
