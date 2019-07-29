import { ABP } from '@abp/ng.core';
export declare namespace TenantManagement {
    interface State {
        result: Response;
        selectedItem: Item;
    }
    type Response = ABP.PagedResponse<Item>;
    interface Item {
        id: string;
        name: string;
    }
    interface AddRequest {
        name: string;
    }
    interface UpdateRequest extends AddRequest {
        id: string;
    }
    interface DefaultConnectionStringRequest {
        id: string;
        defaultConnectionString: string;
    }
}
