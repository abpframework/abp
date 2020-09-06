import { Interface, Property } from '../models';

export const VOLO_REGEX = /^Volo\.Abp\.(Application\.Dtos|ObjectExtending)/;

export const VOLO_NAME_VALUE = new Interface({
  base: null,
  identifier: 'NameValue<T = string>',
  ref: 'Volo.Abp.NameValue',
  namespace: 'Volo.Abp',
  properties: [
    new Property({
      name: 'name',
      type: 'string',
      refs: ['System.String'],
    }),
    new Property({
      name: 'value',
      type: 'T',
      refs: ['T'],
    }),
  ],
});
