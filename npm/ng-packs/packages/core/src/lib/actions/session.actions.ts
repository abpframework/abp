import { ABP } from '../models';

export class SetLanguage {
  static readonly type = '[Session] Set Language';
  constructor(public payload: string) {}
}
export class SetTenant {
  static readonly type = '[Session] Set Tenant';
  constructor(public payload: ABP.BasicItem) {}
}
