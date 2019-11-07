import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { SessionStateService } from '../services/session-state.service';
import { SessionState } from '../states/session.state';
import { Store } from '@ngxs/store';
describe('SessionStateService', () => {
  let service: SessionStateService;
  let spectator: SpectatorService<SessionStateService>;
  let store: SpyObject<Store>;

  const createService = createServiceFactory({ service: SessionStateService, mocks: [Store] });
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
});
