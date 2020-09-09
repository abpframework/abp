import { ABP } from '../models/common';
import { isNumber } from './number-utils';

export function mapEnumToOptions<T>(_enum: T): ABP.Option<T>[] {
  const options: ABP.Option<T>[] = [];

  for (const member in _enum)
    if (!isNumber(member))
      options.push({
        key: member,
        value: _enum[member],
      });

  return options;
}
