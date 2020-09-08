import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngxs/store';
import { of, Subject, BehaviorSubject } from 'rxjs';
import { ReplaceableRouteContainerComponent } from '../components/replaceable-route-container.component';
import { ReplaceableComponentsState } from '../states';
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
  const replaceableComponents = spectator.inject(ReplaceableComponentsService);
  const spy = jest.spyOn(replaceableComponents, 'get$');
  spy.mockReturnValue(get$Res as any);

  const createHost = createHostFactory({
    component: ReplaceableRouteContainerComponent,
    providers: [{ provide: ActivatedRoute, useValue: activatedRouteMock }],
    declarations: [ExternalComponent, DefaultComponent],
    entryComponents: [DefaultComponent, ExternalComponent],
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
