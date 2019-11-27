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

  describe('structural', () => {
    beforeEach(() => {
      spectator = createDirective(
        '<div id="test-element" *abpPermission="condition">Testing Permission Directive</div>',
        { hostProps: { condition: '' } },
      );
      directive = spectator.directive;
    });

    it('should be created', () => {
      expect(directive).toBeTruthy();
    });

    it('should remove the element from DOM', () => {
      expect(spectator.query('#test-element')).toBeTruthy();
      expect(spectator.directive.subscription).toBeUndefined();
      spectator.setHostInput({ condition: 'test' });
      expect(spectator.directive.subscription).toBeTruthy();
      grantedPolicy$.next(true);
      expect(spectator.query('#test-element')).toBeTruthy();
      grantedPolicy$.next(false);
      expect(spectator.query('#test-element')).toBeFalsy();
      grantedPolicy$.next(true);
      grantedPolicy$.next(true);
      expect(spectator.queryAll('#test-element')).toHaveLength(1);
    });

    describe('#subscription', () => {
      it('should call the unsubscribe', () => {
        const spy = jest.fn(() => {});
        spectator.setHostInput({ condition: 'test' });
        spectator.directive.subscription.unsubscribe = spy;
        spectator.setHostInput({ condition: 'test2' });

        expect(spy).toHaveBeenCalled();
      });
    });
  });
});
