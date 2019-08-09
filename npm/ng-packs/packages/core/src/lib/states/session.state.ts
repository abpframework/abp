import { Action, Selector, State, StateContext } from '@ngxs/store';
import { SetLanguage, SetTenant } from '../actions/session.actions';
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

  @Action(SetLanguage)
  setLanguage({ patchState }: StateContext<Session.State>, { payload }: SetLanguage) {
    patchState({
      language: payload,
    });
  }

  @Action(SetTenant)
  setTenantId({ patchState }: StateContext<Session.State>, { payload }: SetTenant) {
    patchState({
      tenant: payload,
    });
  }
}
