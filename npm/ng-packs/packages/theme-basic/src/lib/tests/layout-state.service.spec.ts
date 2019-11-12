import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { LayoutStateService } from '../services/layout-state.service';
import { LayoutState } from '../states/layout.state';
import { Store } from '@ngxs/store';
describe('LayoutStateService', () => {
  let service: LayoutStateService;
  let spectator: SpectatorService<LayoutStateService>;
  let store: SpyObject<Store>;

  const createService = createServiceFactory({ service: LayoutStateService, mocks: [Store] });
  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
    store = spectator.get(Store);
  });

  test('should have the all LayoutState static methods', () => {
    const reg = /(?<=static )(.*)(?=\()/gm;
    LayoutState.toString()
      .match(reg)
      .forEach(fnName => {
        expect(service[fnName]).toBeTruthy();

        const spy = jest.spyOn(store, 'selectSnapshot');
        spy.mockClear();

        const isDynamicSelector = LayoutState[fnName].name !== 'memoized';

        if (isDynamicSelector) {
          LayoutState[fnName] = jest.fn((...args) => args);
          service[fnName]('test', 0, {});
          expect(LayoutState[fnName]).toHaveBeenCalledWith('test', 0, {});
        } else {
          service[fnName]();
          expect(spy).toHaveBeenCalledWith(LayoutState[fnName]);
        }
      });
  });
});
