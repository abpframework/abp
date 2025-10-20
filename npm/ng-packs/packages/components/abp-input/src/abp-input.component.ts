import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  DestroyRef,
  forwardRef,
  inject,
  OnInit,
  input,
  Injector
} from '@angular/core';
import {
  AbstractControl,
  ControlValueAccessor,
  FormBuilder,
  FormControl,
  FormControlName,
  FormGroup,
  FormGroupDirective,
  NG_VALUE_ACCESSOR,
  NgControl,
  ReactiveFormsModule,
} from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { LocalizationPipe } from '@abp/ng.core';

const ABP_INPUT_CONTROL_VALUE_ACCESSOR = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => AbpInputComponent),
  multi: true,
};

@Component({
  selector: 'abp-input',
  templateUrl: './abp-input.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [ReactiveFormsModule, LocalizationPipe],
  exportAs: 'abpInput',
  host: {
    class: 'abp-input',
  },
  providers: [ABP_INPUT_CONTROL_VALUE_ACCESSOR],
})
export class AbpInputComponent implements OnInit, ControlValueAccessor {
  label = input.required<string>();
  type = input<'text' | 'number' | 'password'>('text');
  id = input<string>('');
  placeholder = input<string>('');
  hint = input<string>('');
  control: FormControl;
  readonly formBuilder = inject(FormBuilder);
  readonly changeDetectorRef = inject(ChangeDetectorRef);
  readonly destroyRef = inject(DestroyRef);
  readonly injector = inject(Injector);
  abpInputFormGroup: FormGroup;

  ngOnInit() {

    const ngControl = this.injector.get(NgControl, null);
    if (ngControl) {
      this.control = this.injector.get(FormGroupDirective).getControl(ngControl as FormControlName);
    }

    this.abpInputFormGroup = this.formBuilder.group({
      value: [''],
    });

    this.value.valueChanges.pipe(takeUntilDestroyed(this.destroyRef)).subscribe(val => {
      this.onChange(val);
    });
  }

  writeValue(value: any[]): void {
    console.log(value);
    this.value.setValue(value);
    this.changeDetectorRef.markForCheck();
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState(isDisabled: boolean): void {
    if (isDisabled) {
      this.value.disable();
    } else {
      this.value.enable();
    }
  }

  get errors(): string[] {
    if (this.control && this.control.errors) {
      return []
    }
    return []
  }

  get value(): AbstractControl<any> {
    return this.abpInputFormGroup.get('value');
  }

  private onChange: (value: any) => void = () => {};
  private onTouched: () => void = () => {};
}
