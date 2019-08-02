export class SessionSetLanguage {
  static readonly type = '[Session] Set Language';
  constructor(public payload: string) {}
}
export class SessionSetTenantId {
  static readonly type = '[Session] Set Tenant Id';
  constructor(public payload: string) {}
}
