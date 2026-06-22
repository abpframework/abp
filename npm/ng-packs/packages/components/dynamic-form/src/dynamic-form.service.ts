import {Injectable, inject} from '@angular/core';
import {FormControl, FormGroup, FormArray, ValidatorFn, Validators, FormBuilder} from '@angular/forms';
import {FormFieldConfig, ValidatorConfig} from './dynamic-form.models';
import { RestService } from '@abp/ng.core';

@Injectable({
  providedIn: 'root'
})

export class DynamicFormService {

  private formBuilder = inject(FormBuilder);
  private restService = inject(RestService);
  apiName = 'DynamicFormService';

  createFormGroup(fields: FormFieldConfig[]): FormGroup {
    const group: any = {};

    fields.forEach(field => {
      // Nested Group
      if (field.type === 'group') {
        group[field.key] = this.createFormGroup(field.children || []);
      }
      // Nested Array
      else if (field.type === 'array') {
        group[field.key] = this.createFormArray(field);
      }
      // Regular Field
      else {
        const validators = this.buildValidators(field.validators || []);
        const initialValue = this.getInitialValue(field);

        group[field.key] = new FormControl({
          value: initialValue,
          disabled: field.disabled || false
        }, validators);
      }
    });

    return this.formBuilder.group(group);
  }

  createFormArray(arrayConfig: FormFieldConfig): FormArray {
    const items: FormGroup[] = [];
    const minItems = arrayConfig.minItems || 0;

    // Create minimum required items
    for (let i = 0; i < minItems; i++) {
      items.push(this.createFormGroup(arrayConfig.children || []));
    }

    return this.formBuilder.array(items);
  }

  getInitialValues(fields: FormFieldConfig[]): any {
    const initialValues: any = {};
    fields.forEach(field => {
      if (field.type === 'group') {
        initialValues[field.key] = this.getInitialValues(field.children || []);
      } else if (field.type === 'array') {
        initialValues[field.key] = [];
      } else {
        initialValues[field.key] = this.getInitialValue(field);
      }
    });
    return initialValues;
  }

  getOptions(url: string, apiName?: string): any {
    return this.restService.request<any, any[]>({
        method: 'GET',
        url,
      },
      { apiName: apiName || this.apiName });
  }

  private buildValidators(validatorConfigs: ValidatorConfig[]): ValidatorFn[] {
    return validatorConfigs.map(config => {
      switch (config.type) {
        case 'required':
          return Validators.required;
        case 'email':
          return Validators.email;
        case 'minLength':
          return Validators.minLength(config.value);
        case 'maxLength':
          return Validators.maxLength(config.value);
        case 'pattern':
          return Validators.pattern(config.value);
        case 'min':
          return Validators.min(config.value);
        case 'max':
          return Validators.max(config.value);
        case 'requiredTrue':
          return Validators.requiredTrue;
        default:
          return Validators.nullValidator;
      }
    });
  }

  private getInitialValue(field: FormFieldConfig): any {
    if (field.value !== undefined) {
      return field.value;
    }
    switch (field.type) {
      case 'checkbox':
        return false;
      case 'number':
        return 0;
      case 'group':
        return this.getInitialValues(field.children || []);
      case 'array':
        return [];
      default:
        return '';
    }
  }
}
