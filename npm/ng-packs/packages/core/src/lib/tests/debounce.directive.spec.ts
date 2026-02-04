import { timer , firstValueFrom } from 'rxjs';
import { createDirectiveFactory, SpectatorDirective } from '@ngneat/spectator/vitest';
import { InputEventDebounceDirective } from '../directives/debounce.directive';

describe('InputEventDebounceDirective', () => {
  let spectator: SpectatorDirective<InputEventDebounceDirective>;
  let directive: InputEventDebounceDirective;
  let input: HTMLInputElement;
  const inputEventFn = vi.fn(() => {});

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

  test('should call fromEvent with target element and target event', async () => {
    spectator.dispatchFakeEvent('input', 'input', true);
    timer(0).subscribe(() => expect(inputEventFn).not.toHaveBeenCalled());
    await firstValueFrom(timer(21));
    expect(inputEventFn).toHaveBeenCalled();
  });
});
