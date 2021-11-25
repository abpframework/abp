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
import { FormControl, FormGroup, FormGroupDirective } from '@angular/forms';
import { fromEvent } from 'rxjs';
import { debounceTime, filter } from 'rxjs/operators';
import { SubscriptionService } from '../services/subscription.service';

type Controls = { [key: string]: FormControl } | FormGroup[];

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: 'form[ngSubmit][formGroup]',
  providers: [SubscriptionService],
})
export class FormSubmitDirective implements OnInit {
  @Input()
  debounce = 200;

  @Input()
  notValidateOnSubmit: string | boolean;

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
      this.markAsDirty();
      this.executedNgSubmit = true;
    });

    const keyup$ = fromEvent(this.host.nativeElement as HTMLElement, 'keyup').pipe(
      debounceTime(this.debounce),
      filter(event => !(event.target instanceof HTMLTextAreaElement)),
      filter((event: KeyboardEvent) => event && event.key === 'Enter'),
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

    setDirty(form.controls as { [key: string]: FormControl });
    form.markAsDirty();

    this.cdRef.detectChanges();
  }
}

function setDirty(controls: Controls) {
  if (Array.isArray(controls)) {
    controls.forEach(group => {
      setDirty(group.controls as { [key: string]: FormControl });
    });
    return;
  }

  Object.keys(controls).forEach(key => {
    controls[key].markAsDirty();
    controls[key].updateValueAndValidity();
  });
}
