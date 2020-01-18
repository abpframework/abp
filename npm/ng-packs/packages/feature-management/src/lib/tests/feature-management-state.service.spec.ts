import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import { FeatureManagementStateService } from '../services/feature-management-state.service';
import { FeatureManagementState } from '../states';
import * as FeatureManagementActions from '../actions';

describe('FeatureManagementStateService', () => {
  let service: FeatureManagementStateService;
  let spectator: SpectatorService<FeatureManagementStateService>;
  let store: SpyObject<Store>;

  const createService = createServiceFactory({
    service: FeatureManagementStateService,
    mocks: [Store],
  });
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

  test('should have a dispatch method for every FeatureManagementState action', () => {
    const reg = /(?<=dispatch)(\w+)(?=\()/gm;
    FeatureManagementStateService.toString()
      .match(reg)
      .forEach(fnName => {
        expect(FeatureManagementActions[fnName]).toBeTruthy();

        const spy = jest.spyOn(store, 'dispatch');
        spy.mockClear();

        const params = Array.from(new Array(FeatureManagementActions[fnName].length));

        service[`dispatch${fnName}`](...params);
        expect(spy).toHaveBeenCalledWith(new FeatureManagementActions[fnName](...params));
      });
  });
});
