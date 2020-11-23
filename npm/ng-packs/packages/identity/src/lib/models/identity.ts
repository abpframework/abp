import { PagedResultDto } from '@abp/ng.core';
import { IdentityRoleDto, IdentityUserDto } from '../proxy/identity/models';

export namespace Identity {
  export interface State {
    roles: PagedResultDto<IdentityRoleDto>;
    users: PagedResultDto<IdentityUserDto>;
    selectedRole: IdentityRoleDto;
    selectedUser: IdentityUserDto;
    selectedUserRoles: IdentityRoleDto[];
  }
}
