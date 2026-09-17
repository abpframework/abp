import { DOCUMENT } from '@angular/common';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/vitest';
import { of, Subject } from 'rxjs';
import { vi } from 'vitest';

import { LazyLoadService, LoadingStrategy, LocalizationService } from '@abp/ng.core';
import { DocumentDirHandlerService } from '@abp/ng.theme.shared';
import { BOOTSTRAP, createLazyStyleHref, LazyStyleHandler } from '../handlers';
import { LAZY_STYLES } from '../tokens/lazy-styles.token';
import { setupComponentResources } from './utils';

describe('LazyStyleHandler', () => {
  let spectator: SpectatorService<LazyStyleHandler>;
  let handler: LazyStyleHandler;
  let lazyLoad: any;

  beforeAll(async () => {
    await setupComponentResources(
      '../components/breadcrumb',
      import.meta.url
    );
  });

  const dir$ = new Subject<'ltr' | 'rtl'>();

  const createService = createServiceFactory({
    service: LazyStyleHandler,
    providers: [
      {
        provide: DOCUMENT,
        useValue: document,
      },
      {
        provide: LAZY_STYLES,
        useValue: [BOOTSTRAP],
      },
      {
        provide: LazyLoadService,
        useValue: {
          loaded: new Map(),
          load: vi.fn(() => of(null)),
          remove: vi.fn(),
        },
      },
      {
        provide: DocumentDirHandlerService,
        useValue: {
          dir$,
        },
      },
      {
        provide: LocalizationService,
        useValue: {
          currentLang: 'en',
          currentLang$: of({ payload: 'en' }),
        },
      },
    ],
  });

  beforeEach(() => {
    spectator = createService();
    handler = spectator.service;
    lazyLoad = spectator.inject(LazyLoadService);
  });

  describe('#dir', () => {
    it('should initially be "ltr"', () => {
      expect(handler.dir).toBe('ltr');
    });

    it('should set bootstrap to rtl', () => {
      const oldHref = createLazyStyleHref(BOOTSTRAP, 'ltr');
      const newHref = createLazyStyleHref(BOOTSTRAP, 'rtl');

      lazyLoad.loaded.set(newHref, null);

      const loadSpy = vi.spyOn(lazyLoad, 'load');
      const removeSpy = vi.spyOn(lazyLoad, 'remove');

      handler.dir = 'rtl';

      expect(loadSpy).toHaveBeenCalledTimes(1);
      const [strategy] = loadSpy.mock.calls[0];
      expect((strategy as LoadingStrategy).path).toBe(newHref);

      expect(removeSpy).toHaveBeenCalledWith(oldHref);
    });
  });
});
