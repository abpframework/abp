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
import { By } from '@angular/platform-browser';
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
  let consoleErrorSpy: ReturnType<typeof vi.spyOn>;

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
          skipInitAuthService: true,
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
    consoleErrorSpy = vi.spyOn(console, 'error').mockImplementation(() => undefined);
    spectator = createRouting();
    routes = spectator.inject(RoutesService);
  });

  afterEach(() => {
    consoleErrorSpy.mockRestore();
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

  it('should keep the path on segments so breadcrumbs render as links', async () => {
    routes.add(mockRoutes);
    await spectator.router.navigateByUrl('/identity/users');
    spectator.detectChanges();

    const breadcrumb = spectator.fixture.debugElement.query(By.directive(BreadcrumbComponent))
      .componentInstance as BreadcrumbComponent;

    expect(breadcrumb.segments().map(segment => segment.path)).toEqual([
      '/identity',
      '/identity/users',
    ]);
  });
});
