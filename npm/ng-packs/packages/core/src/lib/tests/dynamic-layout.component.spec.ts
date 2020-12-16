import { HttpClient } from '@angular/common/http';
import { Component, NgModule } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { createRoutingFactory, SpectatorRouting } from '@ngneat/spectator/jest';
import { DynamicLayoutComponent, RouterOutletComponent } from '../components';
import { eLayoutType } from '../enums/common';
import { ABP } from '../models';
import { AbpApplicationConfigurationService } from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/abp-application-configuration.service';
import { ReplaceableComponentsService, RoutesService } from '../services';
import { mockRoutesService } from './routes.service.spec';

@Component({
  selector: 'abp-layout-application',
  template: '<router-outlet></router-outlet>',
})
class DummyApplicationLayoutComponent {}

@Component({
  selector: 'abp-layout-account',
  template: '<router-outlet></router-outlet>',
})
class DummyAccountLayoutComponent {}

@Component({
  selector: 'abp-layout-empty',
  template: '<router-outlet></router-outlet>',
})
class DummyEmptyLayoutComponent {}

const LAYOUTS = [
  DummyApplicationLayoutComponent,
  DummyAccountLayoutComponent,
  DummyEmptyLayoutComponent,
];

@NgModule({
  imports: [RouterModule],
  declarations: [...LAYOUTS],
  entryComponents: [...LAYOUTS],
})
class DummyLayoutModule {}

@Component({
  selector: 'abp-dummy',
  template: '{{route.snapshot.data?.name}} works!',
})
class DummyComponent {
  constructor(public route: ActivatedRoute) {}
}

const routes: ABP.Route[] = [
  {
    path: '',
    name: 'Root',
  },
  {
    path: '/parentWithLayout',
    name: 'ParentWithLayout',
    parentName: 'Root',
    layout: eLayoutType.application,
  },
  {
    path: '/parentWithLayout/childWithoutLayout',
    name: 'ChildWithoutLayout',
    parentName: 'ParentWithLayout',
  },
  {
    path: '/parentWithLayout/childWithLayout',
    name: 'ChildWithLayout',
    parentName: 'ParentWithLayout',
    layout: eLayoutType.account,
  },
  {
    path: '/withData',
    name: 'WithData',
    layout: eLayoutType.application,
  },
];

describe('DynamicLayoutComponent', () => {
  const createComponent = createRoutingFactory({
    component: RouterOutletComponent,
    stubsEnabled: false,
    declarations: [DummyComponent, DynamicLayoutComponent],
    mocks: [AbpApplicationConfigurationService, HttpClient],
    providers: [
      {
        provide: RoutesService,
        useFactory: () => mockRoutesService(),
      },
      ReplaceableComponentsService,
    ],
    imports: [RouterModule, DummyLayoutModule],
    routes: [
      { path: '', component: RouterOutletComponent },
      {
        path: 'parentWithLayout',
        component: DynamicLayoutComponent,
        children: [
          {
            path: 'childWithoutLayout',
            component: DummyComponent,
            data: { name: 'childWithoutLayout' },
          },
          {
            path: 'childWithLayout',
            component: DummyComponent,
            data: { name: 'childWithLayout' },
          },
        ],
      },
      {
        path: 'withData',
        component: DynamicLayoutComponent,
        children: [
          {
            path: '',
            component: DummyComponent,
            data: { name: 'withData' },
          },
        ],
        data: { layout: eLayoutType.empty },
      },
      {
        path: 'withoutLayout',
        component: DynamicLayoutComponent,
        children: [
          {
            path: '',
            component: DummyComponent,
            data: { name: 'withoutLayout' },
          },
        ],
        data: { layout: null },
      },
    ],
  });

  let spectator: SpectatorRouting<RouterOutletComponent>;
  let replaceableComponents: ReplaceableComponentsService;

  beforeEach(async () => {
    spectator = createComponent();
    replaceableComponents = spectator.inject(ReplaceableComponentsService);
    const routesService = spectator.inject(RoutesService);
    routesService.add(routes);

    replaceableComponents.add({
      key: 'Theme.ApplicationLayoutComponent',
      component: DummyApplicationLayoutComponent,
    });
    replaceableComponents.add({
      key: 'Theme.AccountLayoutComponent',
      component: DummyAccountLayoutComponent,
    });
    replaceableComponents.add({
      key: 'Theme.EmptyLayoutComponent',
      component: DummyEmptyLayoutComponent,
    });
  });

  it('should handle application layout from parent abp route and display it', async () => {
    spectator.router.navigateByUrl('/parentWithLayout/childWithoutLayout');
    await spectator.fixture.whenStable();
    spectator.detectComponentChanges();
    expect(spectator.query('abp-dynamic-layout')).toBeTruthy();
    expect(spectator.query('abp-layout-application')).toBeTruthy();
  });

  it('should handle account layout from own property and display it', async () => {
    spectator.router.navigateByUrl('/parentWithLayout/childWithLayout');
    await spectator.fixture.whenStable();
    spectator.detectComponentChanges();
    expect(spectator.query('abp-layout-account')).toBeTruthy();
  });

  it('should handle empty layout from route data and display it', async () => {
    spectator.router.navigateByUrl('/withData');
    await spectator.fixture.whenStable();
    spectator.detectComponentChanges();
    expect(spectator.query('abp-layout-empty')).toBeTruthy();
  });

  it('should display empty layout when layout is null', async () => {
    spectator.router.navigateByUrl('/withoutLayout');
    await spectator.fixture.whenStable();
    spectator.detectComponentChanges();
    expect(spectator.query('abp-layout-empty')).toBeTruthy();
  });

  it('should not display any layout when layouts are empty', async () => {
    const spy = jest.spyOn(replaceableComponents, 'get');
    spy.mockReturnValue(null);
    spectator.detectChanges();

    spectator.router.navigateByUrl('/withoutLayout');
    await spectator.fixture.whenStable();
    spectator.detectComponentChanges();

    expect(spectator.query('abp-layout-empty')).toBeFalsy();
    expect(spectator.query('abp-dynamic-layout').children[0].tagName).toEqual('ROUTER-OUTLET');
  });
});
