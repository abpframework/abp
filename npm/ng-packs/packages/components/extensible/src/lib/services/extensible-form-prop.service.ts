import { inject, Injectable } from '@angular/core';
 import { ValidatorFn, Validators } from '@angular/forms';
import { AbpValidators, ConfigStateService } from '@abp/ng.core';
import { map } from 'rxjs/operators';
import { FormProp } from '../models/form-props';
import { ePropType } from '../enums/props.enum';

@Injectable()
export class ExtensibleFormPropService {
  readonly  #configStateService = inject(ConfigStateService);

  meridian$ = this.#configStateService
    .getDeep$('localization.currentCulture.dateTimeFormat.shortTimePattern')
    .pipe(map((shortTimePattern: string | undefined) => (shortTimePattern || '').includes('tt')));

  isRequired(validator: ValidatorFn) {
    return (
      validator === Validators.required ||
      validator === AbpValidators.required ||
      validator.name === 'required'
    );
  }

  getComponent(prop: FormProp) {
    if (prop.template) {
      return 'template';
    }
    switch (prop.type) {
      case ePropType.Boolean:
        return 'checkbox';
      case ePropType.Date:
        return 'date';
      case ePropType.DateTime:
        return 'dateTime';
      case ePropType.Hidden:
        return 'hidden';
      case ePropType.MultiSelect:
        return 'multiselect';
      case ePropType.Text:
        return 'textarea';
      case ePropType.Time:
        return 'time';
      case ePropType.Typeahead:
        return 'typeahead';
      case ePropType.PasswordInputGroup:
        return 'passwordinputgroup';
      default:
        return prop.options ? 'select' : 'input';
    }
  }

  getType(prop: FormProp) {
    switch (prop.type) {
      case ePropType.Date:
      case ePropType.String:
        return 'text';
      case ePropType.Boolean:
        return 'checkbox';
      case ePropType.Number:
        return 'number';
      case ePropType.Email:
        return 'email';
      case ePropType.Password:
        return 'password';
      case ePropType.PasswordInputGroup:
        return 'passwordinputgroup';
      default:
        return 'hidden';
    }
  }

  calcAsterisks(validators: ValidatorFn[]) {
    if (!validators) return '';
    const required = validators.find(v => this.isRequired(v));
    return required ? '*' : '';
  }
}
