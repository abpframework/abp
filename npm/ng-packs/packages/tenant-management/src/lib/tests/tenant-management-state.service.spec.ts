import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { TenantManagementStateService } from '../services/tenant-management-state.service';
import { TenantManagementState } from '../states/tenant-management.state';
import { Store } from '@ngxs/store';
describe('TenantManagementStateService', () => {
  let service: TenantManagementStateService;
  let spectator: SpectatorService<TenantManagementStateService>;
  let store: SpyObject<Store>;

  const createService = createServiceFactory({ service: TenantManagementStateService, mocks: [Store] });
  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
    store = spectator.get(Store);
  });

  test('should have the all TenantManagementState static methods', () => {
    const reg = /(?<=static )(.*)(?=\()/gm;
    TenantManagementState.toString()
      .match(reg)
      .forEach(fnName => {
        expect(service[fnName]).toBeTruthy();

        const spy = jest.spyOn(store, 'selectSnapshot');
        spy.mockClear();

        const isDynamicSelector = TenantManagementState[fnName].name !== 'memoized';

        if (isDynamicSelector) {
          TenantManagementState[fnName] = jest.fn((...args) => args);
          service[fnName]('test', 0, {});
          expect(TenantManagementState[fnName]).toHaveBeenCalledWith('test', 0, {});
        } else {
          service[fnName]();
          expect(spy).toHaveBeenCalledWith(TenantManagementState[fnName]);
        }
      });
  });
});
