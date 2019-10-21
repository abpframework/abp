import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { ProfileStateService } from '../services/profile-state.service';
import { ProfileState } from '../states/profile.state';
import { Store } from '@ngxs/store';
describe('ProfileStateService', () => {
  let service: ProfileStateService;
  let spectator: SpectatorService<ProfileStateService>;
  const createService = createServiceFactory({ service: ProfileStateService, mocks: [Store] });
  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
  });
  test('should have the all ProfileState static methods', () => {
    const reg = /(?<=static )(.*)(?=\()/gm;
    ProfileState.toString()
      .match(reg)
      .forEach(fnName => {
        expect(service[fnName]).toBeTruthy();
      });
  });
});
