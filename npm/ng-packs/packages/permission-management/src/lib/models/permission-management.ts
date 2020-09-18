import { EventEmitter } from '@angular/core';

export namespace PermissionManagement {
  export interface State {
    permissionRes: Response;
  }

  /**
   * @deprecated To be deleted in v4.0.
   */
  export interface Response {
    entityDisplayName: string;
    groups: Group[];
  }

  /**
   * @deprecated To be deleted in v4.0.
   */
  export interface Group {
    name: string;
    displayName: string;
    permissions: Permission[];
  }

  /**
   * @deprecated To be deleted in v4.0.
   */
  export interface MinimumPermission {
    name: string;
    isGranted: boolean;
  }

  /**
   * @deprecated To be deleted in v4.0.
   */
  export interface Permission extends MinimumPermission {
    displayName: string;
    parentName: string;
    allowedProviders: string[];
    grantedProviders: GrantedProvider[];
  }

  export interface GrantedProvider {
    providerName: string;
    providerKey: string;
  }

  /**
   * @deprecated To be deleted in v4.0.
   */
  export interface UpdateRequest {
    permissions: MinimumPermission[];
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
