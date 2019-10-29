import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { createRoutingFactory, SpectatorRouting, SpyObject } from '@ngneat/spectator/jest';
import { NgxsModule, Store } from '@ngxs/store';
import { OAuthService } from 'angular-oauth2-oidc';
import { NgxsResetPluginModule, StateOverwrite } from 'ngxs-reset-plugin';
import { LAYOUTS, ThemeBasicModule } from '../../../../theme-basic/src/public-api';
import { eLayoutType } from '../enums';
import { ABP } from '../models';
import { RouterOutletComponent, CoreModule, DynamicLayoutComponent, ConfigState } from '@abp/ng.core';
import { ThemeSharedModule } from '../../../../theme-shared/src/public-api';
import { MessageService } from 'primeng/components/common/messageservice';

@Component({
  selector: 'abp-dummy',
  template: '{{route.snapshot.data?.name}} works!',
})
class DummyComponent {
  constructor(public route: ActivatedRoute) {}
}

describe('DynamicLayoutComponent', () => {
  const createComponent = createRoutingFactory({
    component: RouterOutletComponent,
    declareComponent: false,
    imports: [
      CoreModule,
      NgxsModule.forRoot([ConfigState]),
      NgxsResetPluginModule.forRoot(),
      ThemeSharedModule,
      ThemeBasicModule,
    ],
    declarations: [DummyComponent],
    entryComponents: [],
    stubsEnabled: false,
    providers: [MessageService, { provide: OAuthService, useValue: { getAccessToken: () => true } }],
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
  let store: SpyObject<Store>;

  beforeEach(async () => {
    spectator = createComponent();
    store = spectator.get(Store);
    store.dispatch(
      new StateOverwrite([
        ConfigState,
        {
          requirements: { layouts: LAYOUTS },
          routes: [
            {
              path: '',
              wrapper: true,
              children: [
                {
                  path: 'parentWithLayout',
                  layout: eLayoutType.application,
                  children: [{ path: 'childWithoutLayout' }, { path: 'childWithLayout', layout: eLayoutType.account }],
                },
              ],
            },
            { path: 'withData', layout: eLayoutType.application },
            ,
          ] as ABP.FullRoute[],
          environment: { application: {} },
        },
      ]),
    );
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
    store.dispatch(
      new StateOverwrite([ConfigState, { ...store.selectSnapshot(ConfigState), requirements: { layouts: [] } }]),
    );

    spectator.detectChanges();

    spectator.router.navigateByUrl('/withoutLayout');
    await spectator.fixture.whenStable();
    spectator.detectComponentChanges();

    expect(spectator.query('abp-layout-empty')).toBeFalsy();
    expect(spectator.query('abp-dynamic-layout').children[0].tagName).toEqual('ROUTER-OUTLET');
  });
});
