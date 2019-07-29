import { ABP } from '@abp/ng.core';

export namespace TenantManagement {
  export interface State {
    result: Response;
    selectedItem: Item;
  }

  export type Response = ABP.PagedResponse<Item>;

  export interface Item {
    id: string;
    name: string;
  }

  export interface AddRequest {
    name: string;
  }

  export interface UpdateRequest extends AddRequest {
    id: string;
  }

  export interface DefaultConnectionStringRequest {
    id: string;
    defaultConnectionString: string;
  }
}
