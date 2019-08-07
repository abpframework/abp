import { Action, Selector, State, StateContext } from '@ngxs/store';
import { SessionSetLanguage, SessionSetTenant } from '../actions/session.actions';
import { ABP, Session } from '../models';

@State<Session.State>({
  name: 'SessionState',
  defaults: {} as Session.State,
})
export class SessionState {
  @Selector()
  static getLanguage({ language }: Session.State): string {
    return language;
  }

  @Selector()
  static getTenant({ tenant }: Session.State): ABP.BasicItem {
    return tenant;
  }

  constructor() {}

  @Action(SessionSetLanguage)
  sessionSetLanguage({ patchState }: StateContext<Session.State>, { payload }: SessionSetLanguage) {
    patchState({
      language: payload,
    });
  }

  @Action(SessionSetTenant)
  sessionSetTenantId({ patchState }: StateContext<Session.State>, { payload }: SessionSetTenant) {
    patchState({
      tenant: payload,
    });
  }
}
