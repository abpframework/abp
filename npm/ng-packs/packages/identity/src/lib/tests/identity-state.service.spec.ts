import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { IdentityStateService } from '../services/identity-state.service';
import { IdentityState } from '../states/identity.state';
import { Store } from '@ngxs/store';
describe('IdentityStateService', () => {
  let service: IdentityStateService;
  let spectator: SpectatorService<IdentityStateService>;
  let store: SpyObject<Store>;

  const createService = createServiceFactory({ service: IdentityStateService, mocks: [Store] });
  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
    store = spectator.get(Store);
  });

  test('should have the all IdentityState static methods', () => {
    const reg = /(?<=static )(.*)(?=\()/gm;
    IdentityState.toString()
      .match(reg)
      .forEach(fnName => {
        expect(service[fnName]).toBeTruthy();

        const spy = jest.spyOn(store, 'selectSnapshot');
        spy.mockClear();

        const isDynamicSelector = IdentityState[fnName].name !== 'memoized';

        if (isDynamicSelector) {
          IdentityState[fnName] = jest.fn((...args) => args);
          service[fnName]('test', 0, {});
          expect(IdentityState[fnName]).toHaveBeenCalledWith('test', 0, {});
        } else {
          service[fnName]();
          expect(spy).toHaveBeenCalledWith(IdentityState[fnName]);
        }
      });
  });
});
