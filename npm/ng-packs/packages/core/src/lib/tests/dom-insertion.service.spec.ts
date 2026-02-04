import { Injector, runInInjectionContext } from '@angular/core';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator';
import { DomInsertionService } from '../services';
import { CONTENT_STRATEGY } from '../strategies';

describe('DomInsertionService', () => {
  let styleElements: NodeListOf<HTMLStyleElement>;
  let spectator: SpectatorService<DomInsertionService>;
  const createService = createServiceFactory(DomInsertionService);
  const content = '.test {}';

  beforeEach(() => (spectator = createService()));

  afterEach(() => (document.head.innerHTML = ''));

  describe('#insertContent', () => {
    it('should be able to insert given content', () => {
      const injector = spectator.inject(Injector);
      runInInjectionContext(injector, () => spectator.service.insertContent(CONTENT_STRATEGY.AppendStyleToHead(content)));
      styleElements = document.head.querySelectorAll('style');
      expect(styleElements.length).toBe(1);
      expect(styleElements[0].textContent).toBe(content);
    });

    it('should set a hash for the inserted content', () => {
      const injector = spectator.inject(Injector);
      runInInjectionContext(injector, () => spectator.service.insertContent(CONTENT_STRATEGY.AppendStyleToHead(content)));
      expect(spectator.service.has(content)).toBe(true);
    });

    it('should insert only once', () => {
      expect(spectator.service.has(content)).toBe(false);
      const injector = spectator.inject(Injector);
      runInInjectionContext(injector, () => spectator.service.insertContent(CONTENT_STRATEGY.AppendStyleToHead(content)));
      styleElements = document.head.querySelectorAll('style');

      expect(styleElements.length).toBe(1);
      expect(styleElements[0].textContent).toBe(content);
      expect(spectator.service.has(content)).toBe(true);

      runInInjectionContext(injector, () => spectator.service.insertContent(CONTENT_STRATEGY.AppendStyleToHead(content)));
      styleElements = document.head.querySelectorAll('style');

      expect(styleElements.length).toBe(1);
      expect(styleElements[0].textContent).toBe(content);
      expect(spectator.service.has(content)).toBe(true);
    });

    it('should return inserted element', () => {
      const injector = spectator.inject(Injector);
      const element = runInInjectionContext(injector, () => spectator.service.insertContent(CONTENT_STRATEGY.AppendStyleToHead(content)));
      expect(element.tagName).toBe('STYLE');
    });
  });

  describe('#removeContent', () => {
    it('should remove inserted element and the hash for the content', () => {
      expect(document.head.querySelector('style')).toBeNull();
      const injector = spectator.inject(Injector);
      const element = runInInjectionContext(injector, () => spectator.service.insertContent(CONTENT_STRATEGY.AppendStyleToHead(content)));
      expect(spectator.service.has(content)).toBe(true);

      spectator.service.removeContent(element);
      expect(spectator.service.has(content)).toBe(false);
      expect(document.head.querySelector('style')).toBeNull();
    });
  });
});
