import { RoutesService } from '../services/routes.service';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { CORE_OPTIONS } from '../tokens/options.token';
import { HttpClient } from '@angular/common/http';
import { ConfigStateService } from '../services/config-state.service';
import { AbpApplicationConfigurationService } from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/abp-application-configuration.service';
import { RestService } from '../services/rest.service';
import { EnvironmentService } from '../services/environment.service';
import { HttpErrorReporterService } from '../services/http-error-reporter.service';
import { ExternalHttpClient } from '../clients/http.client';
import { OTHERS_GROUP } from '../tokens';
import { SORT_COMPARE_FUNC, compareFuncFactory } from '../tokens/compare-func.token';

describe('Routes Service', () => {
  let spectator: SpectatorService<RoutesService>;
  let service: RoutesService;

  const createService = createServiceFactory({
    service: RoutesService,
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
          get: jest.fn(),
          post: jest.fn(),
          put: jest.fn(),
          delete: jest.fn(),
        },
      },
      {
        provide: ConfigStateService,
        useValue: {
          getOne: jest.fn(),
          getDeep: jest.fn(),
          getDeep$: jest.fn(() => ({ subscribe: jest.fn() })),
          createOnUpdateStream: jest.fn(() => ({ 
            subscribe: jest.fn(() => ({ unsubscribe: jest.fn() })) 
          })),
        },
      },
      {
        provide: AbpApplicationConfigurationService,
        useValue: {
          get: jest.fn(),
        },
      },
      {
        provide: RestService,
        useValue: {
          request: jest.fn(),
        },
      },
      {
        provide: EnvironmentService,
        useValue: {
          getEnvironment: jest.fn(),
        },
      },
      {
        provide: HttpErrorReporterService,
        useValue: {
          reportError: jest.fn(),
        },
      },
      {
        provide: ExternalHttpClient,
        useValue: {
          request: jest.fn(),
        },
      },
      {
        provide: OTHERS_GROUP,
        useValue: 'AbpUi::OthersGroup',
      },
      {
        provide: SORT_COMPARE_FUNC,
        useValue: compareFuncFactory,
      },
    ],
  });

  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
  });

  describe('#add', () => {
    it('should create service successfully', () => {
      expect(service).toBeTruthy();
    });

    it('should have observable properties', () => {
      expect(service.flat$).toBeDefined();
      expect(service.tree$).toBeDefined();
      expect(service.visible$).toBeDefined();
    });
  });
});
