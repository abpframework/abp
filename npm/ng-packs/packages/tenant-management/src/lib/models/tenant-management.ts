import { ABP, PagedResultDto } from '@abp/ng.core';
import { TenantDto } from '../proxy/models';

export namespace TenantManagement {
  export interface State {
    result: PagedResultDto<TenantDto>;
    selectedItem: TenantDto;
  }

  /**
   * @deprecated To be deleted in v4.0
   */
  export type Response = ABP.PagedResponse<Item>;

  /**
   * @deprecated To be deleted in v4.0
   */
  export interface Item {
    id: string;
    name: string;
  }

  /**
   * @deprecated To be deleted in v4.0
   */
  export interface AddRequest {
    adminEmailAddress: string;
    adminPassword: string;
    name: string;
  }

  /**
   * @deprecated To be deleted in v4.0
   */
  export interface UpdateRequest {
    id: string;
    name: string;
  }

  /**
   * @deprecated To be deleted in v4.0
   */
  export interface DefaultConnectionStringRequest {
    id: string;
    defaultConnectionString: string;
  }
}
