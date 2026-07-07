import { createDirectiveFactory, SpectatorDirective } from '@ngneat/spectator/vitest';
import { InputEventDebounceDirective } from '../directives/debounce.directive';
import { afterEach, beforeEach, describe, expect, test, vi } from 'vitest';

describe('InputEventDebounceDirective', () => {
  let spectator: SpectatorDirective<InputEventDebounceDirective>;
  let directive: InputEventDebounceDirective;
  let input: HTMLInputElement;
  const inputEventFn = vi.fn(() => {});

  const createDirective = createDirectiveFactory({
    directive: InputEventDebounceDirective,
  });

  beforeEach(() => {
    vi.useFakeTimers();

    spectator = createDirective('<input input.debounce (input.debounce)="inputEventFn()" />', {
      hostProps: { inputEventFn },
    });
    directive = spectator.directive;
    input = spectator.query('input');
    inputEventFn.mockClear();
  });

  afterEach(() => {
    if (vi.isFakeTimers()) {
      vi.runOnlyPendingTimers();
    }
    vi.useRealTimers();
  });

  test('should be created', () => {
    expect(directive).toBeTruthy();
  });

  test('should have 300ms debounce time', () => {
    expect(directive.debounce()).toBe(300);
  });

  test('should call fromEvent with target element and target event', () => {
    const emitSpy = vi.spyOn(directive.debounceEvent, 'emit');

    spectator.dispatchFakeEvent('input', 'input', true);
    expect(emitSpy).not.toHaveBeenCalled();

    vi.advanceTimersByTime(299);
    expect(emitSpy).not.toHaveBeenCalled();

    vi.advanceTimersByTime(1);
    expect(emitSpy).toHaveBeenCalled();
  });
});
