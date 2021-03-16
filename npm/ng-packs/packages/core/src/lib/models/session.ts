import { CurrentTenantDto } from '../proxy/volo/abp/asp-net-core/mvc/multi-tenancy/models';

export namespace Session {
  export interface State {
    language: string;
    tenant: CurrentTenantDto;
    /**
     *
     * @deprecated To be deleted in v5.0
     */
    sessionDetail: SessionDetail;
  }

  /**
   *
   * @deprecated To be deleted in v5.0
   */
  export interface SessionDetail {
    openedTabCount: number;
    lastExitTime: number;
    remember: boolean;
  }
}
