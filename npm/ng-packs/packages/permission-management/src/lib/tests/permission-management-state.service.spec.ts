import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { PermissionManagementStateService } from '../services/permission-management-state.service';
import { PermissionManagementState } from '../states/permission-management.state';
import { Store } from '@ngxs/store';

describe('PermissionManagementStateService', () => {
  let service: PermissionManagementStateService;
  let spectator: SpectatorService<PermissionManagementStateService>;
  let store: SpyObject<Store>;

  const createService = createServiceFactory({ service: PermissionManagementStateService, mocks: [Store] });
  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
    store = spectator.get(Store);
  });
  test('should have the all PermissionManagementState static methods', () => {
    const reg = /(?<=static )(.*)(?=\()/gm;
    PermissionManagementState.toString()
      .match(reg)
      .forEach(fnName => {
        expect(service[fnName]).toBeTruthy();

        const spy = jest.spyOn(store, 'selectSnapshot');
        spy.mockClear();

        const isDynamicSelector = PermissionManagementState[fnName].name !== 'memoized';

        if (isDynamicSelector) {
          PermissionManagementState[fnName] = jest.fn((...args) => args);
          service[fnName]('test', 0, {});
          expect(PermissionManagementState[fnName]).toHaveBeenCalledWith('test', 0, {});
        } else {
          service[fnName]();
          expect(spy).toHaveBeenCalledWith(PermissionManagementState[fnName]);
        }
      });
  });
});
