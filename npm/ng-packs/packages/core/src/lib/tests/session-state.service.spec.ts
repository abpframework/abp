import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { SessionStateService } from '../services/session-state.service';
import { SessionState } from '../states/session.state';
import { Store } from '@ngxs/store';
describe('SessionStateService', () => {
  let service: SessionStateService;
  let spectator: SpectatorService<SessionStateService>;
  const createService = createServiceFactory({ service: SessionStateService, mocks: [Store] });
  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
  });
  test('should have the all SessionState static methods', () => {
    const reg = /(?<=static )(.*)(?=\()/gm;
    SessionState.toString()
      .match(reg)
      .forEach(fnName => {
        expect(service[fnName]).toBeTruthy();
      });
  });
});
