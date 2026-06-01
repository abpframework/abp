import { createDirectiveFactory, SpectatorDirective } from '@ngneat/spectator/vitest';
import { FormSubmitDirective } from '../directives/form-submit.directive';
import { FormsModule, ReactiveFormsModule, FormGroup } from '@angular/forms';
import { timer, firstValueFrom } from 'rxjs';

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

  test('should be created', () => {
    expect(directive).toBeTruthy();
  });

  test('should have 20ms debounce time', () => {
    expect(directive.debounce).toBe(20);
  });

  test('should dispatch submit event on keyup event triggered after given debounce time', async () => {
    const form = spectator.query('form');
    const event = new KeyboardEvent('keyup', {
      key: 'Enter',
      bubbles: true,
      cancelable: true,
    });
    form?.dispatchEvent(event);
    timer(0).subscribe(() => expect(submitEventFn).not.toHaveBeenCalled());
    await firstValueFrom(timer(directive.debounce + 10));
    expect(submitEventFn).toHaveBeenCalled();
  });
});
