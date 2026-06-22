import { Component, ComponentRef } from '@angular/core';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/vitest';
import { ContentProjectionService } from '../services';

describe('ContentProjectionService', () => {
  @Component({
    template: '<div class="foo">bar</div>',
  })
  class TestComponent {}

  let componentRef: ComponentRef<TestComponent>;
  let spectator: SpectatorService<ContentProjectionService>;
  const createService = createServiceFactory({
    service: ContentProjectionService,
    imports: [TestComponent],
  });

  beforeEach(() => (spectator = createService()));

  afterEach(() => {
    if (componentRef) {
      componentRef.destroy();
    }
  });

  describe('#projectContent', () => {
    it('should create service successfully', () => {
      expect(spectator.service).toBeTruthy();
    });
  });
});
