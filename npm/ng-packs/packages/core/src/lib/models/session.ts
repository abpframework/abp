import { ApplicationConfiguration } from './application-configuration';

export namespace Session {
  export interface State {
    language: string;
    tenant: ApplicationConfiguration.CurrentTenant;
    sessionDetail: SessionDetail;
  }

  export interface SessionDetail {
    openedTabCount: number;
    lastExitTime: number;
    remember: boolean;
  }
}
