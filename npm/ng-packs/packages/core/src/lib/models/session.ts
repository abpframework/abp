import { CurrentTenantDto } from '../proxy/volo/abp/asp-net-core/mvc/multi-tenancy/models';

export namespace Session {
  export interface State {
    language: string;
    tenant: CurrentTenantDto;
  }
}
