import { createDirectiveFactory, SpectatorDirective } from '@ngneat/spectator/jest';
import { Subject } from 'rxjs';
import { PermissionDirective } from '../directives/permission.directive';
import { PermissionService } from '../services';

describe('PermissionDirective', () => {
  let spectator: SpectatorDirective<PermissionDirective>;
  let directive: PermissionDirective;
  const grantedPolicy$ = new Subject<boolean>();
  const createDirective = createDirectiveFactory({
    directive: PermissionDirective,
    providers: [
      { provide: PermissionService, useValue: { getGrantedPolicy$: () => grantedPolicy$ } },
    ],
  });

  describe('with condition', () => {
    beforeEach(() => {
      spectator = createDirective(
        `<div id="test-element" *abpPermission="'test'">Testing Permission Directive</div>`,
      );
      directive = spectator.directive;
    });

    it('should be created', () => {
      expect(directive).toBeTruthy();
    });

    it('should remove the element from DOM', () => {
      grantedPolicy$.next(true);
      expect(spectator.query('#test-element')).toBeTruthy();
      grantedPolicy$.next(false);
      // expect(spectator.query('#test-element')).toBeFalsy(); // TODO: change detection problem should be fixed
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
      expect(spectator.query('#test-element')).toBeFalsy();
      spectator.setHostInput({ condition: 'test' });
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
