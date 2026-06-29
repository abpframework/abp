import { createDirectiveFactory, SpectatorDirective } from '@ngneat/spectator/vitest';
import { FormSubmitDirective } from '../directives/form-submit.directive';
import { FormsModule, ReactiveFormsModule, FormGroup } from '@angular/forms';
import { timer, firstValueFrom } from 'rxjs';
import { afterEach, beforeEach, describe, expect, test, vi } from 'vitest';


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
      '<form [formGroup]="formGroup" (ngSubmit)="submitEventFn()" [debounce]="20">form content</form>',
      {
        hostProps: {
          submitEventFn,
          formGroup,
        },
      },
    );
    directive = spectator.directive;
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
    expect(directive.debounce()).toBe(200);
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
