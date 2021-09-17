import { LazyLoadService, LOADING_STRATEGY, LocalizationService } from '@abp/ng.core';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { EMPTY, of } from 'rxjs';
import { BOOTSTRAP } from '../constants/styles';
import { createLazyStyleHref, initLazyStyleHandler, LazyStyleHandler } from '../handlers';

const languageChange$ = of({ payload: 'en' });

describe('LazyStyleHandler', () => {
  let spectator: SpectatorService<LazyStyleHandler>;
  let handler: LazyStyleHandler;
  let lazyLoad: LazyLoadService;

  const createService = createServiceFactory({
    service: LazyStyleHandler,
    providers: [
      {
        provide: LocalizationService,
        useValue: { currentLang: 'en', languageChange$ },
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

describe('initLazyStyleHandler', () => {
  it('should return a LazyStyleHandler factory', () => {
    const generator = (function* () {
      yield undefined; // LAZY_STYLES
      yield { loaded: new Map() }; // LazyLoadService
      yield { currentLang: 'en', languageChange$: EMPTY }; // LocalizationService
    })();

    const injector = {
      get: () => generator.next().value as any,
    };
    const factory = initLazyStyleHandler(injector);

    expect(factory()).toBeInstanceOf(LazyStyleHandler);
  });
});
