import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { IdentityStateService } from '../services/identity-state.service';
import { IdentityState } from '../states/identity.state';
import { Store } from '@ngxs/store';
describe('IdentityStateService', () => {
  let service: IdentityStateService;
  let spectator: SpectatorService<IdentityStateService>;
  const createService = createServiceFactory({ service: IdentityStateService, mocks: [Store] });
  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
  });
  test('should have the all IdentityState static methods', () => {
    const reg = /(?<=static )(.*)(?=\()/gm;
    IdentityState.toString()
      .match(reg)
      .forEach(fnName => {
        expect(service[fnName]).toBeTruthy();
      });
  });
});
