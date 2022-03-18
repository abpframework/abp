import { LazyLoadService, LOADING_STRATEGY, LocalizationService } from '@abp/ng.core';
import { DocumentDirHandlerService } from '@abp/ng.theme.shared';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { of } from 'rxjs';
import { BOOTSTRAP, createLazyStyleHref, LazyStyleHandler } from '../handlers';

const currentLang$ = of({ payload: 'en' });

describe('LazyStyleHandler', () => {
  let spectator: SpectatorService<LazyStyleHandler>;
  let handler: LazyStyleHandler;
  let lazyLoad: LazyLoadService;

  const createService = createServiceFactory({
    service: LazyStyleHandler,
    providers: [
      DocumentDirHandlerService,
      {
        provide: LocalizationService,
        useValue: { currentLang: 'en', currentLang$ },
      },
    ],
  });

  beforeEach(() => {
    spectator = createService();
    handler = spectator.service;
    lazyLoad = handler['lazyLoad'];
  });

  describe('#dir', () => {
    it('should initially be "ltr"', () => {
      expect(handler.dir).toBe('ltr');
    });

    it('should set bootstrap to rtl', () => {
      const oldHref = createLazyStyleHref(BOOTSTRAP, 'ltr');
      const newHref = createLazyStyleHref(BOOTSTRAP, 'rtl');
      lazyLoad.loaded.set(newHref, null); // avoid actual loading
      const load = jest.spyOn(lazyLoad, 'load');
      const remove = jest.spyOn(lazyLoad, 'remove');
      const strategy = LOADING_STRATEGY.PrependAnonymousStyleToHead(newHref);

      handler.dir = 'rtl';

      expect(load).toHaveBeenCalledWith(strategy);
      expect(remove).toHaveBeenCalledWith(oldHref);
    });
  });
});
