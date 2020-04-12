import { createServiceFactory, SpectatorService } from '@ngneat/spectator';
import { DomInsertionService } from '../services';
import { CONTENT_STRATEGY } from '../strategies';

describe('DomInsertionService', () => {
  let styleElements: NodeListOf<HTMLStyleElement>;
  let spectator: SpectatorService<DomInsertionService>;
  const createService = createServiceFactory(DomInsertionService);

  beforeEach(() => (spectator = createService()));

  afterEach(() => styleElements.forEach(element => element.remove()));

  describe('#insertContent', () => {
    it('should be able to insert given content', () => {
      spectator.service.insertContent(CONTENT_STRATEGY.AppendStyleToHead('.test {}'));
      styleElements = document.head.querySelectorAll('style');
      expect(styleElements.length).toBe(1);
      expect(styleElements[0].textContent).toBe('.test {}');
    });

    it('should insert only once', () => {
      expect(spectator.service.inserted.has(1437348290)).toBe(false);

      spectator.service.insertContent(CONTENT_STRATEGY.AppendStyleToHead('.test {}'));
      styleElements = document.head.querySelectorAll('style');

      expect(styleElements.length).toBe(1);
      expect(styleElements[0].textContent).toBe('.test {}');
      expect(spectator.service.inserted.has(1437348290)).toBe(true);

      spectator.service.insertContent(CONTENT_STRATEGY.AppendStyleToHead('.test {}'));
      styleElements = document.head.querySelectorAll('style');

      expect(styleElements.length).toBe(1);
      expect(styleElements[0].textContent).toBe('.test {}');
      expect(spectator.service.inserted.has(1437348290)).toBe(true);
    });

    it('should be able to insert given content', () => {
      spectator.service.insertContent(CONTENT_STRATEGY.AppendStyleToHead('.test {}'));
      expect(spectator.service.inserted.has(1437348290)).toBe(true);
    });
  });
});
