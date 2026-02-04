import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  DestroyRef,
  forwardRef,
  inject,
  InjectionToken, Injector,
  input,
  OnInit,
} from '@angular/core';
import { FormFieldConfig } from '../dynamic-form.models';
import {
  ControlValueAccessor,
  FormBuilder,
  FormControl,
  FormControlName,
  FormGroupDirective,
  NG_VALUE_ACCESSOR,
  NgControl,
  FormGroup,
  ReactiveFormsModule,
} from '@angular/forms';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { NgTemplateOutlet, AsyncPipe } from '@angular/common';
import { LocalizationPipe } from '@abp/ng.core';
import { FormCheckboxComponent } from '@abp/ng.theme.shared';
import { Observable, of } from 'rxjs';
import { DynamicFormService } from '../dynamic-form.service';

export const ABP_DYNAMIC_FORM_FIELD = new InjectionToken<DynamicFormFieldComponent>('AbpDynamicFormField');

const DYNAMIC_FORM_FIELD_CONTROL_VALUE_ACCESSOR = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => DynamicFormFieldComponent),
  multi: true,
};

@Component({
  selector: 'abp-dynamic-form-field',
  templateUrl: './dynamic-form-field.component.html',
  styleUrls: ['./dynamic-form-field.component.scss'],
  providers: [
    { provide: ABP_DYNAMIC_FORM_FIELD, useExisting: DynamicFormFieldComponent },
    DYNAMIC_FORM_FIELD_CONTROL_VALUE_ACCESSOR,
  ],
  host: { class: 'abp-dynamic-form-field' },
  exportAs: 'abpDynamicFormField',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [NgTemplateOutlet, LocalizationPipe, ReactiveFormsModule, FormCheckboxComponent, AsyncPipe],
})
export class DynamicFormFieldComponent implements OnInit, ControlValueAccessor {
  field = input.required<FormFieldConfig>();
  visible = input<boolean>(true);
  control!: FormControl;
  fieldFormGroup: FormGroup;
  readonly changeDetectorRef = inject(ChangeDetectorRef);
  readonly destroyRef = inject(DestroyRef);
  private injector = inject(Injector);
  private formBuilder = inject(FormBuilder);
  private dynamicFormService = inject(DynamicFormService);

  options$: Observable<{ key: string; value: any }[]> = of([]);

  // Accessibility: Generate unique IDs for ARIA
  get fieldId(): string {
    return `field-${this.field().key}`;
  }

  get errorId(): string {
    return `${this.fieldId}-error`;
  }

  get helpTextId(): string {
    return `${this.fieldId}-help`;
  }

  constructor() {
    this.fieldFormGroup = this.formBuilder.group({
      value: [{ value: '' }],
    });
  }

  ngOnInit() {
    const ngControl = this.injector.get(NgControl, null);
    if (ngControl) {
      this.control = this.injector.get(FormGroupDirective).getControl(ngControl as FormControlName);
    }
    this.value.valueChanges.pipe(takeUntilDestroyed(this.destroyRef)).subscribe(value => {
      this.onChange(value);
    });

    const options = this.field().options;

    if (options?.url) {
      this.options$ = this.dynamicFormService.getOptions(options.url, options.apiName);
    } else if (options?.defaultValues?.length) {
      this.options$ = of(
        options.defaultValues.map(item => {
          return {
            key: item[options.valueProp || 'key'] || item,
            value: item[options.labelProp || 'value'] || item
          };
        })
      );
    } else {
      this.options$ = of([]);
    }
  }

  writeValue(value: any[]): void {
    this.value.setValue(value || '');
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
    this.changeDetectorRef.markForCheck();
  }

  get isInvalid(): boolean {
    if (this.control) {
      return this.control.invalid && (this.control.dirty || this.control.touched);
    }
    return false;
  }

  get errors(): string[] {
    if (!this.control?.errors) return [];
    if (this.control && this.control.errors) {
      const errorKeys = Object.keys(this.control.errors);
      const validators = this.field().validators || [];
      return errorKeys.map(key => {
        const validator = validators.find(
          v => v.type.toLowerCase() === key.toLowerCase(),
        );
        if (validator && validator.message) {
          return validator.message;
        }
        // Fallback error messages
        if (key === 'required') return `${this.field().label} is required`;
        if (key === 'email') return 'Please enter a valid email address';
        if (key === 'minlength')
          return `Minimum length is ${this.control.errors[key].requiredLength}`;
        if (key === 'maxlength')
          return `Maximum length is ${this.control.errors[key].requiredLength}`;
        return `${this.field().label} is invalid due to ${key} validation.`;
      });
    }
    return [];
  }
  get value() {
    return this.fieldFormGroup.get('value');
  }

  onFileChange(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files) {
      const files = Array.from(input.files);
      const value = this.field().multiple ? files : files[0];
      this.value.setValue(value);
      this.onChange(value);
    }
  }

  private onChange: (value: any) => void = () => { };
  private onTouched: () => void = () => { };
}
