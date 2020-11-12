import { APP_BASE_HREF } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { SpyObject } from '@ngneat/spectator';
import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { NgxsModule, Store } from '@ngxs/store';
import { AddReplaceableComponent } from '../actions';
import { ReplaceableComponentsState } from '../states/replaceable-components.state';

@Component({ selector: 'abp-dummy', template: 'dummy works' })
class DummyComponent {}

describe('ReplaceableComponentsState', () => {
  let spectator: SpectatorHost<DummyComponent>;
  let router: SpyObject<Router>;
  const createHost = createHostFactory({
    component: DummyComponent,
    providers: [{ provide: APP_BASE_HREF, useValue: '/' }],
    imports: [RouterModule.forRoot([], { relativeLinkResolution: 'legacy' }), NgxsModule.forRoot([ReplaceableComponentsState])],
  });

  beforeEach(() => {
    spectator = createHost('<abp-dummy></abp-dummy>');
    router = spectator.inject(Router);
  });

  it('should add a component to the state', () => {
    const store = spectator.inject(Store);
    expect(store.selectSnapshot(ReplaceableComponentsState.getAll)).toEqual([]);
    store.dispatch(new AddReplaceableComponent({ component: DummyComponent, key: 'Dummy' }));
    expect(store.selectSnapshot(ReplaceableComponentsState.getComponent('Dummy'))).toEqual({
      component: DummyComponent,
      key: 'Dummy',
    });
  });

  it('should replace a exist component', () => {
    const store = spectator.inject(Store);
    store.dispatch(new AddReplaceableComponent({ component: DummyComponent, key: 'Dummy' }));
    store.dispatch(new AddReplaceableComponent({ component: null, key: 'Dummy' }));
    expect(store.selectSnapshot(ReplaceableComponentsState.getComponent('Dummy'))).toEqual({
      component: null,
      key: 'Dummy',
    });
    expect(store.selectSnapshot(ReplaceableComponentsState.getAll)).toHaveLength(1);
  });

  it('should call reloadRoute when reload parameter is given as true to AddReplaceableComponent', async () => {
    const spy = jest.spyOn(router, 'navigateByUrl');
    const store = spectator.inject(Store);
    store.dispatch(new AddReplaceableComponent({ component: DummyComponent, key: 'Dummy' }));
    store.dispatch(new AddReplaceableComponent({ component: null, key: 'Dummy' }, true));

    await spectator.fixture.whenStable();

    expect(spy).toHaveBeenCalledTimes(1);
    expect(spy).toHaveBeenCalledWith(router.url);
  });
});
