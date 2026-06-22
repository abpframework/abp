import {
  ChangeDetectionStrategy,
  Component,
  input,
  inject,
  ChangeDetectorRef,
  forwardRef,
} from '@angular/core';
import { FormGroup, FormArray, FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { FormFieldConfig } from '../dynamic-form.models';
import { DynamicFormService } from '../dynamic-form.service';
import { LocalizationPipe } from '@abp/ng.core';
import { DynamicFormFieldComponent } from '../dynamic-form-field';
import { DynamicFormGroupComponent } from '../dynamic-form-group';

@Component({
  selector: 'abp-dynamic-form-array',
  templateUrl: './dynamic-form-array.component.html',
  styleUrls: ['./dynamic-form-array.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    LocalizationPipe,
    DynamicFormFieldComponent,
    DynamicFormGroupComponent,
    forwardRef(() => DynamicFormArrayComponent), // Self reference for recursion
  ],
})
export class DynamicFormArrayComponent {
  arrayConfig = input.required<FormFieldConfig>();
  formGroup = input.required<FormGroup>();
  visible = input<boolean>(true);

  private fb = inject(FormBuilder);
  private dynamicFormService = inject(DynamicFormService);
  private cdr = inject(ChangeDetectorRef);

  get formArray(): FormArray {
    return this.formGroup().get(this.arrayConfig().key) as FormArray;
  }

  get sortedChildren(): FormFieldConfig[] {
    const children = this.arrayConfig().children || [];
    return children.sort((a, b) => (a.order || 0) - (b.order || 0));
  }

  get canAddItem(): boolean {
    const maxItems = this.arrayConfig().maxItems;
    return maxItems ? this.formArray.length < maxItems : true;
  }

  get canRemoveItem(): boolean {
    const minItems = this.arrayConfig().minItems || 0;
    return this.formArray.length > minItems;
  }

  addItem() {
    if (!this.canAddItem) return;
    
    const itemGroup = this.dynamicFormService.createFormGroup(
      this.arrayConfig().children || []
    );
    
    this.formArray.push(itemGroup);
    this.cdr.markForCheck();
  }

  removeItem(index: number) {
    if (!this.canRemoveItem) return;
    
    this.formArray.removeAt(index);
    this.cdr.markForCheck();
  }

  getItemFormGroup(index: number): FormGroup {
    return this.formArray.at(index) as FormGroup;
  }

  getNestedFormGroup(index: number, key: string): FormGroup {
    return this.getItemFormGroup(index).get(key) as FormGroup;
  }

  trackByIndex(index: number): number {
    return index;
  }
}
