import { PagedResultDto } from '@abp/ng.core';
import { TenantDto } from '../proxy/models';

export namespace TenantManagement {
  export interface State {
    result: PagedResultDto<TenantDto>;
    selectedItem: TenantDto;
  }
}
