import {
  Directive,
  ElementRef,
  OnDestroy,
  OnInit,
  Self,
  ChangeDetectorRef,
  HostBinding,
  Input,
  Output,
  EventEmitter,
} from '@angular/core';
import { FormGroupDirective, FormGroup, FormControl, ɵNgNoValidate } from '@angular/forms';
import { fromEvent } from 'rxjs';
import { takeUntilDestroy } from '../utils';
import { debounceTime, filter } from 'rxjs/operators';

type Controls = { [key: string]: FormControl } | FormGroup[];

@Directive({
  selector: 'form[ngSubmit][formGroup]',
})
export class FormSubmitDirective implements OnInit, OnDestroy {
  @Input()
  noValidateOnSubmit;

  @Input()
  allowSubmit: boolean = true;

  constructor(
    @Self() private formGroupDirective: FormGroupDirective,
    private host: ElementRef<HTMLFormElement>,
    private cdRef: ChangeDetectorRef,
  ) {}

  ngOnInit() {
    fromEvent(this.host.nativeElement as HTMLElement, 'keyup')
      .pipe(
        debounceTime(200),
        filter((key: KeyboardEvent) => key && key.key === 'Enter' && this.allowSubmit),
        takeUntilDestroy(this),
      )
      .subscribe(() => {
        this.host.nativeElement.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
      });

    if (this.noValidateOnSubmit || typeof this.noValidateOnSubmit === 'string') {
      return;
    }

    fromEvent(this.host.nativeElement, 'submit')
      .pipe(takeUntilDestroy(this))
      .subscribe(() => {
        const { form } = this.formGroupDirective;

        setDirty(form.controls as { [key: string]: FormControl });
        form.markAsDirty();

        this.cdRef.detectChanges();
      });
  }

  ngOnDestroy(): void {}
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
