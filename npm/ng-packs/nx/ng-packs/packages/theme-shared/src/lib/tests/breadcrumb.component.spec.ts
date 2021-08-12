import {
  ABP,
  CORE_OPTIONS,
  LocalizationPipe,
  RouterOutletComponent,
  RoutesService,
} from '@abp/ng.core';
import { HttpClient } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { createRoutingFactory, SpectatorRouting, SpyObject } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import { mockRoutesService } from '../../../../core/src/lib/tests/routes.service.spec';
import { BreadcrumbComponent } from '../components/breadcrumb/breadcrumb.component';

const mockRoutes: ABP.Route[] = [
  { name: 'Identity', path: '/identity' },
  { name: 'Users', path: '/identity/users', parentName: 'Identity' },
];

describe('BreadcrumbComponent', () => {
  let spectator: SpectatorRouting<RouterOutletComponent>;
  let routes: RoutesService;
  let store: SpyObject<Store>;

  const createRouting = createRoutingFactory({
    component: RouterOutletComponent,
    stubsEnabled: false,
    detectChanges: false,
    mocks: [Store, HttpClient],
    providers: [
      { provide: CORE_OPTIONS, useValue: {} },
      {
        provide: RoutesService,
        useFactory: () => mockRoutesService(),
      },
    ],
    declarations: [LocalizationPipe, BreadcrumbComponent],
    imports: [RouterModule],
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
    store = spectator.inject(Store);
  });

  it('should display the breadcrumb', async () => {
    routes.add(mockRoutes);
    await spectator.router.navigateByUrl('/identity/users');
    // for abpLocalization
    store.selectSnapshot.mockReturnValueOnce('Identity');
    store.selectSnapshot.mockReturnValueOnce('Users');
    spectator.detectChanges();

    const elements = spectator.queryAll('li');
    expect(elements).toHaveLength(3);
    expect(elements[1]).toHaveText('Identity');
    expect(elements[2]).toHaveText('Users');
  });

  it('should not display the breadcrumb when empty', async () => {
    routes.add([]);
    await spectator.router.navigateByUrl('/identity/users');

    spectator.detectChanges();
    expect(spectator.query('ol.breadcrumb')).toBeFalsy();
  });
});
