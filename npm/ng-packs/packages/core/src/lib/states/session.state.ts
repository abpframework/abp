import { State, Action, StateContext, Selector } from '@ngxs/store';
import { SessionSetLanguage } from '../actions/session.actions';
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

  constructor() {}

  @Action(SessionSetLanguage)
  sessionSetLanguage({ patchState }: StateContext<Session.State>, { payload }: SessionSetLanguage) {
    patchState({
      language: payload,
    });
  }
}
