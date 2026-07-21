import { createDirectiveFactory, SpectatorDirective } from '@ngneat/spectator/vitest';
import { FormSubmitDirective } from '../directives/form-submit.directive';
import { FormsModule, ReactiveFormsModule, FormGroup } from '@angular/forms';
import { afterEach, beforeEach, describe, expect, test, vi } from 'vitest';
import { setInputSignal } from './utils';


describe('FormSubmitDirective', () => {
  let spectator: SpectatorDirective<FormSubmitDirective>;
  let directive: FormSubmitDirective;

  const formGroup = new FormGroup({});
  const submitEventFn = vi.fn(() => {});

  const createDirective = createDirectiveFactory({
    directive: FormSubmitDirective,
    imports: [FormsModule, ReactiveFormsModule],
  });

  beforeEach(() => {
    vi.useFakeTimers();

    spectator = createDirective(
      '<form ngSubmit [formGroup]="formGroup" (ngSubmit)="submitEventFn()">form content</form>',
      {
        hostProps: {
          submitEventFn,
          formGroup,
        },
      },
    );
    directive = spectator.directive;
    setInputSignal(directive.debounce, 20);
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

  test('should have 20ms debounce time', () => {
    expect(directive.debounce()).toBe(20);
  });

  test('should dispatch submit event on keyup event triggered after given debounce time', async () => {
    const form = spectator.query('form');
    const event = new KeyboardEvent('keyup', {
      key: 'Enter',
      bubbles: true,
      cancelable: true,
    });
    form?.dispatchEvent(event);
    expect(submitEventFn).not.toHaveBeenCalled();

    vi.advanceTimersByTime(199);
    expect(submitEventFn).not.toHaveBeenCalled();

    vi.advanceTimersByTime(1);
    expect(submitEventFn).toHaveBeenCalled();
  });
});
