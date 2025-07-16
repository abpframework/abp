import { Component, ChangeDetectionStrategy, forwardRef, input } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ABP, LocalizationPipe } from '@abp/ng.core';
import { FormProp } from '../../models/form-props';
import { NgxValidateCoreModule } from '@ngx-validate/core';

const EXTENSIBLE_FORM_MULTI_SELECT_CONTROL_VALUE_ACCESSOR = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => ExtensibleFormMultiselectComponent),
  multi: true,
};

@Component({
  selector: 'abp-extensible-form-multi-select',
  template: `
    <div [id]="prop().id">
      @for (option of options(); track option.value) {
        <div class="form-check" validationTarget>
          <input
            type="checkbox"
            class="form-check-input"
            [id]="'checkbox_' + option.value"
            [disabled]="disabled"
            [checked]="isChecked(option.value)"
            (change)="onCheckboxChange(option.value, $event.target.checked)"
          />
          <label [for]="'checkbox_' + option.value">
            @if (prop().isExtra) {
              {{ '::' + option.key | abpLocalization }}
            } @else {
              {{ option.key }}
            }
          </label>
        </div>
      }
    </div>
  `,
  providers: [EXTENSIBLE_FORM_MULTI_SELECT_CONTROL_VALUE_ACCESSOR],
  imports: [LocalizationPipe, CommonModule, ReactiveFormsModule, NgxValidateCoreModule],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExtensibleFormMultiselectComponent implements ControlValueAccessor {
  prop = input.required<FormProp>();
  options = input.required<ABP.Option<any>[]>();

  selectedValues: any[] = [];
  disabled = false;

  private onChange: (value: any) => void = () => {};
  private onTouched: () => void = () => {};

  setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  isChecked(value: any): boolean {
    return this.selectedValues.includes(value);
  }

  onCheckboxChange(value: any, checked: boolean): void {
    this.selectedValues = checked
      ? [...this.selectedValues, value]
      : this.selectedValues.filter(item => item !== value);

    this.onChange(this.selectedValues);
    this.onTouched();
  }

  writeValue(value: any[]): void {
    this.selectedValues = Array.isArray(value) ? [...value] : [];
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
}
