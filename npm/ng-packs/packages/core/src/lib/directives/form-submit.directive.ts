import {
  ChangeDetectorRef,
  Directive,
  ElementRef,
  EventEmitter,
  Input,
  OnInit,
  Output,
  Self,
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
  standalone: true,
  selector: 'form[ngSubmit][formGroup]',
  providers: [SubscriptionService],
})
export class FormSubmitDirective implements OnInit {
  @Input()
  debounce = 200;

  // TODO: Remove unused input
  @Input()
  notValidateOnSubmit?: string | boolean;

  @Input()
  markAsDirtyWhenSubmit = true;

  @Output() readonly ngSubmit = new EventEmitter();

  executedNgSubmit = false;

  constructor(
    @Self() private formGroupDirective: FormGroupDirective,
    private host: ElementRef<HTMLFormElement>,
    private cdRef: ChangeDetectorRef,
    private subscription: SubscriptionService,
  ) {}

  ngOnInit() {
    this.subscription.addOne(this.formGroupDirective.ngSubmit, () => {
      if (this.markAsDirtyWhenSubmit) {
        this.markAsDirty();
      }

      this.executedNgSubmit = true;
    });

    const keyup$ = fromEvent<KeyboardEvent>(this.host.nativeElement as HTMLElement, 'keyup').pipe(
      debounceTime(this.debounce),
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
