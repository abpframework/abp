import { PermissionDirective } from '../directives/permission.directive';
import { SpectatorDirective, createDirectiveFactory, SpyObject } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import { of, Subject } from 'rxjs';

describe('PermissionDirective', () => {
  let spectator: SpectatorDirective<PermissionDirective>;
  let directive: PermissionDirective;
  const grantedPolicy$ = new Subject();

  const createDirective = createDirectiveFactory({
    directive: PermissionDirective,
    providers: [{ provide: Store, useValue: { select: () => grantedPolicy$ } }],
  });

  describe('with condition', () => {
    beforeEach(() => {
      spectator = createDirective(`<div id="test-element" [abpPermission]="'test'">Testing Permission Directive</div>`);
      directive = spectator.directive;
    });

    it('should be created', () => {
      expect(directive).toBeTruthy();
    });

    it('should remove the element from DOM', () => {
      grantedPolicy$.next(true);
      expect(spectator.query('#test-element')).toBeTruthy();
      grantedPolicy$.next(false);
      expect(spectator.query('#test-element')).toBeFalsy();
    });
  });

  describe('without condition', () => {
    beforeEach(() => {
      spectator = createDirective('<div id="test-element" abpPermission>Testing Permission Directive</div>');
      directive = spectator.directive;
    });

    it('should do nothing when condition is undefined', () => {
      const spy = jest.spyOn(spectator.get(Store), 'select');
      grantedPolicy$.next(false);
      expect(spy.mock.calls).toHaveLength(0);
    });
  });
});
