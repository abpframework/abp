import { createDirectiveFactory, SpectatorDirective } from '@ngneat/spectator/jest';
import { InputEventDebounceDirective } from '../directives/debounce.directive';
import { timer } from 'rxjs';

describe('InputEventDebounceDirective', () => {
  let spectator: SpectatorDirective<InputEventDebounceDirective>;
  let directive: InputEventDebounceDirective;
  let input: HTMLInputElement;
  const inputEventFn = jest.fn(() => {});

  const createDirective = createDirectiveFactory({
    directive: InputEventDebounceDirective,
  });

  beforeEach(() => {
    spectator = createDirective('<input (input.debounce)="inputEventFn()" [debounce]="20"  />', {
      hostProps: { inputEventFn },
    });
    directive = spectator.directive;
    input = spectator.query('input');
    inputEventFn.mockClear();
  });

  test('should be created', () => {
    expect(directive).toBeTruthy();
  });

  test('should have 20ms debounce time', () => {
    expect(directive.debounce).toBe(20);
  });

  test('should call fromEvent with target element and target event', done => {
    spectator.dispatchFakeEvent('input', 'input', true);
    timer(0).subscribe(() => expect(inputEventFn).not.toHaveBeenCalled());
    timer(21).subscribe(() => {
      expect(inputEventFn).toHaveBeenCalled();
      done();
    });
  });
});
