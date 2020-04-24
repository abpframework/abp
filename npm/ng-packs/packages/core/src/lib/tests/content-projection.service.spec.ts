import { Component, ComponentRef, NgModule } from '@angular/core';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator';
import { ContentProjectionService } from '../services';
import { PROJECTION_STRATEGY } from '../strategies';

describe('ContentProjectionService', () => {
  @Component({ template: '<div class="foo">bar</div>' })
  class TestComponent {}

  // createServiceFactory does not accept entryComponents directly
  @NgModule({
    declarations: [TestComponent],
    entryComponents: [TestComponent],
  })
  class TestModule {}

  let componentRef: ComponentRef<TestComponent>;
  let spectator: SpectatorService<ContentProjectionService>;
  const createService = createServiceFactory({
    service: ContentProjectionService,
    imports: [TestModule],
  });

  beforeEach(() => (spectator = createService()));

  afterEach(() => componentRef.destroy());

  describe('#projectContent', () => {
    it('should call injectContent of given projectionStrategy and return what it returns', () => {
      const strategy = PROJECTION_STRATEGY.AppendComponentToBody(TestComponent);
      componentRef = spectator.service.projectContent(strategy);
      const foo = document.querySelector('body > ng-component > div.foo');

      expect(componentRef).toBeInstanceOf(ComponentRef);
      expect(foo.textContent).toBe('bar');
    });
  });
});
