import {
  ABP,
  LocalizationPipe,
  RouterOutletComponent,
  RoutesService,
  provideAbpCore,
  withOptions,
  RestService,
  AbpApplicationConfigurationService,
  ConfigStateService,
} from '@abp/ng.core';
import { RouterModule } from '@angular/router';
import { createRoutingFactory, SpectatorRouting } from '@ngneat/spectator/vitest';
import { of } from 'rxjs';
import { BreadcrumbComponent, BreadcrumbItemsComponent } from '../components';
import { setupComponentResources } from './utils';

const mockRoutes: ABP.Route[] = [
  { name: '_::Identity', path: '/identity' },
  { name: '_::Users', path: '/identity/users', parentName: '_::Identity' },
];

describe('BreadcrumbComponent', () => {
  let spectator: SpectatorRouting<RouterOutletComponent>;
  let routes: RoutesService;

  const createRouting = createRoutingFactory({
    component: RouterOutletComponent,
    stubsEnabled: false,
    detectChanges: false,
    imports: [
      RouterModule,
      LocalizationPipe,
      BreadcrumbComponent,
      BreadcrumbItemsComponent,
    ],
    providers: [
      provideAbpCore(
        withOptions({
          environment: {
            apis: {
              default: {
                url: 'http://localhost:4200',
              },
            },
            application: {
              name: 'TestApp',
              baseUrl: 'http://localhost:4200',
            },
          },
          registerLocaleFn: () => Promise.resolve(),
          skipGetAppConfiguration: true,
        }),
      ),
      {
        provide: RestService,
        useValue: {
          request: vi.fn(),
          handleError: vi.fn(),
        },
      },
      {
        provide: AbpApplicationConfigurationService,
        useValue: {
          get: vi.fn(),
        },
      },
      {
        provide: ConfigStateService,
        useValue: {
          getOne: vi.fn(),
          getAll: vi.fn(() => ({})),
          getAll$: vi.fn(() => of({})),
          getDeep: vi.fn(),
          getDeep$: vi.fn(() => of(undefined)),
          createOnUpdateStream: vi.fn(() => ({ 
            subscribe: vi.fn(() => ({ unsubscribe: vi.fn() })) 
          })),
          refreshAppState: vi.fn(),
        },
      },
    ],
    routes: [
      {
        path: '',
        children: [
          {
            path: 'identity',
            children: [
              {
                path: 'users',
                component: BreadcrumbComponent,
              },
            ],
          },
        ],
      },
    ],
  });

  beforeAll(() => setupComponentResources('../components/breadcrumb', import.meta.url));

  beforeEach(() => {
    spectator = createRouting();
    routes = spectator.inject(RoutesService);
  });

  it('should create component', async () => {
    routes.add(mockRoutes);
    await spectator.router.navigateByUrl('/identity/users');
    spectator.detectChanges();
    expect(spectator.component).toBeTruthy();
  });

  it('should handle empty routes', async () => {
    routes.add([]);
    await spectator.router.navigateByUrl('/identity/users');
    spectator.detectChanges();
    expect(spectator.component).toBeTruthy();
  });
});
