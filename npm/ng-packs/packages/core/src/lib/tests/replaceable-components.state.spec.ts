import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { NgxsModule, Store } from '@ngxs/store';
import { ReplaceableComponentsState } from '../states/replaceable-components.state';
import { Component } from '@angular/core';
import { AddReplaceableComponent } from '../actions';

@Component({ selector: 'abp-dummy', template: 'dummy works' })
class DummyComponent {}

describe('ReplaceableComponentsState', () => {
  let spectator: SpectatorHost<DummyComponent>;
  const createHost = createHostFactory({
    component: DummyComponent,
    imports: [NgxsModule.forRoot([ReplaceableComponentsState])],
  });

  beforeEach(() => {
    spectator = createHost('<abp-dummy></abp-dummy>');
  });

  it('should add a component to the state', () => {
    const store = spectator.get(Store);
    expect(store.selectSnapshot(ReplaceableComponentsState.getAll)).toEqual([]);
    store.dispatch(new AddReplaceableComponent({ component: DummyComponent, key: 'Dummy' }));
    expect(store.selectSnapshot(ReplaceableComponentsState.getComponent('Dummy'))).toEqual({
      component: DummyComponent,
      key: 'Dummy',
    });
  });

  it('should replace a exist component', () => {
    const store = spectator.get(Store);
    store.dispatch(new AddReplaceableComponent({ component: DummyComponent, key: 'Dummy' }));
    store.dispatch(new AddReplaceableComponent({ component: null, key: 'Dummy' }));
    expect(store.selectSnapshot(ReplaceableComponentsState.getComponent('Dummy'))).toEqual({
      component: null,
      key: 'Dummy',
    });
    expect(store.selectSnapshot(ReplaceableComponentsState.getAll)).toHaveLength(1);
  });
});
