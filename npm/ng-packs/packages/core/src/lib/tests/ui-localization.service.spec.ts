import { createServiceFactory, SpectatorService } from '@ngneat/spectator/vitest';
import { of, Subject, throwError } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { UILocalizationService } from '../services/ui-localization.service';
import { LocalizationService } from '../services/localization.service';
import { SessionStateService } from '../services/session-state.service';
import { CORE_OPTIONS } from '../tokens/options.token';

describe('UILocalizationService', () => {
  let spectator: SpectatorService<UILocalizationService>;
  let service: UILocalizationService;
  let language$: Subject<string>;
  let httpGet: ReturnType<typeof vi.fn>;
  let addLocalizationSpy: ReturnType<typeof vi.fn>;

  const createService = createServiceFactory({
    service: UILocalizationService,
    mocks: [HttpClient, LocalizationService],
    providers: [
      {
        provide: SessionStateService,
        useFactory: () => {
          let currentLanguage = 'en';
          language$ = new Subject<string>();
          language$.subscribe(lang => {
            currentLanguage = lang;
          });
          return {
            getLanguage: vi.fn(() => currentLanguage),
            getLanguage$: vi.fn(() => language$.asObservable()),
          };
        },
      },
      {
        provide: CORE_OPTIONS,
        useValue: {
          uiLocalization: {
            enabled: true,
            basePath: '/assets/localization',
          },
        },
      },
    ],
  });

  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
    const http = spectator.inject(HttpClient);
    const localizationService = spectator.inject(LocalizationService);
    httpGet = vi.fn();
    (http as any).get = httpGet;
    addLocalizationSpy = vi.fn();
    (localizationService as any).addLocalization = addLocalizationSpy;
  });

  describe('when uiLocalization is enabled', () => {
    it('should load localization file when language changes', () => {
      const uiData = { MyApp: { Welcome: 'Welcome from UI' } };
      httpGet.mockReturnValue(of(uiData));

      language$.next('en');

      expect(httpGet).toHaveBeenCalledWith('/assets/localization/en.json');
      expect(addLocalizationSpy).toHaveBeenCalledWith([
        {
          culture: 'en',
          resources: [{ resourceName: 'MyApp', texts: { Welcome: 'Welcome from UI' } }],
        },
      ]);
    });

    it('should use default basePath when not provided', () => {
      expect(httpGet).not.toHaveBeenCalled();
      httpGet.mockReturnValue(of({}));
      language$.next('en');
      expect(httpGet).toHaveBeenCalledWith('/assets/localization/en.json');
    });

    it('should not call addLocalization when file is missing (HTTP error)', () => {
      httpGet.mockReturnValue(throwError(() => new Error('404')));

      language$.next('fr');

      expect(httpGet).toHaveBeenCalledWith('/assets/localization/fr.json');
      expect(addLocalizationSpy).not.toHaveBeenCalled();
    });

    it('should cache loaded data in getLoadedLocalizations', () => {
      const uiData = { AbpAccount: { Login: 'Sign In (UI)' } };
      httpGet.mockReturnValue(of(uiData));

      language$.next('en');

      const loaded = service.getLoadedLocalizations('en');
      expect(loaded).toEqual(uiData);
    });

    it('should load again when language changes to another culture', () => {
      httpGet.mockReturnValue(of({}));
      language$.next('en');
      expect(httpGet).toHaveBeenCalledTimes(1);

      httpGet.mockClear();
      httpGet.mockReturnValue(of({ MyApp: { Title: 'Titre' } }));
      language$.next('fr');

      expect(httpGet).toHaveBeenCalledWith('/assets/localization/fr.json');
      expect(addLocalizationSpy).toHaveBeenCalledWith([
        {
          culture: 'fr',
          resources: [{ resourceName: 'MyApp', texts: { Title: 'Titre' } }],
        },
      ]);
    });
  });

  describe('addAngularLocalizeLocalization', () => {
    it('should add localization via LocalizationService (UI data in merge pipeline)', () => {
      service.addAngularLocalizeLocalization('en', 'MyApp', {
        CustomKey: 'UI Override',
      });

      expect(addLocalizationSpy).toHaveBeenCalledWith([
        {
          culture: 'en',
          resources: [
            {
              resourceName: 'MyApp',
              texts: { CustomKey: 'UI Override' },
            },
          ],
        },
      ]);
    });

    it('should merge into getLoadedLocalizations cache', () => {
      service.addAngularLocalizeLocalization('en', 'MyApp', { Key1: 'Val1' });
      service.addAngularLocalizeLocalization('en', 'MyApp', { Key2: 'Val2' });

      const loaded = service.getLoadedLocalizations('en');
      expect(loaded.MyApp).toEqual({ Key1: 'Val1', Key2: 'Val2' });
    });
  });

  describe('getLoadedLocalizations', () => {
    it('should return empty object when no culture loaded', () => {
      expect(service.getLoadedLocalizations('de')).toEqual({});
    });

    it('should return current language when culture not passed', () => {
      const uiData = { R: { K: 'V' } };
      httpGet.mockReturnValue(of(uiData));
      language$.next('tr');

      const loaded = service.getLoadedLocalizations();
      expect(loaded).toEqual(uiData);
    });
  });
});
