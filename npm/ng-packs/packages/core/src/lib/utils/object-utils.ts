import {
  exists,
  isArray,
  isNode,
  isNullOrUndefined,
  isObject,
  isObjectAndNotArrayNotNode,
} from './common-utils';

export function deepMerge(target, source) {
  if (isObjectAndNotArrayNotNode(target) && isObjectAndNotArrayNotNode(source)) {
    return deepMergeRecursively(target, source);
  } else if (isNullOrUndefined(target) && isNullOrUndefined(source)) {
    return {};
  } else {
    return exists(source) ? source : target;
  }
}

function deepMergeRecursively(target, source) {
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
    return exists(source) ? source : target;
  }

  const keysOfTarget = Object.keys(target);
  const keysOfSource = Object.keys(source);
  const uniqueKeys = new Set(keysOfTarget.concat(keysOfSource));
  return [...uniqueKeys].reduce((retVal, key) => {
    retVal[key] = deepMergeRecursively(target[key], source[key]);
    return retVal;
  }, {});
}
