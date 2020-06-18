import { Injectable } from '@angular/core';
import {
  Action,
  Actions,
  ofActionSuccessful,
  Selector,
  State,
  StateContext,
  Store,
} from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { fromEvent } from 'rxjs';
import { take } from 'rxjs/operators';
import { GetAppConfiguration } from '../actions/config.actions';
import {
  ModifyOpenedTabCount,
  SetLanguage,
  SetRemember,
  SetTenant,
} from '../actions/session.actions';
import { ABP, Session } from '../models';

@State<Session.State>({
  name: 'SessionState',
  defaults: { sessionDetail: { openedTabCount: 0 } } as Session.State,
})
@Injectable()
export class SessionState {
  @Selector()
  static getLanguage({ language }: Session.State): string {
    return language;
  }

  @Selector()
  static getTenant({ tenant }: Session.State): ABP.BasicItem {
    return tenant;
  }

  @Selector()
  static getSessionDetail({ sessionDetail }: Session.State): Session.SessionDetail {
    return sessionDetail;
  }

  constructor(private oAuthService: OAuthService, private store: Store, private actions: Actions) {
    actions
      .pipe(ofActionSuccessful(GetAppConfiguration))
      .pipe(take(1))
      .subscribe(() => {
        const { sessionDetail } = this.store.selectSnapshot(SessionState) || { sessionDetail: {} };

        const fiveMinutesBefore = new Date().valueOf() - 5 * 60 * 1000;

        if (
          sessionDetail.lastExitTime &&
          sessionDetail.openedTabCount === 0 &&
          this.oAuthService.hasValidAccessToken() &&
          sessionDetail.remember === false &&
          sessionDetail.lastExitTime < fiveMinutesBefore
        ) {
          this.oAuthService.logOut();
        }

        this.store.dispatch(new ModifyOpenedTabCount('increase'));

        fromEvent(window, 'unload').subscribe(event => {
          this.store.dispatch(new ModifyOpenedTabCount('decrease'));
        });
      });
  }

  @Action(SetLanguage)
  setLanguage(
    { patchState, dispatch }: StateContext<Session.State>,
    { payload, dispatchAppConfiguration = true }: SetLanguage,
  ) {
    patchState({
      language: payload,
    });

    if (dispatchAppConfiguration) return dispatch(new GetAppConfiguration());
  }

  @Action(SetTenant)
  setTenant({ patchState }: StateContext<Session.State>, { payload }: SetTenant) {
    patchState({
      tenant: payload,
    });
  }

  @Action(SetRemember)
  setRemember(
    { getState, patchState }: StateContext<Session.State>,
    { payload: remember }: SetRemember,
  ) {
    const { sessionDetail } = getState();

    patchState({
      sessionDetail: {
        ...sessionDetail,
        remember,
      },
    });
  }

  @Action(ModifyOpenedTabCount)
  modifyOpenedTabCount(
    { getState, patchState }: StateContext<Session.State>,
    { operation }: ModifyOpenedTabCount,
  ) {
    // tslint:disable-next-line: prefer-const
    let { openedTabCount, lastExitTime, ...detail } =
      getState().sessionDetail || ({ openedTabCount: 0 } as Session.SessionDetail);

    if (operation === 'increase') {
      openedTabCount++;
    } else if (operation === 'decrease') {
      openedTabCount--;
      lastExitTime = new Date().valueOf();
    }

    if (!openedTabCount || openedTabCount < 0) {
      openedTabCount = 0;
    }

    patchState({
      sessionDetail: {
        openedTabCount,
        lastExitTime,
        ...detail,
      },
    });
  }
}
