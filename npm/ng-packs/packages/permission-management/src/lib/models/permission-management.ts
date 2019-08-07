export namespace PermissionManagement {
  export interface State {
    permissionRes: Response;
  }

  export interface Response {
    entityDisplayName: string;
    groups: Group[];
  }

  export interface Group {
    name: string;
    displayName: string;
    permissions: Permission[];
  }

  export interface MinimumPermission {
    name: string;
    isGranted: boolean;
  }

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

  export interface UpdateRequest {
    permissions: MinimumPermission[];
  }
}
