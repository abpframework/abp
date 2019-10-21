import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import { FeatureManagementStateService } from '../services/feature-management-state.service';
import { FeatureManagementState } from '../states';

describe('FeatureManagementStateService', () => {
  let service: FeatureManagementStateService;
  let spectator: SpectatorService<FeatureManagementStateService>;
  const createService = createServiceFactory({ service: FeatureManagementStateService, mocks: [Store] });
  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
  });
  test('should have the all FeatureManagementState static methods', () => {
    const reg = /(?<=static )(.*)(?=\()/gm;
    FeatureManagementState.toString()
      .match(reg)
      .forEach(fnName => {
        expect(service[fnName]).toBeTruthy();
      });
  });
});
