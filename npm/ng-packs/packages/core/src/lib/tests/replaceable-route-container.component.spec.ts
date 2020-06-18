import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngxs/store';
import { of, Subject, BehaviorSubject } from 'rxjs';
import { ReplaceableRouteContainerComponent } from '../components/replaceable-route-container.component';
import { ReplaceableComponentsState } from '../states';

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
  const selectResponse = new BehaviorSubject(undefined);
  const mockSelect = jest.fn(() => selectResponse);

  let spectator: SpectatorHost<ReplaceableRouteContainerComponent>;
  const createHost = createHostFactory({
    component: ReplaceableRouteContainerComponent,
    providers: [
      { provide: ActivatedRoute, useValue: activatedRouteMock },
      { provide: Store, useValue: { select: mockSelect } },
    ],
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
    selectResponse.next({ component: ExternalComponent });
    spectator.detectChanges();
    expect(spectator.query('p')).toHaveText('external');

    selectResponse.next({ component: null });
    spectator.detectChanges();
    expect(spectator.query('p')).toHaveText('default');
  });
});
