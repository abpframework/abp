import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { ConfigStateService } from '../services/config-state.service';
import { ConfigState } from '../states';
import { Store } from '@ngxs/store';
describe('ConfigStateService', () => {
  let service: ConfigStateService;
  let spectator: SpectatorService<ConfigStateService>;
  const createService = createServiceFactory({ service: ConfigStateService, mocks: [Store] });
  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
  });
  test('should have the all ConfigState static methods', () => {
    const reg = /(?<=static )(.*)(?=\()/gm;
    ConfigState.toString()
      .match(reg)
      .forEach(fnName => {
        expect(service[fnName]).toBeTruthy();
      });
  });
});
