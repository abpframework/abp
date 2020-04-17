import { ABP } from '../models';

export class SetLanguage {
  static readonly type = '[Session] Set Language';
  constructor(public payload: string) {}
}
export class SetTenant {
  static readonly type = '[Session] Set Tenant';
  constructor(public payload: ABP.BasicItem) {}
}
export class ModifyOpenedTabCount {
  static readonly type = '[Session] Modify Opened Tab Count';
  constructor(public operation: 'increase' | 'decrease') {}
}
export class SetRemember {
  static readonly type = '[Session] Set Remember';
  constructor(public payload: boolean) {}
}
