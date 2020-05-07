import { Component, NgModule } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { createRoutingFactory, SpectatorRouting } from '@ngneat/spectator/jest';
import { NgxsModule, Store } from '@ngxs/store';
import { DynamicLayoutComponent, RouterOutletComponent } from '../components';
import { eLayoutType } from '../enums';
import { ABP } from '../models';
import { ConfigState, ReplaceableComponentsState } from '../states';
import { ApplicationConfigurationService } from '../services';
import { HttpClient } from '@angular/common/http';

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

const storeData = {
  ConfigState: {
    routes: [
      {
        path: '',
        wrapper: true,
        children: [
          {
            path: 'parentWithLayout',
            layout: eLayoutType.application,
            children: [
              { path: 'childWithoutLayout' },
              { path: 'childWithLayout', layout: eLayoutType.account },
            ],
          },
        ],
      },
      { path: 'withData', layout: eLayoutType.application },
      ,
    ] as ABP.FullRoute[],
    environment: { application: {} },
  },
  ReplaceableComponentsState: {
    replaceableComponents: [
      {
        key: 'Theme.ApplicationLayoutComponent',
        component: DummyApplicationLayoutComponent,
      },
      {
        key: 'Theme.AccountLayoutComponent',
        component: DummyAccountLayoutComponent,
      },
      {
        key: 'Theme.EmptyLayoutComponent',
        component: DummyEmptyLayoutComponent,
      },
    ],
  },
};

describe('DynamicLayoutComponent', () => {
  const createComponent = createRoutingFactory({
    component: RouterOutletComponent,
    stubsEnabled: false,
    declarations: [DummyComponent, DynamicLayoutComponent],
    mocks: [ApplicationConfigurationService, HttpClient],
    imports: [
      RouterModule,
      DummyLayoutModule,
      NgxsModule.forRoot([ConfigState, ReplaceableComponentsState]),
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
  let store: Store;

  beforeEach(async () => {
    spectator = createComponent();
    store = spectator.get(Store);

    store.reset(storeData);
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
    store.reset({ ...storeData, ReplaceableComponentsState: {} });

    spectator.detectChanges();

    spectator.router.navigateByUrl('/withoutLayout');
    await spectator.fixture.whenStable();
    spectator.detectComponentChanges();

    expect(spectator.query('abp-layout-empty')).toBeFalsy();
    expect(spectator.query('abp-dynamic-layout').children[0].tagName).toEqual('ROUTER-OUTLET');
  });
});
