import { createDirectiveFactory, SpectatorDirective } from '@ngneat/spectator/jest';
import { AutofocusDirective } from '../directives/autofocus.directive';
import { timer } from 'rxjs';

describe('AutofocusDirective', () => {
  let spectator: SpectatorDirective<AutofocusDirective>;
  let directive: AutofocusDirective;
  let input: HTMLInputElement;
  const createDirective = createDirectiveFactory({
    directive: AutofocusDirective,
  });

  beforeEach(() => {
    spectator = createDirective('<input [autofocus]="10" />', {
      hostProps: {},
    });
    directive = spectator.directive;
    input = spectator.query('input');
  });

  test('should be created', () => {
    expect(directive).toBeTruthy();
  });

  test('should have 10ms delay', () => {
    expect(directive.delay).toBe(10);
  });

  test('should focus element after given delay', done => {
    timer(0).subscribe(() => expect('input').not.toBeFocused());
    timer(11).subscribe(() => {
      expect('input').toBeFocused();
      done();
    });
  });
});
