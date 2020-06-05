import { createDirectiveFactory, SpectatorDirective } from '@ngneat/spectator/jest';
import { FormSubmitDirective } from '../directives/form-submit.directive';
import { FormsModule, ReactiveFormsModule, FormGroup } from '@angular/forms';
import { timer } from 'rxjs';

describe('FormSubmitDirective', () => {
  let spectator: SpectatorDirective<FormSubmitDirective>;
  let directive: FormSubmitDirective;

  const formGroup = new FormGroup({});
  const submitEventFn = jest.fn(() => {});

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

  test('should dispatch submit event on keyup event triggered after given debounce time', done => {
    spectator.dispatchKeyboardEvent('form', 'keyup', 'Enter');
    timer(directive.debounce + 10).subscribe(() => {
      expect(submitEventFn).toHaveBeenCalled();
      done();
    });
  });
});
