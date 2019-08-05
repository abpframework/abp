import { Action, Selector, State, StateContext } from '@ngxs/store';
import { SessionSetLanguage, SessionSetTenantId } from '../actions/session.actions';
import { Session } from '../models/session';

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
  static getSelectedTenantId({ tenantId }: Session.State): string {
    return tenantId;
  }

  constructor() {}

  @Action(SessionSetLanguage)
  sessionSetLanguage({ patchState }: StateContext<Session.State>, { payload }: SessionSetLanguage) {
    patchState({
      language: payload,
    });
  }

  @Action(SessionSetTenantId)
  sessionSetTenantId({ patchState }: StateContext<Session.State>, { payload }: SessionSetTenantId) {
    patchState({
      tenantId: payload,
    });
  }
}
