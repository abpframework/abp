import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { LayoutStateService } from '../services/layout-state.service';
import { LayoutState } from '../states/layout.state';
import { Store } from '@ngxs/store';
describe('LayoutStateService', () => {
  let service: LayoutStateService;
  let spectator: SpectatorService<LayoutStateService>;
  const createService = createServiceFactory({ service: LayoutStateService, mocks: [Store] });
  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
  });
  test('should have the all LayoutState static methods', () => {
    const reg = /(?<=static )(.*)(?=\()/gm;
    LayoutState.toString()
      .match(reg)
      .forEach(fnName => {
        expect(service[fnName]).toBeTruthy();
      });
  });
});
