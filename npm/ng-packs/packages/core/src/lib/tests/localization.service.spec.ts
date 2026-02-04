import { createServiceFactory, SpectatorService } from '@ngneat/spectator/vitest';
import { Subject } from 'rxjs';
import { LocalizationService } from '../services/localization.service';
import { SessionStateService } from '../services/session-state.service';
import { ConfigStateService } from '../services/config-state.service';
import { Injector } from '@angular/core';

describe('LocalizationService', () => {
  let spectator: SpectatorService<LocalizationService>;
  let service: LocalizationService;
  let sessionState: SessionStateService;
  let configState: ConfigStateService;
  let injector: Injector;

  const createService = createServiceFactory({
    service: LocalizationService,
    providers: [
      {
        provide: SessionStateService,
        useValue: {
          getLanguage: vi.fn(() => 'en'),
          setLanguage: vi.fn(),
          getLanguage$: vi.fn(() => new Subject()),
          onLanguageChange$: vi.fn(() => new Subject()),
        },
      },
      {
        provide: ConfigStateService,
        useValue: {
          getOne: vi.fn(),
          refreshAppState: vi.fn(),
          getDeep: vi.fn(),
          getDeep$: vi.fn(() => new Subject()),
          getOne$: vi.fn(() => new Subject()),
        },
      },
      {
        provide: Injector,
        useValue: {
          get: vi.fn(),
        },
      },
    ],
  });

  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
    sessionState = spectator.inject(SessionStateService);
    configState = spectator.inject(ConfigStateService);
    injector = spectator.inject(Injector);
  });

  describe('#registerLocale', () => {
    it('should create service successfully', () => {
      expect(service).toBeTruthy();
    });
  });

  describe('#localize', () => {
    it('should return observable for localization', () => {
      const result = service.localize('test', 'key', 'default');
      expect(result).toBeDefined();
    });
  });

  describe('#localizeSync', () => {
    it('should return sync localization', () => {
      const result = service.localizeSync('test', 'key', 'default');
      expect(result).toBeDefined();
    });
  });
});
