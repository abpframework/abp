import { GetPermissionListResultDto } from '@abp/ng.permission-management/proxy';
import { EventEmitter } from '@angular/core';

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
