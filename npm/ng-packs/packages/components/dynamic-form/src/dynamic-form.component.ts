import {
  ChangeDetectionStrategy,
  Component,
  input,
  output,
  inject,
  OnInit,
  DestroyRef,
  ChangeDetectorRef,
} from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DynamicFormService } from './dynamic-form.service';
import { ConditionalAction, FormFieldConfig } from './dynamic-form.models';
import { DynamicFormFieldComponent, DynamicFieldHostComponent } from './dynamic-form-field';
import { DynamicFormGroupComponent } from './dynamic-form-group';
import { DynamicFormArrayComponent } from './dynamic-form-array';

@Component({
  selector: 'abp-dynamic-form',
  templateUrl: './dynamic-form.component.html',
  styleUrls: ['./dynamic-form.component.scss'],
  host: { class: 'abp-dynamic-form' },
  changeDetection: ChangeDetectionStrategy.OnPush,
  exportAs: 'abpDynamicForm',
  imports: [
    CommonModule,
    DynamicFormFieldComponent,
    DynamicFormGroupComponent,
    DynamicFormArrayComponent,
    ReactiveFormsModule,
    DynamicFieldHostComponent,
  ],
})
export class DynamicFormComponent implements OnInit {
  fields = input<FormFieldConfig[]>([]);
  values = input<Record<string, any>>();
  submitButtonText = input<string>('Submit');
  submitInProgress = input<boolean>(false);
  showCancelButton = input<boolean>(false);
  onSubmit = output<any>();
  formCancel = output<void>();
  private dynamicFormService = inject(DynamicFormService);
  readonly destroyRef = inject(DestroyRef);
  readonly changeDetectorRef = inject(ChangeDetectorRef);

  dynamicForm!: FormGroup;
  fieldVisibility: { [key: string]: boolean } = {};

  ngOnInit() {
    this.setupFormAndLogic();
  }

  get sortedFields(): FormFieldConfig[] {
    return this.fields().sort((a, b) => (a.order || 0) - (b.order || 0));
  }

  submit() {
    if (this.dynamicForm.valid) {
      this.onSubmit.emit(this.dynamicForm.getRawValue());
    } else {
      this.markAllFieldsAsTouched();
      this.focusFirstInvalidField();
    }
  }

  onCancel() {
    this.formCancel.emit();
  }

  onFieldChange(event: { fieldKey: string; value: any }) {
    this.evaluateConditionalLogic(event.fieldKey);
  }

  isFieldVisible(field: FormFieldConfig): boolean {
    return this.fieldVisibility[field.key] !== false;
  }

  getChildFormGroup(key: string): FormGroup {
    return this.dynamicForm.get(key) as FormGroup;
  }

  resetForm() {
    const initialValues: { [key: string]: any } = this.dynamicFormService.getInitialValues(
      this.fields(),
    );
    this.dynamicForm.reset({ ...initialValues });
    this.dynamicForm.markAsUntouched();
    this.dynamicForm.markAsPristine();
    this.changeDetectorRef.markForCheck();
  }

  private initializeFieldVisibility() {
    this.fields().forEach(field => {
      this.fieldVisibility = {
        ...this.fieldVisibility,
        [field.key]: !field.conditionalLogic?.length,
      };
    });
  }

  private setupConditionalLogic() {
    this.fields().forEach(field => {
      if (field.conditionalLogic) {
        field.conditionalLogic.forEach(rule => {
          const dependentControl = this.dynamicForm.get(rule.dependsOn);
          if (dependentControl) {
            this.evaluateConditionalLogic(field.key);
            dependentControl.valueChanges
              .pipe(takeUntilDestroyed(this.destroyRef))
              .subscribe(() => {
                this.evaluateConditionalLogic(field.key);
              });
          }
        });
      }
    });
  }

  private evaluateConditionalLogic(fieldKey: string) {
    const field = this.fields().find(f => f.key === fieldKey);
    if (!field?.conditionalLogic) return;

    field.conditionalLogic.forEach(rule => {
      const dependentValue = this.dynamicForm.get(rule.dependsOn)?.value;
      const conditionMet = this.evaluateCondition(dependentValue, rule.condition, rule.value);

      this.applyConditionalAction(fieldKey, rule.action, conditionMet);
    });
  }

  private evaluateCondition(fieldValue: any, condition: string, ruleValue: any): boolean {
    switch (condition) {
      case 'equals':
        return fieldValue === ruleValue;
      case 'notEquals':
        return fieldValue !== ruleValue;
      case 'contains':
        return fieldValue && fieldValue.includes && fieldValue.includes(ruleValue);
      case 'greaterThan':
        return Number(fieldValue) > Number(ruleValue);
      case 'lessThan':
        return Number(fieldValue) < Number(ruleValue);
      default:
        return false;
    }
  }

  private applyConditionalAction(fieldKey: string, action: string, shouldApply: boolean) {
    const control = this.dynamicForm.get(fieldKey);

    switch (action) {
      case ConditionalAction.SHOW:
        this.fieldVisibility = { ...this.fieldVisibility, [fieldKey]: shouldApply };
        break;
      case ConditionalAction.HIDE:
        this.fieldVisibility = { ...this.fieldVisibility, [fieldKey]: !shouldApply };
        break;
      case ConditionalAction.ENABLE:
        if (control) {
          shouldApply ? control.enable() : control.disable();
        }
        break;
      case ConditionalAction.DISABLE:
        if (control) {
          shouldApply ? control.disable() : control.enable();
        }
        break;
    }
  }

  private setupFormAndLogic() {
    this.dynamicForm = this.dynamicFormService.createFormGroup(this.fields());
    this.initializeFieldVisibility();
    this.setupConditionalLogic();
    this.changeDetectorRef.markForCheck();
  }

  private markAllFieldsAsTouched() {
    Object.keys(this.dynamicForm.controls).forEach(key => {
      this.dynamicForm.get(key)?.markAsTouched();
    });
  }

  private focusFirstInvalidField() {
    // Accessibility: Focus first invalid field for screen readers
    const firstInvalidField = this.sortedFields.find(field => {
      const control = this.dynamicForm.get(field.key);
      return control && control.invalid && control.touched;
    });

    if (firstInvalidField) {
      setTimeout(() => {
        const element = document.getElementById(`field-${firstInvalidField.key}`);
        if (element) {
          element.focus();
          element.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
      }, 100);
    }
  }
}
