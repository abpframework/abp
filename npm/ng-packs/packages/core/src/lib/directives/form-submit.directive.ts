import {
  ChangeDetectorRef,
  Directive,
  ElementRef,
  EventEmitter,
  OnInit,
  Output,
  inject,
  input
} from '@angular/core';
import { FormGroupDirective, UntypedFormControl, UntypedFormGroup } from '@angular/forms';
import { fromEvent } from 'rxjs';
import { debounceTime, filter } from 'rxjs/operators';
import { SubscriptionService } from '../services/subscription.service';

type Controls = { [key: string]: UntypedFormControl } | UntypedFormGroup[];
/**
 * @deprecated FormSubmitDirective will be removed in V7.0.0. Use `ngSubmit` instead.
 */
@Directive({
  selector: 'form[ngSubmit][formGroup]',
  providers: [SubscriptionService],
})
export class FormSubmitDirective implements OnInit {
  private formGroupDirective = inject(FormGroupDirective, { self: true });
  private host = inject<ElementRef<HTMLFormElement>>(ElementRef);
  private cdRef = inject(ChangeDetectorRef);
  private subscription = inject(SubscriptionService);

  readonly debounce = input(200);

  // TODO: Remove unused input
  readonly notValidateOnSubmit = input<string | boolean>(undefined);

  readonly markAsDirtyWhenSubmit = input(true);

  @Output() readonly ngSubmit = new EventEmitter();

  executedNgSubmit = false;

  ngOnInit() {
    this.subscription.addOne(this.formGroupDirective.ngSubmit, () => {
      if (this.markAsDirtyWhenSubmit()) {
        this.markAsDirty();
      }

      this.executedNgSubmit = true;
    });

    const keyup$ = fromEvent<KeyboardEvent>(this.host.nativeElement as HTMLElement, 'keyup').pipe(
      debounceTime(this.debounce()),
      filter(event => !(event.target instanceof HTMLTextAreaElement)),
      filter(event => event && event.key === 'Enter'),
    );

    this.subscription.addOne(keyup$, () => {
      if (!this.executedNgSubmit) {
        this.host.nativeElement.dispatchEvent(
          new Event('submit', { bubbles: true, cancelable: true }),
        );
      }

      this.executedNgSubmit = false;
    });
  }

  markAsDirty() {
    const { form } = this.formGroupDirective;

    setDirty(form.controls as { [key: string]: UntypedFormControl });
    form.markAsDirty();

    this.cdRef.detectChanges();
  }
}

function setDirty(controls: Controls) {
  if (Array.isArray(controls)) {
    controls.forEach(group => {
      setDirty(group.controls as { [key: string]: UntypedFormControl });
    });
    return;
  }

  Object.keys(controls).forEach(key => {
    controls[key].markAsDirty();
    controls[key].updateValueAndValidity();
  });
}
