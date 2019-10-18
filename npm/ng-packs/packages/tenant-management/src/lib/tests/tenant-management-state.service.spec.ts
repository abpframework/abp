import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { TenantManagementStateService } from '../services/tenant-management-state.service';
import { TenantManagementState } from '../states/tenant-management.state';
import { Store } from '@ngxs/store';
describe('TenantManagementStateService', () => {
  let service: TenantManagementStateService;
  let spectator: SpectatorService<TenantManagementStateService>;
  const createService = createServiceFactory({ service: TenantManagementStateService, mocks: [Store] });
  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
  });
  test('should have the all TenantManagementState static methods', () => {
    const reg = /(?<=static )(.*)(?=\()/gm;
    TenantManagementState.toString()
      .match(reg)
      .forEach(fnName => {
        expect(service[fnName]).toBeTruthy();
      });
  });
});
