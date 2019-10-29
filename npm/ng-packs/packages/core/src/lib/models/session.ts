import { ABP } from '../models';

export namespace Session {
  export interface State {
    language: string;
    tenant: ABP.BasicItem;
  }
}
