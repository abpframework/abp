import { Component, ComponentRef, NgModule } from '@angular/core';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator';
import { DomInsertionService } from '../services';
import { CONTENT_STRATEGY, PROJECTION_STRATEGY } from '../strategies';

describe('DomInsertionService', () => {
  @Component({ template: '<div class="foo">bar</div>' })
  class TestComponent {}

  // createServiceFactory does not accept entryComponents directly
  @NgModule({
    declarations: [TestComponent],
    entryComponents: [TestComponent],
  })
  class TestModule {}

  let spectator: SpectatorService<DomInsertionService>;
  const createService = createServiceFactory({
    service: DomInsertionService,
    imports: [TestModule],
  });
  let styleElements: NodeListOf<HTMLStyleElement>;

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

  describe('#projectContent', () => {
    it('should call injectContent of given projectionStrategy and return what it returns', () => {
      const strategy = PROJECTION_STRATEGY.AppendComponentToBody(TestComponent);
      const componentRef = spectator.service.projectContent(strategy);
      const foo = document.querySelector('body > ng-component > div.foo');

      expect(componentRef).toBeInstanceOf(ComponentRef);
      expect(foo.textContent).toBe('bar');
      componentRef.destroy();
    });
  });
});
