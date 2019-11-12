import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import { FeatureManagementStateService } from '../services/feature-management-state.service';
import { FeatureManagementState } from '../states';

describe('FeatureManagementStateService', () => {
  let service: FeatureManagementStateService;
  let spectator: SpectatorService<FeatureManagementStateService>;
  let store: SpyObject<Store>;

  const createService = createServiceFactory({ service: FeatureManagementStateService, mocks: [Store] });
  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
    store = spectator.get(Store);
  });

  test('should have the all FeatureManagementState static methods', () => {
    const reg = /(?<=static )(.*)(?=\()/gm;
    FeatureManagementState.toString()
      .match(reg)
      .forEach(fnName => {
        expect(service[fnName]).toBeTruthy();

        const spy = jest.spyOn(store, 'selectSnapshot');
        spy.mockClear();

        const isDynamicSelector = FeatureManagementState[fnName].name !== 'memoized';

        if (isDynamicSelector) {
          FeatureManagementState[fnName] = jest.fn((...args) => args);
          service[fnName]('test', 0, {});
          expect(FeatureManagementState[fnName]).toHaveBeenCalledWith('test', 0, {});
        } else {
          service[fnName]();
          expect(spy).toHaveBeenCalledWith(FeatureManagementState[fnName]);
        }
      });
  });
});
