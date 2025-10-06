import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { BehaviorSubject } from 'rxjs';
import { ReplaceableRouteContainerComponent } from '../components/replaceable-route-container.component';
import { ReplaceableComponentsService } from '../services/replaceable-components.service';

@Component({
  selector: 'abp-external-component',
  template: '<p>external</p>'
})
export class ExternalComponent {}

@Component({
  selector: 'abp-default-component',
  template: '<p>default</p>'
})
export class DefaultComponent {}

const activatedRouteMock = {
  snapshot: {
    data: {
      replaceableComponent: {
        defaultComponent: DefaultComponent,
        key: 'TestModule.TestComponent',
      },
    },
  },
};

describe('ReplaceableRouteContainerComponent', () => {
  let spectator: SpectatorHost<ReplaceableRouteContainerComponent>;
  const get$Res = new BehaviorSubject(undefined);

  const createHost = createHostFactory({
    component: ReplaceableRouteContainerComponent,
    providers: [
      { provide: ActivatedRoute, useValue: activatedRouteMock },
      { provide: ReplaceableComponentsService, useValue: { get$: () => get$Res } },
    ],
    imports: [ExternalComponent, DefaultComponent],
    mocks: [Router],
  });

  beforeEach(() => {
    spectator = createHost('<abp-replaceable-route-container></abp-replaceable-route-container>', {
      detectChanges: true,
    });
  });

  it('should create component successfully', () => {
    expect(spectator.component).toBeTruthy();
  });
});
