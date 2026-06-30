import { createDirectiveFactory, SpectatorDirective } from '@ngneat/spectator/vitest';
import { AutofocusDirective } from '../directives/autofocus.directive';
import { afterEach, beforeEach, describe, expect, test, vi } from 'vitest';

describe('AutofocusDirective', () => {
  let spectator: SpectatorDirective<AutofocusDirective>;
  let directive: AutofocusDirective;
  let input: HTMLInputElement;
  const createDirective = createDirectiveFactory({
    directive: AutofocusDirective,
  });

  beforeEach(() => {
    vi.useFakeTimers();

    spectator = createDirective('<input autofocus />');
    directive = spectator.directive;
    input = spectator.query('input');
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

  test('should have 0ms delay', () => {
    expect(directive.delay()).toBe(0);
  });

  test('should focus element after default delay', () => {
    expect(input).not.toBeFocused();

    vi.runOnlyPendingTimers();
    expect(input).toBeFocused();
  });
});
