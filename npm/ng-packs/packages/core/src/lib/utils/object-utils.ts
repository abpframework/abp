import {
  exists,
  isArray,
  isNode,
  isNullOrUndefined,
  isObject,
  isObjectAndNotArrayNotNode,
} from './common-utils';
import { DeepPartial } from '../models';

export function deepMerge<T>(
  target: DeepPartial<T> | T,
  source: DeepPartial<T> | T,
): DeepPartial<T> | T {
  if (isObjectAndNotArrayNotNode(target) && isObjectAndNotArrayNotNode(source)) {
    return deepMergeRecursively(target, source);
  } else if (isNullOrUndefined(target) && isNullOrUndefined(source)) {
    return {} as T;
  } else {
    return exists(source) ? (source as T) : target;
  }
}

function deepMergeRecursively<T>(
  target: DeepPartial<T> | T,
  source: DeepPartial<T> | T,
): DeepPartial<T> | T {
  const shouldNotRecurse =
    isNullOrUndefined(target) ||
    isNullOrUndefined(source) || // at least one not defined
    isArray(target) ||
    isArray(source) || // at least one array
    !isObject(target) ||
    !isObject(source) || // at least one not an object
    isNode(target) ||
    isNode(source); // at least one node

  /**
   * if we will not recurse any further,
   * we will prioritize source if it is a defined value.
   */
  if (shouldNotRecurse) {
    return exists(source) ? (source as T) : target;
  }

  const keysOfTarget = Object.keys(target as { [key: string]: any });
  const keysOfSource = Object.keys(source as { [key: string]: any });
  const uniqueKeys = new Set(keysOfTarget.concat(keysOfSource));
  return [...uniqueKeys].reduce((retVal, key) => {
    (retVal as any)[key] = deepMergeRecursively(
      (target as { [key: string]: any })[key],
      (source as { [key: string]: any })[key],
    );
    return retVal;
  }, {} as T);
}
