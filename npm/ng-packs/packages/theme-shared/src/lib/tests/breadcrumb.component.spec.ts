import {
  ABP,
  CORE_OPTIONS,
  LocalizationPipe,
  RouterOutletComponent,
  RoutesService,
  LocalizationService,
} from '@abp/ng.core';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { RouterModule } from '@angular/router';
import { createRoutingFactory, SpectatorRouting } from '@ngneat/spectator/jest';
import { BreadcrumbComponent, BreadcrumbItemsComponent } from '../components';
import { OTHERS_GROUP } from '@abp/ng.core';
import { SORT_COMPARE_FUNC } from '@abp/ng.core';

const mockRoutes: ABP.Route[] = [
  { name: '_::Identity', path: '/identity' },
  { name: '_::Users', path: '/identity/users', parentName: '_::Identity' },
];

// Simple compare function that doesn't use inject()
const simpleCompareFunc = (a: any, b: any) => {
  const aNumber = a.order || 0;
  const bNumber = b.order || 0;
  return aNumber - bNumber;
};

describe('BreadcrumbComponent', () => {
  let spectator: SpectatorRouting<RouterOutletComponent>;
  let routes: RoutesService;

  const createRouting = createRoutingFactory({
    component: RouterOutletComponent,
    stubsEnabled: false,
    detectChanges: false,
    providers: [
      provideHttpClient(),
      provideHttpClientTesting(),
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
        } 
      },
      RoutesService,
      LocalizationService,
      {
        provide: OTHERS_GROUP,
        useValue: 'AbpUi::OthersGroup',
      },
      {
        provide: SORT_COMPARE_FUNC,
        useValue: simpleCompareFunc,
      },
    ],
    declarations: [],
    imports: [
      RouterModule,
      LocalizationPipe,
      BreadcrumbComponent,
      BreadcrumbItemsComponent,
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
