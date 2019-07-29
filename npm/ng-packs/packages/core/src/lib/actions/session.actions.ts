export class SessionSetLanguage {
  static readonly type = '[Session] Set Language';
  constructor(public payload: string) {}
}
