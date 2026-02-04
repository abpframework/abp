import { Type } from '@angular/core';
import { ControlValueAccessor } from '@angular/forms';

export interface FormFieldConfig<T = any> {
  key: string;
  value?: any;
  type: 'text' | 'email' | 'number' | 'select' | 'checkbox' | 'date' | 'textarea' | 'datetime-local' | 'time' | 'password' | 'tel' | 'url' | 'radio' | 'file' | 'range' | 'color' | 'group' | 'array';
  label: string;
  placeholder?: string;
  required?: boolean;
  disabled?: boolean;
  options?: OptionProps<T>;
  validators?: ValidatorConfig[];
  conditionalLogic?: ConditionalRule[];
  order?: number;
  gridSize?: number;
  component?: Type<ControlValueAccessor>;
  // Additional field attributes
  min?: number | string; // For number, date, time, range
  max?: number | string; // For number, date, time, range
  step?: number | string; // For number, time, range
  minLength?: number; // For text, password
  maxLength?: number; // For text, password
  pattern?: string; // For tel, text
  accept?: string; // For file input (e.g., "image/*")
  multiple?: boolean; // For file input
  // Nested form support (for group and array types)
  children?: FormFieldConfig[]; // Child fields for nested forms
  minItems?: number; // For array type: minimum number of items
  maxItems?: number; // For array type: maximum number of items
}

export interface ValidatorConfig {
  type: 'required' | 'email' | 'minLength' | 'maxLength' | 'pattern' | 'custom' | 'min' | 'max' | 'requiredTrue';
  value?: any;
  message: string;
}

export interface ConditionalRule {
  dependsOn: string;
  condition: 'equals' | 'notEquals' | 'contains' | 'greaterThan' | 'lessThan';
  value: any;
  action: 'show' | 'hide' | 'enable' | 'disable';
}

export enum ConditionalAction {
  SHOW = 'show',
  HIDE = 'hide',
  ENABLE = 'enable',
  DISABLE = 'disable'
}

export interface OptionProps<T = any> {
  defaultValues?: T[];
  url?: string;
  disabled?: (option: T) => boolean;
  labelProp?: string;
  valueProp?: string;
  apiName?: string;
}
