import {
  ChangeDetectionStrategy,
  Component,
  input,
  forwardRef,
} from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormFieldConfig } from '../dynamic-form.models';
import { LocalizationPipe } from '@abp/ng.core';
import { DynamicFormFieldComponent } from '../dynamic-form-field';
import { DynamicFormArrayComponent } from '../dynamic-form-array';

@Component({
  selector: 'abp-dynamic-form-group',
  templateUrl: './dynamic-form-group.component.html',
  styleUrls: ['./dynamic-form-group.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    LocalizationPipe,
    DynamicFormFieldComponent,
    forwardRef(() => DynamicFormArrayComponent),
    forwardRef(() => DynamicFormGroupComponent), // Self reference for recursion
  ],
})
export class DynamicFormGroupComponent {
  groupConfig = input.required<FormFieldConfig>();
  formGroup = input.required<FormGroup>();
  visible = input<boolean>(true);

  get sortedChildren(): FormFieldConfig[] {
    const children = this.groupConfig().children || [];
    return children.sort((a, b) => (a.order || 0) - (b.order || 0));
  }

  getChildFormGroup(key: string): FormGroup {
    return this.formGroup().get(key) as FormGroup;
  }

  getChildControl(key: string) {
    return this.formGroup().get(key);
  }
}
