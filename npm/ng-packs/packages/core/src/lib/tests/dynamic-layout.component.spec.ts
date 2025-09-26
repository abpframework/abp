import { HttpClient } from '@angular/common/http';
import { Component, NgModule, inject as inject_1 } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { createRoutingFactory, SpectatorRouting } from '@ngneat/spectator/jest';
import { DynamicLayoutComponent, RouterOutletComponent } from '../components';
import { eLayoutType } from '../enums/common';
import { ABP } from '../models';
import { AbpApplicationConfigurationService } from '../proxy/volo/abp/asp-net-core/mvc/application-configurations/abp-application-configuration.service';
import { ReplaceableComponentsService, RoutesService } from '../services';

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

@Component({
  selector: 'abp-dummy',
  template: '{{route.snapshot.data?.name}} works!',
  imports: [],
})
class DummyComponent {
  route = inject_1(ActivatedRoute);
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
    imports: [DummyComponent, RouterModule, DummyApplicationLayoutComponent, DummyAccountLayoutComponent, DummyEmptyLayoutComponent, DynamicLayoutComponent],
    mocks: [AbpApplicationConfigurationService, HttpClient],
    providers: [
      {
        provide: RoutesService,
        useValue: {
          add: jest.fn(),
          flat$: { pipe: jest.fn() },
          tree$: { pipe: jest.fn() },
          visible$: { pipe: jest.fn() },
        },
      },
      ReplaceableComponentsService,
    ],
    routes: [
      { path: '', component: RouterOutletComponent },
      {
        path: 'parentWithLayout',
        component: DynamicLayoutComponent,
        children: [
          {
            path: 'childWithoutLayout',
            component: DummyComponent,
          },
          {
            path: 'childWithLayout',
            component: DummyComponent,
          },
        ],
      },
      {
        path: 'withData',
        component: DummyComponent,
        data: { name: 'Test Data' },
      },
    ],
  });

  let spectator: SpectatorRouting<RouterOutletComponent>;

  beforeEach(() => {
    spectator = createComponent();
  });

  it('should create component', () => {
    expect(spectator.component).toBeTruthy();
  });
});
