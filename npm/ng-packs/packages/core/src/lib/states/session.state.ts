import { Action, Selector, State, StateContext } from '@ngxs/store';
import { SetLanguage, SetTenant } from '../actions/session.actions';
import { ABP, Session } from '../models';
import { GetAppConfiguration } from '../actions/config.actions';
import { LocalizationService } from '../services/localization.service';
import { from, combineLatest } from 'rxjs';

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

  constructor(private localizationService: LocalizationService) {}

  @Action(SetLanguage)
  setLanguage({ patchState, dispatch }: StateContext<Session.State>, { payload }: SetLanguage) {
    patchState({
      language: payload,
    });

    return combineLatest([dispatch(new GetAppConfiguration()), from(this.localizationService.registerLocale(payload))]);
  }

  @Action(SetTenant)
  setTenantId({ patchState }: StateContext<Session.State>, { payload }: SetTenant) {
    patchState({
      tenant: payload,
    });
  }
}
