import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { SessionStateService } from '../services/session-state.service';
import { SessionState } from '../states/session.state';
import { Store } from '@ngxs/store';
import * as SessionActions from '../actions';
import { OAuthService } from 'angular-oauth2-oidc';

describe('SessionStateService', () => {
  let service: SessionStateService;
  let spectator: SpectatorService<SessionStateService>;
  let store: SpyObject<Store>;

  const createService = createServiceFactory({
    service: SessionStateService,
    mocks: [Store, OAuthService],
  });
  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
    store = spectator.get(Store);
  });
  test('should have the all SessionState static methods', () => {
    const reg = /(?<=static )(.*)(?=\()/gm;
    SessionState.toString()
      .match(reg)
      .forEach(fnName => {
        expect(service[fnName]).toBeTruthy();

        const spy = jest.spyOn(store, 'selectSnapshot');
        spy.mockClear();

        const isDynamicSelector = SessionState[fnName].name !== 'memoized';

        if (isDynamicSelector) {
          SessionState[fnName] = jest.fn((...args) => args);
          service[fnName]('test', 0, {});
          expect(SessionState[fnName]).toHaveBeenCalledWith('test', 0, {});
        } else {
          service[fnName]();
          expect(spy).toHaveBeenCalledWith(SessionState[fnName]);
        }
      });
  });

  test('should have a dispatch method for every sessionState action', () => {
    const reg = /(?<=dispatch)(\w+)(?=\()/gm;
    SessionStateService.toString()
      .match(reg)
      .forEach(fnName => {
        expect(SessionActions[fnName]).toBeTruthy();

        const spy = jest.spyOn(store, 'dispatch');
        spy.mockClear();

        const params = Array.from(new Array(SessionActions[fnName].length));

        service[`dispatch${fnName}`](...params);
        expect(spy).toHaveBeenCalledWith(new SessionActions[fnName](...params));
      });
  });
});
