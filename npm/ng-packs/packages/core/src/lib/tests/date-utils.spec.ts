import { ConfigStateService } from '../services';
import { getShortDateFormat, getShortDateShortTimeFormat, getShortTimeFormat } from '../utils';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/vitest';
import { CORE_OPTIONS } from '../tokens/options.token';
import { HttpClient } from '@angular/common/http';
import { AbpApplicationConfigurationService } from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/abp-application-configuration.service';
import { RestService } from '../services/rest.service';
import { EnvironmentService } from '../services/environment.service';
import { HttpErrorReporterService } from '../services/http-error-reporter.service';
import { ExternalHttpClient } from '../clients/http.client';

const dateTimeFormat = {
  calendarAlgorithmType: 'SolarCalendar',
  dateSeparator: '/',
  dateTimeFormatLong: 'dddd, MMMM d, yyyy',
  fullDateTimePattern: 'dddd, MMMM d, yyyy h:mm:ss tt',
  longTimePattern: 'h:mm:ss tt',
  shortDatePattern: 'M/d/yyyy',
  shortTimePattern: 'h:mm tt',
};

describe('Date Utils', () => {
  let spectator: SpectatorService<ConfigStateService>;
  let config: ConfigStateService;

  const createService = createServiceFactory({
    service: ConfigStateService,
    providers: [
      {
        provide: CORE_OPTIONS,
        useValue: {
          environment: {
            apis: {
              default: {
                url: 'http://localhost:4200',
              },
            },
          },
        },
      },
      {
        provide: HttpClient,
        useValue: {
          get: vi.fn(),
          post: vi.fn(),
          put: vi.fn(),
          delete: vi.fn(),
        },
      },
      {
        provide: AbpApplicationConfigurationService,
        useValue: {
          get: vi.fn(),
        },
      },
      {
        provide: RestService,
        useValue: {
          request: vi.fn(),
        },
      },
      {
        provide: EnvironmentService,
        useValue: {
          getEnvironment: vi.fn(),
        },
      },
      {
        provide: HttpErrorReporterService,
        useValue: {
          reportError: vi.fn(),
        },
      },
      {
        provide: ExternalHttpClient,
        useValue: {
          request: vi.fn(),
        },
      },
    ],
  });

  beforeEach(() => {
    spectator = createService();
    config = spectator.service;
  });

  describe('#getShortDateFormat', () => {
    test('should get the short date format from ConfigStateService and return it', () => {
      const getDeepSpy = vi.spyOn(config, 'getDeep');
      getDeepSpy.mockReturnValueOnce(dateTimeFormat);

      expect(getShortDateFormat(config)).toBe('M/d/yyyy');
      expect(getDeepSpy).toHaveBeenCalledWith('localization.currentCulture.dateTimeFormat');
    });
  });

  describe('#getShortTimeFormat', () => {
    test('should get the short time format from ConfigStateService and return it', () => {
      const getDeepSpy = vi.spyOn(config, 'getDeep');
      getDeepSpy.mockReturnValueOnce(dateTimeFormat);

      expect(getShortTimeFormat(config)).toBe('h:mm a');
      expect(getDeepSpy).toHaveBeenCalledWith('localization.currentCulture.dateTimeFormat');
    });
  });

  describe('#getShortDateShortTimeFormat', () => {
    test('should get the short date time format from ConfigStateService and return it', () => {
      const getDeepSpy = vi.spyOn(config, 'getDeep');
      getDeepSpy.mockReturnValueOnce(dateTimeFormat);

      expect(getShortDateShortTimeFormat(config)).toBe('M/d/yyyy h:mm a');
      expect(getDeepSpy).toHaveBeenCalledWith('localization.currentCulture.dateTimeFormat');
    });
  });
});
