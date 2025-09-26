import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
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
          getLanguage: jest.fn(() => 'en'),
          setLanguage: jest.fn(),
          getLanguage$: jest.fn(() => new Subject()),
          onLanguageChange$: jest.fn(() => new Subject()),
        },
      },
      {
        provide: ConfigStateService,
        useValue: {
          getOne: jest.fn(),
          refreshAppState: jest.fn(),
          getDeep: jest.fn(),
          getDeep$: jest.fn(() => new Subject()),
          getOne$: jest.fn(() => new Subject()),
        },
      },
      {
        provide: Injector,
        useValue: {
          get: jest.fn(),
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
