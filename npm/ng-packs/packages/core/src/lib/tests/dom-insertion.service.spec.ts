import { createServiceFactory, SpectatorService } from '@ngneat/spectator';
import { DomInsertionService } from '../services';
import { CONTENT_STRATEGY } from '../strategies';

describe('DomInsertionService', () => {
  let spectator: SpectatorService<DomInsertionService>;
  const createService = createServiceFactory(DomInsertionService);

  beforeEach(() => (spectator = createService()));

  it('should be insert an element', () => {
    spectator.service.insertContent(CONTENT_STRATEGY.AppendStyleToHead('.test {}'));
    expect(spectator.service.inserted.has(1437348290)).toBe(true);
  });
});
