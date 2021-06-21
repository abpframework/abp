import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { BehaviorSubject } from 'rxjs';
import { ReplaceableRouteContainerComponent } from '../components/replaceable-route-container.component';
import { ReplaceableComponentsService } from '../services/replaceable-components.service';

@Component({
  selector: 'abp-external-component',
  template: '<p>external</p>',
})
export class ExternalComponent {}

@Component({
  selector: 'abp-default-component',
  template: '<p>default</p>',
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
    declarations: [ExternalComponent, DefaultComponent],
    entryComponents: [DefaultComponent, ExternalComponent],
    mocks: [Router],
  });

  beforeEach(() => {
    spectator = createHost('<abp-replaceable-route-container></abp-replaceable-route-container>', {
      detectChanges: true,
    });
  });

  it('should display the default component', () => {
    expect(spectator.query('p')).toHaveText('default');
  });

  it("should display the external component if it's available in store.", () => {
    get$Res.next({ component: ExternalComponent });
    spectator.detectChanges();
    expect(spectator.query('p')).toHaveText('external');

    get$Res.next({ component: null });
    spectator.detectChanges();
    expect(spectator.query('p')).toHaveText('default');
  });
});
