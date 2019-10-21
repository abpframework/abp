import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { PermissionManagementStateService } from '../services/permission-management-state.service';
import { PermissionManagementState } from '../states/permission-management.state';
import { Store } from '@ngxs/store';
describe('PermissionManagementStateService', () => {
  let service: PermissionManagementStateService;
  let spectator: SpectatorService<PermissionManagementStateService>;
  const createService = createServiceFactory({ service: PermissionManagementStateService, mocks: [Store] });
  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
  });
  test('should have the all PermissionManagementState static methods', () => {
    const reg = /(?<=static )(.*)(?=\()/gm;
    PermissionManagementState.toString()
      .match(reg)
      .forEach(fnName => {
        expect(service[fnName]).toBeTruthy();
      });
  });
});
