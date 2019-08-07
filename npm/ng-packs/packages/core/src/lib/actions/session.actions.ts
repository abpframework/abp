import { ABP } from '../models';

export class SessionSetLanguage {
  static readonly type = '[Session] Set Language';
  constructor(public payload: string) {}
}
export class SessionSetTenant {
  static readonly type = '[Session] Set Tenant';
  constructor(public payload: ABP.BasicItem) {}
}
