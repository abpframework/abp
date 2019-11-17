import {
  ChangeDetectorRef,
  Directive,
  ElementRef,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  Self,
} from '@angular/core';
import { FormControl, FormGroup, FormGroupDirective } from '@angular/forms';
import { fromEvent } from 'rxjs';
import { debounceTime, filter } from 'rxjs/operators';
import { takeUntilDestroy } from '../utils';

type Controls = { [key: string]: FormControl } | FormGroup[];

@Directive({
  // tslint:disable-next-line: directive-selector
  selector: 'form[ngSubmit][formGroup]',
})
export class FormSubmitDirective implements OnInit, OnDestroy {
  @Input()
  notValidateOnSubmit: string | boolean;

  @Output() readonly ngSubmit = new EventEmitter();

  executedNgSubmit = false;

  constructor(
    @Self() private formGroupDirective: FormGroupDirective,
    private host: ElementRef<HTMLFormElement>,
    private cdRef: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    this.formGroupDirective.ngSubmit.pipe(takeUntilDestroy(this)).subscribe(() => {
      this.markAsDirty();
      this.executedNgSubmit = true;
    });

    fromEvent(this.host.nativeElement as HTMLElement, 'keyup')
      .pipe(
        debounceTime(200),
        filter((key: KeyboardEvent) => key && key.key === 'Enter'),
        takeUntilDestroy(this),
      )
      .subscribe(() => {
        if (!this.executedNgSubmit) {
          this.host.nativeElement.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
        }

        this.executedNgSubmit = false;
      });
  }

  ngOnDestroy(): void {}

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
