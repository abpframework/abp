import { RoutesService } from '../services/routes.service';
import { createServiceFactory, SpectatorService } from '@ngneat/spectator/vitest';
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
          get: vi.fn(),
          post: vi.fn(),
          put: vi.fn(),
          delete: vi.fn(),
        },
      },
      {
        provide: ConfigStateService,
        useValue: {
          getOne: vi.fn(),
          getDeep: vi.fn(),
          getDeep$: vi.fn(() => ({ subscribe: vi.fn() })),
          createOnUpdateStream: vi.fn(() => ({ 
            subscribe: vi.fn(() => ({ unsubscribe: vi.fn() })) 
          })),
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
