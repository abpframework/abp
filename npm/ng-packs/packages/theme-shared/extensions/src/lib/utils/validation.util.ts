import { AbpValidators } from '@abp/ng.core';
import { ValidatorFn } from '@angular/forms';
import { ObjectExtensions } from '../models/object-extensions';

export function getValidatorsFromProperty(
  property: ObjectExtensions.ExtensionPropertyDto,
): ValidatorFn[] {
  const validators: ValidatorFn[] = [];

  property.attributes.forEach(attr => {
    if (attr.typeSimple && attr.typeSimple in AbpValidators) {
      validators.push((AbpValidators as any)[attr.typeSimple](attr.config));
    }
  });

  return validators;
}
